using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsGetCompanyDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetCompanyDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        public long CompanyId { get; set; }
        //public string Description { get; set; }
        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsCompanyVO> objItemMaster = new List<clsCompanyVO>();
        public List<clsCompanyVO> ItemMatserDetails
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }
        //rohinee
        public string Description { get; set; }
        //
        private List<clsTariffDetailsVO> objTariff = new List<clsTariffDetailsVO>();
        public List<clsTariffDetailsVO> TariffDetails
        {
            get
            {
                return objTariff;
            }
            set
            {
                objTariff = value;

            }
        }


        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
    }
}
