using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
  public class clsMaleHistoryVO:IValueObject,INotifyPropertyChanged
    {
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
      private string _Medical;
      public string Medical
      {
          get { return _Medical; }
          set
          {
              if (_Medical != value)
              {
                  _Medical = value;
                  OnPropertyChanged("Medical");
              }
          }
      }

      private string _Surgical;
      public string Surgical
      {
          get { return _Surgical; }
          set
          {
              if (_Surgical != value)
              {
                  _Surgical = value;
                  OnPropertyChanged("Surgical");
              }
          }
      }

      private string _Family;
      public string Family
      {
          get { return _Family; }
          set
          {
              if (_Family != value)
              {
                  _Family = value;
                  OnPropertyChanged("Family");
              }
          }
      }

      private string _Complication;
      public string Complication
      {
          get { return _Complication; }
          set
          {
              if (_Complication != value)
              {
                  _Complication = value;
                  OnPropertyChanged("Complication");
              }
          }
      }

      private string _Allergies;
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

      private string _Undergarments;
      public string Undergarments
      {
          get { return _Undergarments; }
          set
          {
              if (_Undergarments != value)
              {
                  _Undergarments = value;
                  OnPropertyChanged("Undergarments");
              }
          }
      }

      private string _Medications;
      public string Medications
      {
          get { return _Medications; }
          set
          {
              if (_Medications != value)
              {
                  _Medications = value;
                  OnPropertyChanged("Medications");
              }
          }
      }
      private bool _ExposerToHeat;
      public bool ExposerToHeat
      {
          get { return _ExposerToHeat; }
          set
          {
              if (_ExposerToHeat != value)
              {
                  _ExposerToHeat = value;
                  OnPropertyChanged("ExposerToHeat");
              }
          }
      }

      private string _ExposerToHeatDetails;
      public string ExposerToHeatDetails
      {
          get { return _ExposerToHeatDetails; }
          set
          {
              if (_ExposerToHeatDetails != value)
              {
                  _ExposerToHeatDetails = value;
                  OnPropertyChanged("ExposerToHeatDetails");
              }
          }
      }

      private bool _Smoking;
      public bool Smoking
      {
          get { return _Smoking; }
          set
          {
              if (_Smoking != value)
              {
                  _Smoking = value;
                  OnPropertyChanged("Smoking");
              }
          }
      }

      private bool _SmokingHabitual;
      public bool SmokingHabitual
      {
          get { return _SmokingHabitual; }
          set
          {
              if (_SmokingHabitual != value)
              {
                  _SmokingHabitual = value;
                  OnPropertyChanged("SmokingHabitual");
              }
          }
      }

      private bool _Alcohol;
      public bool Alcohol
      {
          get { return _Alcohol; }
          set
          {
              if (_Alcohol != value)
              {
                  _Alcohol = value;
                  OnPropertyChanged("Alcohol");
              }
          }
      }
      private bool _AlcoholHabitual;
      public bool AlcoholHabitual
      {
          get { return _AlcoholHabitual; }
          set
          {
              if (_AlcoholHabitual != value)
              {
                  _AlcoholHabitual = value;
                  OnPropertyChanged("AlcoholHabitual");
              }
          }
      }

      private bool _AnyOther;
      public bool AnyOther
      {
          get { return _AnyOther; }
          set
          {
              if (_AnyOther != value)
              {
                  _AnyOther = value;
                  OnPropertyChanged("AnyOther");
              }
          }
      }
      private string _AnyOtherDetails;
      public string AnyOtherDetails
      {
          get { return _AnyOtherDetails; }
          set
          {
              if (_AnyOtherDetails != value)
              {
                  _AnyOtherDetails = value;
                  OnPropertyChanged("AnyOtherDetails");
              }
          }
      }

      private bool _OtherHabitual;
      public bool OtherHabitual
      {
          get { return _OtherHabitual; }
          set
          {
              if (_OtherHabitual != value)
              {
                  _OtherHabitual = value;
                  OnPropertyChanged("OtherHabitual");
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

      #region Common Properties


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
}
