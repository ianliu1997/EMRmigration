namespace PalashDynamics.DataAccessLayer
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public class DemoImage
    {
        public ImageCodecInfo GetEncoder(ImageFormat format)
        {
            foreach (ImageCodecInfo info in ImageCodecInfo.GetImageDecoders())
            {
                if (info.FormatID == format.Guid)
                {
                    return info;
                }
            }
            return null;
        }

        public void VaryQualityLevel(byte[] imagesSource, string imgName, string imgLocation)
        {
            try
            {
                ImageConverter converter1 = new ImageConverter();
                MemoryStream stream1 = new MemoryStream();
                ImageCodecInfo encoder = this.GetEncoder(ImageFormat.Jpeg);
                EncoderParameters encoderParams = new EncoderParameters(1);
                EncoderParameter parameter = new EncoderParameter(Encoder.Quality, (long) 50);
                encoderParams.Param[0] = parameter;
                new Bitmap(Image.FromStream(new MemoryStream(imagesSource)), new Size(150, 150)).Save(imgLocation + imgName, encoder, encoderParams);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
        }

        public void VaryQualityLevelScanDocument(byte[] imagesSource, string imgName, string imgLocation)
        {
            try
            {
                ImageConverter converter1 = new ImageConverter();
                MemoryStream stream1 = new MemoryStream();
                ImageCodecInfo encoder = this.GetEncoder(ImageFormat.Jpeg);
                EncoderParameters encoderParams = new EncoderParameters(1);
                EncoderParameter parameter = new EncoderParameter(Encoder.Quality, (long) 50);
                encoderParams.Param[0] = parameter;
                new Bitmap(Image.FromStream(new MemoryStream(imagesSource))).Save(imgLocation + imgName, encoder, encoderParams);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
        }
    }
}

