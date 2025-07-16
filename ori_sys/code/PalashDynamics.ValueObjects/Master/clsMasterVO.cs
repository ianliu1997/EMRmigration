using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.Master
{

    public class clsDashBoardVO : IValueObject
    {


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long ID { get; set; }

        public string Code  { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public bool Active { get; set; }

    }
   
   public class clsMasterVO : IValueObject 
    {

        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion

    

    }

   public class clsUserRoleVO : IValueObject, INotifyPropertyChanged
   {
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
                   OnPropertyChanged("ID");
               }
           }
       }
       
       private string _Code;
       public string Code
       {
           get
           {
               return _Code;
           }

           set
           {
               if (value != _Code)
               {
                   _Code = value;
                   OnPropertyChanged("Code");
               }
           }
       }


       private string _Description;
       public string Description
       {
           get
           {
               return _Description;
           }

           set
           {
               if (value != _Description)
               {
                   _Description = value;
                   OnPropertyChanged("Description");
               }
           }
       }
       //private string _RoleStatus;
       //public string Status
       //{
       //    get
       //    {
       //        return _RoleStatus;
       //    }
       //    set
       //    {
       //        if (value != _RoleStatus)
       //        {
       //            _RoleStatus = value;
       //            OnPropertyChanged("RoleStatus");
       //        }
       //    }
       //}

       //private List<clsUserMenuDetailsVO> _RoleMenuDetailsList = new List<clsUserMenuDetailsVO>();

       //public List<clsUserMenuDetailsVO> RoleMenuDetailsList
       //{
       //    get { return _RoleMenuDetailsList; }
       //    set { _RoleMenuDetailsList = value; }
       //}

       private List<clsMenuVO> _MenuList;
       public List<clsMenuVO> MenuList
       {
           get
           {
               if (_MenuList == null)
                   _MenuList = new List<clsMenuVO>();
               return _MenuList;
           }
           set { _MenuList = value; }
       }

       private List<clsDashBoardVO> _DashBoardList;
       public List<clsDashBoardVO> DashBoardList
       {
           get
           {
               if (_DashBoardList == null)
                   _DashBoardList = new List<clsDashBoardVO>();
               return _DashBoardList;
           }
           set { _DashBoardList = value; }
       }

       #region Common Properties

       private long _UnitId;
       public long UnitId { get; set; }

       private bool _Status = true;
       public bool Status
       {
           get
           {
               return _Status;
           }
           set
           {
               _Status = value;
           }
       }

       private bool _IsActive = true;
       public bool IsActive
       {
           get
           {
               return _IsActive;
           }
           set
           {
               _IsActive = value;
           }
       }

       private long _AddedBy;
       public long AddedBy { get; set; }

       private string _AddedOn;
       public string AddedOn { get; set; }

       public DateTime? AddedDateTime { get; set; }

       private string _AddedWindowsLoginName;
       public string AddedWindowsLoginName { get; set; }

       public long? UpdatedBy { get; set; }

       public string UpdatedOn { get; set; }

       public DateTime? UpdatedDateTime { get; set; }

       private string _UpdatedWindowsLoginName;
       public string UpdatedWindowsLoginName { get; set; }

       #endregion
       
       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }
       public override string ToString()
       {
           return Description;
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

   public class clsUserMenuDetailsVO
   {
       public long UserMenuDetailId { get; set; }
       public long UserId { get; set; }
       public long MenuId { get; set; }
       public bool Status { get; set; }
   }


  
}
