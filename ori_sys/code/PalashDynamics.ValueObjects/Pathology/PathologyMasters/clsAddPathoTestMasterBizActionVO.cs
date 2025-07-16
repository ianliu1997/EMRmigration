using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.ValueObjects.Pathology.PathologyMasters
{
    public class clsAddPathoTestMasterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPathoTestMasterBizAction";
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

        private clsPathoTestMasterVO objDetails = null;
        public clsPathoTestMasterVO TestDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        public bool IsUpdate { get; set; }
    }

    public class clsGetPathoTestDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsPathoTestMasterVO> objPathoTestList = null;
        public List<clsPathoTestMasterVO> TestList
        {
            get { return objPathoTestList; }
            set { objPathoTestList = value; }
        }

        public string Description { get; set; }
        public long Category { get; set; }
        public long ServiceID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string SortExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoTestDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetPathoTestListForResultEntryBizActionVO : IBizActionValueObject
    {
        private List<clsPathoTestMasterVO> objPathoTestList = null;
        public List<clsPathoTestMasterVO> TestList
        {
            get { return objPathoTestList; }
            set { objPathoTestList = value; }
        }

        public string Description { get; set; }
        public long Category { get; set; }
        public long ServiceID { get; set; }
        public Int16 ApplicaleTo { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoTestListForResultEnrtyBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }

    public class clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO : IBizActionValueObject
    {
        private List<clsPathoTestParameterVO> objParameterListList = null;
        public List<clsPathoTestParameterVO> ParameterList
        {
            get { return objParameterListList; }
            set { objParameterListList = value; }
        }

        private List<clsPathoSubTestVO> objSubTestList = null;
        public List<clsPathoSubTestVO> SubTestList
        {
            get { return objSubTestList; }
            set { objSubTestList = value; }
        }

        private List<clsPathoTestSampleVO> objSampleList = null;
        public List<clsPathoTestSampleVO> SampleList
        {
            get { return objSampleList; }
            set { objSampleList = value; }
        }

        private List<clsPathoTestItemDetailsVO> objItemList = null;
        public List<clsPathoTestItemDetailsVO> ItemList
        {
            get { return objItemList; }
            set { objItemList = value; }
        }


        public long TestID { get; set; }
        public bool IsFormSubTest { get; set; }


        private List<clsPathoTemplateVO> objTemplateList = null;
        public List<clsPathoTemplateVO> TemplateList
        {
            get { return objTemplateList; }
            set { objTemplateList = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoParameterSampleAndItemDetailsByTestIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }



    public class clsGetPathoSubTestMasterBizActionVO : IBizActionValueObject
    {
        private List<clsPathoTestMasterVO> objPathoTestList = null;
        public List<clsPathoTestMasterVO> TestList
        {
            get { return objPathoTestList; }
            set { objPathoTestList = value; }
        }

        public string Description { get; set; }
        public long Category { get; set; }
        public long ServiceID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string SortExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoSubTestMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    public class clsGetPathoProfileServiceIDForPathoTestMasterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Pathology.PathologyMaster.clsGetPathoProfileServiceIDForPathoTestMasterBizAction";
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

        private List<clsPathoTestMasterVO> objDetails = null;
        public List<clsPathoTestMasterVO> ServiceDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


    }
    //public class clsGetPathoParameterUnitsByParameterIDBizActionVO : IBizActionValueObject
    //{
    //    private List<clsPathoTestParameterVO> objUnitListList = null;
    //    public List<clsPathoTestParameterVO> UnitList
    //    {
    //        get { return objUnitListList; }
    //        set { objUnitListList = value; }
    //    }

    //    private List<clsPathoSubTestVO> objSubTestList = null;
    //    public List<clsPathoSubTestVO> SubTestList
    //    {
    //        get { return objSubTestList; }
    //        set { objSubTestList = value; }
    //    }

    //    private List<clsPathoTestSampleVO> objSampleList = null;
    //    public List<clsPathoTestSampleVO> SampleList
    //    {
    //        get { return objSampleList; }
    //        set { objSampleList = value; }
    //    }

    //    private List<clsPathoTestItemDetailsVO> objItemList = null;
    //    public List<clsPathoTestItemDetailsVO> ItemList
    //    {
    //        get { return objItemList; }
    //        set { objItemList = value; }
    //    }


    //    public long TestID { get; set; }
    //    public long ParamID { get; set; }




    ////    #region IBizActionValueObject Members

    //    public string GetBizAction()
    //    {
    //        return "PalashDynamics.BusinessLayer.clsGetPathoParameterUnitsByParamIDBizAction";
    //    }

    //    #endregion

        //#region IValueObject Members

        //public string ToXml()
        //{
        //    return this.ToString();
        //}

        //#endregion
    //}

    public class clsGetPathoParameterUnitsByParamIDBizActionVO : IBizActionValueObject
    {
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
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
        public KeyValue Parent { get; set; }

        

        private List<clsPathoParameterUnitMaterVO> objSampleList = null;
        public List<clsPathoParameterUnitMaterVO> SampleList
        {
            get { return objSampleList; }
            set { objSampleList = value; }
        }

       


       // public long TestID { get; set; }
      public long ParamID { get; set; }




        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoParameterUnitsByParamIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    public class clsAddPathoTemplateMasterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPathoTemplateMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private long _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public long SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsPathoTestTemplateDetailsVO objDetails = null;
        public clsPathoTestTemplateDetailsVO TemplateDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
        public bool _IsFromCardiologyMaster = false;
        public bool IsFromCardiologyMaster
        {
            get { return _IsFromCardiologyMaster; }
            set { _IsFromCardiologyMaster = value; }
        }
        public bool IsStatusChanged { get; set; }
        public bool IsModifyStatus = false;


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
    }

    public class clsGetPathoTemplateMasterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPathoTemplateMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private long _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public long SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public long GenderID { get; set; }    

        private List<clsPathoTestTemplateDetailsVO> objDetails = new List<clsPathoTestTemplateDetailsVO>();
        public List<clsPathoTestTemplateDetailsVO> TemplateDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
       
        public string Description { get; set; }
        public long Pathologist { get; set; }
        public long TemplateID { get; set; }
        public bool _IsFromCardiologyMaster = false;
        public bool IsFromCardiologyMaster
        {
            get { return _IsFromCardiologyMaster; }
            set { _IsFromCardiologyMaster = value; }
        }



        private string _SearchExpression;
        public string SearchExpression
        {
            get { return _SearchExpression; }
            set { _SearchExpression = value; }
        }


        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }


    }

    public class clsGetPathoTemplateGenderBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPathoTemplateGenderBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private long _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public long SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<MasterListItem> objGenderDetails = new List<MasterListItem>();
        public List<MasterListItem> GenderDetails
        {
            get { return objGenderDetails; }
            set { objGenderDetails = value; }
        }
        public string Description { get; set; }
        public long TemplateID { get; set; }
        public long GenderID { get; set; }    

        private string _SearchExpression;
        public string SearchExpression
        {
            get { return _SearchExpression; }
            set { _SearchExpression = value; }
        }      


    }

    //added by rohini dated 19.1.16
    public class clsAddMachineToTestbizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject



        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddMachineToTestbizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion



        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsPathoTestMasterVO objItemMaster = null;
        public clsPathoTestMasterVO ItemMatserDetails
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }


        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }


        public List<clsPathoTestMasterVO> ItemList { get; set; }

        public clsPathoTestMasterVO ItemSupplier { get; set; }
        public List<clsPathoTestMasterVO> ItemSupplierList { get; set; }
    }
    public class clsGetMachineToTestBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetMachineToTestBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
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

        private Boolean _CheckForTaxExistatnce;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public Boolean CheckForTaxExistatnce
        {
            get { return _CheckForTaxExistatnce; }
            set { _CheckForTaxExistatnce = value; }
        }

        private Boolean _IsTaxAdded;
        public Boolean IsTaxAdded
        {
            get { return _IsTaxAdded; }
            set { _IsTaxAdded = value; }
        }
        private clsPathoTestMasterVO objItemMater = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPathoTestMasterVO ItemDetails
        {
            get { return objItemMater; }
            set { objItemMater = value; }
        }

        public clsPathoTestMasterVO ItemSupplier { get; set; }
        public List<clsPathoTestMasterVO> ItemSupplierList { get; set; }
        public List<clsPathoTestMasterVO> ItemList { get; set; }
    }


    //for sun test
    public class clsAddMachineToSubTestbizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject



        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddMachineToSubTestbizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion



        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsPathoTestMasterVO objItemMaster = null;
        public clsPathoTestMasterVO ItemMatserDetails
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }


        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }


        public List<clsPathoTestMasterVO> ItemList { get; set; }

        public clsPathoTestMasterVO ItemSupplier { get; set; }
        public List<clsPathoTestMasterVO> ItemSupplierList { get; set; }
    }
    public class clsGetMachineToSubTestBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetMachineToSubTestBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
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

        private Boolean _CheckForTaxExistatnce;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public Boolean CheckForTaxExistatnce
        {
            get { return _CheckForTaxExistatnce; }
            set { _CheckForTaxExistatnce = value; }
        }

        private Boolean _IsTaxAdded;
        public Boolean IsTaxAdded
        {
            get { return _IsTaxAdded; }
            set { _IsTaxAdded = value; }
        }
        private clsPathoTestMasterVO objItemMater = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPathoTestMasterVO ItemDetails
        {
            get { return objItemMater; }
            set { objItemMater = value; }
        }

        public clsPathoTestMasterVO ItemSupplier { get; set; }
        public List<clsPathoTestMasterVO> ItemSupplierList { get; set; }
        public List<clsPathoTestMasterVO> ItemList { get; set; }
    }

    //added by rohini dated 11.3.16 for pathologist to template
    public class clsAddPathologistToTempbizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject



        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPathologistToTempbizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion



        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsPathoTestTemplateDetailsVO objItemMaster = null;
        public clsPathoTestTemplateDetailsVO ItemMatserDetails
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }


        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }


        public List<clsPathoTestTemplateDetailsVO> ItemList { get; set; }

        public clsPathoTestTemplateDetailsVO ItemSupplier { get; set; }
        public List<clsPathoTestTemplateDetailsVO> ItemSupplierList { get; set; }
    }

    public class clsGetPathologistToTempBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathologistToTempBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
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

        private Boolean _CheckForTaxExistatnce;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public Boolean CheckForTaxExistatnce
        {
            get { return _CheckForTaxExistatnce; }
            set { _CheckForTaxExistatnce = value; }
        }

        private Boolean _IsTaxAdded;
        public Boolean IsTaxAdded
        {
            get { return _IsTaxAdded; }
            set { _IsTaxAdded = value; }
        }
        private clsPathoTestTemplateDetailsVO objItemMater = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPathoTestTemplateDetailsVO ItemDetails
        {
            get { return objItemMater; }
            set { objItemMater = value; }
        }

        public clsPathoTestTemplateDetailsVO ItemSupplier { get; set; }
        public List<clsPathoTestTemplateDetailsVO> ItemSupplierList { get; set; }
        public List<clsPathoTestTemplateDetailsVO> ItemList { get; set; }
    }
    //by rohini dated 20.1.16
    public class clsGetParameterOrSubTestSearchBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetParameterOrSubTestSearchBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        private int _SuccessStatus;
      
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsPathoTestMasterVO objItemMater = null;
   
        public clsPathoTestMasterVO ParameterDetails
        {
            get { return objItemMater; }
            set { objItemMater = value; }
        }
        public int Flag { get; set; }
        public string Description { get; set; }
        public clsPathoTestMasterVO Parameter { get; set; }
        public List<clsPathoTestMasterVO> ParameterList { get; set; }
    
    }
    public class clsGetWordOrReportTemplateBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetWordOrReportTemplateBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        private int _SuccessStatus;

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsPathoTestTemplateDetailsVO> objDetails = new List<clsPathoTestTemplateDetailsVO>();
        public List<clsPathoTestTemplateDetailsVO> TemplateDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
        public int Flag { get; set; }
        public string Description { get; set; }
        public clsPathoTemplateVO Template { get; set; }
        public List<clsPathoTemplateVO> TemplateList { get; set; }

    }

    //added by rohini dated 17.2.16
    //public class clsAddServiceToParameterbizActionVO : IBizActionValueObject
    //{
    //    #region  IBizActionValueObject
    //    public string GetBizAction()
    //    {
    //        return "PalashDynamics.BusinessLayer.clsAddServiceToParameterbizAction";
    //    }
    //    public string ToXml()
    //    {
    //        return this.ToXml();
    //    }
    //    #endregion
    //    /// <summary>
    //    /// This property contains Item master details.
    //    /// </summary>
    //    private clsPathoParameterDefaultValueMasterVO objItemMaster = null;
    //    public clsPathoParameterDefaultValueMasterVO ItemMatserDetails
    //    {
    //        get
    //        {
    //            return objItemMaster;
    //        }
    //        set
    //        {
    //            objItemMaster = value;

    //        }
    //    }
    //    /// <summary>
    //    ///  Output Property.
    //    /// This property states the outcome of BizAction Process.
    //    /// </summary>
    //    private int _SuccessStatus;
    //    public int SuccessStatus
    //    {
    //        get
    //        {
    //            return _SuccessStatus;
    //        }
    //        set
    //        {
    //            _SuccessStatus = value;
    //        }
    //    }
    //    public List<clsPathoParameterDefaultValueMasterVO> ItemList { get; set; }
    //    public clsPathoParameterDefaultValueMasterVO ItemSupplier { get; set; }
    //    public List<clsPathoParameterDefaultValueMasterVO> ItemSupplierList { get; set; }
    //}
    //public class clsGetServicesToParameterBizActionVO : IBizActionValueObject
    //{
    //    public string GetBizAction()
    //    {
    //        return "PalashDynamics.BusinessLayer.clsGetServicesToParameterBizAction";
    //    }

    //    public string ToXml()
    //    {
    //        return this.ToXml();
    //    }
    //    private int _SuccessStatus;
    //    /// <summary>
    //    /// Output Property.
    //    /// This property states the outcome of BizAction Process.
    //    /// </summary>
    //    public int SuccessStatus
    //    {
    //        get { return _SuccessStatus; }
    //        set { _SuccessStatus = value; }
    //    }

    //    private Boolean _CheckForTaxExistatnce;
    //    /// <summary>
    //    /// Output Property.
    //    /// This property states the outcome of BizAction Process.
    //    /// </summary>
    //    public Boolean CheckForTaxExistatnce
    //    {
    //        get { return _CheckForTaxExistatnce; }
    //        set { _CheckForTaxExistatnce = value; }
    //    }

    //    private Boolean _IsTaxAdded;
    //    public Boolean IsTaxAdded
    //    {
    //        get { return _IsTaxAdded; }
    //        set { _IsTaxAdded = value; }
    //    }
    //    private clsPathoParameterDefaultValueMasterVO objItemMater = null;
    //    /// <summary>
    //    /// Output Property.
    //    /// This Property Contains OPDPatient Details Which is Added.
    //    /// </summary>
    //    public clsPathoParameterDefaultValueMasterVO ItemDetails
    //    {
    //        get { return objItemMater; }
    //        set { objItemMater = value; }
    //    }

    //    public clsPathoParameterDefaultValueMasterVO ItemSupplier { get; set; }
    //    public List<MasterListItem> ItemSupplierList { get; set; }
    //    public List<clsPathoParameterDefaultValueMasterVO> ItemList { get; set; }
    //}
}
