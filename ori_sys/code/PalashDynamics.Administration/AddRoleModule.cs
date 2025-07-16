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
using CIMS.Forms;

namespace PalashDynamics.Administration
{
    public class AddRoleModule: IModule
    {
        private readonly IRegionManager regionManager;

        public AddRoleModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.regionManager.Regions["MainRegion"].Add(new frmAddRole(), "PalashDynamics.Administration.frmAddRole");
        }

    }

}
