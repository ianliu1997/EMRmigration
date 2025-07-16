using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddLabDay5BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddLabDay5BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsFemaleLabDay5VO _Day5Details = null;
        public clsFemaleLabDay5VO Day5Details
        {
            get { return _Day5Details; }
            set { _Day5Details = value; }
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

    public class clsGetLabDay4ForLabDay5BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetLabDay4ForLabDay5BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private List<clsFemaleLabDay5FertilizationAssesmentVO> _Day5Details = new List<clsFemaleLabDay5FertilizationAssesmentVO>();
        public List<clsFemaleLabDay5FertilizationAssesmentVO> Day5Details
        {
            get
            {
                return _Day5Details;
            }
            set
            {
                _Day5Details = value;
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

    public class clsGetDay5DetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay5DetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsFemaleLabDay5VO _LabDay5;
        public clsFemaleLabDay5VO LabDay5
        {
            get
            {
                return _LabDay5;
            }
            set
            {
                _LabDay5 = value;
            }
        }

        public long ID { get; set; }
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

    public class clsGetDay5MediaAndCalcDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay5MediaAndCalcDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private List<clsFemaleMediaDetailsVO> _LabDay5;
        public List<clsFemaleMediaDetailsVO> LabDay5
        {
            get
            {
                return _LabDay5;
            }
            set
            {
                _LabDay5 = value;
            }
        }
        private clsFemaleLabDay5CalculateDetailsVO _LabDayCalDetails;
        public clsFemaleLabDay5CalculateDetailsVO LabDayCalDetails
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

    public class clsGetDay4ScoreBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay4ScoreBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private List<clsFemaleLabDay5FertilizationAssesmentVO> _Day4Score = new List<clsFemaleLabDay5FertilizationAssesmentVO>();
        public List<clsFemaleLabDay5FertilizationAssesmentVO> Day4Score
        {
            get
            {
                return _Day4Score;
            }
            set
            {
                _Day4Score = value;
            }
        }

        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }

        public long DetailID { get; set; }


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
