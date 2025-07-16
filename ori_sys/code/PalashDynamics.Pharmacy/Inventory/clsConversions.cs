using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.Generic;
using System.ComponentModel;
using PalashDynamics.ValueObjects;
using System.Linq;
using CIMS;

namespace PalashDynamics.Pharmacy.Inventory
{
    public class clsConversions : IValueObject, INotifyPropertyChanged
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

        List<clsConversionsVO> _UOMConvertLIst = new List<clsConversionsVO>();
        public List<clsConversionsVO> UOMConvertLIst
        {
            get
            {
                return _UOMConvertLIst;
            }
            set
            {
                if (value != _UOMConvertLIst)
                {
                    _UOMConvertLIst = value;
                    OnPropertyChanged("UOMConvertLIst");

                }
            }

        }

        private float _CalculatedCF;
        public float CalculatedCF
        {
            get { return _CalculatedCF; }
            set
            {
                if (_CalculatedCF != value)
                {
                    _CalculatedCF = value;
                    OnPropertyChanged("CalculatedCF");
                }
            }
        }

        private float _CalculatedFromCF;
        public float CalculatedFromCF
        {
            get { return _CalculatedFromCF; }
            set
            {
                if (_CalculatedFromCF != value)
                {
                    _CalculatedFromCF = value;
                    OnPropertyChanged("CalculatedFromCF");
                }
            }
        }

        private float _CalculatedToCF;
        public float CalculatedToCF
        {
            get { return _CalculatedToCF; }
            set
            {
                if (_CalculatedToCF != value)
                {
                    _CalculatedToCF = value;
                    OnPropertyChanged("CalculatedToCF");
                }
            }
        }

        private float _ConversionFactor;
        public float ConversionFactor
        {
            get { return _ConversionFactor; }
            set
            {
                if (_ConversionFactor != value)
                {
                    _ConversionFactor = value;
                    OnPropertyChanged("ConversionFactor");
                }
            }
        }

        private float _BaseConversionFactor;
        public float BaseConversionFactor
        {
            get { return _BaseConversionFactor; }
            set
            {
                if (_BaseConversionFactor != value)
                {
                    _BaseConversionFactor = value;
                    OnPropertyChanged("BaseConversionFactor");
                }
            }
        }

        private float _MRP;
        public float MRP
        {
            get
            {
                _MRP = (float)Math.Round((decimal)_MRP, 2);
                return _MRP;
            }
            set
            {
                if (value != _MRP)
                {
                    _MRP = value;
                    OnPropertyChanged("MRP");
                }
            }
        }

