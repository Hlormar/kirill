using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WpfApp
{
    public static class ImageProcessor
    {
        //обработка массива файлов
        public static void ProcessAll(IEnumerable<FileInfo> files, IProgress<int> progress, CancellationToken ct,
            IProgress<(int Index, string Status)> fileStatusProgress,
            IProgress<(int Index, string Resolution, string AvgBrightness, string Time)> fileSpecsProgress, 
            ConcurrentBag<long> successBag, ConcurrentBag<long> errorsBag)
        {
            int processed = 0;
            int total = files.Count();
            var options = new ParallelOptions { CancellationToken = ct, MaxDegreeOfParallelism = Environment.ProcessorCount };

            try
            {
                Parallel.ForEach(files, options, (file, _, index) =>
                {
                    ct.ThrowIfCancellationRequested(); // Проверка отмены на каждой итерации

                    try
                    {
                        Debug.WriteLine($"ImageProcessor, started processing {file.Name}");
                        //1
                        ProcessImageRows(file, index, processed, progress, fileSpecsProgress);
                        fileStatusProgress.Report(((int)index, "Обработка..."));
                        //2
                        fileStatusProgress.Report(((int)index, "Обработан"));
                        successBag.Add(index);
                    }
                    catch (OperationCanceledException)
                    {

                        throw;

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ImageProcessor, file:{file.FullName} exception:{ex}");
                        fileStatusProgress.Report(((int)index, $"{ex}"));
                        errorsBag.Add(index);
                    }
                    finally
                    {
                        int current = Interlocked.Increment(ref processed);
                        progress.Report(current);
                    }
                });
            }
            catch (OperationCanceledException) {
                Debug.WriteLine($"ImageProcessor cancel exception");
            }
            
        }

        private static Bitmap NormalizeFormat(Bitmap src)
        {
            if (src.PixelFormat == PixelFormat.Format24bppRgb)
                return src;

            var result = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(result))
            {
                g.DrawImage(src, new System.Drawing.Rectangle(0, 0, src.Width, src.Height));
            }
            return result;
        }


        private static ImageFormat GetImageFormatFromExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".gif":
                    return ImageFormat.Gif;
                case ".tiff":
                case ".tif":
                    return ImageFormat.Tiff;
                default:
                    return ImageFormat.Png;
            }
        }



        //обработка 1 файла
        private static void ProcessImageRows(FileInfo file, long index, int processed, IProgress<int> progress,
            IProgress<(int Index, string Resolution, string AvgBrightness, string Time)> fileSpecsProgress,
            int threshold = 128)
        {
            Stopwatch st = new();
            st.Start();

            var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            var bmp = new Bitmap(fs);
            var normBmp = NormalizeFormat(bmp);
            int height = normBmp.Height;
            int width = normBmp.Width;
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            var bmpData = normBmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
            bmp.PixelFormat);
            try {

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap.
                int bytes = Math.Abs(bmpData.Stride) * height;
                byte[] rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                long totalBrightness = 0; 

                Parallel.For(0, height, (y) => {
                    try
                    {
                        //Thread.Sleep(RandomNumberGenerator.GetInt32(4000) + 1000); // случайное время выполнения
                        int rowOffset = y * bmpData.Stride;
                        long rowSum = 0;


                        for (int x = 0; x < width; x++)
                        {
                            int pixelOffset = rowOffset + x * 3;

                            // B, G, R
                            byte blue = rgbValues[pixelOffset];
                            byte green = rgbValues[pixelOffset + 1];
                            byte red = rgbValues[pixelOffset + 2];

                            //яркость
                            byte gray = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);
                            rowSum += gray;

                            byte newValue = (gray >= threshold) ? (byte)255 : (byte)0;

                            rgbValues[pixelOffset] = newValue;         // Blue
                            rgbValues[pixelOffset + 1] = newValue;     // Green
                            rgbValues[pixelOffset + 2] = newValue;     // Red
                        }

                        //суммируем яркость по всем пикселям
                        Interlocked.Add(ref totalBrightness, rowSum);
                    }
                    catch (Exception ex)
                    {

                    }


                });

                double avgBrightness = (double)totalBrightness / (width * height);

                //запись
                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);

                //сохр
                normBmp.Save($"{file.Directory?.FullName ?? Directory.GetCurrentDirectory()}\\output\\{file.Name}", GetImageFormatFromExtension(file.Extension));
                st.Stop();

                //Update DataGrid result
                fileSpecsProgress.Report(((int)index, $"{width}x{height}", $"{avgBrightness}", $"{st.ElapsedMilliseconds}"));
            }
            finally
            {
                normBmp.UnlockBits(bmpData);
                Debug.WriteLine($"ProcessImageRows, processed {file.Name}");
                
            }


        }
    }
}
