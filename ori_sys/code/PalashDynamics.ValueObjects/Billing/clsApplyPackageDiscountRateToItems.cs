using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsApplyPackageDiscountRateToItems : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Billing.clsApplyPackageDiscountRateToItemsBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion


        public clsApplyPackageDiscountRateOnItemVO objApplyItemPackageDiscountRateDetails { get; set; }

        public List<clsApplyPackageDiscountRateOnItemVO> objApplyItemPackageDiscountRate { get; set; }

        public long ipLoginUnitID { get; set; }

        public long ipPatientID { get; set; }
        public long ipPatientUnitID { get; set; }

        public long ipVisitID { get; set; } // capture Last visit id

        public List<clsServiceMasterVO> ipServiceList { get; set; }

    }

    public class clsApplyPackageDiscountRateOnItemVO : IValueObject
    {
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        public long ItemId { get; set; }


        public long PatientCatagoryL1 { get; set; }
        public long PatientCatagoryL2 { get; set; }
        public long PatientCatagoryL3 { get; set; }
        public long CompanyID { get; set; }
        public float Discount { get; set; }
        public string ItemIDs { get; set; }

        public double DiscountedRate { get; set; }
        public double ApplicableToAllDiscount { get; set; }
        public bool ApplicableToAll { get; set; }       // Package Change 17042017
        public double DiscountedPercentage { get; set; }

        // 1 :  Rate , 2 : Percentage
        public int IsApplyOn_Rate_Percentage { get; set; }

        public long GrossDiscountID { get; set; }

        public Boolean IsServiceItemStockAvailable { get; set; } // only for markup service

        public long CategoryId { get; set; }
        public bool IsCategory { get; set; }
        public long GroupId { get; set; }
        public bool IsGroup { get; set; }


        //By Anjali......................
        public long PatientGenderID { get; set; }
        public DateTime PatientDateOfBirth { get; set; }
        public float Budget { get; set; }
        public float TotalBudget { get; set; }
        public float CalculatedBudget { get; set; }
        public float CalculatedTotalBudget { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long PackageID { get; set; }
        
        //................................

        #region Package Change 18042017

        private long _PackageBillID;
        public long PackageBillID
        {
            get { return _PackageBillID; }
            set
            {
                if (_PackageBillID != value)
                {
                    _PackageBillID = value;
                    OnPropertyChanged("PackageBillID");
                }
            }
        }

        private long _PackageBillUnitID;
        public long PackageBillUnitID
        {
            get { return _PackageBillUnitID; }
            set
            {
                if (_PackageBillUnitID != value)
                {
                    _PackageBillUnitID = value;
                    OnPropertyChanged("PackageBillUnitID");
                }
            }
        }

        #endregion

    }
}
