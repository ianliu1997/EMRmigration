using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsIPDBedDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsIPDBedDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _BedID;
        public long BedID
        {
            get { return _BedID; }
            set
            {
                if (_BedID != value)
                {
                    _BedID = value;
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                }
            }
        }

        private clsIPDBedDetailsVO _BedDetails;
        public clsIPDBedDetailsVO BedDetails
        {
            get { return _BedDetails; }
            set { _BedDetails = value; }

        }

        private List<clsIPDBedAmmenityVO> _BedAmmenity;
        public List<clsIPDBedAmmenityVO> BedAmmenity
        {
            get { return _BedAmmenity; }
            set { _BedAmmenity = value; }

        }
    }
    public class clsIPDAdmissionBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsIPDAdmissionBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }


        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                }
            }
        }

        private clsIPDAdmissionDetailVO _AdmissionDetails;
        public clsIPDAdmissionDetailVO AdmissionDetails
        {
            get { return _AdmissionDetails; }
            set { _AdmissionDetails = value; }
        }

        private bool _IsAdmVisit = false;
        public bool IsadmVisit
        {
            get
            {
                return _IsAdmVisit;
            }
            set
            {
                value = _IsAdmVisit;
            }
        }

    }
}
