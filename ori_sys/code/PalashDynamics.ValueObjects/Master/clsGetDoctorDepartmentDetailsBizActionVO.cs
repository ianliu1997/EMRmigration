using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Radiology;


namespace PalashDynamics.ValueObjects.Master
{
    public class clsGetDoctorDepartmentDetailsBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public long UnitId { get; set; }
        public bool IsNonReferralDoctor { get; set; }
        public long ReferralID { get; set; }
        public long DepartmentId { get; set; }
        public long SpecializationID { get; set; }
        public long DoctorId { get; set; }
        public long DoctorTypeID { get; set; }
        public string StrClinicID { get; set; }
        public bool IsSpecializtion { get; set; }

        #region For Radiology Additions

        public bool AllRecord { get; set; }
        public long ServiceId { get; set; }
        public bool IsServiceWiseDoctorList { get; set; }

        #endregion

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorDepartmentDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    //By Rohinee On 28/12/2015 for all doctors 
    public class clsGetDoctorListForComboBizActionVO : IBizActionValueObject
    {     
        public long ID { get; set; }
        public long Description { get; set; }
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorListForComboBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
                
    public class clsGetRadiologistBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }


        public long UnitId { get; set; }

        public long ID { get; set; }
        public long DepartmentId { get; set; }
        
        public clsRadiologyVO ItemSupplier { get; set; }
        public List<clsRadiologyVO> ItemSupplierList { get; set; }
        public List<clsRadiologyVO> ItemList { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetRadiologistBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetPathologistBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }




        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetPathologistBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetAnesthetistBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }


        public long UnitId { get; set; }

        public long ID { get; set; }
        public long DepartmentId { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetAnesthetistBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetEmbryologistBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }


        public long UnitId { get; set; }

        public long ID { get; set; }
        public long DepartmentId { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetEmbryologistBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetAnesthesiaVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }


        public long UnitId { get; set; }

        public long ID { get; set; }
        public long DepartmentId { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetEmbryologistBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
    
    //BY ROHINI FOR GET USER HAVING RIGHTS OF PATHOLOGY FORMS
    public class clsGetPathoUsersBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }
        public long MenuID { get; set; }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetPathoUsersBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    // BY BHUSHAN . . . . . . .
    public class clsGetPathoParameterUnitBizActionVO : IBizActionValueObject
    {

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public long ParamID { get; set; }
        //   public long DepartmentId { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetPathoParameterUnitBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    

    #region For IPD Module

    public class clsFillDepartmentBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }


        public long UnitId { get; set; }
        public long DepartmentId { get; set; }
        public long SubSpecilizationID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsFillDepartmentBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    #endregion

}
