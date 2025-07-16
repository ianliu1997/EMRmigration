using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PalashDynamics.ValueObjects.Inventory
{
   public  class clsAddItemsEnquiryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddItemsEnquiryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        public long ItemEnquiryId { get; set; }
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// 

        private clsItemEnquiryVO objItems = new clsItemEnquiryVO();
        public clsItemEnquiryVO ItemMatserDetail
        {
            get
            {
                return objItems;
            }
            set
            {
                objItems = value;

            }
        }

        private List<clsEnquiryDetailsVO> objItemMaster = new List<clsEnquiryDetailsVO>();
        public List<clsEnquiryDetailsVO> ItemsDetail
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

        private List<clsItemsEnquirySupplierVO> objItemSupplier = new List<clsItemsEnquirySupplierVO>();
        public List<clsItemsEnquirySupplierVO> ItemsSupplierDetail
        {
            get
            {
                return objItemSupplier;
            }
            set
            {
                objItemSupplier = value;

            }
        }

        private List<clsItemsEnquiryTermConditionVO> objItemTermsCond= new List<clsItemsEnquiryTermConditionVO>();
        public List<clsItemsEnquiryTermConditionVO> ItemsTermsConditionDetail
        {
            get
            {
                return objItemTermsCond;
            }
            set
            {
                objItemTermsCond = value;

            }
        }

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
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



    }

    public class clsItemEnquiryVO : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public long EnquiryId { get; set; }
        public long UnitId { get; set; }
        public string EnquiryNO { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
        public long StoreID { get; set; }
        public string Header { get; set; }
        public string Notes { get; set; }
        public long? AddUnitID { get; set; }

        public string Store { get; set; }
        public string Supplier { get; set; }
        

        public long? By { get; set; }
        public string On { get; set; }
        public DateTime? DateTime { get; set; }
        public string WindowsLoginName { get; set; }


    }
    public class clsEnquiryDetailsVO : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public long Id { get; set; }
        public long UnitId { get; set; }
        public long EnquiryId { get; set; }
        public long ItemId { get; set; }
        public long BatchID { get; set; }
        public String ItemCode { get; set; }
        public String UOM { get; set; }
        public String ItemName { get; set; }
        
        private Double _Quantity;
        public Double Quantity
        {   
            get
            {
                return _Quantity;
            }
            set
            {
                if (value < 0)
                    _Quantity = 1;
                if (value.ToString().Length > 5)
                {
                    throw new ValidationException("Quantity Length Should Not Be Greater Than 5 Digits.");
                }
                else
                _Quantity = value;
                OnPropertyChanged("Quantity");
            }

        }

        private string _Remarks;
        public string Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                if (value.ToString().Length > 5)
                {
                    throw new ValidationException("Quantity Length Should Not Be Greater Than 5 Digits.");
                }
                else
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
                }
            }
        }






        public Double PackSize { get; set; }

        public bool SelectedEnquiry { get; set; }
       

    }
    public class clsItemsEnquiryTermConditionVO : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public long UnitId { get; set; }
        public long EnquiryId { get; set; }
        public long TermsConditionID { get; set; }
        public string TermsCondition{ get; set; }

        private string _Remarks;
        public string Remarks 
        { 
            get
            {
                return _Remarks;
            }
            set
            {
                _Remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        private Boolean _IsCheckedStatus=false;
        public Boolean IsCheckedStatus
        {
            get
            {
                return _IsCheckedStatus;
            }
            set
            {
                _IsCheckedStatus = value;
                OnPropertyChanged("IsCheckedStatus");
            }
        }





    }
    public class clsItemsEnquirySupplierVO : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public long Id { get; set; }
        public long UnitId { get; set; }
        public long EnquiryId { get; set; }
        public long SupplierID { get; set; }
        
    }

}
