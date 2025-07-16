using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects
{
    
    public class clsAddPrintedPatientConscentBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPrintedPatientConscentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
       
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsPatientConsentVO objDetails = new clsPatientConsentVO();

        public clsPatientConsentVO ConsentDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }

    public class clsGetPrintedPatientConscentBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPrintedPatientConscentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        //public long ID { get; set; }
        //public long UnitID { get; set; }
        private clsPatientConsentVO objDetails = new clsPatientConsentVO();

        public clsPatientConsentVO ConsentDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }

    public class clsGetPatientConsentBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientConsentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsPatientConsentVO> objConsentMaster = new List<clsPatientConsentVO>();
        public List<clsPatientConsentVO> ConsentMatserDetails
        {
            get
            {
                return objConsentMaster;
            }
            set
            {
                objConsentMaster = value;

            }
        }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public long DepartmentID { get; set; }
        public long Template { get; set; }

        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }
            set
            {
                _PatientID = value;
            }
        }

    }

    // BY BHUSHAN . . . . .  
    public class clsADDPatientSignConsentBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsADDPatientSignConsentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccesssignStatus;
        public int SuccesssignStatus
        {
            get { return _SuccesssignStatus; }
            set { _SuccesssignStatus = value; }
        }

        private clsPatientSignConsentVO objDetails = new clsPatientSignConsentVO();

        public clsPatientSignConsentVO signConsentDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<clsPatientSignConsentVO> objUpload = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientSignConsentVO> UploadDetails
        {
            get { return objUpload; }
            set { objUpload = value; }
        }

    }

    public class clsGetPatientSignConsentBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientSignConsentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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

        private List<clsPatientSignConsentVO> objInvoiceLIst = null;
        public List<clsPatientSignConsentVO> SignPatientList
        {
            get { return objInvoiceLIst; }
            set { objInvoiceLIst = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }

        public long ConsentID { get; set; }
        public long ConsentUnitID { get; set; }

        
        public Boolean Status { get; set; }
        //added by neena
        private long _PlanTherapyID;
        public long PlanTherapyID
        {
            get { return _PlanTherapyID; }
            set
            {
                if (_PlanTherapyID != value)
                {
                    _PlanTherapyID = value;                    
                }
            }
        }

        private long _PlanTherapyUnitID;
        public long PlanTherapyUnitID
        {
            get { return _PlanTherapyUnitID; }
            set
            {
                if (_PlanTherapyUnitID != value)
                {
                    _PlanTherapyUnitID = value;                 
                }
            }
        }

        //
 
    }

    public class clsDeletePatientSignConsentBizActionVO : IBizActionValueObject 
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsDeletePatientSignConsentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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

        private List<clsPatientSignConsentVO> objInvoiceLIst = null;
        public List<clsPatientSignConsentVO> SignPatientList
        {
            get { return objInvoiceLIst; }
            set { objInvoiceLIst = value; }
        }

        public long ID { get; set; }
        public long UnitID { get; set; }

        private clsPatientSignConsentVO objDeleteVo = new clsPatientSignConsentVO();
        public clsPatientSignConsentVO DeleteVO
        {
            get { return objDeleteVo; }
            set { objDeleteVo = value; }
        }
    }

    //added by neena
    public class clsGetIVFPackegeConsentBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetIVFPackegeConsentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsPatientConsentVO> objConsentMaster = new List<clsPatientConsentVO>();
        public List<clsPatientConsentVO> ConsentMatserDetails
        {
            get
            {
                return objConsentMaster;
            }
            set
            {
                objConsentMaster = value;

            }
        }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public long DepartmentID { get; set; }
        public long Template { get; set; }

        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }
            set
            {
                _PatientID = value;
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get
            {
                return _PatientUnitID;
            }
            set
            {
                _PatientUnitID = value;
            }
        }

        private long _PlanTherapyId;
        public long PlanTherapyId
        {
            get
            {
                return _PlanTherapyId;
            }
            set
            {
                if (value != _PlanTherapyId)
                {
                    _PlanTherapyId = value;                   
                }
            }
        }

        private long _PlanTherapyUnitId;
        public long PlanTherapyUnitId
        {
            get
            {
                return _PlanTherapyUnitId;
            }
            set
            {
                if (value != _PlanTherapyUnitId)
                {
                    _PlanTherapyUnitId = value;                   
                }
            }
        }


    }

    public class clsAddUpdateIVFPackegeConsentBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddUpdateIVFPackegeConsentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsPatientConsentVO> objConsentMaster = new List<clsPatientConsentVO>();
        public List<clsPatientConsentVO> ConsentMatserDetails
        {
            get
            {
                return objConsentMaster;
            }
            set
            {
                objConsentMaster = value;

            }
        }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public long DepartmentID { get; set; }
        public long Template { get; set; }
        public bool UpdateConsentCheckInPlanTherapy { get; set; }
        public bool ConsentCheck { get; set; }
        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }
            set
            {
                _PatientID = value;
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get
            {
                return _PatientUnitID;
            }
            set
            {
                _PatientUnitID = value;
            }
        }

        private long _PlanTherapyId;
        public long PlanTherapyId
        {
            get
            {
                return _PlanTherapyId;
            }
            set
            {
                if (value != _PlanTherapyId)
                {
                    _PlanTherapyId = value;
                }
            }
        }

        private long _PlanTherapyUnitId;
        public long PlanTherapyUnitId
        {
            get
            {
                return _PlanTherapyUnitId;
            }
            set
            {
                if (value != _PlanTherapyUnitId)
                {
                    _PlanTherapyUnitId = value;
                }
            }
        }


    }

    public class clsGetIVFSavedPackegeConsentBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetIVFSavedPackegeConsentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsPatientConsentVO> objConsentMaster = new List<clsPatientConsentVO>();
        public List<clsPatientConsentVO> ConsentMatserDetails
        {
            get
            {
                return objConsentMaster;
            }
            set
            {
                objConsentMaster = value;

            }
        }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public long DepartmentID { get; set; }
        public long Template { get; set; }

        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }
            set
            {
                _PatientID = value;
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get
            {
                return _PatientUnitID;
            }
            set
            {
                _PatientUnitID = value;
            }
        }

        private long _PlanTherapyId;
        public long PlanTherapyId
        {
            get
            {
                return _PlanTherapyId;
            }
            set
            {
                if (value != _PlanTherapyId)
                {
                    _PlanTherapyId = value;
                }
            }
        }

        private long _PlanTherapyUnitId;
        public long PlanTherapyUnitId
        {
            get
            {
                return _PlanTherapyUnitId;
            }
            set
            {
                if (value != _PlanTherapyUnitId)
                {
                    _PlanTherapyUnitId = value;
                }
            }
        }


    }
    //
}
