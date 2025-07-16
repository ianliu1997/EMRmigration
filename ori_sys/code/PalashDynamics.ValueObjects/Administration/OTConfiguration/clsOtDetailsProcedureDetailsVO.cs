using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsOtDetailsProcedureDetailsVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject
        public string ToXml()
        {
            return this.ToString();
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

        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

     
        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                _UnitID = value;
                OnPropertyChanged("UnitID");
            }
        }
        private long _ServiceID;
        public long ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                _ServiceID = value;
                OnPropertyChanged("ServiceID");
            }
        }

        
        private long _ProcedureID;
        public long ProcedureID
        {
            get
            {
                return _ProcedureID;
            }
            set
            {
                _ProcedureID = value;
                OnPropertyChanged("ProcedureID");
            }
        }

        private string _ProcDesc;
        public string ProcDesc
        {
            get
            {
                return _ProcDesc;
            }
            set
            {
                _ProcDesc = value;
                OnPropertyChanged("ProcDesc");
            }
        }

        private string _ProcCode;
        public string ProcCode
        {
            get
            {
                return _ProcCode;
            }
            set
            {
                _ProcCode = value;
                OnPropertyChanged("ProcCode");
            }
        }

        private double _Rate;
        public double Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                _Rate = value;
                OnPropertyChanged("Rate");
            }
        }

        private long _Quantity;
        public long Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                _Quantity = value;
                OnPropertyChanged("Quantity");
                OnPropertyChanged("TotalAmount");
                OnPropertyChanged("ConcessionPercent");
                OnPropertyChanged("ConcessionAmount");
                OnPropertyChanged("ServiceTaxPercent");
                OnPropertyChanged("ServiceTaxAmount");
                OnPropertyChanged("NetAmount");
            }
        }

        private double _TotalAmount;
        public double TotalAmount
        {
            get
            {
                return _TotalAmount = _Rate * _Quantity;
            }
            set
            {
                _TotalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }

        

        private bool _Concession;
        public bool Concession
        {
            get { return _Concession; }
            set
            {
                if (value != _Concession)
                {
                    _Concession = value;
                    OnPropertyChanged("Concession");
                }
            }
        }

        private double _ConcessionPercent;
        public double ConcessionPercent
        {
            get { return _ConcessionPercent; }
            set
            {
                if (value != _ConcessionPercent)
                {
                    _ConcessionPercent = value;
                    OnPropertyChanged("ConcessionPercent");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("NetAmount");


                }
            }
        }


        private double _ConcessionAmount;
        public double ConcessionAmount
        {

            get
            {
                return (_TotalAmount*_ConcessionPercent)/100; 
            }
            set
            {
                if (value != _ConcessionAmount)
                {
                    _ConcessionAmount = value;
                    OnPropertyChanged("ConcessionAmount");
                }
            }
        }


   
        //------------------------------------------------------------------------------------------- 



        private double _ServiceTaxPercent;
        public double ServiceTaxPercent
        {
            get { return _ServiceTaxPercent; }
            set
            {
                if (value != _ServiceTaxPercent)
                {
                    _ServiceTaxPercent = value;
                    OnPropertyChanged("ServiceTaxPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }
        private double _ServiceTaxAmount;
        public double ServiceTaxAmount
        {
            get 
            { 
                return _ServiceTaxAmount = ((_TotalAmount - _ConcessionAmount)*_ServiceTaxPercent)/100; 
            }
            set
            {
                if (value != _ServiceTaxAmount)
                {
                    _ServiceTaxAmount = value;
                    OnPropertyChanged("ServiceTaxAmount");
                }
            }
        }


        


        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                _ConcessionAmount=(_TotalAmount * _ConcessionPercent) / 100; 
                _ServiceTaxAmount = ((_TotalAmount - _ConcessionAmount) * _ServiceTaxPercent) / 100; 
                return _NetAmount = _TotalAmount-_ConcessionAmount+_ServiceTaxAmount;
            }
            set
            {
                _NetAmount = value;
                OnPropertyChanged("NetAmount");
            }
        }

       




        

    }
}
