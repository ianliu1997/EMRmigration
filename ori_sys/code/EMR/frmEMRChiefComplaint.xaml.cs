using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration;
using System.Windows.Data;
using PalashDynamics.Converters;
using PalashDynamics.ValueObjects.RSIJ;

namespace EMR
{
    public partial class frmEMRChiefComplaint : UserControl
    {
        #region DataMember
        public clsVisitVO CurrentVisit { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public clsEMRChiefComplaintsVO SavedComplaints { get; set; }
        ObservableCollection<clsChiefComplaintsVO> PastChiefComplaintList { get; set; }
        public Boolean IsEnabledControl { get; set; }
        int ClickFlag = 0;
        // string strErrorMsg = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
        #endregion

        #region Constructor
        public frmEMRChiefComplaint()
        {
            InitializeComponent();
            SavedComplaints = new clsEMRChiefComplaintsVO();
        }
        #endregion

        #region Loaded
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PastChiefComplaintList = new ObservableCollection<clsChiefComplaintsVO>();
            FetchChiefComplaints();
            this.DataContext = new clsEMRChiefComplaintsVO();
            txtChiefCompl.Width = txtAssCompl.ActualWidth;
            txtChiefCompl.Height = txtAssCompl.ActualHeight;
            FillPreviousChiefComplaints();
            if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
            {
                this.IsEnabledControl = false;
            }
            //else if (this.CurrentVisit.VisitTypeID == 1)
            //{
            //    //spSpecDoctor.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    //spSpecDoctor.Visibility = Visibility.Visible;
            //    //FillSpecialization();
            //    //FillDoctor();
            //}
            //if (!this.IsEnabledControl)
            //{
            // cmdSave.IsEnabled = false;
            // acbAssChiefComplaints.IsEnabled = false;
            // acbChiefComplaints.IsEnabled = false;
            //}
            //else
            //{
            FillChiefComplaints();
            //}
            //DateTime d = CurrentVisit.Date;
            //if (d.ToString("d") != DateTime.Now.ToString("d"))
            //{
            //    cmdSave.IsEnabled = false;
            //    acbAssChiefComplaints.IsEnabled = false;
            //    acbChiefComplaints.IsEnabled = false;
            //}

            // EMR Changes Added by Ashish Z. on dated 02062017
            if (CurrentVisit.EMRModVisitDate <= DateTime.Now)
            {
                cmdSave.IsEnabled = false;
                acbAssChiefComplaints.IsEnabled = false;
                acbChiefComplaints.IsEnabled = false;
            }
        }
        #endregion

        #region Events
        public void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickFlag += 1;
            if (ClickFlag == 1)
            {
                if (IsValidate())
                {
                    //string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
                    //MessageBoxControl.MessageBoxChildWindow msgW =
                    //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    //msgW.Show();

                    if (CurrentVisit != null)
                    {

                        SaveChiefComplaints();

                    }
                }
            }
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //string msgText = DefaultValues.ResourceManager.GetString("DiscardChanges");
            //MessageBoxControl.MessageBoxChildWindow msgWinCancel =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgWinCancel.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinCancel_OnMessageBoxClosed);
            //msgWinCancel.Show();
            NavigateToDashBoard();
        }

