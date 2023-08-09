using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfiumPrinter;

namespace TestPdfium
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var printerName = "HP LaserJet Pro M404-M405 PCL 6 (V3)"; //You can use your own printer;
            var printer = new PdfPrinter(printerName);
            var printFile = "E:\\S20211230053342DTNT_63295746826.pdf"; //The path to the pdf which needs to be printed;
            printer.Print(printFile);
        }
    }
}
