using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects;
using System.Data;

namespace PalashDynamics.Web
{
    /// <summary>
    /// Summary description for PatientRegistration
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PatientRegistration : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        [WebMethod]
        public System.Data.DataSet GiveMeADataSet()
        {
            string strConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
           
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(strConnection);
            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
            adapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select ID,Description From M_BankMaster where status =1", conn);
            adapter.Fill(ds, "BankMaster");
            return ds;
        }



        
        [WebMethod]
        public string SaveRegistration
           (
               long PatientCategoryID, string FirstName, string MiddleName, string LastName, string FamilyName, long GenderID,
               long BloodGroupID, long MaritalStatusID, long ReligionID, long OccupationId, string CompanyName,
               long ResiNoCountryCode, long ResiSTDCode, string ResidenceNo, string MobileNo, string MobileCountryCode, string CivilID,
               string FaxNo, string Email, string AddressLine1, string AddressLine2, string AddressLine3, string Area, string City,
               string District, string State, string Country, string Pincode
           )
        {
            //throw new NotImplementedException();
            string lMRNO = "";
            try
            {
                clsPatientVO patientVO = new clsPatientVO();

                patientVO.GeneralDetails.PatientTypeID = PatientCategoryID;
                //patientVO.GeneralDetails.MRNo = MRNo;
                //patientVO.GeneralDetails.RegistrationDate = RegistrationDate;
                patientVO.GeneralDetails.RegistrationDate = (DateTime.Now);
                patientVO.GeneralDetails.FirstName = FirstName;
                patientVO.GeneralDetails.MiddleName = MiddleName;
                patientVO.GeneralDetails.LastName = LastName;
                patientVO.FamilyName = FamilyName;
                patientVO.GenderID = GenderID;
                patientVO.BloodGroupID = BloodGroupID;
                DateTime date1 = new DateTime(1990, 2, 1);
                patientVO.GeneralDetails.DateOfBirth = date1;
                patientVO.MaritalStatusID = MaritalStatusID;
                patientVO.ReligionID = ReligionID;
                patientVO.OccupationId = OccupationId;
                patientVO.CompanyName = CompanyName;
                patientVO.ResiNoCountryCode = ResiNoCountryCode;
                patientVO.ResiSTDCode = ResiSTDCode;
                patientVO.ContactNo1 = ResidenceNo;
                patientVO.ContactNo2 = MobileNo;
                patientVO.MobileCountryCode = MobileCountryCode;
                patientVO.CivilID = CivilID;
                patientVO.FaxNo = FaxNo;
                patientVO.Email = Email;
                patientVO.AddressLine1 = AddressLine1;
                patientVO.AddressLine2 = AddressLine2;
                patientVO.AddressLine3 = AddressLine3;
                patientVO.Area = Area;
                patientVO.City = City;
                patientVO.District = District;
                patientVO.State = State;
                patientVO.Country = Country;
                patientVO.Pincode = Pincode;
                //patientVO.Photo = pPatient.Photo;

                //Common Properties
                patientVO.Status = true;
                patientVO.AddedDateTime = DateTime.Now;


                clsUserVO userVO = new clsUserVO();
                userVO.UserLoginInfo.UnitId = 1;
                userVO.ID = 1;
                userVO.UserLoginInfo.MachineName = "machine1";
                userVO.UserLoginInfo.WindowsUserName = "user1";


                clsAddPatientBizActionVO BizAction = new clsAddPatientBizActionVO();
                BizAction.PatientDetails = patientVO;



                PalashDynamicsWeb service = new PalashDynamicsWeb();
                BizAction = (clsAddPatientBizActionVO)service.Process(BizAction, userVO);


                lMRNO = BizAction.PatientDetails.GeneralDetails.MRNo;

            }
            catch (Exception)
            {
                lMRNO = "";
                throw;
            }

            return lMRNO;
        }

    }
}
