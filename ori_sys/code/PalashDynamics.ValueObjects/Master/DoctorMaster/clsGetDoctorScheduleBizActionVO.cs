using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
   public class clsGetDoctorScheduleBizActionVO:IBizActionValueObject
    {
       public clsGetDoctorScheduleBizActionVO()
       {

       }


       private clsDoctorVO ObjDoctorVO = null;
       public clsDoctorVO DoctorDetails
       {
           get { return ObjDoctorVO; }
           set { ObjDoctorVO = value; }
       }

       //private List<clsDoctorVO> myVar = new List<clsDoctorVO>(); 

       //public List<clsDoctorVO> DoctorDetailsList
       //{
       //    get { return myVar; }
       //    set { myVar = value; }
       //}
      
       private string _DoctorName;
       public string DoctorName
       {
           get { return _DoctorName; }
           set { _DoctorName = value; }
       }

       private DateTime? _AppDate;
       public DateTime? AppDate
       {
           get { return _AppDate ; }

           set { _AppDate = value; }
       }

       private string _Description;
       public string Description
       {
           get { return _Description; }
           set { _Description = value; }
       }


       private string _UnitName;
       public string UnitName
       {
           get { return _UnitName; }
           set { _UnitName = value; }
       }

       private long _DepartmentId;
       public long DepartmentId
       {
           get { return _DepartmentId; }
           set { _DepartmentId = value; }
       }

      private long _UnitId;
      public long UnitId
      {
          get { return _UnitId; }
          set { _UnitId = value; }
      }


      private int _SuccessStatus;
      public int SuccessStatus
      {
          get { return _SuccessStatus; }
          set { _SuccessStatus = value; }
      }




      private string _SearchExpression;
      public string SearchExpression
      {
          get { return _SearchExpression; }
          set { _SearchExpression = value; }
      }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorScheduleBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
