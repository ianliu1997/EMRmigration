using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddUpdateCompanyDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpdateCompanyDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsCompanyVO objItemMaster = new clsCompanyVO();
        public clsCompanyVO ItemMatserDetails
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }

      


        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
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
    public class clsCompanyVO
    {
        public long Id { get; set; }
        public long UnitId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public long CompanyTypeId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string ContactPerson { get; set; }
        public long ContactNo { get; set; }

        public Boolean Status { get; set; }
        public long? CreatedUnitID { get; set; }
        public long? UpdatedUnitID { get; set; }
        public long? AddedBy { get; set; }
        public string AddedOn { get; set; }
        public DateTime? AddedDateTime { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string AddedWindowsLoginName { get; set; }
        public string UpdateWindowsLoginName { get; set; }

        public Boolean PrimaryKeyViolationError { get; set; }
        public Boolean GeneralError { get; set; }

        private List<clsTariffDetailsVO> _TariffDetails;
        public List<clsTariffDetailsVO> TariffDetails
        {
            get
            {
                if (_TariffDetails == null)
                    _TariffDetails = new List<clsTariffDetailsVO>();

                return _TariffDetails;
            }

            set
            {

                _TariffDetails = value;

            }
        }


        public string Title { get; set; }
        public string AttachedFileName { get; set; }
        public byte[] AttachedFileContent { get; set; }

        public string TitleHeaderImage { get; set; }
        public string AttachedHeadImgFileName { get; set; }
        public byte[] AttachedHeadImgFileContent { get; set; }

        public string TitleFooterImage { get; set; }
        public string AttachedFootImgFileName { get; set; }
        public byte[] AttachedFootImgFileContent { get; set; }

        public string HeaderText { get; set; }
        public string FooterText { get; set; }


        //rohinee
        public long PatientCatagoryID { get; set; }
        public string CompanyCategory { get; set; }
//


    }
}


    
