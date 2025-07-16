using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
   public class clsAddLabDay1BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddLabDay1BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsFemaleLabDay1VO _Day1Details = new clsFemaleLabDay1VO();
        public clsFemaleLabDay1VO Day1Details
        {
            get
            {
                return _Day1Details;
            }
            set
            {
                _Day1Details = value;
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
        public bool IsUpdate { get; set; }
    }

   public class clsGetLabDay0ForLabDay1BizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetLabDay0ForLabDay1BizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion


       private List<clsFemaleLabDay1FertilizationAssesmentVO> _Day1Details = new List<clsFemaleLabDay1FertilizationAssesmentVO>();
       public List<clsFemaleLabDay1FertilizationAssesmentVO> Day1Details
       {
           get
           {
               return _Day1Details;
           }
           set
           {
               _Day1Details = value;
           }
       }

       public long CoupleID { get; set; }
       public long CoupleUnitID { get; set; }

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

   public class clsGetDay1ScoreBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay1ScoreBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion


       private List<clsFemaleLabDay1FertilizationAssesmentVO> _Day0Score = new List<clsFemaleLabDay1FertilizationAssesmentVO>();
       public List<clsFemaleLabDay1FertilizationAssesmentVO> Day0Score
       {
           get
           {
               return _Day0Score;
           }
           set
           {
               _Day0Score = value;
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

   public class clsGetDay1DetailsBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay1DetailsBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion


       private clsFemaleLabDay1VO _LabDay1 ;
       public  clsFemaleLabDay1VO LabDay1
       {
           get
           {
               return _LabDay1;
           }
           set
           {
               _LabDay1 = value;
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

   public class clsGetDay1MediaAndCalcDetailsBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDay1MediaAndCalcDetailsBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion


       private List<clsFemaleMediaDetailsVO> _LabDay1;
       public List<clsFemaleMediaDetailsVO> LabDay1
       {
           get
           {
               return _LabDay1;
           }
           set
           {
               _LabDay1 = value;
           }
       }
       private clsFemaleLabDay1CalculateDetailsVO _LabDayCalDetails;
       public clsFemaleLabDay1CalculateDetailsVO LabDayCalDetails
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

   public class clsGetAllDayMediaDetailsBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetAllDayMediaDetailsBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion


       private List<clsFemaleMediaDetailsVO> _MediaList;
       public List<clsFemaleMediaDetailsVO> MediaList
       {
           get
           {
               return _MediaList;
           }
           set
           {
               _MediaList = value;
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
}
