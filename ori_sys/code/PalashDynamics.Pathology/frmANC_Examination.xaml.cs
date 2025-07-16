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
    public partial class frmANC_Examination : UserControl, IInitiateCIMS
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
        long SuggID = 0;
        #endregion

        #region constructor
        public frmANC_Examination()
        {
            InitializeComponent();
         
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

        #region LoadEvent

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
                        CoupleInfo.Visibility = Visibility.Collapsed;
                        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                  
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

        #endregion

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            // SetupPage();
        }

        void obestricHistoryList_OnRefresh(object sender, RefreshEventArgs r)
        {
            GetObestricHistoryList();
        }

        void investigationList_OnRefresh(object sender, RefreshEventArgs r)
        {
            FillInvestigationDetailsGrid();
        }

        void USGList_OnRefresh(object senser, RefreshEventArgs r)
        {
            FillUSGDataGrid();
        }

        private void AncHyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {

        }

        private void AncHyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {

        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsClosed==false)
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
                    if (GeneralDetail.IsSelected)
                    {
                        IsModify = false;
                       
                       
                        ClearGeneralDetailsUI();
                    }
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

        private void cmdNew_Click(object sender, RoutedEventArgs e) 
        {
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
                if (Investigation.IsSelected)
                {
                    if (investigationList.Count > 0 || USGList.Count > 0)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Are You Sure \n  You Want To Save ANC Investigation and USG Details?";
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
            if (GeneralDetail != null)
            {
                if (GeneralDetail.IsSelected)
                {
                    if (Validation())
                    {
                        string msgTitle = "Palash";
                        string msgText = "Are You Sure \n  You Want To Modify ANC Details?";
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
            }
            if (History != null)
            {
                if (History.IsSelected)
                {
                    if (Validation())
                    {
                        string msgTitle = "Palash";
                        string msgText = "Are You Sure \n  You Want To Modify ANC History?";
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
            }
            if (Investigation.IsSelected)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Modify ANC Investigation and USG Details?";
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
            }
            if (Examination.IsSelected)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Modify ANC Examination?";
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
               
            if (Suggestion.IsSelected)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n  You Want To Modify ANC Suggestion?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            IsModify = true;
                            AddUpdateANCSuggestion();
                        }
                    };
                    msgWin.Show();
                }
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
       
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryValidations())
            {
                IsModify = false;
                AddUpdateObstetricHistory();
                clearObstericHistory();
            }
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryValidations())
            {
                AddUpdateObstetricHistory();
                clearObstericHistory();
                ModifyButton.IsEnabled = false;
            }
        }

        private void AnccmdView_Click(object sender, RoutedEventArgs e)
        {
            ViewANCExamination();
        }

        private void hlbView_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AnccmdBrowse_Click(object sender, RoutedEventArgs e)
        {
        }

        private void hblAttach_Click(object sender, RoutedEventArgs e)
        {
        }

        private void txtStoreCode_AncLostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void AnccmbTest_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
        }
      
        private void AddSuggestionButton_Click(object sender, RoutedEventArgs e)
        {
            if(ConsultDescription =="" || ConsultDescription.EndsWith(","))
            {
                ConsultDescription = ConsultDescription + ((MasterListItem)cmbconsult.SelectedItem).Description;
                 ConsultDescription = ConsultDescription + ",";
            }
            else if(ConsultDescription !="" )
            {
                ConsultDescription = ConsultDescription + ",";
                ConsultDescription = ConsultDescription + ((MasterListItem)cmbconsult.SelectedItem).Description;
                 ConsultDescription = ConsultDescription + ",";
            }
            if (ConsultDescription.EndsWith(","))
            {
                ConsultDescription = ConsultDescription.Remove(ConsultDescription.Length - 1, 1);
                txtSuggestionRemark.Text =ConsultDescription ;
            }
            else
            txtSuggestionRemark.Text =ConsultDescription;       
        }

        private void Diabeties_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Hypertension_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CongentialAnomolies_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TB_Click(object sender, RoutedEventArgs e)
        {
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
            if (txtTitle.Text.Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Title", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else if (txtDescription.Text.Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Description", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else if (txtFileName.Text.Length == 0)
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
    
        private void cmdOutComeCancel_Click(object sender, RoutedEventArgs e)
        {
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

        private void cmdDeleteDoc_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to Delete this Document ?";
            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinDeleteDocument_OnMessageBoxClosed);
            msgWin.Show();
        }

        void msgWinDeleteDocument_OnMessageBoxClosed(MessageBoxResult result) 
        {
            if (result == MessageBoxResult.Yes)
            {
                if (IsClosed == false && chkDocument.IsChecked == false)
                {
                    clsDeleteANCDocumentBizActionVO BizAction = new clsDeleteANCDocumentBizActionVO();
                    BizAction.ANCDocuments = new clsANCDocumentsVO();
                    BizAction.ANCDocuments.ID = ((clsANCDocumentsVO)dgDocumentGrid.SelectedItem).ID;
                    BizAction.ANCDocuments.ANCID = ANCId;
                    BizAction.ANCDocuments.UnitID = ((clsANCDocumentsVO)dgDocumentGrid.SelectedItem).UnitID;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            string msgTitle1 = "Palash";
                            string msgText1 = "Document Deleted Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWin1 =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle1, msgText1, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                            msgWin1.OnMessageBoxClosed += (result1) =>
                            {
                                if (result1 == MessageBoxResult.OK)
                                {
                                    FillDocumentGrid();
                                }
                            };
                            msgWin1.Show();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
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

        private void saveUpdateANCDetails(long ID, long TabID, long DocumentId, String Msg)
        {
            if (TabID == 1)
            {
                SaveANCGeneralDetails();
            }
        }

        #region Validation
        bool result = true;
        public bool Validation()
        {
            if (txtAncCode.Text.Trim() == null)
            {

                txtAncCode.SetValidation("Please Enter ANC Code");
                txtAncCode.RaiseValidationError();
                txtAncCode.Focus();
                result= false;
            }
            else if (StartDateAnc.SelectedDate == null)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.SetValidation("Please Select ANC Date");
                StartDateAnc.RaiseValidationError();
                StartDateAnc.Focus();
                result= false;
            }
            else if (txtM.Text.Trim() == string.Empty)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.SetValidation("Please Enter M");
                txtM.RaiseValidationError();
                txtM.Focus();
                result= false;
            }
            else if (txtG.Text.Trim() == string.Empty)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.ClearValidationError();
                txtG.SetValidation("Please Enter G");
                txtG.RaiseValidationError();
                txtG.Focus();
                result= false;
            }
            else if (txtP.Text.Trim() == string.Empty)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.ClearValidationError();
                txtG.ClearValidationError();
                txtP.SetValidation("Please Enter P");
                txtP.RaiseValidationError();
                txtP.Focus();
                result= false;
            }
            else if (txtA.Text.Trim() == string.Empty)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.ClearValidationError();
                txtG.ClearValidationError();
                txtP.ClearValidationError();
                txtA.SetValidation("Please Enter A");
                txtA.RaiseValidationError();
                txtA.Focus();
                result= false;
            }
            else if (txtL.Text.Trim() == string.Empty)
            {
                txtAncCode.ClearValidationError();
                StartDateAnc.ClearValidationError();
                txtM.ClearValidationError();
                txtG.ClearValidationError();
                txtP.ClearValidationError();
                txtA.ClearValidationError();
                txtL.SetValidation("Please Enter L");
                txtL.RaiseValidationError();
                txtL.Focus();
                result= false;
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
                result= true;
            }
            return result;
        }

        private bool HistoryValidations() 
        {
            if (txtMaturityComplication.Text.Trim() == string.Empty) 
            {
                txtMaturityComplication.SetValidation("Please Enter Maturity/Complication");
                txtMaturityComplication.RaiseValidationError();
                txtMaturityComplication.Focus();
                return false;
            }
            else if (txtModeOfDelivery.Text.Trim() == string.Empty)
            {
                txtMaturityComplication.ClearValidationError();
                txtModeOfDelivery.SetValidation("Please Enter Mode of Delivery");
                txtModeOfDelivery.RaiseValidationError();
                txtModeOfDelivery.Focus();
                return false;
            }
            else 
            {
                txtMaturityComplication.ClearValidationError();
                txtModeOfDelivery.ClearValidationError();
                return true;
            }
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
            else if (txtHeight.Text.Trim().IsValueDouble() == false || txtHeight.Text.Trim() == string.Empty) 
            {
                InvDate.ClearValidationError();
                cmbInvestigation.TextBox.ClearValidationError();
                txtHeight.SetValidation("Please Enter Height in Number");
                txtHeight.RaiseValidationError();
                txtHeight.Focus();
                return false;
            }
            else if (txtBreast.Text.Trim().IsValueDouble() == false || txtBreast.Text.Trim() == string.Empty)
            {
                InvDate.ClearValidationError();
                cmbInvestigation.TextBox.ClearValidationError();
                txtHeight.ClearValidationError();
                txtBreast.SetValidation("Please Enter Breast in Number");
                txtBreast.RaiseValidationError();
                txtBreast.Focus();
                return false;
            }
            else if (txtChest.Text.Trim().IsValueDouble() == false || txtChest.Text.Trim() == string.Empty)
            {
                InvDate.ClearValidationError();
                cmbInvestigation.TextBox.ClearValidationError();
                txtHeight.ClearValidationError();
                txtBreast.ClearValidationError();
                txtChest.SetValidation("Please Enter Chest in Number");
                txtChest.RaiseValidationError();
                txtChest.Focus();
                return false;
            }
            else 
            {
                InvDate.ClearValidationError();
                cmbInvestigation.TextBox.ClearValidationError();
                txtHeight.ClearValidationError();
                txtBreast.ClearValidationError();
                txtChest.ClearValidationError();
                return true;
            }
        }

        private bool USGValidations() 
        {
            if (USGDate.SelectedDate == null)
            {
                USGDate.SetValidation("Please Select USG Date");
                USGDate.RaiseValidationError();
                USGDate.Focus();
                return false;
            }
            else if (txtPV.Text.Trim() == string.Empty)
            {
                USGDate.ClearValidationError();
                txtPV.SetValidation("Please Enter PV");
                txtPV.RaiseValidationError();
                txtPV.Focus();
                return false;
            }
            else
            {
                USGDate.ClearValidationError();
                txtPV.ClearValidationError();
                return true;
            }
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
            else if(ProcTime.Value==null)
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
                cmbDoctor.RaiseValidationError();
                cmbDoctor.Focus();
                return false;
            }
            else if (txtPregPeriod.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPregPeriod.SetValidation("Please Enter Period Of Pregnancy");
                txtPregPeriod.RaiseValidationError();
                txtPregPeriod.Focus();
                return false;
             }
            else if (txtBP.Text.Trim().IsValueDouble() == false || txtBP.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPregPeriod.ClearValidationError();
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
                txtPregPeriod.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.SetValidation("Please Enter Number");
                txtWeight.RaiseValidationError();
                txtWeight.Focus();
                return false;
            }
            else if (txtFHeight.Text.Trim().IsValueDouble() == false || txtFHeight.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPregPeriod.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtFHeight.SetValidation("Please Enter Number");
                txtFHeight.RaiseValidationError();
                txtFHeight.Focus();
                return false;
            }
            else if (txtOedema.Text.Trim() == string.Empty)
            {
                AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPregPeriod.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtFHeight.ClearValidationError();
                txtOedema.SetValidation("Please Enter Oedema");
                txtOedema.RaiseValidationError();
                txtOedema.Focus();
                return false;
            }
            else if ((MasterListItem)cmbPreAndPosition.SelectedItem == null || ((MasterListItem)cmbPreAndPosition.SelectedItem).ID == 0)
            {
                 AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPregPeriod.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtFHeight.ClearValidationError();
                txtOedema.ClearValidationError();
                cmbPreAndPosition.TextBox.SetValidation("Please Select Presentation And Position");
                cmbPreAndPosition.RaiseValidationError();
                cmbPreAndPosition.Focus();
                return false;
            }
            else if ((MasterListItem)cmbFHS.SelectedItem == null || ((MasterListItem)cmbFHS.SelectedItem).ID == 0)
            {
                 AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPregPeriod.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtFHeight.ClearValidationError();
                txtOedema.ClearValidationError();
                 cmbPreAndPosition.TextBox.ClearValidationError();
                cmbFHS.TextBox.SetValidation("Please Select FHS");
                cmbFHS.RaiseValidationError();
                cmbFHS.Focus();
                return false;
            }
            else
            {
                 AncDate.ClearValidationError();
                ProcTime.ClearValidationError();
                cmbDoctor.TextBox.ClearValidationError();
                txtPregPeriod.ClearValidationError();
                txtBP.ClearValidationError();
                txtWeight.ClearValidationError();
                txtFHeight.ClearValidationError();
                txtOedema.ClearValidationError();
                cmbPreAndPosition.TextBox.ClearValidationError();
                cmbFHS.TextBox.ClearValidationError();
                return true;
            }
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

        #region   Clear User Interface

        private void ClearGeneralDetailsUI()
        {
            txtA.Text = "";
            txtM.Text = "";
            txtP.Text = "";
            txtL.Text = "";
            txtG.Text = "";
            txtRemark.Text = "";
            StartDateAnc.SelectedDate = System.DateTime.Now;
            EDD.SelectedDate = System.DateTime.Now;
            LMP.SelectedDate = System.DateTime.Now;
            chkMale.IsChecked = false;
            chkFrmale.IsChecked = false;
            chkFreezeGeneralDetails.IsChecked = false;
            txtAncCode.Text = "Auto Generated";
        }
        private void clearDocumentUI()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtFileName.Text = "";
            AttachedFileName = "";
        }

        private void clearObstericHistory()
        {
            if (txtMaturityComplication.Text.Trim() != "" && txtMaturityComplication.Text.Trim() != null)
            {
                txtMaturityComplication.Text = string.Empty;
            }
            txtModeOfDelivery.Text = string.Empty;
            txtObstrtricRemark.Text = string.Empty;
        }

        private void ClearHistoryUI()
        {
            rbdHypYes.IsChecked = false;
            rdbHypNo.IsChecked = false;
            rdbTBNo.IsChecked = false;
            rdbTBYes.IsChecked = false;
            rdbCAYes.IsChecked = false; rdbCANo.IsChecked = false;
            rdbDiabetesYes.IsChecked = false; rdbHypNo.IsChecked = false;
            chkFirstOK.IsChecked = false; chkSecondOk.IsChecked = false;
            DateLMP.SelectedDate = null;
            DateEDD.SelectedDate = null;
            txtRemark.Text = "";
            firstDoseDate.SelectedDate = null;
            secondDoseDate.SelectedDate = null;
        }

        private void ClearInvestigationDetailsUI()
        {
            txtReport.Text = "";
            txtHeight.Text = "";
            txtBreast.Text = "";
            txtChest.Text = "";
            InvDate.SelectedDate = System.DateTime.Now;
            cmbInvestigation.SelectedValue = (long)0;

        }

        private void ClearUSGUI()
        {
            txtPV.Text = "";
            USGDate.SelectedDate = System.DateTime.Now;
            txtUSGReport.Text = "";
        }

        private void clearExaminationUI()
        {
            AncDate.SelectedDate = System.DateTime.Now;
            txtBP.Text = "";
            txtHeight.Text = "";
            txtOedema.Text = "";
            cmbDoctor.SelectedValue = (long)0;
            txtPregPeriod.Text = "";
            txtFHeight.Text = "";
            cmbFHS.SelectedValue = (long)0;
            cmbPreAndPosition.SelectedValue = (long)0;
            ProcTime.Value = System.DateTime.Now;
            txtRemarks.Text = "";

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

        //General Details
        #region Private Methods for GeneralDetails

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
            bizActionObj.ANCGeneralDetails.Date = (DateTime)StartDateAnc.SelectedDate;
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
            bizActionObj.ANCGeneralDetails.LMPDate = LMP.SelectedDate;
            bizActionObj.ANCGeneralDetails.EDDDate = EDD.SelectedDate;
            if (!String.IsNullOrEmpty(txtSpecialRemark.Text))
                bizActionObj.ANCGeneralDetails.SpecialRemark = txtSpecialRemark.Text;
            if (chkMale.IsChecked == true)
                bizActionObj.ANCGeneralDetails.Gender = 1;
            else
                bizActionObj.ANCGeneralDetails.Gender = 2;
            if (chkFreezeGeneralDetails.IsChecked == true)
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
                        FillANCDataGird();
                        CmdSave.IsEnabled = false;
                    }
                }
            };
            client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ViewANCGeneralDetails()
        {
            clsGetANCGeneralDetailsBizActionVO bizActionObj = new clsGetANCGeneralDetailsBizActionVO();
            bizActionObj.ANCGeneralDetails = new clsANCVO();
            bizActionObj.ANCGeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            bizActionObj.ANCGeneralDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            bizActionObj.ANCGeneralDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            if (CoupleDetails != null)
            {
                bizActionObj.ANCGeneralDetails.CoupleID = CoupleDetails.CoupleId;
                bizActionObj.ANCGeneralDetails.CoupleUnitID = CoupleDetails.CoupleUnitId;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails != null)
                    {
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.ID != null)
                            ANCId = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.ID;
                        
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.ANC_Code != null)
                            txtAncCode.Text = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.ANC_Code;
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.A != null)
                            txtA.Text = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.A;
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.G != null)
                            txtG.Text = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.G;
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.M != null)
                            txtM.Text = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.M;
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.L != null)
                            txtL.Text = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.L;
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.P != null)
                            txtP.Text = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.P;
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.SpecialRemark != null)
                            txtSpecialRemark.Text = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.SpecialRemark;
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.EDDDate != null)
                            EDD.SelectedDate = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.EDDDate;
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.LMPDate != null)
                            LMP.SelectedDate = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.LMPDate;
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.Date != null)
                        {
                            StartDateAnc.SelectedDate = ((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.Date;
                        }
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.Gender != null)
                        {
                            if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.Gender == 1)
                            {
                                chkMale.IsChecked = true;
                            }
                            else
                            {
                                chkFrmale.IsChecked = true;
                            }
                        }
                        if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.Isfreezed != null)
                        {
                            if (((clsGetANCGeneralDetailsBizActionVO)arg.Result).ANCGeneralDetails.Isfreezed == true)
                            {
                                chkFreezeGeneralDetails.IsChecked = true;
                            }
                        }
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
                if (SelectedANCDetails.Gender == 1)
                    chkMale.IsChecked = true;
                else
                    chkFrmale.IsChecked = true;

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

        #endregion

        // History
        #region Private Methods for ANC history

        private void AddUpdateANCHistory()
        {
            clsAddUpdateANCHistoryBizActionVO bizActionObj = new clsAddUpdateANCHistoryBizActionVO();
            bizActionObj.ANCHistory = new clsANCHistoryVO();
            if (IsModify == true)
                bizActionObj.ANCHistory.ID = ((clsANCVO)dgANCList.SelectedItem).ID;
            bizActionObj.ANCHistory.ANCID = ((clsANCVO)dgANCList.SelectedItem).ID;
            bizActionObj.ANCHistory.PatientID = ((clsANCVO)dgANCList.SelectedItem).PatientID;
            bizActionObj.ANCHistory.PatientUnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;

            bizActionObj.ANCHistory.CoupleID = CoupleDetails.CoupleId;
            bizActionObj.ANCHistory.CoupleUnitID = CoupleDetails.CoupleUnitId;
            bizActionObj.ANCHistory.LMPDate = DateLMP.SelectedDate;
            bizActionObj.ANCHistory.EDDDate = DateEDD.SelectedDate;
            if (rbdHypYes.IsChecked == true)
                bizActionObj.ANCHistory.Hypertension = true;
            if (rdbDiabetesYes.IsChecked == true)
                bizActionObj.ANCHistory.Diabeties = true;
            if (rdbCAYes.IsChecked == true)
                bizActionObj.ANCHistory.CongenitalAnomolies = true;
            if (rdbTBYes.IsChecked == true)
                bizActionObj.ANCHistory.TB = true;
            if (!String.IsNullOrEmpty(txtRemark.Text))
                bizActionObj.ANCHistory.PastRemark = txtRemark.Text;
            if (chkFirstOK.IsChecked == true)
                bizActionObj.ANCHistory.TTIstDose = true;
            else
                bizActionObj.ANCHistory.TTIstDose = false;
            if (chkSecondOk.IsChecked == true)
                bizActionObj.ANCHistory.TTIIstDose = true;
            else
                bizActionObj.ANCHistory.TTIIstDose = false;
            if (chkFirstOK.IsChecked == true && firstDoseDate.SelectedDate != null)
                bizActionObj.ANCHistory.DateIstDose = firstDoseDate.SelectedDate;
            if (chkSecondOk.IsChecked == true && secondDoseDate.SelectedDate != null)
                bizActionObj.ANCHistory.DateIIstDose = secondDoseDate.SelectedDate;
            if (chkFreezeHistory.IsChecked == true)
                bizActionObj.ANCHistory.Isfreezed = true;
            bizActionObj.ANCHistory.Status = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddUpdateANCHistoryBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("PALASH", "ANC History Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                        ClearHistoryUI();
                        objAnimation.Invoke(RotationType.Backward);
                        CmdSave.IsEnabled = false;
                    }
                    else if (((clsAddUpdateANCHistoryBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("PALASH", "ANC History Modified Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                        ClearHistoryUI();
                        objAnimation.Invoke(RotationType.Backward);
                        CmdSave.IsEnabled = false;
                    }
                }
            };
            client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ViewANChistory()
        {
            clsGetANCHistoryBizActionVO bizAction = new clsGetANCHistoryBizActionVO();
            bizAction.ANCHistory = new clsANCHistoryVO();
            bizAction.ANCHistory.ANCID = ((clsANCVO)dgANCList.SelectedItem).ID;
            ANCId = ((clsANCVO)dgANCList.SelectedItem).ID;
            bizAction.ANCHistory.UnitID = ((clsANCVO)dgANCList.SelectedItem).UnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetANCHistoryBizActionVO)arg.Result).SuccessStatus != 0)
                    {
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.LMPDate != null)
                            DateLMP.SelectedDate = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.LMPDate;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.EDDDate != null)
                            DateEDD.SelectedDate = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.EDDDate;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Hypertension != null)
                        {
                            if (Convert.ToInt32(((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Hypertension) == 1)
                            {
                                rbdHypYes.IsChecked = true;
                            }
                            else
                            {
                                rdbHypNo.IsChecked = true;
                            }
                        }
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.TB != null)
                        {
                            if (Convert.ToInt32(((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.TB) == 1)
                            {
                                rdbTBYes.IsChecked = true;
                            }
                            else
                            {
                                rdbTBNo.IsChecked = true;
                            }
                        }
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Diabeties != null)
                        {
                            if (Convert.ToInt32(((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Diabeties) == 1)
                            {
                                rdbDiabetesYes.IsChecked = true;
                            }
                            else
                            {
                                rdbDiabetesNo.IsChecked = true;
                            }
                        }
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.CongenitalAnomolies != null)
                        {
                            if (Convert.ToInt32(((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.CongenitalAnomolies) == 1)
                            {
                                rdbCAYes.IsChecked = true;
                            }
                            else
                            {
                                rdbCANo.IsChecked = true;
                            }
                        }
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.PastRemark != null)
                            txtRemark.Text = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.PastRemark;
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.DateIstDose != null)
                        {
                            chkFirstOK.IsChecked = true;
                            firstDoseDate.SelectedDate = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.DateIstDose;
                            firstDoseDate.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.DateIIstDose != null)
                        {
                            chkSecondOk.IsChecked = true;
                            secondDoseDate.SelectedDate = ((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.DateIIstDose;
                            secondDoseDate.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Isfreezed != null)
                        {
                            if (((clsGetANCHistoryBizActionVO)arg.Result).ANCHistory.Isfreezed == true)
                            {
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
                                    AddButton.IsEnabled = false;
                                    ModifyButton.IsEnabled = false;
                                    chkFreezeHistory.IsChecked = false;
                                    CmdModify.IsEnabled = false;
                                    CmdSave.IsEnabled = false;
                                }
                                else
                                {
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
                if (((clsGetANCHistoryBizActionVO)arg.Result).SuccessStatus == 0)
                    SetButtonState("New");
            };
            client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        // Obestical History
        #region private Methods For ObstetricHistory

        private void AddUpdateObstetricHistory()
        {
            clsAddUpdateObestricHistoryBizActionVO bizAction = new clsAddUpdateObestricHistoryBizActionVO();

            bizAction.ANCObstetricHistory = new clsANCObstetricHistoryVO();
            if (IsModify == true)
                bizAction.ANCObstetricHistory.ID = ((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).ID;
            bizAction.ANCObstetricHistory.HistoryID = ((clsANCVO)dgANCList.SelectedItem).ID;
            bizAction.ANCObstetricHistory.PatientID = ((clsANCVO)dgANCList.SelectedItem).PatientID;
            bizAction.ANCObstetricHistory.PatientUnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            bizAction.ANCObstetricHistory.UnitID = ((clsANCVO)dgANCList.SelectedItem).UnitID;
            if (!String.IsNullOrEmpty(txtMaturityComplication.Text))
                bizAction.ANCObstetricHistory.MaturityComplication = txtMaturityComplication.Text;
            if (!String.IsNullOrEmpty(txtObstrtricRemark.Text))
                bizAction.ANCObstetricHistory.ObstetricRemark = txtObstrtricRemark.Text;
            if (!String.IsNullOrEmpty(txtModeOfDelivery.Text))
                bizAction.ANCObstetricHistory.ModeOfDelivary = txtModeOfDelivery.Text;

            bizAction.ANCObstetricHistory.Isfreezed = true;
            bizAction.ANCObstetricHistory.Status = true;

             Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsAddUpdateObestricHistoryBizActionVO)args.Result).SuccessStatus == 1)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Obstetric History Added Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                GetObestricHistoryList();
                            }
                        };
                        msgWin.Show();
                    }
                    else if (((clsAddUpdateObestricHistoryBizActionVO)args.Result).SuccessStatus == 2)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Obstetric History Modified Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                GetObestricHistoryList();
                            }
                        };
                        msgWin.Show();
                    }
                }
            };
            client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void GetObestricHistoryList()
        {
            clsGetObstricHistoryListBizActionVO BizAction = new clsGetObstricHistoryListBizActionVO();
            BizAction.ANCObsetricHistoryList = new List<clsANCObstetricHistoryVO>();
            BizAction.ANCObstetricHistory.UnitID = ((clsANCVO)dgANCList.SelectedItem).UnitID;
            BizAction.ANCObstetricHistory.HistoryID = ((clsANCVO)dgANCList.SelectedItem).ID;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = obestricHistoryList.PageIndex * obestricHistoryList.PageSize;
            BizAction.MaximumRows = obestricHistoryList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    if (((clsGetObstricHistoryListBizActionVO)args.Result).ANCObsetricHistoryList != null)
                    {
                        clsGetObstricHistoryListBizActionVO result = args.Result as clsGetObstricHistoryListBizActionVO;

                        obestricHistoryList.TotalItemCount = result.TotalRows;
                        if (result.ANCObsetricHistoryList != null)
                        {
                            obestricHistoryList.Clear();
                            foreach (var item in result.ANCObsetricHistoryList)
                            {
                                obestricHistoryList.Add(item);
                            }
                            dgObestricHistory.ItemsSource = null;
                            dgObestricHistory.ItemsSource = obestricHistoryList;
                            AddButton.IsEnabled = true;
                            dataGrid2PagerObstricHistory.Source = null;
                            dataGrid2PagerObstricHistory.PageSize = BizAction.MaximumRows;
                            dataGrid2PagerObstricHistory.Source = obestricHistoryList;
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

        private void cmdViewObestricHistory_Click(object sender, RoutedEventArgs e)
        {
            ViewObestricHistory();
        }

        private void ViewObestricHistory()
        {
            IsModify = true;
            ModifyButton.IsEnabled = true;
            AddButton.IsEnabled = false;
            if (dgObestricHistory.SelectedItem != null)
            {
                if (((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).MaturityComplication != null)
                    txtMaturityComplication.Text = ((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).MaturityComplication;
                if (((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).ModeOfDelivary != null)
                    txtModeOfDelivery.Text = ((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).ModeOfDelivary;
                if (((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).ObstetricRemark != null)
                    txtObstrtricRemark.Text = ((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).ObstetricRemark;
            }
        }

        private void cmdDeleteObstricHistory_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are You Sure \n  You Want To Delete Obstric History Details?";
            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinDeleteObstericHistory_OnMessageBoxClosed);
            msgWin.Show();

        }
        void msgWinDeleteObstericHistory_OnMessageBoxClosed(MessageBoxResult result) 
        {
            if (result == MessageBoxResult.Yes)
            {
                if (IsClosed == false && chkFreezeHistory.IsChecked == false)
                {
                    clsDeleteObstericHistoryBizActionVO BizAction = new clsDeleteObstericHistoryBizActionVO();
                    BizAction.ANCObstetricHistory = new clsANCObstetricHistoryVO();
                    BizAction.ANCObstetricHistory.ID = ((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).ID;
                    BizAction.ANCObstetricHistory.UnitID  = ((clsANCObstetricHistoryVO)dgObestricHistory.SelectedItem).UnitID;
                    BizAction.ANCObstetricHistory.HistoryID = ANCId;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {

                            string msgTitle1 = "Palash";
                            string msgText1 = "Obsteric History Details Deleted Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWin1 =
                         new MessageBoxControl.MessageBoxChildWindow(msgTitle1, msgText1, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                            msgWin1.OnMessageBoxClosed += (result1) =>
                            {
                                if (result1 == MessageBoxResult.OK)
                                {
                                    GetObestricHistoryList();
                                }
                            };
                            msgWin1.Show();


                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }

        }
        #endregion

        // Investigation . . 
        #region Methods For Investigation
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
                        string msgTitle = "Palash";
                        string msgText = "Investigation Details Saved Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillInvestigationDetailsGrid();
                            }
                        };
                        msgWin.Show();
                        ClearInvestigationDetailsUI();
                        SetButtonState("New");
                    }
                    else if (((clsAddUpdateANCInvestigationDetailsBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Investigation Details Modified Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillInvestigationDetailsGrid();
                            }
                        };
                        msgWin.Show();
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
                       // investigationList.TotalItemCount = result.TotalRows;
                        if (result.ANCInvestigationDetailsList != null)
                        {
                            investigationList.Clear();
                            foreach (var item in result.ANCInvestigationDetailsList)
                            {
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

        private void cmdViewInvestigation_Click(object sender, RoutedEventArgs e)
        {
            ViewInvestigation();
        }

        private void ViewInvestigation()
        {
            if (dgInvestigation.SelectedItem != null)
            {
                IsModify = true;
                InvModifyButton.IsEnabled = true;
                InvAddButton.IsEnabled = false;
                if (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).Height != null)
                    txtHeight.Text = Convert.ToString(((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).Height);
                if (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).Breast != null)
                    txtBreast.Text = Convert.ToString(((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).Breast);
                if (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).Chest != null)
                    txtChest.Text = Convert.ToString(((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).Chest);
                if (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).Report != null)
                    txtReport.Text = ((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).Report;
                if (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).InvDate != null)
                    InvDate.SelectedDate = ((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).InvDate;
                if (((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).InvestigationID != null)
                    cmbInvestigation.SelectedValue = (long)((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).InvestigationID;
            }
        }

        private void InvAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (InvestigationValidations())
            {
                clsANCInvestigationDetailsVO INV = new clsANCInvestigationDetailsVO();
                if (((MasterListItem)cmbInvestigation.SelectedItem).ID != null)
                    INV.InvestigationID = ((MasterListItem)cmbInvestigation.SelectedItem).ID;
                if (!String.IsNullOrEmpty(txtReport.Text))
                    INV.Report = txtReport.Text;
                INV.InvDate = (DateTime)InvDate.SelectedDate;
                if (!String.IsNullOrEmpty(txtHeight.Text))
                    INV.Height = Convert.ToDecimal(txtHeight.Text);
                if (!String.IsNullOrEmpty(txtBreast.Text))
                    INV.Breast = Convert.ToDecimal(txtBreast.Text);
                if (!String.IsNullOrEmpty(txtChest.Text))
                    INV.Chest = Convert.ToDecimal(txtChest.Text);
                investigationList.Add(INV);
                dgInvestigation.ItemsSource = null;
                dgInvestigation.ItemsSource = investigationList;
                dgInvestigation.UpdateLayout();
                dgInvestigation.Focus();
                ClearInvestigationDetailsUI();
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
                                    item.InvestigationID = ((MasterListItem)cmbInvestigation.SelectedItem).ID;
                                if (!String.IsNullOrEmpty(txtReport.Text))
                                    item.Report = txtReport.Text;
                                item.InvDate = (DateTime)InvDate.SelectedDate;
                                if (!String.IsNullOrEmpty(txtHeight.Text))
                                    item.Height = Convert.ToDecimal(txtHeight.Text);
                                if (!String.IsNullOrEmpty(txtBreast.Text))
                                    item.Breast = Convert.ToDecimal(txtBreast.Text);
                                if (!String.IsNullOrEmpty(txtChest.Text))
                                    item.Chest = Convert.ToDecimal(txtChest.Text);
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

        private void cmdDeleteInvestigation_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are You Sure \n  You Want To Delete ANC Investigation?";
            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinDeleteInvestigation_OnMessageBoxClosed);
            msgWin.Show();

        }
        void msgWinDeleteInvestigation_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            { 
                 if (IsClosed == false && chkUSGInv.IsChecked == false)
                    {

                        clsDeleteANCInvestigationBizActionVO BizAction = new clsDeleteANCInvestigationBizActionVO();
                        BizAction.ANCInvestigationDetails = new clsANCInvestigationDetailsVO();
                        BizAction.ANCInvestigationDetails.ID = ((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).ID;
                        BizAction.ANCInvestigationDetails.UnitID = ((clsANCInvestigationDetailsVO)dgInvestigation.SelectedItem).UnitID;
                        BizAction.ANCInvestigationDetails.ANCID = ANCId;
                        

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                string msgTitle1 = "Palash";
                                string msgText1 = "Investigation Details Deleted Successfully.";
                                MessageBoxControl.MessageBoxChildWindow msgWin1 =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle1, msgText1, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                                msgWin1.OnMessageBoxClosed += (result1) =>
                                {
                                    if (result1 == MessageBoxResult.OK)
                                    {
                                        FillInvestigationDetailsGrid();
                                    }
                                };
                                msgWin1.Show();
                            }
                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
              }
        }
        #endregion

        // USG
        #region Methods for USGDetails
        private void AddUpdateUSGdetails()
        {
            clsAddUpdateUSGdetailsBizActionVO bizActionObj = new clsAddUpdateUSGdetailsBizActionVO();
            bizActionObj.ANCUSGDetails = new clsANCUSGDetailsVO();
            bizActionObj.ANCUSGDetailsList = new List<clsANCUSGDetailsVO>();
            bizActionObj.ANCUSGDetails.ANCID = ((clsANCVO)dgANCList.SelectedItem).ID;
            bizActionObj.ANCUSGDetails.CoupleID = CoupleDetails.CoupleId;
            bizActionObj.ANCUSGDetails.CoupleUnitID = CoupleDetails.CoupleUnitId;
            bizActionObj.ANCUSGDetails.PatientID = ((clsANCVO)dgANCList.SelectedItem).PatientID;
            bizActionObj.ANCUSGDetails.PatientUnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            foreach (var item in USGList) 
            {
                bizActionObj.ANCUSGDetailsList.Add(item);
            }

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
                        string msgText = "USG And Investigation Details Saved Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillUSGDataGrid();
                            }
                        };
                        msgWin.Show();
                        ClearUSGUI();
                        USGAddButton.IsEnabled = true;
                    }
                    else if (((clsAddUpdateUSGdetailsBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        string msgTitle = "Palash";
                        string msgText = "USG And Investigation Details Modified Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillUSGDataGrid();
                            }
                        };
                        msgWin.Show();
                        ClearUSGUI();
                        USGAddButton.IsEnabled = true;
                        USGModifyButton.IsEnabled = false;
                    }
                }
            };
            client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ViewUSGDetails()
        {
            if (dgUSGDetails.SelectedItem != null)
            {
                IsModify = true;
                USGModifyButton.IsEnabled = true;
                USGAddButton.IsEnabled = false;
                if (((clsANCUSGDetailsVO)dgUSGDetails.SelectedItem).PV != null)
                    txtPV.Text = ((clsANCUSGDetailsVO)dgUSGDetails.SelectedItem).PV;
                if (((clsANCUSGDetailsVO)dgUSGDetails.SelectedItem).Report != null)
                    txtUSGReport.Text = ((clsANCUSGDetailsVO)dgUSGDetails.SelectedItem).Report;
                if (((clsANCUSGDetailsVO)dgUSGDetails.SelectedItem).USGDate != null)
                    USGDate.SelectedDate = ((clsANCUSGDetailsVO)dgUSGDetails.SelectedItem).USGDate;
            }
        }

        private void FillUSGDataGrid()
        {
            clsGetANCUSGListBizActionVO BizAction = new clsGetANCUSGListBizActionVO();
            BizAction.ANCUSGDetailsList = new List<clsANCUSGDetailsVO>();
            BizAction.ANCUSGDetails.UnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            BizAction.ANCUSGDetails.ANCID = ANCId;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = USGList.PageIndex * USGList.PageSize;
            BizAction.MaximumRows = USGList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetANCUSGListBizActionVO)args.Result).ANCUSGDetailsList != null)
                    {
                        clsGetANCUSGListBizActionVO result = args.Result as clsGetANCUSGListBizActionVO;

                        USGList.TotalItemCount = result.TotalRows;
                        if (result.ANCUSGDetailsList != null)
                        {
                            USGList.Clear();
                            foreach (var item in result.ANCUSGDetailsList)
                            {
                                USGList.Add(item);
                            }
                            dgUSGDetails.ItemsSource = null;
                            dgUSGDetails.ItemsSource = USGList;

                            dataGridPagerUsgDetails.Source = null;
                            dataGridPagerUsgDetails.PageSize = BizAction.MaximumRows;
                            dataGridPagerUsgDetails.Source = USGList;
                            if (USGList.Count > 0)
                            {
                                if (USGList[USGList.Count - 1].Isfreezed == true)
                                {
                                    chkUSGInv.IsChecked = true;
                                    SetButtonState("New");
                                    chkUSGInv.IsEnabled = false;
                                    USGModifyButton.IsEnabled = false;
                                    USGAddButton.IsEnabled = false;
                                    InvModifyButton.IsEnabled = false;
                                    InvAddButton.IsEnabled = false;
                                    CmdSave.IsEnabled = false;
                                    CmdModify.IsEnabled = false;

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

        private void cmdViewUSG_Click(object sender, RoutedEventArgs e)
        {
            ViewUSGDetails();
        }

        private void USGAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (USGValidations())
            {
                clsANCUSGDetailsVO USG = new clsANCUSGDetailsVO();
                if (!String.IsNullOrEmpty(txtPV.Text))
                    USG.PV = txtPV.Text;
                if (!String.IsNullOrEmpty(txtUSGReport.Text))
                    USG.Report = txtUSGReport.Text;
                if (USGDate.SelectedDate != null)
                    USG.USGDate = USGDate.SelectedDate;
                USGList.Add(USG);
                dgUSGDetails.ItemsSource = null;
                dgUSGDetails.ItemsSource = USGList; 
                dgUSGDetails.UpdateLayout();
                dgUSGDetails.Focus();
                ClearUSGUI();
                
            } 
        }
        
        private void USGModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (USGValidations())
            {

                if (USGList.Count > 0)
                {
                    foreach (var item in USGList)
                    {
                        if (item.ID == (((clsANCUSGDetailsVO)dgUSGDetails.SelectedItem).ID))
                        {
                            clsANCUSGDetailsVO USG = new clsANCUSGDetailsVO();
                            if (!String.IsNullOrEmpty(txtPV.Text))
                                item.PV = txtPV.Text;
                            if (!String.IsNullOrEmpty(txtUSGReport.Text))
                                item.Report = txtUSGReport.Text;
                            if (USGDate.SelectedDate != null)
                                item.USGDate = USGDate.SelectedDate;
                        }
                    }
                    dgUSGDetails.ItemsSource = null;
                    dgUSGDetails.ItemsSource = USGList;
                    dgUSGDetails.Focus();
                    dgUSGDetails.UpdateLayout();
                    dgUSGDetails.SelectedIndex = USGList.Count - 1;
                    ClearUSGUI();
                }
      
            }
        }

        private void cmdDeleteUSG_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are You Sure \n  You Want To Delete ANC USG Details?";
            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinDeleteUSG_OnMessageBoxClosed);
            msgWin.Show();

            

        }
        void msgWinDeleteUSG_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (IsClosed == false && chkUSGInv.IsChecked == false)
                {


                    clsDeleteANCUSGBizActionVO BizAction = new clsDeleteANCUSGBizActionVO();
                    BizAction.ANCUSGDetails = new clsANCUSGDetailsVO();
                    BizAction.ANCUSGDetails.ID = ((clsANCUSGDetailsVO)dgUSGDetails.SelectedItem).ID;
                    BizAction.ANCUSGDetails.ANCID = ANCId;
                    BizAction.ANCUSGDetails.UnitID = ((clsANCUSGDetailsVO)dgUSGDetails.SelectedItem).ID;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {

                            string msgTitle1 = "Palash";
                            string msgText1 = "USG Details Deleted Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWin1 =
                         new MessageBoxControl.MessageBoxChildWindow(msgTitle1, msgText1, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                            msgWin1.OnMessageBoxClosed += (result1) =>
                            {
                                if (result1 == MessageBoxResult.OK)
                                {
                                    FillUSGDataGrid();
                                }
                            };
                            msgWin1.Show();


                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
        }

        #endregion

        // Examination
        #region Methods for Examination
        private void AddUpdateExamination() 
        {
            AddUpdateANCExaminationBizActionVO bizActionObj = new AddUpdateANCExaminationBizActionVO();
            bizActionObj.ANCExaminationDetails = new clsANCExaminationDetailsVO();
            bizActionObj.ANCExaminationDetailsList = new List<clsANCExaminationDetailsVO>();
            if (IsModify == true)
                bizActionObj.ANCExaminationDetails.ID = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).ID;
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
            BizAction.ANCExaminationDetails.UnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
            BizAction.ANCExaminationDetails.ANCID =ANCId;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = 0 ;
            BizAction.MaximumRows =15;
         
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
                                    chkFreezeExamination.IsChecked = true;
                                    CmdModify.IsEnabled = false;
                                    CmdSave.IsEnabled = false;
                                    AncAddButton.IsEnabled = false;
                                    AncModifyButton.IsEnabled = false;
                                }
                                else
                                {
                                    if (IsClosed== true)
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


        private void ViewANCExamination() 
        {
            if (grdResultEntry.SelectedItem != null) 
            {
                IsModify = true;
                AncModifyButton.IsEnabled = true;
                AncAddButton.IsEnabled=false;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).BP != null)
                    txtBP.Text = Convert.ToString(((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).BP);
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Weight != null)
                    txtWeight.Text = Convert.ToString(((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Weight);
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).FundalHeight != null)
                    txtFHeight.Text = Convert.ToString(((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).FundalHeight);
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
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).ExaTime!= null)
                    ProcTime .Value = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).ExaTime;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Oadema != null)
                    txtOedema.Text = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).Oadema;
                if (((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).PeriodOfPreg != null)
                    txtPregPeriod.Text = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).PeriodOfPreg;
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
                    Examination.PresenAndPos = ((MasterListItem)cmbPreAndPosition.SelectedItem).ID;
                if (((MasterListItem)cmbFHS.SelectedItem).ID != null)
                    Examination.FHS = ((MasterListItem)cmbFHS.SelectedItem).ID;
                if (((MasterListItem)cmbDoctor.SelectedItem).ID != null)
                    Examination.Doctor = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                if (!String.IsNullOrEmpty(txtPregPeriod.Text))
                    Examination.PeriodOfPreg = txtPregPeriod.Text.Trim();
                if (!String.IsNullOrEmpty(txtBP.Text))
                    Examination.BP = Convert.ToSingle(txtBP.Text);
                if (!String.IsNullOrEmpty(txtWeight.Text))
                    Examination.Weight = Convert.ToSingle(txtWeight.Text);
                if (!String.IsNullOrEmpty(txtFHeight.Text))
                    Examination.FundalHeight = Convert.ToSingle(txtFHeight.Text);
                if (!String.IsNullOrEmpty(txtOedema.Text))
                    Examination.Oadema = txtOedema.Text.Trim();
                if (!String.IsNullOrEmpty(txtRemarks.Text))
                    Examination.Remark = txtRemarks.Text.Trim();
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
                                item.PresenAndPos = ((MasterListItem)cmbPreAndPosition.SelectedItem).ID;
                            if (((MasterListItem)cmbFHS.SelectedItem).ID != null)
                                item.FHS = ((MasterListItem)cmbFHS.SelectedItem).ID;
                            if (((MasterListItem)cmbDoctor.SelectedItem).ID != null)
                                item.Doctor = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                            if (!String.IsNullOrEmpty(txtPregPeriod.Text))
                                item.PeriodOfPreg = txtPregPeriod.Text.Trim();
                            if (!String.IsNullOrEmpty(txtBP.Text))
                                item.BP = Convert.ToSingle(txtBP.Text);
                            if (!String.IsNullOrEmpty(txtWeight.Text))
                                item.Weight = Convert.ToSingle(txtWeight.Text);
                            if (!String.IsNullOrEmpty(txtFHeight.Text))
                                item.FundalHeight = Convert.ToSingle(txtFHeight.Text);
                            if (!String.IsNullOrEmpty(txtOedema.Text))
                                item.Oadema = txtOedema.Text.Trim();
                            if (!String.IsNullOrEmpty(txtRemarks.Text))
                                item.Remark = txtRemarks.Text.Trim();
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
      
        private void cmdDeleteExamination_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
                        string msgText = "Are You Sure \n  You Want To Delete ANC Examination?";
                        MessageBoxControl.MessageBoxChildWindow msgWin =

                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinDeleteExamination_OnMessageBoxClosed);
                        msgWin.Show();
        }

        void msgWinDeleteExamination_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (IsClosed == false && chkFreezeExamination.IsChecked == false)
                {
                    clsDeleteANCExaminationBizActionVO BizAction = new clsDeleteANCExaminationBizActionVO();
                    BizAction.ANCExaminationDetails = new clsANCExaminationDetailsVO();
                    BizAction.ANCExaminationDetails.ID = ((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).ID;
                    BizAction.ANCExaminationDetails.ANCID = ANCId;
                   
                    BizAction.ANCExaminationDetails.UnitID =((clsANCExaminationDetailsVO)grdResultEntry.SelectedItem).UnitID;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                   
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            string msgTitle1 = "Palash";
                            string msgText1 = "Examination Details Deleted Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWin1 =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle1, msgText1, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                            msgWin1.OnMessageBoxClosed += (result1) =>
                            {
                                if (result1 == MessageBoxResult.OK)
                                {
                                    FillExaminationGrid();
                                }
                            };
                            msgWin1.Show();


                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
        }
        #endregion

        // Document
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
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

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
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

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
            BizAction.ANCDocument.ANCID= ANCId;
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
        #endregion

        // Suggestion
        #region Suggestion
        private void AddUpdateANCSuggestion()
        {
            AddUpdateANCSuggestionBizActionVO BizAction = new AddUpdateANCSuggestionBizActionVO();
            BizAction.ANCSuggestion = new clsANCSuggestionVO();
            if (IsModify == true)
            {
                BizAction.ANCSuggestion.ID = SuggID;
              
            }
            BizAction.ANCSuggestion.ANCID = ANCId;
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
                        objAnimation.Invoke(RotationType.Backward);
                        FillANCDataGird();
                       SetButtonState("New");
                    }
                    else if (((AddUpdateANCSuggestionBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("PALASH", "ANC Suggestion Modified Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                        ClearSuggestionUI();
                        objAnimation.Invoke(RotationType.Backward);
                        FillANCDataGird();
                        SetButtonState("Modify");
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillSuggestionDetails() 
        {
            GetANCSuggestionBizActionVO bizActionObj = new GetANCSuggestionBizActionVO();
            bizActionObj.ANCSuggestion = new clsANCSuggestionVO();
            bizActionObj.ANCSuggestion.UnitID = ((clsANCVO)dgANCList.SelectedItem).PatientUnitID;
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

        #endregion

        #region Tab Selection Changed

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (History != null)
            {
                if (History.IsSelected)
                {
                    clearObstericHistory();

                    IsModify = false;
                    GetObestricHistoryList();
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
                    USGAddButton.IsEnabled = true;
                    USGModifyButton.IsEnabled = false;
                    FillUSGDataGrid();

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

        }
        #endregion        

        #region Freeze click events
        private void chkFreezeGeneralDetails_Click(object sender, RoutedEventArgs e)
        {
            if (chkFreezeGeneralDetails.IsChecked == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Freeze The Details?";
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
            else 
            {
                chkFreezeGeneralDetails.IsChecked = false;
            }
        }

        private void chkFreezeHistory_Click(object sender, RoutedEventArgs e)
        {
            if (chkFreezeHistory.IsChecked == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Freeze  The Details?";
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
            else
            {
                chkFreezeHistory.IsChecked = false;
            }
        }

        private void chkUSGInv_Click(object sender, RoutedEventArgs e)
        {
            if (chkUSGInv.IsChecked == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Freeze  The Details?";
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

            }
            else
            {
                chkUSGInv.IsChecked = false;
            }
        }

        private void chkFreezeExamination_Click(object sender, RoutedEventArgs e)
        {
            if (chkFreezeExamination.IsChecked == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Freeze  The Details?";
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
                chkFreezeExamination.IsChecked = false;
            }
        }

        private void chkDocument_Click(object sender, RoutedEventArgs e)
        {
            if (chkDocument.IsChecked == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Freeze The Details ?";
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
                chkDocument.IsChecked = false;
            }
        }

        private void chkSuggestion_Click(object sender, RoutedEventArgs e)
        {
            if (chkSuggestion.IsChecked == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Freeze The Details?";
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
            else
            {
                chkSuggestion.IsChecked = false;
            }
        }
        #endregion

        #region Print
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgANCList.SelectedItem != null)
            {
                CoupleID = ((clsANCVO)dgANCList.SelectedItem).CoupleID;
                CoupleUnitID = ((clsANCVO)dgANCList.SelectedItem).CoupleUnitID;
                AncID = ((clsANCVO)dgANCList.SelectedItem).ID;
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
                string URL = "../Reports/ANC/ANC_Report.aspx?CoupleID=" + CoupleID + "&CoupleUnitID=" + CoupleUnitID + "&AncID=" + AncID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
        #endregion 

        private void dgANCList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void chkIsClosed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkMale_Checked(object sender, RoutedEventArgs e)
        {
            if (chkMale.IsChecked == true)
                chkFrmale.IsChecked = false;
        }

        private void chkFrmale_Checked(object sender, RoutedEventArgs e)
        {
            if (chkFrmale.IsChecked == true)
                chkMale.IsChecked = false;
        }
    }
}
