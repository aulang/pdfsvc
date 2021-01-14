using System;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace pdfsvc.Converters
{
    public class WordConverter : Converter
    {
        private Word.Application _app = null;
        private Word.Documents _docs = null;

        public WordConverter()
        {
            _app = new Word.Application();
            _app.Visible = false;

            _docs = _app.Documents;
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
                Word.Document doc = _docs.Open(inputFile);

                // 转换文档
                doc.ExportAsFixedFormat(outputFile, Word.WdExportFormat.wdExportFormatPDF);

                // 关闭文档
                Close(doc);
            }
            catch (Exception e)
            {
                // 打开、转换文档失败
                throw new ConvertException(e);
            }
        }

        private void Close(Word.Document doc)
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
            Dispose();
        }

        public override void Dispose()
        {
            if (_app == null)
            {
                // 已经释放，无需再释放
                return;
            }

            Console.WriteLine("关闭Word！");

            try
            {
                if (_docs != null)
                {
                    _docs.Close(false);
                    ReleaseCOMObject(_docs);
                    _docs = null;
                }

                _app.Quit(false);
                ReleaseCOMObject(_app);
                _app = null;
            }
            catch
            {
                // TODO 退出Wold程序失败
            }
        }
    }
}