using Microsoft.Win32;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentPath = Directory.GetCurrentDirectory();
        List<FileInfo> ImageFiles = new();
        private bool isProcessing = false;
        private CancellationTokenSource _cts;

        //Коллекция обьектов с данными для DataGrid для каждого файла
        private ObservableCollection<FileStatusItem> _fileStatuses;
        private ConcurrentBag<long> _errors = new(); // записывать файлы, которые обработались с ошибками
        private ConcurrentBag<long> _success = new(); // записывать файлы, которые обработались с ошибками
        readonly OpenFolderDialog folderDialog = new OpenFolderDialog();

        void UpdatePathTextBox(string newPath)
        {
            PathTextBox.Text = newPath;
        }

        void UpdateStatusMessage(int filesAmount) {
            if (filesAmount <= 0)
            {
                StatusLabel.Content = "Указанный путь не содержит поддерживаемых файлов";
            }
            else {
                StatusLabel.Content = $"Обнаружено {filesAmount} изображений";
            }
            
        }


        // Инициализация dataGrid перед запуском
        private void InitializeDataGrid()
        {
            _fileStatuses = new ObservableCollection<FileStatusItem>();
            _errors = new ConcurrentBag<long>(); // Очищаем перед новым запуском
            _success = new ConcurrentBag<long>();

            foreach (var file in ImageFiles)
            {
                _fileStatuses.Add(new FileStatusItem
                {
                    FileName = file.FullName,
                    Extension = file.Extension,
                   
                    Status = "" // Начальный статус
                });
            }
            ResultDataGrid.ItemsSource = _fileStatuses;
        }

        public async Task<List<FileInfo>> GetImageFiles(string folderPath)
        {
            // Проверка на существование папки
            if (!Directory.Exists(folderPath))
            {
                return await Task.Run(() => new List<FileInfo>());
            }

            // HashSet для быстрого и регистронезависимого поиска расширений 
            var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".jpg", ".png", ".bmp"
            };

            var directory = new DirectoryInfo(folderPath);

            // фильтруем
            List<FileInfo> imageFiles = directory.EnumerateFiles()
                .Where(file => allowedExtensions.Contains(file.Extension))
                .ToList();

            return await Task.Run(() => imageFiles);
        }


        public MainWindow()
        {
            InitializeComponent();
            UpdatePathTextBox(currentPath);

        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            folderDialog.InitialDirectory = currentPath;
            if (folderDialog.ShowDialog() == true) {
                currentPath = folderDialog.FolderName;
                UpdatePathTextBox(currentPath);
            }

        }

        private async void PathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(PathTextBox.Text))
            {
                currentPath = PathTextBox.Text;
                folderDialog.DefaultDirectory = currentPath;

                ImageFiles = await GetImageFiles(currentPath);
                UpdateStatusMessage(ImageFiles.Count);
            }
            else {
                StatusLabel.Content = "Данного пути не существует";
                currentPath = "";
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //cancel button logic
            if (isProcessing) {
                //cancel tasks and print semi-result
                StartButton.IsEnabled = false;
                _cts?.Cancel(); // Отправляем сигнал отмены
                return;
            }


            //check images found
            if (ImageFiles.Count <= 0) {
                //error sound
                System.Media.SystemSounds.Beep.Play();

                //highlight status label border
                var colorAnimation = new ColorAnimation
                {
                    From = Colors.Transparent,
                    To = SystemColors.HighlightColor, 
                    Duration = TimeSpan.FromSeconds(0.125), 
                    AutoReverse = true,               
                    RepeatBehavior = new RepeatBehavior(2)
                };
                StatusLabel.BorderBrush = new SolidColorBrush(Colors.Transparent);
                var brush = (SolidColorBrush)StatusLabel.BorderBrush;
                brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                return;
            }

            //create output folder
            if (!Directory.Exists(currentPath + "\\output")) {
                Directory.CreateDirectory(currentPath + "\\output");
            }
            //change Start button to Cancel
            StartButton.Content = "Отмена";
            isProcessing = true;

            //DataGrid
            ResultDataGrid.Visibility = Visibility.Visible;
            InitializeDataGrid(); // Сброс

            _cts = new CancellationTokenSource(); // Создаем новый источник
            var token = _cts.Token;               // Получаем токен

            //Настройка progress bar
            int ImageCount = ImageFiles.Count;
            StatusBar.Value = 0; 
            StatusBar.Minimum = 0;
            StatusBar.Maximum = ImageCount;
            StatusBar.Visibility = Visibility.Visible;

            var progress = new Progress<int>(percent =>
            {
                StatusBar.Value = percent;
                StatusLabel.Content = $"{percent} / {ImageCount}";
            });


            // Прогресс статуса для DataGrid
            var fileStatusProgress = new Progress<(int Index, string Status)>(data =>
            {
                if (data.Index >= 0 && data.Index < _fileStatuses.Count)
                {
                    _fileStatuses[data.Index].Status = data.Status;
                }
            });
            // Прогресс характеристик для DataGrid
            var fileSpecsProgress = new Progress<(int Index, string Resolution, string AvgBrightness, string Time)>(data =>
            {
                if (data.Index >= 0 && data.Index < _fileStatuses.Count)
                {
                    _fileStatuses[data.Index].Resolution = data.Resolution;
                    _fileStatuses[data.Index].AvgBrightness = data.AvgBrightness;
                    _fileStatuses[data.Index].Time = data.Time;
                }
            });


            try
            {
                await Task.Run(() => {

                    try {
                        ImageProcessor.ProcessAll(ImageFiles, progress, token, fileStatusProgress, fileSpecsProgress, _success, _errors);
                    }
                    catch (OperationCanceledException) {
                        Debug.WriteLine("Task catched cancel token");
                    }  

                }, token);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("CANCEL");
            }
            finally
            {

                StartButton.Content = "Обработка";
                StartButton.IsEnabled = true;
                isProcessing = false;
                StatusBar.Visibility = Visibility.Hidden;
                UpdateStatusMessage(ImageCount);

                _cts?.Dispose();
                _cts = null;

                // результаты из _success и _errors
                StatusLabel.Content = $"Успешно: {_success.Count}, Ошибок: {_errors.Count}";

                // Статус необработнных файлов (если статус пустой)
                foreach (FileStatusItem FileStatus in _fileStatuses) {
                    if (FileStatus.Status.IsWhiteSpace()) {
                        FileStatus.Status = "Не обработан";
                    }
                }

            }
        }
    }
}