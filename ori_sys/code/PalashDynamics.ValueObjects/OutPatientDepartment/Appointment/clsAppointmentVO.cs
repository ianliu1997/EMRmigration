using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
    public class clsAppointmentVO : INotifyPropertyChanged, IValueObject
    {
       public clsAppointmentVO()
       {

           this.FromTime = DateTime.Now;
           this.ToTime = DateTime.Now;
       }


       private long lngAppointmentID;
       public long AppointmentID
       {
           get
           { 
               return lngAppointmentID;
           }
           set
           {
               lngAppointmentID = value;
           }
       }

       private DateTime? dtpAppDate;
       public DateTime? AppointmentDate
       {
           get { return dtpAppDate; }
           set
           {
               if (value != dtpAppDate)
               {
                   dtpAppDate = value;
                   OnPropertyChanged("AppointmentDate");
               }
           }
       }
       
       

       private DateTime? dtpFromTime;
       public DateTime? FromTime
       {
           get { return dtpFromTime; }
           set
           {
               if (value != dtpFromTime)
               {
                   dtpFromTime = value;
                   OnPropertyChanged("FromTime");
               }
           }
       }

       private DateTime? dtpToTime;
       public DateTime? ToTime
       {
           get { return dtpToTime; }
           set
           {
               if (value != dtpToTime)
               {
                   dtpToTime = value;
                   OnPropertyChanged("ToTime");
               }
           }
       }
       private DateTime? _DateOfBirthFromAge = null;
       public DateTime? DateOfBirthFromAge
       {
           get
           {
               return _DateOfBirthFromAge;
           }
           set
           {
               if (value != _DateOfBirthFromAge)
               {

                   _DateOfBirthFromAge = value;
                   OnPropertyChanged("DateOfBirthFromAge");
               }
           }
       }
        //[Required(ErrorMessage = "Birth date is Required")]
       private DateTime? dtpDOB ;
       public DateTime? DOB
       {
           get { return dtpDOB; }
           set
           {
               if (value != dtpDOB)
               {
                   dtpDOB = value;
                   
                   //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "DOB" });
                   OnPropertyChanged("DOB");
               }
           }
       }

       public bool IsAge { get; set; }

       private bool _ResultStatus;
       public bool ResultStatus
       {
           get { return _ResultStatus; }
           set
           {
               if (value != _ResultStatus)
               {

                   _ResultStatus = value;
                   OnPropertyChanged("ResultStatus");


               }
           }

       }

       private string _RegRemark = " ";
       public string RegRemark
       {
           get { return _RegRemark; }
           set
           {
               if (value != _RegRemark)
               {

                   _RegRemark = value;
                   OnPropertyChanged("RegRemark");


               }

           }
       }

       private string strFirstName ="";
       public string FirstName
       {
           get { return strFirstName; }
           set
           {
               if (value != strFirstName)
               {
                   
                   strFirstName = value;
                   OnPropertyChanged("FirstName");
                   
                   
               }

           }
       }

       private string strMiddleName="";

       public string MiddleName
       {
           get { return strMiddleName; }
           set
           {
               if (value != strMiddleName)
               {
                     strMiddleName = value;
                   OnPropertyChanged("MiddleName");
               }
           }
       }

       public int AppointmentStatus { get; set; }
       private string strLastName="";
       //[Required(ErrorMessage ="Last Name is Required")]
       public string LastName
       {
           get { return strLastName; }
           set
           {
               if (value != strLastName)
               {
                   //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "LastName" });
                   strLastName = value;
                  OnPropertyChanged("LastName");
               }
           }
       }

       private string strFamilyName ="";
       public string FamilyName
       {
           get { return strFamilyName; }
           set
           {
               if (value != strFamilyName)
               {
                   
                   strFamilyName = value;
                   OnPropertyChanged("FamilyName");
               }
           }
       }
       public long ParentAppointmentID { get; set; }
       public long ParentAppointmentUnitID { get; set; }
       private string _MobileCountryCode;
       public string MobileCountryCode
       {
           get { return _MobileCountryCode; }
           set
           {
               if (_MobileCountryCode != value)
               {
                   _MobileCountryCode = value;
                   OnPropertyChanged("MobileCountryCode");
               }
           }
       }

       private long _ID;
       public long ID
       {
           get
           {
               return _ID;
           }
           set
           {
               _ID = value;
           }
       }

       private bool _IsReschedule;
       public bool IsReschedule
       {
           get
           {
               return _IsReschedule;
           }
           set
           {
               _IsReschedule = value;
           }

       }

       private string _PatientName2 = String.Empty;
       public string PatientName2
       {
           get { return _PatientName2; }
           set
           {
               if (value != _PatientName2)
               {
                   _PatientName2 = value;
                   OnPropertyChanged("PatientName2");
               }
           }
       }

       private long _ResiNoCountryCode;
       public long ResiNoCountryCode
       {
           get { return _ResiNoCountryCode; }
           set
           {
               if (_ResiNoCountryCode != value)
               {
                   _ResiNoCountryCode = value;
                   OnPropertyChanged("ResiNoCountryCode");
               }
           }
       }

        private long _ResiSTDCode;
        public long ResiSTDCode
        {
            get { return _ResiSTDCode; }
            set
            {
                if (_ResiSTDCode != value)
                {
                    _ResiSTDCode = value;
                    OnPropertyChanged("ResiSTDCode");
                }
            }
        }
        

       private string strContactNo1 ="";
       public string ContactNo1
       {
           get { return strContactNo1; }
           set 
           {
               if (value != strContactNo1)
               {
                   strContactNo1 = value;
                   OnPropertyChanged("ContactNo1");
               }
           }
       }

       private string strContactNo2 ="";
       public string ContactNo2
       {
           get { return strContactNo2; }
           set
           {
               if (value != strContactNo2)
               {
                   strContactNo2 = value;
                   OnPropertyChanged("ContactNo2");
               }
           }
       }

       private string strFaxNo ="";
       public string FaxNo
       {
           get { return strFaxNo; }
           set
           {
               if (value != strFaxNo)
               {
                   strFaxNo = value;
                   OnPropertyChanged("FaxNo");
               }
           }
       }

       private string strEmail ="";
       public string Email
       {
           get { return strEmail; }
           set
           {
               if (value != strEmail)
               {
                   strEmail = value;
                   OnPropertyChanged("Email");
               }
           }
       }

       private bool _IsDocAttached;
       public bool IsDocAttached
       {
           get { return _IsDocAttached; }
           set
           {
               if (value != _IsDocAttached)
               {
                   _IsDocAttached = value;
                   OnPropertyChanged("IsDocAttached");
               }
               }

       }

    

       
       private long lngGenderId;
       public long GenderId
       {
           get { return lngGenderId; }
           set 
           {
               if(value !=lngGenderId)
               {
                   lngGenderId = value;
                   OnPropertyChanged("GenderId");
               }
           }
       }


       private string _Gender;
       public string Gender
       {
           get { return _Gender; }
           set
           {
               if (value != _Gender)
               {
                   _Gender = value;
                   OnPropertyChanged("Gender");
               }
           }
       }

       private long lngDoctorId;
       public long DoctorId
       {
           get { return lngDoctorId; }
           set
           {
               if (value != lngDoctorId)
               {
                   lngDoctorId = value;
                   OnPropertyChanged("DoctorId");
               }
           }
       }

       private string strDoctorName ="";
       public string DoctorName
       {
           get { return strDoctorName; }
           set
           {
               if (value != strDoctorName)
               {
                   strDoctorName = value;
                   OnPropertyChanged("DoctorName");
               }
           }
       }

       private long lngPatientId;
       public long PatientId
       {
           get { return lngPatientId; }
           set
           {
               if (value != lngPatientId)
               {
                   lngPatientId = value;
                   OnPropertyChanged("PatientId");
               }
           }
       }

       private long lngPatientUnitId;
       public long PatientUnitId
       {
           get { return lngPatientUnitId; }
           set
           {
               if (value != lngPatientUnitId)
               {
                   lngPatientUnitId = value;
                   OnPropertyChanged("PatientUnitId");
               }
           }
       }

       private string strPatientName ="";
       public string PatientName
       {
           get { return strPatientName = strFirstName + " " +strMiddleName +" " + strLastName; }
           set
           {
               if (value != strPatientName)
               {
                   strPatientName = value;
                   OnPropertyChanged("PatientName");
               }
           }
       }

       private string strMobileNo = " ";
       public string MobileNo
       {
           get
           {
               
                   return strMobileNo = MobileCountryCode + " " + ContactNo1;
              
           }
           set
           {
               if (value != strMobileNo)
               {
                   strMobileNo = value;
                   OnPropertyChanged("MobileNo");
               }
           }
            
       }

       private string strResidenceNo = " ";
       public string ResidenceNo
       {
           get
           {
               if (_ResiNoCountryCode != 0 && _ResiSTDCode != 0)
               {
                   return strResidenceNo = ResiNoCountryCode.ToString() + "-" + ResiSTDCode.ToString() + "-" + ContactNo2;
               }
               else if (_ResiNoCountryCode == 0 && _ResiSTDCode != 0)
               {
                   return strResidenceNo =  ResiSTDCode.ToString() + "-" + ContactNo2;
               }
               else if (_ResiNoCountryCode != 0 && _ResiSTDCode == 0)
               {
                   return strResidenceNo = ResiNoCountryCode.ToString() +"-" + ContactNo2;
               }
               else
               {
                   return strResidenceNo = ContactNo2;
               }

           }
           set
           {
               if (value != strResidenceNo)
               {
                   strResidenceNo = value;
                  OnPropertyChanged("ResidenceNo");
               }
           }
       }

       private long lngDepartmentId;
       public long DepartmentId
       {
           get { return lngDepartmentId; }
           set
           {
               if (value != lngDepartmentId)
               {
                   lngDepartmentId = value;
                   OnPropertyChanged("DepartmentId");
               }
           }  
       }

       private long lngUnitId;
       public long UnitId
       {
           get { return lngUnitId; }
           set
           {
               if (value != lngUnitId)
               {
                   lngUnitId = value;
                   OnPropertyChanged("UnitId");
               }
           }
       }

       public string UnitName { get; set; }

       private long lngAppointmentReasonId;
       public long AppointmentReasonId
       {
           get { return lngAppointmentReasonId; }
           set
           {
               if (value != lngAppointmentReasonId)
               {
                   lngAppointmentReasonId = value;
                   OnPropertyChanged("AppointmentReasonId");
               }
           }
       }
       private string  _AppointmentReason;
       public string AppointmentReason
       {
           get { return _AppointmentReason; }
           set
           {
               if (value != _AppointmentReason)
               {
                   _AppointmentReason = value;
                   OnPropertyChanged("AppointmentReason");
               }
           }
       }

     private long lngSpecialRegistrationID;
     public long SpecialRegistrationID
     {
         get { return lngSpecialRegistrationID; }
         set
         {
             if (value != lngSpecialRegistrationID)
             {
                 lngSpecialRegistrationID = value;
                 OnPropertyChanged("SpecialRegistrationID");
             }
         }

     }
       private string _SpecialRegistration;
       public string SpecialRegistration
       {
           get { return _SpecialRegistration; }

           set
           {
               if (value != _SpecialRegistration)
               {
                   _SpecialRegistration = value;
                   OnPropertyChanged("SpecialRegistration");
               }
           }
       }
            
       
       private string strRemark ="";
       public string Remark
       {
           get { return strRemark; }
           set
           {
               if (value != strRemark)
               {
                   strRemark = value;
                   OnPropertyChanged("Remark");
               }
           }
       }

//***//--------------------
       private string strReschedule = "";
       public string Reschedule
       {
           get { return strReschedule; }
           set
           {
               if (value != strReschedule)
               {
                   strReschedule = value;
                   OnPropertyChanged("Reschedule");
               }
           }
       }

       private string strCancelschedule = "";
       public string Cancelschedule
       {
           get { return strCancelschedule; }
           set
           {
               if (value != strCancelschedule)
               {
                   strCancelschedule = value;
                   OnPropertyChanged("Cancelschedule");
               }
           }
       }
//***//-----------------------------------

       private string strUserName ="";
       public string UserName
       {
           get { return strUserName; }
           set
           {
               if (value != strUserName)
               {
                   strUserName = value;
                   OnPropertyChanged("UserName");
               }
           }
       }

       private string strCivilId ="";
       public string CivilId
       {
           get { return strCivilId; }
           set
           {
               if (value != strCivilId)
               {
                   strCivilId = value;
                   OnPropertyChanged("CivilId");
               }
           }
       }

       private long lngNationalityId;
       public long NationalityId
       {
           get { return lngNationalityId; }
           set
           {
               if (value != lngNationalityId)
               {
                   lngNationalityId = value;
                   OnPropertyChanged("NationalityId");
               }
           }
       }

       private long lngMaritalStatusId;
       public long MaritalStatusId
       {
           get { return lngMaritalStatusId; }
           set
           {
               if (value != lngMaritalStatusId)
               {
                   lngMaritalStatusId = value;
                   OnPropertyChanged("MaritalStatusId");
               }
           }
       }

       private long lngBloodId;
       public long BloodId
       {
           get { return lngBloodId; }
           set
           {
               if (value != lngBloodId)
               {
                   lngBloodId = value;
                   OnPropertyChanged("BloodId");
               }
           }
       }

       private long lngVisitId;
       public long VisitId
       {
           get { return lngVisitId; }
           set
           {
               if (value != lngVisitId)
               {
                   lngVisitId = value;
                   OnPropertyChanged("VisitId");
               }
           }
       }
       public string VisitType { get; set; }
       private long lngReminderCount;
       public long ReminderCount
       {
           get { return lngReminderCount; }
           set
           {
               if (value != lngReminderCount)
               {
                   lngReminderCount = value;
                   OnPropertyChanged("ReminderCount");
               }
           }
       }

       private long lngCreatedUnitID;
      public long CreatedUnitID
       {
           get { return lngCreatedUnitID; }
           set
           {
               if (value != lngCreatedUnitID)
               {
                   lngCreatedUnitID = value;
                   OnPropertyChanged("CreatedUnitID");
               }
           }
       }


      private long lngUpdatedUnitID;
      public long UpdatedUnitID
      {
          get { return lngUpdatedUnitID; }
          set
          {
              if (value != lngUpdatedUnitID)
              {
                  lngUpdatedUnitID = value;
                  OnPropertyChanged("UpdatedUnitID");
              }
          }
      }

       private bool _UnRegisterd;
       public bool UnRegistered
       {
           get { return _UnRegisterd; }
           set { _UnRegisterd=value ; }
       }



       private bool? blnStatus=null;
       public bool? Status
       {
           get { return blnStatus; }
           set
           {
               if (value != blnStatus)
               {
                   blnStatus = value;
                   OnPropertyChanged("Status");
               }
           }
       }


      

       private string strAddedOn ="";
       public string AddedOn
       {
           get { return strAddedOn; }
           set
           {
               if (value != strAddedOn)
               {
                   strAddedOn = value;
                   OnPropertyChanged("AddedOn");
               }
           }
       }

       private string strAddedWindowsLoginName ="";
       public string AddedWindowsLoginName
       {
           get { return strAddedWindowsLoginName; }
           set
           {
               if (value != strAddedWindowsLoginName)
               {
                   strAddedWindowsLoginName = value;
                   OnPropertyChanged("AddedWindowsLoginName");
               }
           }
       }

       private string strUpdatedOn ="";
       public string UpdatedOn
       {
           get { return strUpdatedOn; }
           set
           {
               if (value != strUpdatedOn)
               {
                   strUpdatedOn = value;
                   OnPropertyChanged("UpdatedOn");
               }
           }
       }

       private string strUpdatedWindowsLoginName ="";
       public string UpdatedWindowsLoginName
       {
           get { return strUpdatedWindowsLoginName; }
           set
           {
               if (value != strUpdatedWindowsLoginName)
               {
                   strUpdatedWindowsLoginName = value;
                   OnPropertyChanged("UpdatedWindowsLoginName");
               }
           }
       }


       public bool? IsAcknowledged { get; set; }
      
       public long? AddedBy { get; set; }
       public long? UpdatedBy { get; set; }

       public long AppointmentSourceId { get; set; }
       public DateTime? AddedDateTime { get; set; }
       public DateTime? UpdatedDateTime { get; set; }
//by Anjali..............................
public string createdByName{get;set;}
public string ModifiedByName { get; set; }
public string MarkVisitStatus { get; set; }
public string AppointmentStatusNew { get; set; }
//.......................................
       public string Description{get;set;}

       public bool _IsEnabled;
       public bool IsEnabled
       {
           get { return _IsEnabled; }
           set { _IsEnabled = value; }
       }


      
       public string _MrNo ="";
        public string MrNo
       {
           get { return _MrNo; }
           set { _MrNo = value; }
       }

        public string _AppCancelReason="";
        public string AppCancelReason
        {
            get { return _AppCancelReason; }
            set { _AppCancelReason = value; }
        }

       private string _Schedule1_StartTime;
       public string Schedule1_StartTime { get; set; }
       private string _Schedule1_EndTime;
       public string Schedule1_EndTime { get; set; }
       private string _Schedule2_StartTime;
       public string Schedule2_StartTime { get; set; }
       private string _Schedule2_EndTime;
       public string Schedule2_EndTime { get; set; }


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

       private bool _VisitMark;
       public bool VisitMark
       {
           get { return _VisitMark; }
           set
           {
               if (_VisitMark != value)
               {
                   _VisitMark = value;
                   OnPropertyChanged("VisitMark");
               }
           }
       }

       private DateTime? _PastAppointmentDate;
       public DateTime? PastAppointmentDate
       {
           get
           {
               return _PastAppointmentDate;
           }

           set
           {
               if (value != _PastAppointmentDate)
               {
                   _PastAppointmentDate = value;
                   OnPropertyChanged("_PastAppointmentDate");
               }
           }
       }

       private string _PastAppointmentFromTime;
       public string PastAppointmentFromTime
       {
           get
           {
               return _PastAppointmentFromTime;
           }

           set
           {
               if (value != _PastAppointmentFromTime)
               {
                   _PastAppointmentFromTime = value;
                   OnPropertyChanged("PastAppointmentFromTime");
               }
           }
       }

       private DateTime? _FutureAppointmentDate;
       public DateTime? FutureAppointmentDate
       {
           get
           {
               return _FutureAppointmentDate;
           }

           set
           {
               if (value != _FutureAppointmentDate)
               {
                   _FutureAppointmentDate = value;
                   OnPropertyChanged("_FutureAppointmentDate");
               }
           }
       }

     

       private string _FutureAppointmentFromTime;
       public string FutureAppointmentFromTime
       {
           get
           {
               return _FutureAppointmentFromTime;
           }
           set
           {
               if (value != _FutureAppointmentFromTime)
               {
                   _FutureAppointmentFromTime = value;
                   OnPropertyChanged("FutureAppointmentFromTime");
               }
           }
       }
       private double? _BalanceAmount;
       public double? BalanceAmount
       {
           get
           {
               return _BalanceAmount;
           }

           set
           {
               if (value != _BalanceAmount)
               {
                   _BalanceAmount = value;
                   OnPropertyChanged("BalanceAmount");
               }
           }
       }
      

                   
       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion

       #region INotifyPropertyChanged Members

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
    }
}
