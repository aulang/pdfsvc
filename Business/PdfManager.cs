using Microsoft.Extensions.Options;

namespace pdfsvc.Business
{
    public class FdfManager
    {
        private readonly SignInfo _signInfo;

        public FdfManager(IOptions<SignInfo> options)
        {
            _signInfo = options.Value;
        }

        
    }
}