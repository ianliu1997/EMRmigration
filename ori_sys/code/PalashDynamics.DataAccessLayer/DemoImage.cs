using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;


namespace PalashDynamics.DataAccessLayer
{
    public class DemoImage
    {
        public void VaryQualityLevel(byte[] imagesSource,string imgName,string imgLocation)
        {
            // Get a bitmap.

            try
            {
                MemoryStream ms = new MemoryStream(imagesSource);
                Image image = Image.FromStream(ms);

                Image Img = (Image)(new Bitmap(image, new Size(600, 600)));

                //MemoryStream ms1 = new MemoryStream();
                //image.Save(ms1, System.Drawing.Imaging.ImageFormat.Gif);
                //byte[] tembmp1 = ms.ToArray();


                ImageConverter converter = new ImageConverter();
                //Bitmap image = (Bitmap)converter.ConvertFrom(imagesSource);


                // Bitmap bmp1 =new Bitmap(image);  

                //Bitmap bmp1 = new Bitmap(@"D:/DSC_0204.jpg");
                //byte[] tembmp1 = (byte[])converter.ConvertTo(image, typeof(byte[]));

                MemoryStream mem = new MemoryStream();
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                // Create an Encoder object based on the GUID
                // for the Quality parameter category.
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter
                // objects. In this case, there is only one
                // EncoderParameter object in the array.
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                Img.Save(imgLocation + imgName, jpgEncoder, myEncoderParameters);
                //Img.Save(@"D:\Palash\PALASH_PATIENTREGISTRATION\IMAGE\" + imgName, jpgEncoder, myEncoderParameters);
                //bmp1.Save(mem, jpgEncoder, myEncoderParameters);


                //byte[] arr = (byte[])converter.ConvertTo(image, typeof(byte[]));

                //return arr;

                //myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                //myEncoderParameters.Param[0] = myEncoderParameter;
                //bmp1.Save(@"d:\TestPhotoQualityHundred.jpg", jpgEncoder, myEncoderParameters);


                //// Save the bitmap as a JPG file with zero quality level compression.
                //myEncoderParameter = new EncoderParameter(myEncoder, 0L);
                //myEncoderParameters.Param[0] = myEncoderParameter;
                //bmp1.Save(@"d:\TestPhotoQualityZero.jpg", jpgEncoder, myEncoderParameters);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

    }

}
