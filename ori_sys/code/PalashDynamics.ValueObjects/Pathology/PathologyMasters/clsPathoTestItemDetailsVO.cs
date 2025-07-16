using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.ValueObjects.Pathology.PathologyMasters
{
   public class clsPathoTestItemDetailsVO : IValueObject, INotifyPropertyChanged
    {
       #region IValueObject
       public string ToXml()
       {
           return this.ToString();
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

        #region Property Declartion
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


           //BY ROHINI DATED 22


           List<MasterListItem> _DeductionList = new List<MasterListItem>();
           public List<MasterListItem> DeductionList
           {
               get
               {
                   return _DeductionList;
               }
               set
               {
                   if (value != _DeductionList)
                   {
                       _DeductionList = value;

                   }
               }

           }
           List<MasterListItem> _UsageList = new List<MasterListItem>();
           public List<MasterListItem> UsageList
           {
               get
               {
                   return _UsageList;
               }
               set
               {
                   if (value != _UsageList)
                   {
                       _UsageList = value;

                   }
               }

           }

           List<MasterListItem> _UOMList = new List<MasterListItem>();
           public List<MasterListItem> UOMList
           {
               get
               {
                   return _UOMList;
               }
               set
               {
                   if (value != _UOMList)
                   {
                       _UOMList = value;

                   }
               }

           }

           List<clsConversionsVO> _UOMConversionList = new List<clsConversionsVO>();
           public List<clsConversionsVO> UOMConversionList
           {
               get
               {
                   return _UOMConversionList;
               }
               set
               {
                   if (value != _UOMConversionList)
                   {
                       _UOMConversionList = value;
                       OnPropertyChanged("UOMConversionList");
                   }
               }

           }

           public string ItemName { get; set; }

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

           private long _ItemID;
           public long ItemID
           {
               get { return _ItemID; }
               set
               {
                   if (_ItemID != value)
                   {
                       _ItemID = value;
                       OnPropertyChanged("ItemID");
                   }
               }
           }
       //BY ROHINI
           private long _UID;
           public long UID
           {
               get { return _UID; }
               set
               {
                   if (_UID != value)
                   {
                       _UID = value;
                       OnPropertyChanged("UID");
                   }
               }
           }
           private long _DID;
           public long DID
           {
               get { return _DID; }
               set
               {
                   if (_DID != value)
                   {
                       _DID = value;
                       OnPropertyChanged("DID");
                   }
               }
           }
           private long _UOMid;
           public long UOMid
           {
               get { return _UOMid; }
               set
               {
                   if (_UOMid != value)
                   {
                       _UOMid = value;
                       OnPropertyChanged("UOMid");
                   }
               }
           }
           private string _UOMName;
           public string UOMName
           {
               get { return _UOMName; }
               set
               {
                   if (_UOMName != value)
                   {
                       _UOMName = value;
                       OnPropertyChanged("UOMName");
                   }
               }
           }

           private string _UName;
           public string UName
           {
               get { return _UName; }
               set
               {
                   if (_UName != value)
                   {
                       _UName = value;
                       OnPropertyChanged("UName");
                   }
               }
           }
           MasterListItem _SelectedDID = new MasterListItem { ID = 0, Description = "--Select--" };
           public MasterListItem SelectedDID
           {
               get
               {
                   return _SelectedDID;
               }
               set
               {
                   if (value != _SelectedDID)
                   {
                       _SelectedDID = value;
                       OnPropertyChanged("SelectedDID");
                   }
               }


           }
           MasterListItem _SelectedUID = new MasterListItem { ID = 0, Description = "--Select--" };
           public MasterListItem SelectedUID
           {
               get
               {
                   return _SelectedUID;
               }
               set
               {

                   if (value != _SelectedUID)
                   {
                       _SelectedUID = value;
                       OnPropertyChanged("SelectedUID");
                   }
               }


           }

           MasterListItem _SelectedUOM = new MasterListItem();
           public MasterListItem SelectedUOM
           {
               get
               {
                   return _SelectedUOM;
               }
               set
               {
                   if (value != _SelectedUOM)
                   {
                       _SelectedUOM = value;
                       OnPropertyChanged("SelectedUOM");
                   }
               }


           }
           private string _DName;
           public string DName
           {
               get { return _DName; }
               set
               {
                   if (_DName != value)
                   {
                       _DName = value;
                       OnPropertyChanged("DName");
                   }
               }
           }
        
       //-------------
           private float _Quantity;
           public float Quantity
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

           private long? _CreatedUnitID;
           public long? CreatedUnitID
           {
               get
               {
                   return _CreatedUnitID;
               }
               set
               {
                   _CreatedUnitID = value;
               }
           }

           private long? _UpdatedUnitID;
           public long? UpdatedUnitID
           {
               get
               {
                   return _UpdatedUnitID;
               }
               set
               {
                   _UpdatedUnitID = value;
               }
           }

           private long? _AddedBy;
           public long? AddedBy
           {
               get
               {
                   return _AddedBy;
               }
               set
               {
                   _AddedBy = value;
               }
           }

           private string _AddedOn;
           public string AddedOn
           {
               get
               {
                   return _AddedOn;
               }
               set
               {
                   _AddedOn = value;
               }
           }

           private DateTime? _AddedDateTime;
           public DateTime? AddedDateTime
           {
               get
               {
                   return _AddedDateTime;
               }
               set
               {
                   _AddedDateTime = value;
               }
           }

           private long? _UpdatedBy;
           public long? UpdatedBy
           {
               get
               {
                   return _UpdatedBy;
               }
               set
               {
                   _UpdatedBy = value;
               }
           }


           private string _UpdatedOn;
           public string UpdatedOn
           {
               get
               {
                   return _UpdatedOn;
               }
               set
               {
                   _UpdatedOn = value;
               }
           }

           private DateTime? _UpdatedDateTime;
           public DateTime? UpdatedDateTime
           {
               get
               {
                   return _UpdatedDateTime;
               }
               set
               {
                   _UpdatedDateTime = value;
               }
           }

           private string _AddedWindowsLoginName;
           public string AddedWindowsLoginName
           {
               get
               {
                   return _AddedWindowsLoginName;
               }
               set
               {
                   _AddedWindowsLoginName = value;
               }
           }

           private string _UpdateWindowsLoginName;
           public string UpdateWindowsLoginName
           {
               get
               {
                   return _UpdateWindowsLoginName;
               }
               set
               {
                   _UpdateWindowsLoginName = value;
               }
           }
           private long _BatchID;
           public long BatchID
           {
               get { return _BatchID; }
               set
               {
                   if (value != _BatchID)
                   {
                       _BatchID = value;
                       OnPropertyChanged("BatchID");
                   }
               }
           }

           private string _BatchCode;
           public string BatchCode
           {
               get { return _BatchCode; }
               set
               {
                   if (value != _BatchCode)
                   {
                       _BatchCode = value;
                       OnPropertyChanged("BatchCode");
                   }
               }
           }

           private double _BalanceQuantity;
           public double BalanceQuantity
           {
               get { return _BalanceQuantity; }
               set
               {
                   if (value != _BalanceQuantity)
                   {
                       _BalanceQuantity = value;
                       OnPropertyChanged("BalanceQuantity");
                   }
               }
           }


           private double _IdealQuantity;
           public double IdealQuantity
           {
               get { return _IdealQuantity; }
               set
               {
                   if (value != _IdealQuantity)
                   {
                       _IdealQuantity = value;
                       OnPropertyChanged("IdealQuantity");
                   }
               }
           }


           private double _ActualQantity;
           public double ActualQantity
           {
               get { return _ActualQantity; }
               set
               {
                   if (_ActualQantity != value)
                   {
                       _ActualQantity = value;
                       OnPropertyChanged("ActualQantity");
                   }
               }
           }


           private DateTime? _ExpiryDate;
           public DateTime? ExpiryDate
           {
               get { return _ExpiryDate; }
               set
               {
                   if (value != _ExpiryDate)
                   {
                       _ExpiryDate = value;
                       OnPropertyChanged("ExpiryDate");
                   }
               }
           }

           private long _StoreID;
           public long StoreID
           {
               get { return _StoreID; }
               set
               {
                   if (value != _StoreID)
                   {
                       _StoreID = value;
                       OnPropertyChanged("StoreID");
                   }
               }
           }

           private string _ItemCode;
           public string ItemCode
           {
               get { return _ItemCode; }
               set
               {
                   if (value != _ItemCode)
                   {
                       _ItemCode = value;
                       OnPropertyChanged("ItemCode");
                   }
               }
           }

           private string _Remarks;
           public string Remarks
           {
               get { return _Remarks; }
               set
               {
                   if (value != _Remarks)
                   {
                       _Remarks = value;
                       OnPropertyChanged("Remarks");
                   }
               }
           }
          
         
        #endregion
    }

      #region Newly Added 
      public class clsPathoResultEntryTemplateVO : IValueObject, INotifyPropertyChanged
   {
       #region IValueObject
       public string ToXml()
       {
           return this.ToString();
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

       #region Property Declartion
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

       private long _OrderDetailID;
       public long OrderDetailID
       {
           get { return _OrderDetailID; }
           set
           {
               if (_OrderDetailID != value)
               {
                   _OrderDetailID = value;
                   OnPropertyChanged("OrderDetailID");
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



       private long _Pathologist;
       public long PathologistID
       {
           get { return _Pathologist; }
           set
           {
               if (_Pathologist != value)
               {
                   _Pathologist = value;
                   OnPropertyChanged("PathologistID");
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


       private string _Template;
       public string Template
       {
           get { return _Template; }
           set
           {
               if (_Template != value)
               {
                   _Template = value;
                   OnPropertyChanged("Template");
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
       #endregion
   }

      public class clsPathoTestTemplateDetailsVO : IValueObject, INotifyPropertyChanged
   {
       #region IValueObject
       public string ToXml()
       {
           return this.ToString();
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

       #region Property Declartion
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
       private string _SupplierName;
       public string SupplierName
       {
           get
           {
               return _SupplierName;
           }
           set
           {
               if (value != _SupplierName)
               {
                   _SupplierName = value;
                   OnPropertyChanged("SupplierName");
               }
           }
       }
       public List<clsPathoTestMasterVO> SupplierList { get; set; }
       List<MasterListItem> _HPLevelList = new List<MasterListItem> 
        { 
            new MasterListItem{ ID=0,Description="--Select--"} ,
            new MasterListItem{ ID=1,Description="I"} ,
            new MasterListItem{ ID=2,Description="II"} ,
            new MasterListItem{ ID=3,Description="III"} ,
        };
       public List<MasterListItem> HPLevelList
       {
           get
           {
               return _HPLevelList;
           }
           set
           {
               if (value != _HPLevelList)
               {
                   _HPLevelList = value;
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

       private bool _MultiplePathoDoctor = false;
       public bool MultiplePathoDoctor
       {
           get { return _MultiplePathoDoctor; }
           set
           {
               if (_MultiplePathoDoctor != value)
               {
                   _MultiplePathoDoctor = value;
                   OnPropertyChanged("MultiplePathoDoctor");
               }
           }
       }
       private string _Code;
       public string Code
       {
           get { return _Code; }
           set
           {
               if (_Code != value)
               {
                   _Code = value;
                   OnPropertyChanged("Code");
               }
           }
       }

       private string _Description;
       public string Description
       {
           get { return _Description; }
           set
           {
               if (_Description != value)
               {
                   _Description = value;
                   OnPropertyChanged("Description");
               }
           }
       }

  

       private string _Template;
       public string Template
       {
           get { return _Template; }
           set
           {
               if (_Template != value)
               {
                   _Template = value;
                   OnPropertyChanged("Template");
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
       private long _Pathologist;
       public long Pathologist
       {
           get { return _Pathologist; }
           set
           {
               if (_Pathologist != value)
               {
                   _Pathologist = value;
                   OnPropertyChanged("Pathologist");
               }
           }
       }
       private string _PathologistName;
       public string PathologistName
       {
           get { return _PathologistName; }
           set
           {
               if (_PathologistName != value)
               {
                   _PathologistName = value;
                   OnPropertyChanged("PathologistName");
               }
           }
       }
       //added by rohini h21/12/2015
       private List<MasterListItem> _GenderList = new List<MasterListItem>();
       public List<MasterListItem> GenderList
       {
           get
           {
               return _GenderList;
           }
           set
           {
               _GenderList = value;
           }
       }
       private long _GenderID;
       public long GenderID
       {
           get { return _GenderID; }
           set
           {
               if (_GenderID != value)
               {
                   _GenderID = value;
                   OnPropertyChanged("GenderID");
               }
           }
       }

       private string _GenderName;
       public string GenderName
       {
           get { return _GenderName; }
           set
           {
               if (_GenderName != value)
               {
                   _GenderName = value;
                   OnPropertyChanged("GenderName");
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
       #endregion
   }

      public class clsPathoTemplateVO : IValueObject, INotifyPropertyChanged
      {
          #region IValueObject
          public string ToXml()
          {
              return this.ToString();
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

          #region Property Declartion
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

          private string _Code;
          public string Code
          {
              get { return _Code; }
              set
              {
                  if (_Code != value)
                  {
                      _Code = value;
                      OnPropertyChanged("Code");
                  }
              }
          }

          private string _TemplateName;
          public string TemplateName
          {
              get { return _TemplateName; }
              set
              {
                  if (_TemplateName != value)
                  {
                      _TemplateName = value;
                      OnPropertyChanged("TemplateName");
                  }
              }
          }


          private string _Template;
          public string Template
          {
              get { return _Template; }
              set
              {
                  if (_Template != value)
                  {
                      _Template = value;
                      OnPropertyChanged("Template");
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
          #endregion
      }

      #endregion
}
