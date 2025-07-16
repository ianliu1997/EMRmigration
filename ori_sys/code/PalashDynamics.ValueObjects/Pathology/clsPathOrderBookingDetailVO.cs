using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsPathOrderBookingDetailVO : IValueObject, INotifyPropertyChanged
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
        private long _UserID;
        public long UserID
        {
            get { return _UserID; }
            set
            {
                if (_UserID != value)
                {
                    _UserID = value;
                    OnPropertyChanged("UserID");
                }
            }
        }
        private long _AgeInDays;
        public long AgeInDays
        {
            get { return _AgeInDays; }
            set
            {
                if (_AgeInDays != value)
                {
                    _AgeInDays = value;
                    OnPropertyChanged("AgeInDays");
                }
            }
        }
       
        private DateTime? _TestDate;
        public DateTime? TestDate
        {
            get { return _TestDate; }
            set
            {
                if (_TestDate != value)
                {
                    _TestDate = value;
                    OnPropertyChanged("TestDate");
                }
            }
        }

        private long _OrderBookingID;
        public long OrderBookingID
        {
            get { return _OrderBookingID; }
            set
            {
                if (_OrderBookingID != value)
                {
                    _OrderBookingID = value;
                    OnPropertyChanged("OrderBookingID");
                }
            }
        }

        private bool _Status;
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

        private bool _Status1;
        public bool Status1
        {
            get { return _Status1; }
            set
            {
                if (_Status1 != value)
                {
                    _Status1 = value;
                    OnPropertyChanged("Status1");
                }
            }
        }
        private bool _IsOutSourced;
        public bool IsOutSourced
        {
            get { return _IsOutSourced; }
            set
            {
                if (_IsOutSourced != value)
                {
                    _IsOutSourced = value;
                    OnPropertyChanged("IsOutSourced");
                }
            }
        }
        private bool _IsOutSourcedD;
         public bool IsOutSourcedD
        {
            get { return _IsOutSourcedD; }
            set
            {
                if (_IsOutSourcedD != value)
                {
                    _IsOutSourcedD = value;
                    OnPropertyChanged("IsOutSourcedD");
                }
            }
        }
        
        private bool _IsSampleDispatched;
        public bool IsSampleDispatched
        {
            get { return _IsSampleDispatched; }
            set
            {
                if (_IsSampleDispatched != value)
                {
                    _IsSampleDispatched = value;
                    OnPropertyChanged("IsSampleDispatched");
                }
            }
        }

        private string _AgencyChangedImage;
        public string AgencyChangedImage
        {
            get { return _AgencyChangedImage; }
            set
            {
                if (_AgencyChangedImage != value)
                {
                    _AgencyChangedImage = value;
                    OnPropertyChanged("AgencyChangedImage");
                }
            }
        }

        private bool _IsSampleCollected;
        public bool IsSampleCollected
        {
            get { return _IsSampleCollected; }
            set
            {
                if (_IsSampleCollected != value)
                {
                    _IsSampleCollected = value;
                    OnPropertyChanged("IsSampleCollected");
                }
            }
        }

        private string _SampleCollecedImage;
        public string SampleCollecedImage
        {
            get { return _SampleCollecedImage; }
            set
            {
                if (_SampleCollecedImage != value)
                {
                    _SampleCollecedImage = value;
                    OnPropertyChanged("SampleCollecedImage");
                }
            }
        }
        private string _BatchCode;
        public string BatchCode
        {
            get { return _BatchCode; }
            set
            {
                if (_BatchCode != value)
                {
                    _BatchCode = value;
                    OnPropertyChanged("BatchCode");
                }
            }
        }
        private string _SampleNumber;
        public string SampleNumber
        {
            get { return _SampleNumber; }
            set
            {
                if (_SampleNumber != value)
                {
                    _SampleNumber = value;
                    OnPropertyChanged(" SampleNumber");
                }
            }
        }

        private string _SampleNumber1;
        public string SampleNumber1
        {
            get { return _SampleNumber1; }
            set
            {
                if (_SampleNumber1 != value)
                {
                    _SampleNumber1 = value;
                    OnPropertyChanged(" SampleNumber1");
                }
            }
        }
        private string _MRNo;
        public string MRNo
        {
            get { return _MRNo; }
            set
            {
                if (_MRNo != value)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
                }

            }
        }
        private string _Prefix;
        public string Prefix
        {
            get { return _Prefix; }
            set
            {
                if (_Prefix != value)
                {
                    _Prefix = value;
                    OnPropertyChanged("_Prefix");
                }
            }
        }
        private string _ClinicName;
        public string ClinicName
        {
            get { return _ClinicName; }
            set
            {
                if (_ClinicName != value)
                {
                    _ClinicName = value;
                    OnPropertyChanged("ClinicName");
                }
            }
        }

        private long _PathPatientReportID;
        public long PathPatientReportID
        {
            get { return _PathPatientReportID; }
            set
            {
                if (_PathPatientReportID != value)
                {
                    _PathPatientReportID = value;
                    OnPropertyChanged("PathPatientReportID");
                }
            }
        }



        private long _OrderID;
        public long OrderID
        {
            get { return _OrderID; }
            set
            {
                if (_OrderID != value)
                {
                    _OrderID = value;
                    OnPropertyChanged("OrderID");
                }
            }
        }



        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }


        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (_ServiceName != value)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }


        private bool _IsEmergency;
        public bool IsEmergency
        {
            get { return _IsEmergency; }
            set
            {
                if (_IsEmergency != value)
                {
                    _IsEmergency = value;
                    OnPropertyChanged("IsEmergency");
                }
            }
        }

        private bool _IsExternalPatient;
        public bool IsExternalPatient
        {
            get { return _IsExternalPatient; }
            set
            {
                if (_IsExternalPatient != value)
                {
                    _IsExternalPatient = value;
                    OnPropertyChanged("IsExternalPatient");
                }
            }
        }


        private long? _PathologistID;
        public long? PathologistID
        {
            get { return _PathologistID; }
            set
            {
                if (_PathologistID != value)
                {
                    _PathologistID = value;
                    OnPropertyChanged("PathologistID");
                }
            }
        }

        private string _Specimen;
        public string Specimen
        {
            get { return _Specimen; }
            set
            {
                if (_Specimen != value)
                {
                    _Specimen = value;
                    OnPropertyChanged("Specimen");
                }
            }
        }

        private string _ClinicalNote;
        public string ClinicalNote
        {
            get { return _ClinicalNote; }
            set
            {
                if (_ClinicalNote != value)
                {
                    _ClinicalNote = value;
                    OnPropertyChanged("ClinicalNote");
                }
            }
        }

        private string _SampleNo;
        public string SampleNo
        {
            get { return _SampleNo; }
            set
            {
                if (_SampleNo != value)
                {
                    _SampleNo = value;
                    OnPropertyChanged("SampleNo");
                }
            }
        }
        //by rohini to check if service is refunded
        private bool _IsServiceRefunded;
        public bool IsServiceRefunded
        {
            get { return _IsServiceRefunded; }
            set
            {
                if (_IsServiceRefunded != value)
                {
                    _IsServiceRefunded = value;
                    OnPropertyChanged("IsServiceRefunded");
                }
            }
        }

        private bool _IsSampleGenerated;
        public bool IsSampleGenerated
        {
            get { return _IsSampleGenerated; }
            set
            {
                if (_IsSampleGenerated != value)
                {
                    _IsSampleGenerated = value;
                    OnPropertyChanged("IsSampleGenerated");
                }
            }
        }
        
        private bool _FirstLevel;
        public bool FirstLevel
        {
            get { return _FirstLevel; }
            set
            {
                if (_FirstLevel != value)
                {
                    _FirstLevel = value;
                    OnPropertyChanged("FirstLevel");
                }
            }
        }

        private bool _SecondLevel;
        public bool SecondLevel
        {
            get { return _SecondLevel; }
            set
            {
                if (_SecondLevel != value)
                {
                    _SecondLevel = value;
                    OnPropertyChanged("SecondLevel");
                }
            }
        }

        private bool _ThirdLevel;
        public bool ThirdLevel
        {
            get { return _ThirdLevel; }
            set
            {
                if (_ThirdLevel != value)
                {
                    _ThirdLevel = value;
                    OnPropertyChanged("ThirdLevel");
                }
            }
        }

        private bool _IsCheckedResults;
        public bool IsCheckedResults
        {
            get { return _IsCheckedResults; }
            set
            {
                if (_IsCheckedResults != value)
                {
                    _IsCheckedResults = value;
                    OnPropertyChanged("IsCheckedResults");
                }
            }
        }


        private long? _FirstLevelID;
        public long? FirstLevelID
        {
            get { return _FirstLevelID; }
            set
            {
                if (_FirstLevelID != value)
                {
                    _FirstLevelID = value;
                    OnPropertyChanged("FirstLevelID");
                }
            }
        }

        private long? _SecondLevelID;
        public long? SecondLevelID
        {
            get { return _SecondLevelID; }
            set
            {
                if (_SecondLevelID != value)
                {
                    _SecondLevelID = value;
                    OnPropertyChanged("SecondLevelID");
                }
            }
        }

        private long? _ThirdLevelID;
        public long? ThirdLevelID
        {
            get { return _ThirdLevelID; }
            set
            {
                if (_ThirdLevelID != value)
                {
                    _ThirdLevelID = value;
                    OnPropertyChanged("ThirdLevelID");
                }
            }
        }

        private bool _IsApproved;
        public bool IsApproved
        {
            get { return _IsApproved; }
            set
            {
                if (_IsApproved != value)
                {
                    _IsApproved = value;
                    OnPropertyChanged("IsApproved");
                }
            }
        }

        private bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }
            set
            {
                if (_IsCompleted != value)
                {
                    _IsCompleted = value;
                    OnPropertyChanged("IsCompleted");
                }
            }
        }

        private bool _IsDelivered;
        public bool IsDelivered
        {
            get { return _IsDelivered; }
            set
            {
                if (_IsDelivered != value)
                {
                    _IsDelivered = value;
                    OnPropertyChanged("IsDelivered");
                }
            }
        }

        private bool _IsPrint;
        public bool IsPrint
        {
            get { return _IsPrint; }
            set
            {
                if (_IsPrint != value)
                {
                    _IsPrint = value;
                    OnPropertyChanged("IsPrint");
                }
            }
        }       

        private bool _IsClosedOrReported;
        public bool IsClosedOrReported
        {
            get { return _IsClosedOrReported; }
            set
            {
                if (_IsClosedOrReported != value)
                {
                    _IsClosedOrReported = value;
                    OnPropertyChanged("IsClosedOrReported");
                }
            }
        }
        private bool _IsPrinted;
        public bool IsPrinted
        {
            get { return _IsPrinted; }
            set
            {
                if (_IsPrinted != value)
                {
                    _IsPrinted = value;
                    OnPropertyChanged("IsPrinted");
                }
            }
        }

        private long? _MicrobiologistID;
        public long? MicrobiologistID
        {
            get { return _MicrobiologistID; }
            set
            {
                if (_MicrobiologistID != value)
                {
                    _MicrobiologistID = value;
                    OnPropertyChanged("MicrobiologistID");
                }
            }
        }

        private long? _Pathologist_1_ID;
        public long? Pathologist_1_ID
        {
            get { return _Pathologist_1_ID; }
            set
            {
                if (_Pathologist_1_ID != value)
                {
                    _Pathologist_1_ID = value;
                    OnPropertyChanged("Pathologist_1_ID");
                }
            }
        }

        private long? _Pathologist_2_ID;
        public long? Pathologist_2_ID
        {
            get { return _Pathologist_2_ID; }
            set
            {
                if (_Pathologist_2_ID != value)
                {
                    _Pathologist_2_ID = value;
                    OnPropertyChanged("Pathologist_2_ID");
                }
            }
        }

        private string _RefDoctor;
        public string RefDoctor
        {
            get { return _RefDoctor; }
            set
            {
                if (_RefDoctor != value)
                {
                    _RefDoctor = value;
                    OnPropertyChanged("RefDoctor");
                }
            }
        }

        private string _SampleCollectionNO;
        public string SampleCollectionNO
        {
            get { return _SampleCollectionNO; }
            set
            {
                if (_SampleCollectionNO != value)
                {
                    _SampleCollectionNO = value;
                    OnPropertyChanged("SampleCollectionNO");
                }
            }
        }

     
        private long? _AgencyID;
        public long? AgencyID
        {
            get { return _AgencyID; }
            set
            {
                if (_AgencyID != value)
                {
                    _AgencyID = value;
                    OnPropertyChanged("AgencyID");
                }
            }
        }
        private long? _StatusID;
        public long? StatusID
        {
            get { return _StatusID; }
            set
            {
                if (_StatusID != value)
                {
                    _StatusID = value;
                    OnPropertyChanged("StatusID");
                }
            }
        }

        private string _AgencyName;
        public string AgencyName
        {
            get { return _AgencyName; }
            set
            {
                if (_AgencyName != value)
                {
                    _AgencyName = value;
                    OnPropertyChanged("AgencyName");
                }
            }
        }

        private double? _Quantity;
        public double? Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

      

        private bool _IsBilled;
        public bool IsBilled
        {
            get { return _IsBilled; }
            set
            {
                if (_IsBilled != value)
                {
                    _IsBilled = value;
                    OnPropertyChanged("IsBilled");
                }
                IsChecked = !_IsBilled;
            }
        }


        private bool _IsSampleReceived;
        public bool IsSampleReceived
        {
            get { return _IsSampleReceived; }
            set
            {
                if (_IsSampleReceived != value)
                {
                    _IsSampleReceived = value;
                    OnPropertyChanged("IsSampleReceived");
                }
                IsChecked = !_IsSampleReceived;
            }
        }

        

      //---by rohini dated 4.2.16---
        private string _Gestation;
        public string Gestation
        {
            get { return _Gestation; }
            set
            {
                if (_Gestation != value)
                {
                    _Gestation = value;
                    OnPropertyChanged("Gestation");
                }
            }
        }
        private long _FastingStatusID;
        public long FastingStatusID
        {
            get { return _FastingStatusID; }
            set
            {
                if (_FastingStatusID != value)
                {
                    _FastingStatusID = value;
                    OnPropertyChanged("FastingStatusID");
                }
            }
        }
        private string _FastingStatusName;
        public string FastingStatusName
        {
            get { return _FastingStatusName; }
            set
            {
                if (_FastingStatusName != value)
                {
                    _FastingStatusName = value;
                    OnPropertyChanged("FastingStatusName");
                }               
            }
        }
        private string _CollectionName;
        public string CollectionName
        {
            get { return _CollectionName; }
            set
            {
                if (_CollectionName != value)
                {
                    _CollectionName = value;
                    OnPropertyChanged("CollectionName");
                }
            }
        }
        private int _FastingStatusHrs;
        public int FastingStatusHrs
        {
            get { return _FastingStatusHrs; }
            set
            {
                if (_FastingStatusHrs != value)
                {
                    _FastingStatusHrs = value;
                    OnPropertyChanged("FastingStatusHrs");
                }
            }
        }
        private string _CollectionCenter;
        public string CollectionCenter
        {
            get { return _CollectionCenter; }
            set
            {
                if (_CollectionCenter != value)
                {
                    _CollectionCenter = value;
                    OnPropertyChanged("CollectionCenter");
                }
            }
        }

        private string _ResultEntryBy;
        public string ResultEntryBy
        {
            get { return _ResultEntryBy; }
            set
            {
                if (_ResultEntryBy != value)
                {
                    _ResultEntryBy = value;
                    OnPropertyChanged("ResultEntryBy");
                }
            }
        }
        private DateTime? _ResultDateTime;
        public DateTime? ResultDateTime
        {
            get { return _ResultDateTime; }
            set
            {
                if (_ResultDateTime != value)
                {
                    _ResultDateTime = value;
                    OnPropertyChanged("ResultDateTime");
                }
            }
        }
        private string _ApproveBy;
        public string ApproveBy
        {
            get { return _ApproveBy; }
            set
            {
                if (_ApproveBy != value)
                {
                    _ApproveBy = value;
                    OnPropertyChanged("ApproveBy");
                }
            }
        }
        private DateTime? _ADateTime;
        public DateTime? ADateTime
        {
            get { return _ADateTime; }
            set
            {
                if (_ADateTime != value)
                {
                    _ADateTime = value;
                    OnPropertyChanged("ADateTime");
                }
            }
        }
        private long _CollectionID;
        public long CollectionID
        {
            get { return _CollectionID; }
            set
            {
                if (_CollectionID != value)
                {
                    _CollectionID = value;
                    OnPropertyChanged("CollectionID");
                }
            }
        }
        private string _SampleCollectedBy;
        public string SampleCollectedBy
        {
            get { return _SampleCollectedBy; }
            set
            {
                if (_SampleCollectedBy != value)
                {
                    _SampleCollectedBy = value;
                    OnPropertyChanged("SampleCollectedBy");
                }
            }
        }

        private long _DispatchToID;
        public long DispatchToID
        {
            get { return _DispatchToID; }
            set
            {
                if (_DispatchToID != value)
                {
                    _DispatchToID = value;
                    OnPropertyChanged("DispatchToID");
                }
            }
        }
        private string _DispatchToName;
        public string DispatchToName
        {
            get { return _DispatchToName; }
            set
            {
                if (_DispatchToName != value)
                {
                    _DispatchToName = value;
                    OnPropertyChanged("DispatchToName");
                }
            }
        }
        private string _DispatchBy;
        public string DispatchBy
        {
            get { return _DispatchBy; }
            set
            {
                if (_DispatchBy != value)
                {
                    _DispatchBy = value;
                    OnPropertyChanged("DispatchBy");
                }
            }
        }
        private string _SampleReceiveBy;
        public string SampleReceiveBy
        {
            get { return _SampleReceiveBy; }
            set
            {
                if (_SampleReceiveBy != value)
                {
                    _SampleReceiveBy = value;
                    OnPropertyChanged("SampleReceiveBy");
                }
            }
        }

        private long _AcceptedOrRejectedByID;
        public long AcceptedOrRejectedByID
        {
            get { return _AcceptedOrRejectedByID; }
            set
            {
                if (_AcceptedOrRejectedByID != value)
                {
                    _AcceptedOrRejectedByID = value;
                    OnPropertyChanged("AcceptedOrRejectedByID");
                }
            }
        }

        private string _AcceptedOrRejectedByName;
        public string AcceptedOrRejectedByName
        {
            get { return _AcceptedOrRejectedByName; }
            set
            {
                if (_AcceptedOrRejectedByName != value)
                {
                    _AcceptedOrRejectedByName = value;
                    OnPropertyChanged("AcceptedOrRejectedByName");
                }
            }
        }

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }
        private bool _IsAccepted;
        public bool IsAccepted
        {
            get { return _IsAccepted; }
            set
            {
                if (_IsAccepted != value)
                {
                    _IsAccepted = value;
                    OnPropertyChanged("IsAccepted");
                }
            }
        }
        private bool _IsRejected;
        public bool IsRejected
        {
            get { return _IsRejected; }
            set
            {
                if (_IsRejected != value)
                {
                    _IsRejected = value;
                    OnPropertyChanged("IsRejected");
                }
            }
        }
        private bool _IsSubOptimal;
        public bool IsSubOptimal
        {
            get { return _IsSubOptimal; }
            set
            {
                if (_IsSubOptimal != value)
                {
                    _IsSubOptimal = value;
                    OnPropertyChanged("IsSubOptimal");
                }
            }
        }
        private bool _IsResendForNewSample;
        public bool IsResendForNewSample
        {
            get { return _IsResendForNewSample; }
            set
            {
                if (_IsResendForNewSample != value)
                {
                    _IsResendForNewSample = value;
                    OnPropertyChanged("IsResendForNewSample");
                }
            }
        }

        private string _BillNo;
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                if (_BillNo != value)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }
        //---------------------//
        private bool _IsResultEntry;
        public bool IsResultEntry
        {
            get { return _IsResultEntry; }
            set
            {
                if (_IsResultEntry != value)
                {
                    _IsResultEntry = value;
                    OnPropertyChanged("IsResultEntry");
                }
            }
        }


        //required for search
        private bool _IsResultEntry1;
        public bool IsResultEntry1
        {
            get { return _IsResultEntry1; }
            set
            {
                if (_IsResultEntry1 != value)
                {
                    _IsResultEntry1 = value;
                    OnPropertyChanged("IsResultEntry1");
                }
            }
        }

        private bool _IsDelivered1;
        public bool IsDelivered1
        {
            get { return _IsDelivered1; }
            set
            {
                if (_IsDelivered1 != value)
                {
                    _IsDelivered1 = value;
                    OnPropertyChanged("IsDelivered1");
                }
            }
        }

        //
        private bool _IsFinalized;
        public bool IsFinalized
        {
            get { return _IsFinalized; }
            set
            {
                if (_IsFinalized != value)
                {
                    _IsFinalized = value;
                    OnPropertyChanged("IsFinalized");
                }
            }
        }

        //IsChecked Only Used for internal purpose . It is Used as invert value of IsSampleCollected.
        private bool _IsChecked;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }
        private DateTime? _SampleCollected;
        public DateTime? SampleCollected
        {
            get { return _SampleCollected; }
            set
            {
                if (_SampleCollected != value)
                {
                    _SampleCollected = value;
                    OnPropertyChanged("SampleCollected");
                }
            }
        }

        private long? _ItemConsID;
        public long? ItemConsID
        {
            get { return _ItemConsID; }
            set
            {
                if (_ItemConsID != value)
                {
                    _ItemConsID = value;
                    OnPropertyChanged("ItemConsID");
                }
            }
        }




        private string _SourceURL;
        public string SourceURL
        {
            get { return _SourceURL; }
            set
            {
                if (_SourceURL != value)
                {
                    _SourceURL = value;
                    OnPropertyChanged("SourceURL");
                }
            }
        }

        private byte[] _Report;
        public byte[] Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }

        private string _Category = "";
        public string Category
        {
            get { return _Category; }
            set
            {
                if (_Category != value)
                {
                    _Category = value;
                    OnPropertyChanged("Category");
                }
            }
        }

        private string _TestCode;
        public string TestCode
        {
            get { return _TestCode; }
            set
            {
                if (_TestCode != value)
                {
                    _TestCode = value;
                    OnPropertyChanged("TestCode");
                }
            }
        }

        private string _TestName = "";
        public string TestName
        {
            get { return _TestName; }
            set
            {
                if (_TestName != value)
                {
                    _TestName = value;
                    OnPropertyChanged("TestName");
                }
            }
        }

        private string _ShortDescription = "";
        public string ShortDescription
        {
            get { return _ShortDescription; }
            set
            {
                if (_ShortDescription != value)
                {
                    _ShortDescription = value;
                    OnPropertyChanged("ShortDescription");
                }
            }
        }

        private string _LongDescription = "";
        public string LongDescription
        {
            get { return _LongDescription; }
            set
            {
                if (_LongDescription != value)
                {
                    _LongDescription = value;
                    OnPropertyChanged("LongDescription");
                }
            }
        }

        private string _TestOrProfile = "";
        public string TestOrProfile
        {
            get { return _TestOrProfile; }
            set
            {
                if (_TestOrProfile != value)
                {
                    _TestOrProfile = value;
                    OnPropertyChanged("TestOrProfile");
                }
            }
        }


        private DateTime? _OrderDate;
        public DateTime? OrderDate
        {
            get { return _OrderDate; }
            set
            {
                if (_OrderDate != value)
                {
                    _OrderDate = value;
                    OnPropertyChanged("OrderDate");
                }
            }
        }

        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }


        private double _TestCharges;
        public double TestCharges
        {
            get { return _TestCharges; }
            set
            {
                if (_TestCharges != value)
                {
                    _TestCharges = value;
                    OnPropertyChanged("TestCharges");
                }
            }
        }




        private long _ChargeID;
        public long ChargeID
        {
            get { return _ChargeID; }
            set
            {
                if (_ChargeID != value)
                {
                    _ChargeID = value;
                    OnPropertyChanged("ChargeID");
                }
            }
        }



        private long _TariffServiceID;
        public long TariffServiceID
        {
            get { return _TariffServiceID; }
            set
            {
                if (_TariffServiceID != value)
                {
                    _TariffServiceID = value;
                    OnPropertyChanged("TariffServiceID");
                }
            }
        }


        # region Pathology Additions

        private bool _ReportTemplate;
        public bool ReportTemplate
        {
            get { return _ReportTemplate; }
            set
            {
                if (_ReportTemplate != value)
                {
                    _ReportTemplate = value;
                    OnPropertyChanged("ReportTemplate");
                }
            }
        }

        private DateTime? _HandDeliverdDateTime;
        public DateTime? HandDeliverdDateTime
        {
            get { return _HandDeliverdDateTime; }
            set
            {
                if (_HandDeliverdDateTime != value)
                {
                    _HandDeliverdDateTime = value;
                    OnPropertyChanged("HandDeliverdDateTime");
                }
            }
        }

        private bool _IsDirectDelivered;
        public bool IsDirectDelivered
        {
            get { return _IsDirectDelivered; }
            set
            {
                if (_IsDirectDelivered != value)
                {
                    _IsDirectDelivered = value;
                    OnPropertyChanged("IsDirectDelivered");
                }
            }
        }

        private DateTime? _EmailDeliverdDateTime;
        public DateTime? EmailDeliverdDateTime
        {
            get { return _EmailDeliverdDateTime; }
            set
            {
                if (_EmailDeliverdDateTime != value)
                {
                    _EmailDeliverdDateTime = value;
                    OnPropertyChanged("EmailDeliverdDateTime");
                }
            }
        }



        //adeede by rohnini for history details
        private DateTime? _DateTimeNow;
        public DateTime? DateTimeNow
        {
            get { return _DateTimeNow; }
            set
            {
                if (_DateTimeNow != value)
                {
                    _DateTimeNow = value;
                    OnPropertyChanged("DateTimeNow");
                }
            }
        }


        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        private string _Reason;
        public string Reason
        {
            get { return _Reason; }
            set
            {
                if (_Reason != value)
                {
                    _Reason = value;
                    OnPropertyChanged("Reason");
                }
            }
        }
        //
        // Added by Rajshree 
        private bool _IsDeliverdthroughEmail;
        public bool IsDeliverdthroughEmail
        {
            get
            {
                return _IsDeliverdthroughEmail;
            }
            set
            {
                if (_IsDeliverdthroughEmail != value)
                {
                    _IsDeliverdthroughEmail = value;
                    OnPropertyChanged("IsDeliverdthroughEmail");
                }
            }
        }

        private DateTime? _SampleCollectedDateTime;
        public DateTime? SampleCollectedDateTime
        {
            get { return _SampleCollectedDateTime; }
            set
            {
                if (_SampleCollectedDateTime != value)
                {
                    _SampleCollectedDateTime = value;
                    OnPropertyChanged("SampleCollectedDateTime");
                }
            }
        }

        private DateTime? _SampleCollectedTime;
        public DateTime? SampleCollectedTime
        {
            get { return _SampleCollectedTime; }
            set
            {
                if (_SampleCollectedTime != value)
                {
                    _SampleCollectedTime = value;
                    OnPropertyChanged("SampleCollectedTime");
                }
            }
        }
        private DateTime? _SampleReceivedDateTime;
        public DateTime? SampleReceivedDateTime
        {
            get { return _SampleReceivedDateTime; }
            set
            {
                if (_SampleReceivedDateTime != value)
                {
                    _SampleReceivedDateTime = value;
                    OnPropertyChanged("SampleReceivedDateTime");
                }
            }
        }
        private DateTime? _SampleReceivedTime;
        public DateTime? SampleReceivedTime
        {
            get { return _SampleReceivedTime; }
            set
            {
                if (_SampleReceivedTime != value)
                {
                    _SampleReceivedTime = value;
                    OnPropertyChanged("SampleReceivedTime");
                }
            }
        }
        private string _SampleCollectionCenter;
        public string SampleCollectionCenter
        {
            get { return _SampleCollectionCenter; }
            set
            {
                if (_SampleCollectionCenter != value)
                {
                    _SampleCollectionCenter = value;
                    OnPropertyChanged("SampleCollectionCenter");
                }
            }
        }

      
        
        public bool SelectedReport { get; set; }

        #endregion

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


        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
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

        private string _AddedOn = "";
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

        private string _AddedWindowsLoginName = "";
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

        private string _UpdatedOn = "";
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

        private string _UpdatedWindowsLoginName = "";
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

        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }



        #endregion

        //  New Properties For Merging Milan Date -----08/09/2015
        #region Newly Added Properties
        private string _SampleCollectedImage;
        public string SampleCollectedImage
        {
            get { return _SampleCollectedImage; }
            set
            {
                if (_SampleCollectedImage != value)
                {
                    _SampleCollectedImage = value;
                    OnPropertyChanged("SampleCollectedImage");
                }
            }
        }

        private string _SampleDispatchImage;
        public string SampleDispatchImage
        {
            get { return _SampleDispatchImage; }
            set
            {
                if (_SampleDispatchImage != value)
                {
                    _SampleDispatchImage = value;
                    OnPropertyChanged("SampleDispatchImage");
                }
            }
        }

        //added by rohini dated 16.2.16

        private string _TubeName;
        public string TubeName
        {
            get { return _TubeName; }
            set
            {
                if (_TubeName != value)
                {
                    _TubeName = value;
                    OnPropertyChanged("TubeName");
                }
            }
        }
        private string _BatchNo;
        public string BatchNo
        {
            get { return _BatchNo; }
            set
            {
                if (_BatchNo != value)
                {
                    _BatchNo = value;
                    OnPropertyChanged("BatchNo");
                }
            }
        }


        //
        private string _SampleReceiveImage;
        public string SampleReceiveImage
        {
            get { return _SampleReceiveImage; }
            set
            {
                if (_SampleReceiveImage != value)
                {
                    _SampleReceiveImage = value;
                    OnPropertyChanged("SampleReceiveImage");
                }
            }
        }

        private string _SampleOutSourceImage;
        public string SampleOutSourceImage
        {
            get { return _SampleOutSourceImage; }
            set
            {
                if (_SampleOutSourceImage != value)
                {
                    _SampleOutSourceImage = value;
                    OnPropertyChanged("SampleOutSourceImage");
                }
            }
        }

        private long? _RefDoctorID;
        public long? RefDoctorID
        {
            get { return _RefDoctorID; }
            set
            {
                if (_RefDoctorID != value)
                {
                    _RefDoctorID = value;
                    OnPropertyChanged("RefDoctorID");
                }
            }
        }

        private string _ReportType;
        public string ReportType
        {
            get { return _ReportType; }
            set
            {
                if (_ReportType != value)
                {
                    _ReportType = value;
                    OnPropertyChanged("ReportType");
                }
            }
        }

        private Int16 _AppendWith;
        public Int16 AppendWith
        {
            get { return _AppendWith; }
            set
            {
                if (_AppendWith != value)
                {
                    _AppendWith = value;
                    OnPropertyChanged("AppendWith");
                }
            }
        }    


        private string _SamplAcceptImage;
        public string SamplAcceptImage
        {
            get { return _SamplAcceptImage; }
            set
            {
                if (_SamplAcceptImage != value)
                {
                    _SamplAcceptImage = value;
                    OnPropertyChanged("SamplAcceptImage");
                }
            }
        }
        private string _SamplRejectionImage;
        public string SamplRejectionImage
        {
            get { return _SamplRejectionImage; }
            set
            {
                if (_SamplRejectionImage != value)
                {
                    _SamplRejectionImage = value;
                    OnPropertyChanged("SamplRejectionImage");
                }
            }
        }

        private string _SamplResendImage;
        public string SamplResendImage
        {
            get { return _SamplResendImage; }
            set
            {
                if (_SamplResendImage != value)
                {
                    _SamplResendImage = value;
                    OnPropertyChanged("SamplResendImage");
                }
            }
        }

        private string _ResultEntryImage;
        public string ResultEntryImage
        {
            get { return _ResultEntryImage; }
            set
            {
                if (_ResultEntryImage != value)
                {
                    _ResultEntryImage = value;
                    OnPropertyChanged("ResultEntryImage");
                }
            }
        }

        private string _ReportUploadImage;
        public string ReportUploadImage
        {
            get { return _ReportUploadImage; }
            set
            {
                if (_ReportUploadImage != value)
                {
                    _ReportUploadImage = value;
                    OnPropertyChanged("ReportUploadImage");
                }
            }
        }

        private string _FinalizedImage;
        public string FinalizedImage
        {
            get { return _FinalizedImage; }
            set
            {
                if (_FinalizedImage != value)
                {
                    _FinalizedImage = value;
                    OnPropertyChanged("FinalizedImage");
                }
            }
        }

        private string _UploadImage;
        public string UploadImage
        {
            get { return _UploadImage; }
            set
            {
                if (_UploadImage != value)
                {
                    _UploadImage = value;
                    OnPropertyChanged("UploadImage");
                }
            }
        }

        private string _DeliveredImage;
        public string DeliveredImage
        {
            get { return _DeliveredImage; }
            set
            {
                if (_DeliveredImage != value)
                {
                    _DeliveredImage = value;
                    OnPropertyChanged("DeliveredImage");
                }
            }
        }

        private byte _SampleAcceptRejectStatus;
        public byte SampleAcceptRejectStatus
        {
            get { return _SampleAcceptRejectStatus; }
            set
            {
                if (_SampleAcceptRejectStatus != value)
                {
                    _SampleAcceptRejectStatus = value;
                    OnPropertyChanged("SampleAcceptRejectStatus");
                }
            }
        }

        private bool _IsSampleChecked = true;
        public bool IsSampleChecked
        {
            get { return _IsSampleChecked; }
            set
            {
                if (_IsSampleChecked != value)
                {
                    _IsSampleChecked = value;
                    OnPropertyChanged("IsSampleChecked");
                }
                IsChecked = _IsSampleChecked;
            }
        }

        private bool _IsSampleDispatchChecked = true;
        public bool IsSampleDispatchChecked
        {
            get { return _IsSampleDispatchChecked; }
            set
            {
                if (_IsSampleDispatchChecked != value)
                {
                    _IsSampleDispatchChecked = value;
                    OnPropertyChanged("IsSampleDispatchChecked");
                }
                IsChecked = _IsSampleDispatchChecked;
            }
        }

        private bool _IsSampleAcceptEnable = true;
        public bool IsSampleAcceptEnable
        {
            get { return _IsSampleAcceptEnable; }
            set
            {
                if (_IsSampleAcceptEnable != value)
                {
                    _IsSampleAcceptEnable = value;
                    OnPropertyChanged("IsSampleAcceptEnable");
                }
                IsChecked = _IsSampleDispatchChecked;
            }
        }

        private bool _IsSampleReceiveChecked = true;
        public bool IsSampleReceiveChecked
        {
            get { return _IsSampleReceiveChecked; }
            set
            {
                if (_IsSampleReceiveChecked != value)
                {
                    _IsSampleReceiveChecked = value;
                    OnPropertyChanged("IsSampleReceiveChecked");
                }
                IsChecked = _IsSampleReceiveChecked;
            }
        }

        private bool _IsDeliveredChecked;
        public bool IsDeliveredChecked
        {
            get { return _IsDeliveredChecked; }
            set
            {
                if (_IsDeliveredChecked != value)
                {
                    _IsDeliveredChecked = value;
                    OnPropertyChanged("IsDeliveredChecked");
                }
            }
        }

        private bool _IsSampleDispatch = false;
        public bool IsSampleDispatch
        {
            get { return _IsSampleDispatch; }
            set
            {
                if (_IsSampleDispatch != value)
                {
                    _IsSampleDispatch = value;
                    OnPropertyChanged("IsSampleDispatch");
                }
                IsChecked = !_IsSampleDispatch;
            }
        }

        private bool _IsSampleReceive = false;
        public bool IsSampleReceive
        {
            get { return _IsSampleReceive; }
            set
            {
                if (_IsSampleReceive != value)
                {
                    _IsSampleReceive = value;
                    OnPropertyChanged("IsSampleReceive");
                }
                IsChecked = !_IsSampleReceive;
            }
        }

        private bool _IsSampleAccepted = false;
        public bool IsSampleAccepted
        {
            get { return _IsSampleAccepted; }
            set
            {
                if (_IsSampleAccepted != value)
                {
                    _IsSampleAccepted = value;
                    OnPropertyChanged("IsSampleAccepted");
                }
                IsChecked = !_IsSampleAccepted;
            }
        }

        private bool _IsSelectedForReceipt = false;
        public bool IsSelectedForReceipt
        {
            get { return _IsSelectedForReceipt; }
            set
            {
                if (_IsSelectedForReceipt != value)
                {
                    _IsSelectedForReceipt = value;
                    OnPropertyChanged("IsSelectedForReceipt");
                }
            }
        }

        private long _RefundID;
        public long RefundID
        {
            get { return _RefundID; }
            set
            {
                if (_RefundID != value)
                {
                    _RefundID = value;
                    OnPropertyChanged("RefundID");
                }
            }
        }

        private bool _IsDefault;
        public bool IsDefault
        {
            get { return _IsDefault; }
            set
            {
                if (_IsDefault != value)
                {
                    _IsDefault = value;
                    OnPropertyChanged("IsDefault");
                }

            }
        }

        private bool _IsFromSampleColletion = false;
        public bool IsFromSampleColletion
        {
            get { return _IsFromSampleColletion; }
            set
            {
                if (_IsFromSampleColletion != value)
                {
                    _IsFromSampleColletion = value;
                    OnPropertyChanged("IsFromSampleColletion");
                }
            }
        }
        private bool _IsFromSampleNumber;
        public bool IsFromSampleNumber
        {
            get { return _IsFromSampleNumber; }
            set
            {
                if (_IsFromSampleNumber != value)
                {
                    _IsFromSampleNumber = value;
                    OnPropertyChanged("IsFromSampleNumber");
                }
            }
        }


        private long _MachineID;
        public long MachineID
        {
            get { return _MachineID; }
            set
            {
                if (_MachineID != value)
                {
                    _MachineID = value;
                    OnPropertyChanged("MachineID");
                }
            }
        }

        private long _MachineUnitID;
        public long MachineUnitID
        {
            get { return _MachineUnitID; }
            set
            {
                if (_MachineUnitID != value)
                {
                    _MachineUnitID = value;
                    OnPropertyChanged("MachineUnitID");
                }
            }
        }

        private string _MachineName;
        public string MachineName
        {
            get { return _MachineName; }
            set
            {
                if (_MachineName != value)
                {
                    _MachineName = value;
                }
            }
        
        }

        private DateTime? _SampleDispatchDateTime;
        public DateTime? SampleDispatchDateTime
        {
            get { return _SampleDispatchDateTime; }
            set
            {
                if (_SampleDispatchDateTime != value)
                {
                    _SampleDispatchDateTime = value;
                    OnPropertyChanged("SampleDispatchDateTime");
                }
            }
        }

        private DateTime? _SampleDispatchTime;
        public DateTime? SampleDispatchTime
        {
            get { return _SampleDispatchTime; }
            set
            {
                if (_SampleDispatchTime != value)
                {
                    _SampleDispatchTime = value;
                    OnPropertyChanged("SampleDispatchTime");
                }
            }
        }

        private DateTime? _SampleAcceptanceDateTime;
        public DateTime? SampleAcceptanceDateTime
        {
            get { return _SampleAcceptanceDateTime; }
            set
            {
                if (_SampleAcceptanceDateTime != value)
                {
                    _SampleAcceptanceDateTime = value;
                    OnPropertyChanged("SampleAcceptanceDateTime");
                }
            }
        }

        private DateTime? _SampleAcceptanceTime;
        public DateTime? SampleAcceptanceTime
        {
            get { return _SampleAcceptanceTime; }
            set
            {
                if (_SampleAcceptanceTime != value)
                {
                    _SampleAcceptanceTime = value;
                    OnPropertyChanged("SampleAcceptanceTime");
                }
            }
        }
        private DateTime? _SampleRejectionDateTime;
        public DateTime? SampleRejectionDateTime
        {
            get { return _SampleRejectionDateTime; }
            set
            {
                if (_SampleRejectionDateTime != value)
                {
                    _SampleRejectionDateTime = value;
                    OnPropertyChanged("SampleAcceptanceDateTime");
                }
            }
        }

        private DateTime? _SampleRejectionTime;
        public DateTime? SampleRejectionTime
        {
            get { return _SampleRejectionTime; }
            set
            {
                if (_SampleRejectionTime != value)
                {
                    _SampleRejectionTime = value;
                    OnPropertyChanged("SampleAcceptanceTime");
                }
            }
        }
        private string _SampleRejectionRemark = String.Empty;
        public string SampleRejectionRemark
        {
            get { return _SampleRejectionRemark; }
            set
            {
                if (_SampleRejectionRemark != value)
                {
                    _SampleRejectionRemark = value;
                    OnPropertyChanged("SampleRejectionRemark");
                }
            }
        }

        private bool _IsFromSampleDispatch = false;
        public bool IsFromSampleDispatch
        {
            get { return _IsFromSampleDispatch; }
            set
            {
                if (_IsFromSampleDispatch != value)
                {
                    _IsFromSampleDispatch = value;
                    OnPropertyChanged("IsFromSampleDispatch");
                }
            }
        }

        private bool _IsFromSampleReceive = false;
        public bool IsFromSampleReceive
        {
            get { return _IsFromSampleReceive; }
            set
            {
                if (_IsFromSampleReceive != value)
                {
                    _IsFromSampleReceive = value;
                    OnPropertyChanged("IsFromSampleReceive");
                }
            }
        }

        private bool _IsFromSampleAcceptReject = false;
        public bool IsFromSampleAcceptReject
        {
            get { return _IsFromSampleAcceptReject; }
            set
            {
                if (_IsFromSampleAcceptReject != value)
                {
                    _IsFromSampleAcceptReject = value;
                    OnPropertyChanged("IsFromSampleAcceptReject");
                }
            }
        }


       
        private long _CategoryID;
        public long CategoryID
        {
            get { return _CategoryID; }
            set
            {
                if (_CategoryID != value)
                {
                    _CategoryID = value;
                    OnPropertyChanged("CategoryID");
                }
            }
        }
        private long _ResultEntryUserID;
        public long ResultEntryUserID
        {
            get { return _ResultEntryUserID; }
            set
            {
                if (_ResultEntryUserID != value)
                {
                    _ResultEntryUserID = value;
                    OnPropertyChanged("ResultEntryUserID");
                }
            }
        }
        private long _AuthUserID;
        public long AuthUserID
        {
            get { return _AuthUserID; }
            set
            {
                if (_AuthUserID != value)
                {
                    _AuthUserID = value;
                    OnPropertyChanged("AuthUserID");
                }
            }
        }

        private long _CategoryTypeID;
        public long CategoryTypeID
        {
            get { return _CategoryTypeID; }
            set
            {
                if (_CategoryTypeID != value)
                {
                    _CategoryTypeID = value;
                    OnPropertyChanged("CategoryTypeID");
                }
            }
        }

        private bool _ThirdLevelCheckResult;
        public bool ThirdLevelCheckResult
        {
            get { return _ThirdLevelCheckResult; }
            set
            {
                if (_ThirdLevelCheckResult != value)
                {
                    _ThirdLevelCheckResult = value;
                    OnPropertyChanged("ThirdLevelCheckResult");
                }
            }
        }

        private long _TemplateResultID;
        public long TemplateResultID
        {
            get { return _TemplateResultID; }
            set
            {
                if (_TemplateResultID != value)
                {
                    _TemplateResultID = value;
                    OnPropertyChanged("TemplateResultID");
                }
            }
        }

        private string _MsgCheckResultValForSecLevel;
        public string MsgCheckResultValForSecLevel
        {
            get { return _MsgCheckResultValForSecLevel; }
            set
            {
                if (_MsgCheckResultValForSecLevel != value)
                {
                    _MsgCheckResultValForSecLevel = value;
                    OnPropertyChanged("MsgCheckResultValForSecLevel");
                }
            }
        }

        private string _CheckResultValueMessage;
        public string CheckResultValueMessage
        {
            get { return _CheckResultValueMessage; }
            set
            {
                if (_CheckResultValueMessage != value)
                {
                    _CheckResultValueMessage = value;
                    OnPropertyChanged("CheckResultValueMessage");
                }
            }
        }

        private bool _IsChangedAgency;
        [DefaultValue(false)]
        public bool IsChangedAgency
        {
            get { return _IsChangedAgency; }
            set
            {
                if (_IsChangedAgency != value)
                {
                    _IsChangedAgency = value;
                    OnPropertyChanged("_IsChangedAgency");
                }

            }
        }

        private bool _SendEmail;
        public bool SendEmail
        {
            get { return _SendEmail; }
            set
            {
                if (_SendEmail != value)
                {
                    _SendEmail = value;
                    OnPropertyChanged("SendEmail");
                }
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }

            }
        }

        private bool _SendEmailEnabled;
        public bool SendEmailEnabled
        {
            get { return _SendEmailEnabled; }
            set
            {
                if (_SendEmailEnabled != value)
                {
                    _SendEmailEnabled = value;
                    OnPropertyChanged("SendEmailEnabled");
                }
            }
        }

        private string _FirstName = "";
        //[Required(ErrorMessage = "First Name Required")]
        //[StringLength(50, ErrorMessage = "First Name must be in between 1 to 50 Characters")]
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _MiddleName = "";
        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                if (_MiddleName != value)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string strPatientName = "";
        public string PatientName
        {
            get { return strPatientName = _Prefix + " " + _FirstName + " " + _MiddleName + " " + _LastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }
        private string _LastName = "";
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private bool _IsTemplateBased;
        public bool IsTemplateBased
        {
            get { return _IsTemplateBased; }
            set
            {
                if (_IsTemplateBased != value)
                {
                    _IsTemplateBased = value;
                    OnPropertyChanged("IsTemplateBased");
                }
            }
        }

        // Added By Anumani

        private long _BillId;
        public long BillID
        {
            get { return _BillId; }
            set
            {
                if (_BillId != value)
                {
                    _BillId = value;
                    OnPropertyChanged("BillID");
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

        //
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


}
