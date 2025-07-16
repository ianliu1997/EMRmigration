using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
  public  class clsGetDoctorDetailListForDoctorMasterBizActionVO:IBizActionValueObject
    {
      private List<clsDoctorVO> _DoctorDetails;
      public List<clsDoctorVO> DoctorDetails
      {
          get { return _DoctorDetails; }
          set { _DoctorDetails = value; }
      }

      public string FirstName { get; set;}
      public string MiddleName { get; set;}
      public string LastName { get; set;}
      public long UnitID { get; set; }

      public bool IsPagingEnabled { get; set; }

      public int StartRowIndex { get; set; }

      public int MaximumRows { get; set; }

      public string SearchExpression { get; set; }

      public int TotalRows { get; set; }
      public string SpecializationID { get; set; }
      public string SubSpecializationID { get; set; }
      public long DoctorTypeID { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDoctorDetailListForDoctorMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
  public class clsGetPendingDoctorDetails : IBizActionValueObject
  {
      private List<clsDoctorVO> _DoctorDetails;
      public List<clsDoctorVO> DoctorDetails
      {
          get { return _DoctorDetails; }
          set { _DoctorDetails = value; }
      }

      private List<MasterListItem> _MasterList = null;
      public List<MasterListItem> MasterList
      {
          get
          { return _MasterList; }

          set
          { _MasterList = value; }
      }


      public string FirstName { get; set; }
      public string MiddleName { get; set; }
      public string LastName { get; set; }
      public long UnitID { get; set; }
      public bool IsPagingEnabled { get; set; }

      public int StartRowIndex { get; set; }
      public string SpecializationID { get; set; }
      public string SubSpecializationID { get; set; }

      public int MaximumRows { get; set; }

      public string SearchExpression { get; set; }

      public int TotalRows { get; set; }

      #region IBizActionValueObject Members

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.clsGetPendingDoctorDetailsBizAction";
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
