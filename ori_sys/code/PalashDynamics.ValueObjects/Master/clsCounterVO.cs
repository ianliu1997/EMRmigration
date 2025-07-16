using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsCounterVO : IValueObject
    {
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public bool Active { get; set; }

    }
   
}
