using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.Indent
{
    public class clsUpdateRemarkForIndentCancellationBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.Indent.clsUpdateRemarkForIndentCancellationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        public Boolean IsEditMode { get; set; }
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
        private clsIndentMasterVO _IndentMaster;
        public clsIndentMasterVO IndentMaster
        {
            get
            {
                return _IndentMaster;
            }
            set
            {
                if (_IndentMaster != value)
                {
                    _IndentMaster = value;

                }
            }
        }
        public string CancellationRemark { get; set; }
        public bool IsRejectIndent { get; set; }
    }
}
