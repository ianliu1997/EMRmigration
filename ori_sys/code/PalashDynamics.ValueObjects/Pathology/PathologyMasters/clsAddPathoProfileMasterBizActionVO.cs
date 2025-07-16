using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Pathology.PathologyMasters
{

    public class clsAddPathoProfileMasterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPathoProfileMasterBizAction";
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

        private clsPathoProfileMasterVO objDetails = null;
        public clsPathoProfileMasterVO ProfileDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


    }

    public class clsGetPathoProfileDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsPathoProfileMasterVO> objPathoProfileList = null;
        public List<clsPathoProfileMasterVO> ProfileList
        {
            get { return objPathoProfileList; }
            set { objPathoProfileList = value; }
        }

        public string Description { get; set; }
      
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string SortExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoProfileDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetPathoProfileTestByIDBizActionVO : IBizActionValueObject
    {
        private List<clsPathoProfileTestDetailsVO> objTestList = null;
        public List<clsPathoProfileTestDetailsVO> TestList
        {
            get { return objTestList; }
            set { objTestList = value; }
        }

           


        public long ProfileID { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoProfileTestByIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    //adeed by rohini dated 11.4.16

    public class clsGetPathoProfileServicesBizActionVO : IBizActionValueObject
    {
        private List<MasterListItem> objServiceList = null;
        public List<MasterListItem> ServiceList
        {
            get { return objServiceList; }
            set { objServiceList = value; }
        }

        public long ProfileID { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoProfileServicesBizAction";
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


