using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Patient
{
    public class AddBarcodeImageVO: IValueObject, INotifyPropertyChanged
    {
        private byte[] _GeneralDetailsBarcodeImage;
        public byte[] GeneralDetailsBarcodeImage
        {
            get { return _GeneralDetailsBarcodeImage; }
            set
            {
                if (_GeneralDetailsBarcodeImage != value)
                {
                    _GeneralDetailsBarcodeImage = value;
                    OnPropertyChanged("GeneralDetailsBarcodeImage");
                }
            }
        }

        private byte[] _SpouseBarcodeImage;
        public byte[] SpouseBarcodeImage
        {
            get { return _SpouseBarcodeImage; }
            set
            {
                if (_SpouseBarcodeImage != value)
                {
                    _SpouseBarcodeImage = value;
                    OnPropertyChanged("SpouseBarcodeImage");
                }
            }
        }

        private String _GeneralDetailsMRNo;
        public String GeneralDetailsMRNo
        {
            get { return _GeneralDetailsMRNo; }
            set
            {
                if (_GeneralDetailsMRNo != value)
                {
                    _GeneralDetailsMRNo = value;
                    OnPropertyChanged("GeneralDetailsMRNo");
                }
            }
        }

        private String _SpouseMRNo;
        public String SpouseMRNo
        {
            get { return _SpouseMRNo; }
            set
            {
                if (_SpouseMRNo != value)
                {
                    _SpouseMRNo = value;
                    OnPropertyChanged("SpouseMRNo");
                }
            }
        }

        private int _Flag1;
        public int Flag1
        {
            get { return _Flag1; }
            set
            {
                if (_Flag1 != value)
                {
                    _Flag1 = value;
                    OnPropertyChanged("Flag1");
                }
            }
        }
        

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
    }
}
