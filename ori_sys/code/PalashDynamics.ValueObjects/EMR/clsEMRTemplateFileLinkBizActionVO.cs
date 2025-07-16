using System.Collections.Generic;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.EMR
{
    class clsEMRTemplateFileLinkBizActionVO
    {
    }
    public class clsUploadPatientImageBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsUploadPatientImageBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long ID { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public bool IsOPDIPD { get; set; }
        public string Remark { get; set; }

        private clsPatientLinkFileBizActionVO _objUploadMaster = new clsPatientLinkFileBizActionVO();
        public clsPatientLinkFileBizActionVO UploadMatserDetails
        {
            get
            {
                return _objUploadMaster;
            }
            set
            {
                _objUploadMaster = value;

            }
        }
        public byte[] OriginalImage { get; set; }
        public byte[] EditImage { get; set; }
        private int _SuccessStatus;

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
    }


    public class clsUploadPatientHystoLapBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsUploadPatientHystoLapBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public byte[] Image { get; set; }
        public long PatientID { get; set; }
        public long VisitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public bool IsOPDIPD { get; set; }
    }

    public class clsDeleteUploadPatientHystoLapBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsdeleteUploadPatientHystoLapBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public byte[] Image { get; set; }
        public long PatientID { get; set; }
        public long VisitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public bool IsOPDIPD { get; set; }
        public long ImageID { get; set; }
        public int SuccessStatus { get; set; }
    }


    public class clsGetPatientUploadedImagetHystoLapBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientUploadedImagetHystoLapBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private List<ClsImages> _Img1 = new List<ClsImages>();
        public List<ClsImages> Img1
        {
            get
            {
                return _Img1;
            }
            set
            {
                _Img1 = value;
            }
        }

        public long PatientID { get; set; }
        public long VisitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public bool IsOPDIPD { get; set; }
        public bool IsFromOtImg { get; set; }
		private string _isOPD = "Collapsed";
        public string isOPD 
        {
            get { return _isOPD; }
            set { _isOPD = value; }
        }

        private string _ISIPD = "Collapsed";
        public string ISIPD 
        {
            get { return _ISIPD; }
            set { _ISIPD = value; }
        }
        

    }

    public class ClsImages
    {
        private byte[] userImage;
        public byte[] UserImage
        {
            get { return userImage; }
            set
            {
                if (value != userImage)
                {
                    userImage = value;
                }
            }
        }

        private string _ImageName;
        public string ImageName
        {
            get { return _ImageName; }
            set
            {
                if (value != _ImageName)
                {
                    _ImageName = value;
                }
            }
        }

        private long _ImageID;
        public long ImageID
        {
            get { return _ImageID; }
            set
            {
                if (value != _ImageID)
                {
                    _ImageID = value;
                }
            }
        }


    }

    public class clsUpdateUploadPatientImageBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsUpdateUploadPatientImageBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long ID { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public string DocumentName { get; set; }
        public string Remark { get; set; }
        public string SourceURL { get; set; }

        public byte[] EditImage { get; set; }
        private int _SuccessStatus;

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
    }

    public class clsGetUploadPatientImageBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetUploadPatientImageBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long ID { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public string DocumentName { get; set; }
        public string Remark { get; set; }
        public bool IShistory { get; set; }
        public bool ISOPDIPD { get; set; }
        //public bool AllImageLoad { get; set; }

        private List<clsPatientFollowUpImageVO> Imagedetails = new List<clsPatientFollowUpImageVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientFollowUpImageVO> ImageDetails
        {
            get { return Imagedetails; }
            set { Imagedetails = value; }
        }

        public byte[] EditImage { get; set; }
        private int _SuccessStatus;

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
    }

   

}
