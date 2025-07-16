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
//using DataDrivenApplication;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Controls.Primitives;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.UserControls;
using PalashDynamics.Controls;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using MessageBoxControl;
using System.Collections.ObjectModel;
using PalashDynamics.IVF.PatientList;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

//using DataDrivenApplication.Forms;




namespace PalashDynamics.IVF
{
    public partial class FemaleLabDay0 : UserControl, IInitiateCIMS
    {
        public byte[] MalePhoto { get; set; }
        public byte[] FemalePhoto { get; set; }
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;
            }
        }

        #endregion

        public FemaleLabDay0()
        {
            try
            {
                InitializeComponent();
                this.DataContext = new clsFemaleLabDay0VO();
                this.Unloaded += new RoutedEventHandler(FemaleLabDay0_Unloaded);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FemaleLabDay0(clsCoupleVO Couple, clsLabDaysSummaryVO Summary)
        {
            try
            {
                InitializeComponent();
                IsSaved = true;
                IsPatientExist = true;
                CoupleDetails = Couple;
                this.DataContext = new clsFemaleLabDay0VO() { ID = Summary.OocyteID, UnitID = Summary.UnitID, CoupleID = CoupleDetails.CoupleId, CoupleUnitID = CoupleDetails.CoupleUnitId, LabDaySummary = Summary };
                this.Unloaded += new RoutedEventHandler(FemaleLabDay0_Unloaded);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void FemaleLabDay0_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                client.GlobalDeleteFileCompleted += (s1, args1) =>
                {
                    if (args1.Error == null)
                    {
                    }
                };
                client.GlobalDeleteFileAsync("../UserUploadedFilesByTemplateTool", listOfReports);
            }
            catch (Exception ex) { throw ex; }
        }

        public bool IsPatientExist = false;
        public bool IsPageLoded = false;
        public bool IsSaved = false;
        WaitIndicator wi = new WaitIndicator();
        public bool IsUpdate = false;

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
        public List<IVFTreatment> IVFSetting { get; set; }
        public List<ICSITreatment> ICSISetting { get; set; }
        public List<PalashDynamics.ValueObjects.IVFPlanTherapy.FileUpload> FUSetting { get; set; }
        private System.Collections.ObjectModel.ObservableCollection<string> listOfReports = new System.Collections.ObjectModel.ObservableCollection<string>();
        public List<MasterListItem> Cumulus { get; set; }
        public List<MasterListItem> Grade { get; set; }
        public List<MasterListItem> MOI { get; set; }
        public List<MasterListItem> Plan { get; set; }
        public List<MasterListItem> DOS { get; set; }
        public List<MasterListItem> PIC { get; set; }
        ListBox lstIVFBox;
        ListBox lstICSIBox;
        ListBox lstFUBox;
        clsFemaleSemenDetailsVO SemenDetails = new clsFemaleSemenDetailsVO();
        long SourceOfSemen { get; set; }
        long MethodOfSpermPreparation { get; set; }

        #endregion

        private void LoadIVFRepeaterControl()
        {
            lstIVFBox = new ListBox();

            if (IsSaved == false)
                IVFSetting = ((clsFemaleLabDay0VO)this.DataContext).IVFSetting = new List<IVFTreatment>();
            else
                IVFSetting = ((clsFemaleLabDay0VO)this.DataContext).IVFSetting;
            lstIVFBox.DataContext = ((clsFemaleLabDay0VO)this.DataContext).IVFSetting;
            if (IsSaved == false || ((clsFemaleLabDay0VO)this.DataContext).IVFSetting.Count == 0)
                IVFSetting.Add(new IVFTreatment() { CumulusSource = Cumulus, GradeSource = Grade, MOISource = MOI, PlanSource = Plan });

            for (int i = 0; i < IVFSetting.Count; i++)
            {
                IVFSetting[i].CumulusSource = Cumulus;
                IVFSetting[i].GradeSource = Grade;
                IVFSetting[i].MOISource = MOI;
                IVFSetting[i].PlanSource = Plan;
               

                IVFTreatmentPlanRepeterControlItem IVFTPrci = new IVFTreatmentPlanRepeterControlItem();
                IVFTPrci.OnAddRemoveClick += new RoutedEventHandler(IVFTPrci_OnAddRemoveClick);
                IVFTPrci.OnCmdMediaDetailClick += new RoutedEventHandler(IVFTPrci_OnCmdMediaDetailClick);
                IVFTPrci.OnchkProceedToDayClick += new RoutedEventHandler(IVFTPrci_OnchkProceedToDayClick);
                IVFTPrci.OnSelectionChanged += new SelectionChangedEventHandler(IVFTPrci_OnSelectionChanged);
                IVFTPrci.OnViewClick += new RoutedEventHandler(IVFTPrci_OnViewClick);
                IVFTPrci.OnBrowseClick += new RoutedEventHandler(IVFTPrci_OnBrowseClick);

                IVFSetting[i].Index = i;
                IVFSetting[i].Command = ((i == IVFSetting.Count - 1) ? "Add" : "Remove");
                IVFTPrci.DataContext = IVFSetting[i];
                lstIVFBox.Items.Add(IVFTPrci);
            }
            Grid.SetRow(lstIVFBox, 0);
            Grid.SetColumn(lstIVFBox, 0);
            IVFRepeater.Children.Add(lstIVFBox);
            txtTotOocytes.Text = lstIVFBox.Items.Count.ToString();
        }

        void IVFTPrci_OnBrowseClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                IVFSetting[(((IVFTreatment)((ToggleButton)sender).DataContext).Index)].FileName = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        IVFSetting[(((IVFTreatment)((ToggleButton)sender).DataContext).Index)].FileContents = new byte[stream.Length];
                        stream.Read(IVFSetting[(((IVFTreatment)((ToggleButton)sender).DataContext).Index)].FileContents, 0, (int)stream.Length);
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error While Reading File.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        void IVFTPrci_OnViewClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IVFSetting[(((IVFTreatment)((HyperlinkButton)sender).DataContext).Index)].FileName))
            {
                if (IVFSetting[(((IVFTreatment)((HyperlinkButton)sender).DataContext).Index)].FileContents != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + IVFSetting[(((IVFTreatment)((HyperlinkButton)sender).DataContext).Index)].FileName });
                            listOfReports.Add(IVFSetting[(((IVFTreatment)((HyperlinkButton)sender).DataContext).Index)].FileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", IVFSetting[(((IVFTreatment)((HyperlinkButton)sender).DataContext).Index)].FileName, IVFSetting[(((IVFTreatment)((HyperlinkButton)sender).DataContext).Index)].FileContents);
                }
            }
        }

        void IVFTPrci_OnCmdMediaDetailClick(object sender, RoutedEventArgs e)
        {
            MediaDetails Win = new MediaDetails();
            if (((IVFTreatment)((HyperlinkButton)sender).DataContext).MediaDetails == null)
                ((IVFTreatment)((HyperlinkButton)sender).DataContext).MediaDetails = new List<clsFemaleMediaDetailsVO>();
            Win.ItemList = GetCollection(((IVFTreatment)((HyperlinkButton)sender).DataContext).MediaDetails);
            Win.Tag = ((IVFTreatment)((HyperlinkButton)sender).DataContext).MediaDetails;
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.Show();
        }

        void IVFTPrci_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteComboBox)sender).SelectedItem != null)
            {
                if ((string)((AutoCompleteComboBox)sender).Tag == "Cumulus")
                {
                    ((IVFTreatment)((AutoCompleteComboBox)sender).DataContext).Cumulus = (MasterListItem)((AutoCompleteComboBox)sender).SelectedItem;
                }
                else if ((string)((AutoCompleteComboBox)sender).Tag == "Grade")
                {
                    ((IVFTreatment)((AutoCompleteComboBox)sender).DataContext).Grade = (MasterListItem)((AutoCompleteComboBox)sender).SelectedItem;
                }
                else if ((string)((AutoCompleteComboBox)sender).Tag == "MOI")
                {
                    ((IVFTreatment)((AutoCompleteComboBox)sender).DataContext).MOI = (MasterListItem)((AutoCompleteComboBox)sender).SelectedItem;
                }
                else if ((string)((AutoCompleteComboBox)sender).Tag == "Plan")
                {
                    ((IVFTreatment)((AutoCompleteComboBox)sender).DataContext).Plan = (MasterListItem)((AutoCompleteComboBox)sender).SelectedItem;
                }
            }
        }

        void IVFTPrci_OnchkProceedToDayClick(object sender, RoutedEventArgs e)
        {
        }

        void IVFTPrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                IVFSetting.RemoveAt(((IVFTreatment)((HyperlinkButton)sender).DataContext).Index);
            }
            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                IVFSetting.Add(new IVFTreatment() { CumulusSource = Cumulus, GradeSource = Grade, MOISource = MOI, PlanSource = Plan });
            }
            lstIVFBox.Items.Clear();
            for (int i = 0; i < IVFSetting.Count; i++)
            {
                IVFTreatmentPlanRepeterControlItem IVFTPrci = new IVFTreatmentPlanRepeterControlItem();
                IVFTPrci.OnAddRemoveClick += new RoutedEventHandler(IVFTPrci_OnAddRemoveClick);
                IVFTPrci.OnCmdMediaDetailClick += new RoutedEventHandler(IVFTPrci_OnCmdMediaDetailClick);
                IVFTPrci.OnchkProceedToDayClick += new RoutedEventHandler(IVFTPrci_OnchkProceedToDayClick);
                IVFTPrci.OnSelectionChanged += new SelectionChangedEventHandler(IVFTPrci_OnSelectionChanged);
                IVFTPrci.OnViewClick += new RoutedEventHandler(IVFTPrci_OnViewClick);
                IVFTPrci.OnBrowseClick += new RoutedEventHandler(IVFTPrci_OnBrowseClick);

                IVFSetting[i].Index = i;
                IVFSetting[i].Command = ((i == IVFSetting.Count - 1) ? "Add" : "Remove");
                IVFTPrci.DataContext = IVFSetting[i];
                lstIVFBox.Items.Add(IVFTPrci);
                txtTotOocytes.Text = lstIVFBox.Items.Count.ToString();
            }
        }

        private void LoadICSIRepeaterControl()
        {
            lstICSIBox = new ListBox();

            if (IsSaved == false)
                ICSISetting = ((clsFemaleLabDay0VO)this.DataContext).ICSISetting = new List<ICSITreatment>();
            else
                ICSISetting = ((clsFemaleLabDay0VO)this.DataContext).ICSISetting;

            lstICSIBox.DataContext = ((clsFemaleLabDay0VO)this.DataContext).ICSISetting;
            if (IsSaved == false || ((clsFemaleLabDay0VO)this.DataContext).ICSISetting.Count == 0)
                ICSISetting.Add(new ICSITreatment() { DOSSource = DOS, PICSource = PIC, PlanSource = Plan });
            for (int i = 0; i < ICSISetting.Count; i++)
            {
                ICSISetting[i].DOSSource = DOS;
                ICSISetting[i].PICSource = PIC;
                ICSISetting[i].PlanSource = Plan;
                ICSITreatmentPlanRepeterControlItem ICSITPrci = new ICSITreatmentPlanRepeterControlItem();
                ICSITPrci.OnAddRemoveClick += new RoutedEventHandler(ICSITPrci_OnAddRemoveClick);
                ICSITPrci.OnCmdMediaDetailClick += new RoutedEventHandler(ICSITPrci_OnCmdMediaDetailClick);
                ICSITPrci.OnchkProceedToDayClick += new RoutedEventHandler(ICSITPrci_OnchkProceedToDayClick);
                ICSITPrci.OnSelectionChanged += new SelectionChangedEventHandler(ICSITPrci_OnSelectionChanged);
                ICSITPrci.OnViewClick += new RoutedEventHandler(ICSITPrci_OnViewClick);
                ICSITPrci.OnBrowseClick += new RoutedEventHandler(ICSITPrci_OnBrowseClick);
                ICSISetting[i].Index = i;
                ICSISetting[i].Command = ((i == ICSISetting.Count - 1) ? "Add" : "Remove");
                ICSITPrci.DataContext = ICSISetting[i];
                lstICSIBox.Items.Add(ICSITPrci);
            }
            Grid.SetRow(lstICSIBox, 0);
            Grid.SetColumn(lstICSIBox, 0);
            ICSIRepeater.Children.Add(lstICSIBox);
            txtTotOocytesICSI.Text = lstICSIBox.Items.Count.ToString();
        }

        void ICSITPrci_OnBrowseClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                ICSISetting[(((ICSITreatment)((ToggleButton)sender).DataContext).Index)].FileName = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        ICSISetting[(((ICSITreatment)((ToggleButton)sender).DataContext).Index)].FileContents = new byte[stream.Length];
                        stream.Read(ICSISetting[(((ICSITreatment)((ToggleButton)sender).DataContext).Index)].FileContents, 0, (int)stream.Length);
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error While Reading File.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    throw ex;
                }
            }
        }

        void ICSITPrci_OnViewClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ICSISetting[(((ICSITreatment)((HyperlinkButton)sender).DataContext).Index)].FileName))
            {
                if (ICSISetting[(((ICSITreatment)((HyperlinkButton)sender).DataContext).Index)].FileContents != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ICSISetting[(((ICSITreatment)((HyperlinkButton)sender).DataContext).Index)].FileName });
                            listOfReports.Add(ICSISetting[(((ICSITreatment)((HyperlinkButton)sender).DataContext).Index)].FileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ICSISetting[(((ICSITreatment)((HyperlinkButton)sender).DataContext).Index)].FileName, ICSISetting[(((ICSITreatment)((HyperlinkButton)sender).DataContext).Index)].FileContents);
                }
            }
        }

        void ICSITPrci_OnCmdMediaDetailClick(object sender, RoutedEventArgs e)
        {
            MediaDetails Win = new MediaDetails();
            if (((ICSITreatment)((HyperlinkButton)sender).DataContext).MediaDetails == null)
                ((ICSITreatment)((HyperlinkButton)sender).DataContext).MediaDetails = new List<clsFemaleMediaDetailsVO>();
            Win.ItemList = GetCollection(((ICSITreatment)((HyperlinkButton)sender).DataContext).MediaDetails);
            Win.Tag = ((ICSITreatment)((HyperlinkButton)sender).DataContext).MediaDetails;
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.Show();
        }

        private ObservableCollection<clsFemaleMediaDetailsVO> GetCollection(List<clsFemaleMediaDetailsVO> list)
        {
            ObservableCollection<clsFemaleMediaDetailsVO> ob = new ObservableCollection<clsFemaleMediaDetailsVO>();
            foreach (var i in list)
            {
                ob.Add(i);
            }
            return ob;
        }

        void ICSITPrci_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteComboBox)sender).SelectedItem != null)
            {
                if ((string)((AutoCompleteComboBox)sender).Tag == "DOS")
                {
                    ((ICSITreatment)((AutoCompleteComboBox)sender).DataContext).DOS = (MasterListItem)((AutoCompleteComboBox)sender).SelectedItem;
                }
                else if ((string)((AutoCompleteComboBox)sender).Tag == "PIC")
                {
                    ((ICSITreatment)((AutoCompleteComboBox)sender).DataContext).PIC = (MasterListItem)((AutoCompleteComboBox)sender).SelectedItem;
                }
                else if ((string)((AutoCompleteComboBox)sender).Tag == "Plan")
                {
                    ((ICSITreatment)((AutoCompleteComboBox)sender).DataContext).Plan = (MasterListItem)((AutoCompleteComboBox)sender).SelectedItem;
                }
            }
        }

        void ICSITPrci_OnchkProceedToDayClick(object sender, RoutedEventArgs e)
        {
        }

        void ICSITPrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                ICSISetting.RemoveAt(((ICSITreatment)((HyperlinkButton)sender).DataContext).Index);
            }
            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                ICSISetting.Add(new ICSITreatment() { DOSSource = DOS, PICSource = PIC, PlanSource = Plan });
            }
            lstICSIBox.Items.Clear();
            for (int i = 0; i < ICSISetting.Count; i++)
            {
                ICSITreatmentPlanRepeterControlItem ICSITPrci = new ICSITreatmentPlanRepeterControlItem();
                ICSITPrci.OnAddRemoveClick += new RoutedEventHandler(ICSITPrci_OnAddRemoveClick);
                ICSITPrci.OnCmdMediaDetailClick += new RoutedEventHandler(ICSITPrci_OnCmdMediaDetailClick);
                ICSITPrci.OnchkProceedToDayClick += new RoutedEventHandler(ICSITPrci_OnchkProceedToDayClick);
                ICSITPrci.OnSelectionChanged += new SelectionChangedEventHandler(ICSITPrci_OnSelectionChanged);
                ICSITPrci.OnViewClick += new RoutedEventHandler(ICSITPrci_OnViewClick);
                ICSITPrci.OnBrowseClick += new RoutedEventHandler(ICSITPrci_OnBrowseClick);
                ICSISetting[i].Index = i;
                ICSISetting[i].Command = ((i == ICSISetting.Count - 1) ? "Add" : "Remove");
                ICSITPrci.DataContext = ICSISetting[i];
                lstICSIBox.Items.Add(ICSITPrci);
                txtTotOocytesICSI.Text = lstICSIBox.Items.Count.ToString();
            }
        }

        private void LoadFURepeaterControl()
        {
            lstFUBox = new ListBox();

            if (IsSaved == false)
                FUSetting = ((clsFemaleLabDay0VO)this.DataContext).FUSetting = new List<PalashDynamics.ValueObjects.IVFPlanTherapy.FileUpload>();
            else
                FUSetting = ((clsFemaleLabDay0VO)this.DataContext).FUSetting;
            lstFUBox.DataContext = ((clsFemaleLabDay0VO)this.DataContext).FUSetting;
            if (IsSaved == false || ((clsFemaleLabDay0VO)this.DataContext).FUSetting.Count == 0)
                FUSetting.Add(new PalashDynamics.ValueObjects.IVFPlanTherapy.FileUpload());
            for (int i = 0; i < FUSetting.Count; i++)
            {
                FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
                FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);
                FUrci.OnBrowseClick += new RoutedEventHandler(FUrci_OnBrowseClick);
                FUrci.OnViewClick += new RoutedEventHandler(FUrci_OnViewClick);
                FUSetting[i].Index = i;
                FUSetting[i].Command = ((i == FUSetting.Count - 1) ? "Add" : "Remove");
                FUrci.DataContext = FUSetting[i];
                lstFUBox.Items.Add(FUrci);
            }
            Grid.SetRow(lstFUBox, 0);
            Grid.SetColumn(lstFUBox, 0);
            GridUploadFile.Children.Add(lstFUBox);
        }

        void FUrci_OnViewClick(object sender, RoutedEventArgs e)
        {
            if (((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName != null && ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName != "")
            {
                if (((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Data != null)
                {
                    string FullFile = "Lab Day0 " + ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Index + ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName;

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FullFile });
                            listOfReports.Add(FullFile);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", FullFile, ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Data);
                }
                else
                {
                    //clsGetPatientEMRUploadedFilesBizActionVO BizActionFU = new clsGetPatientEMRUploadedFilesBizActionVO();
                    //BizActionFU.ControlName = ((FieldDetail)lstBox.DataContext).Name;
                    //BizActionFU.PatientID = CurrentVisit.PatientId;
                    //BizActionFU.PatientUnitID = CurrentVisit.PatientUnitId;
                    //BizActionFU.VisitID = CurrentVisit.ID;
                    //if (GlobalTemplateID != 0)
                    //    BizActionFU.TemplateID = GlobalTemplateID;
                    //else if (cmbComplaint.SelectedItem != null)
                    //    BizActionFU.TemplateID = ((clsEMRTemplateVO)cmbComplaint.SelectedItem).TemplateID;
                    //BizActionFU.UnitID = CurrentVisit.UnitId;
                    //BizActionFU.ControlIndex = ((FileUpload)((HyperlinkButton)sender).DataContext).Index;

                    //Uri address2 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    //PalashServiceClient clientFU = new PalashServiceClient("BasicHttpBinding_IPalashService", address2.AbsoluteUri);
                    //clientFU.ProcessCompleted += (s1, args) =>
                    //{
                    //    if (args.Error == null && args.Result != null)
                    //    {
                    //        List<clsPatientEMRUploadedFilesVO> lstFU = ((clsGetPatientEMRUploadedFilesBizActionVO)args.Result).objPatientEMRUploadedFiles;
                    //        if (lstFU != null && lstFU.Count > 0)
                    //        {
                    //            for (int i = 0; i < lstFU.Count; i++)
                    //            {
                    //                //FUSetting.ItemsSource[lstFU[i].ControlIndex].Data = lstFU[i].Value;
                    //                ((FileUpload)((HyperlinkButton)sender).DataContext).Data = lstFU[i].Value;
                    //            }

                    //            string FullFile = ((FieldDetail)lstBox.DataContext).Name + ((FileUpload)((HyperlinkButton)sender).DataContext).Index + ((FileUpload)((HyperlinkButton)sender).DataContext).FileName;

                    //            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    //            client.GlobalUploadFileCompleted += (s, args1) =>
                    //            {
                    //                if (args1.Error == null)
                    //                {
                    //                    HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FullFile });
                    //                    listOfReports.Add(FullFile);
                    //                }
                    //            };
                    //            client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", FullFile, ((FileUpload)((HyperlinkButton)sender).DataContext).Data);
                    //        }
                    //        else
                    //        {
                    //            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This File is not uploaded. Please upload the File then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    //            mgbx.Show();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "File cannot be loaded.\nError occured during file retrieving.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    //        mgbx.Show();
                    //    }
                    //};
                    //clientFU.ProcessAsync(BizActionFU, ((IApplicationConfiguration)App.Current).CurrentUser);
                    //clientFU.CloseAsync();
                }
            }
            else
            {
                MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This File Is Not Uploaded. Please Upload The File Then Click On Preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }
        }

        void FUrci_OnBrowseClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                ((ValueObjects.IVFPlanTherapy.FileUpload)((Button)sender).DataContext).FileName = openDialog.File.Name;
                ((TextBox)((Grid)((Button)sender).Parent).FindName("FileName")).Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        ((ValueObjects.IVFPlanTherapy.FileUpload)((Button)sender).DataContext).Data = new byte[stream.Length];
                        stream.Read(((ValueObjects.IVFPlanTherapy.FileUpload)((Button)sender).DataContext).Data, 0, (int)stream.Length);
                        //((ValueObjects.IVFPlanTherapy.FileUpload)((Button)sender).DataContext).FileInfo = openDialog.File;
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error While Reading File.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        void FUrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                FUSetting.RemoveAt(((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Index);
            }
            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                FUSetting.Add(new ValueObjects.IVFPlanTherapy.FileUpload());
            }
            lstFUBox.Items.Clear();
            for (int i = 0; i < FUSetting.Count; i++)
            {
                FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
                FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);
                FUrci.OnBrowseClick += new RoutedEventHandler(FUrci_OnBrowseClick);
                FUrci.OnViewClick += new RoutedEventHandler(FUrci_OnViewClick);
                FUSetting[i].Index = i;
                FUSetting[i].Command = ((i == FUSetting.Count - 1) ? "Add" : "Remove");
                FUrci.DataContext = FUSetting[i];
                lstFUBox.Items.Add(FUrci);
            }
        }

        private void MediaDetails_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails Win = new MediaDetails();
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.Show();
        }

        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails ObjWin = (MediaDetails)sender;
            ((List<clsFemaleMediaDetailsVO>)ObjWin.Tag).Clear();
            if (ObjWin.DialogResult == true)
            {
                if (ObjWin.ItemList != null)
                {
                    foreach (var item in ObjWin.ItemList)
                    {
                        clsFemaleMediaDetailsVO objItem = new clsFemaleMediaDetailsVO();

                        objItem.Date = item.Date;
                        objItem.ItemID = item.ItemID;
                        objItem.ItemName = item.ItemName;
                        objItem.BatchID = item.BatchID;
                        objItem.BatchCode = item.BatchCode;
                        objItem.ExpiryDate = item.ExpiryDate;
                        objItem.StoreID = item.StoreID;
                        objItem.PH = item.PH;
                        objItem.OSM = item.OSM;
                        objItem.SelectedStatus = item.SelectedStatus;
                        objItem.VolumeUsed = item.VolumeUsed;
                        ((List<clsFemaleMediaDetailsVO>)ObjWin.Tag).Add(objItem);
                        
                    }
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");               
            }
            else if (IsPageLoded == false)
            {
              
            }
            if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                try
                {
                    wi.Show();
                    if (IsSaved == true)
                    {
                        if (((clsFemaleLabDay0VO)this.DataContext).LabDaySummary.IsFreezed == true)
                            CmdSave.IsEnabled = false;
                        getEMRDetails(CoupleDetails.FemalePatient, "F");
                        getEMRDetails(CoupleDetails.MalePatient, "M");
                        setupPage();
                    }
                    else
                    {
                        fillCoupleDetails();     
                        ProcedureDate.SelectedDate = DateTime.Now.Date.Date;
                        ProcedureTime.Value = DateTime.Now;
                    }
                }
                catch (Exception ex) { throw ex; }
                finally { wi.Close(); }
            }
            IsPageLoded = true;
        }


        //private void fillDoctorMasters()
        //{
        //    clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
        //    //BizAction.MasterTable = ((MasterListItem)listSelectedTableSource.SelectedItem).Description;
        //    //BizAction.ColumnName = ((MasterListItem)listTableColumnSource.SelectedItem).Description;

        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
        //    client1.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            //lstSourceAuto.ItemsSource = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
        //            //grdAutoList.Visibility = Visibility.Visible;
        //        }
        //    };
        //    client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client1.CloseAsync();
        //}

        #region fill comboboxes

        private void fillPlannedTreatment()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPlannedTreatment;
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
                    cmbSrcTreatmentPlan.ItemsSource = null;
                    cmbSrcTreatmentPlan.ItemsSource = objList;
                    cmbSrcTreatmentPlan.SelectedValue = 0;
                    if (this.DataContext != null)
                    {
                        cmbSrcTreatmentPlan.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).TreatmentTypeID;

                    }
                }
                fillPlan();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
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
                    cmbAssistantEmbryologist.ItemsSource = null;
                    cmbAssistantEmbryologist.ItemsSource = objList;

                    if (this.DataContext != null)
                    {
                        cmbEmbryologist.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).EmbryologistID;
                        cmbAssistantEmbryologist.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).AssEmbryologistID;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillAnesthetist()
        {
            clsGetAnesthetistBizActionVO BizAction = new clsGetAnesthetistBizActionVO();
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
                    objList.AddRange(((clsGetAnesthetistBizActionVO)arg.Result).MasterList);
                    cmbAnesthetist.ItemsSource = null;
                    cmbAnesthetist.ItemsSource = objList;
                    cmbAnesthetist.SelectedItem = objList[0];
                    cmbAssistantAnesthetist.ItemsSource = null;
                    cmbAssistantAnesthetist.ItemsSource = objList;
                    cmbAssistantAnesthetist.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbAnesthetist.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).AnesthetistID;
                        cmbAssistantAnesthetist.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).AssAnesthetistID;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
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


                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void fillSemenSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_SourceSemenMaster;
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

                    cmbSrcSemen.ItemsSource = null;
                    cmbSrcSemen.ItemsSource = objList;
                    cmbSrcSemen.SelectedValue = 0;

                    if (this.DataContext != null)
                    {
                        cmbSrcSemen.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).SrcOfSemenID;
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillOocyteSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_SourceOocyteMaster;
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

                    cmbSrcOocyte.ItemsSource = null;
                    cmbSrcOocyte.ItemsSource = objList;
                    cmbSrcOocyte.SelectedValue = 0;

                    if (this.DataContext != null)
                    {
                        cmbSrcOocyte.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).SrcOfOocyteID;
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillNeedleSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_SourceNeedleMaster;
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

                    cmbSrcNeedle.ItemsSource = null;
                    cmbSrcNeedle.ItemsSource = objList;
                    cmbSrcNeedle.SelectedValue = 0;

                    if (this.DataContext != null)
                    {
                        cmbSrcNeedle.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).SrcOfNeedleID;
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillOPSType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_OPSTypeMaster;
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

                    cmbOPSTypeID.ItemsSource = null;
                    cmbOPSTypeID.ItemsSource = objList;
                    cmbOPSTypeID.SelectedValue = 0;

                    if (this.DataContext != null)
                    {
                        cmbOPSTypeID.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).OPSTypeID;
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillDenudingNeedleSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_SourceDenudingNeedleMaster;
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

                    cmbSrcDenNeedle.ItemsSource = null;
                    cmbSrcDenNeedle.ItemsSource = objList;
                    cmbSrcDenNeedle.SelectedValue = 0;

                    if (this.DataContext != null)
                    {
                        cmbSrcDenNeedle.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).SourceOfDenudingNeedle;
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillCumulus()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_CumulusMaster;
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
                    Cumulus = null;
                    Cumulus = objList;
                }
                if (this.DataContext != null)
                {
                    //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                }
                fillGrade();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillGrade()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_GradeMaster;
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

                    Grade = null;
                    Grade = objList;
                    
                }

                if (this.DataContext != null)
                {
                    //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                }
                fillMOI();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillMOI()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_MOIMaster;
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

                    MOI = null;
                    MOI = objList;
                }

                if (this.DataContext != null)
                {
                    //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                }
                LoadIVFRepeaterControl();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillPlan()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_PlanMaster;
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

                    Plan = null;
                    Plan = objList;
                }

                if (this.DataContext != null)
                {
                    //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                }
                fillCumulus();
                fillDOS();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillDOS()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_DOSMaster;
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

                    DOS = null;
                    DOS = objList;
                }

                if (this.DataContext != null)
                {
                    //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                }
                fillPIC();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillPIC()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_PICMaster;
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

                    PIC = null;
                    PIC = objList;
                }

                if (this.DataContext != null)
                {
                    //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                }
                LoadICSIRepeaterControl();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        
        #endregion

        private void setupPage()
        {
            try
            {
                wi.Show();
                clsGetFemaleLabDay0BizActionVO BizAction = new clsGetFemaleLabDay0BizActionVO();
                BizAction.CoupleID = CoupleDetails.CoupleId;
                BizAction.CoupleUnitID = CoupleDetails.CoupleUnitId;
                BizAction.FemaleLabDay0 = (clsFemaleLabDay0VO)this.DataContext;
                BizAction.OocyteID = ((clsFemaleLabDay0VO)this.DataContext).ID;
                BizAction.UnitID = ((clsFemaleLabDay0VO)this.DataContext).UnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        this.DataContext = ((clsGetFemaleLabDay0BizActionVO)args.Result).FemaleLabDay0;
                        IsUpdate = true;
                        SemenDetails = ((clsGetFemaleLabDay0BizActionVO)args.Result).FemaleLabDay0.SemenDetails;
                        if (((clsFemaleLabDay0VO)this.DataContext).IsFreezed == true)
                            CmdSave.IsEnabled = false;
                        GetTherapyDetails();
                        fillControls();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occur during retrieve data.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    wi.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                wi.Close();
            }
        }

        private void fillControls()
        {
            FillEmbryologist();
            FillAnesthetist();
            fillDoctor();
            fillNeedleSource();
            fillOocyteSource();
            fillSemenSource();
            fillOPSType();
            fillDenudingNeedleSource();
            fillPlannedTreatment();
            LoadFURepeaterControl();
        }

        private void GetTherapyDetails()
        {
            clsGetTherapyListBizActionVO BizAction = new clsGetTherapyListBizActionVO();
            BizAction.CoupleID = CoupleDetails.CoupleId;
            BizAction.CoupleUintID = CoupleDetails.CoupleUnitId;
            BizAction.TherapyUnitID = CoupleDetails.CoupleUnitId;
            BizAction.Flag = false;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    TherapyDetails.DataContext = ((clsGetTherapyListBizActionVO)args.Result).TherapyDetails;
                    ((clsFemaleLabDay0VO)this.DataContext).TreatmentTypeID = ((clsGetTherapyListBizActionVO)args.Result).TherapyDetails.PlannedTreatmentID;
                    cmbSrcTreatmentPlan.SelectedValue = ((clsGetTherapyListBizActionVO)args.Result).TherapyDetails.PlannedTreatmentID;

                }
                if (txtPlannedTreatment == null || txtPlannedTreatment.Text == "")
                {
                    MessageBoxControl.MessageBoxChildWindow msgW13 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Generate The Therapy First.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                   msgW13.Show();
                   msgW13.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW13_OnMessageBoxClosed);
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        void msgW13_OnMessageBoxClosed(MessageBoxResult result)
        {
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                ((IApplicationConfiguration)App.Current).FillMenu("IVF Lab Work");
            }
        }

        #region Fill Couple Details
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
                    CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                    if (CoupleDetails.CoupleId == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    {
                        //getEMRDetails(BizAction.CoupleDetails.FemalePatient, "F");
                        //getEMRDetails(BizAction.CoupleDetails.MalePatient, "M");
                        GetHeightAndWeight(BizAction.CoupleDetails);
                        GetTherapyDetails();
                        fillControls();
                        //added by priti
                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
                        {
                            WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                            bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo);
                            imgPhoto13.Source = bmp;
                            imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            imgP1.Visibility = System.Windows.Visibility.Visible;
                        }

                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
                        {
                            WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                            bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo);
                            imgPhoto12.Source = bmp;
                            imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            imgP2.Visibility = System.Windows.Visibility.Visible;
                        }                        //
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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
                        //FemalePatientDetails.BMI = BizAction.CoupleDetails.FemalePatient.BMI;
                        FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                        FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                        Female.DataContext = FemalePatientDetails;

                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        MalePatientDetails = CoupleDetails.MalePatient;
                        MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                        MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        //MalePatientDetails.BMI = BizAction.CoupleDetails.MalePatient.BMI;
                        MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                        MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                        Male.DataContext = MalePatientDetails;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        #region Get Patient EMR Details(Height and Weight)

        private void getEMRDetails(clsPatientGeneralVO PatientDetails, string Gender)
        {
            clsGetEMRDetailsBizActionVO BizAction = new clsGetEMRDetailsBizActionVO();
            BizAction.PatientID = PatientDetails.PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.TemplateID = 8;//Using For Getting Height Wight Of Patient 
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Double height = 0;
            Double weight = 0;

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.EMRDetailsList = ((clsGetEMRDetailsBizActionVO)args.Result).EMRDetailsList;

                    if (BizAction.EMRDetailsList != null || BizAction.EMRDetailsList.Count > 0)
                    {
                        for (int i = 0; i < BizAction.EMRDetailsList.Count; i++)
                        {
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Height"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    height = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Weight"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    weight = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (height != 0 && weight != 0)
                        {
                            if (Gender.Equals("F"))
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                Male.DataContext = PatientDetails;
                            }
                        }
                        else
                        {
                            if (Gender.Equals("F"))
                            {
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                Male.DataContext = PatientDetails;
                            }
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        #region Calculate BMI
        private double CalculateBMI(Double Height, Double Weight)
        {
            try
            {
                if (Weight == 0)
                {
                    return 0.0;

                }
                else if (Height == 0)
                {

                    return 0.0;
                }
                else
                {
                    double weight = Convert.ToDouble(Weight);
                    double height = Convert.ToDouble(Height);
                    double TotalBMI = weight / height;
                    TotalBMI = TotalBMI / height;
                    TotalBMI = TotalBMI * 10000;
                    return TotalBMI;

                }
            }
            catch (Exception ex)
            {
                return 0.0;
                throw ex;
            }
        }
        #endregion

        private void cmbSrcTreatmentPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbSrcTreatmentPlan.SelectedItem != null)
                {
                    switch (((MasterListItem)cmbSrcTreatmentPlan.SelectedItem).ID)
                    {
                        case 1:
                            IVF.Visibility = Visibility.Visible;
                            ICSI.Visibility = Visibility.Collapsed;
                            break;
                        case 2:
                            IVF.Visibility = Visibility.Collapsed;
                            ICSI.Visibility = Visibility.Visible;
                            break;
                        case 3:
                            IVF.Visibility = Visibility.Visible;
                            ICSI.Visibility = Visibility.Visible;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool IsValid = true;    
                    if (txtPlannedTreatment == null || txtPlannedTreatment.Text == "")
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW13 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please First Generate The Therapy.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW13.Show();
                        IsValid = false;
                        return;
                    }  
                    if (IVF.Visibility == Visibility.Visible)
                    {
                        if (IVFSetting.Where(Items => Items.Plan == null).Any() == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW13 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Plan For Fertilization Assessment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW13.Show();
                            IsValid = false;
                            return;
                        }
                        else if (IVFSetting.Where(Items => Items.Plan.ID == 0).Any() == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW13 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Plan For Fertilization Assessment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW13.Show();
                            IsValid = false;
                            return;
                        }
                    }

                    if (ICSI.Visibility == Visibility.Visible)
                    {
                        if (ICSISetting.Where(Plans => Plans.Plan == null).Any() == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW13 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Plan For Fertilization Assessment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                             msgW13.Show();
                            IsValid = false;
                        }
                        else if (ICSISetting.Where(Plans => Plans.Plan.ID == 0).Any() == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW13 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Plan For Fertilization Assessment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                             msgW13.Show();
                            IsValid = false;
                        }
                    }

                    if (IsValid)
                    {                     
                        if (ChkValidation())
                        {
                            validate();
                            ImpressionWindow winImp = new ImpressionWindow();
                            if (((clsFemaleLabDay0VO)this.DataContext).LabDaySummary != null)
                                winImp.Impression = ((clsFemaleLabDay0VO)this.DataContext).LabDaySummary.Impressions;
                            winImp.OnSaveClick += new RoutedEventHandler(winImp_OnSaveClick);
                            winImp.Show();
                        }
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ChkValidation()
        {
            bool result = true;
            if (cmbSrcNeedle.SelectedItem == null)
            {               
                cmbSrcNeedle.TextBox.SetValidation("Please select Source of Needle");
                cmbSrcNeedle.TextBox.RaiseValidationError();
                cmbSrcNeedle.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbSrcNeedle.SelectedItem).ID == 0)
            {
                cmbSrcNeedle.TextBox.SetValidation("Please select the Source of Needle");
                cmbSrcNeedle.TextBox.RaiseValidationError();
                cmbSrcNeedle.Focus();
                result = false;
            }
            else
                cmbSrcNeedle.TextBox.ClearValidationError();
            if (cmbEmbryologist.SelectedItem == null)
            {
                cmbEmbryologist.TextBox.SetValidation("Please select Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbEmbryologist.SelectedItem).ID == 0)
            {
                cmbEmbryologist.TextBox.SetValidation("Please select the Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                result = false;
            }
            else
                cmbEmbryologist.TextBox.ClearValidationError();
            return result;
        }

        void winImp_OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (((clsFemaleLabDay0VO)this.DataContext).LabDaySummary == null)
                ((clsFemaleLabDay0VO)this.DataContext).LabDaySummary = new clsLabDaysSummaryVO();
            ((clsFemaleLabDay0VO)this.DataContext).LabDaySummary.FormID = IVFLabWorkForm.FemaleLabDay0;
            ((clsFemaleLabDay0VO)this.DataContext).LabDaySummary.Impressions = sender.ToString();
            Save();
        }

        private void validate()
        {
            ((clsFemaleLabDay0VO)this.DataContext).CoupleID = CoupleDetails.CoupleId;
            ((clsFemaleLabDay0VO)this.DataContext).CoupleUnitID = CoupleDetails.CoupleUnitId;

            if (cmbEmbryologist.SelectedItem != null && ((MasterListItem)cmbEmbryologist.SelectedItem).ID != 0)
            {
                ((clsFemaleLabDay0VO)this.DataContext).EmbryologistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;
            }
            if (cmbAnesthetist.SelectedItem != null && ((MasterListItem)cmbAnesthetist.SelectedItem).ID != 0)
            {
                ((clsFemaleLabDay0VO)this.DataContext).AnesthetistID = ((MasterListItem)cmbAnesthetist.SelectedItem).ID;
            }
            if (cmbAssistantEmbryologist.SelectedItem != null && ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID != 0)
            {
                ((clsFemaleLabDay0VO)this.DataContext).AssEmbryologistID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;
            }
            if (cmbAssistantAnesthetist.SelectedItem != null && ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID != 0)
            {
                ((clsFemaleLabDay0VO)this.DataContext).AssAnesthetistID = ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID;
            }

            if (cmbSrcNeedle.SelectedItem != null && ((MasterListItem)cmbSrcNeedle.SelectedItem).ID != 0)
            {
                ((clsFemaleLabDay0VO)this.DataContext).SrcOfNeedleID = ((MasterListItem)cmbSrcNeedle.SelectedItem).ID;
            }
            if (cmbSrcOocyte.SelectedItem != null && ((MasterListItem)cmbSrcOocyte.SelectedItem).ID != 0)
            {
                ((clsFemaleLabDay0VO)this.DataContext).SrcOfOocyteID = ((MasterListItem)cmbSrcOocyte.SelectedItem).ID;
            }
            if (cmbSrcSemen.SelectedItem != null && ((MasterListItem)cmbSrcSemen.SelectedItem).ID != 0)
            {
                ((clsFemaleLabDay0VO)this.DataContext).SrcOfSemenID = ((MasterListItem)cmbSrcSemen.SelectedItem).ID;
            }

            if (cmbOPSTypeID.SelectedItem != null && ((MasterListItem)cmbOPSTypeID.SelectedItem).ID != 0)
            {
                ((clsFemaleLabDay0VO)this.DataContext).OPSTypeID = ((MasterListItem)cmbOPSTypeID.SelectedItem).ID;
            }
            if (cmbSrcDenNeedle.SelectedItem != null && ((MasterListItem)cmbSrcDenNeedle.SelectedItem).ID != 0)
            {
                ((clsFemaleLabDay0VO)this.DataContext).SourceOfDenudingNeedle = ((MasterListItem)cmbSrcDenNeedle.SelectedItem).ID;
            }

            if (cmbSrcTreatmentPlan.SelectedItem != null && ((MasterListItem)cmbSrcTreatmentPlan.SelectedItem).ID != 0)
            {
                ((clsFemaleLabDay0VO)this.DataContext).TreatmentTypeID = ((MasterListItem)cmbSrcTreatmentPlan.SelectedItem).ID;
                if (((clsFemaleLabDay0VO)this.DataContext).TreatmentTypeID == 1)
                {
                    ((clsFemaleLabDay0VO)this.DataContext).ICSISetting = null;
                    ((clsFemaleLabDay0VO)this.DataContext).ICSICompletionTime = null;
                    ((clsFemaleLabDay0VO)this.DataContext).SourceOfDenudingNeedle = 0;
                }
                else if (((clsFemaleLabDay0VO)this.DataContext).TreatmentTypeID == 2)
                {
                    ((clsFemaleLabDay0VO)this.DataContext).IVFSetting = null;
                    ((clsFemaleLabDay0VO)this.DataContext).OocytePreparationMedia = null;
                    ((clsFemaleLabDay0VO)this.DataContext).SpermPreparationMedia = null;
                    ((clsFemaleLabDay0VO)this.DataContext).FinalLayering = null;
                    ((clsFemaleLabDay0VO)this.DataContext).Matured = 0;
                    ((clsFemaleLabDay0VO)this.DataContext).Immatured = 0;
                    ((clsFemaleLabDay0VO)this.DataContext).PostMatured = 0;
                    ((clsFemaleLabDay0VO)this.DataContext).Total = 0;
                    ((clsFemaleLabDay0VO)this.DataContext).DoneBy = null;
                    ((clsFemaleLabDay0VO)this.DataContext).FollicularFluid = null;
                    ((clsFemaleLabDay0VO)this.DataContext).OPSTypeID = 0;
                    ((clsFemaleLabDay0VO)this.DataContext).IVFCompletionTime = null;
                }
            }
        }
        
        void Save()
        {
            try
            {
                wi.Show();

                clsAddUpdateFemaleLabDay0BizActionVO BizAction = new clsAddUpdateFemaleLabDay0BizActionVO();
                BizAction.FemaleLabDay0 = (clsFemaleLabDay0VO)this.DataContext;
                BizAction.FemaleLabDay0.SemenDetails = SemenDetails;
                if (IsSaved == true)
                {
                    BizAction.IsUpdate = true;
                }
                else
                {
                    BizAction.IsUpdate = false;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        IsSaved = true;
                        if (((clsFemaleLabDay0VO)this.DataContext).LabDaySummary.IsFreezed == true)
                            CmdSave.IsEnabled = false;
                        wi.Close();
                        MessageBoxControl.MessageBoxChildWindow msgbx = new MessageBoxControl.MessageBoxChildWindow("", "Female Lab Day0 Details Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgbx.Show();
                        LabDaysSummary Lab = new LabDaysSummary();
                        Lab.IsPatientExist = true;
                        ((IApplicationConfiguration)App.Current).OpenMainContent(Lab);
                    }
                    wi.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wi.Close();
                MessageBoxControl.MessageBoxChildWindow msgbx = new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgbx.Show();
                throw ex;
            }
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            ((IApplicationConfiguration)App.Current).FillMenu("IVF Lab Work");
        }

        private void btnSearchOocyteDonor_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.PatientCategoryID = 8;
            Win.Show();

        }

        private void btnSearchSemenDonor_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.PatientCategoryID = 9;
            Win.Show();
        }
        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch ObjWin = (PatientSearch)sender;
            if (ObjWin.DialogResult == true)
            {
                if (ObjWin.PatientCategoryID == 8)
                    txtOocyteDonorCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
                else
                    txtSemenDonorCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
            }
        }

        private void cmbSrcOocyte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSrcOocyte.SelectedItem != null)
            {
                if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 0 )
                {
                    btnSearchOocyteDonor.IsEnabled = false;

                    txtOocyteDonorCode.Text = "";
                    txtOocyteDonorCode.IsEnabled = false;
                    //By Anjali
                   // txtOocyteDonorID.IsEnabled = false;
                }
                else if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 1)
                {
                    btnSearchOocyteDonor.IsEnabled = false;
                    txtOocyteDonorCode.Text = "";
                    txtOocyteDonorCode.IsEnabled = false;
                    //By Anjali
                   // txtOocyteDonorID.IsEnabled = false;
                }
                else
                {
                    btnSearchOocyteDonor.IsEnabled = true;
                  //  txtOocyteDonorID.IsEnabled = true;
                    txtOocyteDonorCode.IsEnabled = true;
                }
            }
        }

        private void cmbSrcSemen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSrcSemen.SelectedItem != null)
            {
                if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 0 )
                {
                    btnSearchSemenDonor.IsEnabled = false;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.IsEnabled = false;
                    //By Anjali
                 //   txtSemenDonorID.IsEnabled = false;
                }
                else if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 1)
                {
                    btnSearchSemenDonor.IsEnabled = false;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.IsEnabled = false;
                    //By Anjali
                //    txtSemenDonorID.IsEnabled = false;
                }
                else
                {
                    btnSearchSemenDonor.IsEnabled = true;
                 //  txtSemenDonorID.IsEnabled = true;
                    txtSemenDonorCode.IsEnabled = true;
                }
            }
        }

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
            if (MaleAlert.Text.Trim().Length > 0)
            {
                DataDrivenApplication.Forms.frmAttention PatientAlert = new DataDrivenApplication.Forms.frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.MalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention Not Entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {
            if (FemaleAlert.Text.Trim().Length > 0)
            {
                DataDrivenApplication.Forms.frmAttention PatientAlert = new DataDrivenApplication.Forms.frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.FemalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention Not Entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

        #region Semen Details
        private void cmdSemenDetails_Click(object sender, RoutedEventArgs e)
        {
            SemenDetails WinSemen = new SemenDetails();
            if (IsUpdate == true)
            {
                WinSemen.IsUpdate = true;
                WinSemen.Details = SemenDetails;
                WinSemen.cmbSourceOfSemen.SelectedValue = SourceOfSemen;
                WinSemen.cmbMethodOfSpermpreparation.SelectedValue = MethodOfSpermPreparation;
            }
            WinSemen.OnSaveButton_Click += new RoutedEventHandler(WinSemen_OnSaveButton_Click);
            WinSemen.Show();
        }

        void WinSemen_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SemenDetails ObjSemen = (SemenDetails)sender;
            if (ObjSemen.DialogResult == true)
            {
                if (ObjSemen.Details != null)
                {
                    SourceOfSemen = ((MasterListItem)(ObjSemen.cmbSourceOfSemen.SelectedItem)).ID;
                    MethodOfSpermPreparation = ((MasterListItem)(ObjSemen.cmbMethodOfSpermpreparation.SelectedItem)).ID;
                    SemenDetails = ObjSemen.Details;
                }
            }
        }
        #endregion


    }
}
