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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.Administration
{
    public partial class AssignTariffPopUp : ChildWindow
    {
        clsServiceMasterVO objMasterVO = null;

        public long ServiceID { get; set; }
        public AssignTariffPopUp()
        {
            InitializeComponent();
            rdbNew.IsChecked = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            frmAssignServiceTariff Win = new frmAssignServiceTariff();
            Win.ServiceID = ServiceID;
            if (rdbNew.IsChecked == true)
            {
                Win.OperationType = Convert.ToInt32(TariffOperationType.New);
                   
            }
            else if(rdbModify.IsChecked==true)
            {
                Win.OperationType = Convert.ToInt32(TariffOperationType.Modify);

            }
            else if (rdbRemove.IsChecked==true)
            {
                Win.OperationType = Convert.ToInt32(TariffOperationType.Remove);

            }
            Win.GetSelectedServiceDetails(objMasterVO);
            Win.Show();
        }

        public void GetSelectedServiceDetails(clsServiceMasterVO objServiceMasterVO)
        {
            objMasterVO = objServiceMasterVO;
            string Title = "Service Name : ";
            this.Title = Title + objMasterVO.ServiceName;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

