using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;

namespace PalashDynamics.ValueObjects
{
    public class clsGetPackageServiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPackageServiceBizAction";
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



        private List<MasterListItem> _MasterList;
        public List<MasterListItem> MasterList
        {
            get { return _MasterList; }
            set { _MasterList = value; }
        }

    }

    public class clsAddPackageServiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPackageServiceBizAction";
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

    }


    public class clsGetPackageServiceListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPackageServiceListBizAction";
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


        public long UnitId { get; set; }
        public long ServiceID { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string sortExpression { get; set; }

        private List<clsPackageServiceVO> _PackageList;
        public List<clsPackageServiceVO> PackageList
        {
            get { return _PackageList; }
            set { _PackageList = value; }
        }

    }

    public class clsGetPackageServiceDetailsListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPackageServiceDetailsListBizAction";
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

        private List<clsPackageServiceDetailsVO> _PackageDetailList;
        public List<clsPackageServiceDetailsVO> PackageDetailList
        {
            get { return _PackageDetailList; }
            set { _PackageDetailList = value; }
        }

    }

    public class clsGetPackageServiceFromServiceIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPackageServiceFromServiceIDBizAction";
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



        public long ServiceID { get; set; }

        private List<clsPackageServiceDetailsVO> _PackageDetailList;
        public List<clsPackageServiceDetailsVO> PackageDetailList
        {
            get { return _PackageDetailList; }
            set { _PackageDetailList = value; }
        }

    }

    #region For IPD Module

    public class clsGetPackageServiceForBillBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPackageServiceForBillBizAction";
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

        public bool ViewPackge { get; set; }

        public long ServiceID { get; set; }
        public long TariffID { get; set; }
        public long TariffServiceID { get; set; }

        private List<clsPackageServiceDetailsVO> _PackageDetailList;
        public List<clsPackageServiceDetailsVO> PackageDetailList
        {
            get { return _PackageDetailList; }
            set { _PackageDetailList = value; }
        }

    }

    #endregion

}
