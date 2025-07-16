using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddUpdateServiceMasterTariffBizActionVO : IBizActionValueObject
    {
        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }
        }

        private List<clsServiceTarrifClassRateDetailsNewVO> _objSelectedTariffClassList = null;
        public List<clsServiceTarrifClassRateDetailsNewVO> SelectedTariffClassList
        {
            get { return _objSelectedTariffClassList; }
            set { _objSelectedTariffClassList = value; }
        }
        private Int64 _SuccessStatus;
        public Int64 SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private string _ClassIDList;
        public string ClassIDList
        {
            get { return _ClassIDList; }
            set { _ClassIDList = value; }
        }

        private string _ClassRateList;
        public string ClassRateList
        {
            get { return _ClassRateList; }
            set { _ClassRateList = value; }
        }

        private string _TariffIDList;
        public string TariffIDList
        {
            get { return _TariffIDList; }
            set { _TariffIDList = value; }
        }
        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }

        public Boolean IsApplyToAllTariff { get; set; }
        public Boolean IsupdatePreviousRate { get; set; }

        public bool _IsModifyTariffClassRates;
        public bool IsModifyTariffClassRates
        {
            get { return _IsModifyTariffClassRates; }
            set { _IsModifyTariffClassRates = value; }
        }

        public bool _IsRemoveTariffClassRatesLink;
        public bool IsRemoveTariffClassRatesLink
        {
            get { return _IsRemoveTariffClassRatesLink; }
            set { _IsRemoveTariffClassRatesLink = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpdateServiceMasterTariffBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsAddUpdateDoctorServiceRateCategoryBizActionVO : IBizActionValueObject
    {
        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }
        }

        private List<clsServiceMasterVO> _objServiceMasterDetailsList = null;
        public List<clsServiceMasterVO> ServiceMasterDetailsList
        {
            get { return _objServiceMasterDetailsList; }
            set { _objServiceMasterDetailsList = value; }
        }

        private List<clsServiceMasterVO> _objDeletedServiceList = null;
        public List<clsServiceMasterVO> DeletedServiceList
        {
            get { return _objDeletedServiceList; }
            set { _objDeletedServiceList = value; }
        }

        private Int64 _SuccessStatus;
        public Int64 SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _CategoryID;
        public long CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        private bool _IsModify;
        public bool IsModify
        {
            get { return _IsModify; }
            set { _IsModify = value; }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpdateDoctorServiceRateCategoryBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsGetFrontPannelDataGridListBizActionVO : IBizActionValueObject
    {
        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }
        }

        private List<clsServiceMasterVO> _objServiceMasterDetailsList = null;
        public List<clsServiceMasterVO> ServiceMasterDetailsList
        {
            get { return _objServiceMasterDetailsList; }
            set { _objServiceMasterDetailsList = value; }
        }

        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        private Int64 _SuccessStatus;
        public Int64 SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _CategoryID;
        public long CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        private bool _IsModify;
        public bool IsModify
        {
            get { return _IsModify; }
            set { _IsModify = value; }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetFrontPannelDataGridListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsGetUnSelectedRecordForCategoryComboBoxBizActionVO : IBizActionValueObject
    {

        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterListForCombo = null;
        public List<MasterListItem> MasterListForCombo
        {
            get
            { return _MasterListForCombo; }

            set
            { _MasterListForCombo = value; }
        }

        private Int64 _SuccessStatus;
        public Int64 SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set { _DoctorID = value; }
        }

        private bool _IsFromDocSerLinling;
        public bool IsFromDocSerLinling
        {
            get { return _IsFromDocSerLinling; }
            set { _IsFromDocSerLinling = value; }
        }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetUnSelectedRecordForCategoryComboBoxBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    #region GST Details added by Ashish Z. on dated 24062017
    public class clsAddUpdateServiceTaxBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpdateServiceTaxBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        /// <summary>
        /// Gets or Sets 
        /// </summary>
        /// 
        private clsServiceTaxVO _ServiceTaxDetailsVO = null;
        public clsServiceTaxVO ServiceTaxDetailsVO
        {
            get { return _ServiceTaxDetailsVO; }
            set { _ServiceTaxDetailsVO = value; }
        }

        private List<clsServiceTaxVO> _ServiceTaxDetailsList = null;
        public List<clsServiceTaxVO> ServiceTaxDetailsVOList
        {
            get
            { return _ServiceTaxDetailsList; }

            set
            { _ServiceTaxDetailsList = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public int OperationType = 0; // 0- None ,1- Save , 2- Modify, 3- Delete
    }

    public class clsGetServiceTaxDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetServiceTaxDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        /// <summary>
        /// Gets or Sets 
        /// </summary>
        /// 
        private clsServiceTaxVO _ServiceTaxDetailsVO = null;
        public clsServiceTaxVO ServiceTaxDetailsVO
        {
            get { return _ServiceTaxDetailsVO; }
            set { _ServiceTaxDetailsVO = value; }
        }

        private List<clsServiceTaxVO> _ServiceTaxDetailsList = null;
        public List<clsServiceTaxVO> ServiceTaxDetailsVOList
        {
            get
            { return _ServiceTaxDetailsList; }

            set
            { _ServiceTaxDetailsList = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
    }
    #endregion
}
