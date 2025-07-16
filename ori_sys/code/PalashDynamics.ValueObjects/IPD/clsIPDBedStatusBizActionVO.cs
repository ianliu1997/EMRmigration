/* Added  By SUDHIR PATIL on 03/March/2014 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsIPDBedStatusBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsIPDBedStatusBizAction";
        }
        public string ToXml()
        {
            return this.ToString();
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsIPDBedStatusVO _BedStatus;
        public clsIPDBedStatusVO BedStatus
        {
            get { return _BedStatus; }
            set { _BedStatus = value; }
        }

        private List<clsIPDBedStatusVO> _BedStatusList;
        public List<clsIPDBedStatusVO> BedStatusList
        {
            get { return _BedStatusList; }
            set { _BedStatusList = value; }
        }

    }

    public class clsGetWardByFloorBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetWardByFloorBizAction";
        }
        public string ToXml()
        {
            return this.ToString();
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsIPDBedStatusVO> _WardList;
        public List<clsIPDBedStatusVO> WardList
        {
            get { return _WardList; }
            set { _WardList = value; }
        }
        private clsIPDBedStatusVO _Floor;
        public clsIPDBedStatusVO Floor
        {
            get { return _Floor; }
            set { _Floor = value; }
        }

    }
}
