using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

//Added By Kiran Add update and Get List BizActionVO for BedUnderMaintenanceDetails  .
//Date:24/08/2012.
namespace PalashDynamics.ValueObjects.IPD
{
    public class clsAddBedUnderMaintenanceBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsAddBedUnderMaintenanceBizAction";
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

        private clsIPDBedUnderMaintenanceVO _BedUnderMDetails;
        public clsIPDBedUnderMaintenanceVO BedUnderMDetails
        {
            get { return _BedUnderMDetails; }
            set { _BedUnderMDetails = value; }

        }
    }

    public class clsGetReleaseBedUnderMaintenanceListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetReleaseBedUnderMaintenanceListBizAction";
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

        private clsIPDBedUnderMaintenanceVO _BedUnderMDetails;
        public clsIPDBedUnderMaintenanceVO BedUnderMDetails
        {
            get { return _BedUnderMDetails; }
            set { _BedUnderMDetails = value; }

        }


        private List<clsIPDBedUnderMaintenanceVO> _BedUnderMList;
        public List<clsIPDBedUnderMaintenanceVO> BedUnderMList
        {
            get
            {
                return _BedUnderMList;
            }
            set
            {
                if (value != null)
                {
                    _BedUnderMList = value;
                }
            }
        }
    }

    public class clsAddUpdateReleaseBedUnderMaintenanceBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsAddUpdateReleaseBedUnderMaintenanceBizAction";
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

        private clsIPDBedUnderMaintenanceVO _BedUnderMDetails;
        public clsIPDBedUnderMaintenanceVO BedUnderMDetails
        {
            get { return _BedUnderMDetails; }
            set { _BedUnderMDetails = value; }

        }
    }
}
