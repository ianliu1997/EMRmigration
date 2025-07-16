using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
   public  class clsComboMasterBizActionVO : NotificationModel
    {
       public clsComboMasterBizActionVO()
       {

       }

       private long _ID;
       public long ID
       {
           get { return _ID; }
           set {_ID=value; }
       }

       private string _Value;
       public string Value
       {
           get { return _Value; }
           set { _Value = value; }
       }

       private string _EmailId;
       public string EmailId
       {
           get { return _EmailId; }
           set { _EmailId = value; }
       }
       
       
      
    }
}
