using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsSaveIPDAdmissionBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            // throw new NotImplementedException();

            return "PalashDynamics.BusinessLayer.IPD.clsSaveIPDAdmissionBizAction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
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

        private clsIPDAdmissionVO _Details = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsIPDAdmissionVO Details
        {
            get { return _Details; }
            set { _Details = value; }

        }
        private List<clsIPDBedMasterVO> _List = null;
        public List<clsIPDBedMasterVO> List
        {
            get { return _List; }
            set { _List = value; }
        }
        private bool _IsMedicoLegalCase = false;
        public bool IsMedicoLegalCase
        {
            get
            {
                return _IsMedicoLegalCase;
            }
            set { _IsMedicoLegalCase = value; }
        }

        //private List<clsIPDAdmissionAddDocVO> _AdmAddDocList = null;
        //public List<clsIPDAdmissionAddDocVO> AdmAddDocList
        //{
        //    get { return _AdmAddDocList; }
        //    set { _AdmAddDocList = value; }
        //}

        //private List<clsIPDAdmCreditSanctionVO> _AdmCreditSactionList = null;
        //public List<clsIPDAdmCreditSanctionVO> AdmCreditSactionList
        //{
        //    get { return _AdmCreditSactionList; }
        //    set { _AdmCreditSactionList = value; }
        //}


        private clsIPDAdmMLCDetailsVO _AdmMLDCDetails ;
        public clsIPDAdmMLCDetailsVO AdmMLDCDetails
        {
            get { return _AdmMLDCDetails; }
            set { _AdmMLDCDetails = value; }
        }

    }

    public class clsGetIPDAdmissionBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDAdmissionBizAction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
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

        private clsIPDAdmissionVO _Details = null;
        public clsIPDAdmissionVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        private List<clsIPDAdmissionVO> _List = null;
        public List<clsIPDAdmissionVO> List
        {
            get { return _List; }
            set { _List = value; }
        }


    }
    public class clsGetIPDAdmissionListBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDAdmissionListBizAction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
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

        private clsIPDAdmissionVO _AdmDetails = null;
        public clsIPDAdmissionVO AdmDetails
        {
            get { return _AdmDetails; }
            set { _AdmDetails = value; }
        }

        private clsIPDAdmMLCDetailsVO _AdmMLDCDetails = null;
        public clsIPDAdmMLCDetailsVO AdmMLDCDetails
        {
            get { return _AdmMLDCDetails; }
            set { _AdmMLDCDetails = value; }
        }

        private List<clsIPDAdmissionVO> _AdmList = null;
        public List<clsIPDAdmissionVO> AdmList
        {
            get { return _AdmList; }
            set { _AdmList = value; }
        }

        private bool _IsMedicoLegalCase = false;
        public bool IsMedicoLegalCase
        {
            get
            {
                return _IsMedicoLegalCase;
            }
            set
            {
                _IsMedicoLegalCase = value;
            }
        }

        private bool _IsDischargeApproval;

        public bool IsDischargeApproval
        {
            get { return _IsDischargeApproval; }
            set { _IsDischargeApproval = value; }
        }

        public string LinkServer { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
    }

    public class clsGetActiveAdmissionBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            // throw new NotImplementedException();

            return "PalashDynamics.BusinessLayer.IPD.clsGetActiveAdmissionBizAction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public string MRNo { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private clsIPDAdmissionVO _Details = null;

        public clsIPDAdmissionVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

    }
    public class clsGetConsentByTempleteIDBizActionVO : IBizActionValueObject 
    {
        private List<clsConsentDetailsVO> _ConsentList;
        public List<clsConsentDetailsVO> ConsentList
        {
            get { return _ConsentList; }
            set { _ConsentList = value; }

        }

        public long ConsentTypeID { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public string GetBizAction()
        {
            // throw new NotImplementedException();

            return "PalashDynamics.BusinessLayer.IPD.clsGetConsentByTempleteIDBizAction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }
    }

    public class clsUpdateAdmissionTypeBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            // throw new NotImplementedException();

            return "PalashDynamics.BusinessLayer.IPD.clsUpdateAdmissionTypeBizAction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
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

        public long AdmID { get; set; }
        public long AdmUnitID { get; set; }

        public long AdmTypeID { get; set; }

        private clsIPDAdmissionVO _UpdateAdmType;
        public clsIPDAdmissionVO UpdateAdmType
        {
            get { return _UpdateAdmType; }
            set { _UpdateAdmType = value; }
        }


    }

    public class clsIPDRoundDetailsBizactionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IPD.clsSaveIPDDoctorAndSpecliztionBizAction";
        }
        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }
        public long ID { get; set; }
        public long AdmisstionId { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string SpecCode { get; set; }
        public string SpecName { get; set; }
    }

    public class ClsIPDCheckRoundExistsBizactionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IPD.ClsIPDCheckRoundExistsBizaction";
        }
        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }
        public long PatientID { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public long VisitId { get; set; }
        public Boolean IsOpdIpd { get; set; }
        public long status { get; set; }

    }

    public class clsCancelIPDAdmissionBizactionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            // throw new NotImplementedException();

            return "PalashDynamics.BusinessLayer.IPD.clsCancelIPDAdmissionBizaction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }

        public long AdmissionID { get; set; }
        public long AdmissionUnitID { get; set; }
        public string MRNo { get; set; }
        public bool UpdateCancelAdmission { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private clsIPDAdmissionVO _AdmissionDetailsVO = null;
        public clsIPDAdmissionVO AdmissionDetailsVO
        {
            get { return _AdmissionDetailsVO; }
            set { _AdmissionDetailsVO = value; }
        }

        private List<clsIPDAdmissionVO> _AdmissionDetailsList = null;
        public List<clsIPDAdmissionVO> AdmissionDetailsList
        {
            get { return _AdmissionDetailsList; }
            set { _AdmissionDetailsList = value; }
        }

    }

    public class clsIPDRoundDoctorBizactionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDDoctorAndSpecliztionBizAction";
        }
        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }
        public long ID { get; set; }
        public long AdmisstionId { get; set; }
        public long PatientID { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string SpecCode { get; set; }
        public string SpecName { get; set; }
    }


}
