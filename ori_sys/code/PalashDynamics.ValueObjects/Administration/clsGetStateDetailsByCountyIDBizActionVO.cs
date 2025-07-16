using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsGetStateDetailsByCountyIDBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetStateDetailsByCountyIDBizAction";
        }
        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        //public long StartRowIndex { get; set; }
        //public long MaximumRows { get; set; }
        //public long TotalRows { get; set; }
        //public bool PagingEnabled { get; set; }
        //public string SearchExpression { get; set; }
        public long CountryId { get; set; }
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsStateVO> objListStateDetails = new List<clsStateVO>();
        public List<clsStateVO> ListStateDetails
        {
            get
            {
                return objListStateDetails;
            }
            set
            {
                objListStateDetails = value;

            }
        }

        private clsStateVO _objState;
        public clsStateVO ObjState
        {
            get { return _objState; }
            set
            {
                _objState = value;
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

    public class clsGetCityDetailsByStateIDBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetCityDetailsByStateIDBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        //public long StartRowIndex { get; set; }
        //public long MaximumRows { get; set; }
        //public long TotalRows { get; set; }
        //public bool PagingEnabled { get; set; }
        //public string SearchExpression { get; set; }

        //public long CityId { get; set; }
        //public long UnitId { get; set; }
        //public string Code { get; set; }
        //public string Description { get; set; }
        //public long CountryId { get; set; }
        public long StateId { get; set; }
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsCityVO> objListCityDetails = new List<clsCityVO>();
        public List<clsCityVO> ListCityDetails
        {
            get
            {
                return objListCityDetails;
            }
            set
            {
                objListCityDetails = value;

            }
        }

        private clsCityVO _objCity;
        public clsCityVO ObjCity
        {
            get { return _objCity; }
            set
            {
                _objCity = value;
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

    public class clsGetRegionDetailsByCityIDBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetRegionDetailsByCityIDBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        //public long StartRowIndex { get; set; }
        //public long MaximumRows { get; set; }
        //public long TotalRows { get; set; }
        //public bool PagingEnabled { get; set; }
        //public string SearchExpression { get; set; }

        //public long RegionId { get; set; }
        //public long UnitId { get; set; }
        //public string Code { get; set; }
        //public string Description { get; set; }
        //public long CountryId { get; set; }
        //public long StateId { get; set; }
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

    #region Defined to get optimised data in DAL (in terms of Data Size) 09012017

    public class clsGetRegionDetailsByCityIDForRegBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetRegionDetailsByCityIDForRegBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long CityId { get; set; }
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsRegionForRegVO> objListRegionDetails = new List<clsRegionForRegVO>();
        public List<clsRegionForRegVO> ListRegionDetails
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

        private clsRegionForRegVO _objRegion;
        public clsRegionForRegVO ObjRegion
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

    #endregion

}
