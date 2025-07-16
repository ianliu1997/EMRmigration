using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsIPDGetAdmissionTypeMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDGetAdmissionTypeMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDAdmissionTypeVO> objAdmissionTypeMaster = new List<clsIPDAdmissionTypeVO>();
        public List<clsIPDAdmissionTypeVO> objAdmissionTypeMasterDetails
        {
            get
            {
                return objAdmissionTypeMaster;
            }
            set
            {
                objAdmissionTypeMaster = value;
            }
        }

        private clsIPDAdmissionTypeVO _objAdmissionTypeDetails = null;
        public clsIPDAdmissionTypeVO AdmissionTypeMasterDetails
        {
            get { return _objAdmissionTypeDetails; }
            set { _objAdmissionTypeDetails = value; }

        }

        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public long SearchCategory { get; set; }
        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
    }

    public class clsIPDAddUpdateAdmissionTypeMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDAddUpdateAdmissionTypeMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsIPDAdmissionTypeVO _objAdmissionTypeDetails = null;
        public clsIPDAdmissionTypeVO AdmissionTypeMasterDetails
        {
            get { return _objAdmissionTypeDetails; }
            set { _objAdmissionTypeDetails = value; }

        }

        public bool IsStatus { get; set; }

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

    }
}
