using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsApplyPackageDiscountRateOnServiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Billing.clsApplyPackageDiscountRateOnServiceBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        public List<clsApplyPackageDiscountRateOnServiceVO> objApplyPackageDiscountRate { get; set; }

        public long ipLoginUnitID { get; set; }

        public long ipPatientID { get; set; }
        public long ipPatientUnitID { get; set; }

        public long ipPatientGenderID { get; set; }
        public DateTime? ipPatientDateOfBirth { get; set; }

        public long ipVisitID { get; set; } // capture Last visit id

        public List<clsServiceMasterVO> ipServiceList { get; set; }

        public long MemberRelationID { get; set; }

        public long SponsorID { get; set; }
        public long SponsorUnitID { get; set; }

        public bool IsIPD { get; set; } 
    }


    public class clsApplyPackageDiscountRateOnServiceVO :IValueObject
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




        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long ServiceID { get; set; }
        public long TariffID { get; set; }
        public long PackageID { get; set; }
        public long ServiceID_AsPackageID { get; set; }

        public double DiscountedRate { get; set; }
        public double DiscountedPercentage { get; set; }

        // 1 :  Rate , 2 : Percentage, 3 : Both (Rate & Percentage)
        public int IsApplyOn_Rate_Percentage { get; set; }

        public  long  GrossDiscountID { get; set; }
        
        public Boolean IsServiceItemStockAvailable { get; set; } // only for markup service

        public bool IsDiscountOnQuantity { get; set; }  //set to check whether discount is apply on qty or not

        public long ActualQuantity { get; set; }
        public long UsedQuantity { get; set; }

        public bool IsAgeApplicable { get; set; }  //set to check whether Age Bracket is apply or not

        public long ServiceMemberRelationID { get; set; }  //set to check whether Relation is apply or not

        public double ConcessionPercentage { get; set; }  //used to get patient wise ConcessionPercentage from its package details

        public long ProcessID { get; set; }     // Package New Changes for Process Added on 20042018

    }
}
