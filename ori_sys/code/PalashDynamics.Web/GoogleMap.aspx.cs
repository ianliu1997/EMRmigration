using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeoCoding;
using GeoCoding.Google;

namespace GoogleMapHoster.Web
{
    public partial class GoogleMap : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //foreach (Control c in Page.Controls)
                //    c.Visible = false;

                string Address = Request.QueryString["Address"];

                if (!string.IsNullOrEmpty(Address))
                {
                    IGeoCoder geoCoder = new GoogleGeoCoder("my-google-api-key");
                    GeoCoding.Address[] addresses = geoCoder.GeoCode(Address);

                    if (addresses.Length > 0)
                    {
                        lblAddress.Text = "Address :- " + Address;
                        Latitude.Value = addresses[0].Coordinates.Latitude.ToString();
                        Langitude.Value = addresses[0].Coordinates.Longitude.ToString();
                        // showVehicle("showVehicle", new object[] { txtLat.Text, txtLon.Text });

                    }

                }

            }
        }

    }
}
