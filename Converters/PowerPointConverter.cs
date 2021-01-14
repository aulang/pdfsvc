using System;
using System.IO;
using Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace pdfsvc.Converters
{

    public class PowerPointConverter : Converter
    {
        private PowerPoint.Application _app = null;

        public PowerPointConverter()
        {
            _app = new PowerPoint.Application();
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
                PowerPoint.Presentation ppt = _app.Presentations.Open(inputFile);

                // 转换文档
                ppt.ExportAsFixedFormat(outputFile, PowerPoint.PpFixedFormatType.ppFixedFormatTypePDF);

                // 关闭文档
                close(ppt);
            }
            catch (Exception e)
            {
                // 打开、转换文档失败
                throw new ConvertException(e);
            }
        }

        private void close(PowerPoint.Presentation ppt)
        {
            try
            {
                ppt.Close();
                ReleaseCOMObject(ppt);
            }
            catch
            {
                // TODO 关闭文档失败
            }
        }

        ~PowerPointConverter()
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

            try
            {
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