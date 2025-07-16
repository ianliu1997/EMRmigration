using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddLabDay6BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddLabDay6BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsFemaleLabDay6VO _Day6Details = null;
        public clsFemaleLabDay6VO Day6Details
        {
            get { return _Day6Details; }
            set { _Day6Details = value; }
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

    public class clsGetDay6DetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay6DetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsFemaleLabDay6VO _LabDay6;
        public clsFemaleLabDay6VO LabDay6
        {
            get
            {
                return _LabDay6;
            }
            set
            {
                _LabDay6 = value;
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

    public class clsGetLabDay5ForLabDay6BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetLabDay5ForLabDay6BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private List<clsFemaleLabDay6FertilizationAssesmentVO> _Day6Details = new List<clsFemaleLabDay6FertilizationAssesmentVO>();
        public List<clsFemaleLabDay6FertilizationAssesmentVO> Day6Details
        {
            get
            {
                return _Day6Details;
            }
            set
            {
                _Day6Details = value;
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

    public class clsGetDay5ScoreBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay5ScoreBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private List<clsFemaleLabDay6FertilizationAssesmentVO> _Day5Score = new List<clsFemaleLabDay6FertilizationAssesmentVO>();
        public List<clsFemaleLabDay6FertilizationAssesmentVO> Day5Score
        {
            get
            {
                return _Day5Score;
            }
            set
            {
                _Day5Score = value;
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