        void msgWinCancel_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                NavigateToDashBoard();
            }
        }

        private void NavigateToDashBoard()
        {
            this.Content = null;
            ((((((((this.Parent) as ContentControl).Parent as Border).Parent as DockPanel).Parent as DockPanel).FindName("tvPatientEMR") as TreeView)).Items[0] as TreeViewItem).IsSelected = true;
        }

        private void chkMultipleCC_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            TextBox txtTarget = chk.Name == "chkMultipleCC" ? txtChiefCompl : txtAssCompl;
            string strDescription = (chk.DataContext as MasterListItem).Description;
            if (chk.IsChecked == true && (chk.DataContext as MasterListItem).ID != 0 && !txtTarget.Text.Contains(strDescription))
            {
                txtTarget.Text = String.Format(txtTarget.Text + "," + strDescription).Trim(',');
            }
            else if (!String.IsNullOrEmpty(txtChiefCompl.Text))
            {
                txtTarget.Text = String.Format(txtTarget.Text.Replace(strDescription, String.Empty).Trim(',')).Trim();
                chk.IsChecked = false;
            }
        }

        //private void PanelDragDropTarget_Drop(object sender, Microsoft.Windows.DragEventArgs e)
        //{
        //    txtChiefCompl.Text = string.Format(txtChiefCompl.Text + ", " + ((clsChiefComplaintsVO)lbxSpecialization.SelectedItem).Description).Trim(',');
        //}

        //private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (cmbSpecialization.SelectedItem != null && ((MasterListItem)cmbSpecialization.SelectedItem).Code != "0")
        //            FillDoctor(((MasterListItem)cmbSpecialization.SelectedItem).Code);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ComplaintStatus win = new ComplaintStatus();
            win.txtComplaints.Text = ((clsChiefComplaintsVO)lbxSpecialization.SelectedItem).Description;
            win.OnSave_Click += new RoutedEventHandler(win_OnSave_Click);
            win.Show();
        }

        void win_OnSave_Click(object sender, RoutedEventArgs e)
        {
            ComplaintStatus wind = sender as ComplaintStatus;
            foreach (RadioButton item in wind.spStatus.Children)
            {
                string sContent = item.Content.ToString();

                if (item.IsChecked == true)
                    break;
            }
        }
        #endregion

        #region Private Methods

        private void SaveChiefComplaints()
        {
            clsAddUpdatePatientChiefComplaintsBizActionVO BizAction = new clsAddUpdatePatientChiefComplaintsBizActionVO();
            BizAction.VisitID = this.CurrentVisit.ID;
            BizAction.VisitUnitID = this.CurrentVisit.UnitId;
            BizAction.PatientID = this.CurrentVisit.PatientId;
            BizAction.PatientUnitID = this.CurrentVisit.PatientUnitId;
            //BizAction.DoctorCode = CurrentVisit.DoctorCode;
            BizAction.DoctorID = CurrentVisit.DoctorID;
            BizAction.CurrentChiefComplaints = this.DataContext as clsEMRChiefComplaintsVO;
            BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        FetchChiefComplaints();
                        //string strsavemsg = defaultvalues.resourcemanager.getstring("recordsaveprompt");
                        //messageboxcontrol.messageboxchildwindow msgw1 =
                        //           new messageboxcontrol.messageboxchildwindow("palash", strsavemsg, messageboxcontrol.messageboxbuttons.ok, messageboxcontrol.messageboxicon.information);
                        //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        //{
                        //    this.Content = null;

                        //    NavigateToNextMenu();
                        //};
                        //msgW1.Show();
                        //this.Content = null;
                        // NavigateToNextMenu();
                    }
                    else
                    {
                        ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                    ClickFlag = 0;
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                ClickFlag = 0;
                throw;
            }
        }

        private void NavigateToNextMenu()
        {
            UserControl winEMR;
            //if (this.CurrentVisit.VisitTypeID==2)
            //{
            //    winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            //}
            //else
            //{
            winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            //}
            TreeView tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            TreeViewItem SelectedItem = tvEMR.SelectedItem as TreeViewItem;
            clsMenuVO objMenu = SelectedItem.DataContext as clsMenuVO;
            if (SelectedItem.HasItems == true)
            {
                (SelectedItem.Items[0] as TreeViewItem).IsSelected = true;
            }
            else if (objMenu.Parent.Trim() == "Patient EMR")
            {
                int iCount = tvEMR.Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (objMenu.MenuOrder < iCount)
                {
                    if ((tvEMR.Items[iMenuIndex] as TreeViewItem).HasItems == true)
                    {
                        ((tvEMR.Items[iMenuIndex] as TreeViewItem).Items[0] as TreeViewItem).IsSelected = true;
                    }
                    else
                        (tvEMR.Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
            }
            else
            {
                int iCount = (SelectedItem.Parent as TreeViewItem).Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (iCount > objMenu.MenuOrder)
                {
                    ((SelectedItem.Parent as TreeViewItem).Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
                else
                {
                    objMenu = (SelectedItem.Parent as TreeViewItem).DataContext as clsMenuVO;
                    int iIndex = Convert.ToInt32(objMenu.MenuOrder);
                    (tvEMR.Items[iIndex] as TreeViewItem).IsSelected = true;
                }
            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes && CurrentVisit != null)
            {
                SaveChiefComplaints();
            }
        }
        private void FetchChiefComplaints()
        {
            clsGetPatientChiefComplaintsBizActionVO BizAction = new clsGetPatientChiefComplaintsBizActionVO();
            BizAction.VisitID = CurrentVisit.ID;
            BizAction.PatientID = CurrentVisit.PatientId;
            BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
            BizAction.UnitID = CurrentVisit.UnitId;
            BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
            BizAction.DoctorID = CurrentVisit.DoctorID;
            //BizAction.DoctorCode = CurrentVisit.DoctorCode;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetPatientChiefComplaintsBizActionVO Obj = e.Result as clsGetPatientChiefComplaintsBizActionVO;
                    this.DataContext = Obj.CurrentChiefComplaints;
                    if (this.DataContext == null)
                        this.DataContext = new clsEMRChiefComplaintsVO();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillPreviousChiefComplaints()
        {
            clsGetPatientPastChiefComplaintsBizActionVO BizAction = new clsGetPatientPastChiefComplaintsBizActionVO();
            BizAction.VisitID = CurrentVisit.ID;
            BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
            BizAction.PatientID = CurrentVisit.PatientId;
            BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
            BizAction.DoctorID = CurrentVisit.DoctorID;
            //BizAction.DoctorCode = CurrentVisit.DoctorCode;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetPatientPastChiefComplaintsBizActionVO Obj = e.Result as clsGetPatientPastChiefComplaintsBizActionVO;
                    lbxSpecialization.ItemsSource = null;
                    if (Obj.ChiefComplaintList != null)
                    {
                        List<clsChiefComplaintsVO> lstChiefComplaint = new List<clsChiefComplaintsVO>();
                        foreach (clsChiefComplaintsVO item in Obj.ChiefComplaintList)
                        {
                            if (!String.IsNullOrEmpty(item.Description))
                            {
                                string[] sArray = item.Description.Split(',');
                                for (int iCount = 0; iCount < sArray.Count(); iCount++)
                                {
                                    clsChiefComplaintsVO ObjChiefComplaints = new clsChiefComplaintsVO();
                                    ObjChiefComplaints.Description = sArray[iCount].ToTitleCase();
                                    ObjChiefComplaints.DoctorID = item.DoctorID;
                                    ObjChiefComplaints.VisitDate = item.VisitDate;
                                    ObjChiefComplaints.VisitID = item.VisitID;
                                    if (this.CurrentVisit.OPDIPD)
                                        ObjChiefComplaints.Datetime = String.Format(item.VisitDate.ToString() + "-" + item.DoctorName + "-" + item.DoctorSpec);
                                    lstChiefComplaint.Add(ObjChiefComplaints);
                                }
                            }
                        }
                        DateConverter dateConverter = new DateConverter();
                        PastChiefComplaintList = new ObservableCollection<clsChiefComplaintsVO>(lstChiefComplaint);
                        PagedCollectionView pcv = new PagedCollectionView(lstChiefComplaint);
                        if (CurrentVisit.VisitTypeID == 2)
                            if (this.CurrentVisit.OPDIPD)
                            {
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                                //  pcv.GroupDescriptions.Add(new PropertyGroupDescription("DoctorName"));
                            }
                            else
                            {
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                            }
                        else
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));

                        lbxSpecialization.ItemsSource = null;
                        lbxSpecialization.ItemsSource = pcv;
                    }
                    lbxSpecialization.UpdateLayout();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
        private Boolean IsValidate()
        {
            try
            {
                Boolean blnValid = true;
                clsEMRChiefComplaintsVO objCC = this.DataContext as clsEMRChiefComplaintsVO;
                if (String.IsNullOrEmpty(objCC.ChiefComplaints) && String.IsNullOrEmpty(objCC.AssChiefComplaints))
                {
                    blnValid = false;
                    // string strMsg = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("EnterCC_Msg");
                    string strMsg = "Please Enter Chief Complaints";
                    ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                else if (objCC.ChiefComplaints == SavedComplaints.ChiefComplaints && objCC.AssChiefComplaints == SavedComplaints.AssChiefComplaints)
                {
                    blnValid = false;
                    string strMsg = "Please Enter New Complaints";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("EnterNewCC_Msg");
                    ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                return blnValid;
            }
            catch (Exception ex)
            {
                return false;
                ClickFlag = 0;
            }
        }
        #endregion

        #region Combo Box
        private void FillChiefComplaints()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_EMRChiefComplaints;
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
                    List<MasterListItem> lstCC = objList.DeepCopy();
                    List<MasterListItem> lstAssCC = objList.DeepCopy();
                    acbChiefComplaints.ItemsSource = null;
                    acbChiefComplaints.ItemsSource = lstCC;
                    acbChiefComplaints.SelectedItem = lstCC[0];
                    acbAssChiefComplaints.ItemsSource = null;
                    acbAssChiefComplaints.ItemsSource = lstAssCC;
                    acbAssChiefComplaints.SelectedItem = lstAssCC[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion
    }
}
