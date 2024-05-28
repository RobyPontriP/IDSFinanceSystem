using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp.text.pdf;

namespace IDS.Tool
{
    public class GenerateQR
    {
        public static void GenerateQRCode(string qrtext,string mapPath,string inputPdfStreammapPath, string inputImageStreammapPath, string outputPdfStreammapPath, float absoluteX, float absoluteY,float newHeight,float newWidth) {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrtext, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                //Bitmap qrCodeImage = qrCode.GetGraphic(20);

                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    //bitMap.Save(Server.MapPath("~/Image/qrCode.png"), ImageFormat.Png);
                    bitMap.Save(mapPath, ImageFormat.Png);
                    //ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }

            //using (Stream inputPdfStream = new FileStream(Server.MapPath("~/Image/doc_sls.pdf"), FileMode.Open, FileAccess.Read, FileShare.Read))
            //using (Stream inputImageStream = new FileStream(Server.MapPath("~/Image/qrCode.png"), FileMode.Open, FileAccess.Read, FileShare.Read))
            //using (Stream outputPdfStream = new FileStream(Server.MapPath("~/Image/qr_dddd.pdf"), FileMode.Create, FileAccess.Write, FileShare.None))
            //using (Stream inputPdfStream = new FileStream(inputPdfStreammapPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //using (Stream inputImageStream = new FileStream(inputImageStreammapPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //using (Stream outputPdfStream = new FileStream(outputPdfStreammapPath, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    var reader = new PdfReader(inputPdfStream);
            //    var stamper = new PdfStamper(reader, outputPdfStream);
            //    var pdfContentByte = stamper.GetOverContent(1);

            //    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);

            //    //image.SetAbsolutePosition(350, 76);
            //    //image.ScaleAbsoluteHeight(100);
            //    //image.ScaleAbsoluteWidth(100);
            //    image.SetAbsolutePosition(absoluteX, absoluteY);
            //    image.ScaleAbsoluteHeight(newHeight);
            //    image.ScaleAbsoluteWidth(newWidth);
            //    pdfContentByte.AddImage(image);
            //    stamper.Close();
            //}
        }

        public static void GenerateQRCode(string qrtext, string mapPath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrtext, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(mapPath, ImageFormat.Png);
                }
            }
        }
    }
}
