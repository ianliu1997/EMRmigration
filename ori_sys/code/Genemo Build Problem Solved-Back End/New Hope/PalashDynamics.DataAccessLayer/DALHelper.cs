namespace PalashDynamics.DataAccessLayer
{
    using System;

    internal class DALHelper
    {
        public static object HandleBoolDBNull(object Value)
        {
            return (!(Value is DBNull) ? Value : false);
        }

        public static DateTime? HandleDate(object Value)
        {
            if (!(Value is DBNull) && !string.IsNullOrEmpty(Value.ToString()))
            {
                return ((Convert.ToDateTime(Value) >= new DateTime(0x6d9, 1, 1)) ? ((Convert.ToDateTime(Value) <= new DateTime(0x270f, 12, 0x1f)) ? ((DateTime?) Value) : new DateTime(0x270f, 12, 0x1f)) : new DateTime(0x6d9, 1, 1));
            }
            return null;
        }

        public static object HandleDBNull(object Value)
        {
            return (!(Value is DBNull) ? Value : null);
        }

        public static double HandleDoubleNull(object Value)
        {
            return (!(Value is DBNull) ? ((double) Value) : 0.0);
        }

        public static long HandleIntegerNull(object Value)
        {
            return (!(Value is DBNull) ? ((long) Value) : 0L);
        }

        public static bool IsValidDateRangeDB(DateTime? Value)
        {
            DateTime? nullable = Value;
            DateTime time = new DateTime(0x6d9, 1, 1);
            if (!((nullable != null) ? (nullable.GetValueOrDefault() > time) : false))
            {
                return false;
            }
            DateTime? nullable2 = Value;
            DateTime time2 = new DateTime(0x270f, 12, 0x1f);
            return ((nullable2 != null) ? (nullable2.GetValueOrDefault() < time2) : false);
        }
    }
}

