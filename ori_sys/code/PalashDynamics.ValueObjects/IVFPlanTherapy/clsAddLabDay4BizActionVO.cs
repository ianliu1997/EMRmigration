using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddLabDay4BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddLabDay4BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsFemaleLabDay4VO _Day4Details = null;
        public clsFemaleLabDay4VO Day4Details
        {
            get { return _Day4Details; }
            set { _Day4Details = value; }
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

    public class clsGetLabDay3ForLabDay4BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetLabDay3ForLabDay4BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private List<clsFemaleLabDay4FertilizationAssesmentVO> _Day4Details = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
        public List<clsFemaleLabDay4FertilizationAssesmentVO> Day4Details
        {
            get
            {
                return _Day4Details;
            }
            set
            {
                _Day4Details = value;
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


    public class clsGetDay4DetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay4DetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsFemaleLabDay4VO _LabDay4;
        public clsFemaleLabDay4VO LabDay4
        {
            get
            {
                return _LabDay4;
            }
            set
            {
                _LabDay4 = value;
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


    public class clsGetDay4MediaAndCalcDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay4MediaAndCalcDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private List<clsFemaleMediaDetailsVO> _LabDay4;
        public List<clsFemaleMediaDetailsVO> LabDay4
        {
            get
            {
                return _LabDay4;
            }
            set
            {
                _LabDay4 = value;
            }
        }
        private clsFemaleLabDay4CalculateDetailsVO _LabDayCalDetails;
        public clsFemaleLabDay4CalculateDetailsVO LabDayCalDetails
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

    public class clsGetDay3ScoreBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay3ScoreBizAction";
        }
        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        
        private List<clsFemaleLabDay4FertilizationAssesmentVO> _Day3Score = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
        public List<clsFemaleLabDay4FertilizationAssesmentVO> Day3Score
        {
            get
            {
                return _Day3Score;
            }
            set
            {
                _Day3Score = value;
            }
        }
        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }
        public long DetailID { get; set; }
        public long Day { get; set; } 
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
