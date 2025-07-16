using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.EMR
{
    // Patient Case Record
    public class clsPCRVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        private string _ComplaintReported = "";
        public string ComplaintReported
        {
            get { return _ComplaintReported; }
            set
            {
                if (_ComplaintReported != value)
                {
                    _ComplaintReported = value;
                    OnPropertyChanged("ComplaintReported");
                }
            }
        }

        private string _ChiefComplaints = "";
        public string ChiefComplaints
        {
            get { return _ChiefComplaints; }
            set
            {
                if (_ChiefComplaints != value)
                {
                    _ChiefComplaints = value;
                    OnPropertyChanged("ChiefComplaints");
                }
            }
        }

        private string _PastHistory = "";
        public string PastHistory
        {
            get { return _PastHistory; }
            set
            {
                if (_PastHistory != value)
                {
                    _PastHistory = value;
                    OnPropertyChanged("PastHistory");
                }
            }
        }

        private string _DrugHistory = "";
        public string DrugHistory
        {
            get { return _DrugHistory; }
            set
            {
                if (_DrugHistory != value)
                {
                    _DrugHistory = value;
                    OnPropertyChanged("DrugHistory");
                }
            }
        }

        private string _Allergies = "";
        public string Allergies
        {
            get { return _Allergies; }
            set
            {
                if (_Allergies != value)
                {
                    _Allergies = value;
                    OnPropertyChanged("Allergies");
                }
            }
        }

        private string _Investigations = "";
        public string Investigations
        {
            get { return _Investigations; }
            set
            {
                if (_Investigations != value)
                {
                    _Investigations = value;
                    OnPropertyChanged("Investigations");
                }
            }
        }

        private string _PovisionalDiagnosis = "";
        public string PovisionalDiagnosis
        {
            get { return _PovisionalDiagnosis; }
            set
            {
                if (_PovisionalDiagnosis != value)
                {
                    _PovisionalDiagnosis = value;
                    OnPropertyChanged("PovisionalDiagnosis");
                }
            }
        }

        private string _FinalDiagnosis = "";
        public string FinalDiagnosis
        {
            get { return _FinalDiagnosis; }
            set
            {
                if (_FinalDiagnosis != value)
                {
                    _FinalDiagnosis = value;
                    OnPropertyChanged("FinalDiagnosis");
                }
            }
        }

        private string _Hydration = "";
        public string Hydration
        {
            get { return _Hydration; }
            set
            {
                if (_Hydration != value)
                {
                    _Hydration = value;
                    OnPropertyChanged("Hydration");
                }
            }
        }

        private string _IVHydration4 = "";
        public string IVHydration4
        {
            get { return _IVHydration4; }
            set
            {
                if (_IVHydration4 != value)
                {
                    _IVHydration4 = value;
                    OnPropertyChanged("IVHydration4");
                }
            }
        }

        private string _ZincSupplement = "";
        public string ZincSupplement
        {
            get { return _ZincSupplement; }
            set
            {
                if (_ZincSupplement != value)
                {
                    _ZincSupplement = value;
                    OnPropertyChanged("ZincSupplement");
                }
            }
        }

        private string _Nutritions = "";
        public string Nutritions
        {
            get { return _Nutritions; }
            set
            {
                if (_Nutritions != value)
                {
                    _Nutritions = value;
                    OnPropertyChanged("Nutritions");
                }
            }
        }

        private string _AdvisoryAttached = "";
        public string AdvisoryAttached
        {
            get { return _AdvisoryAttached; }
            set
            {
                if (_AdvisoryAttached != value)
                {
                    _AdvisoryAttached = value;
                    OnPropertyChanged("AdvisoryAttached");
                }
            }
        }

        private string _WhenToVisitHospital = "";
        public string WhenToVisitHospital
        {
            get { return _WhenToVisitHospital; }
            set
            {
                if (_WhenToVisitHospital != value)
                {
                    _WhenToVisitHospital = value;
                    OnPropertyChanged("WhenToVisitHospital");
                }
            }
        }

        private string _SpecificInstructions = "";
        public string SpecificInstructions
        {
            get { return _SpecificInstructions; }
            set
            {
                if (_SpecificInstructions != value)
                {
                    _SpecificInstructions = value;
                    OnPropertyChanged("SpecificInstructions");
                }
            }
        }



        private DateTime? _FollowUpDate;
        public DateTime? FollowUpDate
        {
            get { return _FollowUpDate; }
            set
            {
                if (_FollowUpDate != value)
                {
                    _FollowUpDate = value;
                    OnPropertyChanged("FollowUpDate");
                }
            }
        }

        private string _FollowUpAt = "";
        public string FollowUpAt
        {
            get { return _FollowUpAt; }
            set
            {
                if (_FollowUpAt != value)
                {
                    _FollowUpAt = value;
                    OnPropertyChanged("FollowUpAt");
                }
            }
        }

        private string _ReferralTo = "";
        public string ReferralTo
        {
            get { return _ReferralTo; }
            set
            {
                if (_ReferralTo != value)
                {
                    _ReferralTo = value;
                    OnPropertyChanged("ReferralTo");
                }
            }
        }
        #endregion

        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn;
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn;
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName;
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        private bool _Synchronized;
        public bool Synchronized
        {
            get { return _Synchronized; }
            set
            {
                if (_Synchronized != value)
                {
                    _Synchronized = value;
                    OnPropertyChanged("Synchronized");
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsAddPCRBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPCRBizAction";
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

        private clsPCRVO objPCR = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPCRVO PCRDetails
        {
            get { return objPCR; }
            set { objPCR = value; }
        }
    }

    public class clsUpdatePCRBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsUpdatePCRBizAction";
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

        private clsPCRVO objPCR = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPCRVO PCRDetails
        {
            get { return objPCR; }
            set { objPCR = value; }
        }
    }


    // Case Referral
    public class clsCaseReferralVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        private long _ReferredToDoctorID;
        public long ReferredToDoctorID
        {
            get { return _ReferredToDoctorID; }
            set
            {
                if (_ReferredToDoctorID != value)
                {
                    _ReferredToDoctorID = value;
                    OnPropertyChanged("ReferredToDoctorID");
                }
            }
        }

        private long _ReferredToClinicID;
        public long ReferredToClinicID
        {
            get { return _ReferredToClinicID; }
            set
            {
                if (_ReferredToClinicID != value)
                {
                    _ReferredToClinicID = value;
                    OnPropertyChanged("ReferredToClinicID");
                }
            }
        }

        private string _ReferredToMobile = "";
        public string ReferredToMobile
        {
            get { return _ReferredToMobile; }
            set
            {
                if (_ReferredToMobile != value)
                {
                    _ReferredToMobile = value;
                    OnPropertyChanged("ReferredToMobile");
                }
            }
        }

        private string _ProvisionalDiagnosis = "";
        public string ProvisionalDiagnosis
        {
            get { return _ProvisionalDiagnosis; }
            set
            {
                if (_ProvisionalDiagnosis != value)
                {
                    _ProvisionalDiagnosis = value;
                    OnPropertyChanged("ProvisionalDiagnosis");
                }
            }
        }


        private string _ChiefComplaints = "";
        public string ChiefComplaints
        {
            get { return _ChiefComplaints; }
            set
            {
                if (_ChiefComplaints != value)
                {
                    _ChiefComplaints = value;
                    OnPropertyChanged("ChiefComplaints");
                }
            }
        }
        private string _Summary = "";
        public string Summary
        {
            get { return _Summary; }
            set
            {
                if (_Summary != value)
                {
                    _Summary = value;
                    OnPropertyChanged("Summary");
                }
            }
        }

        private string _Observations = "";
        public string Observations
        {
            get { return _Observations; }
            set
            {
                if (_Observations != value)
                {
                    _Observations = value;
                    OnPropertyChanged("Observations");
                }
            }
        }


        #endregion

        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn;
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn;
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName;
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        private bool _Synchronized;
        public bool Synchronized
        {
            get { return _Synchronized; }
            set
            {
                if (_Synchronized != value)
                {
                    _Synchronized = value;
                    OnPropertyChanged("Synchronized");
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsAddCaseReferralBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddCaseReferralBizAction";
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

        private clsCaseReferralVO objCaseReferral = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsCaseReferralVO CaseReferralDetails
        {
            get { return objCaseReferral; }
            set { objCaseReferral = value; }
        }
    }

    public class clsUpdateCaseReferralBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsUpdateCaseReferralBizAction";
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

        private clsCaseReferralVO objCaseReferral = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsCaseReferralVO CaseReferralDetails
        {
            get { return objCaseReferral; }
            set { objCaseReferral = value; }
        }
    }
}
