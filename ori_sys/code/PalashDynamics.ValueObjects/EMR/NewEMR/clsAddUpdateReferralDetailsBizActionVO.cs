using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsAddUpdateReferralDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdateReferralDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


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

        private string _DoctCode;
        public string DoctCode
        {
            get { return _DoctCode; }
            set
            {
                if (_DoctCode != value)
                {
                    _DoctCode = value;
                    OnPropertyChanged("DoctCode");
                }
            }
        }


        private string _DeptmtCode;
        public string DeptmtCode
        {
            get { return _DeptmtCode; }
            set
            {
                if (_DeptmtCode != value)
                {
                    _DeptmtCode = value;
                    OnPropertyChanged("DeptmtCode");
                }
            }
        }

        private string _DeptmtName = string.Empty;
        public string DeptmtName
        {
            get { return _DeptmtName; }
            set
            {
                if (_DeptmtName != value)
                {
                    _DeptmtName = value;
                    OnPropertyChanged("DeptmtName");
                }
            }
        }

        private string _SpecializationCode;
        public string SpecializationCode
        {
            get { return _SpecializationCode; }
            set
            {
                if (_SpecializationCode != value)
                {
                    _SpecializationCode = value;
                    OnPropertyChanged("SpecializationCode");
                }
            }
        }

        private string _SpecializationName = string.Empty;
        public string SpecializationName
        {
            get { return _SpecializationName; }
            set
            {
                if (_SpecializationName != value)
                {
                    _SpecializationName = value;
                    OnPropertyChanged("SpecializationName");
                }
            }
        }
        public Boolean IsOPDIPD { get; set; }


        public long VisitID { get; set; }
        public long? ReferalDoctID { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long UnitID { get; set; }
        public long PrescriptionID { get; set; }

        public long DoctorID { get; set; }
        public long TemplateID { get; set; }

        private List<clsPatientPrescriptionDetailVO> objPres = null;

        public List<clsPatientPrescriptionDetailVO> CurrentPres
        {
            get { return objPres; }
            set{objPres = value ; }
        }

        private List<clsDoctorSuggestedServiceDetailVO> objDoctorSuggestedServiceDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsDoctorSuggestedServiceDetailVO> DoctorSuggestedServiceDetail
        {
            get { return objDoctorSuggestedServiceDetail; }
            set { objDoctorSuggestedServiceDetail = value; }
        }

    

    }

    public class clsGetDoctorlistforReferralAsperServiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDoctorlistforReferralAsperServiceBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        private int _SuccessStatus;

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long ID { get; set; }
        public string DoctorName { get; set; }
        public long DoctorTypeID { get; set; }
        public long UnitId { get; set; }
        public long DepartmentId { get; set; }
        public bool FromBill { get; set; }

        public bool AllRecord { get; set; }
        public long ServiceId { get; set; }

      
     


    }
        
     public class clsGetReferralDetailsBizActionVO : IBizActionValueObject
     {
         #region IBizActionValueObject Members

         public string GetBizAction()
         {
             return "PalashDynamics.BusinessLayer.clsGetReferralDetailsBizAction";
         }

         #endregion

         #region IValueObject Members

         public string ToXml()
         {
             return this.ToString();
         }

         #endregion

         private int _SuccessStatus;

         public int SuccessStatus
         {
             get { return _SuccessStatus; }
             set { _SuccessStatus = value; }
         }

         public long VisitID { get; set; }

         public long PatientID { get; set; }
         public Boolean IsOPDIPD { get; set; }
         public long PatientUnitID { get; set; }
         public long PrescriptionID { get; set; }
         public long DoctorID { get; set; }

         public bool Ishistory { get; set; }
         private int _TotalRows;
         public int TotalRows
         {
             get { return _TotalRows; }
             set { _TotalRows = value; }
         }
         private int _StartRowIndex = 0;

         public int StartRowIndex
         {
             get { return _StartRowIndex; }
             set { _StartRowIndex = value; }
         }
         private int _MaximumRows = 10;

         public int MaximumRows
         {
             get { return _MaximumRows; }
             set { _MaximumRows = value; }
         }

         private bool _PagingEnabled;

         public bool PagingEnabled
         {
             get { return _PagingEnabled; }
             set { _PagingEnabled = value; }
         }

         public long UnitID { get; set; }

         private List<clsDoctorSuggestedServiceDetailVO> objDoctorSuggestedServiceDetail = null;

         public List<clsDoctorSuggestedServiceDetailVO> DoctorSuggestedServiceDetail
         {
             get { return objDoctorSuggestedServiceDetail; }
             set { objDoctorSuggestedServiceDetail = value; }
         }


     }


     public class clsGetReferredDetailsBizActionVO : IBizActionValueObject
     {
         #region IBizActionValueObject Members

         public string GetBizAction()
         {
             return "PalashDynamics.BusinessLayer.clsGetReferredDetailsBizAction";
         }

         #endregion

         #region IValueObject Members

         public string ToXml()
         {
             return this.ToString();
         }

         #endregion

         private int _SuccessStatus;

         public int SuccessStatus
         {
             get { return _SuccessStatus; }
             set { _SuccessStatus = value; }
         }

         public long VisitID { get; set; }

         public long PatientID { get; set; }
         public Boolean IsOPDIPD { get; set; }
         public long PatientUnitID { get; set; }
         public long PrescriptionID { get; set; }
         public long DoctorID { get; set; }

         public bool Ishistory { get; set; }
         private int _TotalRows;
         public int TotalRows
         {
             get { return _TotalRows; }
             set { _TotalRows = value; }
         }
         private int _StartRowIndex = 0;

         public int StartRowIndex
         {
             get { return _StartRowIndex; }
             set { _StartRowIndex = value; }
         }
         private int _MaximumRows = 10;

         public int MaximumRows
         {
             get { return _MaximumRows; }
             set { _MaximumRows = value; }
         }

         private bool _PagingEnabled;

         public bool PagingEnabled
         {
             get { return _PagingEnabled; }
             set { _PagingEnabled = value; }
         }

         public long UnitID { get; set; }

         private List<clsDoctorSuggestedServiceDetailVO> objDoctorSuggestedServiceDetailforReferred = null;

         public List<clsDoctorSuggestedServiceDetailVO> DoctorSuggestedServiceDetailforReferred
         {
             get { return objDoctorSuggestedServiceDetailforReferred; }
             set { objDoctorSuggestedServiceDetailforReferred = value; }
         }
     }


     public class clsGetDoctorlistReferralServiceBizActionVO : IBizActionValueObject
     {
         #region IBizActionValueObject Members

         public string GetBizAction()
         {
             return "PalashDynamics.BusinessLayer.clsGetDoctorlistReferralServiceBizAction";
         }

         #endregion

         #region IValueObject Members

         public string ToXml()
         {
             return this.ToString();
         }

         #endregion

         private List<MasterListItem> _MasterList = null;
         public List<MasterListItem> MasterList
         {
             get
             { return _MasterList; }

             set
             { _MasterList = value; }
         }

         private int _SuccessStatus;

         public int SuccessStatus
         {
             get { return _SuccessStatus; }
             set { _SuccessStatus = value; }
         }

         public long ID { get; set; }
         public string DoctorName { get; set; }
         public long UnitId { get; set; }
         public long ServiceId { get; set; }





     }



}
