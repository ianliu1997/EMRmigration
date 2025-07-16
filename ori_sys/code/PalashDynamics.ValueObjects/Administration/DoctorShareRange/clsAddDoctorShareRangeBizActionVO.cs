using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.DoctorShareRange
{
    public class clsAddDoctorShareRangeBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DoctorShareRange.clsAddDoctorShareRangeBizAction";
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

        private clsDoctorShareRangeVO _ShareRangeDetails;
        public clsDoctorShareRangeVO ShareRangeDetails
        {
            get { return _ShareRangeDetails; }
            set { _ShareRangeDetails = value; }
        }
        private List<clsDoctorShareRangeVO> _ShareRangeList = new List<clsDoctorShareRangeVO>();
        public List<clsDoctorShareRangeVO> ShareRangeList
        {
            get { return _ShareRangeList; }
            set { _ShareRangeList = value; }
        }
        public bool IsStatusChanged { get; set; }
    }

    public class clsGetDoctorShareRangeListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DoctorShareRange.clsGetDoctorShareRangeListBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }
        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsDoctorShareRangeVO _ShareRangeDetails;
        public clsDoctorShareRangeVO ShareRangeDetails
        {
            get { return _ShareRangeDetails; }
            set { _ShareRangeDetails = value; }
        }
        private List<clsDoctorShareRangeVO> _ShareRangeList = new List<clsDoctorShareRangeVO>();
        public List<clsDoctorShareRangeVO> ShareRangeList
        {
            get { return _ShareRangeList; }
            set { _ShareRangeList = value; }
        }
    }
}