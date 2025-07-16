namespace PalashDynamics.DataAccessLayer
{
    using System;
    using System.Text;

    public static class Security
    {
        public static string base64Decode(string sData)
        {
            if (string.IsNullOrEmpty(sData))
            {
                return sData;
            }
            try
            {
                System.Text.Decoder decoder = new UTF8Encoding().GetDecoder();
                byte[] bytes = Convert.FromBase64String(sData);
                char[] chars = new char[decoder.GetCharCount(bytes, 0, bytes.Length)];
                decoder.GetChars(bytes, 0, bytes.Length, chars, 0);
                return new string(chars);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string base64Encode(string sData)
        {
            string str2;
            try
            {
                if (string.IsNullOrEmpty(sData))
                {
                    str2 = sData;
                }
                else
                {
                    byte[] buffer = new byte[sData.Length];
                    str2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(sData));
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error in base64Encode" + exception.Message);
            }
            return str2;
        }
    }
}

