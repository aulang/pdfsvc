using System;
using System.Collections.Generic;

namespace pdfsvc.Converters
{
    enum DocumentType
    {
        DOC,
        DOCX,
        XLS,
        XLSX,
        PPT,
        PPTX
    }

    public class ConverterFactory
    {
        private const string DOT = ".";

        private static Dictionary<DocumentType, Converter> converters = new Dictionary<DocumentType, Converter>();

        private static Converter NewInstance(DocumentType type)
        {
            switch (type)
            {
                case DocumentType.DOC:
                case DocumentType.DOCX:
                    return new WordConverter();
                case DocumentType.XLS:
                case DocumentType.XLSX:
                    return new ExcelConverter();
                case DocumentType.PPT:
                case DocumentType.PPTX:
                    return new PowerPointConverter();
                default:
                    return null;
            }
        }
        public static Converter GetConverter(string extension)
        {
            if (extension.StartsWith(DOT))
            {
                extension = extension.Remove(0, 1).ToUpper();
            }

            DocumentType type = (DocumentType)Enum.Parse(typeof(DocumentType), extension);

            if (converters.ContainsKey(type))
            {
                return converters[type];
            }

            Converter converter = NewInstance(type);
            if (converter == null)
            {
                throw new ConvertException("不支持的文件格式！");
            }

            converters.Add(type, converter);
            return converter;
        }
    }
}