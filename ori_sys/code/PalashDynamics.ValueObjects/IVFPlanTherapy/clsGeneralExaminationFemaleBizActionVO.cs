using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
   public class clsAddGeneralExaminationFemaleBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddGeneralExaminationFemaleBizAction";
        }

        public string ToXml()
        {
            return this.ToString();

        }

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

        private clsGeneralExaminationVO _Details = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsGeneralExaminationVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }




    }


   public class clsGetGeneralExaminationFemaleBizActionVO : IBizActionValueObject
   {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetGeneralExaminationFemaleBizAction";
       }

       public string ToXml()
       {
           return this.ToString();

       }

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


       public long PatientID { get; set; }

       public long UnitID { get; set; }
       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }


       private List<clsGeneralExaminationVO> _List = null;
       /// <summary>
       /// Output Property.
       /// This Property Contains OPDPatient Details Which is Added.
       /// </summary>
       public List<clsGeneralExaminationVO> List
       {
           get { return _List; }
           set { _List = value; }
       }




   }

}
