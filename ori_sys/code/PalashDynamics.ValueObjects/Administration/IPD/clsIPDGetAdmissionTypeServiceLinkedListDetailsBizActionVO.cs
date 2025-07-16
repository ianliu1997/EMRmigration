using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsIPDGetAdmissionTypeServiceLinkedListDetailsBizActionVO : IBizActionValueObject
    {
        public bool IsPagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public string SearchExpression { get; set; }
        public int TotalRows { get; set; }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    //  OnPropertyChanged("ID");
                }
            }
        }
        private long _AdmissionTypeID;
        public long AdmissionTypeID
        {
            get { return _AdmissionTypeID; }
            set
            {
                if (value != _AdmissionTypeID)
                {
                    _AdmissionTypeID = value;
                    //  OnPropertyChanged("ID");
                }
            }
        }

        public List<clsServiceMasterVO> ServiceList { get; set; }





        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDGetAdmissionTypeServiceLinkedListDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
