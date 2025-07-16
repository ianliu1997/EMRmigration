using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class cls_IVFDashboard_GetVitrificationDetailsForCryoBank : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboard_GetVitrificationDetailsForCryoBankBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        //public long CoupleID { get; set; }
        public long CoupleUintID { get; set; }
        public bool IsEdit { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public int FromID { get; set; }
        public long PatientID { get; set; }
        public long Cane { get; set; }

        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }

        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string FamilyName { get; set; }
        public string MRNo { get; set; }
        public string CtcNo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        private clsIVFDashboard_GetVitrificationBizActionVO _Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
        public clsIVFDashboard_GetVitrificationBizActionVO Vitrification
        {
            get
            {
                return _Vitrification;
            }
            set
            {
                _Vitrification = value;
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

        private bool _IsFreezeOocytes;      //Flag to get Freeze Oocytes under Freeze All Oocytes Cycle  // For IVF ADM Changes
        public bool IsFreezeOocytes
        {
            get { return _IsFreezeOocytes; }
            set
            {
                _IsFreezeOocytes = value;
            }
        }
    }

    //Added By CDS On 13/07/2016 For Oocyte Cryo Bank 
    public class cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBankBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        //public long CoupleID { get; set; }
        public long CoupleUintID { get; set; }
        public bool IsEdit { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public int FromID { get; set; }
        public long PatientID { get; set; }


        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }

        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string FamilyName { get; set; }
        public string MRNo { get; set; }
        public string CtcNo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        private clsIVFDashboard_GetVitrificationBizActionVO _Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
        public clsIVFDashboard_GetVitrificationBizActionVO Vitrification
        {
            get
            {
                return _Vitrification;
            }
            set
            {
                _Vitrification = value;
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

    //Added by neena
    public class cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBankBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        //public long CoupleID { get; set; }
        public long CoupleUintID { get; set; }
        public bool IsEdit { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public int FromID { get; set; }
        public long PatientID { get; set; }
        public long Cane { get; set; }

        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }

        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string FamilyName { get; set; }
        public string MRNo { get; set; }
        public string CtcNo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public bool IsOocyte { get; set; }
        public bool IsEmb { get; set; }

        private clsIVFDashboard_GetVitrificationBizActionVO _Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
        public clsIVFDashboard_GetVitrificationBizActionVO Vitrification
        {
            get
            {
                return _Vitrification;
            }
            set
            {
                _Vitrification = value;
            }
        }


        private clsIVFDashBoard_VitrificationDetailsVO _VitrificationDetails = new clsIVFDashBoard_VitrificationDetailsVO();
        public clsIVFDashBoard_VitrificationDetailsVO VitrificationDetails
        {
            get
            {
                return _VitrificationDetails;
            }
            set
            {
                _VitrificationDetails = value;
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

        private bool _IsFreezeOocytes;      //Flag to get Freeze Oocytes under Freeze All Oocytes Cycle  // For IVF ADM Changes
        public bool IsFreezeOocytes
        {
            get { return _IsFreezeOocytes; }
            set
            {
                _IsFreezeOocytes = value;
            }
        }
    }
    //
}

