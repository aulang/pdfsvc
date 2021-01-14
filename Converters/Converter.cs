using System;
using System.Runtime.InteropServices;

namespace pdfsvc.Converters
{
    public abstract class Converter : IDisposable
    {
        public abstract void Convert(string inputFile, string outputFile);

        public abstract void Dispose();

        protected void ReleaseCOMObject(object obj)
        {
            try
            {
                if (Marshal.IsComObject(obj))
                {
                    Marshal.FinalReleaseComObject(obj);
                }
            }
            catch
            {
                // 无法处理异常
            }
        }
    }
}