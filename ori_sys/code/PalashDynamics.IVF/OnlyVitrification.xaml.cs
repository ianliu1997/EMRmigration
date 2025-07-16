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
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.IVF.PatientList;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using MessageBoxControl;
using System.IO;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;

namespace PalashDynamics.IVF
{
    public partial class OnlyVitrification : UserControl, IInitiateCIMS
    {
        #region Variables
        WaitIndicator wait = new WaitIndicator();
        public bool IsPatientExist;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        ListBox lstFUBox;
   
        #endregion

        #region Properties
        public bool IsExpendedWindow { get; set; }
        public string Impression { get; set; }
        public Boolean IsEdit { get; set; }
        public Int64 ID { get; set; }
        public Int64 UnitID { get; set; }

        private ObservableCollection<clsGetVitrificationDetailsVO> _VitriDetails = new ObservableCollection<clsGetVitrificationDetailsVO>();
        public ObservableCollection<clsGetVitrificationDetailsVO> VitriDetails
        {
            get { return _VitriDetails; }
            set { _VitriDetails = value; }
        }

        private ObservableCollection<clsGetVitrificationDetailsVO> _RemoveVitriDetails = new ObservableCollection<clsGetVitrificationDetailsVO>();
        public ObservableCollection<clsGetVitrificationDetailsVO> RemoveVitriDetails
        {
            get { return _RemoveVitriDetails; }
            set { _RemoveVitriDetails = value; }
        }

        private List<FileUpload> _FileUpLoadList = new List<FileUpload>();
        public List<FileUpload> FileUpLoadList
        {
            get
            {
                return _FileUpLoadList;
            }
            set
            {
                _FileUpLoadList = value;
            }
        }

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
       
       
        private List<MasterListItem> _Grade = new List<MasterListItem>();
        public List<MasterListItem> Grade
        {
            get
            {
                return _Grade;
            }
            set
            {
                _Grade = value;
            }
        }

        private List<MasterListItem> _CanList = new List<MasterListItem>();
        public List<MasterListItem> CanList
        {
            get
            {
                return _CanList;
            }
            set
            {
                _CanList = value;
            }
        }
       
       
        private List<MasterListItem> _CellStage = new List<MasterListItem>();
        public List<MasterListItem> CellStage
        {
            get
            {
                return _CellStage;
            }
            set
            {
                _CellStage = value;
            }
        }

        // Added by Saily P

        private List<MasterListItem> _Straw = new List<MasterListItem>();
        public List<MasterListItem> Straw
        {
            get
            {
                return _Straw;
            }
            set
            {
                _Straw = value;
            }
        }

        private List<MasterListItem> _GobletShape = new List<MasterListItem>();
        public List<MasterListItem> GobletShape
        {
            get
            {
                return _GobletShape;
            }
            set
            {
                _GobletShape = value;
            }
        }

        private List<MasterListItem> _GobletSize = new List<MasterListItem>();
        public List<MasterListItem> GobletSize
        {
            get
            {
                return _GobletSize;
            }
            set
            {
                _GobletSize = value;
            }
        }

        private List<MasterListItem> _Canister = new List<MasterListItem>();
        public List<MasterListItem> Canister
        {
            get
            {
                return _Canister;
            }
            set
            {
                _Canister = value;
            }
        }

        private List<MasterListItem> _Tank = new List<MasterListItem>();
        public List<MasterListItem> Tank
        {
            get
            {
                return _Tank;
            }
            set
            {
                _Tank = value;
            }
        }
        #endregion  

