using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.EMR.NewEMR;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.EMR
{
    public class  clsGetPatientDiagnosisDataBizActionVO  : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientDiagnosisDataBizAction";
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
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public long VisitID { get; set; }
        public bool IsICDX { get; set; }
        public bool IsICDXhistory { get; set; }
        public bool IsOPDIPD { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public bool Ishistory { get; set; }
        public long UnitID { get; set; }
        public bool ISDashBoard { get; set; }
        private int _TotalRows;
        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }
        private bool _PagingEnabled;
        private int _StartRowIndex = 0;

        public int StartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }
        private int _MaximumRows = 10;

        public int MaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }

        public bool PagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }

        private List<clsEMRAddDiagnosisVO> objPatientDiagnosis = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsEMRAddDiagnosisVO> PatientDiagnosisDetails
        {
            get { return objPatientDiagnosis; }
            set { objPatientDiagnosis = value; }
        }

    }

    public class clsGetPatientProcedureDataBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientProcedureDataBizAction";
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

        public long VisitID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long PatientID { get; set; }

        public bool IsOPDIPD { get; set; }
        private int _TotalRows;
        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }
        private int _StartRowIndex = 0;

        public int StartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }
        private int _MaximumRows = 10;

        public int MaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }

        private bool _PagingEnabled;

        public bool PagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }
        private List<clsEMRAddDiagnosisVO> objPatientDiagnosis = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsEMRAddDiagnosisVO> PatientDiagnosisDetails
        {
            get { return objPatientDiagnosis; }
            set { objPatientDiagnosis = value; }
        }

    }
}
