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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.IVF.PatientList;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.Collections;
using System.Windows.Data;
using PalashDynamics.Converters;
using System.Collections.ObjectModel;
//using EMR;


namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmARTGeneralDetails : ChildWindow
    {
        DateConverter dateConverter;
        public ObservableCollection<clsEMRAddDiagnosisVO> DiagnosisList { get; set; }
        clsCoupleVO coupleDetasils = new clsCoupleVO();
        public event RoutedEventHandler OnSaveButton_Click;
        //public event RoutedEventHandler OnCancelButton_Click;
        public long SelectedPlanTreatmentID;
        public long EMRProcedureID = 0;
        public long EMRProcedureUnitID = 0;
        public bool IsDonorCycle = false;
        public bool IsSurrogacy = false;
        // public bool IsEnabledControl = false;


        public frmARTGeneralDetails(clsCoupleVO CoupleDetails)
        {
            InitializeComponent();
            coupleDetasils = CoupleDetails;
            DataList = new PagedSortableCollectionView<clsEMRAddDiagnosisVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            cmbPhysician.IsEnabled = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = Title + " ( MRNO.:" + coupleDetasils.FemalePatient.MRNo + "/ " + coupleDetasils.FemalePatient.FirstName + " " + coupleDetasils.FemalePatient.LastName + " )";
            grdBackPanel.DataContext = new clsPlanTherapyVO();
            dtpLMPdate.SelectedDate = DateTime.Now.Date;
            //dtStartDateOVarian.SelectedDate = DateTime.Now.Date;
            //dtStartDateStimulation.SelectedDate = DateTime.Now.Date;
            //dtStartDateTrigger.SelectedDate = DateTime.Now.Date;
            dtSpermCollDate.SelectedDate = DateTime.Now.Date;
            txtTrigTime.Value = DateTime.Now;

            fillProtocolType();
            fillMainIndication();
            fillSpermCollection();
            fillPlannedTreatment();
            fillFinalPlannedTreatment();
            fillExternalStimulation();
            fillDoctor();
            dtStartDate.IsEnabled = false;
            dtEndDate.IsEnabled = false;


            dateConverter = new DateConverter();
            //GetPrevious diagnosis Patient aganist
            FillDiagnosisType();
            GetPatientPreviousDiagnosis();
            if (DiagnosisList == null)
                DiagnosisList = new ObservableCollection<clsEMRAddDiagnosisVO>();
            dgDiagnosisList.ItemsSource = DiagnosisList;
            GetPatientDiagnosis();
            txtReason.IsEnabled = false;

        }

        #region Paging
        public PagedSortableCollectionView<clsEMRAddDiagnosisVO> DataList { get; private set; }
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
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetPatientPreviousDiagnosis();
        }
        #endregion



        public List<clsEMRAddDiagnosisVO> DiagnosisListHistory { get; set; }
        List<clsEMRAddDiagnosisVO> PatientDiagnosiDeletedList = new List<clsEMRAddDiagnosisVO>();
        List<MasterListItem> DiagnosisTypeList = new List<MasterListItem>();
        private void GetPatientPreviousDiagnosis()
        {
            try
            {
                DiagnosisListHistory = new List<clsEMRAddDiagnosisVO>();
                clsGetIVFDashboardPatientDiagnosisDataBizActionVO BizAction = new clsGetIVFDashboardPatientDiagnosisDataBizActionVO();
                BizAction.PatientID = coupleDetasils.FemalePatient.PatientID;
                BizAction.PatientUnitID = coupleDetasils.FemalePatient.UnitId;
                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    if ((clsGetIVFDashboardPatientDiagnosisDataBizActionVO)arg.Result != null)
                    {
                        if (((clsGetIVFDashboardPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails != null)
                        {
                            DataList.TotalItemCount = ((clsGetIVFDashboardPatientDiagnosisDataBizActionVO)arg.Result).TotalRows;
                            DataList.Clear();
                            foreach (var item in ((clsGetIVFDashboardPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails)
                            {
                                item.SelectedDiagnosisType = new MasterListItem(item.SelectedDiagnosisType.ID, item.SelectedDiagnosisType.Description);
                                item.DiagnosisTypeList = DiagnosisTypeList;

                                if (item.SelectedDiagnosisType.ID == 1)
                                {
                                    item.Image = "../Icons/green.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 2)
                                {
                                    item.Image = "../Icons/yellow.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 3)
                                {
                                    item.Image = "../Icons/orange.png";
                                }
                                DataList.Add(item);
                            }
                        }
                        DiagnosisListHistory = DataList.ToList();
                        PagedCollectionView pcvDiagnosisListHistory = new PagedCollectionView(DiagnosisListHistory);
                        pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Date", dateConverter));
                        dgPreviousDiagnosisList.ItemsSource = null;
                        dgPreviousDiagnosisList.ItemsSource = pcvDiagnosisListHistory;
                        dgPreviousDiagnosisList.UpdateLayout();
                        pgrPatientpreviousDiagnosis.Source = DataList;
                    }
                    else
                    {
                        ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        private void GetPatientDiagnosis()
        {
            try
            {
                clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO BizAction = new clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO();
                BizAction.PatientID = coupleDetasils.FemalePatient.PatientID;
                BizAction.PatientUnitID = coupleDetasils.FemalePatient.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    if ((clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO)arg.Result != null)
                    {
                        if (((clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails != null)
                        {
                            foreach (var item in ((clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails)
                            {
                                //if (IsEnabledControl == false)
                                //    item.IsEnabled = true;
                                item.SelectedDiagnosisType = new MasterListItem(item.SelectedDiagnosisType.ID, item.SelectedDiagnosisType.Description);
                                item.DiagnosisTypeList = DiagnosisTypeList;

                                if (item.SelectedDiagnosisType.ID == 1)
                                {
                                    item.Image = "../Icons/green.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 2)
                                {
                                    item.Image = "../Icons/yellow.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 3)
                                {
                                    item.Image = "../Icons/orange.png";
                                }
                                DiagnosisList.Add(item);
                            }
                        }
                        dgDiagnosisList.UpdateLayout();
                    }
                    else
                    {
                        ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }
        private void FillDiagnosisType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_DiagnosisMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        DiagnosisTypeList = new List<MasterListItem>();
                        DiagnosisTypeList.Add(new MasterListItem(0, "-- Select --"));
                        DiagnosisTypeList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region MessageBox

        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Fill Combobox
        private void fillProtocolType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFProtocolType;
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
                    cmbProtocol.ItemsSource = null;
                    cmbProtocol.ItemsSource = objList;
                    cmbProtocol.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillMainIndication()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVf_MainIndication;
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
                    cmbMainIndication.ItemsSource = null;
                    cmbMainIndication.ItemsSource = objList;
                    cmbMainIndication.SelectedValue = (long)0;
                    fillMainSubIndication();
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillMainSubIndication()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVf_MainSubIndication;
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
                    cmbSubMainIndication.ItemsSource = null;
                    cmbSubMainIndication.ItemsSource = objList;
                    cmbSubMainIndication.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillSpermCollection()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPlannedSpermCollection;
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

                    if (SelectedPlanTreatmentID == 15)
                    {
                        objList = (from r in objList where r.ID != 11 select r).ToList();

                        //objList.Add(new MasterListItem(0, "-- Select --"));
                        //objList.AddRange((from r in ((clsGetMasterListBizActionVO)args.Result).MasterList where r.ID != 11 select r).ToList());
                    }
                    if (SelectedPlanTreatmentID == 16)
                    {
                        objList = (from r in objList where r.ID != 4 select r).ToList();
                        //objList.Add(new MasterListItem(0, "-- Select --"));
                        //objList.AddRange((from r in ((clsGetMasterListBizActionVO)args.Result).MasterList where r.ID != 4 select r).ToList());
                    }

                    cmbSpermCollection.ItemsSource = null;
                    cmbSpermCollection.ItemsSource = objList;
                    cmbSpermCollection.SelectedValue = (long)0;


                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

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
                    cmbPlannedTreatment.ItemsSource = null;
                    cmbPlannedTreatment.ItemsSource = objList;
                    //cmbPlannedTreatment.SelectedValue = (long)0;

                    //if (SelectedPlanTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID && coupleDetasils.CoupleId != 0)
                    //{
                    //    ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsDonorCycle = true;
                    //    cmbPlannedTreatment.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteReceipentID;
                    //}
                    //else
                        cmbPlannedTreatment.SelectedValue = SelectedPlanTreatmentID;

                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillFinalPlannedTreatment()
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
                    cmbFinalPlanofTreatment.ItemsSource = null;
                    cmbFinalPlanofTreatment.ItemsSource = objList;
                    cmbFinalPlanofTreatment.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                fillSpermSource();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }



        private void fillExternalStimulation()
        {

            List<MasterListItem> Items = new List<MasterListItem>();

            MasterListItem Item = new MasterListItem();
            Item.ID = (int)0;
            Item.Description = "--Select--";
            Items.Add(Item);

            Item = new MasterListItem();
            Item.ID = (int)ExternalStimulation.Yes;
            Item.Description = ExternalStimulation.Yes.ToString();
            Items.Add(Item);

            Item = new MasterListItem();
            Item.ID = (int)ExternalStimulation.No;
            Item.Description = ExternalStimulation.No.ToString();
            Items.Add(Item);

            cmbSimulation.ItemsSource = Items;
            cmbSimulation.SelectedValue = (long)0;



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

                    cmbPhysician.ItemsSource = null;
                    cmbPhysician.ItemsSource = objList;
                    cmbPhysician.SelectedValue = (long)0;

                    //by neena
                    if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID > 0)
                        cmbPhysician.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                    //
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void fillSpermSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_SpermSource;
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
                    cmbSpermSource.ItemsSource = null;
                    cmbSpermSource.ItemsSource = objList;
                    cmbSpermSource.SelectedValue = (long)0;

                    if (SelectedPlanTreatmentID == 15)
                    {
                        cmbSpermSource.SelectedItem = objList.Where(x => x.ID == 1).FirstOrDefault();
                        cmbSpermSource.IsEnabled = false;
                    }
                    else if (SelectedPlanTreatmentID == 16)
                    {
                        cmbSpermSource.SelectedItem = objList.Where(x => x.ID == 2).FirstOrDefault();
                        cmbSpermSource.IsEnabled = false;
                    }

                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }



        #endregion
        public bool GenValidation()
        {
            //if()
            if (cmbPlannedTreatment.SelectedItem == null)
            {

                cmbPlannedTreatment.TextBox.SetValidation("Please Select Planned Treatment");
                cmbPlannedTreatment.TextBox.RaiseValidationError();
                cmbPlannedTreatment.Focus();
                return false;
            }
            else if (((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 0)
            {

                cmbPlannedTreatment.TextBox.SetValidation("Please Select Planned Treatment");
                cmbPlannedTreatment.TextBox.RaiseValidationError();
                cmbPlannedTreatment.Focus();
                return false;
            }
            else if (cmbMainIndication.SelectedItem == null)
            {
                cmbPlannedTreatment.ClearValidationError();
                cmbMainIndication.TextBox.SetValidation("Please Select Main Indication");
                cmbMainIndication.TextBox.RaiseValidationError();
                cmbMainIndication.Focus();
                return false;
            }

            else if (((MasterListItem)cmbMainIndication.SelectedItem).ID == 0)
            {
                cmbPlannedTreatment.ClearValidationError();
                cmbMainIndication.TextBox.SetValidation("Please Select Main Indication");
                cmbMainIndication.TextBox.RaiseValidationError();
                cmbMainIndication.Focus();
                return false;
            }
            else if (cmbSpermSource.SelectedItem == null)
            {
                cmbMainIndication.ClearValidationError();
                cmbSpermSource.TextBox.SetValidation("Please Select Source of Sperm");
                cmbSpermSource.TextBox.RaiseValidationError();
                cmbSpermSource.Focus();
                return false;
            }
            else if (((MasterListItem)cmbSpermSource.SelectedItem).ID == 0)
            {
                cmbMainIndication.ClearValidationError();
                cmbSpermSource.TextBox.SetValidation("Please Select Source of Sperm");
                cmbSpermSource.TextBox.RaiseValidationError();
                cmbSpermSource.Focus();
                return false;
            }
            else if (((MasterListItem)cmbMainIndication.SelectedItem).ID == 8)
            {
                if (cmbSubMainIndication.SelectedItem == null)
                {
                    cmbSpermSource.ClearValidationError();
                    cmbSubMainIndication.TextBox.SetValidation("Please Select Sub Main Indication");
                    cmbSubMainIndication.TextBox.RaiseValidationError();
                    cmbSubMainIndication.Focus();
                    return false;
                }
                else if (((MasterListItem)cmbSubMainIndication.SelectedItem).ID == 0)
                {
                    cmbSpermSource.ClearValidationError();
                    cmbSubMainIndication.TextBox.SetValidation("Please Select Sub Main Indication");
                    cmbSubMainIndication.TextBox.RaiseValidationError();
                    cmbSubMainIndication.Focus();
                    return false;
                }

                if (txtNotes.Text.Trim() == string.Empty)
                {
                    cmbSubMainIndication.ClearValidationError();
                    txtNotes.SetValidation("Please Add Notes");
                    txtNotes.RaiseValidationError();
                    txtNotes.Focus();
                    return false;
                }
                else
                    return true;
            }

            //else if (dtStartDateOVarian.SelectedDate > dtEndDateOvarian.SelectedDate)
            //{
            //    dtStartDateOVarian.SetValidation("Start Date Cannot Be After End Date");
            //    dtStartDateOVarian.RaiseValidationError();
            //    dtStartDateOVarian.Focus();
            //    return false;
            //}
            //else if (dtStartDateStimulation.SelectedDate > dtEndDateStimulation.SelectedDate)
            //{
            //    dtStartDateOVarian.SetValidation("Start Date Cannot Be After End Date");
            //    dtStartDateOVarian.RaiseValidationError();
            //    dtStartDateOVarian.Focus();
            //    return false;
            //}
            //else if (chkCycleCancellation.IsChecked == true)
            //{
            //    if (string.IsNullOrWhiteSpace(txtReason.Text))
            //    {
            //        dtStartDateOVarian.SetValidation("Please Enter Reason For Cancellation");
            //        dtStartDateOVarian.RaiseValidationError();
            //        dtStartDateOVarian.Focus();
            //        return false;

        //    }
            //    else
            //        return true;
            //}
            else
            {
                cmbPlannedTreatment.ClearValidationError();
                cmbMainIndication.ClearValidationError();
                cmbPhysician.ClearValidationError();
                dtOPUDate.ClearValidationError();
                cmbSpermSource.ClearValidationError();
                cmbSubMainIndication.ClearValidationError();
                txtNotes.ClearValidationError();
                //dtStartDateStimulation.ClearValidationError();
                //dtStartDateOVarian.ClearValidationError();
                return true;
            }
            //else if (cmbPhysician.SelectedItem == null)
            //{
            //    cmbPlannedTreatment.ClearValidationError();
            //    cmbMainIndication.ClearValidationError();
            //    cmbPhysician.TextBox.SetValidation("Please Select Physician");
            //    cmbPhysician.TextBox.RaiseValidationError();
            //    cmbPhysician.Focus();
            //    return false;
            //}
            //else if (((MasterListItem)cmbPhysician.SelectedItem).ID == 0)
            //{
            //    cmbPlannedTreatment.ClearValidationError();

            //    cmbMainIndication.ClearValidationError();
            //    cmbPhysician.TextBox.SetValidation("Please Select Physician");
            //    cmbPhysician.TextBox.RaiseValidationError();
            //    cmbPhysician.Focus();
            //    return false;
            //}
            //else if (chkSurrogate.IsChecked == true && txtSurrogateCode.Text == string.Empty)
            //{
            //    string msgTitle = "Palash";
            //    string msgText = "Please Link surrogate ";
            //    MessageBoxControl.MessageBoxChildWindow msgWin =
            //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgWin.Show();
            //    return false;
            //}
            //else if (((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 1 || ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 2 || ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 3 || ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 11)
            //{
            //    if (dtOPUDate.SelectedDate == null)
            //    {
            //        cmbPlannedTreatment.ClearValidationError();
            //        cmbMainIndication.ClearValidationError();
            //        cmbPhysician.ClearValidationError();
            //        dtOPUDate.SetValidation("Please Select OPU Date");
            //        dtOPUDate.RaiseValidationError();
            //        dtOPUDate.Focus();
            //        return false;
            //    }
            //    else
            //    {
            //        return true;
            //    }

            //}

        }

        private void cmdGenerateCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
           // OnCancelButton_Click(this, new RoutedEventArgs());
        }

        private void cmdSaveGeneral_Click(object sender, RoutedEventArgs e)
        {
            if (GenValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Save Therapy General Details?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        saveUpdateGeneralDetails(0, (long)IVFDashBoardTab.TabOverview, 0, "Therapy General Details Saved Successfully");
                    }

                };
                msgWin.Show();
            }
        }
        private void saveUpdateGeneralDetails(long ID, long TabID, long DocumentId, String Msg)
        {
            try
            {
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails = ((clsPlanTherapyVO)grdBackPanel.DataContext);
                //BizAction.TherapyDetails.ID = ID;
                BizAction.TherapyDetails.TabID = TabID;
                BizAction.TherapyDetails.PatientUintId = coupleDetasils.FemalePatient.UnitId;
                BizAction.TherapyDetails.CoupleId = coupleDetasils.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = coupleDetasils.CoupleUnitId;
                if (chkSurrogate.IsChecked == true)
                    BizAction.TherapyDetails.AttachedSurrogate = true;
                else
                    BizAction.TherapyDetails.AttachedSurrogate = false;
                BizAction.TherapyDetails.SurrogateID = SurrogateID;
                BizAction.TherapyDetails.SurrogateUnitID = SurrogateUnitID;
                BizAction.TherapyDetails.SurrogateMRNo = txtSurrogateCode.Text;
                BizAction.TherapyDetails.PatientId = coupleDetasils.FemalePatient.PatientID;
                BizAction.TherapyDetails.PatientUintId = coupleDetasils.FemalePatient.UnitId;

                //added by neena
                BizAction.TherapyDetails.EMRProcedureID = EMRProcedureID;
                BizAction.TherapyDetails.EMRProcedureUnitID = EMRProcedureUnitID;
                BizAction.TherapyDetails.IsDonorCycle = IsDonorCycle;
                BizAction.TherapyDetails.IsSurrogate = IsSurrogacy;
                //

                if (DiagnosisList != null)
                {
                    List<clsEMRAddDiagnosisVO> SavePatientDiagnosiList = new List<clsEMRAddDiagnosisVO>();
                    foreach (clsEMRAddDiagnosisVO item in DiagnosisList)
                    {
                        SavePatientDiagnosiList.Add(item);
                    }
                    BizAction.DiagnosisDetails = SavePatientDiagnosiList;
                }
                #region Data Used For Therapy General Details
                if (TabID == 1)
                {
                    // Added by Anumani as per new Therapy Form
                    //BizAction.TherapyDetails.OPUtDate = dtOPUDate.SelectedDate;
                    //BizAction.TherapyDetails.StartOvarian = dtStartDateOVarian.SelectedDate;
                    //BizAction.TherapyDetails.EndOvarian = dtEndDateOvarian.SelectedDate;
                    //BizAction.TherapyDetails.StartStimulation = dtStartDateStimulation.SelectedDate;
                    //BizAction.TherapyDetails.EndStimulation = dtEndDateStimulation.SelectedDate;
                    //BizAction.TherapyDetails.StartTrigger = dtStartDateTrigger.SelectedDate;
                    //BizAction.TherapyDetails.TriggerTime = txtTrigTime.Value;
                    BizAction.TherapyDetails.CancellationReason = txtReason.Text;
                    BizAction.TherapyDetails.Note = txtNotes.Text;
                    BizAction.TherapyDetails.SpermCollectionDate = dtSpermCollDate.SelectedDate;

                    if (chkCycleCancellation.IsChecked == true)
                    {
                        BizAction.TherapyDetails.IsCancellation = true;
                    }
                    else
                    {
                        BizAction.TherapyDetails.IsCancellation = false;
                    }


                    if (cmbPhysician.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PhysicianId = ((MasterListItem)cmbPhysician.SelectedItem).ID;
                    }
                    if (cmbMainIndication.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.MainInductionID = ((MasterListItem)cmbMainIndication.SelectedItem).ID;
                    }
                    if (cmbSubMainIndication.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.MainSubInductionID = ((MasterListItem)cmbSubMainIndication.SelectedItem).ID;
                    }
                    if (cmbPlannedTreatment.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PlannedTreatmentID = ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID;
                    }
                    if (cmbFinalPlanofTreatment.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.FinalPlannedTreatmentID = ((MasterListItem)cmbFinalPlanofTreatment.SelectedItem).ID;
                    }
                    if (cmbProtocol.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.ProtocolTypeID = ((MasterListItem)cmbProtocol.SelectedItem).ID;
                    }

                    if (txtPlannedNoOfEmbryos.Text != null)
                    {
                        BizAction.TherapyDetails.PlannedEmbryos = txtPlannedNoOfEmbryos.Text.Trim();
                    }
                    if (txtLongtermMedication.Text != null)
                    {
                        BizAction.TherapyDetails.LongtermMedication = txtLongtermMedication.Text.Trim();
                    }
                    if (cmbSimulation.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.ExternalSimulationID = ((MasterListItem)cmbSimulation.SelectedItem).ID;
                    }
                    if (cmbSpermCollection.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PlannedSpermCollectionID = ((MasterListItem)cmbSpermCollection.SelectedItem).ID;
                    }

                    if (cmbSpermSource.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.SpermSource = ((MasterListItem)cmbSpermSource.SelectedItem).ID;
                    }


                }
                #endregion

                #region Service Call (Check Validation)
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", Msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                this.Close();
                            }
                        };
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void chkPill_Checked(object sender, RoutedEventArgs e)
        {
            dtStartDate.IsEnabled = true;
            dtEndDate.IsEnabled = true;
        }

        private void chkPill_Unchecked(object sender, RoutedEventArgs e)
        {
            dtStartDate.IsEnabled = false;
            dtEndDate.IsEnabled = false;
        }
        private void cmdNewExeDrug_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdOutComeCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ExeGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void ExeGrid_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {

        }

        private void ExeGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

        }

        private void ExeGrid_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void TherpayExeRowDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbPlannedNoOfEmbryos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtStartDate.SelectedDate != null)
            {
                dtEndDate.DisplayDateStart = dtStartDate.SelectedDate.Value.AddDays(1);
                dtEndDate.DisplayDateEnd = dtpLMPdate.SelectedDate.Value.AddDays(60);
            }
        }

        private void dtEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtpLMPdate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpLMPdate.SelectedDate != null)
            {
                dtOPUDate.SelectedDate = dtpLMPdate.DisplayDate.AddDays(9);
            }
            else
            {
                dtpLMPdate.SelectedDate = DateTime.Now.Date;
            }
            if (dtpLMPdate.SelectedDate != null)
            {
                dtStartDate.DisplayDateStart = dtpLMPdate.SelectedDate;
                dtStartDate.DisplayDateEnd = dtpLMPdate.SelectedDate.Value.AddDays(60);
                dtEndDate.DisplayDateStart = dtpLMPdate.SelectedDate;
                dtEndDate.DisplayDateEnd = dtpLMPdate.SelectedDate.Value.AddDays(60);
            }
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtPlannedNoOfEmbryos_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtPlannedNoOfEmbryos_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmbProtocol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbProtocol.SelectedItem == null)
            {
                cmbProtocol.SelectedValue = (long)0;
            }
        }

        private void cmbSpermCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSpermCollection.SelectedItem == null)
            {
                cmbSpermCollection.SelectedValue = (long)0;
            }
        }

        private void cmbSimulation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSimulation.SelectedItem == null)
            {
                cmbSimulation.SelectedValue = (long)0;
            }
        }

        private void chkSurrogate_Click(object sender, RoutedEventArgs e)
        {
            if (chkSurrogate.IsChecked == true)
                btnSearchSurrogate.IsEnabled = true;
            else
                btnSearchSurrogate.IsEnabled = false;
        }
        private void btnSearchSurrogate_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.PatientCategoryID = 10;
            Win.IsFromSurrogacyModule = true;
            Win.Show();
        }
        long SurrogateID = 0;
        long SurrogateUnitID = 0;
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch ObjWin = (PatientSearch)sender;
            if (ObjWin.DialogResult == true)
            {
                if (ObjWin.PatientCategoryID == 10)
                    txtSurrogateCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
                SurrogateID = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).PatientID;
                SurrogateUnitID = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).UnitId;
            }
        }

        private void cmbPlannedTreatment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPlannedTreatment.SelectedItem != null && ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID > 0)
            {
                SetDetailsTherapyWise(((MasterListItem)cmbPlannedTreatment.SelectedItem).ID);
            }

        }
        private void SetDetailsTherapyWise(long pPlanTherapyID)
        {
            txtSurrogateCode.Text = string.Empty;
            chkSurrogate.IsChecked = false;
            switch (pPlanTherapyID)
            {
                case 1://IVF
                    chkSurrogate.IsEnabled = true;
                    break;

                case 2://ICSI
                    chkSurrogate.IsEnabled = true;
                    break;
                case 3://IVF - ICSI
                    chkSurrogate.IsEnabled = true;
                    break;

                case 5://Thaw/Transfer
                    chkSurrogate.IsEnabled = true;
                    break;

                case 11://OocyteDonation
                    chkSurrogate.IsEnabled = false;
                    break;
                case 12://Oocyte Receipant
                    chkSurrogate.IsEnabled = true;
                    break;

                case 15: // IUI(H)
                    chkSurrogate.IsEnabled = false;
                    break;

                case 16: // IUI(D)
                    chkSurrogate.IsEnabled = false;
                    break;

                case 14: // Embryo Recepient
                    chkSurrogate.IsEnabled = true;
                    break;

                default:
                    chkSurrogate.IsEnabled = false; ;
                    break;


            }

        }

        private void cmdAddDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            EMR.frmDiagnosisSelection Win = new EMR.frmDiagnosisSelection();
            Win.OnAddButton_Click += new RoutedEventHandler(Win_OnAddButton_Click);
            Win.Show();
        }

        void Win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((EMR.frmDiagnosisSelection)sender).DialogResult == true)
                {
                    foreach (var item in (((EMR.frmDiagnosisSelection)sender).DiagnosisList))
                    {
                        if (DiagnosisList.Count > 0)
                        {
                            var item1 = from r in DiagnosisList
                                        //where (r.DiagnosisId == item.ID)
                                        where (r.Code == item.Code)
                                        select r;
                            if (item1.ToList().Count == 0)
                            {
                                clsEMRAddDiagnosisVO OBj = new clsEMRAddDiagnosisVO();
                                OBj.Diagnosis = item.Diagnosis;
                                OBj.DiagnosisId = item.ID;
                                OBj.Code = item.Code;
                                OBj.ICDId = item.ICDId;
                                OBj.DiagnosisTypeId = item.DiagnosisTypeId;
                                OBj.IsEnabled = true;
                                OBj.TemplateID = item.TemplateID;
                                OBj.TemplateName = item.TemplateName;
                                OBj.DiagnosisTypeList = DiagnosisTypeList;
                                OBj.SelectedDiagnosisType = new MasterListItem(0, "-- Select --");
                                DiagnosisList.Add(OBj);
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Diagnosis already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            clsEMRAddDiagnosisVO OBj = new clsEMRAddDiagnosisVO();
                            OBj.Diagnosis = item.Diagnosis;
                            OBj.DiagnosisId = item.ID;
                            OBj.Code = item.Code;
                            OBj.Diagnosis = item.Diagnosis;
                            OBj.ICDId = item.ICDId;
                            OBj.DiagnosisTypeId = item.DiagnosisTypeId;
                            OBj.IsEnabled = true;
                            OBj.TemplateID = item.TemplateID;
                            OBj.TemplateName = item.TemplateName;
                            OBj.DiagnosisTypeList = DiagnosisTypeList;
                            OBj.SelectedDiagnosisType = new MasterListItem(0, "-- Select --");
                            DiagnosisList.Add(OBj);
                        }
                    }
                    dgDiagnosisList.ItemsSource = DiagnosisList;
                    dgDiagnosisList.UpdateLayout();
                    dgDiagnosisList.Focus();
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void cmbDiagnosis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdDeleteDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            if (dgDiagnosisList.SelectedItem != null)
            {
                string msgText = "Are you sure you want to Delete the selected Diagnosis ?"; //((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ConfirmDeleteDiagnosis_Msg");

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            clsEMRAddDiagnosisVO objVo = (clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem;
                            objVo.Status = false;
                            objVo.IsDeleted = true;
                            DiagnosisList.RemoveAt(dgDiagnosisList.SelectedIndex);
                            if (objVo.ID != null && objVo.ID > 0)
                                PatientDiagnosiDeletedList.Add(objVo);
                            dgDiagnosisList.UpdateLayout();
                        }
                    }
                };
                msgWD.Show();
            }
        }

        private void cmbSpermSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtReason_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void PrescTriggeer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PresStimulation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PresOvarian_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkIsSetCancellation_Click(object sender, RoutedEventArgs e)
        {
            if (chkCycleCancellation.IsChecked == true)
            {
                txtReason.IsEnabled = true;
            }
            else
            {
                txtReason.IsEnabled = false;
            }
        }

        private void cmbMainIndication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbMainIndication.SelectedItem).ID == 8)
                cmbSubMainIndication.IsEnabled = true;
            else
            {
                cmbSubMainIndication.IsEnabled = false;
                cmbSubMainIndication.SelectedValue = (long)0;
            }
        }
    }
}
