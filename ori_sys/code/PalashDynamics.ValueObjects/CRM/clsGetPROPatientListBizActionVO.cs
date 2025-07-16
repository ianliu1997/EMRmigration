using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.CRM
{
    public class clsGetPROPatientListBizActionVO:IBizActionValueObject
    {
        public DateTime? VisitFromDate { get; set; }
        public DateTime? VisitToDate { get; set; }     
        public long? ReferredDoctorId { get; set; }

        private List<clsPatientGeneralVO> myVar = new List<clsPatientGeneralVO>();

        public List<clsPatientGeneralVO> PatientList
        {
            get { return myVar; }
            set { myVar = value; }
        }
     
        private bool _PagingEnabled;
        public bool InputPagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }

        private int _StartRowIndex = 0;
        public int InputStartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }

        private int _TotalRows = 0;
        public int OutputTotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }

        private int _MaximumRows;
        public int InputMaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }


        public string SortExpression { get; set; }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CRM.clsGetPROPatientListBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }
}
