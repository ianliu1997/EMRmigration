using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddLabDay2BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddLabDay2BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsFemaleLabDay2VO _Day2Details = new clsFemaleLabDay2VO();
        public clsFemaleLabDay2VO Day2Details
        {
            get
            {
                return _Day2Details;
            }
            set
            {
                _Day2Details = value;
            }
        }
        
        public long Day;

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

    public class clsGetLabDay1ForLabDay2BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetLabDay1ForLabDay2BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private List<clsFemaleLabDay2FertilizationAssesmentVO> _Day2Details = new List<clsFemaleLabDay2FertilizationAssesmentVO>();
        public List<clsFemaleLabDay2FertilizationAssesmentVO> Day2Details
        {
            get
            {
                return _Day2Details;
            }
            set
            {
                _Day2Details = value;
            }
        }

        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }

        public long Day;

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
        //Added By Saily P

        private long _EmbryologistID;
        public long EmbryologistID
        {
            get { return _EmbryologistID; }
            set
            {
                if (_EmbryologistID != value)
                {
                    _EmbryologistID = value;
                    //  OnPropertyChanged("EmbryologistID");
                }
            }
        }

        private long _AnaesthetistID;
        public long AnaesthetistID
        {
            get { return _AnaesthetistID; }
            set
            {
                if (_AnaesthetistID != value)
                {
                    _AnaesthetistID = value;
                    // OnPropertyChanged("AnaesthetistID");
                }
            }
        }
        private long _AssEmbryologistID;
        public long AssEmbryologistID
        {
            get { return _AssEmbryologistID; }
            set
            {
                if (_AssEmbryologistID != value)
                {
                    _AssEmbryologistID = value;
                    // OnPropertyChanged("AssEmbryologistID");
                }
            }
        }

        private long _AssAnaesthetistID;
        public long AssAnaesthetistID
        {
            get { return _AssAnaesthetistID; }
            set
            {
                if (_AssAnaesthetistID != value)
                {
                    _AssAnaesthetistID = value;
                    // OnPropertyChanged("AssAnaesthetistID");
                }
            }
        }
        //
       
    }


    public class clsGetDay2DetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay2DetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsFemaleLabDay2VO _LabDay2;
        public clsFemaleLabDay2VO LabDay2
        {
            get
            {
                return _LabDay2;
            }
            set
            {
                _LabDay2 = value;
            }
        }

        public long ID { get; set; }
        public long UnitID { get; set; }
        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }
        public long Day { get; set; }
        public int LabDay { get; set; }


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


    public class clsGetDay2MediaAndCalcDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay2MediaAndCalcDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private List<clsFemaleMediaDetailsVO> _LabDay2;
        public List<clsFemaleMediaDetailsVO> LabDay2
        {
            get
            {
                return _LabDay2;
            }
            set
            {
                _LabDay2 = value;
            }
        }
        
        private clsFemaleLabDay2CalculateDetailsVO _LabDayCalDetails;
        public clsFemaleLabDay2CalculateDetailsVO LabDayCalDetails
        {
            get
            {
                return _LabDayCalDetails;
            }
            set
            {
                _LabDayCalDetails = value;
            }
        }

        public long ID { get; set; }
        public long DetailID { get; set; }
        public long UnitID { get; set; }
        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }

        public int LabDay { get; set; }


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


    public class clsGetCleavageGradeMasterListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetCleavageGradeMasterListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private List<MasterListItem> _CleavageGradeList;
        public List<MasterListItem> CleavageGradeList
        {
            get
            {
                return _CleavageGradeList;
            }
            set
            {
                _CleavageGradeList = value;
            }
        }

        private clsCleavageGradeMasterVO _CleavageGrade;
        public clsCleavageGradeMasterVO CleavageGrade
        {
            get
            {
                return _CleavageGrade;
            }
            set
            {
                _CleavageGrade = value;
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

    public class clsGetLab5_6MasterListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetLab5And6MasterListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsGetLab5_6MasterListBizActionVO()
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


}
