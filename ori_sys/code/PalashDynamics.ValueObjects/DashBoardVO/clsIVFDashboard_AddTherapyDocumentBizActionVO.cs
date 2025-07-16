using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddTherapyDocumentBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddTherapyDocumentBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsIVFDashboard_TherapyDocumentVO _Details = new clsIVFDashboard_TherapyDocumentVO();
        public clsIVFDashboard_TherapyDocumentVO Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
            }
        }
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
    public class clsIVFDashboard_GetTherapyDocumentBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetTherapyDocumentBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsIVFDashboard_TherapyDocumentVO _Details = new clsIVFDashboard_TherapyDocumentVO();
        public clsIVFDashboard_TherapyDocumentVO Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
            }
        }
        private List<clsIVFDashboard_TherapyDocumentVO> _DetailList = new List<clsIVFDashboard_TherapyDocumentVO>();
        public List<clsIVFDashboard_TherapyDocumentVO> DetailList
        {
            get
            {
                return _DetailList;
            }
            set
            {
                _DetailList = value;
            }
        }
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
    public class clsIVFDashboard_DeleteTherapyDocumentBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_DeleteTherapyDocumentBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsIVFDashboard_TherapyDocumentVO _Details = new clsIVFDashboard_TherapyDocumentVO();
        public clsIVFDashboard_TherapyDocumentVO Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
            }
        }

        private List<clsIVFDashboard_TherapyDocumentVO> _DetailsList = new List<clsIVFDashboard_TherapyDocumentVO>();
        public List<clsIVFDashboard_TherapyDocumentVO> DetailsList
        {
            get
            {
                return _DetailsList;
            }
            set
            {
                _DetailsList = value;
            }
        }
     
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
