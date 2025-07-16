using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.VisitType
{
    public class clsCheckVisitTypeMappedWithPackageServiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionVO Member
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsCheckVisitTypeMappedWithPackageServiceBizAction";
        }
        #endregion


        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }

        }


        public long VisitTypeID { get; set; }

        public bool Res_ISPackageService { get; set; }
    }
}
