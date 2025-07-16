using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Modularity;

namespace CIMS.Forms
{
    public class AddUserModule: IModule
    {
        private readonly IRegionManager regionManager;

        public AddUserModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.regionManager.Regions["MainRegion"].Add(new frmAddUser(), "PalashDynamics.Administration.frmAddUser");
            //this.regionManager.RegisterViewWithRegion("MainRegion", typeof(frmReservation));

        }
    }

   
}
