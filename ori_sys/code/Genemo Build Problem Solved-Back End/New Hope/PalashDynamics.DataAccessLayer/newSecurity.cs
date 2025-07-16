namespace PalashDynamics.DataAccessLayer
{
    using Microsoft.VisualBasic;
    using System;
    using System.Runtime.InteropServices;

    internal static class newSecurity
    {
        public static string EncryptDecryptAccessKey(string sAccessKey, bool bFlag)
        {
            int start = 0;
            string str = null;
            str = "";
            for (start = 1; start <= sAccessKey.Length; start++)
            {
                str = bFlag ? (str + Strings.Chr((Strings.Asc(Convert.ToString((int) (Convert.ToInt32(Strings.Mid(sAccessKey, start, 1)) - start))) * 2) - 100)) : (str + Strings.Chr((Strings.Asc(Strings.Mid(sAccessKey, start, 1) + ((int) 100)) / 2) + start));
            }
            return str;
        }

        public static string EncryptDecryptUserKey(string sUserKey, bool bFlag)
        {
            int start = 0;
            string str = null;
            str = "";
            for (start = 1; start <= sUserKey.Length; start++)
            {
                str = bFlag ? (str + Strings.Chr((Strings.Asc(Strings.Mid(sUserKey, start, 1)) + 5) / 2)) : (str + Strings.Chr((Strings.Asc(Strings.Mid(sUserKey, start, 1)) * 2) - 5));
            }
            return str;
        }
    }
}

