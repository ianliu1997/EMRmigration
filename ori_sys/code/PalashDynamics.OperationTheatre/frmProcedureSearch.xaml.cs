using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using System.Windows.Input;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmProcedureSearch : ChildWindow
    {
        #region Variable Declaration

        public event RoutedEventHandler OnAddButton_Click;
        string msgText;
        public List<clsProcedureMasterVO> procedureList = new List<clsProcedureMasterVO>();
        public List<clsProcedureMasterVO> AddedprocedureList = new List<clsProcedureMasterVO>();
        WaitIndicator Indicatior = null;

        #endregion

        #region Constructor
        public frmProcedureSearch()
        {
            InitializeComponent();
        }
        #endregion

        #region Overriden Method
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        #endregion

        /// <summary>
        /// fills Service combo box
        /// </summary>
        private void FillProcedureType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureTypeMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbProcedureType.ItemsSource = null;
                        cmbProcedureType.ItemsSource = objList;
                        cmbProcedureType.SelectedItem = objList[0];
                        FetchData();
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
            FillProcedureType();

        }

        private void FetchData()
        {
            try
            {
                clsGetProcedureMasterBizActionVO BizAction = new clsGetProcedureMasterBizActionVO();
                BizAction.ProcDetails = new List<clsProcedureMasterVO>();
                procedureList = new List<clsProcedureMasterVO>();

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
                    if (arg.Error == null && arg.Result != null && ((clsGetProcedureMasterBizActionVO)arg.Result).ProcDetails != null)
                    {
                        clsGetProcedureMasterBizActionVO result = arg.Result as clsGetProcedureMasterBizActionVO;
                        if (result.ProcDetails != null)
                        {
                            foreach (clsProcedureMasterVO item in result.ProcDetails)
                            {
                               // item.Status = false;
                                var obj = procedureList.FirstOrDefault(q => q.ID == item.ID);
                                if (obj != null)
                                    continue;
                                else
                                  //  procedureList.Add(item);
                                    if (item.Status == true) //add  by devidas
                                    {
                                        item.Status = false;
                                        procedureList.Add(item);
                                    }
                            }

                            if (dgProcedureList.ItemsSource != null)
                            {
                                foreach (var item in dgProcedureList.ItemsSource)
                                {
                                    if (((clsProcedureMasterVO)item).Status == true)
                                        procedureList.Add(((clsProcedureMasterVO)item));
                                }
                            }
                            dgProcedureList.ItemsSource = null;
                            dgProcedureList.ItemsSource = procedureList;
                        }
                    }
                    else
                    {
                        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        //{
                        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                            msgText = "Error occured while processing.";
                        //}
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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
            //try
            //{
            //    if (dgProcedureList.SelectedItem != null)
            //        ((clsProcedureMasterVO)dgProcedureList.SelectedItem).Status = true;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        private void chkProcedure_Unchecked(object sender, RoutedEventArgs e)
        {
            //if (dgProcedureList.SelectedItem != null)
            //    ((clsProcedureMasterVO)dgProcedureList.SelectedItem).Status = false;
        }

        private void txtProcedureName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }

        private void chkProcedure_Click(object sender, RoutedEventArgs e)
        {
            //bool IsValid = true;
            if (dgProcedureList.SelectedItem != null)
            {
                try
                {
                    if (AddedprocedureList == null)
                        AddedprocedureList = new List<clsProcedureMasterVO>();

                    CheckBox chk = (CheckBox)sender;
                    //StringBuilder strError = new StringBuilder();
                    if (chk.IsChecked == true)
                    {
                        if (AddedprocedureList.Count > 0)
                        {
                            var item = from r in AddedprocedureList
                                       where r.ID == ((clsProcedureMasterVO)dgProcedureList.SelectedItem).ID
                                       select new clsProcedureMasterVO
                                       {
                                           Status = r.Status,
                                           ID=r.ID
                                       };
                            if (item.ToList().Count > 0)
                            {
                                //if (strError.ToString().Length > 0)
                                //    strError.Append(",");
                                //strError.Append(((MasterListItem)dgStaffList.SelectedItem).Code);
                                //if (!string.IsNullOrEmpty(strError.ToString()))
                                //{
                                //    string strMsg = "Staff already Selected : " + strError.ToString();

                                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //    msgW1.Show();
                                //    ((clsOTDetailsStaffDetailsVO)dgStaffList.SelectedItem).Status = false;

                                //    IsValid = false;
                                //}
                            }
                            else
                            {
                                AddedprocedureList.Add((clsProcedureMasterVO)dgProcedureList.SelectedItem);
                            }
                        }
                        else
                        {
                            AddedprocedureList.Add((clsProcedureMasterVO)dgProcedureList.SelectedItem);
                        }
                    }
                    else
                        AddedprocedureList.Remove((clsProcedureMasterVO)dgProcedureList.SelectedItem);

                }
                catch (Exception) { }
            }
        }
    }
}

