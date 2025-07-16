using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
   public class clsGetRefernceDoctorBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }
        private List<clsComboMasterBizActionVO> _ComboList = new List<clsComboMasterBizActionVO>();
        public List<clsComboMasterBizActionVO> ComboList
        {
            get { return _ComboList; }
            set { _ComboList = value; }
        }

        public long UnitId { get; set; }
        public long Id { get; set; }

        public bool IsFromVisit { get; set; }

        private MasterTableNameList _MasterTable = MasterTableNameList.None;
        public MasterTableNameList MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }

        public bool _SuccessStatus;
        public bool SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private bool _IsDecode;
        public bool IsDecode
        {
            get { return _IsDecode; }
            set { _IsDecode = value; }
        }

       

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetRefernceDoctorBizAction";
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
