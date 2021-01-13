using System;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace pdfsvc.Converters
{
    public class WordConverter : Converter
    {
        private Word.Application _app;

        public WordConverter()
        {
            _app = new Word.Application();
            _app.Visible = false;
        }

        public override void Convert(string inputFile, string outputFile)
        {
            if (!File.Exists(inputFile))
            {
                throw new ConvertException("文件不存在！");
            }

            try
            {
                // 打开文档
                Word.Document doc = _app.Documents.Open(inputFile);

                // 转换文档
                doc.ExportAsFixedFormat(outputFile, Word.WdExportFormat.wdExportFormatPDF);

                // 关闭文档
                close(doc);
            }
            catch (Exception e)
            {
                // 打开、转换文档失败
                throw new ConvertException(e);
            }
        }

        private void close(Word.Document doc)
        {
            try
            {
                doc.Close(false);
                ReleaseCOMObject(doc);
            }
            catch
            {
                // TODO 关闭文档失败
            }
        }

        ~WordConverter()
        {
            try
            {
                _app.Quit(false);
                ReleaseCOMObject(_app);
            }
            catch
            {
                // TODO 退出Wold程序失败
            }
        }
    }
}