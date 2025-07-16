using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.PackageNew
{
    public class clsAddPackageSourceTariffCompanyRelationsBizActionVO : IBizActionValueObject
    {
        private clsPackageSourceRelationVO _PackageSourceRelation;
        public clsPackageSourceRelationVO PackageSourceRelation
        {
            get
            {
                if (_PackageSourceRelation == null)
                    _PackageSourceRelation = new clsPackageSourceRelationVO();

                return _PackageSourceRelation;
            }

            set
            {
                _PackageSourceRelation = value;
            }
        }

        private long _ResultSuccessStatus;
        public long ResultSuccessStatus
        {
            get { return _ResultSuccessStatus; }
            set { _ResultSuccessStatus = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsAddPackageSourceTariffCompanyRelationsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetPackageSourceTariffCompanyListBizActionVO : IBizActionValueObject
    {
        private List<clsPackageSourceRelationVO> _PackageLinkingDetails;
        public List<clsPackageSourceRelationVO> PackageLinkingDetails
        {
            get { return _PackageLinkingDetails; }
            set { _PackageLinkingDetails = value; }
        }

        public long tariffID { get; set; }
        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }

        public string SearchExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsGetPackageSourceTariffCompanyListBizAction";
        }

        #endregion

        public string ToXml()
        {
            return this.ToString();
        }


    }

    public class clsAddPackageRateClinicWiseBizActionVO : IBizActionValueObject
    {
        public long tariffID { get; set; }
        public long PackageID { get; set; }
        public long PackageServiceID { get; set; }

        private List<clsPackageRateClinicWiseVO> _PackageRateClinicWiseDetails;
        public List<clsPackageRateClinicWiseVO> PackageRateClinicWiseList
        {
            get { return _PackageRateClinicWiseDetails; }
            set { _PackageRateClinicWiseDetails = value; }
        }

        private clsPackageRateClinicWiseVO _PackageRateClinicWise;
        public clsPackageRateClinicWiseVO PackageRateClinicWise
        {
            get
            {
                if (_PackageRateClinicWise == null)
                    _PackageRateClinicWise = new clsPackageRateClinicWiseVO();

                return _PackageRateClinicWise;
            }

            set
            {
                _PackageRateClinicWise = value;
            }
        }

        private long _ResultSuccessStatus;
        public long ResultSuccessStatus
        {
            get { return _ResultSuccessStatus; }
            set { _ResultSuccessStatus = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsAddPackageRateClinicWiseBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetPackageRateClinicWiseBizActionVO : IBizActionValueObject
    {
        private List<clsPackageRateClinicWiseVO> _PackageRateClinicWiseList;
        public List<clsPackageRateClinicWiseVO> PackageRateClinicWiseList
        {
            get { return _PackageRateClinicWiseList; }
            set { _PackageRateClinicWiseList = value; }
        }

        public long tariffID { get; set; }
        public long PackageID { get; set; }
        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }


        public string sortExpression { get; set; }

        public string SearchExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsGetPackageRateClinicWiseBizAction";
        }

        #endregion

        public string ToXml()
        {
            return this.ToString();
        }


    }

}
