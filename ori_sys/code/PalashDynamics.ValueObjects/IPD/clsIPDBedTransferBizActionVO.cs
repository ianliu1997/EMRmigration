using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsGetIPDPatientDetailsBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDPatientDetailsBizAction";
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

        public long ID { get; set; }
        public long UnitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public string MRNo { get; set; }

        private clsIPDAdmissionVO _PatientDetails;
        public clsIPDAdmissionVO PatientDetails
        {
            get { return _PatientDetails; }
            set { _PatientDetails = value; }

        }

       


    }

    public class clsAddIPDBedTransferBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsAddIPDBedTransferBizAction";
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
        private clsIPDBedTransferVO _BedDetails;
        public clsIPDBedTransferVO BedDetails
        {
            get { return _BedDetails; }
            set { _BedDetails = value; }

        }

        private List<clsIPDBedTransferVO> _BedList;
        public List<clsIPDBedTransferVO> BedList
        {
            get { return _BedList; }
            set { _BedList = value; }
        }

        public bool IsTransfer { get; set; }
    }

    public class clsGetIPDBedTransferBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDBedTransferBizAction";
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
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string sortExpression { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        private List<clsIPDBedTransferVO> _BedDetails;
        public List<clsIPDBedTransferVO> BedList
        {
            get { return _BedDetails; }
            set { _BedDetails = value; }

        }




    }

    #region Added By SUDHIR PATIL
    /* Adde by SUDHIR  on 3rd March 2014 */
    public class clsGetIPDWardByClassIDBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDWardByClassIDBizAction";
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

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        private List<clsIPDBedTransferVO> _BedList;
        public List<clsIPDBedTransferVO> BedList
        {
            get { return _BedList; }
            set { _BedList = value; }
        }

        private clsIPDBedTransferVO _BedDetails;
        public clsIPDBedTransferVO BedDetails
        {
            get { return _BedDetails; }
            set { _BedDetails = value; }
        }
    }


    public class clsGetIPDBedTransferListBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDBedTransferListBizAction";
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

        private bool _CheckFinalBill = false;
        public bool IsCheckFinalBill
        {
            get { return _CheckFinalBill; }
            set { _CheckFinalBill = value; }
        }

        private bool _CheckFinalBillByAdmID = false;
        public bool IsCheckFinalBillByAdmID
        {
            get { return _CheckFinalBillByAdmID; }
            set { _CheckFinalBillByAdmID = value; }
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
        public bool IsSelectedPatient { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public string MRNo { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long AdmID { get; set; }
        public long AdmUnitID { get; set; }
        public long UnitID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        private List<clsIPDBedTransferVO> _BedList;
        public List<clsIPDBedTransferVO> BedList
        {
            get { return _BedList; }
            set { _BedList = value; }
        }

        private clsIPDBedTransferVO _BedDetails;
        public clsIPDBedTransferVO BedDetails
        {
            get { return _BedDetails; }
            set { _BedDetails = value; }
        }
    }

    public class clsGetIPDDischargeBedListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDDischargeBedListBizAction";
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
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public string MRNo { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        private List<clsIPDBedTransferVO> _BedList;
        public List<clsIPDBedTransferVO> BedList
        {
            get { return _BedList; }
            set { _BedList = value; }
        }

        private clsIPDBedTransferVO _BedDetails;
        public clsIPDBedTransferVO BedDetails
        {
            get { return _BedDetails; }
            set { _BedDetails = value; }
        }

    }

    public class clsUpdateIPDBedReleasedBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsUpdateIPDBedReleasedBizAction";
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

        private clsIPDBedTransferVO _BedDetails;
        public clsIPDBedTransferVO BedDetails
        {
            get { return _BedDetails; }
            set { _BedDetails = value; }
        }

        private List<clsIPDBedTransferVO> _BedList;
        public List<clsIPDBedTransferVO> BedList
        {
            get { return _BedList; }
            set { _BedList = value; }
        }
    }


    #endregion

}