        #region Validation
        public Boolean Validation()
        {
            //if (string.IsNullOrEmpty(txtVitriNo.Text.Trim()))
            //{
            //    txtVitriNo.SetValidation("Please Enter Only Vitrification Number");
            //    txtVitriNo.RaiseValidationError();
            //    txtVitriNo.Focus();
            //    return false;
            //}
            //else 
            if (dtVitrificationDate.SelectedDate == null)
            {
               // txtVitriNo.ClearValidationError();
                dtVitrificationDate.SetValidation("Please Select Only Vitrification Date");
                dtVitrificationDate.RaiseValidationError();
                dtVitrificationDate.Focus();
                return false;
            }
            else if (txtTime.Value == null)
            {
                //txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.SetValidation("Please Select Only Vitrification Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                return false;
            }
            else if (dtPickUpDate.SelectedDate == null)
            {
                //txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();

                dtPickUpDate.SetValidation("Please Select PickUp Date");
                dtPickUpDate.RaiseValidationError();
                dtPickUpDate.Focus();
                return false;
            }
            //else if(cmbSrcProtocolType.SelectedItem==null || ((MasterListItem)cmbSrcProtocolType.SelectedItem).ID==0)
            //{
            //    txtVitriNo.ClearValidationError();
            //    dtVitrificationDate.ClearValidationError();
            //    txtTime.ClearValidationError();
            //    dtPickUpDate.ClearValidationError();
            //    cmbSrcProtocolType.TextBox.SetValidation("Please Select Protocol Type");
            //    cmbSrcProtocolType.TextBox.RaiseValidationError();
            //    cmbSrcProtocolType.Focus();
            //    return false;
            //}
            else if (cmbSrcOocyte.SelectedItem == null || ((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 0)
            {
               // txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();
                cmbSrcProtocolType.TextBox.ClearValidationError();

                
                cmbSrcOocyte.TextBox.SetValidation("Please Select Source of Oocyte");
                cmbSrcOocyte.TextBox.RaiseValidationError();
                cmbSrcOocyte.Focus();
                return false;
            }

            else if (cmbSrcSemen.SelectedItem == null || ((MasterListItem)cmbSrcSemen.SelectedItem).ID == 0)
            {
              //  txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();
                cmbSrcProtocolType.TextBox.ClearValidationError();
                cmbSrcOocyte.TextBox.ClearValidationError();

                cmbSrcSemen.TextBox.SetValidation("Please Select Source of Semen");
                cmbSrcSemen.TextBox.RaiseValidationError();
                cmbSrcSemen.Focus();
                return false;
            }
            else if ((((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 2 || ((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 3) && txtOocyteDonorCode.Text.Length==0)
            {
             //   txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();
                cmbSrcProtocolType.TextBox.ClearValidationError();
                cmbSrcOocyte.TextBox.ClearValidationError();
                cmbSrcSemen.TextBox.ClearValidationError();
             
             


                txtOocyteDonorCode.SetValidation("Please Select Oocyte Donor Code");
                txtOocyteDonorCode.RaiseValidationError();
                txtOocyteDonorCode.Focus();
                return false;
            }
            else if ((((MasterListItem)cmbSrcSemen.SelectedItem).ID == 2 || ((MasterListItem)cmbSrcSemen.SelectedItem).ID == 3) && txtSemenDonorCode.Text.Length==0)
            {
               // txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();
                cmbSrcProtocolType.TextBox.ClearValidationError();
                cmbSrcOocyte.TextBox.ClearValidationError();
                cmbSrcSemen.TextBox.ClearValidationError();
                txtOocyteDonorCode.ClearValidationError();
               

                txtSemenDonorCode.SetValidation("Please Select Semen Donor Code");
                txtSemenDonorCode.RaiseValidationError();
                txtSemenDonorCode.Focus();
                return false;
            }

            else if (VitriDetails.Count <= 0)
            {
               // txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();
                cmbSrcProtocolType.TextBox.ClearValidationError();
                cmbSrcOocyte.TextBox.ClearValidationError();
                cmbSrcSemen.TextBox.ClearValidationError();
                txtOocyteDonorCode.ClearValidationError();
                txtSemenDonorCode.ClearValidationError();
               
               


                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Only Vitrification Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            else
            {
               // txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();
                cmbSrcProtocolType.TextBox.ClearValidationError();
                cmbSrcOocyte.TextBox.ClearValidationError();
                cmbSrcSemen.TextBox.ClearValidationError();
                txtOocyteDonorCode.ClearValidationError();
                txtSemenDonorCode.ClearValidationError();
                return true;
            }

        }


        #endregion
       

        #region Constructor
        public OnlyVitrification()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(OnlyVitrification_Loaded);
            this.Unloaded += new RoutedEventHandler(OnlyVitrification_Unloaded);
        }
        #endregion

        #region Loaded Event
        void OnlyVitrification_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsExpendedWindow)
            {
                try
                {
                    txtVitriNo.Text = "Auto Generate";
                    if (IsPatientExist == false)
                    {
                        ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    {
                        SSemenMaster();
                        IsExpendedWindow = true;

                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region Unloaded Event

        void OnlyVitrification_Unloaded(object sender, RoutedEventArgs e)
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
                client.GlobalDeleteFileAsync("../UserUploadedFilesByTemplateTool", AttachedFileNameList);
            }
            catch (Exception Exception) { }
        }

        #endregion

        #region Check Patient Is Selected/Not
        public void Initiate(string Mode)
        {
            switch (Mode.ToUpper())
            {
                case "NEW":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        #endregion

        #region Fill Master Item

        public void SSemenMaster()
        {
            try
            {
                wait.Show();
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0,new MasterListItem( 0, "--Select--" ));
                        cmbSrcSemen.ItemsSource = ((clsGetMasterListBizActionVO)args.Result).MasterList; ;
                        cmbSrcSemen.SelectedValue = (long)0;
                        fillGrade();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void fillGrade()
        {
            try
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Grade = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillOocyteSource();
                    }

                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void fillOocyteSource()
        {
            try
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0,new MasterListItem(0, "--Select--"));
                        cmbSrcOocyte.ItemsSource = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        cmbSrcOocyte.SelectedValue = (long)0;
                        fillCanID();
                    }
                    //wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }

        }

        private void fillCanID()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        CanList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillStrawList();
                        //fillPlan();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillStrawList()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFStrawMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Straw = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillGobletshape();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillGobletshape()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletShapeMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        GobletShape = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillGobletSize();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillGobletSize()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletSizeMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        GobletSize = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillCanister();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillCanister()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanisterMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Canister = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillTank();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillTank()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFTankMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Tank = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillPlan();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void fillPlan()
        {
            try
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0,new MasterListItem(0, "--Select--"));
                        cmbSrcProtocolType.ItemsSource = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        cmbSrcProtocolType.SelectedValue = (long)0;
                        FillCellStage();
                    }

                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillCellStage()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_FertilizationStageMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        CellStage = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillCoupleDetails();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        #endregion

        #region Fill Couple Details

        //Added by Saily P on 25.09.12 to get the patient details in case of Donor
        private void LoadPatientHeader()
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
                    #region Commented by Saily P on 25.09.12
                    //if (BizAction.PatientDetails.GenderID == 1)
                    //{
                    //    Male.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //    if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                    //    {
                    //        Female.DataContext = BizAction.PatientDetails.SpouseDetails;
                    //        CoupleInfo.Visibility = Visibility.Visible;
                    //        PatientInfo.Visibility = Visibility.Collapsed;
                    //    }
                    //    else
                    //    {
                    //        PatientInfo.Visibility = Visibility.Visible;
                    //        CoupleInfo.Visibility = Visibility.Collapsed;
                    //        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //    }
                    //}
                    //else
                    //{
                    //    Female.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //    if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                    //    {
                    //        Male.DataContext = BizAction.PatientDetails.SpouseDetails;
                    //        CoupleInfo.Visibility = Visibility.Visible;
                    //        PatientInfo.Visibility = Visibility.Collapsed;
                    //    }
                    //    else
                    //    {
                    //        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //        PatientInfo.Visibility = Visibility.Visible;
                    //        CoupleInfo.Visibility = Visibility.Collapsed;
                    //    }
                    //}
                    #endregion
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        
        private void fillCoupleDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
            {
                PatientInfo.Visibility = Visibility.Visible;
                CoupleInfo.Visibility = Visibility.Collapsed;
                LoadPatientHeader();
                //wait.Close();
                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Only Vitrification is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //msgW1.Show();

                //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Female.ToString())
                {
                    Female.DataContext = ((IApplicationConfiguration)App.Current).SelectedPatient;
                    if (IsEdit == true)
                    {
                        fillOnlyVitrificationDetails();
                    }
                    else
                    {
                        fillInitailOnlyVitrificationDetails();
                    }
                }
                else if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Male.ToString())
                {
                    Male.DataContext = ((IApplicationConfiguration)App.Current).SelectedPatient;
                    if (IsEdit == true)
                    {
                        fillOnlyVitrificationDetails();
                    }
                    else
                    {
                        fillInitailOnlyVitrificationDetails();
                    }
                }

            }
            else
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
                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null)
                            {
                                if (CoupleDetails.CoupleId == 0)
                                {
                                    LoadPatientHeader();
                                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    //msgW1.Show();
                                    //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                                }
                                else
                                {
                                    PatientInfo.Visibility = Visibility.Collapsed;
                                    CoupleInfo.Visibility = Visibility.Visible;
                                    //getEMRDetails(BizAction.CoupleDetails.FemalePatient, "F");
                                    //getEMRDetails(BizAction.CoupleDetails.MalePatient, "M");
                                    GetHeightAndWeight(BizAction.CoupleDetails);
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
                                    }
                                    if (IsEdit == true)
                                    {
                                        fillOnlyVitrificationDetails();
                                    }
                                    else
                                    {
                                        fillInitailOnlyVitrificationDetails();
                                    }
                                }
                            }
                            else
                            {
                                LoadPatientHeader();
                            }
                            
                        }
                        wait.Close();
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                
                //Female.DataContext = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient;
                //Male.DataContext = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient;
                
            }

        }

        #endregion

        #region  Fill Vitrification Details

        public void fillOnlyVitrificationDetails()
        {
            try
            {

                clsGetVitrificationDetailsBizActionVO BizAction = new clsGetVitrificationDetailsBizActionVO();
                BizAction.Vitrification = new clsGetVitrificationVO();
                BizAction.IsEdit = IsEdit;
                BizAction.ID = ID;
                BizAction.UnitID = UnitID;
                BizAction.FromID = (int)IVFLabWorkForm.OnlyVitrification;
                if (CoupleDetails != null)
                {
                    BizAction.CoupleID = CoupleDetails.CoupleId;
                    BizAction.CoupleUintID = CoupleDetails.CoupleUnitId;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (IsEdit)
                        {
                            VitrificationMainGrid.DataContext = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification;
                           
                            if (((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.IsFreezed == true)
                            {
                                cmdSave.IsEnabled = false;
                            }
                            chkFreeze.IsChecked = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.IsFreezed;
                        }
                        else
                        {
                            rdoYes.IsChecked = true;
                        }
                        if (((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting.Count <= 0)
                        {
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting.Add(new FileUpload());
                            FileUpLoadList = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting;
                        }
                        else
                        {
                            FileUpLoadList = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting;
                        }
                       
                        for (int i = 0; i < ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails.Count; i++)
                        {
                            txtOocyteDonorCode.Text = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].OSCode;
                            txtSemenDonorCode.Text = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SSCode;
                            cmbSrcOocyte.SelectedValue =Convert.ToInt64( ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytes);
                            cmbSrcSemen.SelectedValue =Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemen);
                            cmbSrcProtocolType.SelectedValue =Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolType);



                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStageList = CellStage;
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange));
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStangeID = Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange);

                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GradeList = Grade;
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade));
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GradeID = Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade);

                            
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanIdList = CanList;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID;


                            //Added by Saily P 
                            //Straw
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawIdList = Straw;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId;
                           
                            //Goblet Shape
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeList = GobletShape;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId;

                            //Goblet Size
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeList = GobletSize;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId;

                            //Canister
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterIdList = Canister;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId;

                            //Tank
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankList = Tank;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId;


                            VitriDetails.Add(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i]);
                            wait.Close();
                        }



