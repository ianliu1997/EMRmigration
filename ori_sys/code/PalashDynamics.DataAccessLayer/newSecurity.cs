using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace PalashDynamics.DataAccessLayer
{
    internal static class newSecurity
    {
        public static string EncryptDecryptAccessKey(string sAccessKey, bool bFlag = false)
        {
            int i = 0;
            string s = null;
            string DbTp = null;
            s = "";
            for (i=1; i <= sAccessKey.Length; i++)
            //for (i = 1; i <= Strings.Len(Strings.Trim(sAccessKey)); i++)
            {
                if (bFlag == false)
                {                    
                    s = s + Strings.Chr((Strings.Asc(Strings.Mid(sAccessKey, i, 1) + 100)) / 2 + i);
                    //s = s & Chr(Asc(Mid$(sAccessKey, i, 1)) + 100 + i)
                }
                else
                {
                    int a = Convert.ToInt32(Strings.Mid(sAccessKey, i, 1));
                    string first = Convert.ToString(a-i);
                    s = s + Strings.Chr((Strings.Asc(first) * 2 - 100));
                    //s = s & Chr(Asc(Mid$(sAccessKey, i, 1)) - 100 - i)
                }
            }
            return s;
        }

        public static string EncryptDecryptUserKey(string sUserKey, bool bFlag = false)
        {
            int i = 0;
            string s = null;
            string DbTp = null;
            s = "";
           // for (i = 1; i <= Strings.Len(Strings.Trim(sUserKey)); i++)
            for(i=1; i<=sUserKey.Length; i++)
            {
                if (bFlag == false)
                {
                    s = s + Strings.Chr(Strings.Asc(Strings.Mid(sUserKey, i, 1)) * 2 - 5);
                    // 97  43
                }
                else
                {
                    s = s + Strings.Chr((Strings.Asc(Strings.Mid(sUserKey, i, 1)) + 5) / 2);
                }
            }
            return s;
        }
    }

    
}
