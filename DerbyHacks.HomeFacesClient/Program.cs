using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing.Imaging;
using DerbyHacks.HomeFacesClient;
using System.Diagnostics;
using System.Threading;
using System.Net;

namespace DerbyHacks.HomeFacesClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press to capture image");
            Console.ReadKey();
            Capture capture = new Capture();

            Thread.Sleep(1000);

            capture.Grab();
            Mat frame = capture.QueryFrame();
            frame = capture.QueryFrame();
            Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();
            
            Save(image, "C:\\homeFaces.png", 100);

            WebRequest req = WebRequest.Create("http://localhost:1795/api/HomeFaces");
            req.Method = "POST";
            req.ContentLength = 0;
            req.ContentType = "application/json";

            try
            {
                WebResponse response = req.GetResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }

        public static void Save(Emgu.CV.Image<Bgr, Byte> img, string filename, double quality)
        {
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(
                System.Drawing.Imaging.Encoder.Quality,
                (long)quality
                );

            var jpegCodec = (from codec in ImageCodecInfo.GetImageEncoders()
                             where codec.MimeType == "image/jpeg"
                             select codec).Single();

            img.Bitmap.Save(filename, jpegCodec, encoderParams);
        }
    }
}

