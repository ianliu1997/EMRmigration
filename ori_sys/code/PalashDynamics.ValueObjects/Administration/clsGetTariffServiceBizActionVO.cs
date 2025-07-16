using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
   public class clsGetTariffServiceBizActionVO: IBizActionValueObject
    {
        private List<clsServiceMasterVO> _ServiceList;
        public List<clsServiceMasterVO> ServiceList
        {
            get { return _ServiceList; }
            set { _ServiceList = value; }
        }

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
        public clsServiceMasterVO ServiceMaster { get; set; }

        //public bool GetAllServiceListDetails { get; set; }
        //public bool GetTariffServiceMasterID { get; set; }
        //public bool GetAllTariffIDDetails { get; set; }
        //public bool GetAllServiceClassRateDetails { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetTariffServiceBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    //Added By Manisha
   public class clsGetTariffServiceListBizActionVO : IBizActionValueObject
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
       private int _TotalRows1;
       public int TotalRows1
       {
           get { return _TotalRows1; }
           set { _TotalRows1 = value; }
       }
       private int _StartRowIndex = 0;
       public int StartRowIndex
       {
           get { return _StartRowIndex; }
           set { _StartRowIndex = value; }
       }
       private int _MaximumRows1 = 10;
       public int MaximumRows1
       {
           get { return _MaximumRows1; }
           set { _MaximumRows1 = value; }
       }
       private bool _PagingEnabled;
       public bool PagingEnabled
       {
           get { return _PagingEnabled; }
           set { _PagingEnabled = value; }
       }

       //public long TariffID { get; set; } //Commented By CDS 
       public long? TariffID { get; set; }
       public long ServiceID { get; set; }
       public long ForFilterPackageID { get; set; }
       public short PatientSourceType { get; set; }
       public long PatientSourceTypeID { get; set; }
       public long PatientID { get; set; }
       public long PatientUnitID { get; set; }
       public int Age { get; set; }
       public bool PrescribedService { get; set; }
       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }
       public List<clsServiceMasterVO> ServiceList { get; set; }

       public bool GetAllTariffServices { get; set; }

        public  bool? GetSuggestedServices  { get; set; }
        public long? VisitID { get; set; }
        public long? UnitID  { get; set; }
        public long ClassID { get; set; }
        public Boolean IsOPDIPD { get; set; }
       //public bool GetTariffServiceMasterID { get; set; }
       //public bool GetAllTariffIDDetails { get; set; }
       //public bool GetAllServiceClassRateDetails { get; set; }

        public bool IsPackage { get; set; }
        public bool IsPrescribedService { get; set; }
        public bool UsePackageSubsql { get; set; }

        public long ChargeID { get; set; }
        public long SponsorID { get; set; }
        public long SponsorUnitID { get; set; }

        //public int TotalRows { get; set; }
        //public int StartIndex { get; set; }
        //public int MaximumRows { get; set; }
        //public bool IsPagingEnabled { get; set; }
        public string SearchExpression { get; set; }

       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.clsGetTariffServiceListBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion
   }

   public class clsGetTariffServiceListBizActionForPathologyVO : IBizActionValueObject
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

       //public long TariffID { get; set; } //Commented By CDS 
       public long? TariffID { get; set; }
       public long ServiceID { get; set; }
       public long ForFilterPackageID { get; set; }
       public short PatientSourceType { get; set; }
       public long PatientSourceTypeID { get; set; }
       public long PatientID { get; set; }
       public long PatientUnitID { get; set; }
       public int Age { get; set; }
       public bool PrescribedService { get; set; }
       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }
       public List<clsServiceMasterVO> ServiceList { get; set; }

       public bool GetAllTariffServices { get; set; }

       public bool? GetSuggestedServices { get; set; }
       public long? VisitID { get; set; }
       public long? UnitID { get; set; }
       public long ClassID { get; set; }
       //public bool GetTariffServiceMasterID { get; set; }
       //public bool GetAllTariffIDDetails { get; set; }
       //public bool GetAllServiceClassRateDetails { get; set; }

       public bool IsPackage { get; set; }
       public bool IsPrescribedService { get; set; }
       public bool UsePackageSubsql { get; set; }

       public long SponsorID { get; set; }
       public long SponsorUnitID { get; set; }

       //public int TotalRows { get; set; }
       //public int StartIndex { get; set; }
       //public int MaximumRows { get; set; }
       //public bool IsPagingEnabled { get; set; }
       public string SearchExpression { get; set; }

       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.clsGetTariffServiceListBizActionForPathologyBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion
   }

    // Added BY Changdeo sase 

   public class clsGetAdmissionTypeTariffServiceListBizActionVO : IBizActionValueObject
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

       public long AdmissionTypeID { get; set; }
       public long TariffID { get; set; }
       public long ServiceID { get; set; }
       public short PatientSourceType { get; set; }
       public long PatientSourceTypeID { get; set; }
       public long PatientID { get; set; }
       public long PatientUnitID { get; set; }
       public int Age { get; set; }
       public bool PrescribedService { get; set; }
       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }
       public List<clsServiceMasterVO> ServiceList { get; set; }

       public bool GetAllTariffServices { get; set; }

       public bool? GetSuggestedServices { get; set; }
       public long? VisitID { get; set; }
       public long? UnitID { get; set; }
       public long ClassID { get; set; }
       

       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.clsGetAdmissionTypeTariffServiceListBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion
   }

   // End Block 

    //Added By Pallavi
   public class clsGetTariffServiceMasterListBizActionVO : IBizActionValueObject
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

       public long TariffID { get; set; }
       public long ServiceID { get; set; }
       //public short PatientSourceType { get; set; }
       //public long PatientSourceTypeID { get; set; }
       //public long PatientID { get; set; }
       public List<clsServiceMasterVO> ServiceList { get; set; }

       public bool GetAllTariffServices { get; set; }
       //public bool GetTariffServiceMasterID { get; set; }
       //public bool GetAllTariffIDDetails { get; set; }
       //public bool GetAllServiceClassRateDetails { get; set; }
       
       
       
       
       //added by akshays
       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }
       //closed by akshays

       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.clsGetTariffServiceMasterListBizAction";
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
