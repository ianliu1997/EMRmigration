using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace PalashDynamics.ValueObjects.Inventory.EnquirySearch
{
    public class clsEnquirySearchVO:INotifyPropertyChanged,IValueObject
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        #region ALL PROPERTIES
        private float _Quantity;
        public float Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");

                }
            }
        }

        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set
            {
                if (_ItemCode != value)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");

                }
            }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");

                }
            }
        }

        private float _PackSize;
        public float PackSize
        {
            get { return _PackSize; }
            set
            {
                if (_PackSize != value)
                {
                    _PackSize = value;
                    OnPropertyChanged("PackSize");

                }
            }
        }

        private long _EnquiryNumber;
        public long EnquiryNumber
        {
            get { return _EnquiryNumber; }
            set
            {
                if (_EnquiryNumber != value)
                {
                    _EnquiryNumber = value;
                    OnPropertyChanged("EnquiryNumber");

                }
            }
        }

        private DateTime _EnquiryDate;
        public DateTime EnquiryDate
        {
            get { return _EnquiryDate; }
            set
            {
                if (_EnquiryDate != value)
                {
                    _EnquiryDate = value;
                    OnPropertyChanged("EnquiryDate");

                }
            }
        }

        private long _EnquiryID;
        public long EnquiryID
        {
            get { return _EnquiryID; }
            set
            {
                if (_EnquiryID != value)
                {
                    _EnquiryID = value;
                    OnPropertyChanged("EnquiryID");

                }
            }
        }



        public List<clsEnquirySearchVO> EnquiryList { get; set; }
        #endregion

        



    }
}
