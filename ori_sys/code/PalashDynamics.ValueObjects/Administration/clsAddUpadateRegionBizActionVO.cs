using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddUpadateRegionBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpadateRegionBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsRegionVO> objListRegionDetails = new List<clsRegionVO>();
        public List<clsRegionVO> ListRegionDetails
        {
            get
            {
                return objListRegionDetails;
            }
            set
            {
                objListRegionDetails = value;

            }
        }

        private clsRegionVO _objRegion;
        public clsRegionVO ObjRegion
        {
            get { return _objRegion; }
            set
            {
                _objRegion = value;
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

    public class clsRegionVO
    {
        public long Id { get; set; }
        public long UnitId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long CityId { get; set; }
        public long StateId { get; set; }
        public long CountryId { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
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
        public string PinCode { get; set; }
    }

    #region Defined to get optimised data in DAL (in terms of Data Size) 09012017

    public class clsRegionForRegVO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string PinCode { get; set; }
    }

    #endregion

    public class clsGetRegionDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetRegionDetailsBizAction";
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

        public long RegionId { get; set; }
        public long UnitId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long CountryId { get; set; }
        public long StateId { get; set; }
        public long CityId { get; set; }
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsRegionVO> objListRegionDetails = new List<clsRegionVO>();
        public List<clsRegionVO> ListRegionDetails
        {
            get
            {
                return objListRegionDetails;
            }
            set
            {
                objListRegionDetails = value;

            }
        }

        private clsRegionVO _objRegion;
        public clsRegionVO ObjRegion
        {
            get { return _objRegion; }
            set
            {
                _objRegion = value;
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


