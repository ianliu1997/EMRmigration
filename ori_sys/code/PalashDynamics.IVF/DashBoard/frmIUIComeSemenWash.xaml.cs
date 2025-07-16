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
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Windows.Browser;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmIUIComeSemenWash : UserControl
    {

        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public bool IsClosed;
        public long PlannedSpermCollection;
        public long PlannedTreatmentID;
        public frmIUIComeSemenWash()
        {
            InitializeComponent();
        }

        #region FillCombox
        //private void FillInseminatedBy()
        //{
        //    clsGetEmbryologistBizActionVO BizAction = new clsGetEmbryologistBizActionVO();
        //    BizAction.MasterList = new List<MasterListItem>();
        //    BizAction.UnitId = 0;
        //    BizAction.DepartmentId = 0;
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetEmbryologistBizActionVO)arg.Result).MasterList);

        //            cmbInseminatedBy.ItemsSource = null;
        //            cmbInseminatedBy.ItemsSource = objList;
        //            cmbInseminatedBy.SelectedItem = objList[0];

        //            cmbWitnessedBy.ItemsSource = null;
        //            cmbWitnessedBy.ItemsSource = objList;
        //            cmbWitnessedBy.SelectedItem = objList[0];
        //            //   fillfillInseminationmethod();
        //            FillCollectionMethod();   //  as per the requirements of Milann
        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}
        private void fillfillInseminationmethod()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_InseminationmethodMaster;
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

                    cmbInseminationMethod.ItemsSource = null;
                    cmbInseminationMethod.ItemsSource = objList;
                    cmbInseminationMethod.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {

                    }
                    fillInseminationLocationMaster();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillInseminationLocationMaster()
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_InseminationLocationMaster;
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

                    cmbnseminationLocation.ItemsSource = null;
                    cmbnseminationLocation.ItemsSource = objList;
                    cmbnseminationLocation.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {

                    }
                    FillCollectionMethod();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void FillCollectionMethod()
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

                    cmbCollection.ItemsSource = null;
                    cmbCollection.ItemsSource = objList;
                    cmbCollection.SelectedItem = objList[0];

                    //cmbCollection.SelectedValue = (long)PlannedSpermCollection;
                    cmbCollection.SelectedItem = objList.Where(x => x.ID == Convert.ToInt64(PlannedSpermCollection)).FirstOrDefault();
                         

                    if (this.DataContext != null)
                    {

                    }
                    fillDetails();
                    //FillAbstience();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void FillAbstience()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            BizAction.MasterTable = MasterTableNameList.M_Abstinence;
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


                    cmbAbstience.ItemsSource = null;
                    cmbAbstience.ItemsSource = objList;
                    cmbAbstience.SelectedItem = objList[0];
                }

                if (this.DataContext != null)
                {

                }
                FillColor();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillColor()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SemenColorMaster;
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
                    cmbColor.ItemsSource = null;
                    cmbColor.ItemsSource = objList;
                    cmbColor.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {

                }
                fillDetails();
                //fillPlannedTreatment();
                // fillFinalPlannedTreatment();

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //private void fillPlannedTreatment()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_IVFPlannedTreatment;
        //    BizAction.Parent = new KeyValue();
        //    BizAction.Parent.Key = "1";
        //    BizAction.Parent.Value = "Status";
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
        //            cmbPlannedTreatment.ItemsSource = null;
        //            cmbPlannedTreatment.ItemsSource = objList;
        //            cmbPlannedTreatment.SelectedValue = (long)0;
        //        }
        //        if (this.DataContext != null)
        //        {
        //            //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //private void fillFinalPlannedTreatment()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_IVFPlannedTreatment;
        //    BizAction.Parent = new KeyValue();
        //    BizAction.Parent.Key = "1";
        //    BizAction.Parent.Value = "Status";
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
        //            cmbPlannedTreatment.ItemsSource = null;
        //            cmbPlannedTreatment.ItemsSource = objList;
        //            cmbPlannedTreatment.SelectedValue = (long)0;
        //        }
        //        if (this.DataContext != null)
        //        {
        //            //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
        //        }
        //        fillExternalStimulation();
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //private void fillExternalStimulation()
        //{

        //    List<MasterListItem> Items = new List<MasterListItem>();

        //    MasterListItem Item = new MasterListItem();
        //    Item.ID = (int)0;
        //    Item.Description = "--Select--";
        //    Items.Add(Item);

        //    Item = new MasterListItem();
        //    Item.ID = (int)ExternalStimulation.Yes;
        //    Item.Description = ExternalStimulation.Yes.ToString();
        //    Items.Add(Item);

        //    Item = new MasterListItem();
        //    Item.ID = (int)ExternalStimulation.No;
        //    Item.Description = ExternalStimulation.No.ToString();
        //    Items.Add(Item);

        //    cmbSimulation.ItemsSource = Items;
        //    cmbSimulation.SelectedValue = (long)0;

        //    fillMainIndication();
        //}


        //private void fillMainIndication()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_IVf_MainIndication;
        //    BizAction.Parent = new KeyValue();
        //    BizAction.Parent.Key = "1";
        //    BizAction.Parent.Value = "Status";
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
        //            cmbMainIndication.ItemsSource = null;
        //            cmbMainIndication.ItemsSource = objList;
        //            cmbMainIndication.SelectedValue = (long)0;
        //        }
        //        if (this.DataContext != null)
        //        {
        //            //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
        //        }               
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}
        #endregion

        private bool Validate()
        {
            bool result = true;
            if (dtIUIDate.SelectedDate == null)
            {
                dtIUIDate.SetValidation("Please Select IUI Date");
                dtIUIDate.RaiseValidationError();
                dtIUIDate.Focus();
                result = false;
            }
            else
                dtIUIDate.ClearValidationError();
            if (IUITime.Value == null)
            {
                IUITime.SetValidation("Please Select IUI Time");
                IUITime.RaiseValidationError();
                IUITime.Focus();
                result = false;
            }
            else
                IUITime.ClearValidationError();

            if (dtCollection.SelectedDate == null)
            {
                dtCollection.SetValidation("Please Select Collection Date");
                dtCollection.RaiseValidationError();
                dtCollection.Focus();
                result = false;
            }
            else
                dtCollection.ClearValidationError();

            if (CollectionTime.Value == null)
            {
                CollectionTime.SetValidation("Please Select Time of Collection");
                CollectionTime.RaiseValidationError();
                CollectionTime.Focus();
                result = false;
            }
            else
                CollectionTime.ClearValidationError();



            if (PlannedSpermCollection == 4 || PlannedSpermCollection == 11 || PlannedSpermCollection == 13 || PlannedSpermCollection == 17 || PlannedSpermCollection == 18)
            {
                if (dtThawDate.SelectedDate == null)
                {
                    dtThawDate.SetValidation("Please Select Thaw Date");
                    dtThawDate.RaiseValidationError();
                    dtThawDate.Focus();
                    result = false;
                }
                else
                    dtThawDate.ClearValidationError();

                if (ThawTime.Value == null)
                {
                    ThawTime.SetValidation("Please Select Time of Thaw");
                    ThawTime.RaiseValidationError();
                    ThawTime.Focus();
                    result = false;
                }
                else
                    ThawTime.ClearValidationError();

                if (txtSampleID.Text.Trim() == string.Empty)
                {
                    txtSampleID.SetValidation("Please Select Sample ID");
                    txtSampleID.RaiseValidationError();
                    txtSampleID.Focus();
                    result = false;
                }
                else
                    txtSampleID.ClearValidationError();

                if (PlannedTreatmentID == 16)
                {
                    if (txtDonorCode.Text.Trim() == string.Empty)
                    {
                        txtDonorCode.SetValidation("Please Select Donor Code");
                        txtDonorCode.RaiseValidationError();
                        txtDonorCode.Focus();
                        result = false;
                    }
                    else
                        txtDonorCode.ClearValidationError();
                }

            }

            if (dtpreperation.SelectedDate == null)
            {
                dtpreperation.SetValidation("Please Select Preperation Date");
                dtpreperation.RaiseValidationError();
                dtpreperation.Focus();
                result = false;
            }
            else
                dtpreperation.ClearValidationError();

            if (PreperationTime.Value == null)
            {
                PreperationTime.SetValidation("Please Select Time of Preperation");
                PreperationTime.RaiseValidationError();
                PreperationTime.Focus();
                result = false;
            }
            else
                PreperationTime.ClearValidationError();

            if (cmbCollection.SelectedItem == null)
            {
                cmbCollection.TextBox.SetValidation("Please select Collection Method");
                cmbCollection.TextBox.RaiseValidationError();
                cmbCollection.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbCollection.SelectedItem).ID == 0)
            {
                cmbCollection.TextBox.SetValidation("Please select Collection Method");
                cmbCollection.TextBox.RaiseValidationError();
                cmbCollection.Focus();
                result = false;
            }
            else
                cmbCollection.TextBox.ClearValidationError();


            if (cmbInseminatedBy.SelectedItem == null)
            {
                cmbInseminatedBy.TextBox.SetValidation("Please select Inseminated By");
                cmbInseminatedBy.TextBox.RaiseValidationError();
                cmbInseminatedBy.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbInseminatedBy.SelectedItem).ID == 0)
            {
                cmbInseminatedBy.TextBox.SetValidation("Please select Inseminated By");
                cmbInseminatedBy.TextBox.RaiseValidationError();
                cmbInseminatedBy.Focus();
                result = false;
            }
            else
                cmbInseminatedBy.TextBox.ClearValidationError();

            if (cmbWitnessedBy.SelectedItem == null)
            {
                cmbWitnessedBy.TextBox.SetValidation("Please select Witnessed By");
                cmbWitnessedBy.TextBox.RaiseValidationError();
                cmbWitnessedBy.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbWitnessedBy.SelectedItem).ID == 0)
            {
                cmbWitnessedBy.TextBox.SetValidation("Please select Witnessed By");
                cmbWitnessedBy.TextBox.RaiseValidationError();
                cmbWitnessedBy.Focus();
                result = false;
            }
            else
                cmbWitnessedBy.TextBox.ClearValidationError();

            DateTime? CollectionDate = new DateTime();
            DateTime? ThawDate = new DateTime();
            DateTime? PreparationDate = new DateTime();
            DateTime? IUIDate = new DateTime();

            if (dtCollection.SelectedDate != null)
            {
                CollectionDate = dtCollection.SelectedDate.Value.Date;
                CollectionDate = CollectionDate.Value.Add(CollectionTime.Value.Value.TimeOfDay);
            }

            if (dtThawDate.SelectedDate != null)
            {
                ThawDate = dtThawDate.SelectedDate.Value.Date;
                ThawDate = ThawDate.Value.Add(ThawTime.Value.Value.TimeOfDay);
            }

            if (dtpreperation.SelectedDate != null)
            {
                PreparationDate = dtpreperation.SelectedDate.Value.Date;
                PreparationDate = PreparationDate.Value.Add(PreperationTime.Value.Value.TimeOfDay);
            }

            if (dtIUIDate.SelectedDate != null)
            {
                IUIDate = dtIUIDate.SelectedDate.Value.Date;
                IUIDate = IUIDate.Value.Add(IUITime.Value.Value.TimeOfDay);
            }


            if (CollectionDate > PreparationDate)
            {
                dtCollection.SetValidation("Collection Date Cannot Be Greater Than Preperation Date");
                dtCollection.RaiseValidationError();
                dtCollection.Focus();
                result = false;
            }
            else if (CollectionDate > IUIDate)
            {
                dtCollection.SetValidation("Collection Date Cannot Be Greater Than IUI Date");
                dtCollection.RaiseValidationError();
                dtCollection.Focus();
                result = false;
            }
            else if (PreparationDate > IUIDate)
            {
                dtpreperation.SetValidation("Preperation Date Cannot Be Greater Than IUI Date");
                dtpreperation.RaiseValidationError();
                dtpreperation.Focus();
                result = false;
            }
            else if (dtThawDate.SelectedDate != null)
            {
                if (CollectionDate > ThawDate)
                {
                    dtCollection.SetValidation("Collection Date Cannot Be Greater Than Thaw Date");
                    dtCollection.RaiseValidationError();
                    dtCollection.Focus();
                    result = false;
                }
                else if (ThawDate > PreparationDate)
                {
                    dtThawDate.SetValidation("Thaw Date Cannot Be Greater Than Preperation Date");
                    dtThawDate.RaiseValidationError();
                    dtThawDate.Focus();
                    result = false;
                }
                else if (ThawDate > IUIDate)
                {
                    dtThawDate.SetValidation("Thaw Date Cannot Be Greater Than IUI Date");
                    dtThawDate.RaiseValidationError();
                    dtThawDate.Focus();
                    result = false;
                }
                else
                {
                    dtCollection.ClearValidationError();
                    dtThawDate.ClearValidationError();
                }
            }
            else
            {
                dtCollection.ClearValidationError();
                dtpreperation.ClearValidationError();
            }

            
            
            if (TxtPostProgMotility.Text != null && TxtPostNonProgressive.Text != null && TxtPostNonMotile.Text != null && TxtPreProgMotility.Text != null && TxtPreNonProgressive.Text != null && TxtPreNonMotile.Text != null)
            {
                double valuePost  = (Convert.ToDouble(TxtPostProgMotility.Text) + Convert.ToDouble(TxtPostNonProgressive.Text) + Convert.ToDouble(TxtPostNonMotile.Text));
                double valuePre = (Convert.ToDouble(TxtPreProgMotility.Text) + Convert.ToDouble(TxtPreNonProgressive.Text) + Convert.ToDouble(TxtPreNonMotile.Text));

                if (valuePre == 100)
                {
                    TxtPreProgMotility.ClearValidationError();
                    TxtPreNonProgressive.ClearValidationError();
                    TxtPreNonMotile.ClearValidationError();
                }

                if (valuePost == 100)
                {
                    TxtPostProgMotility.ClearValidationError();
                    TxtPostNonProgressive.ClearValidationError();
                    TxtPostNonMotile.ClearValidationError();
                }

                if (valuePost < 100 && valuePre < 100)
                {
                    TxtPreProgMotility.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreProgMotility.RaiseValidationError();
                    TxtPreProgMotility.Focus();

                    TxtPreNonProgressive.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreNonProgressive.RaiseValidationError();
                    TxtPreNonProgressive.Focus();

                    TxtPreNonMotile.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreNonMotile.RaiseValidationError();
                    TxtPreNonMotile.Focus();

                    //TxtPreNonProgressive.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    //TxtPreNonProgressive.RaiseValidationError();
                    //TxtPreNonProgressive.Focus();
                    //TxtPreNonMotile.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    //TxtPreNonMotile.RaiseValidationError();
                    //TxtPreNonMotile.Focus();
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //           new MessageBoxControl.MessageBoxChildWindow("", " For Pre and Post Wash Sum of Progressive,Non Progressive,Non motile can not be less than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //msgW1.Show();
                    result = false;
                }
                else if (valuePre < 100)
                {
                    TxtPreProgMotility.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreProgMotility.RaiseValidationError();
                    TxtPreProgMotility.Focus();

                    TxtPreNonProgressive.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreNonProgressive.RaiseValidationError();
                    TxtPreNonProgressive.Focus();

                    TxtPreNonMotile.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreNonMotile.RaiseValidationError();
                    TxtPreNonMotile.Focus();

                    //TxtPreProgMotility.RaiseValidationError();
                    //TxtPreProgMotility.Focus();
                    //TxtPreNonProgressive.RaiseValidationError();
                    //TxtPreNonProgressive.Focus();
                    //TxtPreNonMotile.RaiseValidationError();
                    //TxtPreNonMotile.Focus();
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //           new MessageBoxControl.MessageBoxChildWindow("", " For Pre Wash Sum of Progressive,Non Progressive,Non motile can not be less than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //msgW1.Show();


                    result = false;
                }
                else if (valuePost < 100)
                {
                    TxtPostProgMotility.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPostProgMotility.RaiseValidationError();
                    TxtPostProgMotility.Focus();

                    TxtPostNonProgressive.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPostNonProgressive.RaiseValidationError();
                    TxtPostNonProgressive.Focus();

                    TxtPostNonMotile.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPostNonMotile.RaiseValidationError();
                    TxtPostNonMotile.Focus();

                    //TxtPostProgMotility.RaiseValidationError();
                    //TxtPostProgMotility.Focus();
                    //TxtPostNonProgressive.RaiseValidationError();
                    //TxtPostNonProgressive.Focus();
                    //TxtPostNonMotile.RaiseValidationError();
                    //TxtPostNonMotile.Focus();
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //           new MessageBoxControl.MessageBoxChildWindow("", " For POst Wash Sum of Progressive,Non Progressive,Non motile can not be less than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //msgW1.Show();


                    result = false;
                }
                else
                {
                    TxtPreProgMotility.ClearValidationError();
                    TxtPreNonProgressive.ClearValidationError();
                    TxtPreNonMotile.ClearValidationError();
                    TxtPostProgMotility.ClearValidationError();
                    TxtPostNonProgressive.ClearValidationError();
                    TxtPostNonMotile.ClearValidationError();
                }

            }

            return result;

               //if (SampRecTime.Value == null)
            //{
            //    SampRecTime.SetValidation("Please Select Time of Receiving");
            //    SampRecTime.RaiseValidationError();
            //    SampRecTime.Focus();
            //    result = false;
            //}
            //else
            //    SampRecTime.ClearValidationError();



        }
        int ClickedFlag = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                if (Validate())
                {
                    string msgText = "";
                    msgText = "Are You Sure \n You Want To Save The Donor Details?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
                else
                    ClickedFlag = 0;
            }
            else
                ClickedFlag = 0;

        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveDetails();
            else
                ClickedFlag = 0;

        }
        private void SaveDetails()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();


            cls_IVFDashboard_AddUpdateSemenWashBizActionVO BizAction = new cls_IVFDashboard_AddUpdateSemenWashBizActionVO();


            BizAction.SemensWashDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.SemensWashDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.SemensWashDetails.PlanTherapyID = PlanTherapyID;
            BizAction.SemensWashDetails.PlanTherapyUnitID = PlanTherapyUnitID;

            BizAction.SemensWashDetails.BatchID = BatchID;
            BizAction.SemensWashDetails.BatchUnitID = BatchUnitID;

            BizAction.SemensWashDetails.BatchCode = txtBatchCode.Text;

            BizAction.SemensWashDetails.ISFromIUI = true;
            BizAction.SemensWashDetails.ID = IUIID;

            //For IUI
            if (dtIUIDate.SelectedDate != null)
                BizAction.SemensWashDetails.IUIDate = dtIUIDate.SelectedDate.Value.Date;
            if (IUITime.Value != null)
                BizAction.SemensWashDetails.IUIDate = BizAction.SemensWashDetails.IUIDate.Value.Add(IUITime.Value.Value.TimeOfDay);

            if (dtCollection.SelectedDate != null)
                BizAction.SemensWashDetails.CollectionDate = dtCollection.SelectedDate.Value.Date;
            if (CollectionTime.Value != null)
                BizAction.SemensWashDetails.CollectionDate = BizAction.SemensWashDetails.CollectionDate.Value.Add(CollectionTime.Value.Value.TimeOfDay);

            if (dtThawDate.SelectedDate != null)
                BizAction.SemensWashDetails.ThawDate = dtThawDate.SelectedDate.Value.Date;
            if (ThawTime.Value != null)
                BizAction.SemensWashDetails.ThawDate = BizAction.SemensWashDetails.ThawDate.Value.Add(ThawTime.Value.Value.TimeOfDay); //added by neena

            if (dtpreperation.SelectedDate != null)
                BizAction.SemensWashDetails.PreperationDate = dtpreperation.SelectedDate.Value.Date;
            if (PreperationTime.Value != null)
                BizAction.SemensWashDetails.PreperationDate = BizAction.SemensWashDetails.PreperationDate.Value.Add(PreperationTime.Value.Value.TimeOfDay);

            if (cmbCollection.SelectedItem != null)
                BizAction.SemensWashDetails.MethodOfCollectionID = ((MasterListItem)cmbCollection.SelectedItem).ID;

            BizAction.SemensWashDetails.Inseminated = txtInseminated.Text.Trim();

            if (cmbInseminatedBy.SelectedItem != null)
                BizAction.SemensWashDetails.InSeminatedByID = ((MasterListItem)cmbInseminatedBy.SelectedItem).ID;
            if (cmbWitnessedBy.SelectedItem != null)
                BizAction.SemensWashDetails.WitnessByID = ((MasterListItem)cmbWitnessedBy.SelectedItem).ID;

            if (!string.IsNullOrEmpty(TxtPreAmount.Text.Trim()))
                BizAction.SemensWashDetails.PreAmount = float.Parse(TxtPreAmount.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostAmount.Text.Trim()))
                BizAction.SemensWashDetails.PostAmount = float.Parse(TxtPostAmount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreSpermCount.Text.Trim()))
                BizAction.SemensWashDetails.PreSpermCount = float.Parse(TxtPreSpermCount.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostSpermCount.Text.Trim()))
                BizAction.SemensWashDetails.PostSpermCount = float.Parse(TxtPostSpermCount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreTotCount.Text.Trim()))
                BizAction.SemensWashDetails.PreTotalSpermCount = float.Parse(TxtPreTotCount.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostTotCount.Text.Trim()))
                BizAction.SemensWashDetails.PostTotalSpermCount = float.Parse(TxtPostTotCount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreProgMotility.Text.Trim()))
                BizAction.SemensWashDetails.PreProgMotility = float.Parse(TxtPreProgMotility.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostProgMotility.Text.Trim()))
                BizAction.SemensWashDetails.PostProgMotility = float.Parse(TxtPostProgMotility.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreNonProgressive.Text.Trim()))
                BizAction.SemensWashDetails.PreNonProgressive = float.Parse(TxtPreNonProgressive.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostNonProgressive.Text.Trim()))
                BizAction.SemensWashDetails.PostNonProgressive = float.Parse(TxtPostNonProgressive.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreNonMotile.Text.Trim()))
                BizAction.SemensWashDetails.PreNonMotile = float.Parse(TxtPreNonMotile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostNonMotile.Text.Trim()))
                BizAction.SemensWashDetails.PostNonMotile = float.Parse(TxtPostNonMotile.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreMotileSpermCount.Text.Trim()))
                BizAction.SemensWashDetails.PreMotileSpermCount = float.Parse(TxtPreMotileSpermCount.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostMotileSpermCount.Text.Trim()))
                BizAction.SemensWashDetails.PostMotileSpermCount = float.Parse(TxtPostMotileSpermCount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreTotalMotility.Text.Trim()))
                BizAction.SemensWashDetails.PreTotalMotility = float.Parse(TxtPreTotalMotility.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostTotalMotility.Text.Trim()))
                BizAction.SemensWashDetails.PostTotalMotility = float.Parse(TxtPostTotalMotility.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreNormalForms.Text.Trim()))
                BizAction.SemensWashDetails.PreNormalForms = float.Parse(TxtPreNormalForms.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostNormalForms.Text.Trim()))
                BizAction.SemensWashDetails.PostNormalForms = float.Parse(TxtPostNormalForms.Text.Trim());

            if (!string.IsNullOrEmpty(TxtRemark.Text.Trim()))
                BizAction.SemensWashDetails.Comment = TxtRemark.Text.Trim();

            if (!string.IsNullOrEmpty(txtSampleID.Text.Trim()))
                BizAction.SemensWashDetails.SampleID = txtSampleID.Text;

            if (!string.IsNullOrEmpty(txtDonorCode.Text.Trim()))
            {
                BizAction.SemensWashDetails.DonorID = DonorID;
                BizAction.SemensWashDetails.DonorUnitID = DonorUnitID;
            }



            if (cmbnseminationLocation.SelectedItem != null)
                BizAction.SemensWashDetails.InSeminationLocationID = ((MasterListItem)cmbnseminationLocation.SelectedItem).ID;
            if (cmbInseminationMethod.SelectedItem != null)
                BizAction.SemensWashDetails.InSeminationMethodID = ((MasterListItem)cmbInseminationMethod.SelectedItem).ID;
            //BizAction.SemensWashDetails.SampleID = txtSemenBankSampleID.Text;
            //BizAction.SemensWashDetails.DonorID = DonorID;
            //BizAction.SemensWashDetails.DonorUnitID = DonorUnitID;

            if (!string.IsNullOrEmpty(TxtOtherCells.Text.Trim()))
                BizAction.SemensWashDetails.AnyOtherCells = TxtOtherCells.Text.Trim();
            //..........................

            if (cmbAbstience.SelectedItem != null)
                BizAction.SemensWashDetails.AbstinenceID = ((MasterListItem)cmbAbstience.SelectedItem).ID;
            BizAction.SemensWashDetails.TimeRecSampLab = SampRecTime.Value;

            if (rbtCentre.IsChecked == true)
                BizAction.SemensWashDetails.CollecteAtCentre = true;
            else if (rbtOutSideCentre.IsChecked == true)
                BizAction.SemensWashDetails.CollecteAtCentre = false;
            if (cmbColor.SelectedItem != null)
                BizAction.SemensWashDetails.ColorID = ((MasterListItem)cmbColor.SelectedItem).ID;
            if (!string.IsNullOrEmpty(txtVolume.Text.Trim()))
                BizAction.SemensWashDetails.Quantity = float.Parse(txtVolume.Text.Trim());
            if (!string.IsNullOrEmpty(txtpH.Text.Trim()))
                BizAction.SemensWashDetails.PH = float.Parse(txtpH.Text.Trim());
            BizAction.SemensWashDetails.LiquificationTime = txtLiqueficationTime.Text;
            if (rbtNormal.IsChecked == true)
                BizAction.SemensWashDetails.Viscosity = true;
            else if (rbtViscous.IsChecked == true)
                BizAction.SemensWashDetails.Viscosity = false;
            if (rbtPresent.IsChecked == true)
                BizAction.SemensWashDetails.Odour = true;
            else if (rbtNotPresent.IsChecked == true)
                BizAction.SemensWashDetails.Odour = false;

            if (!string.IsNullOrEmpty(TxtPreGradeI.Text.Trim()))
                BizAction.SemensWashDetails.PreMotilityGradeI = float.Parse(TxtPreGradeI.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPreGradeII.Text.Trim()))
                BizAction.SemensWashDetails.PreMotilityGradeII = float.Parse(TxtPreGradeII.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPreGradeIII.Text.Trim()))
                BizAction.SemensWashDetails.PreMotilityGradeIII = float.Parse(TxtPreGradeIII.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPreGradeIV.Text.Trim()))
                BizAction.SemensWashDetails.PreMotilityGradeIV = float.Parse(TxtPreGradeIV.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPreNormalMorphology.Text.Trim()))
                BizAction.SemensWashDetails.PreNormalMorphology = float.Parse(TxtPreNormalMorphology.Text.Trim());


            if (!string.IsNullOrEmpty(TxtPostGradeI.Text.Trim()))
                BizAction.SemensWashDetails.PostMotilityGradeI = float.Parse(TxtPostGradeI.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostGradeII.Text.Trim()))
                BizAction.SemensWashDetails.PostMotilityGradeII = float.Parse(TxtPostGradeII.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostGradeIII.Text.Trim()))
                BizAction.SemensWashDetails.PostMotilityGradeIII = float.Parse(TxtPostGradeIII.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostGradeIV.Text.Trim()))
                BizAction.SemensWashDetails.PostMotilityGradeIV = float.Parse(TxtPostGradeIV.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostNormalMorphology.Text.Trim()))
                BizAction.SemensWashDetails.PostNormalMorphology = float.Parse(TxtPostNormalMorphology.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPusCells.Text.Trim()))
                BizAction.SemensWashDetails.PusCells = TxtPusCells.Text.Trim();
            BizAction.SemensWashDetails.MotileSperm = txtMotile.Text.Trim();
            if (!string.IsNullOrEmpty(TxtRoundCells.Text.Trim()))
                BizAction.SemensWashDetails.RoundCells = TxtRoundCells.Text.Trim();
            if (!string.IsNullOrEmpty(TxtEpithelialCells.Text.Trim()))
                BizAction.SemensWashDetails.EpithelialCells = TxtEpithelialCells.Text.Trim();


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient IUI Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();

                    gb1.DataContext = null;
                    //gb2.DataContext = null;
                    grpIUID.DataContext = null;
                    grpPhysicalCharacteristics.DataContext = null;
                    grpClassification.DataContext = null;
                    grpCellInfo.DataContext = null;
                    AccDonor.DataContext = null;
                    grpContactDetails.DataContext = null;
                    fillDetails();

                }
                else
                {
                    //CmdSave.IsEnabled = true;
                    ClickedFlag = 0;
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient semen wash.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        bool IsModify;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbInseminatedBy.IsEnabled = false;
            rbtExisting.IsChecked = true;
            //  CollectionTime.SetValue = 

            FillInseminatedBy();

            dtIUIDate.SelectedDate = DateTime.Now.Date;
            IUITime.Value = DateTime.Now.Date;
            dtCollection.SelectedDate = DateTime.Now;
            CollectionTime.Value = DateTime.Now.Date;
            //dtThawDate.SelectedDate = DateTime.Now;
            //ThawTime.Value = DateTime.Now.Date;
            dtpreperation.SelectedDate = DateTime.Now;
            PreperationTime.Value = DateTime.Now.Date;

            //if (PlannedSpermCollection == 4 || PlannedSpermCollection == 11 || PlannedSpermCollection == 13 || PlannedSpermCollection == 17 || PlannedSpermCollection == 18)
            if (PlannedSpermCollection == 24)
            {
                dtThawDate.IsEnabled = true;
                ThawTime.IsEnabled = true;
                SelectSample.IsEnabled = true;
            }
            if (IsClosed)
                cmdNew.IsEnabled = false;
            else
                cmdNew.IsEnabled = true;
        }

        private void FillInseminatedBy()
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

                    cmbInseminatedBy.ItemsSource = null;
                    cmbInseminatedBy.ItemsSource = objList;
                    cmbInseminatedBy.SelectedValue = (long)0;


                    //by neena
                    if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID > 0)
                        cmbInseminatedBy.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                    //

                    cmbWitnessedBy.ItemsSource = null;
                    cmbWitnessedBy.ItemsSource = objList;
                    cmbWitnessedBy.SelectedItem = objList[0];
                    //   fillfillInseminationmethod();
                    FillCollectionMethod();   //  as per the requirements of Milann
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        private void cmdLinkDonor_Click(object sender, RoutedEventArgs e)
        {
            if (rbtExisting.IsChecked == true)
            {
                //FrmDonorSearch win = new FrmDonorSearch();
                //win.Show();
                //win.OnSaveButton_Click += new RoutedEventHandler(Win_DonorLinkClickFromDonorSearch);
            }
            else
            {
                //frmDonorRegistration win = new frmDonorRegistration();
                //win.IsFromDonorSearch = false;
                //win.Initiate("New");
                //win.Title = "New Donor";
                //win.Show();
                //win.OnSaveButton_Click += new RoutedEventHandler(Win_DonorLinkClickFromregistration);

            }
        }
        private void Win_DonorLinkClickFromregistration(object sender, RoutedEventArgs e)
        {
            //frmDonorRegistration win = (frmDonorRegistration)sender;
            //if (win.PatientID != 0 && win.PatientUnitID != 0)
            //{
            //    try 
            //    {
            //        clsGetDonorDetailsForIUIBizActionVO BizAction = new clsGetDonorDetailsForIUIBizActionVO();
            //        BizAction.BatchDetails = new clsDonorBatchVO();
            //        BizAction.BatchDetails.DonorID =win.PatientID;
            //        BizAction.BatchDetails.DonorUnitID = win.PatientUnitID;
            //        BizAction.BatchDetails.ID = win.BatchID;
            //        BizAction.BatchDetails.UnitID = win.BatchUnitID;

            //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //        Client.ProcessCompleted += (s, arg) =>
            //        {
            //            if (arg.Error == null)
            //            {
            //                if (arg.Result != null)
            //                {
            //                    if (((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails != null)
            //                    {
            //                        AccDonor.DataContext = ((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails;
            //                        txtDonorCode.Text = ((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.DonorCode;
            //                        txtSemenBankValue.Text = ((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.Lab;
            //                        DonorID=((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.DonorID;
            //                         DonorUnitID=((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.DonorUnitID;
            //                         BatchID = ((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.ID;
            //                         BatchUnitID = ((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.UnitID;


            //                    }
            //                }


            //            }
            //            else
            //            {
            //                MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving Sponsor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                msgW1.Show();
            //            }
            //        };
            //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //        Client.CloseAsync();
            //    }
            //    catch (Exception)
            //    {
            //        throw;
            //    }
            //}
        }
        long DonorID = 0, DonorUnitID = 0, BatchID = 0, BatchUnitID = 0;
        private void Win_DonorLinkClickFromDonorSearch(object sender, RoutedEventArgs e)
        {
            //FrmDonorSearch win = (FrmDonorSearch)sender;
            //if (win.PatientID != 0 && win.PatientUnitID != 0)
            //{
            //    try
            //    {
            //        clsGetDonorDetailsForIUIBizActionVO BizAction = new clsGetDonorDetailsForIUIBizActionVO();
            //        BizAction.BatchDetails = new clsDonorBatchVO();
            //        BizAction.BatchDetails.DonorID = win.PatientID;
            //        BizAction.BatchDetails.DonorUnitID = win.PatientUnitID;
            //        BizAction.BatchDetails.ID = win.BatchID;
            //        BizAction.BatchDetails.UnitID = win.BatchUnitID;

            //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //        Client.ProcessCompleted += (s, arg) =>
            //        {
            //            if (arg.Error == null)
            //            {
            //                if (arg.Result != null)
            //                {
            //                    if (((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails != null)
            //                    {
            //                        AccDonor.DataContext = ((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails;
            //                        txtDonorCode.Text = ((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.DonorCode;
            //                        txtBatchCode.Text = ((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.BatchCode;
            //                        txtSemenBankValue.Text = ((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.Lab;
            //                         DonorID=((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.DonorID;
            //                         DonorUnitID=((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.DonorUnitID;
            //                         BatchID = ((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.ID;
            //                            BatchUnitID=((clsGetDonorDetailsForIUIBizActionVO)arg.Result).BatchDetails.UnitID;
            //                    }
            //                }


            //            }
            //            else
            //            {
            //                MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving Sponsor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                msgW1.Show();
            //            }
            //        };
            //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //        Client.CloseAsync();
            //    }
            //    catch (Exception)
            //    {
            //        throw;
            //    }
            //}
        }

        private void rbtExisting_Click(object sender, RoutedEventArgs e)
        {
            if (rbtExisting.IsChecked == true)
                rbtNew.IsChecked = false;
            else
                rbtNew.IsChecked = true;
        }

        private void rbtNew_Click(object sender, RoutedEventArgs e)
        {
            if (rbtNew.IsChecked == true)
                rbtExisting.IsChecked = false;
            else
                rbtExisting.IsChecked = true;
        }

        private void acc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {

        }
        long IUIID = 0;
        string PreviousSampleID = string.Empty;
        private void fillDetails()
        {
            cls_GetIVFDashboard_NewIUIDetailsBizActionVO BizAction = new cls_GetIVFDashboard_NewIUIDetailsBizActionVO();
            BizAction.SemensExaminationDetails = new cls_IVFDashboard_SemenWashVO();
            BizAction.SemensExaminationDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.SemensExaminationDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.SemensExaminationDetails.PlanTherapyID = PlanTherapyID;
            BizAction.SemensExaminationDetails.PlanTherapyUnitID = PlanTherapyUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails != null)
                    {
                        //this.DataContext = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails;

                        ISSavedIUIID = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ID;                       
                        IUIID = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ID;
                        BatchID = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.BatchID;
                        BatchUnitID = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.BatchUnitID;
                        DonorID = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.DonorID;
                        DonorUnitID = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.DonorUnitID;
                        //gb1.DataContext = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails;
                        //gb2.DataContext = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails;
                        grpIUID.DataContext = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails;
                        grpPhysicalCharacteristics.DataContext = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails;
                        grpClassification.DataContext = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails;
                        grpCellInfo.DataContext = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails;
                        AccDonor.DataContext = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails;
                        grpContactDetails.DataContext = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.IsFrozenSample == true)
                            rbtYes.IsChecked = true;
                        else
                            rbtNo.IsChecked = true;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.CollecteAtCentre == true)
                            rbtCentre.IsChecked = true;
                        else
                            rbtOutSideCentre.IsChecked = true;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.Odour == true)
                            rbtPresent.IsChecked = true;
                        else
                            rbtNotPresent.IsChecked = true;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.Viscosity == true)
                            rbtViscous.IsChecked = true;
                        else
                            rbtNormal.IsChecked = true;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.InSeminatedByID != null && ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.InSeminatedByID > 0)
                            cmbInseminatedBy.SelectedValue = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.InSeminatedByID;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.WitnessByID != null)
                            cmbWitnessedBy.SelectedValue = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.WitnessByID;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.InSeminationLocationID != null)
                            cmbnseminationLocation.SelectedValue = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.InSeminationLocationID;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.InSeminationMethodID != null)
                            cmbInseminationMethod.SelectedValue = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.InSeminationMethodID;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.MethodOfCollectionID != null && ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.MethodOfCollectionID >0)
                            cmbCollection.SelectedValue = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.MethodOfCollectionID;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.AbstinenceID != null)
                            cmbAbstience.SelectedValue = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.AbstinenceID;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ColorID != null)
                            cmbColor.SelectedValue = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ColorID;
                        //Added as per Milann's Requirement 
                        //if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ExternalSimulationID != null)
                        //    cmbSimulation.SelectedValue = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ExternalSimulationID;
                        //if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PlannedTreatmentID != null)
                        //    cmbPlannedTreatment.SelectedValue = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PlannedTreatmentID;

                        //if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.MainInductionID != null)
                        //    cmbMainIndication.SelectedValue = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.MainInductionID;
                        //if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.Cyclecode != null)
                        //    txtCycleCode.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.Cyclecode;
                        //if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.CycleDuration != null)
                        //    txtCycleDuration.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.CycleDuration;
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreAmount != null)
                            TxtPreAmount.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreAmount.ToString();
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostAmount != null)
                            TxtPostAmount.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostAmount.ToString();
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreNormalForms != null)
                            TxtPreNormalForms.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreNormalForms.ToString();
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostNormalForms != null)
                            TxtPostNormalForms.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreNormalForms.ToString();
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreProgMotility != null)
                            TxtPreProgMotility.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreProgMotility.ToString();
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostProgMotility != null)
                            TxtPostProgMotility.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostProgMotility.ToString();

                        //if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreperationDate != null)
                        //    dtpreperation.SelectedDate = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreperationDate;

                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.Inseminated != null)
                            txtInseminated.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.Inseminated;

                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.MotileSperm != null)
                            txtMotile.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.MotileSperm;
                        //


                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ID != null)
                            cmdPrint.IsEnabled = true;
                        else
                            cmdPrint.IsEnabled = false;

                        //added by neena
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.IUIDate != null && ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.IUIDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtIUIDate.SelectedDate = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.IUIDate;
                            IUITime.Value = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.IUIDate;
                        }
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.CollectionDate != null && ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.CollectionDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtCollection.SelectedDate = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.CollectionDate;
                            CollectionTime.Value = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.CollectionDate;
                        }
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ThawDate != null && ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ThawDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtThawDate.SelectedDate = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ThawDate;
                            ThawTime.Value = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.ThawDate;
                        }
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreperationDate != null && ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreperationDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtpreperation.SelectedDate = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreperationDate;
                            PreperationTime.Value = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreperationDate;
                        }

                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreNonProgressive != null)
                            TxtPreNonProgressive.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreNonProgressive.ToString();
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostNonProgressive != null)
                            TxtPostNonProgressive.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostNonProgressive.ToString();

                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreNonMotile != null)
                            TxtPreNonMotile.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreNonMotile.ToString();
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostNonMotile != null)
                            TxtPostNonMotile.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostNonMotile.ToString();

                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreMotileSpermCount != null)
                            TxtPreMotileSpermCount.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PreMotileSpermCount.ToString();
                        if (((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostMotileSpermCount != null)
                            TxtPostMotileSpermCount.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.PostMotileSpermCount.ToString();

                        txtDonorCode.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.DonorCode;
                        txtSampleID.Text = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.SampleID;
                        PreviousSampleID = ((cls_GetIVFDashboard_NewIUIDetailsBizActionVO)arg.Result).SemensExaminationDetails.SampleID;
                        if (PreviousSampleID.Equals(ThawedSampleID))
                            IsSampleIDChanged = false;
                        //
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void rbtCentre_Click(object sender, RoutedEventArgs e)
        {
            if (rbtCentre.IsChecked == true)
                rbtOutSideCentre.IsChecked = false;
            else
                rbtOutSideCentre.IsChecked = true;
        }

        private void rbtOutSideCentre_Click(object sender, RoutedEventArgs e)
        {
            if (rbtOutSideCentre.IsChecked == true)
                rbtCentre.IsChecked = false;
            else
                rbtCentre.IsChecked = true;
        }

        private void rbtYes_Click(object sender, RoutedEventArgs e)
        {
            if (rbtYes.IsChecked == true)
                rbtNo.IsChecked = false;
            else
                rbtNo.IsChecked = true;
        }

        private void rbtNo_Click(object sender, RoutedEventArgs e)
        {
            if (rbtNo.IsChecked == true)
                rbtYes.IsChecked = false;
            else
                rbtYes.IsChecked = true;

        }

        private void rbtNormal_Click(object sender, RoutedEventArgs e)
        {
            if (rbtNormal.IsChecked == true)
                rbtViscous.IsChecked = false;
            else
                rbtViscous.IsChecked = true;
        }

        private void rbtViscous_Click(object sender, RoutedEventArgs e)
        {
            if (rbtViscous.IsChecked == true)
                rbtNormal.IsChecked = false;
            else
                rbtNormal.IsChecked = true;
        }

        private void rbtPresent_Click(object sender, RoutedEventArgs e)
        {
            if (rbtPresent.IsChecked == true)
                rbtNotPresent.IsChecked = false;
            else
                rbtNotPresent.IsChecked = true;

        }

        private void rbtNotPresent_Click(object sender, RoutedEventArgs e)
        {
            if (rbtNotPresent.IsChecked == true)
                rbtPresent.IsChecked = false;
            else
                rbtPresent.IsChecked = true;
        }

        decimal PreSumOfGrade = 0;
        private void PreTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //TextBox txtbox = (TextBox)sender;
                //App.GenericMethod.IsValidEntry(txtbox, ValidationType.IsDecimalDigitsOnly);


                decimal varpreRapid = 0;
                decimal varpreSlow = 0;
                decimal varpreNon = 0;


                if (TxtPreGradeI.Text == "" || TxtPreGradeI.Text == null)
                    varpreRapid = 0;
                else
                    varpreRapid = Convert.ToDecimal(TxtPreGradeI.Text.Trim());


                if (TxtPreGradeII.Text == "" || TxtPreGradeII.Text == null)
                    varpreSlow = 0;
                else
                    varpreSlow = Convert.ToDecimal(TxtPreGradeII.Text.Trim());

                if (TxtPreGradeIII.Text == "" || TxtPreGradeIII.Text == null)
                    varpreNon = 0;
                else
                    varpreNon = Convert.ToDecimal(TxtPreGradeIII.Text.Trim());

                PreSumOfGrade = (Convert.ToDecimal(varpreRapid)) + (Convert.ToDecimal(varpreSlow)) + (Convert.ToDecimal(varpreNon));

                TxtPreTotalMotility.Text = PreSumOfGrade.ToString();
                //txtSpermConcentration.Text = SumOfGrade.ToString();

                TxtPreGradeIV.Text = (100 - PreSumOfGrade).ToString();

            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);

            }
        }

        decimal PostSumOfGrade = 0;

        private void PostTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //TextBox txtbox = (TextBox)sender;
                //App.GenericMethod.IsValidEntry(txtbox, ValidationType.IsDecimalDigitsOnly);


                decimal varpreRapid = 0;
                decimal varpreSlow = 0;
                decimal varpreNon = 0;


                if (TxtPostGradeI.Text == "" || TxtPostGradeI.Text == null)
                    varpreRapid = 0;
                else
                    varpreRapid = Convert.ToDecimal(TxtPostGradeI.Text.Trim());


                if (TxtPostGradeII.Text == "" || TxtPostGradeII.Text == null)
                    varpreSlow = 0;
                else
                    varpreSlow = Convert.ToDecimal(TxtPostGradeII.Text.Trim());

                if (TxtPostGradeIII.Text == "" || TxtPostGradeIII.Text == null)
                    varpreNon = 0;
                else
                    varpreNon = Convert.ToDecimal(TxtPostGradeIII.Text.Trim());

                PostSumOfGrade = (Convert.ToDecimal(varpreRapid)) + (Convert.ToDecimal(varpreSlow)) + (Convert.ToDecimal(varpreNon));

                TxtPostTotalMotility.Text = PostSumOfGrade.ToString();
                //txtSpermConcentration.Text = SumOfGrade.ToString();

                TxtPostGradeIV.Text = (100 - PostSumOfGrade).ToString();

            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);

            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/SemenWashAndIUI.aspx?ID=" + IUIID + "&UnitID=" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId + "&IsFromIUI=" + true + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
        }

        private void cmbSimulation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrescriptionOvarian_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        SpermThawingForPartnerIUI1 winPartner;
        SpermThawingForDonorIUI WinDonor;
        private void SelectSample_Click(object sender, RoutedEventArgs e)
        {
            if (PlannedTreatmentID == 15)
            {
                winPartner = new SpermThawingForPartnerIUI1();
                winPartner.IsClosed = IsClosed;
                winPartner.CoupleDetails = CoupleDetails;
                winPartner.PlanTherapyID = PlanTherapyID;
                winPartner.PlanTherapyUnitID = PlanTherapyUnitID;
                winPartner.OKButtonCode_Click += new RoutedEventHandler(winPartner_OKButtonCode_Click);
                winPartner.Show();
            }
            else if (PlannedTreatmentID == 16)
            {
                WinDonor = new SpermThawingForDonorIUI();
                WinDonor.IsClosed = IsClosed;
                WinDonor.CoupleDetails = CoupleDetails;
                WinDonor.PlanTherapyID = PlanTherapyID;
                WinDonor.PlanTherapyUnitID = PlanTherapyUnitID;
                WinDonor.OnSaveButton_Click += new RoutedEventHandler(WinDonor_OnSaveButton_Click);
                WinDonor.Show();
            }
        }

        public bool IsSampleIDChanged = false;
        void winPartner_OKButtonCode_Click(object sender, RoutedEventArgs e)
        {
            txtSampleID.Text = winPartner.SampleID.ToString();
            ThawedSampleID = winPartner.SampleID.ToString();
            if (winPartner.ThawDate!=null && winPartner.ThawDate.Value != null)
            {
                dtThawDate.SelectedDate = winPartner.ThawDate.Value;
                ThawTime.Value = winPartner.ThawTime.Value;
                ThawedDateTime = winPartner.ThawDate.Value;
            }
            ISSavedIUIID = winPartner.IUIID;
            if (!PreviousSampleID.Equals(ThawedSampleID))
                IsSampleIDChanged = true;
            //throw new NotImplementedException();
        }

        public long ISSavedIUIID = 0;
        public string ThawedSampleID = string.Empty;
        public string ThawedDonorCode = string.Empty;
        public DateTime? ThawedDateTime = null;


        void WinDonor_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            txtSampleID.Text = WinDonor.SampleID.ToString();
            ThawedSampleID = WinDonor.SampleID.ToString();
            if (WinDonor.ThawDate != null && WinDonor.ThawDate.Value != null)
            {
                dtThawDate.SelectedDate = WinDonor.ThawDate.Value;
                ThawTime.Value = WinDonor.ThawTime.Value;
                ThawedDateTime = WinDonor.ThawDate.Value;
            }
            txtDonorCode.Text = WinDonor.txtSelectedMrNO.Text;
            ThawedDonorCode = WinDonor.txtSelectedMrNO.Text;
            DonorID = WinDonor.DonorID;
            DonorUnitID = WinDonor.DonorUnitID;
            ISSavedIUIID = WinDonor.IUIID;
            if (!PreviousSampleID.Equals(ThawedSampleID))
                IsSampleIDChanged = true;
            //throw new NotImplementedException();
        }


        private void TxtPreAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidOneDigitWithTwoDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void TxtPreAmount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void TxtPreTotCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidThreeDigit() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void TxtPreTotCount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void TxtPreProgMotility_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidTwoDigit() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void TxtPreProgMotility_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void TxtPreAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtPreAmount.Text != "0" && TxtPreAmount.Text.Trim() != null && TxtPreAmount.Text.Trim() != "")
            {
                if (TxtPreSpermCount.Text != "0" && TxtPreSpermCount.Text.Trim() != null && TxtPreSpermCount.Text.Trim() != "")
                {
                    double value = Convert.ToDouble(TxtPreAmount.Text) * Convert.ToDouble(TxtPreSpermCount.Text);
                    double PreTot = Math.Round(value, 0);
                    TxtPreTotCount.Text = PreTot.ToString();
                }
            }

            if (TxtPreMotileSpermCount.Text != "0" && TxtPreMotileSpermCount.Text.Trim() != null && TxtPreMotileSpermCount.Text.Trim() != "")
            {
                if (TxtPreTotCount.Text != "0" && TxtPreTotCount.Text.Trim() != null && TxtPreMotileSpermCount.Text.Trim() != "")
                {
                    double value = (Convert.ToDouble(TxtPreMotileSpermCount.Text) / Convert.ToDouble(TxtPreTotCount.Text)) * 100;
                    double PreMot = Math.Round(value, 0);
                    TxtPreTotalMotility.Text = PreMot.ToString();

                }
            }
        }

        private void TxtPostAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtPostAmount.Text != "0" && TxtPostAmount.Text.Trim() != null && TxtPostAmount.Text.Trim() != "")
            {
                if (TxtPostSpermCount.Text != "0" && TxtPostSpermCount.Text.Trim() != null && TxtPostSpermCount.Text.Trim() != "")
                {
                    double value = Convert.ToDouble(TxtPostAmount.Text) * Convert.ToDouble(TxtPostSpermCount.Text);
                    double PostTot = Math.Round(value, 0);
                    TxtPostTotCount.Text = PostTot.ToString();
                }
            }

            if (TxtPostMotileSpermCount.Text != "0" && TxtPostMotileSpermCount.Text.Trim() != null && TxtPostMotileSpermCount.Text.Trim() != "")
            {
                if (TxtPostTotCount.Text != "0" && TxtPostTotCount.Text.Trim() != null && TxtPostMotileSpermCount.Text.Trim() != "")
                {
                    double value = (Convert.ToDouble(TxtPostMotileSpermCount.Text) / Convert.ToDouble(TxtPostTotCount.Text)) * 100;
                    double PostMot = Math.Round(value, 0);
                    TxtPostTotalMotility.Text = PostMot.ToString();

                }
            }
        }

        private void TxtPreProgMotility_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtPreProgMotility.Text.Trim() != null && TxtPreProgMotility.Text.Trim() != "" && TxtPreNonProgressive.Text.Trim() != null && TxtPreNonProgressive.Text.Trim() != "")
            {
                double valuePre = (Convert.ToDouble(TxtPreProgMotility.Text) + Convert.ToDouble(TxtPreNonProgressive.Text));
                 TxtPreTotalMotility.Text = valuePre.ToString();
                 TxtPreNonMotile.Text = (100 - valuePre).ToString();
            }

           
           

            //if (TxtPreProgMotility.Text.Trim() != null && TxtPreProgMotility.Text.Trim() != "")
            //{
            //    if (TxtPreNonProgressive.Text.Trim() != null && TxtPreNonProgressive.Text.Trim() != "")
            //    {
            //        if (TxtPreNonMotile.Text.Trim() != null && TxtPreNonMotile.Text.Trim() != "")
            //        {
            //            TxtPreProgMotility.ClearValidationError();
            //            TxtPreNonProgressive.ClearValidationError();
            //            TxtPreNonMotile.ClearValidationError();

            //            double value = (Convert.ToDouble(TxtPreProgMotility.Text) + Convert.ToDouble(TxtPreNonProgressive.Text) + Convert.ToDouble(TxtPreNonMotile.Text));
            //            if (value != 100)
            //            {
            //                TxtPreProgMotility.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
            //                TxtPreProgMotility.RaiseValidationError();
            //                //TxtPreProgMotility.Focus();
            //            }
            //        }
            //    }
            //}
        }

        private void TxtPostProgMotility_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtPostProgMotility.Text.Trim() != null && TxtPostProgMotility.Text.Trim() != "" && TxtPostNonProgressive.Text.Trim() != null && TxtPostNonProgressive.Text.Trim() != "")
            {
                double valuePost = (Convert.ToDouble(TxtPostProgMotility.Text) + Convert.ToDouble(TxtPostNonProgressive.Text));
                TxtPostTotalMotility.Text=valuePost.ToString();
                TxtPostNonMotile.Text = (100 - valuePost).ToString();
            }

            //if (TxtPostProgMotility.Text.Trim() != null && TxtPostProgMotility.Text.Trim() != "")
            //{
            //    if (TxtPostNonProgressive.Text.Trim() != null && TxtPostNonProgressive.Text.Trim() != "")
            //    {
            //        if (TxtPostNonMotile.Text.Trim() != null && TxtPostNonMotile.Text.Trim() != "")
            //        {
            //            TxtPostProgMotility.ClearValidationError();
            //            TxtPostNonProgressive.ClearValidationError();
            //            TxtPostNonMotile.ClearValidationError();

            //            double value = (Convert.ToDouble(TxtPostProgMotility.Text) + Convert.ToDouble(TxtPostNonProgressive.Text) + Convert.ToDouble(TxtPostNonMotile.Text));
            //            if (value != 100)
            //            {
            //                TxtPostProgMotility.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
            //                TxtPostProgMotility.RaiseValidationError();
            //                //TxtPostProgMotility.Focus();
            //            }
            //        }
            //    }
            //}
        }

        private void TxtPreNonProgressive_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (TxtPreProgMotility.Text.Trim() != null && TxtPreProgMotility.Text.Trim() != "")
            //{
            //    if (TxtPreNonProgressive.Text.Trim() != null && TxtPreNonProgressive.Text.Trim() != "")
            //    {
            //        if (TxtPreNonMotile.Text.Trim() != null && TxtPreNonMotile.Text.Trim() != "")
            //        {
            //            TxtPreProgMotility.ClearValidationError();
            //            TxtPreNonProgressive.ClearValidationError();
            //            TxtPreNonMotile.ClearValidationError();

            //            double value = (Convert.ToDouble(TxtPreProgMotility.Text) + Convert.ToDouble(TxtPreNonProgressive.Text) + Convert.ToDouble(TxtPreNonMotile.Text));
            //            if (value != 100)
            //            {
            //                TxtPreNonProgressive.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
            //                TxtPreNonProgressive.RaiseValidationError();
            //                //TxtPreNonProgressive.Focus();
            //            }
            //        }
            //    }
            //}
        }

        private void TxtPostNonProgressive_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (TxtPostProgMotility.Text.Trim() != null && TxtPostProgMotility.Text.Trim() != "")
            //{
            //    if (TxtPostNonProgressive.Text.Trim() != null && TxtPostNonProgressive.Text.Trim() != "")
            //    {
            //        if (TxtPostNonMotile.Text.Trim() != null && TxtPostNonMotile.Text.Trim() != "")
            //        {
            //            TxtPostProgMotility.ClearValidationError();
            //            TxtPostNonProgressive.ClearValidationError();
            //            TxtPostNonMotile.ClearValidationError();

            //            double value = (Convert.ToDouble(TxtPostProgMotility.Text) + Convert.ToDouble(TxtPostNonProgressive.Text) + Convert.ToDouble(TxtPostNonMotile.Text));
            //            if (value != 100)
            //            {
            //                TxtPostNonProgressive.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
            //                TxtPostNonProgressive.RaiseValidationError();
            //                //TxtPostNonProgressive.Focus();
            //            }
            //        }
            //    }
            //}
        }

        private void TxtPreNonMotile_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (TxtPreProgMotility.Text.Trim() != null && TxtPreProgMotility.Text.Trim() != "")
            //{
            //    if (TxtPreNonProgressive.Text.Trim() != null && TxtPreNonProgressive.Text.Trim() != "")
            //    {
            //        if (TxtPreNonMotile.Text.Trim() != null && TxtPreNonMotile.Text.Trim() != "")
            //        {
            //            TxtPreProgMotility.ClearValidationError();
            //            TxtPreNonProgressive.ClearValidationError();
            //            TxtPreNonMotile.ClearValidationError();
            //            double value = (Convert.ToDouble(TxtPreProgMotility.Text) + Convert.ToDouble(TxtPreNonProgressive.Text) + Convert.ToDouble(TxtPreNonMotile.Text));
            //            if (value != 100)
            //            {
            //                TxtPreNonMotile.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
            //                TxtPreNonMotile.RaiseValidationError();
            //                //xtPreNonMotile.Focus();
            //            }
            //        }
            //    }
            //}
        }

        private void TxtPostNonMotile_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (TxtPostProgMotility.Text.Trim() != null && TxtPostProgMotility.Text.Trim() != "")
            //{
            //    if (TxtPostNonProgressive.Text.Trim() != null && TxtPostNonProgressive.Text.Trim() != "")
            //    {
            //        if (TxtPostNonMotile.Text.Trim() != null && TxtPostNonMotile.Text.Trim() != "")
            //        {
            //            TxtPostProgMotility.ClearValidationError();
            //            TxtPostNonProgressive.ClearValidationError();
            //            TxtPostNonMotile.ClearValidationError();

            //            double value = (Convert.ToDouble(TxtPostProgMotility.Text) + Convert.ToDouble(TxtPostNonProgressive.Text) + Convert.ToDouble(TxtPostNonMotile.Text));
            //            if (value != 100)
            //            {
            //                TxtPostNonMotile.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
            //                TxtPostNonMotile.RaiseValidationError();
            //                //TxtPostNonMotile.Focus();
            //            }
            //        }
            //    }
            //}
        }

    }
}
