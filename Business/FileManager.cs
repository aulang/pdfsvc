using pdfsvc.Converters;
using System;
using System.IO;
using System.Text;

namespace pdfsvc.Business
{
    public class FileManager
    {
        public const string PDF = "pdf";
        public const string DOC = "doc";
        private const string DATEFORMAT = "yyyyMMdd";
        private static volatile int count = 0;

        public readonly string _baseDir;
        public FileManager(string baseDir)
        {
            _baseDir = baseDir;
        }

        private string GetInputFilePath(string fileName, string type)
        {
            ++count;

            String today = DateTime.Now.ToString(DATEFORMAT);

            StringBuilder builder = new StringBuilder(_baseDir);
            builder.Append(Path.DirectorySeparatorChar).Append(today).Append(Path.DirectorySeparatorChar).Append(type);

            string parentDir = builder.ToString();

            if (!Directory.Exists(parentDir))
            {
                count = 0;
                Directory.CreateDirectory(parentDir);
            }

            builder.Append(Path.DirectorySeparatorChar).Append(count).Append(fileName);
            return builder.ToString();
        }

        private string GetParentDirectory(string path)
        {
            string currentDir = Path.GetDirectoryName(path);
            int lastIndex = currentDir.LastIndexOf(Path.DirectorySeparatorChar);

            return currentDir.Remove(lastIndex);
        }

        public string GetInputDocPath(string fileName)
        {
            return GetInputFilePath(fileName, DOC);
        }

        public string GetInputPdfPath(string fileName)
        {
            return GetInputFilePath(fileName, PDF);
        }

        private string GetOutputFilePath(string inputPath)
        {
            StringBuilder builder = new StringBuilder(GetParentDirectory(inputPath));
            builder.Append(Path.DirectorySeparatorChar).Append(PDF);

            string outParentDir = builder.ToString();
            if (!Directory.Exists(outParentDir))
            {
                Directory.CreateDirectory(outParentDir);
            }

            String inputName = Path.GetFileNameWithoutExtension(inputPath);
            builder.Append(Path.DirectorySeparatorChar).Append(Path.ChangeExtension(inputName, PDF));

            return builder.ToString();
        }

        public string Convert(string inputPath)
        {
            string outputPath = GetOutputFilePath(inputPath);

            string extension = Path.GetExtension(inputPath);

            Converter converter = ConverterFactory.GetConverter(extension);

            converter.Convert(inputPath, outputPath);

            return outputPath;
        }
    }
}