using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsGetServiceMasterListBizActionVO : IBizActionValueObject
    {
        
        private bool _IsOLDServiceMaster = false;
        public bool IsOLDServiceMaster
        {
            get { return _IsOLDServiceMaster; }
            set { _IsOLDServiceMaster = value; }
        }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }
        public bool GetAllServiceListDetails { get; set; }
        public bool GetTariffServiceMasterID { get; set; }
        public bool GetAllTariffIDDetails { get; set; }
        public bool GetAllServiceClassRateDetails { get; set; }
        public bool GetAllServiceMasterDetailsForID { get; set; }
        public bool GetServicesForTariffMaster { get; set; }

        public bool GetServicesForPathology { get; set; }

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

        private List<MasterListItem> _MasterList;
        public List<MasterListItem> MasterList
        {
            get { return _MasterList; }
            set { _MasterList = value; }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }



        }

        private string _ServiceCode;
        public string ServiceCode
        {
            get { return _ServiceCode; }
            set { _ServiceCode = value; }
        }

        public clsServiceMasterVO ServiceMaster { get; set; }

        public bool IsStatus { get; set; }  // to check service status

        public bool IsFromDocSerRateCat;  // IsFromDocSerRateCat=true set from frmDoctorServiceRateCategory.xaml.cs form.

        public bool IsFromPackage = false;  // Only In Case From PackageMaster
        
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetServiceMasterListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetServiceListByUnitIDBizActionVO : IBizActionValueObject
    {
        public bool IsPagingEnabled { get; set; }

        public bool IsCheckServiceValidation { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public long TariffServiceID { get; set; }

        public string SearchExpression { get; set; }

        public bool IsGetServicePackage { get; set; }

        public string IsPackage { get; set; }
        public bool IsBasePackage { get; set; }
        public bool IsBaseHealthPlan { get; set; }
        public int TotalRows { get; set; }

        public bool GetAllServiceListDetails { get; set; }
        public bool GetTariffServiceMasterID { get; set; }
        public bool GetAllTariffIDDetails { get; set; }
        public bool GetAllServiceClassRateDetails { get; set; }
        public bool GetAllServiceMasterDetailsForID { get; set; }
        public bool GetServicesForTariffMaster { get; set; }

        private List<clsServiceMasterVO> _TariffServiceMasterList;
        public List<clsServiceMasterVO> TariffServiceMasterList
        {
            get { return _TariffServiceMasterList; }
            set
            {
                _TariffServiceMasterList = value;
            }
        }

        private List<clsServiceMasterVO> _ServiceList;
        public List<clsServiceMasterVO> ServiceList
        {
            get { return _ServiceList; }
            set { _ServiceList = value; }
        }

        private List<MasterListItem> _MasterList;
        public List<MasterListItem> MasterList
        {
            get { return _MasterList; }
            set { _MasterList = value; }
        }

        public long ClassID { get; set; }

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

        private long _RadSpecialization;
        public long RadSpecialization
        {
            get { return _RadSpecialization; }
            set { _RadSpecialization = value; }
        }

        private long _PathoSpecialization;
        public long PathoSpecialization
        {
            get { return _PathoSpecialization; }
            set { _PathoSpecialization = value; }
        }

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

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }

        public clsServiceMasterVO ServiceMaster { get; set; }

       
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetServiceListByUnitIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    //Added for Only IPD by CDS

    public class clsGetServiceWiseClassRateBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public List<clsServiceMasterVO> ServiceClassList { get; set; }
        public long ServiceID { get; set; }
        public long UnitID { get; set; }
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetServiceWiseClassRateBizAction";
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
   
