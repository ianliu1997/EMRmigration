using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsAddUpdatePGDHistoryBizActionVO: IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsAddUpdatePGDHistoryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        private clsPGDHistoryVO _PGDHistoryDetails = new clsPGDHistoryVO();
        public clsPGDHistoryVO PGDHistoryDetails
        {
            get
            {
                return _PGDHistoryDetails;
            }
            set
            {
                _PGDHistoryDetails = value;
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

    public class clsGetPGDHistoryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsGetPGDHistoryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsPGDHistoryVO _PGDDetails = new clsPGDHistoryVO();
        public clsPGDHistoryVO PGDDetails
        {
            get
            {
                return _PGDDetails;
            }
            set
            {
                _PGDDetails = value;
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

    public class clsAddUpdatePGDGeneralDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsAddUpdatePGDGeneralDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        private clsPGDGeneralDetailsVO _PGDGeneralDetails = new clsPGDGeneralDetailsVO();
        public clsPGDGeneralDetailsVO PGDGeneralDetails
        {
            get
            {
                return _PGDGeneralDetails;
            }
            set
            {
                _PGDGeneralDetails = value;
            }
        }

        private List<clsPGDFISHVO> _PGDFISHList = new List<clsPGDFISHVO>();
        public List<clsPGDFISHVO> PGDFISHList
        {
            get
            {
                return _PGDFISHList;
            }
            set
            {
                _PGDFISHList = value;
            }
        }
        private List<clsPGDKaryotypingVO> _PGDKaryotypingList = new List<clsPGDKaryotypingVO>();
        public List<clsPGDKaryotypingVO> PGDKaryotypingList
        {
            get
            {
                return _PGDKaryotypingList;
            }
            set
            {
                _PGDKaryotypingList = value;
            }
        }

        private clsPGDFISHVO _PGDFISHDetails = new clsPGDFISHVO();
        public clsPGDFISHVO PGDFISHDetails
        {
            get
            {
                return _PGDFISHDetails;
            }
            set
            {
                _PGDFISHDetails = value;
            }
        }
        private clsPGDKaryotypingVO _PGDKaryotypingDetails = new clsPGDKaryotypingVO();
        public clsPGDKaryotypingVO PGDKaryotypingDetails
        {
            get
            {
                return _PGDKaryotypingDetails;
            }
            set
            {
                _PGDKaryotypingDetails = value;
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

    public class clsGetPGDGeneralDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsGetPGDGeneralDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        private clsPGDGeneralDetailsVO _PGDGeneralDetails = new clsPGDGeneralDetailsVO();
        public clsPGDGeneralDetailsVO PGDGeneralDetails
        {
            get
            {
                return _PGDGeneralDetails;
            }
            set
            {
                _PGDGeneralDetails = value;
            }
        }

        private List<clsPGDFISHVO> _PGDFISHList = new List<clsPGDFISHVO>();
        public List<clsPGDFISHVO> PGDFISHList
        {
            get
            {
                return _PGDFISHList;
            }
            set
            {
                _PGDFISHList = value;
            }
        }

        private List<clsPGDKaryotypingVO> _PGDKaryotypingList = new List<clsPGDKaryotypingVO>();
        public List<clsPGDKaryotypingVO> PGDKaryotypingList
        {
            get
            {
                return _PGDKaryotypingList;
            }
            set
            {
                _PGDKaryotypingList = value;
            }
        }

        private clsPGDFISHVO _PGDFISHDetails = new clsPGDFISHVO();
        public clsPGDFISHVO PGDFISHDetails
        {
            get
            {
                return _PGDFISHDetails;
            }
            set
            {
                _PGDFISHDetails = value;
            }
        }

        private clsPGDKaryotypingVO _PGDKaryotypingDetails = new clsPGDKaryotypingVO();
        public clsPGDKaryotypingVO PGDKaryotypingDetails
        {
            get
            {
                return _PGDKaryotypingDetails;
            }
            set
            {
                _PGDKaryotypingDetails = value;
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
