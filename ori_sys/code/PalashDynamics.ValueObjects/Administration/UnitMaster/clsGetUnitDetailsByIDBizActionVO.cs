using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.UnitMaster
{
  public class clsGetUnitDetailsByIDBizActionVO:IBizActionValueObject
    {
        private clsUnitMasterVO objDetails = null;
        public clsUnitMasterVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<MasterListItem> _objMasterList;

        public List<MasterListItem> ObjMasterList
        {
            get { return _objMasterList; }
            set { _objMasterList = value; }
        }

        private long _ID;
        public long UnitID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private long _UserID;

        public long UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        private bool _Status;
        public bool SuccessStatus
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private bool _IsUserWise;

        public bool IsUserWise
        {
            get { return _IsUserWise; }
            set { _IsUserWise = value; }
        }
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.UnitMaster.clsGetUnitDetailsByIDBizAction";
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
