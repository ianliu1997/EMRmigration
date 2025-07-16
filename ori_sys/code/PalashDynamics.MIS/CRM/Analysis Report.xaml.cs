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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Inventory;
using System.Windows.Browser;
using System.Reflection;

namespace PalashDynamics.MIS.CRM
{
    public partial class Analysis_Report : UserControl
    {
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;

        public Analysis_Report()
        {
            InitializeComponent();
        }

        private void SearchPatient_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;

            FillUnitList();
            //FillStateList();
            fillProtocolType();
            fillOocyteSource();
            fillSemenSource();
            fillPlannedTreatment();
            FillState();
            cmbAge.ItemsSource = FillAge();
            cmbAge.SelectedValue = (long)0;
            fillDrugComboBox();
            dtpFromDate.Focus();
        }
        //rohinee
        public void FillState()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_StateMaster;
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

                    cmbState.ItemsSource = null;
                    cmbState.ItemsSource = objList;
                    cmbState.SelectedValue = (long)0;

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillUnitList()
        {
            try
            {

                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                        cmbClinic.ItemsSource = null;
                        cmbClinic.ItemsSource = objList;

                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {

                            var res = from r in objList
                                      where r.ID == User.UserLoginInfo.UnitId
                                      select r;
                            cmbClinic.SelectedItem = ((MasterListItem)res.First());
                            cmbClinic.IsEnabled = false;
                        }
                        else
                            cmbClinic.SelectedItem = objList[0];

                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //rohinee
        //private void FillStateList()
        //{
        //    clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
        //    BizAction.TableName = "T_Registration";
        //    BizAction.ColumnName = "State";
        //    BizAction.IsDecode = true;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            txtState.ItemsSource = null;
        //            txtState.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

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

        //private void fillOocyteSource()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_IVF_SourceOocyteMaster;
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

        //            cmbSrcOocyte.ItemsSource = null;
        //            cmbSrcOocyte.ItemsSource = objList;
        //            cmbSrcOocyte.SelectedValue = 0;

        //            if (this.DataContext != null)
        //            {
        //                cmbSrcOocyte.SelectedValue = (long)0; //((clsFemaleLabDay0VO)this.DataContext).SrcOfOocyteID;
        //            }
        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //private void fillSemenSource()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_IVF_SourceSemenMaster;
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

        //            cmbSrcSemen.ItemsSource = null;
        //            cmbSrcSemen.ItemsSource = objList;
        //            cmbSrcSemen.SelectedValue = 0;

        //            if (this.DataContext != null)
        //            {
        //                cmbSrcSemen.SelectedValue = (long)0; //((clsFemaleLabDay0VO)this.DataContext).SrcOfSemenID;
        //            }
        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

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
                    cmbSrcSemen.SelectedValue = (long)0;

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
                    cmbSrcOocyte.SelectedValue = (long)0;

                    if (this.DataContext != null)
                    {
                        cmbSrcOocyte.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).SrcOfOocyteID;
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
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
        //            cmbSrcTreatmentPlan.ItemsSource = null;
        //            cmbSrcTreatmentPlan.ItemsSource = objList;
        //            cmbSrcTreatmentPlan.SelectedValue = 0;
        //            if (this.DataContext != null)
        //            {
        //                cmbSrcTreatmentPlan.SelectedValue = (long)0;  //((clsFemaleLabDay0VO)this.DataContext).TreatmentTypeID;
        //            }
        //        }

        //        //fillPlan();

        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

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
                    cmbSrcTreatmentPlan.SelectedValue = (long)0;
                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void fillDrugComboBox()
        {

            clsGetItemListBizActionVO BizAction = new clsGetItemListBizActionVO();
            BizAction.ItemDetails = new clsItemMasterVO();
            BizAction.ItemDetails.RetrieveDataFlag = false;


            //BizAction.FilterClinicId =0;
            BizAction.FilterCriteria = 1;
            //BizAction.FilterICatId=0;
            //    BizAction.FilterIDispensingId=0;
            BizAction.FilterIGroupID = 1;
            // BizAction.FilterIMoleculeNameId=0;
            // BizAction.FilterITherClassId = 0;
            // BizAction.FilterStoreId=0;
            BizAction.ForReportFilter = true;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {
                    MasterListItem item = new MasterListItem();
                    item.ID = 0;
                    item.Description = "--Select--";
                    ((clsGetItemListBizActionVO)args.Result).MasterList.Insert(0, item);
                    CmbDrug.ItemsSource = ((clsGetItemListBizActionVO)args.Result).MasterList;
                    //if (DrugId == 0)
                    CmbDrug.SelectedValue = (long)0;
                    //else
                    //    CmbDrug.SelectedValue = (long)DrugId;
                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public static List<MasterListItem> FillAge()
        {
            List<MasterListItem> objAge = new List<MasterListItem>();
            objAge.Add(new MasterListItem(0, "Select"));
            objAge.Add(new MasterListItem(1, "="));
            objAge.Add(new MasterListItem(2, "<"));
            objAge.Add(new MasterListItem(3, ">"));

            return objAge;
        }

        private void cmbAge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAge.SelectedItem != null && ((MasterListItem)cmbAge.SelectedItem).ID != 0)
            {
                txtAge.IsReadOnly = false;
            }
            else
            {
                txtAge.IsReadOnly = true;
                txtAge.Text = string.Empty;
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Nullable<DateTime> dtFT = null;
            Nullable<DateTime> dtTT = null;
            Nullable<DateTime> dtTP = null;

            string AgeFilter = null;
            long StateID = 0;
            long UnitID = 0;
            int Age = 0;
            long ProtocolTypeID = 0;
            long TreatmentPlanID = 0;
            long SrcOocyteId = 0;
            long SrcSemenID = 0;
            bool chkToDate = true;
            long DrugID = 0;
            string msgTitle = "";

            if (dtpFromDate.SelectedDate != null)
            {
                dtFT = dtpFromDate.SelectedDate.Value.Date.Date;
            }

            if (dtpToDate.SelectedDate != null)
            {
                dtTT = dtpToDate.SelectedDate.Value.Date.Date;
                if (dtFT.Value > dtTT.Value)
                {
                    dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                    dtTP = dtFT;
                    chkToDate = false;
                }
                else
                {
                    dtTP = dtTT;
                    //dtTT = dtTP.Value.Date.AddDays(1);
                    dtTT = dtTP.Value.AddDays(1);
                }

            }

            if (dtTT != null)
            {
                if (dtFT != null)
                {
                    dtFT = dtpFromDate.SelectedDate.Value.Date.Date;

                    //if (dtpF.Equals(dtpT))
                    //    dtpT = dtpF.Value.Date.AddDays(1);
                }
            }

            //if (txtState.Text != null)
            //    State = txtState.Text;
            if(cmbState.SelectedItem!=null)
                StateID=((MasterListItem)cmbState.SelectedItem).ID;

            if (cmbClinic.SelectedItem != null)
                UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;

            if (cmbProtocol.SelectedItem != null)
                ProtocolTypeID = ((MasterListItem)cmbProtocol.SelectedItem).ID;

            if (cmbSrcTreatmentPlan.SelectedItem != null)
                TreatmentPlanID = ((MasterListItem)cmbSrcTreatmentPlan.SelectedItem).ID;

            if (cmbSrcOocyte.SelectedItem != null)
                SrcOocyteId = ((MasterListItem)cmbSrcOocyte.SelectedItem).ID;


            if (cmbSrcSemen.SelectedItem != null)
                SrcSemenID = ((MasterListItem)cmbSrcSemen.SelectedItem).ID;

            if (CmbDrug.SelectedItem != null)
                DrugID = ((MasterListItem)CmbDrug.SelectedItem).ID;

            if (txtAge.Text.Trim() != "")
            {
                Age = int.Parse(txtAge.Text);
            }

            if (cmbAge.SelectedItem != null)
                AgeFilter = ((MasterListItem)cmbAge.SelectedItem).Description;

            if (chkToDate == true)
            {

                string URL;

                URL = "../Reports/CRM/PatientAnalysisReport.aspx?FromDate=" + dtFT.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtTT.Value.ToString("dd/MMM/yyyy") + "&ClinicID=" + UnitID + "&ProtocolTypeID=" + ProtocolTypeID + "&TreatmentPlanID=" + TreatmentPlanID + "&SrcOocyteId=" + SrcOocyteId + "&SrcSemenID=" + SrcSemenID + "&DrugID=" + DrugID + "&Age=" + Age + "&StateID=" + StateID + "&AgeFilter=" + AgeFilter;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

            }
            else
            {
                string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.CRM.CRMReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

    }
}
