using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsAddUpdateTESEBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Patient.clsAddUpdateTESEBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long FemalePatientID { get; set; }
        public long MalePatientID { get; set; }
        public long PatientUnitID { get; set; }


        private clsTESEVO _TESE = new clsTESEVO();
        public clsTESEVO TESE
        {
            get
            {
                return _TESE;
            }
            set
            {
                _TESE = value;
            }
        }

        private List<clsTESEDetailsVO> _TESEDetailsList = new List<clsTESEDetailsVO>();
        public List<clsTESEDetailsVO> TESEDetailsList
        {
            get
            {
                return _TESEDetailsList;
            }
            set
            {
                _TESEDetailsList = value;
            }
        }



        private List<clsTESEVO> _TESEList = new List<clsTESEVO>();
        public List<clsTESEVO> TESEList
        {
            get
            {
                return _TESEList;
            }
            set
            {
                _TESEList = value;
            }
        }

        private clsTESEDetailsVO _TESEDetails = new clsTESEDetailsVO();
        public clsTESEDetailsVO TESEDetails
        {
            get
            {
                return _TESEDetails;
            }
            set
            {
                _TESEDetails = value;
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

    public class clsGetTESEBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Patient.clsGetTESEBizAction";
        }

        #endregion
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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

        private clsTESEVO objTESE = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsTESEVO TESE
        {
            get { return objTESE; }
            set { objTESE = value; }
        }

        private clsTESEDetailsVO objTESEDetails = null;
        public clsTESEDetailsVO TESEDetails
        {
            get { return objTESEDetails; }
            set { objTESEDetails = value; }
        }

        private List<clsTESEDetailsVO> objTESEDetailsList = null;
        public List<clsTESEDetailsVO> TESEDeatailsList
        {
            get { return objTESEDetailsList; }
            set { objTESEDetailsList = value; }
        }

        private List<clsTESEVO> objTESEList = null;
        public List<clsTESEVO> TESEList
        {
            get { return objTESEList; }
            set { objTESEList = value; }
        }

    }


}