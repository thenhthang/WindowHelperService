using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
            return Ok("Available");
        }
        //In bằng pdfium không hiển thị được chữ ký số
        [HttpPost]
        [Route("from-formfile")]
        public async Task<IActionResult> PrintFromPdf([FromForm] PrintFromPdfTemplateRequest request)
        {

            await using Stream pdfStream = request.PdfFile.OpenReadStream();
            _pdfPrintService.Print(pdfStream, request.PrinterName, request.PageRange);
            
            return Ok("Da gui lenh in den may in");
        }
        //Gọi exe file, sử dụng thư viên Spire để in file, có hiển thị chữ ký số
        //Window service không gọi trực tiếp Spire được, phải gọi qua file exe
        [HttpPost]
        [Route("from-formfile2")]
        public async Task<IActionResult> PrintFromPdf2([FromForm] PrintFromPdfTemplateRequest request)
        {
            //await using Stream pdfStream = request.PdfFile.OpenReadStream();
            string sPath =  Directory.GetCurrentDirectory() + "\\Temporary\\" + Guid.NewGuid().ToString() + ".pdf";
            await using FileStream fs = new FileStream(sPath, FileMode.Create);
            await request.PdfFile.CopyToAsync(fs);
            fs.Close();

            PrintToPrinter(sPath,request.PrinterName);

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

        

        [HttpPost]
        [Route("from-base642")]
        public async Task<IActionResult> PrintFromBase642([FromForm] PrintFromBase64TemplateRequest request)
        {
            await using Stream pdfStream = new MemoryStream(Convert.FromBase64String(request.PdfBase64));
            string sPath = Directory.GetCurrentDirectory() + "\\Temporary\\" + Guid.NewGuid().ToString() + ".pdf";
            await using FileStream fs = new FileStream(sPath, FileMode.Create);
            await pdfStream.CopyToAsync(fs);
            fs.Close();

            PrintToPrinter(sPath, request.PrinterName);

            return Ok("Da gui lenh in den may in");
        }

        private void PrintToPrinter(string pdfPath, string printerName)
        {
            string printExe = Directory.GetCurrentDirectory() + "\\print.exe";
            Process process = new Process();
            process.StartInfo.FileName = printExe;
            StringBuilder str = new StringBuilder("\"");
            str.Append(printerName);
            str.Append(";");
            str.Append(pdfPath);
            str.Append("\"");
            process.StartInfo.Arguments = str.ToString();
            process.Start();
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
