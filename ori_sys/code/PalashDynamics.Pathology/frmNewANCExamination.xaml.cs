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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.IO;
using System.Windows.Browser;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;
using PalashDynamics.ValueObjects.ANC;
using MessageBoxControl;
using System.Text;

namespace PalashDynamics.Pathology
{
    public partial class frmNewANCExamination : UserControl,IInitiateCIMS
    {
        #region Variables
        public bool IsPatientExist = false;
        public bool IsPageLoded = false;
        bool IsCancel = false;
        bool IsModify = false;
        bool IsNew = false;
        WaitIndicator wait = new WaitIndicator();
        private SwivelAnimation objAnimation;
        WaitIndicator wi = new WaitIndicator();
        public PagedSortableCollectionView<clsANCVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsANCObstetricHistoryVO> obestricHistoryList { get; set; }
        public PagedSortableCollectionView<clsANCInvestigationDetailsVO> investigationList { get; set; }
        public PagedSortableCollectionView<clsANCUSGDetailsVO> USGList { get; set; }
        List<clsANCExaminationDetailsVO> ExaminationList1 = new List<clsANCExaminationDetailsVO>();
        List<clsANCDocumentsVO> DoucumentList = new List<clsANCDocumentsVO>();
        List<MasterListItem> ObjSig = new List<MasterListItem>();
        List<MasterListItem> objList1 = new List<MasterListItem>();
        public int DataListPageSize;
        public int obestricHistoryListPageSize;
        public int investigationListPageSize;
        byte[] AttachedFileContents;
        string AttachedFileName;
        string ConsultDescription = "";
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        long CoupleId { get; set; }
        long CoupleUnitId { get; set; }
        long PatientID = 0;
        long GridSelectedItemID = 0;
        long ANCId = 0;
        bool IsClosed = true;
        long CoupleID = 0;
        long CoupleUnitID = 0;
        long AncID = 0;
        long DocumentId;
        long SuggID = 0, SuggUnitID = 0;
        long HisID = 0, HisUnitID = 0;
        bool HisIsfreezed = false;
        bool result = true;
        DateTime dateEDD;
        long USGID = 0;
        long USGUnitID = 0;
        #endregion

        #region IInitiateCIMS Members
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "New":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();

