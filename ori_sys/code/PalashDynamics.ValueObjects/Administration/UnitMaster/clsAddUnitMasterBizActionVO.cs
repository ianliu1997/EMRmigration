using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.UnitMaster
{
    public class clsCheckLicenseToBizActionVO : IBizActionValueObject
    {
        private clsUnitMasterVO _UnitDetails;
        public clsUnitMasterVO UnitDetails
        {
            get { return _UnitDetails; }
            set { _UnitDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.UnitMaster.clsCheckLicenseBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

   

   public class clsAddUnitMasterBizActionVO : IBizActionValueObject
    {
        private clsUnitMasterVO _UnitDetails;
        public clsUnitMasterVO UnitDetails
        {
            get { return _UnitDetails; }
            set { _UnitDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.UnitMaster.clsAddUnitMasterBizAction";
           
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
