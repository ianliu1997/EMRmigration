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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using System.IO;
using PalashDynamics.ValueObjects.DashBoardVO;
using CIMS;
using System.Collections.ObjectModel;
using MessageBoxControl;
using PalashDynamics.Controls;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmPGD :ChildWindow
    {
            public long LabDayID ;
            public  long   LabDayUnitID ;
            public long  LabDayNo ;
            public long PatientID ;
            public long PatientUnitID ;
            public long PlanTherapyID ;
            public long PlanTherapyUnitID ;
            public  long OocyteNumber ;
            public long SerialOocyteNumber;
            private ObservableCollection<clsPGDFISHVO> _FISH = new ObservableCollection<clsPGDFISHVO>();
            public ObservableCollection<clsPGDFISHVO> FISH
            {
                get { return _FISH; }
                set { _FISH = value; }
            }
            private ObservableCollection<clsPGDFISHVO> _RemoveFISH = new ObservableCollection<clsPGDFISHVO>();
            public ObservableCollection<clsPGDFISHVO> RemoveFISH
            {
                get { return _RemoveFISH; }
                set { _RemoveFISH = value; }
            }
            private ObservableCollection<clsPGDKaryotypingVO> _Karyotyping = new ObservableCollection<clsPGDKaryotypingVO>();
            public ObservableCollection<clsPGDKaryotypingVO> Karyotyping
            {
                get { return _Karyotyping; }
                set { _Karyotyping = value; }
            }
            private ObservableCollection<clsPGDKaryotypingVO> _RemoveKaryotyping = new ObservableCollection<clsPGDKaryotypingVO>();
            public ObservableCollection<clsPGDKaryotypingVO> RemoveKaryotyping
            {
                get { return _RemoveKaryotyping; }
                set { _RemoveKaryotyping = value; }
            }
        public frmPGD()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }

        }
        private bool Validate()
        {
            bool result = true;
            
            if (txtChromosomalDisease.Text == null)
            {
                txtChromosomalDisease.SetValidation("Please Enter Chromosomal Disease");
                txtChromosomalDisease.RaiseValidationError();
                txtChromosomalDisease.Focus();
                result = false;
            }
            else
                txtChromosomalDisease.ClearValidationError();
            if (PGDDate.SelectedDate == null)
            {
                PGDDate.SetValidation("Please Select Date");
                PGDDate.RaiseValidationError();
                PGDDate.Focus();
                return false;
            }
            else
                PGDDate.ClearValidationError();

            if (PGDTime.Value == null)
            {
                PGDTime.SetValidation("Please Select Time");
                PGDTime.RaiseValidationError();
                PGDTime.Focus();
                return false;
            }
            else
                PGDTime.ClearValidationError();
            

            if (cmbPhysician.SelectedItem == null)
            {
                cmbPhysician.TextBox.SetValidation("Please select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbPhysician.SelectedItem).ID == 0)
            {
                cmbPhysician.TextBox.SetValidation("Please select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                result = false;
            }
            else
                cmbPhysician.TextBox.ClearValidationError();
            if (cmbBiopsy.SelectedItem == null)
            {
                cmbBiopsy.TextBox.SetValidation("Please select Biopsy");
                cmbBiopsy.TextBox.RaiseValidationError();
                cmbBiopsy.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbBiopsy.SelectedItem).ID == 0)
            {
                cmbBiopsy.TextBox.SetValidation("Please select Biopsy");
                cmbBiopsy.TextBox.RaiseValidationError();
                cmbBiopsy.Focus();
                result = false;
            }
            else
                cmbBiopsy.TextBox.ClearValidationError();

            if (cmbSpecimanUsed.SelectedItem == null)
            {
                cmbSpecimanUsed.TextBox.SetValidation("Please select Speciman Used");
                cmbSpecimanUsed.TextBox.RaiseValidationError();
                cmbSpecimanUsed.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbSpecimanUsed.SelectedItem).ID == 0)
            {
                cmbSpecimanUsed.TextBox.SetValidation("Please select Speciman Used");
                cmbSpecimanUsed.TextBox.RaiseValidationError();
                cmbSpecimanUsed.Focus();
                result = false;
            }
            else
                cmbSpecimanUsed.TextBox.ClearValidationError();
            if (txtReasonofreferral.Text == null)
            {
                txtReasonofreferral.SetValidation("Please Enter Reason of referral");
                txtReasonofreferral.RaiseValidationError();
                txtReasonofreferral.Focus();
                result = false;
            }
            else
                txtReasonofreferral.ClearValidationError();

            return result;
        }       
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdatePGDHistoryBizActionVO BizActionVO = new clsAddUpdatePGDHistoryBizActionVO();
                    BizActionVO.PGDHistoryDetails = new clsPGDHistoryVO();
                    BizActionVO.PGDHistoryDetails.ID = HistoryID;
                    BizActionVO.PGDHistoryDetails.PatientID = PatientID;
                    BizActionVO.PGDHistoryDetails.PatientUnitID = PatientUnitID;
                    BizActionVO.PGDHistoryDetails.PlanTherapyID = PlanTherapyID;
                    BizActionVO.PGDHistoryDetails.PlanTherapyUnitID = PlanTherapyUnitID;
                    BizActionVO.PGDHistoryDetails.ChromosomalDisease=txtChromosomalDisease.Text;
                    BizActionVO.PGDHistoryDetails.XLinkedDominant=txtXDominantDisorder.Text;
                    BizActionVO.PGDHistoryDetails.XLinkedRecessive=txtXRecessiveDisorder.Text;
                    BizActionVO.PGDHistoryDetails.AutosomalRecessive=txtAutosomalRecessiveDisorder.Text;
                    BizActionVO.PGDHistoryDetails.AutosomalDominant=txtAutosomalDominantDisorder.Text;
                    BizActionVO.PGDHistoryDetails.Ylinked = txtYLinkedDisorder.Text;

                  Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
              PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
              client.ProcessCompleted += (s, arg) =>
              {
                  if (arg.Error == null && arg.Result != null)
                  {
                      clsAddUpdatePGDGeneralDetailsBizActionVO BizActionObj = new clsAddUpdatePGDGeneralDetailsBizActionVO();
                      BizActionObj.PGDGeneralDetails = new clsPGDGeneralDetailsVO();
                      BizActionObj.PGDFISHList = new List<clsPGDFISHVO>();
                      BizActionObj.PGDKaryotypingList = new List<clsPGDKaryotypingVO>();
                      BizActionObj.PGDGeneralDetails.ID = ((clsPGDGeneralDetailsVO)this.DataContext).ID;
                      BizActionObj.PGDGeneralDetails.SerialEmbNumber = SerialOocyteNumber;
                        BizActionObj.PGDGeneralDetails.OocyteNumber = OocyteNumber;
                      BizActionObj.PGDGeneralDetails.LabDayNo = LabDayNo;
                      BizActionObj.PGDGeneralDetails.LabDayID = LabDayID;
                      BizActionObj.PGDGeneralDetails.LabDayUnitID = LabDayUnitID;
                      BizActionObj.PGDGeneralDetails.PlanTherapyID = PlanTherapyID;
                      BizActionObj.PGDGeneralDetails.PlanTherapyUnitID = PlanTherapyUnitID;
                      BizActionObj.PGDGeneralDetails.Date = PGDDate.SelectedDate.Value.Date;
                      if (txtFN.Text.Trim() != null)
                      {
                          BizActionObj.PGDGeneralDetails.SourceURL = "LabDayNo" + LabDayNo + "LabDayID" + LabDayID + "LabDayUnitID" + LabDayUnitID + "OocyteNumber" + OocyteNumber + "SerialOocyteNumber" + SerialOocyteNumber + txtFN.Text.Trim();
                          BizActionObj.PGDGeneralDetails.FileName = txtFN.Text.Trim();
                          BizActionObj.PGDGeneralDetails.Report = AttachedFileContents;
                      }
                      BizActionObj.PGDGeneralDetails.MainFISHRemark = txtMainFSHRemark.Text;
                      BizActionObj.PGDGeneralDetails.MainKaryotypingRemark =txtMainKaryotypingRemark.Text;
                      BizActionObj.PGDGeneralDetails.Date = BizActionObj.PGDGeneralDetails.Date.Value.Add(PGDTime.Value.Value.TimeOfDay);
                      if(cmbPhysician.SelectedItem !=null)
                          BizActionObj.PGDGeneralDetails.Physician=((MasterListItem)cmbPhysician.SelectedItem).ID;
                      if (cmbBiopsy.SelectedItem != null)
                          BizActionObj.PGDGeneralDetails.BiospyID = ((MasterListItem)cmbBiopsy.SelectedItem).ID;
                      if (cmbSpecimanUsed.SelectedItem != null)
                          BizActionObj.PGDGeneralDetails.SpecimanUsedID = ((MasterListItem)cmbSpecimanUsed.SelectedItem).ID;
                      BizActionObj.PGDGeneralDetails.ReferringFacility = txtReferringFacitity.Text;
                      BizActionObj.PGDGeneralDetails.ResonOfReferal = txtReasonofreferral.Text;
                          for (int i = 0; i < FISH.Count; i++)
                            {
                                FISH[i].ChromosomeStudiedID = FISH[i].SelectedChromosomeStudiedId.ID;
                                FISH[i].TestOrderedID = FISH[i].SelectedTestOrderedId.ID;
                            }
                          BizActionObj.PGDFISHList = (List<clsPGDFISHVO>)FISH.ToList();
                          for (int i = 0; i < RemoveFISH.Count; i++)
                          {
                              BizActionObj.PGDFISHList.Add(RemoveFISH[i]);
                          }
                          for (int i = 0; i < Karyotyping.Count; i++)
                          {
                              Karyotyping[i].ChromosomeStudiedID = Karyotyping[i].SelectedChromosomeStudiedId.ID;
                              if ((MasterListItem)cmbBindingTechnique.SelectedItem !=null)
                              Karyotyping[i].BindingTechnique = ((MasterListItem)cmbBindingTechnique.SelectedItem).ID;
                              if ((MasterListItem)cmbCultureType.SelectedItem != null)
                              Karyotyping[i].CultureTypeID = ((MasterListItem)cmbCultureType.SelectedItem).ID;
                          }
                          BizActionObj.PGDKaryotypingList = (List<clsPGDKaryotypingVO>)Karyotyping.ToList();
                          for (int i = 0; i < RemoveKaryotyping.Count; i++)
                          {
                              BizActionObj.PGDKaryotypingList.Add(RemoveKaryotyping[i]);
                          }
                           // Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                          PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                          client1.ProcessCompleted += (s1, arg1) =>
                          {
                              if (arg1.Error == null && arg1.Result != null)
                              {

                                  //if (AttachedFileContents !=null)
                                  //{
                                  //    Uri address2 = new Uri(Application.Current.Host.Source, "../EmailTemplateAttachment");
                                  //    string filePath = address2.ToString();
                                      
                                  //    string file = filePath+"/"+"LabDayNo" + LabDayNo + "LabDayID" + LabDayID + "LabDayUnitID" + LabDayUnitID + txtFN.Text.Trim();
                                  //    using (FileStream fs = new FileStream(file, FileMode.Create))
                                  //    {
                                  //        fs.Write(AttachedFileContents, 0, (int)AttachedFileContents.Length);
                                  //    }
                                  //}
                                  string name = "LabDayNo" + LabDayNo + "LabDayID" + LabDayID + "LabDayUnitID" + LabDayUnitID + "OocyteNumber" + OocyteNumber + "SerialOocyteNumber" + SerialOocyteNumber+txtFN.Text.Trim();
                                  obName.Add(Name);
                                 // DeleteImage(obName);
                                  if (AttachedFileContents !=null)
                                  UploadImage(name, AttachedFileContents);
                                 
                                  MessageBoxControl.MessageBoxChildWindow msgForTemplateSave = new MessageBoxChildWindow("Palash", "Details Saved Sucessfully", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                  msgForTemplateSave.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgForTemplateUpdate_OnMessageBoxClosed);
                                  msgForTemplateSave.Show();
                              }
                          };
                          client1.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                          client1.CloseAsync();
                  }
                  else
                  {
                      MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                      msgW1.Show();
                  }
              };
              client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
              client.CloseAsync();
                
            }
                
        }
        private void UploadImage(string name, byte[] AttachedFileContents)
          {
              Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
              DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
              client.UploadReportFileCompleted += (s, args) =>
              {
                  if (args.Error == null)
                  {
                      //if (OnSaveButtonClick != null)
                      //{
                      //    OnSaveButtonClick((clsRadOrderBookingDetailsVO)(this.DataContext), new RoutedEventArgs());
                      //}
                      //this.DialogResult = true;
                  }
              };
              client.UploadReportFileAsync(name, AttachedFileContents);
          }
        ObservableCollection<string> obName = new ObservableCollection<string>();
        private void DeleteImage(ObservableCollection<string> name)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.DeleteReportFileCompleted += (s1, args1) =>
            {
                if (args1.Error == null)
                {

                }
            };
            client.DeleteReportFileAsync(name);
        }
        void msgForTemplateUpdate_OnMessageBoxClosed(MessageBoxResult result)
        {
            this.DialogResult = true;
        }
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            fillChromosomeStudied();
        }
        private List<MasterListItem> _ChromosomeStudied = new List<MasterListItem>();
        public List<MasterListItem> ChromosomeStudied
        {
            get
            {
                return _ChromosomeStudied;
            }
            set
            {
                _ChromosomeStudied = value;
            }
        }
        private List<MasterListItem> _TestOrdered = new List<MasterListItem>();
        public List<MasterListItem> TestOrdered
        {
            get
            {
                return _TestOrdered;
            }
            set
            {
                _TestOrdered = value;
            }
        }
        private void fillChromosomeStudied()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_Chromosome;
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

                    ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                    ChromosomeStudied = ((clsGetMasterListBizActionVO)args.Result).MasterList;


                    fillTestOrdered();
                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillTestOrdered()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_ChromosomeTestOrdered;
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

                    ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                    TestOrdered = ((clsGetMasterListBizActionVO)args.Result).MasterList;


                    fillBiopsy();
                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillBiopsy()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFBiopsy;
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

                    cmbBiopsy.ItemsSource = null;
                    cmbBiopsy.ItemsSource = objList;
                    cmbBiopsy.SelectedItem = objList[0];
                    
                    FillCultureType();
                }

               
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillCultureType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPGDCultureType;
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
                    cmbCultureType.ItemsSource = null;
                    cmbCultureType.ItemsSource = objList;
                    cmbCultureType.SelectedItem = objList[0];

                    FillBindingTechnique();
                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillBindingTechnique()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPGDBindingTechnique;
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

                    cmbBindingTechnique.ItemsSource = null;
                    cmbBindingTechnique.ItemsSource = objList;
                    cmbBindingTechnique.SelectedItem = objList[0];
                    FillSpecimanUsed();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
           private void FillSpecimanUsed()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPGDSpecimanUsed;
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

                    cmbSpecimanUsed.ItemsSource = null;
                    cmbSpecimanUsed.ItemsSource = objList;
                    cmbSpecimanUsed.SelectedItem = objList[0];
                    fillPhysician();


                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        
        private void fillPhysician()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbPhysician.ItemsSource = null;
                    cmbPhysician.ItemsSource = objList;
                    cmbPhysician.SelectedItem = objList[0];
                    FillHistoryDetails();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public byte[] AttachedFileContents { get; set; }
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFN.Text))
            {
                if (AttachedFileContents != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + txtFN.Text.Trim() });
                            AttachedFileNameList.Add(txtFN.Text.Trim());

                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", txtFN.Text.Trim(), AttachedFileContents);
                }
            }
        }

        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {

                txtFN.Text = openDialog.File.Name;
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
                }
            }
        }

        private void ImgLink_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFN.Text))
            {
                //{
                //    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                //    client.GlobalUploadFileCompleted += (s, args) =>
                //    {
                //        if (args.Error == null)
                //        {
                //            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + txtFN.Text });
                //            AttachedFileNameList.Add(txtFN.Text);

                //        }
                //    };
                //    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", txtFN.Text, null);
                //}

                //Uri address = new Uri(Application.Current.Host.Source, "EmailTemplateAttachment"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                //client.UploadReportFileCompleted += (s, args) =>
                //{
                //    if (args.Error == null)
                //    {
                //        HtmlPage.Window.Invoke("OpenReport", new string[] { SourceURL });
                //        AttachedFileNameList.Add(SourceURL);
                //    }
                //};
                //client.UploadReportFileAsync(SourceURL, null);
                //client.CloseAsync();


                Uri address = new Uri(Application.Current.Host.Source, "../PatientPathTestReportDocuments");
                string fileName1 = address.ToString();
                fileName1 = fileName1 + "/" + SourceURL.Trim();  //txtFN.Text.Trim();
                //HtmlPage.Window.Invoke("open", new string[] { fileName1, "", "" });
                HtmlPage.Window.Invoke("Open", fileName1);
                
               
            }
        }

        private void cmbTestOrdered_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (FISH == null)
            {
                FISH = new ObservableCollection<clsPGDFISHVO>();
            }
            FISH.Add(new clsPGDFISHVO() { ChromosomeStudiedIdList = ChromosomeStudied, ChromosomeStudiedID = 0, TestOrderedIdList = TestOrdered, TestOrderedID = 0 ,Status=true});
            dgFISHGrid.ItemsSource = FISH;
            if (Karyotyping == null)
            {
                Karyotyping = new ObservableCollection<clsPGDKaryotypingVO>();
            }
            Karyotyping.Add(new clsPGDKaryotypingVO() { ChromosomeStudiedIdList = ChromosomeStudied, ChromosomeStudiedID = 0,Status=true});
            dgKaryotypingGrid.ItemsSource = Karyotyping;

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dgFISHGrid.SelectedItem != null)
            {
                int index;
                if (FISH.Count > 1)
                {
                    if (dgFISHGrid.SelectedIndex > 0 )
                    {
                        index = dgFISHGrid.SelectedIndex;
                        if (FISH[index].ID > 0)
                        {

                            FISH[index].Status = false;
                            Karyotyping[index].Status = false;
                            RemoveFISH.Add(FISH[index]);
                            RemoveKaryotyping.Add(Karyotyping[index]);
                        }
                        FISH.RemoveAt(index);
                        Karyotyping.RemoveAt(index);
                    }
                }
            }
            dgFISHGrid.ItemsSource = FISH;
            dgFISHGrid.UpdateLayout();
            dgKaryotypingGrid.ItemsSource = Karyotyping;
            dgKaryotypingGrid.UpdateLayout();
        }

        private void cmbChromosomeStudied_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteComboBox)sender).Name.Equals("cmbChromosomeStudied"))
            {
                for (int i = 0; i < FISH.Count; i++)
                {
                    if (FISH[i] == ((clsPGDFISHVO)dgFISHGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteComboBox)sender).SelectedItem) != null)
                        {
                            FISH[i].ChromosomeStudiedID = ((MasterListItem)((AutoCompleteComboBox)sender).SelectedItem).ID;
                            if (FISH.Count == Karyotyping.Count)
                            Karyotyping[i].SelectedChromosomeStudiedId=ChromosomeStudied.FirstOrDefault(p => p.ID == ((MasterListItem)((AutoCompleteComboBox)sender).SelectedItem).ID);
                            
                            
                        }
                    }
                }
                dgKaryotypingGrid.ItemsSource = null;
                dgKaryotypingGrid.ItemsSource = Karyotyping;
                dgKaryotypingGrid.UpdateLayout();
            }
        }

        private void cmbKaryotypingChromosomeStudied_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        long HistoryID = 0;
        public void FillHistoryDetails() 
        {
            clsGetPGDHistoryBizActionVO BizAction = new clsGetPGDHistoryBizActionVO();
             BizAction.PGDDetails = new clsPGDHistoryVO();
             BizAction.PGDDetails.PlanTherapyID = PlanTherapyID;
             BizAction.PGDDetails.PlanTherapyUnitID = PlanTherapyUnitID;
             BizAction.PGDDetails.PatientID = PatientID;
             BizAction.PGDDetails.PatientUnitID = PatientUnitID;
             Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
             PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

             client.ProcessCompleted += (s, arg) =>
             {
                 if (arg.Error == null && arg.Result != null)
                 {
                     if (((clsGetPGDHistoryBizActionVO)arg.Result).SuccessStatus != null)
                     {
                         this.DataContext = null;
                         this.DataContext = ((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails;
                         HistoryDetails.DataContext = ((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails;
                         HistoryID = ((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails.ID;
                     }
                 }
                 FillDetails();
             };
             client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
             client.CloseAsync();
        }
        string SourceURL = "";
        public void FillDetails()
        {

            clsGetPGDGeneralDetailsBizActionVO BizAction = new clsGetPGDGeneralDetailsBizActionVO();
            BizAction.PGDGeneralDetails = new clsPGDGeneralDetailsVO();
            BizAction.PGDFISHList = new List<clsPGDFISHVO>();
            BizAction.PGDKaryotypingList = new List<clsPGDKaryotypingVO>();
            BizAction.PGDGeneralDetails.LabDayID = LabDayID;
            BizAction.PGDGeneralDetails.LabDayUnitID = LabDayUnitID;
            BizAction.PGDGeneralDetails.LabDayNo = LabDayNo;
            
             Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
             PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

             client.ProcessCompleted += (s, arg) =>
             {
                 if (arg.Error == null && arg.Result != null)
                 {
                     if ((clsGetPGDGeneralDetailsBizActionVO)arg.Result != null)
                     {
                         this.DataContext = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails;
                         if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.FileName != null)
                         {
                             txtFN.Text = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.FileName;
                         }
                         if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SourceURL != null)
                         {
                             SourceURL = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SourceURL;
                         }
                         
                              if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.Physician != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.Physician != 0)
                         {
                             cmbPhysician.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.Physician;
                         }
                           if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.BiospyID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.BiospyID != 0)
                         {
                             cmbBiopsy.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.BiospyID;
                         }
                          if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SpecimanUsedID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SpecimanUsedID != 0)
                         {
                             cmbSpecimanUsed.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SpecimanUsedID;
                         }

                          if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList.Count > 0)
                          {
                              for (int i = 0; i < ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList.Count; i++)
                              {
                                  ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].ChromosomeStudiedIdList = ChromosomeStudied;

                                  if (Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].ChromosomeStudiedID) > 0)
                                  {
                                      ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].SelectedChromosomeStudiedId = ChromosomeStudied.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].ChromosomeStudiedID));
                                  }
                                  else
                                  {
                                      ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].SelectedChromosomeStudiedId = ChromosomeStudied.FirstOrDefault(p => p.ID == 0);
                                  }
                                  ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].TestOrderedIdList = TestOrdered;

                                  if (Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].TestOrderedID) > 0)
                                  {
                                      ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].SelectedTestOrderedId = TestOrdered.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].TestOrderedID));
                                  }
                                  else
                                  {
                                      ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].SelectedTestOrderedId = TestOrdered.FirstOrDefault(p => p.ID == 0);
                                  }

                                  FISH.Add(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i]);

                              }
                              cmdPrint.IsEnabled = true;
                          }
                          else
                          {
                              cmdPrint.IsEnabled = false;
                              fillInitailFISHDetails();
                          }
                          if(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList!=null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList.Count>0)
                         {
                             if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[0].BindingTechnique !=null)
                             cmbBindingTechnique.SelectedValue=((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[0].BindingTechnique;
                             if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[0].CultureTypeID != null)
                             cmbCultureType.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[0].CultureTypeID;
                            for (int i = 0; i < ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList.Count; i++)
                            {
                                 ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i].ChromosomeStudiedIdList = ChromosomeStudied;

                                    if (Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i].ChromosomeStudiedID) > 0)
                                    {
                                        ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i].SelectedChromosomeStudiedId = ChromosomeStudied.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i].ChromosomeStudiedID));
                                    }
                                    else
                                    {
                                        ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i].SelectedChromosomeStudiedId = ChromosomeStudied.FirstOrDefault(p => p.ID == 0);
                                    }
                                    Karyotyping.Add(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i]);
                          
                             }
                         }


                          dgFISHGrid.ItemsSource = null;
                          dgFISHGrid.ItemsSource = FISH;

                          dgKaryotypingGrid.ItemsSource = null;
                          dgKaryotypingGrid.ItemsSource = Karyotyping;
                             
                     }
                 }
        
             };
             client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
             client.CloseAsync();
        }
        public void fillInitailFISHDetails()
        {
            FISH.Add(new clsPGDFISHVO() { ChromosomeStudiedIdList = ChromosomeStudied, ChromosomeStudiedID =0, TestOrderedIdList = TestOrdered, TestOrderedID = 0,Status=true });
            dgFISHGrid.ItemsSource = FISH;
            Karyotyping.Add(new clsPGDKaryotypingVO { ChromosomeStudiedIdList = ChromosomeStudied, ChromosomeStudiedID = 0, Status = true });
            dgKaryotypingGrid.ItemsSource = Karyotyping;

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/IVFDashboard_PatientFISHReport.aspx?TherapyId=" + PlanTherapyID + "&TherapyUnitId=" + PlanTherapyUnitID + "&PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&OocyteNumber=" + OocyteNumber + "&SerialOocyteNumber=" + SerialOocyteNumber), "_blank");
        }
    }
}
