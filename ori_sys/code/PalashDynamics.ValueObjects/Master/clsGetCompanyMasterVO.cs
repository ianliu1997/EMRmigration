using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsGetCompanyMasterVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetCompanyMaster";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public long ID { get; set; }
        public long PatientCategoryID { get; set; }
        //added by rohini
        public bool IsForPathology { get; set; }
        public long PathologyCompanyType { get; set; }
        //
        public long CompanyID { get; set; }
        public short PatientSourceType { get; set; }
        public long ParentPatientID { get; set; }
        public bool Status { get; set; }

        public long UnitID { get; set; }

        List<MasterListItem> _List = new List<MasterListItem>();

        /// <summary>
        /// Output Property.
        /// Get Property To Access And Modify RoleList
        /// </summary>
        public List<MasterListItem> List
        {
            get { return _List; }
            set { _List = value; }

        }
    }
}