                        dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                        LoadFURepeaterControl();
                        wait.Close();
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        public void fillInitailOnlyVitrificationDetails()
        {
            VitriDetails.Add(new clsGetVitrificationDetailsVO() { CanID = "0", StrawId="0", GobletShapeId="0", GobletSizeId="0",CanisterId="0", TankId="0", CanIdList = CanList, StrawIdList=Straw, GobletSizeList=GobletSize, GobletShapeList=GobletShape,CanisterIdList=Canister, TankList=Tank, EmbNo = "Auto Generated", GradeList = Grade, CellStageList = CellStage ,SerialOccyteNo="Auto Generated"});
            dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
            LoadFURepeaterControl();
            wait.Close();
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
            }
        }
        #endregion
        int j=0;
        #region Save Event
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save Only Vitrification Details";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        ImpressionWindow winImp = new ImpressionWindow();
                        winImp.Day = true;
                        winImp.Impression = Impression;
                        winImp.OnSaveClick += new RoutedEventHandler(winImp_OnSaveClick);
                        winImp.Show();
                    }
                };
                msgWin.Show();
            }
          
        }
        private void SaveVitrification()
        {
            try
            {
                wait.Show();
                ObservableCollection<clsGetVitrificationDetailsVO> newvit = VitriDetails;
                clsAddUpdateVitrificationBizActionVO BizAction = new clsAddUpdateVitrificationBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.Vitrification = new clsGetVitrificationVO();
                BizAction.Vitrification.Impression = Impression;
                BizAction.Vitrification.IsOnlyVitrification = true;
               
                BizAction.ID = ID;
                BizAction.UintID = UnitID;
                BizAction.CoupleID = CoupleDetails.CoupleId;
                BizAction.CoupleUintID = CoupleDetails.CoupleUnitId;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                
                if (rdoNo.IsChecked == true)
                {
                    BizAction.Vitrification.ConsentForm = false;
                }
                else
                {
                    BizAction.Vitrification.ConsentForm = true;
                }
                BizAction.Vitrification.IsOnlyVitrification = true;
                BizAction.Vitrification.IsFreezed = (bool)chkFreeze.IsChecked;
                BizAction.Vitrification.PickupDate = dtPickUpDate.SelectedDate.Value;
                BizAction.Vitrification.VitrificationDate = dtVitrificationDate.SelectedDate.Value.Date;
                BizAction.Vitrification.VitrificationDate = BizAction.Vitrification.VitrificationDate.Value.Add(txtTime.Value.Value.TimeOfDay);
                BizAction.Vitrification.VitrificationNo = txtVitriNo.Text.Trim();



                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    VitriDetails[i].ProtocolTypeID = ((MasterListItem)cmbSrcProtocolType.SelectedItem).ID;
                    VitriDetails[i].SOOcytesID = ((MasterListItem)cmbSrcOocyte.SelectedItem).ID;
                    VitriDetails[i].SOSemenID = ((MasterListItem)cmbSrcSemen.SelectedItem).ID;
                    VitriDetails[i].SSCode = txtSemenDonorCode.Text.Trim();
                    VitriDetails[i].OSCode = txtOocyteDonorCode.Text.Trim();
                    VitriDetails[i].CellStangeID = VitriDetails[i].SelectedCellStage.ID;
                    VitriDetails[i].GradeID = VitriDetails[i].SelectedGrade.ID;
                    VitriDetails[i].CanID = VitriDetails[i].SelectedCanId.ID.ToString();
                    VitriDetails[i].StrawId = VitriDetails[i].SelectedStrawId.ID.ToString();
                    VitriDetails[i].GobletShapeId = VitriDetails[i].SelectedGobletShape.ID.ToString();
                    VitriDetails[i].GobletSizeId = VitriDetails[i].SelectedGobletSize.ID.ToString();
                    VitriDetails[i].CanisterId = VitriDetails[i].SelectedCanisterId.ID.ToString();
                    VitriDetails[i].TankId = VitriDetails[i].SelectedTank.ID.ToString();
                    if (VitriDetails[i].EmbNo.Equals("Auto Generated"))
                    {
                        VitriDetails[i].EmbNo = "0";
                    }
                    //By Anjali............
                    j = i + 1;
                    VitriDetails[i].SerialOccyteNo =Convert.ToString(j);
                    //................................
                }
                for (int i = 0; i < RemoveVitriDetails.Count; i++)
                {
                    VitriDetails.Add(RemoveVitriDetails[i]);
                }


                BizAction.Vitrification.VitrificationDetails = ((List<clsGetVitrificationDetailsVO>)VitriDetails.ToList());
                BizAction.Vitrification.FUSetting = FileUpLoadList;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string txtmsg = "";
                        if (IsEdit == true)
                        {
                            txtmsg = "Only Vitrification Details Updated Successfully";
                        }
                        else
                        {
                            txtmsg = "Only Vitrification Details Saved Successfully";
                        }

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", txtmsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        LabDaysSummary LabSumm = new LabDaysSummary();
                        LabSumm.IsPatientExist = true;
                        ((IApplicationConfiguration)App.Current).OpenMainContent(LabSumm);
                        wait.Close();

                    }
                    else
                    {
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                #endregion

            }
            catch (Exception ex)
            {
                wait.Close();
            }

        }

        void winImp_OnSaveClick(object sender, RoutedEventArgs e)
        {
            ImpressionWindow ObjImp = (ImpressionWindow)sender;
            Impression = ObjImp.Impression;
            SaveVitrification();
        }
        #endregion

        #region Cancel
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            LabDaysSummary LabSumm = new LabDaysSummary();
            LabSumm.IsPatientExist = true;
            ((IApplicationConfiguration)App.Current).OpenMainContent(LabSumm);
        }
        #endregion

        #region Media Cilck
        private void MediaCilck_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails Win = new MediaDetails();
            if (((clsGetVitrificationDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails == null)
                ((clsGetVitrificationDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails = new List<clsFemaleMediaDetailsVO>();
            Win.ItemList = GetCollection(((clsGetVitrificationDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails);
            Win.Tag = ((clsGetVitrificationDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails;
            Win.OnSaveButton_Click+=new RoutedEventHandler(Win_OnSaveButton_Click1);
            Win.Show();
        }

        void Win_OnSaveButton_Click1(object sender, RoutedEventArgs e)
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
                        //MediaList.Add(objItem);
                    }
                }
            }
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

        #endregion

        #region Selection Changed (Colorcode and CanId)
        private void cmbCanID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteBox)sender).Name.Equals("cmbCanID"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].CanID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbCellStage"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].CellStangeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGrade"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].GradeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbStraw"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].StrawId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGobletShape"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].GobletShapeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGobletSize"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].GobletSizeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbCanisterId"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].CanisterId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbTankId"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].TankId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
            
        }

        
        private void ColorSelector_SelectionChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < VitriDetails.Count; i++)
            {
                if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                {
                    VitriDetails[i].SelectesColor = ((Liquid.ColorSelector)sender).Selected;
                    VitriDetails[i].ColorCode = ((Liquid.ColorSelector)sender).Selected.ToString();
                }
            }
        }
        #endregion

        #region Search Donor
        private void btnSearchSemenDonor_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.PatientCategoryID = 9;
            Win.Show();
        }

        private void btnSearchOocyteDonor_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);   
            Win.PatientCategoryID = 8;
            Win.Show();
        }

        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
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
        #endregion

        #region Add and Remove Only Vitrification

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (VitriDetails == null)
            {
                VitriDetails = new ObservableCollection<clsGetVitrificationDetailsVO>();
            }
            VitriDetails.Add(new clsGetVitrificationDetailsVO() { CanID = "0", StrawId = "0", GobletShapeId = "0", GobletSizeId = "0", CanisterId = "0", TankId = "0", CanIdList = CanList, StrawIdList = Straw, GobletShapeList = GobletShape, GobletSizeList = GobletSize, CanisterIdList = Canister, TankList = Tank, EmbNo = "Auto Generated", GradeList = Grade, CellStageList = CellStage, SerialOccyteNo = "Auto Generated" });
            dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dgVitrificationDetilsGrid.SelectedItem != null)
            {
                if (VitriDetails.Count > 1)
                {
                    VitriDetails[dgVitrificationDetilsGrid.SelectedIndex].EmbNo = "-1";
                    if (VitriDetails[dgVitrificationDetilsGrid.SelectedIndex].ID != 0 && !VitriDetails[dgVitrificationDetilsGrid.SelectedIndex].EmbNo.Equals("Auto Generated"))
                    {
                        RemoveVitriDetails.Add(VitriDetails[dgVitrificationDetilsGrid.SelectedIndex]);
                    }
                    VitriDetails.RemoveAt(dgVitrificationDetilsGrid.SelectedIndex);
                }
            }

            dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
        }

        #endregion

        #region Source of Semen and Source of Oocytes Selection Changed Code 

        private void cmbSrcSemen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbSrcSemen.SelectedItem != null)
            {
                if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 2 || ((MasterListItem)cmbSrcSemen.SelectedItem).ID == 3)
                {
                    txtSemenDonorCode.ClearValidationError();
                    btnSearchSemenDonor.IsEnabled = true;
                    txtSemenDonorCode.Text = "";
                }
                else
                {
                    btnSearchSemenDonor.IsEnabled = false;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.ClearValidationError();
                }
            }
        }

        private void cmbSrcOocyte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSrcOocyte.SelectedItem != null)
            {
                if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 2 || ((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 3)
                {
                    btnSearchOocyteDonor.IsEnabled = true;
                    txtOocyteDonorCode.ClearValidationError();
                    txtOocyteDonorCode.Text = "";
                    
                }
                else
                {
                    btnSearchOocyteDonor.IsEnabled = false;
                    txtOocyteDonorCode.Text = "";
                    txtOocyteDonorCode.ClearValidationError();
                }
            }
        }

        #endregion

        #region Upload File
        private void LoadFURepeaterControl()
        {

            lstFUBox = new ListBox();
            if (FileUpLoadList == null || FileUpLoadList.Count == 0)
            {
                FileUpLoadList = new List<FileUpload>();
                FileUpLoadList.Add(new FileUpload());
            }

            lstFUBox.DataContext = FileUpLoadList;


            if (FileUpLoadList != null)
            {
                for (int i = 0; i < FileUpLoadList.Count; i++)
                {
                    FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
                    FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);
                    FUrci.OnBrowseClick += new RoutedEventHandler(FUrci_OnBrowseClick);
                    FUrci.OnViewClick += new RoutedEventHandler(FUrci_OnViewClick);

                    FileUpLoadList[i].Index = i;
                    FileUpLoadList[i].Command = ((i == FileUpLoadList.Count - 1) ? "Add" : "Remove");

                    FUrci.DataContext = FileUpLoadList[i];
                    lstFUBox.Items.Add(FUrci);
                }
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
                    string FullFile = "ET" + ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Index + ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName;

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FullFile });
                            AttachedFileNameList.Add(FullFile);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", FullFile, ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Data);
                }
                else
                {

                }
            }
            else
            {
                MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This File is not uploaded. Please upload the File then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
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

        void FUrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                FileUpLoadList.RemoveAt(((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Index);
            }
            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                if (FileUpLoadList.Where(Items => Items.FileName == null).Any() == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW13 =
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select File.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW13.Show();


                }
                else
                {
                    FileUpLoadList.Add(new ValueObjects.IVFPlanTherapy.FileUpload());
                }
            }
            lstFUBox.Items.Clear();
            for (int i = 0; i < FileUpLoadList.Count; i++)
            {
                FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
                FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);
                FUrci.OnBrowseClick += new RoutedEventHandler(FUrci_OnBrowseClick);
                FUrci.OnViewClick += new RoutedEventHandler(FUrci_OnViewClick);


                FileUpLoadList[i].Index = i;
                FileUpLoadList[i].Command = ((i == FileUpLoadList.Count - 1) ? "Add" : "Remove");

                FUrci.DataContext = FileUpLoadList[i];
                lstFUBox.Items.Add(FUrci);
            }
        }
        #endregion


        #region Print

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            long _UnitID = 0;
            long _ID = 0;
            _ID = ID;
            _UnitID = UnitID;


            // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source,  "../Reports/InventoryPharmacy/GRNPrint.aspx?"+Parameters , "_blank");

            string URL = "../Reports/IVF/VitrificationReport.aspx?ID=" + _ID + "&UnitID=" + _UnitID + "&FormId=" + (int)IVFLabWorkForm.OnlyVitrification;

            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

        }

        #endregion Print

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
            if (MaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.MalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {
            if (FemaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.FemalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }
    }
}
