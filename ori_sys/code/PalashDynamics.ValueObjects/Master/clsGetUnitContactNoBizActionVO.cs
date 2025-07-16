//Created Date:06/June/2013
//Created By: Nilesh Raut
//Specification: Biz Action VO for Display Unit Contact No

//Review By:
//Review Date:

//Modified By:
//Modified Date: 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsGetUnitContactNoBizActionVO : IBizActionValueObject
    {

        public long UnitID { get; set; }
        public string ContactNo { get; set; }
        public bool SuccessStatus { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetUnitContactNoBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {

            return this.ToString();
        }

        #endregion
    }
}
