using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.IO;
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

        public PdfController(ILogger<PdfController> logger, FileManager fileManager)
        {
            _logger = logger;
            _fileManager = fileManager;
        }

        /// <summary>
        /// Office格式(word、excel、ppt)文件转PDF
        /// </summary>
        /// <param name="file">PDF文件</param>
        /// <returns>PDF文件</returns>
        [HttpPost("convert")]
        public async Task<IActionResult> Convert([Required] IFormFile file)
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

            // 更改文件后缀
            fileName = Path.ChangeExtension(fileName, FileManager.PDF);
            // 文件名UTF-8编码
            // fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);

            return PhysicalFile(outputFilePath, "application/pdf", fileName);
        }
    }
}
