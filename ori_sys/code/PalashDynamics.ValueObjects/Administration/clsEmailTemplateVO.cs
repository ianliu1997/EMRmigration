using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration
{
  public class clsEmailTemplateVO:IValueObject, INotifyPropertyChanged 
    {
        private long _id;
        public long ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public bool EmailFormat { get; set; }

        public string EmailFormatDisp { get; set; }

        public long AttachmentNos { get; set; }

        public bool Status { get; set; }

        public long CreatedUnitID { get; set; }

        public long UpdatedUnitId { get; set; }

        public long AddedBy { get; set; }

        public string AddedOn { get; set; }

        public DateTime AddedDateTime { get; set; }

        public long UpdatedBy { get; set; }

        public string UpdatedOn { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public string AddedWindowsLoginName { get; set; }

        public string UpdateWindowsLoginName { get; set; }

        private string _SourceURL;
        public string SourceURL
        {
            get { return _SourceURL; }
            set
            {
                if (_SourceURL != value)
                {
                    _SourceURL = value;
                    OnPropertyChanged("SourceURL");
                }
            }
        }

        private byte[] _Report;
        public byte[] Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }

        //private List<clsEmailAttachmentVO> _AttachmentDetails;
        //public List<clsEmailAttachmentVO> AttachmentDetails
        //{
        //    get
        //    {
        //        if (_AttachmentDetails == null)
        //            _AttachmentDetails = new List<clsEmailAttachmentVO>();
        //        return _AttachmentDetails;                           
        //    }
        //    set
        //    {
        //        _AttachmentDetails = value;
        //    }
        //}

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

        #region IValueObject
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        #region INotifyPropertyChanged
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
    }

  public class clsEmailAttachmentVO :IValueObject, INotifyPropertyChanged 
  {
      private long _id;
      public long ID
      {
          get { return _id; }
          set { _id = value; }
      }

      public long TemplateId { get; set; }

      public byte [] Attachment {get; set;}

      public string AttachmentFileName { get; set; }

      public bool Status { get; set; }

      public long CreatedUnitID { get; set; }

      public long UpdatedUnitId { get; set; }

      public long AddedBy { get; set; }

      public string AddedOn { get; set; }

      public DateTime AddedDateTime { get; set; }

      public long UpdatedBy { get; set; }

      public string UpdatedOn { get; set; }

      public DateTime UpdatedDateTime { get; set; }

      public string AddedWindowsLoginName { get; set; }

      public string UpdateWindowsLoginName { get; set; }

      public string ToXml()
      {
          return this.ToString();
      }

      #region INotifyPropertyChanged
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
  }
}
