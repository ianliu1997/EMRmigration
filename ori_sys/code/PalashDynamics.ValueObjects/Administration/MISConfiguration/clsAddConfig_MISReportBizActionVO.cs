using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
   public  class clsAddConfig_MISReportBizActionVO:IBizActionValueObject
    {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.MISConfiguration.clsAddConfig_MISReportBizAction";
       }

       public string ToXml()
       {
           return this.ToString();
       }

       private int _SuccessStatus;

       public int SuccessStatus
       {
           get { return _SuccessStatus;}
           set{_SuccessStatus =value;}
       }

       private clsMISConfigurationVO objMISConfig = null;

       public clsMISConfigurationVO AddMISConfig
       {
           get { return objMISConfig; }
           set { value = objMISConfig; }
       }



     
    }
}
