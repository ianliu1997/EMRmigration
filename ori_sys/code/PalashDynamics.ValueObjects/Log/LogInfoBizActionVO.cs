using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Log
{
    public class LogInfoBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Log.clsLogInfoBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        private LogInfo _LogInformation = new LogInfo();
        public LogInfo LogInformation
        {
            get
            {
                return _LogInformation;
            }
            set
            {
                _LogInformation = value;
            }
        }
    }
}
