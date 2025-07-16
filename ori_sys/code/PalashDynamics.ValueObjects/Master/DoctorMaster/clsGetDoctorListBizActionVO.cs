using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
  public   class clsGetDoctorListBizActionVO:IBizActionValueObject
    {
      public clsGetDoctorListBizActionVO()
      {
      }
      private List<clsDoctorVO> myVar = new List<clsDoctorVO>();

        public List<clsDoctorVO> DoctorDetailsList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public bool IsComboBoxFill { get; set; }

        public DateTime? AppDate { get; set; }

        public long? UnitId { get; set; }

        public long? DepartmentId { get; set; }
        
        //Added by Bhushanp 09/01/2017
        public bool IsInternal { get; set; }
        public bool IsExternal { get; set; }

        #region IBizAction Members
        /// <summary>
        /// Retuns the bizAction Class Name.
        /// </summary>
        /// <returns></returns>
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorListBizAction";
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
