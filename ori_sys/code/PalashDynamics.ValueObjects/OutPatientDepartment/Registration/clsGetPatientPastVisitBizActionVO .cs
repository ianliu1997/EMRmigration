using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration
{
  public class clsGetPatientPastVisitBizActionVO :IBizActionValueObject
    {
        private List<clsVisitVO> objVisit = new List<clsVisitVO>();
        public List<clsVisitVO> VisitList
        {
            get { return objVisit; }
            set { objVisit = value; }
        }

        private string _MRNO;
        public string MRNO
        {
            get { return _MRNO; }
            set { _MRNO = value; }
        }
        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set { _PatientUnitID = value; }
        }
 private long _SpouseID;
        public long SpouseID
        {
            get { return _SpouseID; }
            set { _SpouseID = value; }
        }
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public string LinkServer { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientPastVisitBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }

  public class clsGetPendingVisitBizActioVO : IBizActionValueObject
  {
      private int _Count;
      public int Count
      {
          get { return _Count; }
          set { _Count = value; }
      }



      #region IBizActionValueObject Members

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.clsGetPendingVisitBizAction";
      }

      #endregion

      #region IValueObject Members

      public string ToXml()
      {
          return this.ToString();
      }

      #endregion
  }

  public class clsClosePendingVisitBizActioVO : IBizActionValueObject
  {
      


      #region IBizActionValueObject Members

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.clsClosePendingVisitBizAction";
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
