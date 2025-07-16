using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.IPD
{

    public class clsAddIPDBedReservationBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsAddIPDBedReservationBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private List<clsIPDBedReservationVO> _LogList = null;
        public List<clsIPDBedReservationVO> LogList
        {
            get
            { return _LogList; }

            set
            { _LogList = value; }
        }

        private clsIPDBedReservationVO _BedDetails;
        public clsIPDBedReservationVO BedDetails
        {
            get { return _BedDetails; }
            set { _BedDetails = value; }

        }
        
    }

    public class clsGetIPDBedReservationListBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDBedReservationListBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        private MasterTableNameList _MasterTable = MasterTableNameList.None;
        public MasterTableNameList MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }
        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public string MRNo { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public bool Occupied { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        private List<clsIPDBedReservationVO> _BedList;
        public List<clsIPDBedReservationVO> BedList
        {
            get { return _BedList; }
            set { _BedList = value; }
        }

        private clsIPDBedReservationVO _BedDetails;
        public clsIPDBedReservationVO BedDetails
        {
            get { return _BedDetails; }
            set { _BedDetails = value; }
        }
    }

    public class clsGetIPDBedReservationStatusBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDBedReservationStatusBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }

        private List<clsIPDBedReservationVO> _LogList = null;
        public List<clsIPDBedReservationVO> LogList
        {
            get
            { return _LogList; }

            set
            { _LogList = value; }
        }

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        private MasterTableNameList _MasterTable = MasterTableNameList.None;
        public MasterTableNameList MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }
        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public string MRNo { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public bool Occupied { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        private List<clsIPDBedReservationVO> _BedList;
        public List<clsIPDBedReservationVO> BedList
        {
            get { return _BedList; }
            set { _BedList = value; }
        }

        private clsIPDBedReservationVO _BedDetails;
        public clsIPDBedReservationVO BedDetails
        {
            get { return _BedDetails; }
            set { _BedDetails = value; }
        }
    }


    public class clsAddIPDBedUnReservationBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsAddIPDBedUnReservationBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsIPDBedUnReservationVO _BedUnResDetails;
        public clsIPDBedUnReservationVO BedUnResDetails
        {
            get { return _BedUnResDetails; }
            set { _BedUnResDetails = value; }

        }
    }
}
