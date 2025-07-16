using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
   public class clsGetPatientMasterListBizActionVO:IBizActionValueObject
    {
       public clsGetPatientMasterListBizActionVO()
       {

       }

       private List<clsComboMasterBizActionVO> _ComboList = new List<clsComboMasterBizActionVO>();
       public List<clsComboMasterBizActionVO> ComboList
       {
           get { return _ComboList; }
           set { _ComboList = value; }
       }

       public long ID { get; set; }
      

       private bool _IsDecode;
       public bool IsDecode
       {
           get { return _IsDecode; }
           set { _IsDecode = value; }
       }

       public bool _SuccessStatus;
       public bool SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private const string _BizActionName = "PalashDynamics.BusinessLayer.Master.clsGetPatientMasterListBizAction";

        
       
       #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return _BizActionName ;
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {

            return this.ToString();
        }

        #endregion
    }
    


    /// <summary>
    /// /////////////Added By YK for fill bd master
    /// </summary>
   public class clsGetBdMasterBizActionVO : IBizActionValueObject
   {



       public clsGetBdMasterBizActionVO()
       {

       }

       private const string _BizActionName = "PalashDynamics.BusinessLayer.Master.clsGetBdMasterListBizAction";
       public string GetBizAction()
       {
           return _BizActionName;
       }

       //private List<clsComboMasterBizActionVO> _ComboList = new List<clsComboMasterBizActionVO>();
       //public List<clsComboMasterBizActionVO> ComboList
       //{
       //    get { return _ComboList; }
       //    set { _ComboList = value; }
       //}

       private List<MasterListItem> _MasterList = null;
       public List<MasterListItem> MasterList
       {
           get
           { return _MasterList; }

           set
           { _MasterList = value; }
       }

       public long UnitID { get; set; }


       //private bool _IsDecode;
       //public bool IsDecode
       //{
       //    get { return _IsDecode; }
       //    set { _IsDecode = value; }
       //}

       public bool _SuccessStatus;
       public bool SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       



       #region IBizActionValueObject Members

       

       #endregion

       #region IValueObject Members

       public string ToXml()
       {

           return this.ToString();
       }

       #endregion
   }



   public class clsGetUserMasterListBizActionVO : IBizActionValueObject
   {
       public clsGetUserMasterListBizActionVO()
       {

       }

       private List<MasterListItem> _MasterList = null;
       public List<MasterListItem> MasterList
       {
           get
           { return _MasterList; }

           set
           { _MasterList = value; }
       }

       public long ID { get; set; }


       private bool _IsDecode;
       public bool IsDecode
       {
           get { return _IsDecode; }
           set { _IsDecode = value; }
       }

       public bool _SuccessStatus;
       public bool SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }
       //// For IVF ADM Changes
       private bool _IsDisplayStaffName;
       public bool IsDisplayStaffName
       {
           get { return _IsDisplayStaffName; }
           set { _IsDisplayStaffName = value; }
       }

       private const string _BizActionName = "PalashDynamics.BusinessLayer.Master.clsGetUserMasterListBizAction";



       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return _BizActionName;
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
