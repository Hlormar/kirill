using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp
{
    //обьект хранит характеристики и статус для каждого файла в dataGrid
    public class FileStatusItem : INotifyPropertyChanged
    {
        private string _fileName = "";
        private string _status = "";
        private string _extension = "";
        private string _resolution = "";
        private string _AvgBrightness = "";
        private string _time = "";


        public string FileName
        {
            get => _fileName;
            set { _fileName = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public string Extension
        {
            get => _extension;
            set { _extension = value; OnPropertyChanged(); }
        }
        public string Resolution
        {
            get => _resolution;
            set { _resolution = value; OnPropertyChanged(); }
        }
        public string AvgBrightness
        {
            get => _AvgBrightness;
            set { _AvgBrightness = value; OnPropertyChanged(); }
        }
        public string Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
