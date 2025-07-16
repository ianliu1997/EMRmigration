using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.CRM
{
    public class clsEmailSMSSentListVO : INotifyPropertyChanged, IValueObject
    {
        private long _Id;
        public long Id
        {
            get { return _Id; }
            set
            {
                if (value != _Id)
                {
                    _Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public long PatientId { get; set; }
        public long PatientUnitID { get; set; } //***//
        public bool Email_SMS { get; set; }

        public long TemplateID { get; set; }

        public string PatientEmailId { get; set; }

        public string EmailSubject { get; set; }

        public string EmailText { get; set; }

        public string EmailAttachment { get; set; }

        public string PatientMobileNo { get; set; }

        public string EnglishText { get; set; }

        public string LocalText { get; set; }

        public bool SuccessStatus { get; set; }

        public string FailureReason { get; set; }

        public long AttachmentNos { get; set; }

        private List<clsEmailAttachmentVO> _AttachmentDetails;
        public List<clsEmailAttachmentVO> AttachmentDetails
        {
            get
            {
                if (_AttachmentDetails == null)
                    _AttachmentDetails = new List<clsEmailAttachmentVO>();

                return _AttachmentDetails;
            }
            set
            {
                _AttachmentDetails = value;
            }
        }


        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }

            }
        }
        
        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }

            }

        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }

        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public string ToXml()
        {
            return this.ToString();
        }
    }
}
