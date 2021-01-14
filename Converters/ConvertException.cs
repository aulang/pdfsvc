using System;

namespace pdfsvc.Converters
{
    public class ConvertException : Exception
    {
        public ConvertException(String message) : base(message)
        {
        }

        public ConvertException(Exception exception) : base(exception.Message, exception)
        {
        }
    }
}