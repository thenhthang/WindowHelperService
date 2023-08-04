using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PrintIt.Core;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System;
using System.IO;
using System.Security.Principal;

namespace UMCHelperService.Controllers
{
    [ApiController]
    [Route("")]
    public sealed class HomeController : ControllerBase
    {
        private readonly IPrinterService _printerService;

        public HomeController(IPrinterService printerService)
        {
            _printerService = printerService;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            string msg = "Started: Support GetPrinters, GetComputerName, GetIP, GetWindowsUserName, PrintPDF";
            return Ok(msg);
        }
        [HttpGet]
        [Route("list")]
        public IActionResult ListPrinters()
        {
            string[] installedPrinters = _printerService.GetInstalledPrinters();
            return Ok(installedPrinters);
        }

        [HttpPost]
        [Route("install")]
        public IActionResult InstallPrinter([FromQuery] string printerPath)
        {
            _printerService.InstallPrinter(printerPath);
            return Ok();
        }
        
        [HttpGet]
        [Route("GetPrinters")]
        public IActionResult GetPrinters()
        {
            //List<string> lstPrinters = new List<string>();
            //foreach (string PrintName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //{
            //    lstPrinters.Add(PrintName);
            //}
            string[] installedPrinters = _printerService.GetInstalledPrinters();
            return Ok(installedPrinters);
        }
        [HttpGet]
        [Route("GetComputerName")]
        public IActionResult GetComputerName()
        {
            return Ok(System.Environment.MachineName);
        }
        [HttpGet]
        [Route("GetIP")]
        public IActionResult GetIP()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST

            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            return Ok(myIP);
        }
        [HttpGet]
        [Route("GetWindowsUserName")]
        public IActionResult GetWindowsUserName()
        {
            string[] temp = Convert.ToString(WindowsIdentity.GetCurrent().Name).Split('\\');
            string userName = temp[1];
            return Ok(userName);
        }
    }
}
