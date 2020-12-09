using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace pdfsvc.Controllers
{
    /// <summary>
    /// PDF转换和签名控制器
    /// </summary>
    [ApiController]
    [Route("/pdf")]
    public class PdfController : ControllerBase
    {
        private readonly ILogger<PdfController> _logger;

        public PdfController(ILogger<PdfController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 根据ID获取PDF文件
        /// </summary>
        /// <param name="id">PDF文件ID</param>
        /// <returns>PDF文件</returns>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            _logger.LogInformation("get /pdf/{id}", id);
            return "Hello World";
        }
    }
}