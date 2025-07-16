using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using GeoCoding;
using GeoCoding.Google;


namespace PalashDynamics.Web
{
    /// <summary>
    /// Summary description for UserInformation
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UserInformation : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetClientUserName()
        {
            // Add your operation implementation here
            //return "nileshi";
            return System.Web.HttpContext.Current.User.Identity.Name.ToString();

        }
        [WebMethod]
        public string GetClientMachineName()
        {
            // Add your operation implementation here
            //return "nileshi";
            //return "Machine Name : " + System.Web.HttpContext.Current.Server.MachineName.ToString(); 
            string hn = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
            return System.Web.HttpContext.Current.Request.UserHostName.ToString() + " : " + System.Net.Dns.GetHostEntry(hn).HostName.ToString();
            //return System.Net.Dns.GetHostEntry(hn).HostName.ToString();
        }


        [WebMethod]
        public string GetCoordinates(string strAddress)
        {
            //return "Hello World";
            IGeoCoder geoCoder = new GoogleGeoCoder("my-google-api-key");
            GeoCoding.Address[] addresses = geoCoder.GeoCode(strAddress.Trim());

            string strCoordinates = null;

            if (addresses.Length > 0)
            {
                strCoordinates = addresses[0].Coordinates.Latitude.ToString();
                strCoordinates += "," + addresses[0].Coordinates.Longitude.ToString();
            }

            return strCoordinates;
        }
        
    }
}
