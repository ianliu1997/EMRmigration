using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{

    public class clsAddProcedureMasterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddProcedureMasterBizAction";
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

        private clsProcedureMasterVO objProcDetails = null;
        public clsProcedureMasterVO ProcDetails
        {
            get { return objProcDetails; }
            set { objProcDetails = value; }
        }

        
    }

    public class clsGetProcedureMasterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetProcedureMasterBizAction";
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

        private List<clsProcedureMasterVO> objProcDetails = null;
        public List<clsProcedureMasterVO> ProcDetails
        {
            get { return objProcDetails; }
            set { objProcDetails = value; }
        }
        public string Description { get; set; }
        public long? ProcedureTypeID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string SortExpression { get; set; }

    }

    public class clsGetProcDetailsByProcIDBizActionVO : IBizActionValueObject

    {

        private List<clsProcedureConsentDetailsVO> _ConsentList = null;
        public List<clsProcedureConsentDetailsVO> ConsentList
        {
            get
            {
                return _ConsentList;
            }
            set
            {
                _ConsentList = value;
            }
        }

        private List<clsProcedureInstructionDetailsVO> _InstructionList = null;
        public List<clsProcedureInstructionDetailsVO> InstructionList
        {
            get
            {
                return _InstructionList;
            }
            set
            {
                _InstructionList = value;
            }
        }
        private List<clsProcedureInstructionDetailsVO> _PreInstructionList = null;
        public List<clsProcedureInstructionDetailsVO> PreInstructionList
        {
            get
            {
                return _PreInstructionList;
            }
            set
            {
                _PreInstructionList = value;
            }
        }
        private List<clsProcedureInstructionDetailsVO> _PostInstructionList = null;
        public List<clsProcedureInstructionDetailsVO> PostInstructionList
        {
            get
            {
                return _PostInstructionList;
            }
            set
            {
                _PostInstructionList = value;
            }
        }
        private List<clsProcedureItemDetailsVO> _ItemList = null;
        public List<clsProcedureItemDetailsVO> ItemList
        {
            get
            {
                return _ItemList;
            }
            set
            {
                _ItemList = value;
            }
        }

        private List<clsDoctorSuggestedServiceDetailVO> _ServiceList = null;
        public List<clsDoctorSuggestedServiceDetailVO> ServiceList
        {
            get
            {
                return _ServiceList;
            }
            set
            {
                _ServiceList = value;
            }
        }

        private List<clsProcedureStaffDetailsVO> _StaffList = null;
        public List<clsProcedureStaffDetailsVO> StaffList
        {
            get
            {
                return _StaffList;
            }
            set
            {
                _StaffList = value;
            }
        }

        private List<clsProcedureChecklistDetailsVO> _CheckList = null;
        public List<clsProcedureChecklistDetailsVO> CheckList
        {
            get
            {
                return _CheckList;
            }
            set
            {
                _CheckList = value;
            }
        }

        private List<clsProcedureEquipmentDetailsVO> _EquipList = null;
        public List<clsProcedureEquipmentDetailsVO> EquipList
        {
            get
            {
                return _EquipList;
            }
            set
            {
                _EquipList = value;
            }
        }


        public long ProcID { get; set; }
        public long UnitID { get; set; }

        private List<clsProcedureTemplateDetailsVO> _ProcedureTempalateList = new List<clsProcedureTemplateDetailsVO>();
        public List<clsProcedureTemplateDetailsVO> ProcedureTempalateList
        {
            get
            {
                return _ProcedureTempalateList;
            }
            set
            {
                _ProcedureTempalateList = value;
            }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetProcDetailsByProcIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsUpdStatusProcedureMasterBizActionVO : IBizActionValueObject
    {
        private clsProcedureMasterVO objDetails = null;
        public clsProcedureMasterVO ProcedureObj
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<clsProcedureMasterVO> ProcedureObjList = new List<clsProcedureMasterVO>();
        public List<clsProcedureMasterVO> ProcedureObjDetailList
        {
            get { return ProcedureObjList; }
            set { ProcedureObjList = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsUpdStatusProcedureMasterBizAction";

        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
    }

    public class clsGetServicesForProcedureBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetServicesForProcedureBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion



        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }



    }
}
