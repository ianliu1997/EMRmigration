using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddUpadateCountryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpadateCountryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsCountryVO> objListCountryDetails = new List<clsCountryVO>();
        public List<clsCountryVO> ListCountryDetails
        {
            get
            {
                return objListCountryDetails;
            }
            set
            {
                objListCountryDetails = value;

            }
        }

        private clsCountryVO _objCountry;
        public clsCountryVO ObjCountry
        {
            get { return _objCountry; }
            set
            {
                _objCountry = value;
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

    public class clsCountryVO
    {
        public long Id { get; set; }
        public long UnitId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Nationality { get; set; }
        public Boolean Status { get; set; }
        public long? CreatedUnitID { get; set; }
        public long? UpdatedUnitID { get; set; }
        public long? AddedBy { get; set; }
        public string AddedOn { get; set; }
        public DateTime? AddedDateTime { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string AddedWindowsLoginName { get; set; }
        public string UpdateWindowsLoginName { get; set; }
    }


    public class clsGetCountryDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetCountryDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion


        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }


        public long UnitId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long CountryId { get; set; }
        public string Nationality { get; set; }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsCountryVO> objListCountryDetails = new List<clsCountryVO>();
        public List<clsCountryVO> ListCountryDetails
        {
            get
            {
                return objListCountryDetails;
            }
            set
            {
                objListCountryDetails = value;

            }
        }

        private clsCountryVO _objCountry;
        public clsCountryVO ObjCountry
        {
            get { return _objCountry; }
            set
            {
                _objCountry = value;
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
