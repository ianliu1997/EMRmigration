using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsAddUpdateSurrogactAgencyMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsAddUpdateSurrogactAgencyMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        

        private clsSurrogateAgencyMasterVO _AgencyDetails = new clsSurrogateAgencyMasterVO();
        public clsSurrogateAgencyMasterVO AgencyDetails
        {
            get
            {
                return _AgencyDetails;
            }
            set
            {
                _AgencyDetails = value;

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
    }

    public class clsAddUpdateCleavageGradeMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsAddUpdateCleavageGradeMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion



        private clsCleavageGradeMasterVO _CleavageDetails = new clsCleavageGradeMasterVO();
        public clsCleavageGradeMasterVO CleavageDetails
        {
            get
            {
                return _CleavageDetails;
            }
            set
            {
                _CleavageDetails = value;

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
    }

    public class clsGetSurrogactAgencyMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetSurrogactAgencyMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

       

        private clsSurrogateAgencyMasterVO _AgencyDetails = new clsSurrogateAgencyMasterVO();
        public clsSurrogateAgencyMasterVO AgencyDetails
        {
            get
            {
                return _AgencyDetails;
            }
            set
            {
                _AgencyDetails = value;

            }
        }

        private List<clsSurrogateAgencyMasterVO> _AgencyDetailsList = new  List<clsSurrogateAgencyMasterVO>();
        public List<clsSurrogateAgencyMasterVO> AgencyDetailsList
        {
            get
            {
                return _AgencyDetailsList;
            }
            set
            {
                _AgencyDetailsList = value;

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
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
    }

    public class clsUpdateStatusSurrogactAgencyMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsUpdateStatusSurrogactAgencyMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion



        private clsSurrogateAgencyMasterVO _AgencyDetails = new clsSurrogateAgencyMasterVO();
        public clsSurrogateAgencyMasterVO AgencyDetails
        {
            get
            {
                return _AgencyDetails;
            }
            set
            {
                _AgencyDetails = value;

            }
        }

        private List<clsSurrogateAgencyMasterVO> _AgencyDetailsList = new List<clsSurrogateAgencyMasterVO>();
        public List<clsSurrogateAgencyMasterVO> AgencyDetailsList
        {
            get
            {
                return _AgencyDetailsList;
            }
            set
            {
                _AgencyDetailsList = value;

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
    }
  
}
