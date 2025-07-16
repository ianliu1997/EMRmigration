using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsAddModalityMasterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Radiology.clsAddModalityMasterBizAction";  //BL
        }

        #endregion
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private long _ResultStatus;
        public long ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        public clsModalityMasterVO modalityObj { get; set; }
    }

    public class clsUpdateModalityMasterBizActionVO : IBizActionValueObject
    {
        private clsModalityMasterVO objModalityMaster;
        public clsModalityMasterVO ModalityMasterDetails
        {
            get { return objModalityMaster; }
            set { objModalityMaster = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _ResultStatus;
        public long ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Radiology.clsUpdateModalityMasterBizAction";
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