        private float _Rate;
        public float Rate
        {
            get { return _Rate; }
            set
            {
                if (_Rate != value)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }

        private float _Quantity;
        public float Quantity
        {
            get
            {

                return _Quantity;
            }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        private float _BaseRate;
        public float BaseRate
        {
            get { return _BaseRate; }
            set
            {
                if (_BaseRate != value)
                {
                    _BaseRate = value;
                    OnPropertyChanged("BaseRate");
                }
            }
        }


        private float _BaseMRP;
        public float BaseMRP
        {
            get
            {
                _BaseMRP = (float)Math.Round((decimal)_BaseMRP, 2);
                return _BaseMRP;
            }
            set
            {
                if (value != _BaseMRP)
                {
                    _BaseMRP = value;
                    OnPropertyChanged("BaseMRP");
                }
            }
        }

        private float _SingleQuantity;
        public float SingleQuantity
        {
            get
            {
                return _SingleQuantity;
            }
            set
            {
                if (_SingleQuantity != value)
                {

                    _SingleQuantity = value;
                    OnPropertyChanged("SingleQuantity");
                    OnPropertyChanged("Quantity");
                }
            }
        }


        private float _MainMRP;
        public float MainMRP
        {
            get
            {
                _MainMRP = (float)Math.Round((decimal)_MainMRP, 2);
                return _MainMRP;
            }
            set
            {
                if (value != _MainMRP)
                {
                    _MainMRP = value;
                    OnPropertyChanged("MainMRP");
                }
            }
        }

        private float _MainRate;
        public float MainRate
        {
            get { return _MainRate; }
            set
            {
                if (_MainRate != value)
                {
                    _MainRate = value;
                    OnPropertyChanged("MainRate");
                }
            }
        }

        private float _BaseQuantity;
        public float BaseQuantity
        {
            get
            {

                return _BaseQuantity;
            }
            set
            {
                if (_BaseQuantity != value)
                {
                    _BaseQuantity = value;
                    OnPropertyChanged("BaseQuantity");
                }
            }
        }

        // Function Parameters
        // FromUOMID - Transaction UOM
        // ToUOMID - Stocking UOM
        // BaseUOMID - Base UOM
        public clsConversions CalculateConversionFactor(long FromUOMID, long ToUOMID, long BaseUOMID)
        {
            clsConversions objConversions = new clsConversions();

            try
            {
                clsConversionsVO objConversionFrom = new clsConversionsVO();
                clsConversionsVO objConversionTo = new clsConversionsVO();

                if (UOMConvertLIst.Count > 0)
                {
                    objConversionFrom = UOMConvertLIst.Where(z => z.FromUOMID == FromUOMID && z.ToUOMID == BaseUOMID).FirstOrDefault();
                    objConversionTo = UOMConvertLIst.Where(z => z.FromUOMID == ToUOMID && z.ToUOMID == BaseUOMID).FirstOrDefault();
                }


                if (objConversionFrom != null) //&& objConversionTo != null
                {
                    CalculatedFromCF = objConversionFrom.ConversionFactor;

                    if (objConversionTo != null)
                        CalculatedToCF = objConversionTo.ConversionFactor;
                    else if (ToUOMID == BaseUOMID)
                        CalculatedToCF = 1;

                    CalculatedCF = CalculatedFromCF / CalculatedToCF;

                    ConversionFactor = CalculatedCF;
                    BaseConversionFactor = CalculatedFromCF;
                }


                if (CalculatedCF > 0 && FromUOMID != ToUOMID) // e.g. (Selected Transaction UOM) Box != Strip (Item Master Stock UOM) 
                {
                    if (CalculatedCF.ToString().IsItNumber()) // e.g. Strip to Tab
                    {
                        MRP = MainMRP * CalculatedFromCF;
                        Rate = MainRate * CalculatedFromCF;

                        Quantity = SingleQuantity * ConversionFactor;
                        BaseQuantity = SingleQuantity * BaseConversionFactor;

                        BaseRate = MainRate;
                        BaseMRP = MainMRP;
                    }
                    else     // e.g. Tab to Strip  (Reverse flow 1 Tablet = How many Strip ? if CF = 10 then 1/10)
                    {
                        MRP = MainMRP * CalculatedFromCF;
                        Rate = MainRate * CalculatedFromCF;

                        Quantity = SingleQuantity * CalculatedCF;
                        BaseQuantity = SingleQuantity * CalculatedFromCF;

                        BaseRate = MainRate;
                        BaseMRP = MainMRP;
                    }
                }
                else if (CalculatedCF > 0 && FromUOMID == ToUOMID)  // e.g. (Selected Transaction UOM) Strip == Strip (Item Master Stock UOM) 
                {
                    if (UOMConvertLIst.Count > 0)
                    {
                        clsConversionsVO objConversionFromSame = new clsConversionsVO();
                        clsConversionsVO objConversionToSame = new clsConversionsVO();

                        objConversionFromSame = UOMConvertLIst.Where(z => z.FromUOMID == FromUOMID && z.ToUOMID == BaseUOMID).FirstOrDefault();
                        objConversionToSame = UOMConvertLIst.Where(z => z.FromUOMID == FromUOMID && z.ToUOMID == BaseUOMID).FirstOrDefault();

                        float CalculatedFromCFSame = 0;
                        float CalculatedToCFSame = 0;

                        CalculatedFromCFSame = objConversionFromSame.ConversionFactor;
                        CalculatedToCFSame = objConversionToSame.ConversionFactor;

                        CalculatedCF = CalculatedFromCFSame / CalculatedToCFSame;

                        ConversionFactor = CalculatedCF;
                        BaseConversionFactor = CalculatedFromCFSame;

                        MRP = MainMRP * CalculatedFromCFSame;
                        Rate = MainRate * CalculatedFromCFSame;

                        Quantity = SingleQuantity * CalculatedCF;
                        BaseQuantity = SingleQuantity * BaseConversionFactor;

                        BaseRate = MainRate;
                        BaseMRP = MainMRP;

                    }
                }

            }
            catch (Exception ex)
            {

            }

            return objConversions;

        }

    }
}
