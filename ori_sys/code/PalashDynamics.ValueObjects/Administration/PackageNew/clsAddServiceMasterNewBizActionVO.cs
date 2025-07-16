using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;

namespace PalashDynamics.ValueObjects.Administration.PackageNew
{
    public class clsAddServiceMasterNewBizActionVO : IBizActionValueObject
    {
        public clsAddServiceMasterNewBizActionVO()
        {

        }

        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }

        }

        private List<clsServiceItemMasterVO> _ServiceItemMasterDetails;
        public List<clsServiceItemMasterVO> ServiceItemMasterDetails
        {
            get
            {
                if (_ServiceItemMasterDetails == null)
                    _ServiceItemMasterDetails = new List<clsServiceItemMasterVO>();

                return _ServiceItemMasterDetails;
            }

            set
            {

                _ServiceItemMasterDetails = value;

            }
        }

        private List<clsServiceAgencyMasterVO> _ServiceAgencyMasterDetails;
        public List<clsServiceAgencyMasterVO> ServiceAgencyMasterDetails
        {
            get
            {
                if (_ServiceAgencyMasterDetails == null)
                    _ServiceAgencyMasterDetails = new List<clsServiceAgencyMasterVO>();

                return _ServiceAgencyMasterDetails;
            }
            set
            {
                _ServiceAgencyMasterDetails = value;
            }
        }

        private List<clsPackageRelationsVO> _FamilyMemberMasterDetails;
        public List<clsPackageRelationsVO> FamilyMemberMasterDetails
        {
            get
            {
                if (_FamilyMemberMasterDetails == null)
                    _FamilyMemberMasterDetails = new List<clsPackageRelationsVO>();

                return _FamilyMemberMasterDetails;
            }

            set
            {

                _FamilyMemberMasterDetails = value;

            }
        }

        private clsPackageRelationsVO _objPackageRelations;
        public clsPackageRelationsVO objPackageRelations
        {
            get
            {
                if (_objPackageRelations == null)
                    _objPackageRelations = new clsPackageRelationsVO();
                return _objPackageRelations;
            }
            set
            {
                _objPackageRelations = value;
            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        #region IBizActionValueObject Members
        public bool IsModify { get; set; }
        public bool IsNewServiceMaster { get; set; }
        public bool AppliToAllTariff { get; set; }
        public bool AddItems { get; set; }
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsAddServiceMasterNewBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }



        public bool UpdateServiceMasterStatus { get; set; }
        #endregion

    }

    public class clsGetServiceMasterListNewBizActionVO : IBizActionValueObject
    {
        private List<clsServiceMasterVO> _ServiceList;
        public List<clsServiceMasterVO> ServiceList
        {
            get { return _ServiceList; }
            set { _ServiceList = value; }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; }

        }

        private string _ServiceCode;
        public string ServiceCode
        {
            get { return _ServiceCode; }
            set { _ServiceCode = value; }

        }
        private long _Specialization;
        public long Specialization
        {
            get { return _Specialization; }
            set { _Specialization = value; }



        }

        public bool FromPackage { get; set; }
        private long _SubSpecialization;
        public long SubSpecialization
        {
            get { return _SubSpecialization; }
            set { _SubSpecialization = value; }



        }
        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }



        }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

        private bool _IsNewServiceMaster = false;
        public bool IsNewServiceMaster
        {
            get { return _IsNewServiceMaster; }
            set { _IsNewServiceMaster = value; }
        }
        public clsServiceMasterVO ServiceMaster { get; set; }

        public bool GetAllServiceListDetails { get; set; }
        public bool GetTariffServiceMasterID { get; set; }
        public bool GetAllTariffIDDetails { get; set; }
        public bool GetAllServiceClassRateDetails { get; set; }
        public bool GetAllServiceMasterDetailsForID { get; set; }
        public bool GetServiceItemsList { get; set; }

        private List<clsServiceItemMasterVO> _ServiceItemMasterDetails;
        public List<clsServiceItemMasterVO> ServiceItemMasterDetails
        {
            get
            {
                if (_ServiceItemMasterDetails == null)
                    _ServiceItemMasterDetails = new List<clsServiceItemMasterVO>();

                return _ServiceItemMasterDetails;
            }

            set
            {

                _ServiceItemMasterDetails = value;

            }
        }

        private List<clsServiceAgencyMasterVO> _ServiceAgencyMasterDetails;
        public List<clsServiceAgencyMasterVO> ServiceAgencyMasterDetails
        {
            get
            {
                if (_ServiceAgencyMasterDetails == null)
                    _ServiceAgencyMasterDetails = new List<clsServiceAgencyMasterVO>();

                return _ServiceAgencyMasterDetails;
            }
            set
            {
                _ServiceAgencyMasterDetails = value;
            }
        }

        private List<clsPackageRelationsVO> _FamilyMemberMasterDetails;
        public List<clsPackageRelationsVO> FamilyMemberMasterDetails
        {
            get
            {
                if (_FamilyMemberMasterDetails == null)
                    _FamilyMemberMasterDetails = new List<clsPackageRelationsVO>();

                return _FamilyMemberMasterDetails;
            }

            set
            {

                _FamilyMemberMasterDetails = value;

            }
        }

        private clsPackageRelationsVO _objPackageRelations;
        public clsPackageRelationsVO objPackageRelations
        {
            get
            {
                if (_objPackageRelations == null)
                    _objPackageRelations = new clsPackageRelationsVO();
                return _objPackageRelations;
            }
            set
            {
                _objPackageRelations = value;
            }
        }


        public bool IsStatus { get; set; }  // to check service status

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsGetServiceMasterListNewBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetPackageRelationsBizActionVO : IBizActionValueObject
    {

        private long _PackageServiceID;
        public long PackageServiceID
        {
            get { return _PackageServiceID; }
            set { _PackageServiceID = value; }
        }

        private long _PackageServiceUnitID;
        public long PackageServiceUnitID
        {
            get { return _PackageServiceUnitID; }
            set { _PackageServiceUnitID = value; }
        }

        private List<MasterListItem> _PackageRelationsList;
        public List<MasterListItem> PackageRelationsList
        {
            get { return _PackageRelationsList; }
            set { _PackageRelationsList = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsGetPackageRelationsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }

    public class clsAddPackageServiceNewBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsAddPackageServiceNewBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }



        private clsPackageServiceVO _Details;
        public clsPackageServiceVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        public List<clsServiceMasterVO> PackageSeviceItemsDetails = new List<clsServiceMasterVO>();

        public bool UpdatePackageStatus { get; set; }

        public bool UpdatePackageFreezeStatus { get; set; }

        public bool UpdatePackgeApproveStatus { get; set; }

        public bool IsSavePatientData { get; set; }


        public bool IsFixedRate { get; set; }

        private double _ServiceFixedRate;
        public double ServiceFixedRate
        {
            get { return _ServiceFixedRate; }
            set
            {
                if (_ServiceFixedRate != value)
                {
                    _ServiceFixedRate = value;
                   // OnPropertyChanged("ServiceFixedRate");
                }
            }
        }


        private double _PharmacyFixedRate;
        public double PharmacyFixedRate
        {
            get { return _PharmacyFixedRate; }
            set
            {
                if (_PharmacyFixedRate != value)
                {
                    _PharmacyFixedRate = value;
                   // OnPropertyChanged("PharmacyFixedRate");
                }
            }
        }

        private double _ServicePercentage;
        public double ServicePercentage
        {
            get { return _ServicePercentage; }
            set
            {
                if (_ServicePercentage != value)
                {
                    _ServicePercentage = value;
                    //OnPropertyChanged("ServicePercentage");
                }
            }
        }

        private double _PharmacyPercentage;
        public double PharmacyPercentage
        {
            get { return _PharmacyPercentage; }
            set
            {
                if (_PharmacyPercentage != value)
                {
                    _PharmacyPercentage = value;
                   // OnPropertyChanged("PharmacyPercentage");
                }
            }
        }

    }

    public class clsGetPackageServiceDetailsListNewBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsGetPackageServiceDetailsListNewBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }



        public long PackageID { get; set; }
        public long UnitId { get; set; }

        private clsPackageServiceVO _PackageMasterList;
        public clsPackageServiceVO PackageMasterList
        {
            get { return _PackageMasterList; }
            set { _PackageMasterList = value; }
        }

        private List<clsPackageServiceDetailsVO> _PackageDetailList;
        public List<clsPackageServiceDetailsVO> PackageDetailList
        {
            get { return _PackageDetailList; }
            set { _PackageDetailList = value; }
        }

        private List<clsPackageItemMasterVO> _ItemDetails;
        public List<clsPackageItemMasterVO> ItemDetails
        {
            get
            {
                if (_ItemDetails == null)
                    _ItemDetails = new List<clsPackageItemMasterVO>();

                return _ItemDetails;
            }
            set
            {
                _ItemDetails = value;
            }
        }

    }

    public class clsGetPackageServicesAndRelationsNewBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsGetPackageServicesAndRelationsNewBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }



        public long PackageID { get; set; }
        public long UnitId { get; set; }
        public long ServiceID { get; set; }

        private List<clsPackageServiceConditionsVO> _ServiceConditionList;
        public List<clsPackageServiceConditionsVO> ServiceConditionList
        {
            get { return _ServiceConditionList; }
            set { _ServiceConditionList = value; }
        }

        private List<clsPackageServiceRelationsVO> _PackageServiceRelationList;
        public List<clsPackageServiceRelationsVO> PackageServiceRelationList
        {
            get { return _PackageServiceRelationList; }
            set { _PackageServiceRelationList = value; }
        }


    }

    public class clsAddPackagePharmacyItemsNewBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsAddPackagePharmacyItemsNewBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsPackageItemMasterVO> _ItemDetails;
        public List<clsPackageItemMasterVO> ItemDetails
        {
            get
            {
                if (_ItemDetails == null)
                    _ItemDetails = new List<clsPackageItemMasterVO>();

                return _ItemDetails;
            }
            set
            {
                _ItemDetails = value;
            }
        }

        public long PackageID { get; set; }

        public long PackageUnitID { get; set; }

        public bool ApplicableToAll { get; set; }
        public double ApplicableToAllDiscount { get; set; }
        public double TotalBudget { get; set; }

    }

    public class clsGetPackagePharmacyItemListNewBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsGetPackagePharmacyItemListNewBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }



        public long PackageID { get; set; }
        public long UnitId { get; set; }

        private List<clsPackageItemMasterVO> _ItemDetails;
        public List<clsPackageItemMasterVO> ItemDetails
        {
            get
            {
                if (_ItemDetails == null)
                    _ItemDetails = new List<clsPackageItemMasterVO>();

                return _ItemDetails;
            }
            set
            {
                _ItemDetails = value;
            }
        }

    }

    public class clsDeletePackageServiceDetilsListNewBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsDeletePackageServiceDetilsListNewBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long PackageID { get; set; }

        public long PackageUnitID { get; set; }

        public long ServiceID { get; set; }

        public long SpecilizationID { get; set; }

        public bool IsSpecilizationGroup { get; set; }

        public long UnitID { get; set; }

        public bool Status { get; set; }

        public bool IsDeletePatientData { get; set; }

    }

    public class clsGetPackageConditionalServicesNewBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsGetPackageConditionalServicesNewBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }



        public long TariffID { get; set; }
        public long ServiceID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        //Added By CDS
        public long PackageID { get; set; }
        //END
        public long SponsorID { get; set; }
        public long SponsorUnitID { get; set; }

        public DateTime? PatientDateOfBirth { get; set; }
        public long MemberRelationID { get; set; }

        //private List<clsPackageServiceConditionsVO> _ServiceConditionList;
        //public List<clsPackageServiceConditionsVO> ServiceConditionList
        //{
        //    get { return _ServiceConditionList; }
        //    set { _ServiceConditionList = value; }
        //}

        private List<clsServiceMasterVO> _ServiceConditionList;
        public List<clsServiceMasterVO> ServiceConditionList
        {
            get { return _ServiceConditionList; }
            set { _ServiceConditionList = value; }
        }


    }

    public class clsGetPackageRelationListBizActionVO : IBizActionValueObject
    {

        private long _PackageTariffID;
        public long PackageTariffID
        {
            get { return _PackageTariffID; }
            set { _PackageTariffID = value; }
        }

        //private long _PackageServiceUnitID;
        //public long PackageServiceUnitID
        //{
        //    get { return _PackageServiceUnitID; }
        //    set { _PackageServiceUnitID = value; }
        //}

        private List<MasterListItem> _PackageRelationsList;
        public List<MasterListItem> PackageRelationsList
        {
            get { return _PackageRelationsList; }
            set { _PackageRelationsList = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsGetPackageRelationListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }

    public class clsDeletePackageItemDetilsListNewBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsDeletePackageItemDetilsListNewBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long PackageID { get; set; }

        public long PackageUnitID { get; set; }

        public long ItemID { get; set; }

        public long ItemGroupID { get; set; }

        public long ItemCategoryID { get; set; }

        public bool IsCategory { get; set; }

        public bool IsGroup { get; set; }

        public long UnitID { get; set; }

        public bool Status { get; set; }

        public bool IsDeletePatientData { get; set; }

    }

    //added by neena
    public class clsAddPackageConsentLinkBizActionVO : IBizActionValueObject
    {
        public clsAddPackageConsentLinkBizActionVO()
        {

        }

        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }

        }

        private List<clsServiceMasterVO> _ServiceItemMasterDetails;
        public List<clsServiceMasterVO> ServiceItemMasterDetails
        {
            get
            {
                if (_ServiceItemMasterDetails == null)
                    _ServiceItemMasterDetails = new List<clsServiceMasterVO>();

                return _ServiceItemMasterDetails;
            }

            set
            {

                _ServiceItemMasterDetails = value;

            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        #region IBizActionValueObject Members
        public bool IsModify { get; set; }
        public bool IsNewServiceMaster { get; set; }
        public bool AppliToAllTariff { get; set; }
        public bool AddItems { get; set; }
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsAddPackageConsentLinkBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }



        public bool UpdateServiceMasterStatus { get; set; }
        #endregion

    }

    public class clsGetPackageConsentLinkBizActionVO : IBizActionValueObject
    {
        public clsGetPackageConsentLinkBizActionVO()
        {

        }

        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }

        }

        private List<clsServiceMasterVO> _ServiceItemMasterDetails;
        public List<clsServiceMasterVO> ServiceItemMasterDetails
        {
            get
            {
                if (_ServiceItemMasterDetails == null)
                    _ServiceItemMasterDetails = new List<clsServiceMasterVO>();

                return _ServiceItemMasterDetails;
            }

            set
            {

                _ServiceItemMasterDetails = value;

            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        #region IBizActionValueObject Members
        public bool IsModify { get; set; }
        public bool IsNewServiceMaster { get; set; }
        public bool AppliToAllTariff { get; set; }
        public bool AddItems { get; set; }
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsGetPackageConsentLinkBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }



        public bool UpdateServiceMasterStatus { get; set; }
        #endregion

    }
    //
}
