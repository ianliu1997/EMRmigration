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
using PalashDynamics.ValueObjects.Pathology;
using CIMS;
using PalashDynamics.ValueObjects;
using MessageBoxControl;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class ChangeTestAgency : ChildWindow
    {
        public clsPathoTestOutSourceDetailsVO TestOutSourceDetails;
        public long AssignedAgencyID;
        public bool IsAssignAgency;
        public long ServiceId;
        public event RoutedEventHandler OnSaveButtonClick;
        public event RoutedEventHandler OnCancelButtonClick;
        public List<clsPathoTestOutSourceDetailsVO> AssignAgencyToTestList { get; set; }

        public ChangeTestAgency()
        {
            InitializeComponent();
            FillAgency();
        }

        private void ChangeTestAgency_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in AssignAgencyToTestList)
            {
                ServiceId = item.ServiceID;
            }

            if (IsAssignAgency == true && AssignAgencyToTestList.Count > 0)
            {
                lblAssignedAgency.Visibility = Visibility.Collapsed;
                txtAssignedAgency.Visibility = Visibility.Collapsed;

            }
            else if (IsAssignAgency == false && TestOutSourceDetails != null)
            {
                txtAssignedAgency.Text = TestOutSourceDetails.AgencyName;
                AssignedAgencyID = TestOutSourceDetails.AgencyID;
                ServiceId = TestOutSourceDetails.ServiceID;
            }
        }

        #region Private Methods
        private void FillAgency()
        {
            
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PathAgencyMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    List<MasterListItem> objList1 = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    // Added By Anumani on 14.03.2016 to get the Assigned agencies to a respective Service
                    //clsGetAssignedAgencyBizActionVO BizAction1 = new clsGetAssignedAgencyBizActionVO();
                    //BizAction1.ServiceID = ServiceId;
                    //List<clsGetAssignedAgencyBizActionVO> AgencyList = new List<clsGetAssignedAgencyBizActionVO>();

                    //Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    //PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    //Client1.ProcessCompleted += (s1, e1) =>
                    // {
                    //     if (e1.Error == null && e1.Result != null)
                    //     {
                             
                    //         AgencyList.AddRange(((clsGetAssignedAgencyBizActionVO)e1.Result).AgencyList);

                    //         foreach (var item in objList)
                    //         {
                    //             foreach (var item1 in AgencyList)
                    //             {
                    //                 if (item.ID == item1.AgencyID)
                    //                 {
                    //                     objList1.Add(item);
                    //                 }

                    //             }
                    //         }
                             cmbAgency.ItemsSource = null;
                             cmbAgency.ItemsSource = objList.DeepCopy();
                             cmbAgency.SelectedItem = objList[0];

                    //         if (txtAssignedAgency.Text == "" || txtAssignedAgency.Text == null)
                    //         {
                    //             txtAssignedAgency.Text = ((clsGetAssignedAgencyBizActionVO)e1.Result).DefaultAgencyName.ToString();

                    //         }

                    //     }
                    // };
                    //Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                    //Client1.CloseAsync();
                    //






                  
                    //if (IsAssignAgency == false && objList.Any(c => c.ID != AssignedAgencyID))
                    //       objList.Remove(objList.Where(x => x.ID != AssignedAgencyID).First());


                   
                  
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void ChangeAgency()
        {
            try
            {
                clsChangePathoTestAgencyBizActionVO BizAction = new clsChangePathoTestAgencyBizActionVO();
                BizAction.PathoOutSourceTestDetails = new clsPathoTestOutSourceDetailsVO();
                if (TestOutSourceDetails != null)
                {
                    BizAction.PathoOutSourceTestDetails.OrderDetailID = TestOutSourceDetails.OrderDetailID;
                }
                if (cmbAgency.SelectedItem != null && ((MasterListItem)cmbAgency.SelectedItem).ID > 0)
                {
                    BizAction.PathoOutSourceTestDetails.ChangedAgencyID = ((MasterListItem)cmbAgency.SelectedItem).ID;
                }
                if (!String.IsNullOrEmpty(txtChangeReason.Text))
                {
                    BizAction.PathoOutSourceTestDetails.ReasonToChangeAgency = txtChangeReason.Text;
                }
                BizAction.PathoOutSourceTestDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.PathoOutSourceTestDetails.IsForUnassignedAgencyTest = false;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Agency Changed From " + "'" + txtAssignedAgency.Text + "' " + "to" + "' " + ((MasterListItem)cmbAgency.SelectedItem).Description + " '", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    this.DialogResult = false;
                                    if (OnSaveButtonClick != null)
                                        OnSaveButtonClick(this, new RoutedEventArgs());
                                }
                            };
                            msgbox.Show();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //this.DialogResult = false;
            }
        }

        private void AssignAgency()
        {
            try
            {
                clsChangePathoTestAgencyBizActionVO BizAction = new clsChangePathoTestAgencyBizActionVO();
                if (AssignAgencyToTestList != null && AssignAgencyToTestList.Count > 0)
                    BizAction.AssignedAgnecyTestList = AssignAgencyToTestList;
                foreach (clsPathoTestOutSourceDetailsVO item in AssignAgencyToTestList)
                {
                    if (!String.IsNullOrEmpty(txtChangeReason.Text))
                        item.ReasonToChangeAgency = txtChangeReason.Text;
                    if (cmbAgency.SelectedItem != null && ((MasterListItem)cmbAgency.SelectedItem).ID > 0)
                        item.ChangedAgencyID = ((MasterListItem)cmbAgency.SelectedItem).ID;
                }
               
                BizAction.PathoOutSourceTestDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.PathoOutSourceTestDetails.IsForUnassignedAgencyTest = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Agency Assigned to" + "' " + ((MasterListItem)cmbAgency.SelectedItem).Description + " '", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                this.DialogResult = false;
                                if (OnSaveButtonClick != null)
                                    OnSaveButtonClick(this, new RoutedEventArgs());
                            }
                        };
                        msgbox.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
            catch (Exception)
            {

            }
            finally
            {
                //this.DialogResult = false;
            }
        }

        private bool Validation()
        {
            bool IsValidate = true;
            if (cmbAgency.SelectedItem == null || ((MasterListItem)cmbAgency.SelectedItem).ID == 0)
            {
                cmbAgency.TextBox.SetValidation("Please, Select Agecy.");
                cmbAgency.TextBox.RaiseValidationError();
                cmbAgency.TextBox.Focus();
                IsValidate = false;
            }
            //else
            //    cmbAgency.TextBox.ClearValidationError();
            if (String.IsNullOrEmpty(txtChangeReason.Text))
            {
                txtChangeReason.SetValidation("Reason Is Mandetory");
                txtChangeReason.RaiseValidationError();
                txtChangeReason.Focus();
                IsValidate = false;
            }
            //else
            //    txtChangeReason.Focus();

            return IsValidate;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        #endregion

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    if (IsAssignAgency == true)
                    {
                        AssignAgency();
                    }
                    else
                    {
                        ChangeAgency();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //this.DialogResult = false;
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

