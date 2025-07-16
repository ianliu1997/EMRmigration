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
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Controls.Primitives;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using System.Collections.ObjectModel;


namespace PalashDynamics.IVF
{
    public partial class TESE_Report : UserControl, IInitiateCIMS
    {
        #region Variables
        public bool IsPatientExist = false;
        public bool IsPageLoded = false;
        public bool IsSaved = false;
        WaitIndicator wi = new WaitIndicator();
        long PatientID = 0;
        long NewTeseID = 0;
        bool IsNew = false;
        string Impression { get; set; }
        #endregion

        #region Properties
        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }
        public PagedSortableCollectionView<clsArtCycleSummaryVO> DataList { get; private set; }
        //public PagedSortableCollectionView<clsTESEDetailsVO> DataList1 { get; private set; }
        // public PagedSortableCollectionView<clsAddUpdateTESEBizActionVO> DataList2 { get; private set; }

        public ObservableCollection<clsTESEDetailsVO> DataList1 { get; set; }
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }
        string msgTitle = "";
        string msgText = "";
        #endregion

        public void Initiate(string Mode)
        {
            switch (Mode.ToUpper())
            {
                case "NEW":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;

                default:
                    break;
            }
        }


        public TESE_Report()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender != Genders.Female.ToString())
            {
                if (!IsPageLoded)
                {
                    if (IsPatientExist == false)
                    {
                        dtETDate.SelectedDate = DateTime.Now.Date.Date;
                        txtTime.Value = DateTime.Now;
                    }

                    if (PatientID >= 0)
                    {
                        try
                        {
                            wi.Show();
                            fillCoupleDetails();
                            FillEmbryologist();
                            FillLabIncharge();
                            FillTissueSide();
                            Cmdmodify.IsEnabled = false;
                            CmdSave.IsEnabled = false;
                        }
                        catch (Exception ex)
                        { throw ex; }
                        finally
                        { wi.Close(); }
                    }
                    IsPageLoded = true;
                }
            }
            else
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void FillEmbryologist()
        {
            clsGetEmbryologistBizActionVO BizAction = new clsGetEmbryologistBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = 0;
            BizAction.DepartmentId = 0;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetEmbryologistBizActionVO)arg.Result).MasterList);

                    cmbEmbryologist.ItemsSource = null;
                    cmbEmbryologist.ItemsSource = objList;
                    cmbEmbryologist.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbEmbryologist.SelectedValue = ((clsFemaleLabDay4VO)this.DataContext).EmbryologistID;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillTissueSide()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TESETissueSide;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbtissueSIde.ItemsSource = null;
                    cmbtissueSIde.ItemsSource = objList;
                    cmbtissueSIde.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void FillLabIncharge()
        {
            clsGetUserMasterListBizActionVO BizAction = new clsGetUserMasterListBizActionVO();
            BizAction.IsDecode = true;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetUserMasterListBizActionVO)e.Result).MasterList);

                    var result = from item in objList
                                 where item.ID == ((IApplicationConfiguration)App.Current).CurrentUser.ID
                                 select item;
                    cmbAssistantEmbryologist.ItemsSource = null;
                    cmbAssistantEmbryologist.ItemsSource = objList;
                    cmbAssistantEmbryologist.SelectedItem = result.ToList()[0];
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                    BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                    CoupleDetails.MalePatient = new clsPatientGeneralVO();
                    CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                    {
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                        GetHeightAndWeight(BizAction.CoupleDetails);
                        FetchData(CoupleDetails);
                    }
                    else
                    {
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private bool ChkValidation()
        {
            bool result = true;
            if (dtETDate.SelectedDate == null)
            {
                dtETDate.SetValidation("Please Select The Cryopreservation Date.");
                dtETDate.RaiseValidationError();
                dtETDate.Focus();
                result = false;
            }
            else
                dtETDate.ClearValidationError();

            if (txtTime.Value == null)
            {
                txtTime.SetValidation("Please Select The Cryopreservation Time.");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                result = false;
            }
            else
                txtTime.ClearValidationError();

            if (cmbEmbryologist.SelectedItem == null)
            {
                cmbEmbryologist.TextBox.SetValidation("Please Select The Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                result = false;

            }
            else if (((MasterListItem)cmbEmbryologist.SelectedItem).ID == 0)
            {
                cmbEmbryologist.TextBox.SetValidation("Please Select The Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                result = false;

            }
            else
                cmbEmbryologist.TextBox.ClearValidationError();
            if (cmbAssistantEmbryologist.SelectedItem == null)
            {
                cmbAssistantEmbryologist.TextBox.SetValidation("Please Select The Lab Incharge");
                cmbAssistantEmbryologist.TextBox.RaiseValidationError();
                cmbAssistantEmbryologist.Focus();
                result = false;

            }
            else if (((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID == 0)
            {
                cmbAssistantEmbryologist.TextBox.SetValidation("Please Select The Lab Incharge");
                cmbAssistantEmbryologist.TextBox.RaiseValidationError();
                cmbAssistantEmbryologist.Focus();
                result = false;

            }
            else
                cmbAssistantEmbryologist.TextBox.ClearValidationError();

            if (cmbtissueSIde.SelectedItem == null)
            {
                cmbtissueSIde.TextBox.SetValidation("Please Select The Tissue Side");
                cmbtissueSIde.TextBox.RaiseValidationError();
                cmbtissueSIde.Focus();
                result = false;

            }
            else if (((MasterListItem)cmbtissueSIde.SelectedItem).ID == 0)
            {
                cmbtissueSIde.TextBox.SetValidation("Please Select The Tissue Side");
                cmbtissueSIde.TextBox.RaiseValidationError();
                cmbtissueSIde.Focus();
                result = false;

            }
            else
                cmbtissueSIde.TextBox.ClearValidationError();

            if (dtETDate.SelectedDate > dtRnwlDate.SelectedDate)
            {
                dtRnwlDate.SetValidation("Please select valid Renewal date");
                dtRnwlDate.RaiseValidationError();
                dtRnwlDate.Focus();
                result = false;
            }
            else
                dtRnwlDate.ClearValidationError();
            if (dtRnwlDate.SelectedDate == null)
            {
                dtRnwlDate.SetValidation("Please Select The Renewal Date.");
                dtRnwlDate.RaiseValidationError();
                dtRnwlDate.Focus();
                result = false;
            }
            else
                dtRnwlDate.ClearValidationError();
            return result;
        }

        private void GetHeightAndWeight(clsCoupleVO CoupleDetails)
        {
            clsGetGetCoupleHeightAndWeightBizActionVO BizAction = new clsGetGetCoupleHeightAndWeightBizActionVO();
            BizAction.CoupleDetails = new clsCoupleVO();
            BizAction.FemalePatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails != null)
                        BizAction.CoupleDetails = ((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails;
                    if (BizAction.CoupleDetails != null)
                    {
                        clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                        FemalePatientDetails = CoupleDetails.FemalePatient;
                        FemalePatientDetails.Height = BizAction.CoupleDetails.FemalePatient.Height;
                        FemalePatientDetails.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                        FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                        FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                        Female.DataContext = FemalePatientDetails;

                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        MalePatientDetails = CoupleDetails.MalePatient;
                        MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                        MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                        MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                        Male.DataContext = MalePatientDetails;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "";
            string msgText = "Are You Sure \n You Want To SAVE  ?";

            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinSAVE_OnMessageBoxClosed);
            msgWin.Show();
        }
        void msgWinSAVE_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Modify();
                CmdAdd.IsEnabled = true;
            }
            else
            {
                string msgTitle = "";
                string msgText = "Your Connection Is Slow.";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            }
        }

        private void Modify()
        {
            clsAddUpdateTESEBizActionVO BizAction = new clsAddUpdateTESEBizActionVO();
            BizAction.TESEDetailsList = new List<clsTESEDetailsVO>();
            List<clsTESEDetailsVO> mylist = new List<clsTESEDetailsVO>();
            BizAction.TESE.CoupleDetail = CoupleDetails;
            if (TeseList.Count > 0)
            {
                mylist = TeseList;
                BizAction.TESEDetailsList = mylist;
            }
            else
            {
                foreach (clsTESEDetailsVO item in DataList1)
                {
                    mylist.Add(item);
                }
                BizAction.TESEDetailsList = mylist;
            }
            BizAction.TESE.ID = teseid;
            BizAction.TESE.UnitID = teseunitid;  
            BizAction.TESE.EmbroLogistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;
            BizAction.TESE.LabInchargeID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;

            BizAction.TESE.TissueSideID = ((MasterListItem)cmbtissueSIde.SelectedItem).ID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsAddUpdateTESEBizActionVO result = arg.Result as clsAddUpdateTESEBizActionVO;
                    if (result.SuccessStatus != 0)
                    {

                        FetchData(CoupleDetails);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "TESE  Report Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            ((IApplicationConfiguration)App.Current).FillMenu("Clinical");
        }

        private void SaveRecord()
        {
            try
            {
                PalashDynamics.ValueObjects.Patient.clsTESEDetailsVO TeseDetail = new PalashDynamics.ValueObjects.Patient.clsTESEDetailsVO();
                TeseDetail.NoofVailsFrozen = Convert.ToInt32(txtvisalfrozen.Text);
                //TeseDetail.TANKNumber = Convert.ToInt32(txtTankNo.Text);

                TeseDetail.ContainerNumber = Convert.ToInt32(txtContNo.Text);
                TeseDetail.HolderNumber = Convert.ToInt32(txtholderNo.Text);
                TeseDetail.Tissue = txtTissue.Text;
                TeseDetail.No_of_VailsUsed = Convert.ToInt32(txtvisalUsed.Text);
                TeseDetail.No_of_VailsRemain = Convert.ToInt32(txvisalRmng.Text);
                TeseDetail.RenewalDate = dtRnwlDate.SelectedDate;
                TeseDetail.CryoDate = dtETDate.SelectedDate.Value.Date;
                TeseDetail.CryoTime = txtTime.Value;
                TeseDetail.TissueSideID = ((MasterListItem)cmbtissueSIde.SelectedItem).ID;

                TeseDetail.EmbroLogistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;
                TeseDetail.EmbroLogist = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).Description;
                TeseDetail.LabInchargeID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;
                TeseDetail.LabIncharge = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).Description;

                TeseList.Add(TeseDetail);
                dgETDetilsGrid.ItemsSource = TeseList; // TeseList;
                dgETDetilsGrid.UpdateLayout();
                dgETDetilsGrid.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            ((IApplicationConfiguration)App.Current).FillMenu("Clinical");
        }

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
        }

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {
        }

        public void ClearControl()
        {
            dtETDate.IsEnabled = false;
            txtTime.IsEnabled = false;
            cmbEmbryologist.IsEnabled = false;
            cmbAssistantEmbryologist.IsEnabled = false;
            // Cmdmodify.IsEnabled = false;
            txtvisalfrozen.Text = " ";
            txtContNo.Text = " ";
            txtholderNo.Text = " ";
            txtTissue.Text = " ";
            txvisalRmng.Text = " ";
            txtvisalUsed.Text = " ";
            dtRnwlDate.Text = " ";
            //txtTankNo.Text = " ";
            //    cmbtissueSIde.SelectedItem = null;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dgETDetilsGrid.SelectedItem != null)
            {
                CmdSave.IsEnabled = false;
                Cmdmodify.IsEnabled = true;
                // CmdSave.Content = "Modify";
                IsNew = false;
                this.DataContext = (clsTESEDetailsVO)dgETDetilsGrid.SelectedItem;
                dtETDate.Text = Convert.ToString(((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).CryoDate);
                txtTime.Value = Convert.ToDateTime(((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).CryoTime);
                cmbEmbryologist.SelectedValue = ((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).EmbroLogistID;
                cmbAssistantEmbryologist.SelectedValue = ((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).LabInchargeID;
                cmbtissueSIde.SelectedValue = ((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).TissueSideID;
                txtvisalfrozen.Text = Convert.ToString(((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).NoofVailsFrozen);
               // txtTankNo.Text = Convert.ToString(((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).TANKNumber);
                txtContNo.Text = Convert.ToString(((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).ContainerNumber);
                txtholderNo.Text = Convert.ToString(((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).HolderNumber);
                txtTissue.Text = Convert.ToString(((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).Tissue);
                txvisalRmng.Text = Convert.ToString(((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).No_of_VailsRemain);
                txtvisalUsed.Text = Convert.ToString(((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).No_of_VailsUsed);
                dtRnwlDate.Text = Convert.ToString(((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).RenewalDate);
            }
            CmdAdd.IsEnabled = false;
            dtETDate.IsEnabled = false;
            txtTime.IsEnabled = false;
        }
        private void hprlnkViewFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgETDetilsGrid.SelectedItem != null)
            {
                TESEID = ((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).TESE_ID;
                TESE_UnitID = ((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).TESE_UnitID;
                string msgTitle = "";
                string msgText = "Are You Sure \n You Want To Print ?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinPrint_OnMessageBoxClosed);

                msgWin.Show();
            }
            else
            {
                string msgTitle = "";
                string msgText = "Please Select The TESE To Print. ";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.Show();
            }
        }

        long TESEID;
        long TESE_UnitID;

        void msgWinPrint_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                string URL = "../Reports/IVF/TESE_Report.aspx?TESEID=" + TESEID + "&TESE_UnitID=" + TESE_UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void CmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ChkValidation())
            {
                string msgTitle = "";
                string msgText = "Are You Sure \n You Want To ADD ?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                msgWin.Show();
            }
            CmdSave.IsEnabled = true;
        }

        List<clsTESEDetailsVO> TeseList = new List<clsTESEDetailsVO>();
        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                try
                {
                    SaveRecord();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            ClearControl();
        }
        private void dgETDetilsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgETDetilsGrid.SelectedItem != null)
            {
            }
        }

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            CmdSave.IsEnabled = true;

            if (DataList1.Count > 0)
            {
                foreach (var item in DataList1)
                {
                    if (item.ID == (((clsTESEDetailsVO)dgETDetilsGrid.SelectedItem).ID))
                    {
                        item.LabInchargeID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;
                        item.LabIncharge = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).Description;
                        item.TissueSideID = ((MasterListItem)cmbtissueSIde.SelectedItem).ID;
                        item.Tissue = txtTissue.Text;
                        item.NoofVailsFrozen = Convert.ToInt32(txtvisalfrozen.Text);
                        item.ContainerNumber = Convert.ToInt32(txtContNo.Text);
                       // item.TANKNumber = Convert.ToInt32(txtTankNo.Text);
                        item.HolderNumber = Convert.ToInt32(txtholderNo.Text);
                        item.No_of_VailsUsed = Convert.ToInt32(txtvisalUsed.Text);
                        item.No_of_VailsRemain = Convert.ToInt32(txvisalRmng.Text);
                        item.RenewalDate = dtRnwlDate.SelectedDate;
                        item.CryoDate = dtETDate.SelectedDate.Value.Date;
                        item.CryoTime = txtTime.Value;
                    }
                }
                dgETDetilsGrid.ItemsSource = null;
                dgETDetilsGrid.ItemsSource = DataList1;
                dgETDetilsGrid.Focus();
                dgETDetilsGrid.UpdateLayout();
                dgETDetilsGrid.SelectedIndex = DataList1.Count - 1;
            }

        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtvisalfrozen_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid())
            {
                // ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void cmbAssistantEmbryologist_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }
        long teseid = 0;
        long teseunitid = 0;
        private void FetchData(clsCoupleVO CoupleDetails)
        {
            try
            {
                PalashDynamics.ValueObjects.Patient.clsGetTESEBizActionVO bizaction = new PalashDynamics.ValueObjects.Patient.clsGetTESEBizActionVO();
                bizaction.TESE = new clsTESEVO();
                bizaction.TESE.CoupleID = CoupleDetails.CoupleId;
                bizaction.TESE.CoupleUnitID = CoupleDetails.CoupleUnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        clsGetTESEBizActionVO result = e.Result as clsGetTESEBizActionVO;
                        DataList1 = new PagedSortableCollectionView<clsTESEDetailsVO>();
                       
                        DataList1.Clear();
                        if (result.TESEDeatailsList != null)
                        {
                            foreach (var item in result.TESEDeatailsList)
                            {
                                DataList1.Add(item);
                            }
                        }
                        if (DataList1.Count != 0)
                        {
                            teseid = DataList1[0].TESE_ID;
                            teseunitid = DataList1[0].TESE_UnitID;
                        }
                        else
                        {
                            teseid = 0;
                            teseunitid = 0;
                        }
                        dgETDetilsGrid.ItemsSource = null;
                        dgETDetilsGrid.ItemsSource = DataList1;
                    }
                };
                client.ProcessAsync(bizaction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dtETDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void CmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            ((IApplicationConfiguration)App.Current).FillMenu("Clinical");
        }

        private void CmdSave_Checked(object sender, RoutedEventArgs e)
        {
        }
        private bool CheckDuplicasy()
        {

            PalashDynamics.ValueObjects.Patient.clsAddUpdateTESEBizActionVO bizactionVO = new PalashDynamics.ValueObjects.Patient.clsAddUpdateTESEBizActionVO();
            clsTESEDetailsVO Item;
            Item = ((PagedSortableCollectionView<clsTESEDetailsVO>)dgETDetilsGrid.ItemsSource).FirstOrDefault(p => Equals(bizactionVO.TESE.ID));
            if (Item != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Cannot Be Save Because Record Already Exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void cmbEmbryologist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cmbAssistantEmbryologist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cmbtissueSIde_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cmbtissueSIde_KeyDown(object sender, KeyEventArgs e)
        {
        }

    }
}
