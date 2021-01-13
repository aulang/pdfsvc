using System;
using System.Collections.Generic;

namespace pdfsvc.Converters
{
    public class ConverterFactory
    {
        private static Converter wordConverter = new WordConverter();
        private static Converter excelConverter = new ExcelConverter();
        private static Converter powerPointConverter = new PowerPointConverter();

        private static Converter GetByType(String type)
        {
            switch (type)
            {
                case "DOC":
                case "DOCX":
                    return wordConverter;
                case "XLS":
                case "XLSX":
                    return excelConverter;
                case "PPT":
                case "PPTX":
                    return powerPointConverter;
                default:
                    throw new ConvertException("不支持的文件格式！");
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
    }
}