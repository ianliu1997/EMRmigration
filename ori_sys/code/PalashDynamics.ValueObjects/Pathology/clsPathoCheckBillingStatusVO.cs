using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsPathoCheckBillingStatusVO : IBizActionValueObject, INotifyPropertyChanged
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsPathoCheckBillingStatus";
        }

        #endregion

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

   


        #region Property Declarations

        private long _OrderId;
        public long OrderId
        {
            get { return _OrderId; }
            set
            {
                if (_OrderId != value)
                {
                    _OrderId = value;
                    OnPropertyChanged("OrderId");
                }
            }
        }

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        private string _SampleNo;
        public string SampleNo
        {
            get { return _SampleNo; }
            set
            {
                if (_SampleNo != value)
                {
                    _SampleNo = value;
                    OnPropertyChanged("SampleNo");
                }
            }
        }

        private bool _ResultStatus;
        public bool ResultStatus
        {
            get { return _ResultStatus; }
            set
            {
                if (_ResultStatus != value)
                {
                    _ResultStatus = value;
                    OnPropertyChanged("ResultStatus");

                }
            }

        }




        #endregion

    }
}
