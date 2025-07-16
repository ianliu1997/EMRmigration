using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddServiceMasterBizActionVO:IBizActionValueObject
    {
        public clsAddServiceMasterBizActionVO()
        {

        }

        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }

        }

        private List<clsServiceMasterVO> objServiceClassList = new List<clsServiceMasterVO>();
        public List<clsServiceMasterVO> ServiceClassList
        {
            get
            {
                return objServiceClassList;
            }
            set
            {
                objServiceClassList = value;
            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        #region IBizActionValueObject Members

        public bool IsModify { get; set; }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddServiceMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        private bool _IsOLDServiceMaster = false;
        public bool IsOLDServiceMaster
        {
            get { return _IsOLDServiceMaster; }
            set { _IsOLDServiceMaster = value; }
        }

        public bool UpdateServiceMasterStatus { get; set; }
        #endregion
    }
}
