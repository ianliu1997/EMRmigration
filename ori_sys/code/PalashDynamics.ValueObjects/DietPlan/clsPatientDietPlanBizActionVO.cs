using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects
{

    public class clsAddPatientDietPlanBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPatientDietPlanBizAction";
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

        private clsPatientDietPlanVO objDetails = new clsPatientDietPlanVO();

        public clsPatientDietPlanVO DietPlan
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }


    public class clsGetPatientDietPlanBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientDietPlanBizAction";
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

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string sortExpression { get; set; }

        private List<clsPatientDietPlanVO> _DietList;
        public List<clsPatientDietPlanVO> DietList
        {
            get { return _DietList; }
            set { _DietList = value; }
        }

        public long PlanID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

    }

    public class clsGetPatientDietPlanDetailsBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientDietPlanDetailsBizAction";
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

        private List<clsPatientDietPlanDetailVO> _DietDetailsList;
        public List<clsPatientDietPlanDetailVO> DietDetailsList
        {
            get { return _DietDetailsList; }
            set { _DietDetailsList = value; }
        }

        public long ID { get; set; }

    }

    public class clsGeDietPlanMasterBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGeDietPlanMasterBizAction";
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

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string sortExpression { get; set; }

        private List<clsPatientDietPlanDetailVO> _DietDetailList;
        public List<clsPatientDietPlanDetailVO> DietDetailList
        {
            get { return _DietDetailList; }
            set { _DietDetailList = value; }
        }

        public long PlanID { get; set; }

        public string GeneralInfo { get; set; }

    }
}
