using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.NursingStation.EMR;
using System.Collections.ObjectModel;

namespace PalashDynamics.ValueObjects.NursingStation
{
    public class clsDrugAdministrationChartBizActionVO : IBizActionValueObject
    {

        private clsDrugAdministrationChartVO _DrugAdministrationChart = null;
        public clsDrugAdministrationChartVO DrugAdministrationChart
        {
            get { return _DrugAdministrationChart; }
            set { _DrugAdministrationChart = value; }
        }

        private List<clsPrescriptionMasterVO> _PrescriptionMasterList = null;
        public List<clsPrescriptionMasterVO> PrescriptionMasterList
        {
            get { return _PrescriptionMasterList; }
            set { _PrescriptionMasterList = value; }
        }

        

        private List<clsFeedingDetailsVO> _FeedingDetails = null;
        public List<clsFeedingDetailsVO> FeedingDetailsList
        {
            get { return _FeedingDetails; }
            set { _FeedingDetails = value; }
        }


        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.NursingStation.clsDrugAdministrationChartBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }
    public class clsGetDrugListForDrugChartBizActionVO : IBizActionValueObject
    {
        private List<clsPrescriptionDetailsVO> _DrugList = null;
        public List<clsPrescriptionDetailsVO> DrugList
        {
            get { return _DrugList; }
            set { _DrugList = value; }
        }

        private long _PrescriptionID;
        public long PrescriptionID
        {
            get { return _PrescriptionID; }
            set { _PrescriptionID = value; }
        }

        private long _PrescriptionUnitID;
        public long PrescriptionUnitID
        {
            get { return _PrescriptionUnitID; }
            set { _PrescriptionUnitID = value; }
        }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.NursingStation.clsGetDrugListForDrugChartBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsSaveDrugFeedingDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsPrescriptionDetailsVO> _DrugFeedingList = null;
        public List<clsPrescriptionDetailsVO> DrugFeedingList
        {
            get { return _DrugFeedingList; }
            set { _DrugFeedingList = value; }
        }

        private ObservableCollection<clsPrescriptionDetailsVO> _DrugFeedingListObserv = null;
        public ObservableCollection<clsPrescriptionDetailsVO> DrugFeedingListObserv
        {
            get { return _DrugFeedingListObserv; }
            set { _DrugFeedingListObserv = value; }
        }

        private long _PrescriptionID;
        public long PrescriptionID
        {
            get { return _PrescriptionID; }
            set { _PrescriptionID = value; }
        }

        private long _PrescriptionUnitID;
        public long PrescriptionUnitID
        {
            get { return _PrescriptionUnitID; }
            set { _PrescriptionUnitID = value; }
        }

        private long _Opd_Ipd_Id;
        public long Opd_Ipd_Id
        {
            get { return _Opd_Ipd_Id; }
            set
            {
                if (value != _Opd_Ipd_Id)
                {
                    _Opd_Ipd_Id = value;
                }
            }
        }

        private long _Opd_Ipd_UnitID;
        public long Opd_Ipd_UnitID
        {
            get { return _Opd_Ipd_UnitID; }
            set
            {
                if (value != _Opd_Ipd_UnitID)
                {
                    _Opd_Ipd_UnitID = value;
                }
            }
        }
        
        private Int16 _OPD_IPD;
        public Int16 OPD_IPD
        {
            get { return _OPD_IPD; }
            set
            {
                if (value != _OPD_IPD)
                {
                    _OPD_IPD = value;
                }
            }
        }


        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.NursingStation.clsSaveDrugFeedingDetailsBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsGetFeedingDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsPrescriptionDetailsVO> _DrugFeedingList = null;
        public List<clsPrescriptionDetailsVO> DrugFeedingList
        {
            get { return _DrugFeedingList; }
            set { _DrugFeedingList = value; }
        }

        private long _PrescriptionID;
        public long PrescriptionID
        {
            get { return _PrescriptionID; }
            set { _PrescriptionID = value; }
        }

        private long _PrescriptionUnitID;
        public long PrescriptionUnitID
        {
            get { return _PrescriptionUnitID; }
            set { _PrescriptionUnitID = value; }
        }

        private long _Opd_Ipd_Id;
        public long Opd_Ipd_Id
        {
            get { return _Opd_Ipd_Id; }
            set
            {
                if (value != _Opd_Ipd_Id)
                {
                    _Opd_Ipd_Id = value;
                }
            }
        }

        private long _Opd_Ipd_UnitID;
        public long Opd_Ipd_UnitID
        {
            get { return _Opd_Ipd_UnitID; }
            set
            {
                if (value != _Opd_Ipd_UnitID)
                {
                    _Opd_Ipd_UnitID = value;
                }
            }
        }

        private Int16 _OPD_IPD;
        public Int16 OPD_IPD
        {
            get { return _OPD_IPD; }
            set
            {
                if (value != _OPD_IPD)
                {
                    _OPD_IPD = value;
                }
            }
        }


        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.NursingStation.clsGetFeedingDetailsBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsUpdateFeedingDetailsIsFreezeBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.NursingStation.clsUpdateFeedingDetailsIsFreezeBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
        private List<clsPrescriptionDetailsVO> _DrugFeedingList = null;
        public List<clsPrescriptionDetailsVO> DrugFeedingList
        {
            get { return _DrugFeedingList; }
            set { _DrugFeedingList = value; }
        }
        private clsPrescriptionDetailsVO myVar = new clsPrescriptionDetailsVO();
        public clsPrescriptionDetailsVO PrescriptionDetails
        {
            get { return myVar; }
            set { myVar = value; }
        }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }


      
    }
}
