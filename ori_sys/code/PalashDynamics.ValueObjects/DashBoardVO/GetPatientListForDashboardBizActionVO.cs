using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
  public class GetPatientListForDashboardBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.GetPatientListForDashboardBizAction";
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
        public string SearchKeyword { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? VisitFromDate { get; set; }
        public DateTime? VisitToDate { get; set; }

        public DateTime? AdmissionFromDate { get; set; }
        public DateTime? AdmissionToDate { get; set; }

        public DateTime? DOBFromDate { get; set; }
        public DateTime? DOBToDate { get; set; }

        public bool RegistrationWise { get; set; }
        public bool VisitWise { get; set; }
        public bool AdmissionWise { get; set; }

        public bool DOBWise { get; set; }

        public bool IsLoyaltyMember { get; set; }
        public long LoyaltyProgramID { get; set; }
        public long PatientCategoryID { get; set; }
        public bool IsDonorLink { get; set; } //***//

        ////public bool IncludeSpouse { get; set; }

        public string MRNo { get; set; }
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }
        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FamilyName { get; set; }

        public string LinkServer { get; set; }

        public string OPDNo { get; set; }

        public string ContactNo { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Pincode { get; set; }

        public long GenderID { get; set; }

        public string CivilID { get; set; }

        public long UnitID { get; set; }

        public bool SearchInAnotherClinic { get; set; }

        public long RegistrationTypeID { get; set; }

        public long EMRProcedureID { get; set; }

        public long EMRProcedureUnitID { get; set; }

        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientGeneralVO> PatientDetailsList
        {
            get;
            set;
        }
        public bool IsSurrogacy { get; set; }
        public bool IsSurrogacyForTransfer { get; set; }
    }

  public class GetSearchkeywordForPatientBizActionVO : IBizActionValueObject
  {
      #region IBizActionValueObject Members

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.GetSearchkeywordForPatientBizAction";
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
      public string SearchKeyword { get; set; }
      public int TotalRows { get; set; }
      public int StartIndex { get; set; }
      public int MaximumRows { get; set; }
      public bool IsPagingEnabled { get; set; }
      public DateTime? FromDate { get; set; }
      public DateTime? ToDate { get; set; }
      public DateTime? VisitFromDate { get; set; }
      public DateTime? VisitToDate { get; set; }

      public DateTime? AdmissionFromDate { get; set; }
      public DateTime? AdmissionToDate { get; set; }

      public DateTime? DOBFromDate { get; set; }
      public DateTime? DOBToDate { get; set; }

      public bool RegistrationWise { get; set; }
      public bool VisitWise { get; set; }
      public bool AdmissionWise { get; set; }

      public bool DOBWise { get; set; }

      public bool IsLoyaltyMember { get; set; }
      public long LoyaltyProgramID { get; set; }
      public long PatientCategoryID { get; set; }

      //public bool IncludeSpouse { get; set; }

      public string MRNo { get; set; }
      private List<MasterListItem> _MasterList = null;
      public List<MasterListItem> MasterList
      {
          get
          { return _MasterList; }

          set
          { _MasterList = value; }
      }
      private string _FirstName;
      public string FirstName
      {
          get
          {
              return _FirstName;
          }
          set
          {
              _FirstName = value;
          }
      }

      public string MiddleName { get; set; }

      public string LastName { get; set; }

      public string FamilyName { get; set; }

      public string LinkServer { get; set; }

      public string OPDNo { get; set; }

      public string ContactNo { get; set; }

      public string Country { get; set; }

      public string State { get; set; }

      public string City { get; set; }

      public string Pincode { get; set; }

      public long GenderID { get; set; }

      public string CivilID { get; set; }

      public long UnitID { get; set; }

      public bool SearchInAnotherClinic { get; set; }

      public long RegistrationTypeID { get; set; }

      /// <summary>
      /// Output Property.
      /// This Property Contains OPDPatient Details Which is Added.
      /// </summary>
      public List<clsPatientGeneralVO> PatientDetailsList
      {
          get;
          set;
      }

  }

    //added by neena
  //public class GetSurrogatePatientListForDashboardBizActionVO : IBizActionValueObject
  //{
  //    #region IBizActionValueObject Members

  //    public string GetBizAction()
  //    {
  //        return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.GetSurrogatePatientListForDashboardBizAction";
  //    }

  //    #endregion

  //    #region IValueObject Members

  //    public string ToXml()
  //    {
  //        return this.ToString();
  //    }

  //    #endregion

  //    private int _SuccessStatus;
  //    /// <summary>
  //    /// Output Property.
  //    /// This property states the outcome of BizAction Process.
  //    /// </summary>
  //    public int SuccessStatus
  //    {
  //        get { return _SuccessStatus; }
  //        set { _SuccessStatus = value; }
  //    }
     
   
  //    public List<clsPatientGeneralVO> PatientDetailsList
  //    {
  //        get;
  //        set;
  //    }

  //}
    //
}
