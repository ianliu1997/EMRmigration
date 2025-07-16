using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.PatientSourceMaster
{
    public class clsGetTariffDetailsListBizActionVO : IBizActionValueObject
    {
        // Added by Ashish Z.
        public bool IsPagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public string SearchExpression { get; set; }
        public int TotalRows { get; set; }
        public bool IsFromCompanyMaster { get; set; }
        //

        private List<clsTariffDetailsVO> _PatientSourceDetails;
        public List<clsTariffDetailsVO> PatientSourceDetails
        {
            get { return _PatientSourceDetails; }
            set { _PatientSourceDetails = value; }
        }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PatientSourceMaster.clsGetTariffDetailsListBizAction";
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
