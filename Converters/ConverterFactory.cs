using System;

namespace pdfsvc.Converters
{

    public class ConverterFactory
    {
        private static Converter wordConverter = null;
        private static Converter excelConverter = null;
        private static Converter powerPointConverter = null;

        private static Converter GetByType(String type)
        {
            switch (type)
            {
                case "DOC":
                case "DOCX":
                    return wordConverter != null ? wordConverter : (wordConverter = new WordConverter());
                case "XLS":
                case "XLSX":
                    return excelConverter != null ? excelConverter : (excelConverter = new ExcelConverter());
                case "PPT":
                case "PPTX":
                    return powerPointConverter != null ? powerPointConverter : (powerPointConverter = new PowerPointConverter());
                default:
                    throw new ConvertException("不支持的文件格式！");
            }
        }

        private static void DisposeConverter(Converter converter)
        {
            if (converter != null)
            {
                converter.Dispose();
            }
        }

        public static Converter GetConverter(string extension)
        {
            string type = extension;
            if (type.StartsWith("."))
            {
                type = type.Remove(0, 1).ToUpper();
            }

            return GetByType(type);
        }

        public static void DisposeConverters()
        {
            // 释放资源，关闭Office程序
            DisposeConverter(wordConverter);
            DisposeConverter(excelConverter);
            DisposeConverter(powerPointConverter);
            Console.WriteLine("程序退出，关闭Office");
        }
    }
}