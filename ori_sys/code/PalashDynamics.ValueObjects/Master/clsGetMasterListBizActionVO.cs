using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsGetMasterListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetMasterListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        private List<MasterListItem> objGenderDetails = new List<MasterListItem>();
        public List<MasterListItem> GenderDetails
        {
            get { return objGenderDetails; }
            set { objGenderDetails = value; }
        }

        public clsGetMasterListBizActionVO()
        {

        }
        private MasterTableNameList _MasterTable = MasterTableNameList.None;
        public MasterTableNameList MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }

        public KeyValue Category { get; set; }

        private string _Error = "";
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public KeyValue Parent { get; set; }

        /// <summary>
        /// Datatype: Boolean
        /// Gets or Sets active record from list
        /// </summary>
        public bool? IsActive { get; set; }       

        public bool _IsFromPOGRN = false;
        public bool IsFromPOGRN
        {
            get { return _IsFromPOGRN; }
            set { _IsFromPOGRN = value; }
        }

       
        /// <summary>
        /// Datatype: Boolean
        /// Gets or Sets IsDate 
        /// </summary>
        public bool? IsDate { get; set; }
       
        public bool IsParameterSearch = false;
        public string parametername = string.Empty;
        public bool IsGST { get; set; }
    }



   



    public class clsGetSearchMasterListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetSearchMasterListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsGetSearchMasterListBizActionVO()
        {

        }
        private MasterTableNameList _MasterTable = MasterTableNameList.None;
        public MasterTableNameList MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }


        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
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

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }


        public string Code { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }


    }

    public class KeyValue
    {
        public object Key { get; set; }
        public string Value { get; set; }
    }

    public class clsGetAutoCompleteListVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetAutoCompleteList";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private string _TableName = "";
        public string TableName
        {
            get
            {
                return _TableName;
            }
            set
            {
                _TableName = value;
            }
        }


        private string _ColumnName = "";
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                _ColumnName = value;
            }
        }

        public bool IsDecode { get; set; }
        public KeyValue Parent { get; set; }

        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<string> _List = null;
        public List<string> List
        {
            get
            { return _List; }

            set
            { _List = value; }
        }

    }

    // BY BHUSHAN . . .
    public class clsGetAutoCompleteListVO_2Colums : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetAutoCompleteList_2Colums";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private string _TableName = "";
        public string TableName
        {
            get
            {
                return _TableName;
            }
            set
            {
                _TableName = value;
            }
        }


        private string _ColumnName = "";
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                _ColumnName = value;
            }
        }

        private string _ColumnName2 = "";
        public string ColumnName2
        {
            get
            {
                return _ColumnName2;
            }
            set
            {
                _ColumnName2 = value;
            }
        }

        public bool IsDecode { get; set; }
        public KeyValue Parent { get; set; }

        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<string> _List = null;
        public List<string> List
        {
            get
            { return _List; }

            set
            { _List = value; }
        }

    }

    public class clsGetUserDashBoardVO : IBizActionValueObject
    {
        //This is to get the dash board assigned to the respective user.

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetUserDashBoard";
        }
        #endregion
        public string ToXml()
        {
            return this.ToString();
        }

        public long ID { get; set; }
        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<clsDashBoardVO> _List = null;
        public List<clsDashBoardVO> List
        {
            get
            { return _List; }

            set
            { _List = value; }
        }
    }
    public class clsGetDashBoardListVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDashBoardList";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long ID { get; set; }
        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<clsDashBoardVO> _List = null;
        public List<clsDashBoardVO> List
        {
            get
            { return _List; }

            set
            { _List = value; }
        }

    }

    public class clsGetMasterListByTableNameBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetMasterListByTableNameBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsGetMasterListByTableNameBizActionVO()
        {

        }
        private string _MasterTable = "";
        public string MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }

        private string _Error = "";
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public KeyValue Parent { get; set; }

        /// <summary>
        /// Datatype: Boolean
        /// Gets or Sets active record from list
        /// </summary>
        public bool? IsActive { get; set; }
        /// <summary>
        /// Datatype: Boolean
        /// Gets or Sets IsDate 
        /// </summary>
        public bool? IsDate { get; set; }
    }

    // Added By Harish
    // Date 1 Aug 2011
    // For Dynamic get Master list from Any Master Table with Dynamic Column Names
    public class clsGetMasterListByTableNameAndColumnNameBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetMasterListByTableNameAndColumnNameBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsGetMasterListByTableNameAndColumnNameBizActionVO()
        {

        }
        private string _MasterTable = "";
        public string MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }

        private string _ColumnName = "";
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                _ColumnName = value;
            }
        }

        private string _Error = "";
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }


    }

    public class clsGetColumnListByTableNameBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetColumnListByTableNameBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsGetColumnListByTableNameBizActionVO()
        {

        }
        private string _MasterTable = "";
        public string MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }

        private string _Error = "";
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _ColumnList = null;
        public List<MasterListItem> ColumnList
        {
            get
            { return _ColumnList; }

            set
            { _ColumnList = value; }
        }
    }

    #region For IPD Module

    //By Anjali............ on 03/04/2014
    public class clsGetMasterListConsentBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetMasterListConsentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsGetMasterListConsentBizActionVO()
        {

        }
        private MasterTableNameList _MasterTable = MasterTableNameList.None;
        public MasterTableNameList MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }

        private string _Error = "";
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        private string _FilterExpression;
        public string FilterExpression
        {
            get { return _FilterExpression; }
            set { _FilterExpression = value; }
        }
        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public KeyValue Parent { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDate { get; set; }
        private bool _IsSubTest = false;
        public bool IsSubTest
        {
            get { return _IsSubTest; }
            set { _IsSubTest = value; }
        }
    }

    #endregion


    //added by rohini dated 4/2/16

    public class clsGetPathoFastingBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetPathoFastingBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsGetPathoFastingBizActionVO()
        {

        }
        private MasterTableNameList _MasterTable = MasterTableNameList.None;
        public MasterTableNameList MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }


        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
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

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }


        public long id { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public bool? IsInHrs { get; set; }


    }

    public class clsGetMarketingExecutivesListVO : IBizActionValueObject
    {
         #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetMarketingExecutivesListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsGetMarketingExecutivesListVO()
        {

        }

        private List<MasterListItem> _MarketingExecutivesList = null;

        public List<MasterListItem> MarketingExecutivesList
        {
            get { return _MarketingExecutivesList;}
            set { _MarketingExecutivesList = value; }
        }

        public bool IsMarketingExecutives { get; set; }
        public long UnitID;
    }
}
