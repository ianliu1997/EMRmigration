using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
  public  class clsAddTariffServiceBizActionVO:IBizActionValueObject
    {
      public clsAddTariffServiceBizActionVO()
        {

        }

        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }

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

        private long _TariffServiceID;
        public long TariffServiceID
        {
            get { return _TariffServiceID; }
            set { _TariffServiceID = value; }
        }

        private bool _TariffServiceForm;
        public bool TariffServiceForm
        {
            get { return _TariffServiceForm; }
            set { _TariffServiceForm = value; }
        }
      



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddTariffServiceBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        public bool UpdateTariffServiceMaster { get; set; }

        public List<long> TariffList { get; set; }

        public List<MasterListItem> Tariffs { get; set; }
     
        public string Query { get; set; }

        public List<clsServiceTarrifClassRateDetailsVO> ClassList { get; set; }
       
        #endregion
    }

 // Added for IPD Module by CDS

  public class clsAddTariffServiceClassRateBizActionVO : IBizActionValueObject
  {
      public clsAddTariffServiceClassRateBizActionVO()
      {

      }

      private clsServiceTarrifClassRateDetailsNewVO _serviceTariffClassRate = null;
      public clsServiceTarrifClassRateDetailsNewVO ServiceTariffClassRate
      {
          get { return _serviceTariffClassRate; }
          set { _serviceTariffClassRate = value; }

      }

      private clsServiceMasterVO _objServiceMasterDetails = null;
      public clsServiceMasterVO ServiceDetails
      {
          get { return _objServiceMasterDetails; }
          set { _objServiceMasterDetails = value; }

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

      private long _TariffID;
      public long TariffID
      {
          get { return _TariffID; }
          set { _TariffID = value; }
      }

      private long _TSMID;
      public long TSMID
      {
          get { return _TSMID; }
          set { _TSMID = value; }
      }

      #region IBizActionValueObject Members

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.Administration.clsAddTariffServiceClassRateBizAction";
      }

      #endregion

      #region IValueObject Members

      public string ToXml()
      {
          return this.ToString();
      }

      private bool _UpdateTariffServiceMaster;
      public bool UpdateTariffServiceMaster
      {
          get { return _UpdateTariffServiceMaster; }
          set { _UpdateTariffServiceMaster = value; }
      }

      private bool _DeleteTariffServiceMaster;
      public bool DeleteTariffServiceMaster
      {
          get { return _DeleteTariffServiceMaster; }
          set { _DeleteTariffServiceMaster = value; }
      }

      public List<long> TariffList { get; set; }

      public List<MasterListItem> Tariffs { get; set; }

      public List<clsServiceTarrifClassRateDetailsNewVO> ServiceTariffList { get; set; }
      public List<clsServiceTarrifClassRateDetailsNewVO> DeleteServiceTariffList { get; set; }
      public List<clsServiceTarrifClassRateDetailsNewVO> ModifyTariffList { get; set; }

      #endregion
  }


}
