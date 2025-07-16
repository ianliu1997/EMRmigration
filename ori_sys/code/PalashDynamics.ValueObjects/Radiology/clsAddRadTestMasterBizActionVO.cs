using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master;


namespace PalashDynamics.ValueObjects.Radiology
{               
   public  class clsAddRadTestMasterBizActionVO:IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddRadTestMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private clsRadiologyVO objDetails = null;
        public clsRadiologyVO TestDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


        private int _SuccessStatus;
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
       ////clsPathoTestTemplateDetailsVO 
       // private clsRadioTestTemplateDetailsVO objItemMater = null;
       // /// <summary>
       // /// Output Property.
       // /// This Property Contains OPDPatient Details Which is Added.
       // /// </summary>
       // public clsRadioTestTemplateDetailsVO ItemDetails
       // {
       //     get { return objItemMater; }
       //     set { objItemMater = value; }
       // }

       // public clsRadioTestTemplateDetailsVO ItemSupplier { get; set; }
       // public List<clsRadioTestTemplateDetailsVO> ItemSupplierList { get; set; }
       // public List<clsRadioTestTemplateDetailsVO> ItemList { get; set; }
        //public clsRadiologyVO ItemSupplier { get; set; }
        //public List<clsRadiologyVO> ItemSupplierList { get; set; }
        //public List<clsRadiologyVO> ItemList { get; set; }
    }

   public class clsGetRadiologistToTempBizActionVO : IBizActionValueObject
   {
       //public string GetBizAction()
       //{
       //    return "PalashDynamics.BusinessLayer.clsGetRadiologistToTempBizAction";
       //}

       //public string ToXml()
       //{
       //    return this.ToXml();
       //}
       //private int _SuccessStatus;
       ///// <summary>
       ///// Output Property.
       ///// This property states the outcome of BizAction Process.
       ///// </summary>
       //public int SuccessStatus
       //{
       //    get { return _SuccessStatus; }
       //    set { _SuccessStatus = value; }
       //}

       //private Boolean _CheckForTaxExistatnce;
       ///// <summary>
       ///// Output Property.
       ///// This property states the outcome of BizAction Process.
       ///// </summary>
       //public Boolean CheckForTaxExistatnce
       //{
       //    get { return _CheckForTaxExistatnce; }
       //    set { _CheckForTaxExistatnce = value; }
       //}

       //private Boolean _IsTaxAdded;
       //public Boolean IsTaxAdded
       //{
       //    get { return _IsTaxAdded; }
       //    set { _IsTaxAdded = value; }
       //}
       ////private clsRadTemplateDetailMasterVO objItemMater = null;
       /////// <summary>
       /////// Output Property.
       /////// This Property Contains OPDPatient Details Which is Added.
       /////// </summary>
       ////public clsRadTemplateDetailMasterVO ItemDetails
       ////{
       ////    get { return objItemMater; }
       ////    set { objItemMater = value; }
       ////}


       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.clsGetRadiologistToTempBizAction";
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
       //private clsRadioTestTemplateDetailsVO objItemMater = null;
       ///// <summary>
       ///// Output Property.
       ///// This Property Contains OPDPatient Details Which is Added.
       ///// </summary>
       //public clsPathoTestTemplateDetailsVO ItemDetails
       //{
       //    get { return objItemMater; }
       //    set { objItemMater = value; }
       //}

       //public clsPathoTestTemplateDetailsVO ItemSupplier { get; set; }
       //public List<clsPathoTestTemplateDetailsVO> ItemSupplierList { get; set; }
       //public List<clsPathoTestTemplateDetailsVO> ItemList { get; set; }

       public clsRadiologyVO ItemSupplier { get; set; }
       public List<clsRadiologyVO> ItemSupplierList { get; set; }
       public List<clsRadiologyVO> ItemList { get; set; }
     
   }

   public class clsAddRadiologistToTempbizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject



       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.clsAddRadiologistToTempbizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion



        //<summary>
        //This property contains Item master details.
        //</summary>
       private clsRadiologyVO objItemMaster = null;
       public clsRadiologyVO ItemMatserDetails
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


       ///// <summary>
       /////  Output Property.
       ///// This property states the outcome of BizAction Process.
       ///// </summary>
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


       public List<clsRadiologyVO> ItemList { get; set; }

       public clsRadiologyVO ItemSupplier { get; set; }
       public List<clsRadiologyVO> ItemSupplierList { get; set; }
   }

   public class clsGetRadioTemplateGenderBizActionVO : IBizActionValueObject
   {
       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           // throw new NotImplementedException();
           return "PalashDynamics.BusinessLayer.clsGetRadioTemplateGenderBizAction";
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
    
                
  // by Yogesh dated 20.1.16
}
