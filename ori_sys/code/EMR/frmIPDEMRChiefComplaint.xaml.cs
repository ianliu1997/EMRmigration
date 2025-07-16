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
    public partial class frmIPDEMRChiefComplaint : UserControl
    {
        #region DataMember
        public clsVisitVO CurrentVisit { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public clsEMRChiefComplaintsVO SavedComplaints { get; set; }
        ObservableCollection<clsChiefComplaintsVO> PastChiefComplaintList { get; set; }
        public Boolean IsEnabledControl { get; set; }
        string strErrorMsg = "Error ocurred while processing.";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
        #endregion

        #region Constructor
        public frmIPDEMRChiefComplaint()
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
                spSpecDoctor.Visibility = Visibility.Collapsed;
                this.IsEnabledControl = false;
            }
            else
            {
                if (CurrentVisit.ISIPDDischarge)
                {
                    this.IsEnabledControl = false;
                }
                else
                {
                    this.IsEnabledControl = true;
                }
                spSpecDoctor.Visibility = Visibility.Visible;
            }
            FillSpecialization();
            FillDoctor();

            if (!this.IsEnabledControl)
            {
                cmdSave.IsEnabled = false;
                acbAssChiefComplaints.IsEnabled = false;
                acbChiefComplaints.IsEnabled = false;
            }
            else
            {
                FillChiefComplaints();
            }
        }
        #endregion

        int Flag = 0;
        #region Events
        public void cmdSave_Click(object sender, RoutedEventArgs e)
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
                    Flag += 1;
                    if (Flag == 1)
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
                        Flag = 0;
                    }
                    else
                    {
                        ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                    
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                Flag = 0;
                throw;
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
                                        ObjChiefComplaints.Datetime = String.Format(item.VisitDate.ToString("dd/MM/yyyy") + " - " + item.DoctorName.Trim() + " - " + item.DoctorSpec);
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
            Boolean blnValid = true;
            clsEMRChiefComplaintsVO objCC = this.DataContext as clsEMRChiefComplaintsVO;
            if (String.IsNullOrEmpty(objCC.ChiefComplaints) && String.IsNullOrEmpty(objCC.AssChiefComplaints))
            {
                blnValid = false;
                string strMsg = "Please Enter Chief Complaints";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("EnterCC_Msg");
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

        private void FillDoctor()
        {
            //private void FillDoctor(string sDeptCode)
            //clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = new clsGetRSIJDoctorDepartmentDetailsBizActionVO();
            //BizAction.MasterList = new List<MasterListItem>();
            //if (cmbSpecialization.SelectedItem != null)
            //{
            //    BizAction.IsForReferral = true;
            //    BizAction.DoctorCode = sDeptCode;
            //}
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem("0", "-- Select --"));
            //        objList.AddRange(((clsGetRSIJDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
            //        cmbDoctor.ItemsSource = null;
            //        cmbDoctor.ItemsSource = objList;
            //        cmbDoctor.SelectedItem = objList[0];
            //        if (this.DataContext != null)
            //        {
            //            cmbDoctor.SelectedValue = objList[0].ID;
            //            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            //            {
            //                cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
            //            }
            //            else
            //                cmbDoctor.SelectedValue = 0;
            //        }
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(this.CurrentVisit.DoctorCode, this.CurrentVisit.Doctor));
            cmbDoctor.ItemsSource = null;
            cmbDoctor.ItemsSource = objList;
            cmbDoctor.SelectedItem = objList[0];
            cmbDoctor.IsEnabled = false;
        }

        private void FillSpecialization()
        {
            try
            {
                //clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
                //BizAction.MasterTable = MasterTableNameList.SPESIAL;
                //BizAction.CodeColumn = "KDSPESIAL";
                //BizAction.DescriptionColumn = "NMSPESIAL";
                //BizAction.MasterList = new List<MasterListItem>();
                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //client.ProcessCompleted += (s, arg) =>
                //{
                //    if (arg.Error == null && arg.Result != null)
                //    {
                //        List<MasterListItem> objList = new List<MasterListItem>();
                //        objList.Add(new MasterListItem("0", "-- Select --"));
                //        objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
                //        cmbSpecialization.ItemsSource = null;
                //        cmbSpecialization.ItemsSource = objList;
                //        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                //        {
                //            string sSpecCode = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorSpecCode;
                //            cmbSpecialization.SelectedItem = objList.Where(z => z.Code == sSpecCode).FirstOrDefault();
                //        }
                //        else
                //        {
                //            cmbSpecialization.SelectedItem = objList[0];
                //        }
                //    }
                //};
                //client.ProcessAsync(BizAction, new clsUserVO());
                List<MasterListItem> objList = new List<MasterListItem>();
                objList.Add(new MasterListItem("0", this.CurrentVisit.DoctorSpecialization));
                cmbSpecialization.ItemsSource = null;
                cmbSpecialization.ItemsSource = objList;
                cmbSpecialization.SelectedItem = objList[0];
                cmbSpecialization.IsEnabled = false;

            }
            catch (Exception)
            {
            }
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
