using System.Runtime.InteropServices;

namespace pdfsvc.Converters
{
    public abstract class Converter
    {
        public abstract void Convert(string inputFile, string outputFile);

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
            obj = null;
        }
    }
}