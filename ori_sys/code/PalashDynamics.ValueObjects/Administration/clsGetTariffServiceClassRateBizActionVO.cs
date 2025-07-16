using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration
{
  public  class clsGetTariffServiceClassRateBizActionVO:IBizActionValueObject
    {
        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; }

        }

        private long _Specialization;
        public long Specialization
        {
            get { return _Specialization; }
            set { _Specialization = value; }



        }

        private long _SubSpecialization;
        public long SubSpecialization
        {
            get { return _SubSpecialization; }
            set { _SubSpecialization = value; }



        }

        public long TariffServiceID { get; set; }

        public List<clsServiceMasterVO> ServiceList { get; set; }

        public bool GetAllTariffServices { get; set; }
        //public bool GetTariffServiceMasterID { get; set; }
        //public bool GetAllTariffIDDetails { get; set; }
        //public bool GetAllServiceClassRateDetails { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetTariffServiceClassRateBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsServiceMasterVO ServiceMaster { get; set; }
    }

    // Added For IPD Module by CDS
  public class clsGetTariffServiceClassRateNewBizActionVO : IBizActionValueObject
  {

      #region IBizActionValueObject Members

      public string GetBizAction()
      {
          // throw new NotImplementedException();
          return "PalashDynamics.BusinessLayer.Administration.clsGetTariffServiceClassRateNewBizAction";
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
      private List<clsServiceTarrifClassRateDetailsNewVO> _ServiceList;
      /// <summary>
      /// Output Property.
      /// This Property Contains OPDPatient Details Which is Added.
      /// </summary>
      public List<clsServiceTarrifClassRateDetailsNewVO> ServiceList
      {
          get { return _ServiceList; }
          set { _ServiceList = value; }
      }
      private List<clsServiceTarrifClassRateDetailsNewVO> _ExistingClassRates;
      /// <summary>
      /// Output Property.
      /// This Property Contains OPDPatient Details Which is Added.
      /// </summary>
      public List<clsServiceTarrifClassRateDetailsNewVO> ExistingClassRates
      {
          get { return _ExistingClassRates; }
          set { _ExistingClassRates = value; }
      }
      #region INotifyPropertyChanged Members

      public event PropertyChangedEventHandler PropertyChanged;

      public void OnPropertyChanged(string PropertyName)
      {
          if (PropertyChanged != null)
              PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
      }

      #endregion

      private clsServiceMasterVO objServiceDetails = null;
      /// <summary>
      /// Output Property.
      /// This Property Contains OPDPatient Details Which is Added.
      /// </summary>
      public clsServiceMasterVO ServiceDetails
      {
          get { return objServiceDetails; }
          set { objServiceDetails = value; }
      }





      public int OperationType { get; set; }
      public bool? Modify { get; set; }
      public bool ViewPatient { get; set; }
      public bool FollowupPatientlist { get; set; }
      public string ServiceName { get; set; }
      public long ServiceID { get; set; }
      public long TSMID { get; set; }
      public string TariffName { get; set; }
      public long TariffID { get; set; }
      public string ClassName { get; set; }
      public long ClassID { get; set; }
      public long UnitId { get; set; }
      public int TotalRows { get; set; }
      public int StartIndex { get; set; }
      public int MaximumRows { get; set; }
      public bool IsPagingEnabled { get; set; }
      public string SortExpression { get; set; }
      public bool GetAllTariffServicesClass { get; set; }
      private decimal _Rate;
      public decimal Rate
      {
          get { return _Rate; }
          set
          {
              if (value != _Rate)
              {
                  _Rate = value;
                  OnPropertyChanged("Rate");
              }
          }
      }


      private bool _IsEnable = false;
      public bool IsEnable
      {
          get { return _IsEnable; }
          set
          {
              if (value != _IsEnable)
              {
                  _IsEnable = value;
                  OnPropertyChanged("IsEnable");
              }
          }
      }

      private bool _IsChkEnable = true;
      public bool IsChkEnable
      {
          get { return _IsChkEnable; }
          set
          {
              if (value != _IsChkEnable)
              {
                  _IsChkEnable = value;
                  OnPropertyChanged("IsChkEnable");
              }
          }
      }
      private bool _Status = false;
      public bool Status
      {
          get { return _Status; }
          set
          {
              if (value != _Status)
              {
                  _Status = value;
                  OnPropertyChanged("Status");
              }
          }
      }
  }

}
