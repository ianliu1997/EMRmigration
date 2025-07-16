using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddBirthDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddBirthDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_BirthDetailsVO _BirthDetails = new clsIVFDashboard_BirthDetailsVO();
        public clsIVFDashboard_BirthDetailsVO BirthDetails
        {
            get
            {
                return _BirthDetails;
            }
            set
            {
                _BirthDetails = value;
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


    public class clsIVFDashboard_GetBirthDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetBirthDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_BirthDetailsVO _BirthDetails = new clsIVFDashboard_BirthDetailsVO();
        public clsIVFDashboard_BirthDetailsVO BirthDetails
        {
            get
            {
                return _BirthDetails;
            }
            set
            {
                _BirthDetails = value;
            }
        }
        private List<clsIVFDashboard_BirthDetailsVO> _BirthDetailsList = new List<clsIVFDashboard_BirthDetailsVO>();
        public List<clsIVFDashboard_BirthDetailsVO> BirthDetailsList
        {
            get
            {
                return _BirthDetailsList;
            }
            set
            {
                _BirthDetailsList = value;
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

    public class clsIVFDashboard_DeleteBirthDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_DeleteBirthDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_BirthDetailsVO _BirthDetails = new clsIVFDashboard_BirthDetailsVO();
        public clsIVFDashboard_BirthDetailsVO BirthDetails
        {
            get
            {
                return _BirthDetails;
            }
            set
            {
                _BirthDetails = value;
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

    //added by neena
    public class clsIVFDashboard_GetBirthDetailsMasterListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
             return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetBirthDetailsMasterListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsIVFDashboard_GetBirthDetailsMasterListBizActionVO()
        {

        }
        private MasterTableNameList _MasterTable = MasterTableNameList.None;
        public MasterTableNameList MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }

        public KeyValue Category { get; set; }

        private string _Error = "";
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public KeyValue Parent { get; set; }

        /// <summary>
        /// Datatype: Boolean
        /// Gets or Sets active record from list
        /// </summary>
        public bool? IsActive { get; set; }

        public bool _IsFromPOGRN = false;
        public bool IsFromPOGRN
        {
            get { return _IsFromPOGRN; }
            set { _IsFromPOGRN = value; }
        }


        /// <summary>
        /// Datatype: Boolean
        /// Gets or Sets IsDate 
        /// </summary>
        public bool? IsDate { get; set; }

        public bool IsParameterSearch = false;
        public string parametername = string.Empty;
    }

    public class clsIVFDashboard_GetBirthDetailsListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetBirthDetailsListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_BirthDetailsVO _BirthDetails = new clsIVFDashboard_BirthDetailsVO();
        public clsIVFDashboard_BirthDetailsVO BirthDetails
        {
            get
            {
                return _BirthDetails;
            }
            set
            {
                _BirthDetails = value;
            }
        }
        private List<clsIVFDashboard_BirthDetailsVO> _BirthDetailsList = new List<clsIVFDashboard_BirthDetailsVO>();
        public List<clsIVFDashboard_BirthDetailsVO> BirthDetailsList
        {
            get
            {
                return _BirthDetailsList;
            }
            set
            {
                _BirthDetailsList = value;
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

    public class clsIVFDashboard_AddBirthDetailsListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddBirthDetailsListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_BirthDetailsVO _BirthDetails = new clsIVFDashboard_BirthDetailsVO();
        public clsIVFDashboard_BirthDetailsVO BirthDetails
        {
            get
            {
                return _BirthDetails;
            }
            set
            {
                _BirthDetails = value;
            }
        }

        private List<clsIVFDashboard_BirthDetailsVO> _BirthDetailsList = new List<clsIVFDashboard_BirthDetailsVO>();
        public List<clsIVFDashboard_BirthDetailsVO> BirthDetailsList
        {
            get
            {
                return _BirthDetailsList;
            }
            set
            {
                _BirthDetailsList = value;
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
    //
}
