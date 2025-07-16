using System;
using System.Collections.Generic;

namespace PalashDynamics.ValueObjects.EMR.GrowthChart
{

    public class clsGetPatientGrowthChartBizActionVO : IBizActionValueObject
    {
        public long PatientUnitID { get; set; }
        public long DrID { get; set; }
        public long UnitID { get; set; }
        public long PatientID { get; set; }
        public long GenderID { get; set; }
        public long Id { get; set; }
        public DateTime Date { get; set; }

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.GrowthChart.clsGetPatientGrowthChartBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        private List<clsGrowthChartVO> objGrowthChartDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsGrowthChartVO> GrowthChartDetailList
        {
            get { return objGrowthChartDetail; }
            set { objGrowthChartDetail = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        private long _ResultStatus;
        public long ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }
    }

    public class clsGrowthChartVO
    {
        public long Id { get; set; }
        public string OPD { get; set; }
        public string DrName { get; set; }
        public string VisitType { get; set; }
        public string ChiefComplaint { get; set; }

        public long DrID { get; set; }
        public long UnitID { get; set; }
        public long VisitID { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime DOB { get; set; }
        public string AgeInYearMonthDays { get; set; }
        public long AgeInMonth { get; set; }
        public long Age { get; set; }

        public Double Height { get; set; }
        public Double Weight { get; set; }
        public Double BMI { get; set; }

        public Double Temprature { get; set; }
        public Double SBP { get; set; }
        public Double DBP { get; set; }
        public Double Spo2 { get; set; }
        public Double RR { get; set; }
        public Double HC { get; set; }
        public Double Pulse { get; set; }
        public string Gender { get; set; }

        public string MRNo { get; set; }
        public long GenderID { get; set; }
        public string PatientName { get; set; }
        public long MobileCountryCode { get; set; }
        public string MOB { get; set; }
        public bool ViewDetails { get; set; }

    }

    public class clsGetPatientGrowthChartMonthlyBizActionVO : IBizActionValueObject
    {
        public long PatientUnitID { get; set; }
        public long DrID { get; set; }
        public long UnitID { get; set; }
        public long PatientID { get; set; }
        public long GenderID { get; set; }
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public Boolean IsOPDIPD { get; set; }

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.GrowthChart.clsGetPatientGrowthChartMonthlyBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        private List<clsGrowthChartVO> objGrowthChartDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsGrowthChartVO> GrowthChartDetailList
        {
            get { return objGrowthChartDetail; }
            set { objGrowthChartDetail = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        private long _ResultStatus;
        public long ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }
    }

    public class clsGetPatientGrowthChartYearlyBizActionVO : IBizActionValueObject
    {
        public long PatientUnitID { get; set; }
        public long DrID { get; set; }
        public long UnitID { get; set; }
        public long PatientID { get; set; }
        public long GenderID { get; set; }
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public Boolean IsopdIPd { get; set; }

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.GrowthChart.clsGetPatientGrowthChartYearlyBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        private List<clsGrowthChartVO> objGrowthChartDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsGrowthChartVO> GrowthChartDetailList
        {
            get { return objGrowthChartDetail; }
            set { objGrowthChartDetail = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        private long _ResultStatus;
        public long ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }
    }

}
