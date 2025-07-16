using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Pathology.PathologyMasters
{
    public class clsGetPathoAgencyApplicableUnitListBizActionVO : IBizActionValueObject
    {
        public long ServiceId { get; set; }

        public long ApplicableUnitID { get; set; }

        public long UnitId { get; set; }

        private List<clsServiceAgencyMasterVO> _ServiceAgencyMasterDetails;
        public List<clsServiceAgencyMasterVO> ServiceAgencyMasterDetails
        {
            get
            {
                if (_ServiceAgencyMasterDetails == null)
                    _ServiceAgencyMasterDetails = new List<clsServiceAgencyMasterVO>();

                return _ServiceAgencyMasterDetails;
            }
            set
            {
                _ServiceAgencyMasterDetails = value;
            }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.PathologyMaster.clsGetPathoAgencyApplicableUnitListBizAction";
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
