using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsArtCycleSummaryVO : IValueObject, INotifyPropertyChanged
    {

        private long _PlanTherapyId;
        public long PlanTherapyId
        {
            get { return _PlanTherapyId; }
            set
            {
                if (_PlanTherapyId != value)
                {
                    _PlanTherapyId = value;
                    OnPropertyChanged("PlanTherapyId");
                }
            }
        }

        private DateTime? _TherapyStartDate;
        public DateTime? TherapyStartDate
        {
            get { return _TherapyStartDate; }
            set
            {
                if (_TherapyStartDate != value)
                {
                    _TherapyStartDate = value;
                    OnPropertyChanged("TherapyStartDate");
                }
            }
        }


        private String _Oocytes;
        public String Oocytes
        {
            get { return _Oocytes; }
            set
            {
                if (_Oocytes != value)
                {
                    _Oocytes = value;
                    OnPropertyChanged("Oocytes");
                }
            }
        }



        private String _Treatment;
        public String Treatment
        {
            get { return _Treatment; }
            set
            {
                if (_Treatment != value)
                {
                    _Treatment = value;
                    OnPropertyChanged("Treatment");
                }
            }
        }



        private String _PronucleusStages;
        public String PronucleusStages
        {
            get { return _PronucleusStages; }
            set
            {
                if (_PronucleusStages != value)
                {
                    _PronucleusStages = value;
                    OnPropertyChanged("PronucleusStages");
                }
            }
        }



        private String _Embryos;
        public String Embryos
        {
            get { return _Embryos; }
            set
            {
                if (_Embryos != value)
                {
                    _Embryos = value;
                    OnPropertyChanged("Embryos");
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
