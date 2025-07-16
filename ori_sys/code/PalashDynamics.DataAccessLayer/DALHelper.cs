using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;

namespace PalashDynamics.DataAccessLayer
{
    internal class DALHelper
    {
        public static object HandleDBNull(object Value)
       {
           if (Value is DBNull)
           {
               return null;
           }
           else
           {
               return Value;
           }

       }

        public static object HandleBoolDBNull(object Value)
        {
            if (Value is DBNull)
            {
                return false;
            }
            else
            {
                return Value;
            }

        }

        public static double HandleDoubleNull(object Value)
        {
            if (Value is DBNull)
            {
                return 0;
            }
            else
            {
                return (double)Value;
            }

        }

        public static DateTime? HandleDate(object Value)
        {
            if (Value is DBNull || string.IsNullOrEmpty(Value.ToString()))
            {
                return null;// DateTime.MinValue;
            }
            else
            {
                if (Convert.ToDateTime(Value) < new DateTime(1753, 1, 1)) 
                    return new DateTime(1753, 1, 1);
                if (Convert.ToDateTime(Value) > new DateTime(9999, 12, 31))
                    return new DateTime(9999, 12, 31);

                return (DateTime?)Value;
            }

        }

        public static long HandleIntegerNull(object Value)
        {
            if (Value is DBNull)
            {
                return 0;
            }
            else
            {
                return (long)Value;
            }

        }

        public static Boolean IsValidDateRangeDB(DateTime?  Value)
        {
            if (Value > new DateTime(1753, 1, 1) && Value < new DateTime(9999, 12, 31))
                return true;
            else
                return false;
        }

    }

    internal static  class DALExtension
    {
        public static object HandleDBNull(this object Value)
        {
            if (Value is DBNull)
            {
                return null;
            }
            else
            {
                return Value;
            }

        }
               

    }
}
