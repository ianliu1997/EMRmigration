/* Added  By SUDHIR PATIL on 03/March/2014 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsIPDBedStatusVO : IValueObject, INotifyPropertyChanged
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


        public string ToXml()
        {
            return this.ToString();
        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }


        private long _BedID;
        public long BedID
        {
            get { return _BedID; }
            set { _BedID = value; }
        }

        private long _BedUnitID;
        public long BedUnitID
        {
            get { return _BedUnitID; }
            set { _BedUnitID = value; }
        }

        private string _BedDescription;
        public string BedDescription
        {
            get { return _BedDescription; }
            set { _BedDescription = value; }
        }



        private long _ClassID;
        public long ClassID
        {
            get { return _ClassID; }
            set { _ClassID = value; }
        }

        private long _WardID;
        public long WardID
        {
            get { return _WardID; }
            set { _WardID = value; }
        }

        private long _FloorID;
        public long FloorID
        {
            get { return _FloorID; }
            set { _FloorID = value; }
        }


        private long _ToBed;
        public long ToBed
        {
            get { return _ToBed; }
            set { _ToBed = value; }
        }

        private long _FromBed;
        public long FromBed
        {
            get { return _FromBed; }
            set { _FromBed = value; }
        }

        private long _BedCategoryId;
        public long BedCategoryId
        {
            get { return _BedCategoryId; }
            set { _BedCategoryId = value; }
        }

        private bool _IsSecondaryBed;
        public bool IsSecondaryBed
        {
            get { return _IsSecondaryBed; }
            set { _IsSecondaryBed = value; }
        }



        private string _IsNonCensus;
        public string IsNonCensus
        {
            get { return _IsNonCensus; }
            set { _IsNonCensus = value; }
        }

        private bool _IsOccupiedBoth;
        public bool IsOccupiedBoth
        {
            get { return _IsOccupiedBoth; }
            set { _IsOccupiedBoth = value; }
        }
        private bool _Occupied;
        public bool Occupied
        {
            get { return _Occupied; }
            set { _Occupied = value; }
        }

        private bool _IsUnderMaintanence;
        public bool IsUnderMaintanence
        {
            get { return _IsUnderMaintanence; }
            set { _IsUnderMaintanence = value; }
        }

        private bool _IsCancel;
        public bool IsCancel
        {
            get { return _IsCancel; }
            set { _IsCancel = value; }
        }

        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }

        private bool _IsDischarged;
        public bool IsDischarged
        {
            get { return _IsDischarged; }
            set { _IsDischarged = value; }
        }

        private bool _IsReserved;
        public bool IsReserved
        {
            get { return _IsReserved; }
            set { _IsReserved = value; }
        }
        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set { _PatientID = value; }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set { _PatientUnitID = value; }
        }

        private string _MRNO;
        public string MRNO
        {
            get { return _MRNO; }
            set { _MRNO = value; }
        }



    }
}
