using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.PackageNew
{
    public class clsPackageSourceRelationVO : IValueObject, INotifyPropertyChanged
    {
        #region Properties


        public long ID { get; set; }
        public long UnitID { get; set; }

        public long PatientCategoryL1ID { get; set; }
        public long PatientCategoryL2ID { get; set; }
        public long PatientCategoryL3ID { get; set; }
        public long CompanyID { get; set; }

        public string PatientCategoryL1 { get; set; }
        public string PatientCategoryL2 { get; set; }
        public string PatientCategoryL3 { get; set; }
        public string Company { get; set; }

        public bool Status { get; set; }

        public bool IsSaveForL2 { get; set; }

        #endregion

        public string ToXml()
        {
            // throw new NotImplementedException();
            return this.ToString();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    public class clsPackageRateClinicWiseVO : IValueObject, INotifyPropertyChanged
    {
        #region Properties


        public long ID { get; set; }
        public long UnitID { get; set; }
        public long PatientCategoryL3ID { get; set; }
        public string PatientCategoryL3 { get; set; }
        public bool Status { get; set; }
        public string UnitName { get; set; }
        public string TariffName { get; set; }        
        public decimal Rate { get; set; }
        public bool IsRateEnabled { get; set; }

        #endregion

        public string ToXml()
        {
            // throw new NotImplementedException();
            return this.ToString();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    //public class clsAddPackageSourceRelationsBizActionVO : IBizActionValueObject
    //{
    //    private List<clsPackageSourceRelationVO> _PackageSourceRelationList;
    //    public List<clsPackageSourceRelationVO> PackageSourceRelationList
    //    {
    //        get
    //        {
    //            if (_PackageSourceRelationList == null)
    //                _PackageSourceRelationList = new List<clsPackageSourceRelationVO>();

    //            return _PackageSourceRelationList;
    //        }

    //        set
    //        {
    //            _PackageSourceRelationList = value;
    //        }
    //    }

    //    #region IBizActionValueObject Members

    //    public string GetBizAction()
    //    {
    //        return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsAddPackageSourceRelationsBizAction";
    //    }

    //    #endregion

    //    #region IValueObject Members

    //    public string ToXml()
    //    {
    //        return this.ToString();
    //    }

    //    #endregion
    //}

    //public class clsAddPackageCompanyTariffBizActionVO : IBizActionValueObject
    //{
    //    private List<clsPackageSourceRelationVO> _PackageSourceRelationList;
    //    public List<clsPackageSourceRelationVO> PackageSourceRelationList
    //    {
    //        get
    //        {
    //            if (_PackageSourceRelationList == null)
    //                _PackageSourceRelationList = new List<clsPackageSourceRelationVO>();

    //            return _PackageSourceRelationList;
    //        }

    //        set
    //        {
    //            _PackageSourceRelationList = value;
    //        }
    //    }

    //    #region IBizActionValueObject Members

    //    public string GetBizAction()
    //    {
    //        return "PalashDynamics.BusinessLayer.Administration.PackageNew.clsAddPackageCompanyTariffBizAction";
    //    }

    //    #endregion

    //    #region IValueObject Members

    //    public string ToXml()
    //    {
    //        return this.ToString();
    //    }

    //    #endregion
    //}
}
