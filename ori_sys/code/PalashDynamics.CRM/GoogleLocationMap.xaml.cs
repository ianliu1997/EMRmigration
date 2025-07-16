using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.Service.UserInforServiceRef;

namespace PalashDynamics.CRM
{
    public partial class GoogleLocationMap : UserControl
    {
        public GoogleLocationMap()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            customHost.Width = 600;
            customHost.Height = 500;
            customHost.HostVisible = true;
        }

        private void SetPosition_Click(object sender, RoutedEventArgs e)
        {
            try
            {



                //Uri address1 = new Uri(Application.Current.Host.Source, "../GetLocationCoordinates.asmx"); // this url will work both in dev and after deploy

               

                //GetLocationCoordinatesSoapClient client1 = new LocationCoordinates.GetLocationCoordinatesSoapClient("UserInformationSoap", address1.AbsoluteUri);


                Uri address1 = new Uri(Application.Current.Host.Source, "../UserInformation.asmx"); // this url will work both in dev and after deploy
                UserInformationSoapClient client1 = new UserInformationSoapClient("UserInformationSoap", address1.AbsoluteUri);
                             

                client1.GetCoordinatesCompleted += (s1, e1) =>
                {
                    string winname = (string)e1.Result;

                    if (!string.IsNullOrEmpty(winname))
                    {
                        string[] Coordinates = winname.Split(',');

                        if (Coordinates.Length == 2)
                        {
                            string strLatitude;
                            string strLongitude;

                            strLatitude = Coordinates[0];
                            strLongitude = Coordinates[1];
                        }
                      

                    }

                };
                client1.GetCoordinatesAsync(txtAddress.Text.Trim());
                client1.CloseAsync();
            }
            catch (Exception)
            {

                //  throw;
            }
        }
    }
}
