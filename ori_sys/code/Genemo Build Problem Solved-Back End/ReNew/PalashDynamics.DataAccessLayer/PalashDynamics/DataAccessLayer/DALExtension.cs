namespace PalashDynamics.DataAccessLayer
{
    using System;
    using System.Runtime.CompilerServices;

    internal static class DALExtension
    {
        public static object HandleDBNull(this object Value)
        {
            return (!(Value is DBNull) ? Value : null);
        }
    }
}

