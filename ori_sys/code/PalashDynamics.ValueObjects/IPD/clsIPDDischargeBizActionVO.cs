using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsAddIPDDischargeBizActionVO : IBizActionValueObject
    {

        

            public string GetBizAction()
            {
                return "PalashDynamics.BusinessLayer.IPD.clsAddIPDDischargeBizAction";
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



            private clsIPDDischargeVO _DischargeDetails;
            public clsIPDDischargeVO DischargeDetails
            {
                get { return _DischargeDetails; }
                set { _DischargeDetails = value; }

            }




        
    }

    public class clsGetIPDDischargeBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDDischargeBizAction";
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



        private List<clsIPDDischargeVO> _DischargeList;
        public List<clsIPDDischargeVO> DischargeList
        {
            get { return _DischargeList; }
            set { _DischargeList = value; }

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




    }


    
    public class clsGetDischargeStatusBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetDischargeStatusBizAction";
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

        private clsIPDDischargeVO _DischargeDetails;
        public clsIPDDischargeVO DischargeDetails
        {
            get { return _DischargeDetails; }
            set { _DischargeDetails = value; }

        }

        private List<clsIPDDischargeVO> _DischargeList;
        public List<clsIPDDischargeVO> DischargeList
        {
            get { return _DischargeList; }
            set { _DischargeList = value; }

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
    }

    //public class clsGetDischargeStatusBizActionVO : IBizActionValueObject
    //{
    //    public string GetBizAction()
    //    {
    //        return "PalashDynamics.BusinessLayer.IPD.clsGetDischargeStatusBizAction";
    //    }

    //    public string ToXml()
    //    {

    //        return this.ToString();
    //    }

    //    //private int _SuccessStatus;        
    //    //public int SuccessStatus
    //    //{
    //    //    get { return _SuccessStatus; }
    //    //    set { _SuccessStatus = value; }
    //    //}

    //    //private clsIPDDischargeVO _DischargeDetails;
    //    //public clsIPDDischargeVO DischargeDetails
    //    //{
    //    //    get { return _DischargeDetails; }
    //    //    set { _DischargeDetails = value; }

    //    //}

    //    private List<clsIPDDischargeVO> _DischargeStatusList;
    //    public List<clsIPDDischargeVO> DischargeStatusList
    //    {
    //        get { return _DischargeStatusList; }
    //        set { _DischargeStatusList = value; }

    //    }

    //    public long StartRowIndex { get; set; }
    //    public int MaximumRows { get; set; }
    //    public int TotalRows { get; set; }
    //    public bool PagingEnabled { get; set; }
    //    public string sortExpression { get; set; }

    //    public long PatientID { get; set; }
    //    public long PatientUnitID { get; set; }
    //    public long UnitID { get; set; }

    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }

    //    public DateTime? FromDate { get; set; }
    //    public DateTime? ToDate { get; set; }
    //}

    #region For Nursing Station

    public class clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetBillAndBedByAdmIDAndAdmUnitIDBizAction";
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

        private bool _IsFinalBillPrepare;
        public bool IsFinalBillPrepare
        {
            get { return _IsFinalBillPrepare; }
            set { _IsFinalBillPrepare = value; }
        }

        private bool _IsBedRelease;
        public bool IsBedRelease
        {
            get { return _IsBedRelease; }
            set { _IsBedRelease = value; }
        }

        private List<clsIPDDischargeVO> _DischargeList;
        public List<clsIPDDischargeVO> DischargeList
        {
            get { return _DischargeList; }
            set { _DischargeList = value; }
        }

        private List<clsIPDDischargeVO> _BedList;
        public List<clsIPDDischargeVO> BedList
        {
            get { return _BedList; }
            set { _BedList = value; }
        }

        private List<clsIPDDischargeVO> _billList;
        public List<clsIPDDischargeVO> BillList
        {
            get { return _billList; }
            set { _billList = value; }
        }

        private clsIPDDischargeVO _DischargeDetails;
        public clsIPDDischargeVO DischargeDetails
        {
            get { return _DischargeDetails; }
            set { _DischargeDetails = value; }
        }

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public int StartIndex { get; set; }
        public int DisAdm { get; set; }
        public string StrDisAdm { get; set; }
        public string MrNo { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long AdmID { get; set; }
        public long AdmUnitID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }


    }

    #endregion

}