                        IsPatientExist = false;
                        break;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID != 12)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select ANC Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;
            }
        }
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

        string msgTitle = "";
        string msgText = "";
        #endregion


        public frmNewANCExamination()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (!IsPageLoded)
            {
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                }

                if (PatientID >= 0)
                {
                    if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                    {
                        try
                        {
                            objAnimation = new SwivelAnimation(grdFrontPanel, grdBackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                            DataList = new PagedSortableCollectionView<clsANCVO>();
                            obestricHistoryList = new PagedSortableCollectionView<clsANCObstetricHistoryVO>();
                            investigationList = new PagedSortableCollectionView<clsANCInvestigationDetailsVO>();
                            USGList = new PagedSortableCollectionView<clsANCUSGDetailsVO>();
                            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                            obestricHistoryList.OnRefresh += new EventHandler<RefreshEventArgs>(obestricHistoryList_OnRefresh);
                            investigationList.OnRefresh += new EventHandler<RefreshEventArgs>(investigationList_OnRefresh);
                            USGList.OnRefresh += new EventHandler<RefreshEventArgs>(USGList_OnRefresh);
                            DataListPageSize = 15;
                            obestricHistoryListPageSize = 15;
                            this.DataContext = new clsANCVO();
                            PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                            wi.Show();
                            LoadPatientHeader();
                            fillDoctor();
                            FillFHS();
                            FillInvestigation();
                            FillPresentationAndPosition();
                            SetButtonState("New");
                            CmdSave.IsEnabled = false;
                            FillANCDataGird();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            wi.Close();
                        }
                        IsPageLoded = true;
                    }
                }
            }
        }
        #region List_OnRefresh events
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            // SetupPage();
        }

        void obestricHistoryList_OnRefresh(object sender, RefreshEventArgs r)
        {
            //  GetObestricHistoryList();
        }

        void investigationList_OnRefresh(object sender, RefreshEventArgs r)
        {
            // FillInvestigationDetailsGrid();
        }

        void USGList_OnRefresh(object senser, RefreshEventArgs r)
        {
            // FillUSGDataGrid();
        }
        #endregion

        #region Couple Details
        private void LoadPatientHeader()
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Female.ToString())
            {
                clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
                BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();
                BizAction.PatientDetails.GeneralDetails = (clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.PatientDetails.GeneralDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                        BizAction.PatientDetails.SpouseDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.SpouseDetails;
                        PatientInfo.Visibility = Visibility.Visible;
                        //CoupleInfo.Visibility = Visibility.Collapsed;
                        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                        
                            if (((clsGetPatientBizActionVO)args.Result).PatientDetails.Photo != null)
                            {
                                WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                bmp.FromByteArray(((clsGetPatientBizActionVO)args.Result).PatientDetails.Photo);
                                imgPhoto13.Source = bmp;
                                imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                                imgP1.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            else
                            {
                                imgP1.Visibility = System.Windows.Visibility.Visible;
                                imgPhoto13.Visibility = System.Windows.Visibility.Collapsed;
                            }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                wait.Close();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "ANC is For Female", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
        }
        #endregion

        #region fill Combox

        private void FillFHS()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ANC_FHS;
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

                    cmbFHS.ItemsSource = null;
                    cmbFHS.ItemsSource = objList;
                    cmbFHS.SelectedValue = (long)0;
                }

            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillDoctor()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
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
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;
                    cmbDoctor.SelectedValue = (long)0;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillInvestigation()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ANC_Investigation;
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
                    cmbInvestigation.ItemsSource = null;
                    cmbInvestigation.ItemsSource = objList;
                    cmbInvestigation.SelectedValue = (long)0;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillPresentationAndPosition()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ANC_PresentationPosition;
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

                    cmbPreAndPosition.ItemsSource = null;
                    cmbPreAndPosition.ItemsSource = objList;
                    cmbPreAndPosition.SelectedValue = (long)0;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillConsult()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ANC_Consult;
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

                    cmbconsult.ItemsSource = null;
                    cmbconsult.ItemsSource = objList;
                    cmbconsult.SelectedValue = (long)0;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillSignificant()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ANC_Consult;
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
                    objList1 = objList;
                    cmbconsult.ItemsSource = null;
                    cmbconsult.ItemsSource = objList1;
                    cmbconsult.SelectedValue = (long)0; ;
                }
                if (ObjSig.Count > 0)
                {
                    if (ObjSig != null)
                    {
                        foreach (var item in ObjSig)
                        {
                            if (objList1 != null)
                            {
                                foreach (MasterListItem item1 in objList1)
                                {
                                    if (item.ID == item1.ID)
                                    {
                                        item1.Status = item.Status;
                                    }
                                }
                            }
                        }
                        cmbconsult.ItemsSource = null;
                        cmbconsult.ItemsSource = objList1;
                        cmbconsult.UpdateLayout();
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void fillHirsutism()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ANC_Hirsutism;
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
                    objList1 = objList;
                    cmbHirsutism.ItemsSource = null;
                    cmbHirsutism.ItemsSource = objList1;
                    cmbHirsutism.SelectedValue = (long)0;

                    HistoryDetails();
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void fillOedema()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ANC_Oedema;
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
                    objList1 = objList;
                    cmbOedema.ItemsSource = null;
                    cmbOedema.ItemsSource = objList1;
                    cmbOedema.SelectedValue = (long)0; ;
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        #endregion

        #region Button State
        private void SetButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    CmdNew.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Save":
                    CmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    CmdNew.IsEnabled = false;
                    CmdModify.IsEnabled = true;
                    CmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    CmdNew.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region button New/Save/Modify/cancel/View click
        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsClosed == false)
                {
                    string msgText = "Please close Previous ANC therapy";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();

                }
                else
                {
                    objAnimation.Invoke(RotationType.Forward);
                    if (GeneralDetail != null)
                    {
                        GeneralDetail.IsSelected = true;
                        //if (GeneralDetail.IsSelected)
                        //{
                        IsModify = false;


                        ClearGeneralDetailsUI();
                        //}
                    }
                    History.Visibility = Visibility.Collapsed;
                    Investigation.Visibility = Visibility.Collapsed;
                    Examination.Visibility = Visibility.Collapsed;
                    TabDocuments.Visibility = Visibility.Collapsed;
                    Suggestion.Visibility = Visibility.Collapsed;
                    SetButtonState("Save");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

            if (GeneralDetail.IsSelected)
            {
                if (Validation())
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n  You Want To Save ANC Details?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            SaveANCGeneralDetails();
                        }
                    };
                    msgWin.Show();
                }
            }
            if (History.IsSelected)
            {
                if (HistoryValidations())
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n  You Want To Save ANC History?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            AddUpdateANCHistory();
                        }
                    };
                    msgWin.Show();
                }
            }
            if (Investigation.IsSelected)
            {
                //if (investigationList.Count > 0)
                //{
                //    string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Save ANC Investigation Details?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        AddUpdateInvestigationDetails();
                        AddUpdateUSGdetails();
                    }
                };
                msgWin.Show();
                //}
                //else
                //{
                //    string msgTitle = "Palash";
                //    string msgText = "Please fill the Details First";
                //    MessageBoxControl.MessageBoxChildWindow msgWin =
                //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgWin.Show();
                //}
            }
            if (Examination.IsSelected)
            {
                if (ExaminationList1.Count > 0)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n  You Want To Save ANC Examination?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            AddUpdateExamination();
                        }
                    };
                    msgWin.Show();
                }
                else
                {
                    string msgTitle = "Palash";
                    string msgText = "Please fill the Details First";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWin.Show();
                }
            }

            if (Suggestion.IsSelected)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Save ANC Suggestion?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        AddUpdateANCSuggestion();

                    }
                };
                msgWin.Show();
            }
            if (TabDocuments.IsSelected)
            {
                if (DoucumentList.Count > 0)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n  You Want To Save ANC Document?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            SaveDocument();
                        }
                    };
                    msgWin.Show();
                }
                else
                {
                    string msgTitle = "Palash";
                    string msgText = "Please Add File first";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWin.Show();
                }
            }

        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsCancel == true)
                {
                    grdBackPanel.DataContext = new clsPlanTherapyVO();
                    objAnimation.Invoke(RotationType.Backward);
                    CmdNew.Visibility = Visibility.Visible;
                    FillANCDataGird();
                    IsCancel = false;
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                }
                else
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void cmdViewANC_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                GeneralDetail.IsSelected = true;
                objAnimation.Invoke(RotationType.Forward);
                if (GeneralDetail.IsSelected)
                {
                    ViewGeneralDetails();
                    HistoryDetails();
                    if (IsClosed == true)
                    {
                        CmdNew.IsEnabled = false;
                        CmdModify.IsEnabled = false;
                        CmdSave.IsEnabled = false;
                    }
                    else
                    {
                        CmdNew.IsEnabled = true;
                        CmdModify.IsEnabled = true;
                        CmdSave.IsEnabled = true;
                    }
                    History.Visibility = Visibility.Visible;
                    Investigation.Visibility = Visibility.Visible;
                    Examination.Visibility = Visibility.Visible;
                    TabDocuments.Visibility = Visibility.Visible;
                    Suggestion.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region selection changed events
        private void LMP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateEDD = LMP.SelectedDate.Value.Date;
            dateEDD = dateEDD.AddYears(1);
            dateEDD = dateEDD.AddMonths(-3);
            dateEDD = dateEDD.AddDays(7);
            EDD.SelectedDate = dateEDD;
        }
        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            wait.Show();
            if (GeneralDetail != null)
            {
                if (GeneralDetail.IsSelected)
                {

                }
            }
            if (History != null)
            {

                if (History.IsSelected)
                {

                    fillHirsutism();
                    clearObstericHistory();
                    ClearHistoryUI();
                    IsModify = false;
                    //DateEDD.SelectedDate = ((clsANCVO)dgANCList.SelectedItem).EDDDate;
                    //DateLMP.SelectedDate = ((clsANCVO)dgANCList.SelectedItem).LMPDate;
                    //txtGravida.Text = ((clsANCVO)dgANCList.SelectedItem).G;
                    //txtPara.Text = ((clsANCVO)dgANCList.SelectedItem).P;
                    //txtAd.Text = ((clsANCVO)dgANCList.SelectedItem).A;
                    // GetObestricHistoryList();
                    ViewANChistory();
                    if (chkFreezeHistory.IsChecked == true)
                    {
                        AddButton.IsEnabled = false;
                        CmdModify.IsEnabled = false;
                        CmdSave.IsEnabled = false;
                    }
                    else
                    {
                        if (IsClosed == true)
                        {
                            AddButton.IsEnabled = false;
                            CmdModify.IsEnabled = false;
                            CmdSave.IsEnabled = false;
                        }
                        else
                        {
                            AddButton.IsEnabled = true;
                            CmdModify.IsEnabled = true;
                            CmdSave.IsEnabled = true;
                        }
                    }


                }

            }
            if (Investigation != null)
            {
                if (Investigation.IsSelected)
                {
                    ClearUSGUI();
                    ClearInvestigationDetailsUI();
                    IsModify = false;
                    InvAddButton.IsEnabled = true;
                    InvModifyButton.IsEnabled = false;
                    FillInvestigationDetailsGrid();
                    //USGAddButton.IsEnabled = true;
                    //USGModifyButton.IsEnabled = false;
                    FillUSGData();

                    if (chkUSGInv.IsChecked == true)
                    {
                        CmdModify.IsEnabled = false;
                        CmdSave.IsEnabled = false;
                    }
                    else
                    {
                        if (IsClosed == true)
                        {
                            CmdModify.IsEnabled = false;
                            CmdSave.IsEnabled = false;
                        }
                        else
                        {
                            CmdModify.IsEnabled = true;
                            CmdSave.IsEnabled = true;
                        }
                    }
                }
            }
            if (Examination != null)
            {
                if (Examination.IsSelected)
                {
                    fillOedema();
                    clearExaminationUI();
                    IsModify = false;
                    FillExaminationGrid();
                    AncAddButton.IsEnabled = true;
                    AncModifyButton.IsEnabled = false;
                    if (chkFreezeExamination.IsChecked == true)
                    {
                        CmdModify.IsEnabled = false;
                        CmdSave.IsEnabled = false;
                    }
                    else
                    {
                        if (IsClosed == true)
                        {
                            CmdModify.IsEnabled = false;
                            CmdSave.IsEnabled = false;
                        }
                        else
                        {
                            CmdModify.IsEnabled = true;
                            CmdSave.IsEnabled = true;
                        }
                    }


                }
            }
            if (TabDocuments != null)
            {
                if (TabDocuments.IsSelected)
                {
                    FillDocumentGrid();
                    if (chkDocument.IsChecked == true)
                    {
                        CmdModify.IsEnabled = false;
                        CmdSave.IsEnabled = false;
                    }
                    else
                    {
                        if (IsClosed == true)
                        {
                            CmdModify.IsEnabled = false;
                            CmdSave.IsEnabled = false;
                        }
                        else
                        {
                            CmdModify.IsEnabled = true;
                            CmdSave.IsEnabled = true;
                        }
                    }
                }
            }
            if (Suggestion != null)
            {
                if (Suggestion.IsSelected)
                {
                    FillConsult();
                    ClearSuggestionUI();
                    IsModify = false;
                    FillSuggestionDetails();
                    if (chkSuggestion.IsChecked == true)
                    {
                        CmdModify.IsEnabled = false;
                        CmdSave.IsEnabled = false;
                    }
                    else
                    {
                        if (IsClosed == true)
                        {
                            CmdModify.IsEnabled = false;
                            CmdSave.IsEnabled = false;
                        }
                        else
                        {
                            CmdModify.IsEnabled = true;
                            CmdSave.IsEnabled = true;
                        }
                    }

                }
            }
            wait.Close();
        }
        private void DateLMP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateLMP.SelectedDate != null)
            {
                dateEDD = DateLMP.SelectedDate.Value.Date;
                dateEDD = dateEDD.AddYears(1);
                dateEDD = dateEDD.AddMonths(-3);
                dateEDD = dateEDD.AddDays(7);
                DateEDD.SelectedDate = dateEDD;
            }
        }
        #endregion


        private void chkFreezeExamination_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hblAttach_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AnccmdBrowse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdOutComeCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkDocument_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkIsClosed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkSuggestion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgANCList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void DateYear_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {

            if (DateYear != null)
            {
                DateYear.DisplayMode = CalendarMode.Decade;

            }
        }

        long PatientUnitID = 0, AncId = 0;
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgANCList.SelectedItem != null)
            {
                PatientID = ((clsANCVO)dgANCList.SelectedItem).PatientID;
                PatientUnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
                AncId = ((clsANCVO)dgANCList.SelectedItem).ID;
                string msgTitle = "";
                string msgText = "Are You Sure \n You Want To Print ?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinPrint_OnMessageBoxClosed);

                msgWin.Show();
            }
        }
        void msgWinPrint_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                string URL = "../Reports/ANC/ANC_Report.aspx?PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&AncId=" + AncId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void chkFreezeHistory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {

        }

        #region checkBox click events
        private void chkHirsutism_Click(object sender, RoutedEventArgs e)
        {
            if (chkHirsutism.IsChecked == true)
            {
                cmbHirsutism.Visibility = Visibility.Visible;
            }
            else
            {
                cmbHirsutism.Visibility = Visibility.Collapsed;
            }
        }

        private void chkDiabetes_Click(object sender, RoutedEventArgs e)
        {
            if (chkDiabetes.IsChecked == true)
            {
                txtDiabetes.Visibility = Visibility.Visible;
            }
            else
            {
                txtDiabetes.Visibility = Visibility.Collapsed;
            }
        }

        private void chkTwins_Click(object sender, RoutedEventArgs e)
        {
            if (chkTwins.IsChecked == true)
            {
                txtTwins.Visibility = Visibility.Visible;
            }
            else
            {
                txtTwins.Visibility = Visibility.Collapsed;
            }

        }

        private void chkHypertension_Click(object sender, RoutedEventArgs e)
        {
            if (chkHypertension.IsChecked == true)
            {
                txtHypertension.Visibility = Visibility.Visible;
            }
            else
            {
                txtHypertension.Visibility = Visibility.Collapsed;
            }
        }

        private void chkAsthmaorTB_Click(object sender, RoutedEventArgs e)
        {
            if (chkAsthmaorTB.IsChecked == true)
            {
                txtAsthmaorTB.Visibility = Visibility.Visible;
            }
            else
            {
                txtAsthmaorTB.Visibility = Visibility.Collapsed;
            }
        }

        private void chkPersonalHistory_Click(object sender, RoutedEventArgs e)
        {
            if (chkPersonalHistory.IsChecked == true)
            {
                txtPersonalHistory.Visibility = Visibility.Visible;
            }
            else
            {
                txtPersonalHistory.Visibility = Visibility.Collapsed;
            }
        }
        private void chkFirstOK_click(object sender, RoutedEventArgs e)
        {
            if (chkFirstOK.IsChecked == true)
                firstDoseDate.IsEnabled = true;
            else
                firstDoseDate.IsEnabled = false;
            firstDoseDate.SelectedDate = null;
        }

        private void chkSecondOk_click(object sender, RoutedEventArgs e)
        {
            if (chkSecondOk.IsChecked == true)
                secondDoseDate.IsEnabled = true;
            else
                secondDoseDate.IsEnabled = false;
            secondDoseDate.SelectedDate = null;
        }
        #endregion

        #region GeneralDetails
        private void ClearGeneralDetailsUI()
        {
            txtA.Text = "";
            txtM.Text = "";
            txtP.Text = "";
            txtL.Text = "";
            txtG.Text = "";
            txtSpecialRemark.Text = "";
            StartDateAnc.SelectedDate = System.DateTime.Now;
            // EDD.SelectedDate = System.DateTime.Now;
            txtAgeofMenarche.Text = "";
            LMP.SelectedDate = System.DateTime.Now;
            DateofMarriage.SelectedDate = null;
            txtAncCode.Text = "Auto Generated";
        }
        private void SaveANCGeneralDetails()
        {
            clsAddANCBizActionVO bizActionObj = new clsAddANCBizActionVO();
            bizActionObj.ANCGeneralDetails = new clsANCVO();
            if (IsModify == true)
                bizActionObj.ANCGeneralDetails.ID = ((clsANCVO)dgANCList.SelectedItem).ID;
            bizActionObj.ANCGeneralDetails.CoupleID = CoupleDetails.CoupleId;
            bizActionObj.ANCGeneralDetails.CoupleUnitID = CoupleDetails.CoupleUnitId;
            bizActionObj.ANCGeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            bizActionObj.ANCGeneralDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            bizActionObj.ANCGeneralDetails.Date = StartDateAnc.SelectedDate.Value.Date;
            if (DateofMarriage.SelectedDate != null)
                bizActionObj.ANCGeneralDetails.DateofMarriage = DateofMarriage.SelectedDate.Value.Date;
            bizActionObj.ANCGeneralDetails.AgeOfMenarche = txtAgeofMenarche.Text;
            if (!String.IsNullOrEmpty(txtM.Text))
                bizActionObj.ANCGeneralDetails.M = txtM.Text;
            if (!String.IsNullOrEmpty(txtL.Text))
                bizActionObj.ANCGeneralDetails.L = txtL.Text;
            if (!String.IsNullOrEmpty(txtG.Text))
                bizActionObj.ANCGeneralDetails.G = txtG.Text;
            if (!String.IsNullOrEmpty(txtA.Text))
                bizActionObj.ANCGeneralDetails.A = txtA.Text;
            if (!String.IsNullOrEmpty(txtP.Text))
                bizActionObj.ANCGeneralDetails.P = txtP.Text;
            bizActionObj.ANCGeneralDetails.LMPDate = LMP.SelectedDate.Value.Date;
            bizActionObj.ANCGeneralDetails.EDDDate = EDD.SelectedDate.Value.Date;
            if (!String.IsNullOrEmpty(txtSpecialRemark.Text))
                bizActionObj.ANCGeneralDetails.SpecialRemark = txtSpecialRemark.Text;
            bizActionObj.ANCGeneralDetails.Isfreezed = true;
            bizActionObj.ANCGeneralDetails.Status = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddANCBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("PALASH", "ANC General Details Saved Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                        ClearGeneralDetailsUI();
                        objAnimation.Invoke(RotationType.Backward);



                        FillANCDataGird();
                        SetButtonState("New");
                        CmdSave.IsEnabled = false;
                    }
                    else if (((clsAddANCBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("PALASH", "ANC General Details Modified Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                        ClearGeneralDetailsUI();
                        objAnimation.Invoke(RotationType.Backward);

                        History.Visibility = Visibility.Visible;
                        Investigation.Visibility = Visibility.Visible;
                        Examination.Visibility = Visibility.Visible;
                        TabDocuments.Visibility = Visibility.Visible;
                        Suggestion.Visibility = Visibility.Visible;

                        FillANCDataGird();
                        CmdSave.IsEnabled = false;
                    }
                }
            };
            client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void ViewGeneralDetails()
        {
            var SelectedANCDetails = (clsANCVO)dgANCList.SelectedItem;
            GridSelectedItemID = ((clsANCVO)dgANCList.SelectedItem).ID;
            ANCId = ((clsANCVO)dgANCList.SelectedItem).ID;
            if (dgANCList.SelectedItem != null)
            {
                IsModify = true;
                if (SelectedANCDetails.A != null)
                    txtA.Text = SelectedANCDetails.A;
                if (SelectedANCDetails.G != null)
                    txtG.Text = SelectedANCDetails.G;
                if (SelectedANCDetails.M != null)
                    txtM.Text = SelectedANCDetails.M;
                if (SelectedANCDetails.P != null)
                    txtP.Text = SelectedANCDetails.P;
                if (SelectedANCDetails.L != null)
                    txtL.Text = SelectedANCDetails.L;
                if (SelectedANCDetails.SpecialRemark != null)
                    txtSpecialRemark.Text = SelectedANCDetails.SpecialRemark;
                if (SelectedANCDetails.EDDDate != null)
                    EDD.SelectedDate = SelectedANCDetails.EDDDate;
                if (SelectedANCDetails.LMPDate != null)
                    LMP.SelectedDate = SelectedANCDetails.LMPDate;
                if (SelectedANCDetails.ANC_Code != null)
                    txtAncCode.Text = SelectedANCDetails.ANC_Code;
                if (SelectedANCDetails.Date != null)
                    StartDateAnc.SelectedDate = SelectedANCDetails.Date;
                if (SelectedANCDetails.DateofMarriage != null && SelectedANCDetails.DateofMarriage.Value.Date != Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                    DateofMarriage.SelectedDate = SelectedANCDetails.DateofMarriage;
                if (SelectedANCDetails.AgeOfMenarche != null)
                    txtAgeofMenarche.Text = SelectedANCDetails.AgeOfMenarche;
                if (Convert.ToInt32(SelectedANCDetails.IsClosed) == 1)
                {
                    chkIsClosed.IsChecked = true;
                    chkIsClosed.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdNew.Visibility = Visibility.Visible;
                    SetButtonState("New");
                }
                else
                {
                    chkIsClosed.IsChecked = false;
                    CmdSave.IsEnabled = true;
                    CmdNew.Visibility = Visibility.Collapsed;
                    SetButtonState("Modify");
                }
                HistoryDetails();
            }
        }
        private void FillANCDataGird()
        {
            clsANCGetGeneralDetailsListBizActionVO BizAction = new clsANCGetGeneralDetailsListBizActionVO();
            BizAction.ANCGeneralDetailsList = new List<clsANCVO>();
            BizAction.ANCGeneralDetails.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.ANCGeneralDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.ANCGeneralDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    if (((clsANCGetGeneralDetailsListBizActionVO)args.Result).ANCGeneralDetailsList != null)
                    {
                        clsANCGetGeneralDetailsListBizActionVO result = args.Result as clsANCGetGeneralDetailsListBizActionVO;

                        DataList.TotalItemCount = result.TotalRows;
                        if (result.ANCGeneralDetailsList != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.ANCGeneralDetailsList)
                            {
                                DataList.Add(item);
                            }

                            if (DataList.Count > 0)
                            {
                                foreach (var item in DataList)
                                {
                                    if (item.IsClosed == false)
                                    {
                                        IsClosed = false;
                                    }
                                    else
                                    {
                                        IsClosed = true;
                                    }
                                }
                            }
                            dgANCList.ItemsSource = null;
                            dgANCList.ItemsSource = DataList;

                            dataGrid2PagerANC.Source = null;
                            dataGrid2PagerANC.PageSize = BizAction.MaximumRows;
                            dataGrid2PagerANC.Source = DataList;
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void UpdateGeneralDetails()
        {
            clsAddANCBizActionVO bizActionObj = new clsAddANCBizActionVO();
            bizActionObj.ANCGeneralDetails = new clsANCVO();
            bizActionObj.ANCGeneralDetails.ID = ((clsANCVO)dgANCList.SelectedItem).ID;
            bizActionObj.ANCGeneralDetails.CoupleID = CoupleDetails.CoupleId;
            bizActionObj.ANCGeneralDetails.CoupleUnitID = CoupleDetails.CoupleUnitId;
            bizActionObj.ANCGeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            bizActionObj.ANCGeneralDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            bizActionObj.ANCGeneralDetails.Date = StartDateAnc.SelectedDate.Value.Date;
            bizActionObj.ANCGeneralDetails.DateofMarriage = DateofMarriage.SelectedDate.Value.Date;
            bizActionObj.ANCGeneralDetails.AgeOfMenarche = txtAgeofMenarche.Text;
            if (!String.IsNullOrEmpty(txtM.Text))
                bizActionObj.ANCGeneralDetails.M = txtM.Text;
            if (!String.IsNullOrEmpty(txtL.Text))
                bizActionObj.ANCGeneralDetails.L = txtL.Text;
            if (!String.IsNullOrEmpty(txtGravida.Text))
                bizActionObj.ANCGeneralDetails.G = txtGravida.Text;
            if (!String.IsNullOrEmpty(txtAd.Text))
                bizActionObj.ANCGeneralDetails.A = txtAd.Text;
            if (!String.IsNullOrEmpty(txtPara.Text))
                bizActionObj.ANCGeneralDetails.P = txtPara.Text;
            bizActionObj.ANCGeneralDetails.LMPDate = DateLMP.SelectedDate.Value.Date;
            bizActionObj.ANCGeneralDetails.EDDDate = DateEDD.SelectedDate.Value.Date;
            if (!String.IsNullOrEmpty(txtSpecialRemark.Text))
                bizActionObj.ANCGeneralDetails.SpecialRemark = txtSpecialRemark.Text;
            bizActionObj.ANCGeneralDetails.Isfreezed = true;
            bizActionObj.ANCGeneralDetails.Status = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                }
            };
            client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        public bool Validation()
        {
            //if (txtAncCode.Text.Trim() == null)
            //{

            //    txtAncCode.SetValidation("Please Enter ANC Code");
            //    txtAncCode.RaiseValidationError();
            //    txtAncCode.Focus();
            //    result = false;
            //}
            //else 
            if (StartDateAnc.SelectedDate == null)
            {
                // txtAncCode.ClearValidationError();
                StartDateAnc.SetValidation("Please Select ANC Date");
                StartDateAnc.RaiseValidationError();
                StartDateAnc.Focus();
                result = false;
            }
            else if (txtM.Text.Trim() == string.Empty)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.SetValidation("Please Enter Married Since");
                txtM.RaiseValidationError();
                txtM.Focus();
                result = false;
            }
            else if (txtG.Text.Trim() == string.Empty)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.ClearValidationError();
                txtG.SetValidation("Please Enter Gravida");
                txtG.RaiseValidationError();
                txtG.Focus();
                result = false;
            }
            else if (txtP.Text.Trim() == string.Empty)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.ClearValidationError();
                txtG.ClearValidationError();
                txtP.SetValidation("Please Enter Para");
                txtP.RaiseValidationError();
                txtP.Focus();
                result = false;
            }
            else if (txtA.Text.Trim() == string.Empty)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.ClearValidationError();
                txtG.ClearValidationError();
                txtP.ClearValidationError();
                txtA.SetValidation("Please Enter Abortion");
                txtA.RaiseValidationError();
                txtA.Focus();
                result = false;
            }
            else if (txtL.Text.Trim() == string.Empty)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.ClearValidationError();
                txtG.ClearValidationError();
                txtP.ClearValidationError();
                txtA.ClearValidationError();
                txtL.SetValidation("Please Enter Live Birth");
                txtL.RaiseValidationError();
                txtL.Focus();
                result = false;
            }
            else
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.ClearValidationError();
                txtG.ClearValidationError();
                txtP.ClearValidationError();
                txtA.ClearValidationError();
                txtL.ClearValidationError();
                result = true;
            }
            return result;
        }
        #endregion

        #region History
        private void HistoryDetails()
        {
            DateEDD.SelectedDate = ((clsANCVO)dgANCList.SelectedItem).EDDDate;
            DateLMP.SelectedDate = ((clsANCVO)dgANCList.SelectedItem).LMPDate;
            txtGravida.Text = ((clsANCVO)dgANCList.SelectedItem).G;
            txtPara.Text = ((clsANCVO)dgANCList.SelectedItem).P;
            txtAd.Text = ((clsANCVO)dgANCList.SelectedItem).A;
        }
        private bool ValidateObstericHistory()
        {

            if (DateYear.DisplayDate == null)
            {
                DateYear.SetValidation("Please select year");
                DateYear.RaiseValidationError();
                DateYear.Focus();
                result = false;
            }
            else if (txtGestation.Text == " " || txtGestation.Text == string.Empty)
            {
                DateYear.ClearValidationError();
                txtGestation.SetValidation("Please Enter Gestation");
                txtGestation.RaiseValidationError();
                txtGestation.Focus();
                result = false;
            }
            else if (txtTypeofDelivery.Text.IsOnlyCharacters() == false || txtTypeofDelivery.Text.Trim() == string.Empty)
            {
                DateYear.ClearValidationError();
                txtGestation.ClearValidationError();
                txtTypeofDelivery.SetValidation("Please Enter Type of Delivery");
                txtTypeofDelivery.RaiseValidationError();
                txtTypeofDelivery.Focus();
                result = false;
            }
            else if (txtBaby.Text == " " || txtBaby.Text.IsItNumber() == false)
            {
                DateYear.ClearValidationError();
                txtGestation.ClearValidationError();
                txtTypeofDelivery.ClearValidationError();
                txtBaby.SetValidation("Please Enter Number of Baby");
                txtBaby.RaiseValidationError();
                txtBaby.Focus();
                result = false;
            }
            else
            {
                DateYear.ClearValidationError();
                txtGestation.ClearValidationError();
                txtTypeofDelivery.ClearValidationError();
                txtBaby.ClearValidationError();
                result = true;


            }

            return result;
        }
        private void clearObstericHistory()
        {

            txtGestation.Text = string.Empty;
            txtTypeofDelivery.Text = string.Empty;
            txtBaby.Text = string.Empty;
            DateYear.DisplayDate = DateTime.Now;
        }
        private void ClearHistoryUI()
        {
            chkDiabetes.IsChecked = false;
            chkTwins.IsChecked = false;
            chkHypertension.IsChecked = false;
            chkAsthmaorTB.IsChecked = false;
            chkPersonalHistory.IsChecked = false;
            chkHirsutism.IsChecked = false;
            chkFirstOK.IsChecked = false;
            chkSecondOk.IsChecked = false;
            DateLLMP.SelectedDate = null;
            //DateLMP.SelectedDate = null;
            //DateEDD.SelectedDate = null;
            txtTwins.Text = "";
            txtDiabetes.Text = "";
            txtHypertension.Text = "";
            txtAsthmaorTB.Text = "";
            txtPersonalHistory.Text = "";
            txtDrugsPresent.Text = "";
            txtDrugsPast.Text = "";
            txtDrugs.Text = "";
            txtSurgery.Text = "";
            txtAnaemia.Text = "";
            txtBPdystolic.Text = "";
            txtBPsystolic.Text = "";
            txtHistoryWeight.Text = "";
            txtLymphadenopathy.Text = "";
            txtPulse.Text = "";
            txtHeight.Text = "";
            txtBreasts.Text = "";
            txtGoitre.Text = "";
            txtCVS.Text = "";
            txtCNS.Text = "";
            txtRS.Text = "";
            txtfrequencyandtimingofintercourse.Text = "";
            txtFlurseminis.Text = "";
            txtAnyOtherfactor.Text = "";
            txtOtherimportantreleventfactor.Text = "";
            txtDuration.Text = "";
            txtMenstrualcycle.Text = "";
            txtDisorder.Text = "";
            cmbHirsutism.SelectedItem = (long)0;
            firstDoseDate.SelectedDate = null;
            secondDoseDate.SelectedDate = null;
        }
        private void cmdViewObestricHistory_Click(object sender, RoutedEventArgs e)
        {
            ViewObestricHistory();

        }
        private void ViewObestricHistory()
        {
            if ((HisIsfreezed == true))
            {
            }
            else
            {
                IsModify = true;
                ModifyButton.IsEnabled = true;
                AddButton.IsEnabled = false;
            }
            if (dgObestricHistory.SelectedItem != null)
            {
                if (((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).Gestation != null)
                    txtGestation.Text = ((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).Gestation;
                if (((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).Baby != null)
                    txtBaby.Text = Convert.ToString(((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).Baby);
                if (((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).TypeofDelivery != null)
                    txtTypeofDelivery.Text = ((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).TypeofDelivery;
                if (((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).DateYear != null)
                    DateYear.DisplayDate = ((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).DateYear.Value.Date;
            }

        }
        private void cmdDeleteObstricHistory_Click(object sender, RoutedEventArgs e)
        {
            if (dgObestricHistory.SelectedItem != null)
            {
                if (HisIsfreezed == false)
                {

                    string msgText = "Are you sure you want to Delete?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {

                        if (res == MessageBoxResult.Yes)
                        {
                            foreach (var item in obestricHistoryList)
                            {
                                if (item.ID != 0)
                                    item.IsView = true;

                            }

                            obestricHistoryList.RemoveAt(dgObestricHistory.SelectedIndex);
                            dgObestricHistory.ItemsSource = null;
                            dgObestricHistory.ItemsSource = obestricHistoryList;
                            dgObestricHistory.UpdateLayout();
                            dgObestricHistory.Focus();

                        }

                    };

                    msgWD.Show();
                }
            }
        }
        private void ViewANChistory()
        {
            clsGetANCHistoryBizActionVO bizAction = new clsGetANCHistoryBizActionVO();
            bizAction.ANCHistory = new clsANCHistoryVO();
            bizAction.ANCHistory.ANCID = ((clsANCVO)dgANCList.SelectedItem).ID;
            ANCId = ((clsANCVO)dgANCList.SelectedItem).ID;
            bizAction.ANCHistory.UnitID = ((clsANCVO)dgANCList.SelectedItem).UnitID;

            //DateEDD.SelectedDate = ((clsANCVO)dgANCList.SelectedItem).EDDDate;
            //DateLMP.SelectedDate = ((clsANCVO)dgANCList.SelectedItem).LMPDate;
            //txtGravida.Text = ((clsANCVO)dgANCList.SelectedItem).G;
            //txtPara.Text = ((clsANCVO)dgANCList.SelectedItem).P;
            //txtAd.Text = ((clsANCVO)dgANCList.SelectedItem).A;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetANCHistoryBizActionVO)arg.Result).SuccessStatus != 0)
                    {

                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCObsetricHistoryList != null)
                        {
                            if (((clsGetANCHistoryBizActionVO)arg.Result).ANCObsetricHistoryList.Count > 0)
                            {
                                obestricHistoryList.Clear();
                                foreach (var item in ((clsGetANCHistoryBizActionVO)arg.Result).ANCObsetricHistoryList)
                                {
                                    if (item.ID != 0)
                                    {
                                        item.IsView = true;
                                        obestricHistoryList.Add(item);
                                    }
                                }
                                dgObestricHistory.ItemsSource = null;
                                dgObestricHistory.ItemsSource = obestricHistoryList;
                            }

                        }

                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.ID != null)
                        {
                            HisID = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.ID;
                        }
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.UnitID != null)
                        {
                            HisUnitID = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.UnitID;
                        }


                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Hypertension == true)
                        {
                            chkHypertension.IsChecked = true;
                            txtHypertension.Visibility = Visibility.Visible;
                            txtHypertension.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.txtHypertension;

                        }
                        else
                        {
                            chkHypertension.IsChecked = false;
                            txtHypertension.Visibility = Visibility.Collapsed;
                        }

                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.TB == true)
                        {
                            chkAsthmaorTB.IsChecked = true;
                            txtAsthmaorTB.Visibility = Visibility.Visible;
                            txtAsthmaorTB.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.txtTB;

                        }
                        else
                        {
                            chkAsthmaorTB.IsChecked = false;
                            txtAsthmaorTB.Visibility = Visibility.Collapsed;
                        }

                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Diabeties == true)
                        {
                            chkDiabetes.IsChecked = true;
                            txtDiabetes.Visibility = Visibility.Visible;
                            txtDiabetes.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.txtDiabeties;

                        }
                        else
                        {
                            chkDiabetes.IsChecked = false;
                            txtDiabetes.Visibility = Visibility.Collapsed;
                        }

                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Twins == true)
                        {
                            chkTwins.IsChecked = true;
                            txtTwins.Visibility = Visibility.Visible;
                            txtTwins.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.txtTwins;

                        }
                        else
                        {
                            chkTwins.IsChecked = false;
                            txtTwins.Visibility = Visibility.Collapsed;
                        }
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.PersonalHistory == true)
                        {
                            chkPersonalHistory.IsChecked = true;
                            txtPersonalHistory.Visibility = Visibility.Visible;
                            txtPersonalHistory.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.txtPersonalHistory;

                        }
                        else
                        {
                            chkPersonalHistory.IsChecked = false;
                            txtPersonalHistory.Visibility = Visibility.Collapsed;
                        }
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Menstrualcycle != null)
                            txtMenstrualcycle.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Menstrualcycle;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Duration != null)
                            txtDuration.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Duration;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Disorder != null)
                            txtDisorder.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Disorder;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.LLMPDate != null && ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.LLMPDate.Value.Date != Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                            DateLLMP.SelectedDate = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.LLMPDate;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.DrugsPast != null)
                            txtDrugsPast.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.DrugsPast;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.DrugsPresent != null)
                            txtDrugsPresent.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.DrugsPresent;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Drugs != null)
                            txtDrugs.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Drugs;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Surgery != null)
                            txtSurgery.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Surgery;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Lymphadenopathy != null)
                            txtLymphadenopathy.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Lymphadenopathy;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Weight != null)
                            txtHistoryWeight.Text = Convert.ToString(((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Weight);
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Height != null)
                            txtHeight.Text = Convert.ToString(((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Height);
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Breasts != null)
                            txtBreasts.Text = Convert.ToString(((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Breasts);
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Pulse != null)
                            txtPulse.Text = Convert.ToString(((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Pulse);
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.BpInHg != null)
                            txtBPdystolic.Text = Convert.ToString(((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.BpInHg);
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.BpInMm != null)
                            txtBPsystolic.Text = Convert.ToString(((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.BpInMm);
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Anaemia != null)
                            txtAnaemia.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Anaemia;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Edema != null)
                            txtEdema.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Edema;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Goitre != null)
                            txtGoitre.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Goitre;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.CVS != null)
                            txtCVS.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.CVS;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.CNS != null)
                            txtCNS.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.CNS;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.RS != null)
                            txtRS.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.RS;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.frequencyandtimingofintercourse != null)
                            txtfrequencyandtimingofintercourse.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.frequencyandtimingofintercourse;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Flurseminis != null)
                            txtFlurseminis.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Flurseminis;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.AnyOtherfactor != null)
                            txtAnyOtherfactor.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.AnyOtherfactor;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Otherimportantreleventfactor != null)
                            txtOtherimportantreleventfactor.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Otherimportantreleventfactor;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.CNS != null)
                            txtCNS.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.CNS;

                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Hirsutism == true)
                        {
                            chkHirsutism.IsChecked = true;
                            cmbHirsutism.Visibility = Visibility.Visible;
                            cmbHirsutism.SelectedValue = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.HirsutismID;
                        }
                        else
                        {
                            chkHirsutism.IsChecked = false;
                            cmbHirsutism.Visibility = Visibility.Collapsed;

                        }

                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.TTIstDose == true)
                        {
                            chkFirstOK.IsChecked = true;
                            firstDoseDate.SelectedDate = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.DateIstDose;
                            firstDoseDate.IsEnabled = true;
                        }
                        else
                        {
                            chkFirstOK.IsChecked = false;
                            firstDoseDate.IsEnabled = false;
                        }
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.TTIIstDose == true)
                        {
                            chkSecondOk.IsChecked = true;
                            secondDoseDate.SelectedDate = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.DateIIstDose;
                            secondDoseDate.IsEnabled = true;
                        }
                        else
                        {
                            chkSecondOk.IsChecked = false;
                            secondDoseDate.IsEnabled = false;
                        }
                        IsModify = true;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Isfreezed != null)
                        {
                            if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Isfreezed == true)
                            {
                                HisIsfreezed = true;
                                chkFreezeHistory.IsChecked = true;
                                AddButton.IsEnabled = false;
                                ModifyButton.IsEnabled = false;
                                CmdModify.IsEnabled = false;
                                CmdSave.IsEnabled = false;

                            }
                            else
                            {
                                if (IsClosed == true)
                                {
                                    HisIsfreezed = true;
                                    AddButton.IsEnabled = false;
                                    ModifyButton.IsEnabled = false;
                                    chkFreezeHistory.IsChecked = false;
                                    CmdModify.IsEnabled = false;
                                    CmdSave.IsEnabled = false;
                                }
                                else
                                {
                                    HisIsfreezed = false;
                                    AddButton.IsEnabled = true;
                                    ModifyButton.IsEnabled = false;
                                    chkFreezeHistory.IsChecked = false;
                                    CmdModify.IsEnabled = true;
                                    CmdSave.IsEnabled = true;
                                }

                            }
                        }
                    }
                }
                //if (((clsGetANCHistoryBizActionVO)arg.Result).SuccessStatus == 0)
                //    SetButtonState("New");
            };
            client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void AddUpdateANCHistory()
        {
            clsAddUpdateANCHistoryBizActionVO bizActionObj = new clsAddUpdateANCHistoryBizActionVO();




            bizActionObj.ANCHistory = new clsANCHistoryVO();
            bizActionObj.ANCObsetricHistoryList = new List<clsANCObstetricHistoryVO>();
            if (IsModify == true)
            {
                bizActionObj.ANCHistory.ID = HisID;
                bizActionObj.ANCHistory.UnitID = HisUnitID;
            }
            else
            {
                bizActionObj.ANCHistory.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            bizActionObj.ANCHistory.ANCID = ((clsANCVO)dgANCList.SelectedItem).ID;
            bizActionObj.ANCHistory.PatientID = ((clsANCVO)dgANCList.SelectedItem).PatientID;
            bizActionObj.ANCHistory.PatientUnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            bizActionObj.ANCHistory.CoupleID = CoupleDetails.CoupleId;
            bizActionObj.ANCHistory.CoupleUnitID = CoupleDetails.CoupleUnitId;
            //bizActionObj.ANCHistory.LMPDate = DateLMP.SelectedDate.Value.Date;
            //bizActionObj.ANCHistory.EDDDate = DateEDD.SelectedDate.Value.Date;
            if (DateLLMP.SelectedDate != null)
                bizActionObj.ANCHistory.LLMPDate = DateLLMP.SelectedDate.Value.Date;
            if (chkDiabetes.IsChecked == true)
            {
                bizActionObj.ANCHistory.Diabeties = true;
                bizActionObj.ANCHistory.txtDiabeties = txtDiabetes.Text;
            }
            else
                bizActionObj.ANCHistory.Diabeties = false;

            if (chkTwins.IsChecked == true)
            {
                bizActionObj.ANCHistory.Twins = true;
                bizActionObj.ANCHistory.txtTwins = txtTwins.Text;
            }
            else
                bizActionObj.ANCHistory.Twins = false;
            if (chkHypertension.IsChecked == true)
            {
                bizActionObj.ANCHistory.Hypertension = true;
                bizActionObj.ANCHistory.txtHypertension = txtHypertension.Text;
            }
            else
                bizActionObj.ANCHistory.Hypertension = false;
            if (chkAsthmaorTB.IsChecked == true)
            {
                bizActionObj.ANCHistory.TB = true;
                bizActionObj.ANCHistory.txtTB = txtAsthmaorTB.Text;
            }
            else
                bizActionObj.ANCHistory.TB = false;
            if (chkPersonalHistory.IsChecked == true)
            {
                bizActionObj.ANCHistory.PersonalHistory = true;
                bizActionObj.ANCHistory.txtPersonalHistory = txtPersonalHistory.Text;
            }
            else
                bizActionObj.ANCHistory.PersonalHistory = false;

            if (!String.IsNullOrEmpty(txtDrugsPresent.Text))
                bizActionObj.ANCHistory.DrugsPresent = txtDrugsPresent.Text;
            if (!String.IsNullOrEmpty(txtDrugsPast.Text))
                bizActionObj.ANCHistory.DrugsPast = txtDrugsPast.Text;
            if (!String.IsNullOrEmpty(txtDrugs.Text))
                bizActionObj.ANCHistory.Drugs = txtDrugs.Text;
            if (!String.IsNullOrEmpty(txtSurgery.Text))
                bizActionObj.ANCHistory.Surgery = txtSurgery.Text;

            bizActionObj.ANCHistory.Anaemia = txtAnaemia.Text;
            bizActionObj.ANCHistory.BpInHg = Convert.ToSingle(txtBPdystolic.Text);
            bizActionObj.ANCHistory.BpInMm = Convert.ToSingle(txtBPsystolic.Text);
            bizActionObj.ANCHistory.Weight = Convert.ToSingle(txtHistoryWeight.Text);
            bizActionObj.ANCHistory.Lymphadenopathy = txtLymphadenopathy.Text;
            bizActionObj.ANCHistory.Pulse = Convert.ToSingle(txtPulse.Text);
            bizActionObj.ANCHistory.Height = Convert.ToSingle(txtHeight.Text);
            bizActionObj.ANCHistory.Breasts = txtBreasts.Text;
            if (chkHirsutism.IsChecked == true)
            {
                bizActionObj.ANCHistory.Hirsutism = true;
                if ((MasterListItem)cmbHirsutism.SelectedItem != null)
                    bizActionObj.ANCHistory.HirsutismID = ((MasterListItem)cmbHirsutism.SelectedItem).ID;
            }
            else
                bizActionObj.ANCHistory.Hirsutism = false;
            bizActionObj.ANCHistory.Goitre = txtGoitre.Text;
            bizActionObj.ANCHistory.CVS = txtCVS.Text;
            bizActionObj.ANCHistory.CNS = txtCNS.Text;
            bizActionObj.ANCHistory.RS = txtRS.Text;
            bizActionObj.ANCHistory.frequencyandtimingofintercourse = txtfrequencyandtimingofintercourse.Text;
            bizActionObj.ANCHistory.Flurseminis = txtFlurseminis.Text;
            bizActionObj.ANCHistory.AnyOtherfactor = txtAnyOtherfactor.Text;
            bizActionObj.ANCHistory.Otherimportantreleventfactor = txtOtherimportantreleventfactor.Text;
            bizActionObj.ANCHistory.Menstrualcycle = txtMenstrualcycle.Text;
            bizActionObj.ANCHistory.Duration = txtDuration.Text;
            bizActionObj.ANCHistory.Disorder = txtDisorder.Text;
            bizActionObj.ANCHistory.Edema = txtEdema.Text;
            if (chkFirstOK.IsChecked == true)
                bizActionObj.ANCHistory.TTIstDose = true;
            else
                bizActionObj.ANCHistory.TTIstDose = false;
            if (chkSecondOk.IsChecked == true)
                bizActionObj.ANCHistory.TTIIstDose = true;
            else
                bizActionObj.ANCHistory.TTIIstDose = false;
            if (chkFirstOK.IsChecked == true && firstDoseDate.SelectedDate != null)
                bizActionObj.ANCHistory.DateIstDose = firstDoseDate.SelectedDate.Value.Date;
            if (chkSecondOk.IsChecked == true && secondDoseDate.SelectedDate != null)
                bizActionObj.ANCHistory.DateIIstDose = secondDoseDate.SelectedDate.Value.Date;
            if (chkFreezeHistory.IsChecked == true)
                bizActionObj.ANCHistory.Isfreezed = true;
            bizActionObj.ANCHistory.Status = true;

            foreach (var item in obestricHistoryList)
            {
                bizActionObj.ANCObsetricHistoryList.Add(item);
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddUpdateANCHistoryBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        if (((clsANCVO)dgANCList.SelectedItem).EDDDate.Value.Date != DateEDD.SelectedDate.Value.Date || ((clsANCVO)dgANCList.SelectedItem).LMPDate.Value.Date != DateLMP.SelectedDate.Value.Date || ((clsANCVO)dgANCList.SelectedItem).G != txtGravida.Text || ((clsANCVO)dgANCList.SelectedItem).P != txtPara.Text || ((clsANCVO)dgANCList.SelectedItem).A != txtAd.Text)
                        {
                            UpdateGeneralDetails();
                        }
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("PALASH", "ANC History Saved Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                        ClearHistoryUI();
                        // objAnimation.Invoke(RotationType.Backward);
                        ViewANChistory();
                        CmdSave.IsEnabled = false;
                    }
                    else if (((clsAddUpdateANCHistoryBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        if (((clsANCVO)dgANCList.SelectedItem).EDDDate.Value.Date != DateEDD.SelectedDate.Value.Date || ((clsANCVO)dgANCList.SelectedItem).LMPDate.Value.Date != DateLMP.SelectedDate.Value.Date || ((clsANCVO)dgANCList.SelectedItem).G != txtGravida.Text || ((clsANCVO)dgANCList.SelectedItem).P != txtPara.Text || ((clsANCVO)dgANCList.SelectedItem).A != txtAd.Text)
                        {
                            UpdateGeneralDetails();
                        }
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("PALASH", "ANC History Modified Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                        ClearHistoryUI();
                        //  objAnimation.Invoke(RotationType.Backward);
                        ViewANChistory();
                        CmdSave.IsEnabled = false;
                    }
                }
            };
            client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateObstericHistory())
            {
                clsANCObstetricHistoryVO Hist = new clsANCObstetricHistoryVO();
                if (DateYear.DisplayDate != null)
                    Hist.DateYear = DateYear.DisplayDate;
                if (!String.IsNullOrEmpty(txtGestation.Text))
                    Hist.Gestation = txtGestation.Text;

                if (!String.IsNullOrEmpty(txtTypeofDelivery.Text))
                    Hist.TypeofDelivery = txtTypeofDelivery.Text;
                if (!String.IsNullOrEmpty(txtBaby.Text))
                    Hist.Baby = Convert.ToInt64(txtBaby.Text);
                Hist.Status = true;
                Hist.IsView = false;

                obestricHistoryList.Add(Hist);
                dgObestricHistory.ItemsSource = null;
                dgObestricHistory.ItemsSource = obestricHistoryList;
                dgObestricHistory.UpdateLayout();
                dgObestricHistory.Focus();
                clearObstericHistory();
            }

        }
        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateObstericHistory())
            {
                if (obestricHistoryList.Count > 0)
                {
                    if (((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem) != null)
                    {
                        foreach (var item in obestricHistoryList)
                        {
                            if (item.ID == (((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).ID))
                            {
                                item.DateYear = DateYear.DisplayDate;
                                item.Gestation = txtGestation.Text;
                                item.Baby = Convert.ToInt64(txtBaby.Text);
                                item.TypeofDelivery = txtTypeofDelivery.Text;
                            }
                        }
                    }
                    dgObestricHistory.ItemsSource = null;
                    dgObestricHistory.ItemsSource = obestricHistoryList;
                    dgObestricHistory.Focus();
                    dgObestricHistory.UpdateLayout();
                    dgObestricHistory.SelectedIndex = obestricHistoryList.Count - 1;
                    clearObstericHistory();
                }

            }
        }
        public bool HistoryValidations()
        {
            if (txtHistoryWeight.Text.IsValueDouble() == false || txtHistoryWeight.Text.Trim() == string.Empty)
            {
                txtHistoryWeight.SetValidation("Please Enter Number");
                txtHistoryWeight.RaiseValidationError();
                txtHistoryWeight.Focus();
                result = false;

            }
            else if (txtHeight.Text.IsValueDouble() == false || txtHeight.Text.Trim() == string.Empty)
            {
                txtHistoryWeight.ClearValidationError();
                txtHeight.SetValidation("Please Enter Number");
                txtHeight.RaiseValidationError();
                txtHeight.Focus();
                result = false;
            }
            else if (txtBreasts.Text.IsValueDouble() == false || txtBreasts.Text.Trim() == string.Empty)
            {
                txtHistoryWeight.ClearValidationError();
                txtHeight.ClearValidationError();
                txtBreasts.SetValidation("Please Enter Number");
                txtBreasts.RaiseValidationError();
                txtBreasts.Focus();
                result = false;
            }
            else if (txtBPdystolic.Text.IsValueDouble() == false || txtBPdystolic.Text.Trim() == string.Empty)
            {
                txtHistoryWeight.ClearValidationError();
                txtHeight.ClearValidationError();
                txtBreasts.ClearValidationError();
                txtBPdystolic.SetValidation("Please Enter Number");
                txtBPdystolic.RaiseValidationError();
                txtBPdystolic.Focus();
                result = false;
            }
            else if (txtBPsystolic.Text.IsValueDouble() == false || txtBPsystolic.Text.Trim() == string.Empty)
            {
                txtHistoryWeight.ClearValidationError();
                txtHeight.ClearValidationError();
                txtBreasts.ClearValidationError();
                txtBPdystolic.ClearValidationError();
                txtBPsystolic.SetValidation("Please Enter Number");
                txtBPsystolic.RaiseValidationError();
                txtBPsystolic.Focus();
                result = false;
            }
            else if (txtPulse.Text.IsValueDouble() == false || txtBPsystolic.Text.Trim() == string.Empty)
            {
                txtHistoryWeight.ClearValidationError();
                txtHeight.ClearValidationError();
                txtBreasts.ClearValidationError();
                txtBPdystolic.ClearValidationError();
                txtBPsystolic.ClearValidationError();
                txtPulse.SetValidation("Please Enter Number");
                txtPulse.RaiseValidationError();
                txtPulse.Focus();
                result = false;
            }
            else
            {
                txtHistoryWeight.ClearValidationError();
                txtHeight.ClearValidationError();
                txtBreasts.ClearValidationError();
                txtBPdystolic.ClearValidationError();
                txtBPsystolic.ClearValidationError();
                txtPulse.ClearValidationError();
                result = true;
            }
            return result;

        }
        #endregion

        #region Investigation
        private void AddUpdateInvestigationDetails()
        {
            clsAddUpdateANCInvestigationDetailsBizActionVO bizActionObj = new clsAddUpdateANCInvestigationDetailsBizActionVO();
            bizActionObj.ANCInvestigationDetails = new clsANCInvestigationDetailsVO();
            bizActionObj.ANCInvestigationDetailsList = new List<clsANCInvestigationDetailsVO>();
            bizActionObj.ANCInvestigationDetails.ANCID = ANCId;
            bizActionObj.ANCInvestigationDetails.PatientID = ((clsANCVO)dgANCList.SelectedItem).PatientID;
            bizActionObj.ANCInvestigationDetails.PatientUnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            bizActionObj.ANCInvestigationDetails.CoupleID = CoupleDetails.CoupleId;
            bizActionObj.ANCInvestigationDetails.CoupleUnitID = CoupleDetails.CoupleUnitId;
            foreach (var item in investigationList)
            {
                bizActionObj.ANCInvestigationDetailsList.Add(item);
            }
            bizActionObj.ANCInvestigationDetails.Isfreezed = true;
            bizActionObj.ANCInvestigationDetails.Status = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddUpdateANCInvestigationDetailsBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        //string msgTitle = "Palash";
                        //string msgText = "Investigation Details Saved Successfully.";
                        //MessageBoxControl.MessageBoxChildWindow msgWin =
                        //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        //msgWin.OnMessageBoxClosed += (result) =>
                        //{
                        //    if (result == MessageBoxResult.OK)
                        //    {
                        FillInvestigationDetailsGrid();
                        //    }
                        //};
                        // msgWin.Show();
                        ClearInvestigationDetailsUI();
                        SetButtonState("New");
                    }
                    else if (((clsAddUpdateANCInvestigationDetailsBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        //string msgTitle = "Palash";
                        //string msgText = "Investigation Details Modified Successfully.";
                        //MessageBoxControl.MessageBoxChildWindow msgWin =
                        //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        //msgWin.OnMessageBoxClosed += (result) =>
                        //{
                        //    if (result == MessageBoxResult.OK)
                        //    {
                        FillInvestigationDetailsGrid();
                        //    }
                        //};
                        //msgWin.Show();
                        ClearInvestigationDetailsUI();

                    }
                }
            };
            client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillInvestigationDetailsGrid()
        {
            clsGetANCInvestigationListBizActionVO BizAction = new clsGetANCInvestigationListBizActionVO();
            BizAction.ANCInvestigationDetailsList = new List<clsANCInvestigationDetailsVO>();
            BizAction.ANCInvestigationDetails.UnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            BizAction.ANCInvestigationDetails.ANCID = ANCId;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = investigationList.PageIndex * investigationList.PageSize;
            BizAction.MaximumRows = investigationList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetANCInvestigationListBizActionVO)args.Result).ANCInvestigationDetailsList != null)
                    {
                        clsGetANCInvestigationListBizActionVO result = args.Result as clsGetANCInvestigationListBizActionVO;
                        if (result.ANCInvestigationDetailsList != null)
                        {
                            investigationList.Clear();
                            foreach (var item in result.ANCInvestigationDetailsList)
                            {
                                item.IsView = true;
                                investigationList.Add(item);
                            }
                            dgInvestigation.ItemsSource = null;
                            dgInvestigation.ItemsSource = investigationList;

                            dataGridPagerInvestigation.Source = null;
                            dataGridPagerInvestigation.PageSize = BizAction.MaximumRows;
                            dataGridPagerInvestigation.Source = investigationList;
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void InvAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (InvestigationValidations())
            {
                clsANCInvestigationDetailsVO INV = new clsANCInvestigationDetailsVO();
                if (((MasterListItem)cmbInvestigation.SelectedItem).ID != null)
                {
                    INV.InvestigationID = ((MasterListItem)cmbInvestigation.SelectedItem).ID;
                    INV.Investigation = ((MasterListItem)cmbInvestigation.SelectedItem).Description;
                }
                if (!String.IsNullOrEmpty(txtReport.Text))
                    INV.Report = txtReport.Text;
                INV.InvDate = (DateTime)InvDate.SelectedDate;
                INV.Status = true;
                INV.IsView = false;
                investigationList.Add(INV);
                dgInvestigation.ItemsSource = null;
                dgInvestigation.ItemsSource = investigationList;
                dgInvestigation.UpdateLayout();
                dgInvestigation.Focus();
                ClearInvestigationDetailsUI();
            }
        }
        private void ClearInvestigationDetailsUI()
        {
            txtReport.Text = "";
            InvDate.SelectedDate = null;
            cmbInvestigation.SelectedValue = (long)0;

        }
        private bool InvestigationValidations()
        {
            if (InvDate.SelectedDate == null)
            {
                InvDate.SetValidation("Please Select Investigation Date");
                InvDate.RaiseValidationError();
                InvDate.Focus();
                return false;
            }
            else if ((MasterListItem)cmbInvestigation.SelectedItem == null || ((MasterListItem)cmbInvestigation.SelectedItem).ID == 0)
            {
                InvDate.ClearValidationError();
                cmbInvestigation.TextBox.SetValidation("Please Select Investigation");
                cmbInvestigation.TextBox.RaiseValidationError();
                cmbInvestigation.Focus();
                return false;
            }

            else
            {
                InvDate.ClearValidationError();
                cmbInvestigation.TextBox.ClearValidationError();

                return true;
            }
        }
        private void InvModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (InvestigationValidations())
            {
                if (investigationList.Count > 0)
                {
                    if (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem) != null)
                    {
                        foreach (var item in investigationList)
                        {
                            if (item.ID == (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).ID))
                            {
                                //clsANCInvestigationDetailsVO Examination = new clsANCInvestigationDetailsVO();
                                if (((MasterListItem)cmbInvestigation.SelectedItem).ID != null)
                                {
                                    item.InvestigationID = ((MasterListItem)cmbInvestigation.SelectedItem).ID;
                                    item.Investigation = ((MasterListItem)cmbInvestigation.SelectedItem).Description;
                                }
                                if (!String.IsNullOrEmpty(txtReport.Text))
                                    item.Report = txtReport.Text;
                                item.InvDate = (DateTime)InvDate.SelectedDate;

                            }
                        }
                    }
                    dgInvestigation.ItemsSource = null;
                    dgInvestigation.ItemsSource = investigationList;
                    dgInvestigation.Focus();
                    dgInvestigation.UpdateLayout();
                    dgInvestigation.SelectedIndex = investigationList.Count - 1;
                    ClearInvestigationDetailsUI();
                }

            }
        }

        private void cmdViewInvestigation_Click(object sender, RoutedEventArgs e)
        {
            ViewInvestigation();
        }
        private void ViewInvestigation()
        {
            if (dgInvestigation.SelectedItem != null)
            {
                if (HisIsfreezed == true)
                { }
                else
                {
                    IsModify = true;
                    InvModifyButton.IsEnabled = true;
                    InvAddButton.IsEnabled = false;
                }
                if (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).Report != null)
                    txtReport.Text = ((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).Report;
                if (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).InvDate != null)
                    InvDate.SelectedDate = ((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).InvDate;
                if (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).InvestigationID != null)
                    cmbInvestigation.SelectedValue = ((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).InvestigationID;

            }
        }
        private void cmdDeleteInvestigation_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvestigation.SelectedItem != null)
            {
                if (HisIsfreezed == false)
                {
                    string msgText = "Are you sure you want to Delete?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {

                        if (res == MessageBoxResult.Yes)
                        {
                            foreach (var item in investigationList)
                            {
                                if (item.ID != 0)
                                    item.IsView = true;

                            }

                            investigationList.RemoveAt(dgInvestigation.SelectedIndex);
                            dgInvestigation.ItemsSource = null;
                            dgInvestigation.ItemsSource = investigationList;
                            dgInvestigation.UpdateLayout();
                            dgInvestigation.Focus();

                        }

                    };

                    msgWD.Show();
                }
            }
        }

        private void FillUSGData()
        {
            clsGetANCUSGListBizActionVO BizAction = new clsGetANCUSGListBizActionVO();
            BizAction.ANCUSGDetailsList = new List<clsANCUSGDetailsVO>();
            BizAction.ANCUSGDetails.UnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            BizAction.ANCUSGDetails.ANCID = ANCId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetANCUSGListBizActionVO)args.Result).ANCUSGDetailsList != null)
                    {
                        clsGetANCUSGListBizActionVO result = args.Result as clsGetANCUSGListBizActionVO;
                        if (result.ANCUSGDetails != null)
                        {
                            ClearUSGUI();
                            if (result.ANCUSGDetails.ID != null)
                                USGID = result.ANCUSGDetails.ID;
                            if (result.ANCUSGDetails.UnitID != null)
                                USGUnitID = result.ANCUSGDetails.UnitID;
                            if (result.ANCUSGDetails.txtSIFT != null)
                                txtSIFT.Text = result.ANCUSGDetails.txtSIFT;
                            if (result.ANCUSGDetails.txtGIFT != null)
                                txtGIFT.Text = result.ANCUSGDetails.txtGIFT;
                            if (result.ANCUSGDetails.txtIVF != null)
                                txtIVF.Text = result.ANCUSGDetails.txtIVF;
                            if (result.ANCUSGDetails.SIFT != null && result.ANCUSGDetails.SIFT.Value.Date != Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                                SIFTDate.SelectedDate = result.ANCUSGDetails.SIFT;
                            else
                                SIFTDate.SelectedDate = null;
                            if (result.ANCUSGDetails.GIFT != null && result.ANCUSGDetails.GIFT.Value.Date != Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                                GIFTDate.SelectedDate = result.ANCUSGDetails.GIFT;
                            else
                                GIFTDate.SelectedDate = null;
                            if (result.ANCUSGDetails.IVF != null && result.ANCUSGDetails.IVF.Value.Date != Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                                IVFDate.SelectedDate = result.ANCUSGDetails.IVF;
                            else
                                IVFDate.SelectedDate = null;
                            if (result.ANCUSGDetails.Sonography != null)
                                txtSonography.Text = result.ANCUSGDetails.Sonography;
                            if (result.ANCUSGDetails.Mysteroscopy != null)
                                txtMysteroscopy.Text = result.ANCUSGDetails.Mysteroscopy;
                            if (result.ANCUSGDetails.INVTreatment != null)
                                txtINVTreatment.Text = result.ANCUSGDetails.INVTreatment;
                            if (result.ANCUSGDetails.Laparoscopy != null)
                                txtLaparoscopy.Text = result.ANCUSGDetails.Laparoscopy;
                            if (result.ANCUSGDetails.OvulationMonitors != null)
                                txtOvulationMonitors.Text = result.ANCUSGDetails.OvulationMonitors;
                            IsModify = true;
                            if (result.ANCUSGDetails.ID != null)
                            {
                                if (result.ANCUSGDetails.Isfreezed == true)
                                {
                                    HisIsfreezed = true;
                                    chkUSGInv.IsChecked = true;
                                    SetButtonState("New");
                                    chkUSGInv.IsEnabled = false;
                                    InvModifyButton.IsEnabled = false;
                                    InvAddButton.IsEnabled = false;
                                    CmdSave.IsEnabled = false;
                                    CmdModify.IsEnabled = false;

                                }
                                else
                                {
                                    if (IsClosed == true)
                                    {
                                        HisIsfreezed = true;
                                        CmdModify.IsEnabled = false;
                                        CmdSave.IsEnabled = false;
                                    }
                                    else
                                    {
                                        HisIsfreezed = false;
                                        CmdModify.IsEnabled = true;
                                        CmdSave.IsEnabled = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void AddUpdateUSGdetails()
        {
            clsAddUpdateUSGdetailsBizActionVO bizActionObj = new clsAddUpdateUSGdetailsBizActionVO();
            bizActionObj.ANCUSGDetails = new clsANCUSGDetailsVO();
            if (IsModify == true)
            {
                bizActionObj.ANCUSGDetails.ID = USGID;
                bizActionObj.ANCUSGDetails.UnitID = USGUnitID;
            }
            else
            {
                bizActionObj.ANCUSGDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            bizActionObj.ANCUSGDetails.ANCID = ((clsANCVO)dgANCList.SelectedItem).ID;
            bizActionObj.ANCUSGDetails.CoupleID = CoupleDetails.CoupleId;
            bizActionObj.ANCUSGDetails.CoupleUnitID = CoupleDetails.CoupleUnitId;
            bizActionObj.ANCUSGDetails.PatientID = ((clsANCVO)dgANCList.SelectedItem).PatientID;
            bizActionObj.ANCUSGDetails.PatientUnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            bizActionObj.ANCUSGDetails.INVTreatment = txtINVTreatment.Text;
            bizActionObj.ANCUSGDetails.Laparoscopy = txtINVTreatment.Text;
            bizActionObj.ANCUSGDetails.Mysteroscopy = txtMysteroscopy.Text;
            bizActionObj.ANCUSGDetails.OvulationMonitors = txtOvulationMonitors.Text;
            bizActionObj.ANCUSGDetails.Sonography = txtSonography.Text;
            if (SIFTDate.SelectedDate != null)
                bizActionObj.ANCUSGDetails.SIFT = SIFTDate.SelectedDate.Value.Date;
            if (GIFTDate.SelectedDate != null)
                bizActionObj.ANCUSGDetails.GIFT = GIFTDate.SelectedDate.Value.Date;
            if (IVFDate.SelectedDate != null)
                bizActionObj.ANCUSGDetails.IVF = IVFDate.SelectedDate.Value.Date;
            bizActionObj.ANCUSGDetails.txtSIFT = txtSIFT.Text;
            bizActionObj.ANCUSGDetails.txtGIFT = txtGIFT.Text;
            bizActionObj.ANCUSGDetails.txtIVF = txtIVF.Text;

            if (chkUSGInv.IsChecked == true)
                bizActionObj.ANCUSGDetails.Isfreezed = true;
            else
                bizActionObj.ANCUSGDetails.Isfreezed = false;
            bizActionObj.ANCUSGDetails.Status = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddUpdateUSGdetailsBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Investigation Details Saved Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillUSGData();
                            }
                        };
                        msgWin.Show();
                        ClearUSGUI();

                    }
                    else if (((clsAddUpdateUSGdetailsBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Investigation Details Modified Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillUSGData();
                            }
                        };
                        msgWin.Show();
                        ClearUSGUI();

                    }
                }
            };
            client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void ClearUSGUI()
        {
            txtSIFT.Text = "";
            SIFTDate.SelectedDate = null;
            GIFTDate.SelectedDate = null;
            IVFDate.SelectedDate = null;
            txtGIFT.Text = "";
            txtIVF.Text = "";
            txtSonography.Text = "";
            txtMysteroscopy.Text = "";
            txtINVTreatment.Text = "";
            txtLaparoscopy.Text = "";
            txtOvulationMonitors.Text = "";

        }
        #endregion

        #region Examination
        private void ViewANCExamination()
        {
            if (grdResultEntry.SelectedItem != null)
            {
                if (HisIsfreezed == true)
                { }
                else
                {
                    IsModify = true;
                    AncModifyButton.IsEnabled = true;
                    AncAddButton.IsEnabled = false;
                }
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).BP != null)
                    txtBP.Text = Convert.ToString(((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).BP);
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Weight != null)
                    txtWeight.Text = Convert.ToString(((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Weight);
                //if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).FundalHeight != null)
                //    txtFHeight.Text = Convert.ToString(((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).FundalHeight);
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Remark != null)
                    txtRemarks.Text = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Remark;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Doctor != null)
                    cmbDoctor.SelectedValue = (long)((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Doctor;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).PresenAndPos != null)
                    cmbPreAndPosition.SelectedValue = (long)((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).PresenAndPos;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).FHS != null)
                    cmbFHS.SelectedValue = (long)((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).FHS;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).ExaDate != null)
                    AncDate.SelectedDate = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).ExaDate;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).ExaTime != null)
                    ProcTime.Value = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).ExaTime;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).OademaID != null)
                    cmbOedema.SelectedValue = (long)((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).OademaID;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).AbdGirth != null)
                    txtAbdGirth.Text = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).AbdGirth;
                //if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Oadema3 != null)
                //    txtOedema3.Text = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Oadema3;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).PeriodOfPreg != null)
                    txtPeriodofGestation.Text = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).PeriodOfPreg;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).RelationtoBrim != null)
                    txtRelationtoBrim.Text = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).RelationtoBrim;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).HTofUterus != null)
                    txtHTofUterus.Text = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).HTofUterus;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Treatment != null)
                    txtTreatment.Text = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Treatment;
            }
        }
        private void clearExaminationUI()
        {
            AncDate.SelectedDate = null;
            ProcTime.Value = null;
            txtBP.Text = "";
            txtHeight.Text = "";
            txtWeight.Text = "";
            //txtOedema.Text = "";
            //txtOedema2.Text = "";
            //txtOedema3.Text = "";
            cmbOedema.SelectedValue = (long)0;
            cmbDoctor.SelectedValue = (long)0;
            txtPeriodofGestation.Text = "";
            txtHTofUterus.Text = "";
            cmbFHS.SelectedValue = (long)0;
            cmbPreAndPosition.SelectedValue = (long)0;
            ProcTime.Value = null;
            txtRemarks.Text = "";
            txtRelationtoBrim.Text = "";
            txtAbdGirth.Text = "";
            txtTreatment.Text = "";

        }
        private bool ExaminationValidations()
        {
            if (AncDate.SelectedDate == null)
            {
                AncDate.SetValidation("Please Enter Date");
                AncDate.RaiseValidationError();
                AncDate.Focus();
                return false;
            }
            else if (ProcTime.Value == null)
            {
                AncDate.ClearValidationError();
                ProcTime.SetValidation("Please Select Time");
                ProcTime.RaiseValidationError();
                ProcTime.Focus();
                return false;
            }
            else if ((MasterListItem)cmbDoctor.SelectedItem == null || ((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.SetValidation("Please Select Doctor");
                cmbDoctor.TextBox.RaiseValidationError();
                cmbDoctor.TextBox.Focus();
                return false;
            }
            else if (txtPeriodofGestation.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.SetValidation("Please Enter Period Of Gestation");
                txtPeriodofGestation.RaiseValidationError();
                txtPeriodofGestation.Focus();
                return false;
            }
            else if (txtBP.Text.Trim().IsValueDouble() == false || txtBP.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.ClearValidationError();
                txtBP.SetValidation("Please Enter Number");
                txtBP.RaiseValidationError();
                txtBP.Focus();
                return false;
            }
            else if (txtWeight.Text.Trim().IsValueDouble() == false || txtWeight.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.SetValidation("Please Enter Number");
                txtWeight.RaiseValidationError();
                txtWeight.Focus();
                return false;
            }
            else if (txtHTofUterus.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtHTofUterus.SetValidation("Please Enter HT. of Uterus");
                txtHTofUterus.RaiseValidationError();
                txtHTofUterus.Focus();
                return false;
            }
            else if ((MasterListItem)cmbOedema.SelectedItem == null || ((MasterListItem)cmbOedema.SelectedItem).ID == 0)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                cmbOedema.TextBox.SetValidation("Please Select Oedema");
                cmbOedema.TextBox.RaiseValidationError();
                cmbOedema.TextBox.Focus();
                return false;
            }
            else if ((MasterListItem)cmbPreAndPosition.SelectedItem == null || ((MasterListItem)cmbPreAndPosition.SelectedItem).ID == 0)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtHTofUterus.ClearValidationError();
                cmbOedema.TextBox.ClearValidationError();

                cmbPreAndPosition.TextBox.SetValidation("Please Select Presentation And Position");
                cmbPreAndPosition.TextBox.RaiseValidationError();
                cmbPreAndPosition.TextBox.Focus();
                return false;
            }
            else if ((MasterListItem)cmbFHS.SelectedItem == null || ((MasterListItem)cmbFHS.SelectedItem).ID == 0)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtHTofUterus.ClearValidationError();
                cmbOedema.TextBox.ClearValidationError();

                cmbPreAndPosition.TextBox.ClearValidationError();
                cmbFHS.TextBox.SetValidation("Please Select FHS");
                cmbFHS.TextBox.RaiseValidationError();
                cmbFHS.TextBox.Focus();
                return false;
            }
            else if (txtTreatment.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtHTofUterus.ClearValidationError();
                cmbOedema.TextBox.ClearValidationError();

                cmbPreAndPosition.TextBox.ClearValidationError();
                txtTreatment.SetValidation("Please Enter Treatment");
                txtTreatment.RaiseValidationError();
                txtTreatment.Focus();
                return false;
            }
            else if (txtAbdGirth.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtHTofUterus.ClearValidationError();
                cmbOedema.TextBox.ClearValidationError();

                txtTreatment.ClearValidationError();
                cmbPreAndPosition.TextBox.ClearValidationError();
                txtAbdGirth.SetValidation("Please Enter Abd. Girth");
                txtAbdGirth.RaiseValidationError();
                txtAbdGirth.Focus();
                return false;
            }
            else if (txtRelationtoBrim.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtHTofUterus.ClearValidationError();
                cmbOedema.TextBox.ClearValidationError();

                txtTreatment.ClearValidationError();
                cmbPreAndPosition.TextBox.ClearValidationError();
                txtRelationtoBrim.SetValidation("Please Enter Relation to Brim");
                txtRelationtoBrim.RaiseValidationError();
                txtRelationtoBrim.Focus();
                return false;
            }
            else
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPeriodofGestation.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtHTofUterus.ClearValidationError();
                cmbOedema.TextBox.ClearValidationError();
                cmbPreAndPosition.TextBox.ClearValidationError();
                cmbFHS.TextBox.ClearValidationError();
                txtTreatment.ClearValidationError();
                txtAbdGirth.ClearValidationError();
                txtRelationtoBrim.ClearValidationError();
                return true;
            }
        }
        private void cmdDeleteExamination_Click(object sender, RoutedEventArgs e)
        {
            if (grdResultEntry.SelectedItem != null)
            {
                if (HisIsfreezed == false)
                {
                    string msgText = "Are you sure you want to Delete?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {

                        if (res == MessageBoxResult.Yes)
                        {
                            foreach (var item in ExaminationList1)
                            {
                                if (item.ID != 0)
                                    item.IsView = true;

                            }

                            ExaminationList1.RemoveAt(grdResultEntry.SelectedIndex);
                            grdResultEntry.ItemsSource = null;
                            grdResultEntry.ItemsSource = ExaminationList1;
                            grdResultEntry.UpdateLayout();
                            grdResultEntry.Focus();

                        }

                    };

                    msgWD.Show();
                }
            }
        }

        private void AncAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExaminationValidations())
            {
                clsANCExaminationDetailsVO Examination = new clsANCExaminationDetailsVO();
                if (AncDate.SelectedDate != null)
                    Examination.ExaDate = (DateTime)AncDate.SelectedDate;
                if (ProcTime.Value != null)
                    Examination.ExaTime = (DateTime)ProcTime.Value;
                if (((MasterListItem)cmbPreAndPosition.SelectedItem).ID != null)
                {
                    Examination.PresenAndPos = ((MasterListItem)cmbPreAndPosition.SelectedItem).ID;
                    Examination.PresenAndPosDescription = ((MasterListItem)cmbPreAndPosition.SelectedItem).Description;
                }
                if (((MasterListItem)cmbFHS.SelectedItem).ID != null)
                {
                    Examination.FHS = ((MasterListItem)cmbFHS.SelectedItem).ID;
                    Examination.FHSDescription = ((MasterListItem)cmbFHS.SelectedItem).Description;
                }
                if (((MasterListItem)cmbDoctor.SelectedItem).ID != null)
                {
                    Examination.Doctor = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                    Examination.DoctorName = ((MasterListItem)cmbDoctor.SelectedItem).Description;
                }
                if (!String.IsNullOrEmpty(txtPeriodofGestation.Text))
                    Examination.PeriodOfPreg = txtPeriodofGestation.Text.Trim();
                if (!String.IsNullOrEmpty(txtBP.Text))
                    Examination.BP = Convert.ToSingle(txtBP.Text);
                if (!String.IsNullOrEmpty(txtWeight.Text))
                    Examination.Weight = Convert.ToSingle(txtWeight.Text);
                if (!String.IsNullOrEmpty(txtHTofUterus.Text))
                    Examination.HTofUterus = txtHTofUterus.Text.Trim();
                if (((MasterListItem)cmbOedema.SelectedItem).ID != null)
                {
                    Examination.OademaID = ((MasterListItem)cmbOedema.SelectedItem).ID;
                    Examination.Oadema = ((MasterListItem)cmbOedema.SelectedItem).Description;
                }

                //if (!String.IsNullOrEmpty(txtOedema.Text))
                //    Examination.Oadema = txtOedema.Text.Trim();
                //if (!String.IsNullOrEmpty(txtOedema2.Text))
                //    Examination.Oadema2 = txtOedema2.Text.Trim();
                //if (!String.IsNullOrEmpty(txtOedema3.Text))
                //    Examination.Oadema3 = txtOedema3.Text.Trim();
                if (!String.IsNullOrEmpty(txtRelationtoBrim.Text))
                    Examination.RelationtoBrim = txtRelationtoBrim.Text.Trim();
                if (!String.IsNullOrEmpty(txtAbdGirth.Text))
                    Examination.AbdGirth = txtAbdGirth.Text.Trim();
                if (!String.IsNullOrEmpty(txtTreatment.Text))
                    Examination.Treatment = txtTreatment.Text.Trim();
                if (!String.IsNullOrEmpty(txtRemarks.Text))
                    Examination.Remark = txtRemarks.Text.Trim();
                Examination.IsView = false;
                Examination.Status = true;
                ExaminationList1.Add(Examination);
                grdResultEntry.ItemsSource = null;
                grdResultEntry.ItemsSource = ExaminationList1;
                grdResultEntry.UpdateLayout();
                grdResultEntry.Focus();
                clearExaminationUI();
            }
        }

        private void AncModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExaminationValidations())
            {
                if (ExaminationList1.Count > 0)
                {
                    if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem) != null)
                    {
                        foreach (var item in ExaminationList1)
                        {
                            if (item.ID == (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).ID))
                            {
                                clsANCExaminationDetailsVO Examination = new clsANCExaminationDetailsVO();
                                if (AncDate.SelectedDate != null)
                                    item.ExaDate = (DateTime)AncDate.SelectedDate;
                                if (ProcTime.Value != null)
                                    item.ExaTime = (DateTime)ProcTime.Value;
                                if (((MasterListItem)cmbPreAndPosition.SelectedItem).ID != null)
                                {
                                    item.PresenAndPos = ((MasterListItem)cmbPreAndPosition.SelectedItem).ID;
                                    item.PresenAndPosDescription = ((MasterListItem)cmbPreAndPosition.SelectedItem).Description;
                                }
                                if (((MasterListItem)cmbFHS.SelectedItem).ID != null)
                                {
                                    item.FHS = ((MasterListItem)cmbFHS.SelectedItem).ID;
                                    item.FHSDescription = ((MasterListItem)cmbFHS.SelectedItem).Description;
                                }
                                if (((MasterListItem)cmbDoctor.SelectedItem).ID != null)
                                {
                                    item.Doctor = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                                    item.DoctorName = ((MasterListItem)cmbDoctor.SelectedItem).Description;
                                }
                                if (!String.IsNullOrEmpty(txtPeriodofGestation.Text))
                                    item.PeriodOfPreg = txtPeriodofGestation.Text.Trim();
                                if (!String.IsNullOrEmpty(txtBP.Text))
                                    item.BP = Convert.ToSingle(txtBP.Text);
                                if (!String.IsNullOrEmpty(txtWeight.Text))
                                    item.Weight = Convert.ToSingle(txtWeight.Text);
                                if (!String.IsNullOrEmpty(txtHTofUterus.Text))
                                    item.HTofUterus = txtHTofUterus.Text.Trim();
                                if (((MasterListItem)cmbOedema.SelectedItem).ID != null)
                                {
                                    item.OademaID = ((MasterListItem)cmbOedema.SelectedItem).ID;
                                    item.Oadema = ((MasterListItem)cmbOedema.SelectedItem).Description;
                                }

                                if (!String.IsNullOrEmpty(txtRelationtoBrim.Text))
                                    item.RelationtoBrim = txtRelationtoBrim.Text.Trim();
                                if (!String.IsNullOrEmpty(txtAbdGirth.Text))
                                    item.AbdGirth = txtAbdGirth.Text.Trim();
                                if (!String.IsNullOrEmpty(txtTreatment.Text))
                                    item.Treatment = txtTreatment.Text.Trim();
                                if (!String.IsNullOrEmpty(txtRemarks.Text))
                                    item.Remark = txtRemarks.Text.Trim();

                            }
                        }
                    }
                    grdResultEntry.ItemsSource = null;
                    grdResultEntry.ItemsSource = ExaminationList1;
                    grdResultEntry.Focus();
                    grdResultEntry.UpdateLayout();
                    grdResultEntry.SelectedIndex = ExaminationList1.Count - 1;
                    clearExaminationUI();
                }

            }
        }

        private void AnccmdView_Click(object sender, RoutedEventArgs e)
        {
            ViewANCExamination();

        }
        private void AddUpdateExamination()
        {
            AddUpdateANCExaminationBizActionVO bizActionObj = new AddUpdateANCExaminationBizActionVO();
            bizActionObj.ANCExaminationDetails = new clsANCExaminationDetailsVO();
            bizActionObj.ANCExaminationDetailsList = new List<clsANCExaminationDetailsVO>();
            if (IsModify == true)
            {
                // bizActionObj.ANCExaminationDetails.ID = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).ID;
                bizActionObj.ANCExaminationDetails.UnitID = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).UnitID;
            }
            else
            {
                bizActionObj.ANCExaminationDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            bizActionObj.ANCExaminationDetails.ANCID = ANCId;
            bizActionObj.ANCExaminationDetails.PatientID = ((clsANCVO)dgANCList.SelectedItem).PatientID;
            bizActionObj.ANCExaminationDetails.PatientUnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            bizActionObj.ANCExaminationDetails.CoupleID = CoupleDetails.CoupleId;
            bizActionObj.ANCExaminationDetails.CoupleUnitID = CoupleDetails.CoupleUnitId;
            foreach (var item in ExaminationList1)
            {
                bizActionObj.ANCExaminationDetailsList.Add(item);
            }
            if (chkFreezeExamination.IsChecked == true)
                bizActionObj.ANCExaminationDetails.Isfreezed = true;
            else
                bizActionObj.ANCExaminationDetails.Isfreezed = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((AddUpdateANCExaminationBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Examination Details Saved Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillExaminationGrid();
                            }
                        };
                        msgWin.Show();
                        clearExaminationUI();
                        AncAddButton.IsEnabled = true;
                    }
                    else if (((AddUpdateANCExaminationBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Examination Details Modified Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillExaminationGrid();
                            }
                        };
                        msgWin.Show();
                        clearExaminationUI();
                        AncAddButton.IsEnabled = true;
                        AncModifyButton.IsEnabled = false;
                    }
                }
            };
            client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillExaminationGrid()
        {
            clsGetANCExaminationBizActionVO BizAction = new clsGetANCExaminationBizActionVO();
            BizAction.ANCExaminationList = new List<clsANCExaminationDetailsVO>();
            BizAction.ANCExaminationDetails.UnitID = ((clsANCVO)dgANCList.SelectedItem).UnitID;
            BizAction.ANCExaminationDetails.ANCID = ANCId;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = 0;
            BizAction.MaximumRows = 15;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetANCExaminationBizActionVO)args.Result).ANCExaminationList != null)
                    {
                        clsGetANCExaminationBizActionVO result = args.Result as clsGetANCExaminationBizActionVO;
                        ExaminationList1.Clear();
                        if (result.ANCExaminationList != null)
                        {
                            foreach (var item in result.ANCExaminationList)
                            {
                                item.IsView = true;
                                ExaminationList1.Add(item);

                            }
                            grdResultEntry.ItemsSource = null;
                            grdResultEntry.ItemsSource = ExaminationList1;

                            dataGrid2PagerTherapy.Source = null;
                            dataGrid2PagerTherapy.PageSize = BizAction.MaximumRows;
                            dataGrid2PagerTherapy.Source = ExaminationList1;
                            if (ExaminationList1.Count > 0)
                            {
                                if (ExaminationList1[ExaminationList1.Count - 1].Isfreezed == true)
                                {
                                    HisIsfreezed = true;
                                    chkFreezeExamination.IsChecked = true;
                                    CmdModify.IsEnabled = false;
                                    CmdSave.IsEnabled = false;
                                    AncAddButton.IsEnabled = false;
                                    AncModifyButton.IsEnabled = false;
                                }
                                else
                                {
                                    if (IsClosed == true)
                                    {
                                        HisIsfreezed = true;
                                        CmdModify.IsEnabled = false;
                                        CmdSave.IsEnabled = false;
                                    }
                                    else
                                    {
                                        HisIsfreezed = false;
                                        CmdModify.IsEnabled = true;
                                        CmdSave.IsEnabled = true;
                                    }
                                }
                            }

                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        #endregion

        #region Document
        private void SaveDocument()
        {

            AddUpdateANCDocumentBizActionVO BizAction = new AddUpdateANCDocumentBizActionVO();
            BizAction.ANCDocuments = new clsANCDocumentsVO();
            BizAction.ANCDocumentsList = new List<clsANCDocumentsVO>();
            BizAction.ANCDocuments.ANCID = ANCId;
            BizAction.ANCDocuments.CoupleID = CoupleDetails.CoupleId;
            BizAction.ANCDocuments.CoupleUnitID = CoupleDetails.CoupleUnitId;
            BizAction.ANCDocuments.PatientID = ((clsANCVO)dgANCList.SelectedItem).PatientID;
            BizAction.ANCDocuments.PatientUnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            BizAction.ANCDocuments.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            foreach (var item in DoucumentList)
            {
                BizAction.ANCDocumentsList.Add(item);
            }

            if (chkDocument.IsChecked == true)
                BizAction.ANCDocuments.Isfreezed = true;
            else
                BizAction.ANCDocuments.Isfreezed = false;
            BizAction.ANCDocuments.Status = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((AddUpdateANCDocumentBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Doument Saved Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillDocumentGrid();
                            }
                        };
                        msgWin.Show();

                    }
                    if (((AddUpdateANCDocumentBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Doument Modified Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillDocumentGrid();
                            }
                        };
                        msgWin.Show();
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDocumentGrid()
        {
            GetANCDocumentBizActionVO BizAction = new GetANCDocumentBizActionVO();
            BizAction.ANCDocumentList = new List<clsANCDocumentsVO>();
            BizAction.ANCDocument.UnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.ANCDocument.ANCID = ANCId;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = 0;
            BizAction.MaximumRows = 15;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((GetANCDocumentBizActionVO)args.Result).ANCDocumentList != null)
                    {
                        GetANCDocumentBizActionVO result = args.Result as GetANCDocumentBizActionVO;
                        DoucumentList.Clear();
                        if (result.ANCDocumentList != null)
                        {
                            foreach (var item in result.ANCDocumentList)
                            {
                                DoucumentList.Add(item);
                            }
                            dgDocumentGrid.ItemsSource = null;
                            dgDocumentGrid.ItemsSource = DoucumentList;

                            DocumentGridPager.Source = null;
                            DocumentGridPager.PageSize = BizAction.MaximumRows;
                            DocumentGridPager.Source = DoucumentList;

                            if (DoucumentList.Count > 0)
                            {
                                if (DoucumentList[DoucumentList.Count - 1].Isfreezed == true)
                                {
                                    chkDocument.IsChecked = true;
                                    CmdModify.IsEnabled = false;
                                    CmdSave.IsEnabled = false;
                                }
                                else
                                {
                                    if (IsClosed == true)
                                    {
                                        CmdModify.IsEnabled = false;
                                        CmdSave.IsEnabled = false;
                                    }
                                    else
                                    {
                                        CmdModify.IsEnabled = true;
                                        CmdSave.IsEnabled = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void cmdDeleteDoc_Click(object sender, RoutedEventArgs e)
        {

            string msgTitle = "Palash";
            string msgText = "Are you sure you want to Delete this Document ?";
            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinDeleteDocument_OnMessageBoxClosed);
            msgWin.Show();
        }
        private void hpyrlinkFileView_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsANCDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileName))
            {
                if (((clsANCDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileContent != null)
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsANCDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileName });
                            AttachedFileNameList.Add(((clsANCDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsANCDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileName, ((clsANCDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileContent);
                }
            }
        }
        void msgWinDeleteDocument_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (IsClosed == false && chkDocument.IsChecked == false)
                {

                    DoucumentList.RemoveAt(dgDocumentGrid.SelectedIndex);
                    dgDocumentGrid.ItemsSource = null;
                    dgDocumentGrid.ItemsSource = DoucumentList;
                    dgDocumentGrid.UpdateLayout();
                    dgDocumentGrid.Focus();
                }
            }
        }
        private void cmdAttachedDoc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                AttachedFileName = openDialog.File.Name;
                txtFileName.Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        AttachedFileContents = new byte[stream.Length];
                        stream.Read(AttachedFileContents, 0, (int)stream.Length);
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error while reading file.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    throw ex;
                }
            }
        }
        private void CmdAddDocument_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text.Trim().Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Title", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else if (txtDescription.Text.Trim().Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Description", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else if (txtFileName.Text.Trim().Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Browse File", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else
            {
                clsANCDocumentsVO Doc = new clsANCDocumentsVO();
                Doc.Date = System.DateTime.Now;
                Doc.AttachedFileContent = AttachedFileContents;
                Doc.AttachedFileName = AttachedFileName;
                Doc.Title = txtTitle.Text.Trim();
                Doc.Description = txtDescription.Text.Trim();
                Doc.IsDeleted = false;
                DoucumentList.Add(Doc);
                dgDocumentGrid.ItemsSource = null;
                dgDocumentGrid.ItemsSource = DoucumentList;
                dgDocumentGrid.UpdateLayout();
                dgDocumentGrid.Focus();
                clearDocumentUI();
            }

        }
        private void clearDocumentUI()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtFileName.Text = "";
            AttachedFileName = "";
        }
        #endregion

        #region Suggestion
        private void FillSuggestionDetails()
        {
            GetANCSuggestionBizActionVO bizActionObj = new GetANCSuggestionBizActionVO();
            bizActionObj.ANCSuggestion = new clsANCSuggestionVO();
            bizActionObj.ANCSuggestion.UnitID = ((clsANCVO)dgANCList.SelectedItem).UnitID;
            bizActionObj.ANCSuggestion.ANCID = ANCId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion != null)
                    {
                        GetANCSuggestionBizActionVO result = arg.Result as GetANCSuggestionBizActionVO;

                        if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion != null)
                        {
                            SuggID = ((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.ID;
                            SuggUnitID = ((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.UnitID;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Smoking == true)
                                chkSmoking.IsChecked = true;
                            else
                                chkSmoking.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Alcoholic == true)
                                chkAlcoholicBeverages.IsChecked = true;
                            else
                                chkAlcoholicBeverages.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Cplace == true)
                                chkCrowdedPlace.IsChecked = true;
                            else
                                chkCrowdedPlace.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Bag == true)
                                chkBag.IsChecked = true;
                            else
                                chkBag.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Sheets == true)
                                chkSheet.IsChecked = true;
                            else
                                chkSheet.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Tea == true)
                                chkteaCoffee.IsChecked = true;
                            else
                                chkteaCoffee.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Xray == true)
                                chkXRay.IsChecked = true;
                            else
                                chkXRay.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Medication == true)
                                chkSelfMedication.IsChecked = true;
                            else
                                chkSelfMedication.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Isfreezed == true)
                            {
                                chkSuggestion.IsChecked = true;
                                CmdModify.IsEnabled = false;
                                CmdSave.IsEnabled = false;
                            }
                            else
                            {
                                if (IsClosed == true)
                                {
                                    chkSuggestion.IsChecked = false;
                                    CmdModify.IsEnabled = false;
                                    CmdSave.IsEnabled = false;
                                }
                                else
                                {
                                    chkSuggestion.IsChecked = false;
                                    CmdModify.IsEnabled = true;
                                    CmdSave.IsEnabled = true;
                                }
                            }
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.IrregularDiet == true)
                                chkIrregularDiet.IsChecked = true;
                            else
                                chkIrregularDiet.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.HeavyObject == true)
                                chkheavyObject.IsChecked = true;
                            else
                                chkheavyObject.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Exertion == true)
                                chkExertion.IsChecked = true;
                            else
                                chkExertion.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Exercise == true)
                                chkExcessiveExercise.IsChecked = true;
                            else
                                chkExcessiveExercise.IsChecked = false;
                            if (((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Remark != null)
                                txtSuggestionRemark.Text = ((GetANCSuggestionBizActionVO)arg.Result).ANCSuggestion.Remark;
                            IsModify = true;
                        }
                    }

                }

            };
            client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void AddSuggestionButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder str = new StringBuilder();
            str.Append(txtSuggestionRemark.Text.Trim());
            if (txtSuggestionRemark.Text.Trim() == "")
            {
                str.Append(((MasterListItem)cmbconsult.SelectedItem).Description);

            }
            else if (txtSuggestionRemark.Text.Trim() != "")
            {
                str.Append(",");
                str.Append(((MasterListItem)cmbconsult.SelectedItem).Description);
            }

            txtSuggestionRemark.Text = str.ToString();
            //  if (ConsultDescription == "" || ConsultDescription.EndsWith(","))
            //{
            //    ConsultDescription = ConsultDescription + ((MasterListItem)cmbconsult.SelectedItem).Description;
            //    ConsultDescription = ConsultDescription + ",";
            //}
            //else if (ConsultDescription != "")
            //{
            //    ConsultDescription = ConsultDescription + ",";
            //    ConsultDescription = ConsultDescription + ((MasterListItem)cmbconsult.SelectedItem).Description;
            //    ConsultDescription = ConsultDescription + ",";
            //}
            //if (ConsultDescription.EndsWith(","))
            //{
            //    ConsultDescription = ConsultDescription.Remove(ConsultDescription.Length - 1, 1);
            //    txtSuggestionRemark.Text = ConsultDescription;
            //}
            //else
            //    txtSuggestionRemark.Text = ConsultDescription;
        }
        private void AddUpdateANCSuggestion()
        {
            AddUpdateANCSuggestionBizActionVO BizAction = new AddUpdateANCSuggestionBizActionVO();
            BizAction.ANCSuggestion = new clsANCSuggestionVO();

            if (IsModify == true)
            {
                BizAction.ANCSuggestion.ID = SuggID;
                BizAction.ANCSuggestion.UnitID = SuggUnitID;
            }
            else
            {
                BizAction.ANCSuggestion.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            BizAction.ANCSuggestion.ANCID = ((clsANCVO)dgANCList.SelectedItem).ID;
            BizAction.ANCSuggestion.PatientID = ((clsANCVO)dgANCList.SelectedItem).PatientID;
            BizAction.ANCSuggestion.PatientUnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            BizAction.ANCSuggestion.CoupleID = CoupleDetails.CoupleId;
            BizAction.ANCSuggestion.CoupleUnitID = CoupleDetails.CoupleUnitId;
            if (chkSmoking.IsChecked == true)
                BizAction.ANCSuggestion.Smoking = true;
            else
                BizAction.ANCSuggestion.Smoking = false;
            if (chkAlcoholicBeverages.IsChecked == true)
                BizAction.ANCSuggestion.Alcoholic = true;
            else
                BizAction.ANCSuggestion.Alcoholic = false;
            if (chkSelfMedication.IsChecked == true)
                BizAction.ANCSuggestion.Medication = true;
            else
                BizAction.ANCSuggestion.Medication = false;
            if (chkSmoking.IsChecked == true)
                BizAction.ANCSuggestion.Smoking = true;
            else
                BizAction.ANCSuggestion.Smoking = false;
            if (chkXRay.IsChecked == true)
                BizAction.ANCSuggestion.Xray = true;
            else
                BizAction.ANCSuggestion.Xray = false;
            if (chkIrregularDiet.IsChecked == true)
                BizAction.ANCSuggestion.IrregularDiet = true;
            else
                BizAction.ANCSuggestion.IrregularDiet = false;
            if (chkExertion.IsChecked == true)
                BizAction.ANCSuggestion.Exertion = true;
            else
                BizAction.ANCSuggestion.Exertion = false;
            if (chkCrowdedPlace.IsChecked == true)
                BizAction.ANCSuggestion.Cplace = true;
            else
                BizAction.ANCSuggestion.Cplace = false;
            if (chkteaCoffee.IsChecked == true)
                BizAction.ANCSuggestion.Tea = true;
            else
                BizAction.ANCSuggestion.Tea = false;
            if (chkheavyObject.IsChecked == true)
                BizAction.ANCSuggestion.HeavyObject = true;
            else
                BizAction.ANCSuggestion.HeavyObject = false;
            if (chkBag.IsChecked == true)
                BizAction.ANCSuggestion.Bag = true;
            else
                BizAction.ANCSuggestion.Bag = false;
            if (chkSheet.IsChecked == true)
                BizAction.ANCSuggestion.Sheets = true;
            else
                BizAction.ANCSuggestion.Sheets = false;
            if (chkSuggestion.IsChecked == true)
                BizAction.ANCSuggestion.Isfreezed = true;
            else
                BizAction.ANCSuggestion.Isfreezed = false;
            if (chkExcessiveExercise.IsChecked == true)
                BizAction.ANCSuggestion.Exercise = true;
            else
                BizAction.ANCSuggestion.Exercise = false;

            if (chkIsClosed.IsChecked == true)
                BizAction.ANCSuggestion.IsClosed = true;
            else
                BizAction.ANCSuggestion.IsClosed = false;
            BizAction.ANCSuggestion.Remark = txtSuggestionRemark.Text.Trim();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((AddUpdateANCSuggestionBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("PALASH", "ANC Suggestion Saved Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                        ClearSuggestionUI();
                        FillSuggestionDetails();
                        // objAnimation.Invoke(RotationType.Backward);
                        FillANCDataGird();
                        SetButtonState("New");
                    }
                    else if (((AddUpdateANCSuggestionBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("PALASH", "ANC Suggestion Modified Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                        ClearSuggestionUI();
                        FillSuggestionDetails();
                        // objAnimation.Invoke(RotationType.Backward);
                        FillANCDataGird();
                        SetButtonState("Modify");
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void ClearSuggestionUI()
        {
            chkSmoking.IsChecked = false;
            chkAlcoholicBeverages.IsChecked = false;
            chkBag.IsChecked = false;
            chkCrowdedPlace.IsChecked = false;
            chkExcessiveExercise.IsChecked = false;
            chkExertion.IsChecked = false;
            chkheavyObject.IsChecked = false;
            chkIrregularDiet.IsChecked = false;
            chkSelfMedication.IsChecked = false;
            chkSheet.IsChecked = false;
            chkteaCoffee.IsChecked = false;
            chkXRay.IsChecked = false;
            txtSuggestionRemark.Text = "";
            cmbconsult.SelectedValue = (long)0;
            foreach (MasterListItem item1 in objList1)
            {

                item1.Status = false;

            }
        }
        #endregion

        private void chkUSGInv_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
