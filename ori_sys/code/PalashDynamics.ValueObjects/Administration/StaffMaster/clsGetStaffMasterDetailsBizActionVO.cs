using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.StaffMaster
{
   public class clsGetStaffMasterDetailsBizActionVO:IBizActionValueObject
    {
       private List<clsStaffMasterVO> myVar = new List<clsStaffMasterVO>();
       public List<clsStaffMasterVO> StaffMasterList
        {
            get { return myVar; }
            set { myVar = value; }
        }

       public long  ID { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public long DesignationID { get; set; }
       public long UnitID { get; set; }
       public long ClinicId { get; set; }
       public string ClinicName { get; set; }
       public string StrClinicID { get; set; }

       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }

       public string sortExpression { get; set; }
      
       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsGetStaffMasterDetailsBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion
    }

   public class clsStaffAddressDetailsVO : INotifyPropertyChanged, IValueObject
   {
       public clsStaffAddressDetailsVO()
       {

       }

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
               PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
       }

       #endregion

       private long _RowID;
       public long RowID
       {
           get
           {
               return _RowID;
           }

           set
           {
               if (value != _RowID)
               {
                   _RowID = value;

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
               if (value != _ID)
               {
                   _ID = value;

               }
           }
       }

       private long _StaffId;
       public long StaffId
       {
           get { return _StaffId; }
           set
           {
               if (value != _StaffId)
               {
                   _StaffId = value;
                   OnPropertyChanged("StaffId");
               }
           }
       }

       private string _AddressType;
       public string AddressType
       {
           get { return _AddressType; }
           set
           {
               if (value != _AddressType)
               {
                   _AddressType = value;
                   OnPropertyChanged("AddressType");
               }
           }
       }

       private string _IsNew;
       public string IsNew
       {
           get { return _IsNew; }
           set
           {
               if (value != _IsNew)
               {
                   _IsNew = value;
                   OnPropertyChanged("IsNew");
               }
           }
       }

       private long _AddressTypeID;
       public long AddressTypeID
       {
           get { return _AddressTypeID; }
           set
           {
               if (value != _AddressTypeID)
               {
                   _AddressTypeID = value;
                   OnPropertyChanged("AddressTypeID");
               }
           }
       }

       private string _Name;
       public string Name
       {
           get { return _Name; }
           set
           {
               if (value != _Name)
               {
                   _Name = value;
                   OnPropertyChanged("Name");
               }
           }
       }

       private long _BranchId;
       public long BranchId
       {
           get { return _BranchId; }
           set
           {
               if (value != _BranchId)
               {
                   _BranchId = value;
                   OnPropertyChanged("BranchId");
               }
           }
       }

       private string _Address;
       public string Address
       {
           get { return _Address; }
           set
           {
               if (value != _Address)
               {
                   _Address = value;
                   OnPropertyChanged("Address");
               }
           }
       }

       private string _Contact1;
       public string Contact1
       {
           get { return _Contact1; }
           set
           {
               if (value != _Contact1)
               {
                   _Contact1 = value;
                   OnPropertyChanged("Contact1");
               }
           }
       }

       public int TotalRows { get; set; }

       private string _Contact2;
       public string Contact2
       {
           get { return _Contact2; }
           set
           {
               if (value != _Contact2)
               {
                   _Contact2 = value;
                   OnPropertyChanged("Contact2");
               }
           }
       }
   }

   public class clsGetStaffMasterByUnitIDBizActionVO : IBizActionValueObject
   {

       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsGetStaffMasterByUnitIDBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion

       public bool IsStaffPatient { get; set; }
       private List<clsStaffAddressDetailsVO> StaffAddress = new List<clsStaffAddressDetailsVO>();
       public List<clsStaffAddressDetailsVO> StaffAddressList
       {
           get { return StaffAddress; }
           set { StaffAddress = value; }
       }
       private List<clsStaffMasterVO> myVar = new List<clsStaffMasterVO>();
       public List<clsStaffMasterVO> StaffMasterList
       {
           get { return myVar; }
           set { myVar = value; }
       }

       public long ID { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public long DesignationID { get; set; }
       public long UnitID { get; set; }
       public int FromNurseSchedule { get; set; }


       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }
   }

   public class clsGetUserSearchBizActionVO : IBizActionValueObject
   {
       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsGetUserSearchBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion

       private List<clsStaffMasterVO> myVar = new List<clsStaffMasterVO>();
       public List<clsStaffMasterVO> StaffMasterList
       {
           get { return myVar; }
           set { myVar = value; }
       }

       public long ID { get; set; }
       public bool IsRegisteredStaff { get; set; }
       public string FirstName { get; set; }
       public string MiddelName { get; set; }

       public string LastName { get; set; }
       public long DesignationID { get; set; }
       public long UnitID { get; set; }
       public long DepartmentID { get; set; }

       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }
       public string sortExpression { get; set; }
   }
}
