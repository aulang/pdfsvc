using System;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace pdfsvc.Converters
{
    public class ExcelConverter : Converter
    {
        private static Excel.Application _app;

        public ExcelConverter()
        {
            if (_app == null)
            {
                _app = new Excel.Application();
                _app.Visible = false;
            }
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
                Excel.Workbook book = _app.Workbooks.Open(inputFile);

                // 转换文档
                book.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, outputFile);

                // 关闭文档
                close(book);
            }
            catch (Exception e)
            {
                // 打开、转换文档失败
                throw new ConvertException(e);
            }
        }

        private void close(Excel.Workbook book)
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
    }
}