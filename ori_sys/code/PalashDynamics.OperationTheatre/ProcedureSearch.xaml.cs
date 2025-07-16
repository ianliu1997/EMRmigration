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
using System.Windows.Browser;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;

using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using CIMS;

namespace PalashDynamics.OperationTheatre
{
    public partial class ProcedureSearch : ChildWindow
    {
        public event RoutedEventHandler OnAddButton_Click;

        public List<clsProcedureMasterVO> procedureList = new List<clsProcedureMasterVO>();


        WaitIndicator Indicatior = null;

        public ProcedureSearch()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }




        /// <summary>
        /// fills Service combo box
        /// </summary>
        private void FetchProcedureType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_ProcedureTypeMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbProcedureType.ItemsSource = null;
                        cmbProcedureType.ItemsSource = objList;
                        cmbProcedureType.SelectedItem = objList[0];
                      


                        //if (this.DataContext != null)
                        //{
                        //    CmbProcedureType.SelectedValue = ((clsProcedureMasterVO)this.DataContext).ProcedureTypeID;


                        //}
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }



        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            FetchData();

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FetchProcedureType();
           // FetchData();
        }
        private void FetchData()
        {
            try
            {
                clsGetProcedureMasterBizActionVO BizAction = new clsGetProcedureMasterBizActionVO();
                BizAction.ProcDetails = new List<clsProcedureMasterVO>();

                if (txtProcedureName.Text != "")
                {
                    BizAction.Description = txtProcedureName.Text;
                }

                if (cmbProcedureType.SelectedItem != null)
                {
                    BizAction.ProcedureTypeID = ((MasterListItem)cmbProcedureType.SelectedItem).ID;
                }






                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {

                        if (((clsGetProcedureMasterBizActionVO)arg.Result).ProcDetails != null)
                        {
                            clsGetProcedureMasterBizActionVO result = arg.Result as clsGetProcedureMasterBizActionVO;


                            if (result.ProcDetails != null)
                            {


                                foreach (var item in result.ProcDetails)
                                {
                                    item.Status = false;

                                    var obj = procedureList.FirstOrDefault(q => q.ID == ((clsProcedureMasterVO)item).ID);
                                   if (obj != null)
                                   continue;
                                   else

                                    procedureList.Add(item);
                                }

                                dgProcedureList.ItemsSource = null;
                                dgProcedureList.ItemsSource = procedureList;



                            }

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// add buttton click
        /// </summary>
      
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OnAddButton_Click != null)
                {
                    this.DialogResult = true;
                    OnAddButton_Click(this, new RoutedEventArgs());

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

      

        private void chkProcedure_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ((clsProcedureMasterVO)dgProcedureList.SelectedItem).Status = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkProcedure_Unchecked(object sender, RoutedEventArgs e)
        {
            ((clsProcedureMasterVO)dgProcedureList.SelectedItem).Status = false;
        }









    }



}

