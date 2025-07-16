using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.EMR.NewEMR
{
    public class clsAddUpdateDeleteVitalDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddUpdateDeleteVitalDetailsBizAction";
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

        public String DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsOPDIPD { get; set; }
        public long VisitID { get; set; }
        public long TakenBy { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long UnitID { get; set; }

        public long PatientDiagnosisID { get; set; }

        private List<clsEMRVitalsVO> _objVitalDetails = null;

        public List<clsEMRVitalsVO> PatientVitalDetails
        {
            get { return _objVitalDetails; }
            set { _objVitalDetails = value; }
        }


    }

    public class clsGetVitalListDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetVitalListDetailsBizAction";
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

        public long VisitID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long UnitID { get; set; }

        private List<clsEMRVitalsVO> objListDetail = null;

        public List<clsEMRVitalsVO> vitalListDetail
        {
            get { return objListDetail; }
            set { objListDetail = value; }
        }


    }

    public class clsGetPatientVitalChartBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientVitalChartBizAction";
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
        private int _MaximumRows;

        public int MaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {

                _PatientID = value;
            }
        }

        public bool IsFromDashBoard { get; set; }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {

                _PatientUnitID = value;
            }
        }


        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {

                _VisitID = value;


            }
        }

        public Boolean IsOPDIPD { get; set; }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                _UnitID = value;
            }
        }
        private DateTime _VisitDate = DateTime.Now;
        public DateTime VisitDate
        {
            get
            {
                return _VisitDate;
            }
            set
            {
                if (value != _VisitDate)
                {
                    _VisitDate = value;
                }
            }
        }


        private clsVitalChartVO objPatientVitalChart;
        public clsVitalChartVO PatientVitalChart
        {
            get { return objPatientVitalChart; }
            set { objPatientVitalChart = value; }
        }
        private List<clsVitalChartVO> objPatientVitalChartList;
        public List<clsVitalChartVO> PatientVitalChartlst
        {
            get { return objPatientVitalChartList; }
            set { objPatientVitalChartList = value; }
        }
    }

    public class clsVitalChartVO
    {
        public string NutritionStatus { get; set; }
        public DateTime Date { get; set; }
        public string Datetime { get; set; }
        public double BMI { get; set; }
        public double Pulse { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double SystolicBP { get; set; }
        public double DiastolicBP { get; set; }
        public double Temperature { get; set; }
        public double Waistgirth { get; set; }
        public double Hipgirth { get; set; }
        public double RBS { get; set; }
        public double O2 { get; set; }
        public double RR { get; set; }
        public double HC { get; set; }
        public double TotalCholesterol { get; set; }
        public double RandomBloodSugar { get; set; }
        public double FastingSugar { get; set; }
        public double HeadCircumference { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpeclization { get; set; }
    }
}
