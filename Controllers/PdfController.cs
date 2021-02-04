using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Web;
using System.IO;
using System.Text;
using pdfsvc.Business;

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
        private readonly FileManager _fileManager;
        private readonly FdfManager _pdfManager;

        public PdfController(ILogger<PdfController> logger, FileManager fileManager, FdfManager pdfManager)
        {
            _logger = logger;
            _fileManager = fileManager;
            _pdfManager = pdfManager;
        }

        /// <summary>
        /// Office格式(word、excel、ppt)文件转PDF
        /// </summary>
        /// <param name="file">PDF文件</param>
        /// <param name="sign">是否签名</param>
        /// <param name="regex">签名标记正则表达式</param>
        /// <param name="pages">标记查找页码，负数倒数页码</param>
        /// <param name="latest">是否在最后一个标记处签名</param>
        /// <returns>PDF文件</returns>
        [HttpPost("convert")]
        public async Task<IActionResult> Convert([Required] IFormFile file,
            [FromForm] bool sign = false,
            [FromForm] string regex = null,
            [FromForm] List<int> pages = null,
            [FromForm] bool latest = true)
        {
            // 文件名
            string fileName = Path.GetFileName(file.FileName);

            // 上传Office文件保持路径
            string inputFilePath = _fileManager.GetInputDocPath(fileName);

            using (FileStream stream = new FileStream(inputFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 输出PDF文件保存路径
            string outputFilePath;
            try
            {
                // PDF转换
                outputFilePath = _fileManager.Convert(inputFilePath);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "转换PDF出错");
                return Problem(e.StackTrace, e.Message, StatusCodes.Status500InternalServerError);
            }

            if (sign)
            {
                // TODO PDF 签名盖章
            }

            // 更改文件后缀
            fileName = Path.ChangeExtension(fileName, FileManager.PDF);
            // 文件名UTF-8编码
            fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);

            return PhysicalFile(outputFilePath, "application/pdf", fileName);
        }

        /// <summary>
        /// PDF文件签名盖章
        /// </summary>
        /// <param name="file">PDF文件</param>
        /// <param name="regex">签名标记正则表达式</param>
        /// <param name="pages">标记查找页码，负数倒数页码</param>
        /// <param name="latest">是否在最后一个标记处签名</param>
        /// <returns>PDF文件</returns>
        [HttpPost("sign")]
        public IActionResult Sign([Required] IFormFile file,
            [FromForm] string regex = null,
            [FromForm] List<int> pages = null,
            [FromForm] bool latest = true)
        {
            _logger.LogInformation("PDF文件签名盖章");

            // 文件名
            string fileName = Path.GetFileName(file.FileName);
            // TODO PDF签名

            // 文件名UTF-8编码
            fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
            return Ok();
        }
    }
}