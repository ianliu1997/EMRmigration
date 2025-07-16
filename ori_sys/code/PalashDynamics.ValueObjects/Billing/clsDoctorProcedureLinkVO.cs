using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Radiology;

namespace PalashDynamics.ValueObjects.Billing
{
   public class clsDoctorProcedureLinkVO : IValueObject, INotifyPropertyChanged
    {
       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion

       #region INotifyPropertyChanged Members

       public event PropertyChangedEventHandler PropertyChanged;

       public void OnPropertyChanged(string PropertyName)
       {
           if (PropertyChanged != null)
               PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
       }

       #endregion

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

       private long _BillID;
       public long BillID
       {
           get { return _BillID; }
           set
           {
               if (_BillID != value)
               {
                   _BillID = value;
                   OnPropertyChanged("BillID");
               }
           }
       }

       private DateTime? _Date;
       public DateTime? Date
       {
           get { return _Date; }
           set
           {
               if (_Date != value)
               {
                   _Date = value;
                   OnPropertyChanged("Date");
               }
           }
       }

       private DateTime? _Time;
       public DateTime? Time
       {
           get { return _Time; }
           set
           {
               if (_Time != value)
               {
                   _Time = value;
                   OnPropertyChanged("Time");
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

       private DateTime? _BillDate;
       public DateTime? BillDate
       {
           get { return _BillDate; }
           set
           {
               if (_BillDate != value)
               {
                   _BillDate = value;
                   OnPropertyChanged("BillDate");
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

       private string _Procedure;
       public string Procedure
       {
           get { return _Procedure; }
           set
           {
               if (_Procedure != value)
               {
                   _Procedure = value;
                   OnPropertyChanged("Procedure");
               }
           }
       }

       private string _Specilazation;
       public string Specilazation
       {
           get { return _Specilazation; }
           set
           {
               if (_Specilazation != value)
               {
                   _Specilazation = value;
                   OnPropertyChanged("Specilazation");
               }
           }
       }

       private string _Doctor;
       public string Doctor
       {
           get { return _Doctor; }
           set
           {
               if (_Doctor != value)
               {
                   _Doctor = value;
                   OnPropertyChanged("Doctor");
               }
           }
       }

       private string _Nurse;
       public string Nurse
       {
           get { return _Nurse; }
           set
           {
               if (_Nurse != value)
               {
                   _Nurse = value;
                   OnPropertyChanged("Nurse");
               }
           }
       }

       private long _BillUnitID;
       public long BillUnitID
       {
           get { return _BillUnitID; }
           set
           {
               if (_BillUnitID != value)
               {
                   _BillUnitID = value;
                   OnPropertyChanged("BillUnitID");
               }
           }
       }

       private long _NurseID;
       public long NurseID
       {
           get { return _NurseID; }
           set
           {
               if (_NurseID != value)
               {
                   _NurseID = value;
                   OnPropertyChanged("NurseID");
               }
           }
       }

       private long _DoctorID;
       public long DoctorID
       {
           get { return _DoctorID; }
           set
           {
               if (_DoctorID != value)
               {
                   _DoctorID = value;
                   OnPropertyChanged("DoctorID");
               }
           }
       }

       private long _SpecilazationID;
       public long SpecilazationID
       {
           get { return _SpecilazationID; }
           set
           {
               if (_SpecilazationID != value)
               {
                   _SpecilazationID = value;
                   OnPropertyChanged("SpecilazationID");
               }
           }
       }

       private long _ProcedureID;
       public long ProcedureID
       {
           get { return _ProcedureID; }
           set
           {
               if (_ProcedureID != value)
               {
                   _ProcedureID = value;
                   OnPropertyChanged("ProcedureID");
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
       
       private long _UpdatedUnitID;
       public long UpdatedUnitID
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
       
       private long _AddedBy;
       public long AddedBy
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

       private long _AddedOn;
       public long AddedOn
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

       private DateTime? _AddedDateTime;
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
       
       private long _UpdatedOn;
       public long UpdatedOn
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
       
       private DateTime? _UpdatedDateTime;
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
    }
}
