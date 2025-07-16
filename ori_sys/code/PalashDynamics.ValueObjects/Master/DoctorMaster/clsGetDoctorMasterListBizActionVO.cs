using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
   public  class clsGetDoctorMasterListBizActionVO:IBizActionValueObject
    {
       public clsGetDoctorMasterListBizActionVO()
       {

       }
        
       private List<clsComboMasterBizActionVO> _ComboList = new List<clsComboMasterBizActionVO>();
       public List<clsComboMasterBizActionVO> ComboList
       {
           get { return _ComboList; }
           set { _ComboList = value; }
       }

       public long ID { get; set; }

       private bool _IsDecode;
       public bool IsDecode
       {
           get { return _IsDecode; }
           set { _IsDecode = value; }
       }

       public bool _SuccessStatus;
       public bool SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private const string _BizActionName = "PalashDynamics.BusinessLayer.Master.clsGetDoctorMasterListBizAction";



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return _BizActionName;
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
