using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.PatientSourceMaster
{
   public  class clsGetPatientSourceListBizActionVO:IBizActionValueObject
    {

        private List<clsPatientSourceVO> _PatientSourceDetails;
        public List<clsPatientSourceVO> PatientSourceDetails
        {
            get { return _PatientSourceDetails; }
            set { _PatientSourceDetails = value; }
        }

        private List<MasterListItem> _MasterList;
        public List<MasterListItem> MasterList
        {
            get { return _MasterList; }
            set { _MasterList = value; }
        }
        public long ID { get; set; }

        public long FilterPatientSourceType { get; set; }

        public bool ValidPatientMasterSourceList { get; set; }
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
            return "PalashDynamics.BusinessLayer.Administration.PatientSourceMaster.clsGetPatientSourceListBizAction";
        }

        #endregion
       
        public string ToXml()
        {
           return this.ToString();
        }

        public bool IsFromItemGroupMaster { get; set; }  // set true from PalashDynamics.Administration.ItemGroupMaster
    }

   public class clsGetRegistrationChargesListBizActionVO : IBizActionValueObject
   {

       private List<clsRegistrationChargesVO> _PatientSourceDetails;
       public List<clsRegistrationChargesVO> PatientSourceDetails
       {
           get { return _PatientSourceDetails; }
           set { _PatientSourceDetails = value; }
       }

       private List<MasterListItem> _MasterList;
       public List<MasterListItem> MasterList
       {
           get { return _MasterList; }
           set { _MasterList = value; }
       }
       public long ID { get; set; }

       public long FilterPatientSourceType { get; set; }

       public bool ValidPatientMasterSourceList { get; set; }
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
           return "PalashDynamics.BusinessLayer.Administration.PatientSourceMaster.clsGetRegistrationChargesListBizAction";
       }

       #endregion

       public string ToXml()
       {
           return this.ToString();
       }
   }
    
}
