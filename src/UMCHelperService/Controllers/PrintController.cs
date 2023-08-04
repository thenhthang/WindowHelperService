using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintIt.Core;

namespace UMCHelperService.Controllers
{
    [ApiController]
    [Route("printpdf")]
    public class PrintController : ControllerBase
    {
        private readonly IPdfPrintService _pdfPrintService;

        public PrintController(IPdfPrintService pdfPrintService)
        {
            _pdfPrintService = pdfPrintService;
        }
        [HttpGet]
       
        [Route("test")]
        public IActionResult PrintFromPdf(string a)
        {
            return Ok(a);
        }
        [HttpPost]
        [Route("from-formfile")]
        public async Task<IActionResult> PrintFromPdf([FromForm] PrintFromPdfTemplateRequest request)
        {
            await using Stream pdfStream = request.PdfFile.OpenReadStream();
            _pdfPrintService.Print(pdfStream, request.PrinterName, request.PageRange);
            
            return Ok("Da gui lenh in den may in");
        }

        [HttpPost]
        [Route("from-base64")]
        public async Task<IActionResult> PrintFromBase64([FromForm] PrintFromBase64TemplateRequest request)
        {
            await using Stream pdfStream = new MemoryStream(Convert.FromBase64String(request.PdfBase64));

            _pdfPrintService.Print(pdfStream, request.PrinterName, request.PageRange);
           
            return Ok("Da gui lenh in den may in");
        }
        
    }

    public sealed class PrintFromPdfTemplateRequest
    {
        [Required]
        public IFormFile PdfFile { get; set; }

        public string PrinterName { get; set; }

        public string PageRange { get; set; }
    }

    public sealed class PrintFromBase64TemplateRequest
    {
        [Required]
        public string PdfBase64 { get; set; }

        public string PrinterName { get; set; }

        public string PageRange { get; set; }
    }
}
