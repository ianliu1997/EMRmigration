using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.ValueObjects.User
{
    public class clsAddUserBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddUserBizAction";
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
        /// 
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsUserVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains User Details Which is Added.
        /// </summary>
        
        public clsUserVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

    public class clsSecretQtnBizActionVO:IBizActionValueObject
    {
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

        private List<clsSecretQtnVO> objDetailsSecret = null;
        // private clsItemStoreVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>

        public List<clsSecretQtnVO> Details
        // public clsItemStoreVO Details
        {
            get { return objDetailsSecret; }
            set { objDetailsSecret = value; }
        }

        private List<clsSecretQtnVO> objDetails;

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsSecretQtnBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

    public class clsUpdateUserLockedStatusBizActionVO:IBizActionValueObject 
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.User.clsUpdateUserLockedStatusBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
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

        //   public long ID { get; set; }
        private clsUserVO objUserLocked = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>

        public clsUserVO UserLockedStatus
        {
            get { return objUserLocked; }
            set { objUserLocked = value; }
        }
    }

    public class clsUpdateAuditTrailBizActionVO:IBizActionValueObject 
    {
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

        //   public long ID { get; set; }
        private clsUserVO objUser = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>

        public clsUserVO UserLog
        {
            get { return objUser; }
            set { objUser = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.User.clsUpdateUserAuditTrailBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }
    public class clsUpdateUserStatusBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.User.clsUpdateUserStatusBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
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

        //   public long ID { get; set; }
        private clsUserVO objUserStatus = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>

        public clsUserVO UserStatus
        {
            get { return objUserStatus; }
            set { objUserStatus = value; }
        }
    }

    public class clsResetPasswordBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.User.clsResetPasswordBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
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

        //   public long ID { get; set; }
        private clsUserVO objUserPass = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>

        public clsUserVO RPassword
        {
            get { return objUserPass; }
            set { objUserPass = value; }
        }      
    }

    public class clsGetUnitStoreStatusBizActionVO : IBizActionValueObject
    {
        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

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

        public long ID { get; set; }

        public long UserId { get; set; }

        private List<clsItemStoreVO> objStoreDetails = null;
        // private clsItemStoreVO objStoreDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>

        public List<clsItemStoreVO> Details
        // public clsItemStoreVO Details
        {
            get { return objStoreDetails; }
            set { objStoreDetails = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetUnitStoreStatusListBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

    public class clsGetUnitStoreBizActionVO:IBizActionValueObject 
    {
        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

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

        public long ID { get; set; }

        public long UserId { get; set; }

        private List<clsItemStoreVO> objStoreDetails = null;
       // private clsItemStoreVO objStoreDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>

        public List<clsItemStoreVO> Details
       // public clsItemStoreVO Details
        {
            get { return objStoreDetails; }
            set { objStoreDetails = value; }
        }
        
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetUnitStoreListBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
       
    }
    public class clsGetUserListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetUserListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

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

        public long ID { get; set; }
         
        private List<clsUserVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsUserVO> Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        //Added By Umesh
        public long UserRoleID { get; set; }
        public bool IsDoctor { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsPatient { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeActive { get; set; }

    }

    public class clsGetLoginNameBizActionVO: IBizActionValueObject
    {
        public string GetBizAction()
        {
              return "PalashDynamics.BusinessLayer.clsGetLoginNameBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
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

        public string LoginName { get; set; }

        private clsUserVO objUserName = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains Login Name Details Which is Added.
        /// </summary>

        public clsUserVO UserName
        {
            get { return objUserName; }
            set { objUserName = value; }
        }
    }
    public class clsGetExistingUserNameBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetExistingUserNameBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
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

        public long Id { get; set; }

        private clsUserVO objUserName = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains Login Details Which is Added.
        /// </summary>

        public clsUserVO UserName
        {
            get { return objUserName;}
            set { objUserName = value;}
        }
    }
    public class clsGetSecretQuestionBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetSecretQuestionBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
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

        public string LoginName { get; set; }

        private clsUserVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsUserVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }
    public class clsGetUserBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetUserBizAction";
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

        public long ID { get; set; }
        public int UserType { get; set; }
        public bool FlagDisableUser { get; set; }

        private clsUserVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsUserVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }
    
    public class clsAssignUserEMRTemplatesBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAssignUserEMRTemplatesBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

      

        public long UserID { get; set; }

        private List<clsUserEMRTemplateDetailsVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsUserEMRTemplateDetailsVO> Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }    
    public class clsGetUserEMRTemplateListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetUserEMRTemplateListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion



        public long UserID { get; set; }

        private List<clsUserEMRTemplateDetailsVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsUserEMRTemplateDetailsVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }
}
