using System;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace pdfsvc.Converters
{
    public class ExcelConverter : Converter
    {
        private Excel.Application _app = null;
        private Excel.Workbooks _books = null;

        public ExcelConverter()
        {
            _app = new Excel.Application();
            _app.Visible = false;

            _books = _app.Workbooks;
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
                Excel.Workbook book = _books.Open(inputFile);

                // 转换文档
                book.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, outputFile);

                // 关闭文档
                Close(book);
            }
            catch (Exception e)
            {
                // 打开、转换文档失败
                throw new ConvertException(e);
            }
        }

        private void Close(Excel.Workbook book)
        {
            try
            {
                book.Close(false);
                ReleaseCOMObject(book);
            }
            catch
            {
                // TODO 关闭文档失败
            }
        }

        ~ExcelConverter()
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

            Console.WriteLine("关闭Excel！");

            try
            {
                if (_books != null)
                {
                    _books.Close();
                    ReleaseCOMObject(_books);
                    _books = null;
                }

                _app.Quit();
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