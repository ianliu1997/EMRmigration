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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.Controls;
using System.Windows.Markup;
using System.Text;
using System.Windows.Controls.Primitives;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PalashDynamics.Administration
{
    public partial class frmPackageDefineRuleMaster : ChildWindow
    {
        List<MasterListItem> objSpecializationList = new List<MasterListItem>();
        public frmPackageDefineRuleMaster()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler OnAddButton_Click;
        //List<clsPackageServiceDetailsVO> ConditionAND = new List<clsPackageServiceDetailsVO>();

        //List<clsPackageServiceDetailsVO> ConditionOR = new List<clsPackageServiceDetailsVO>();

        public ObservableCollection<clsPackageServiceConditionsVO> ConditionANDOR = new ObservableCollection<clsPackageServiceConditionsVO>();
        public ObservableCollection<clsPackageServiceConditionsVO> ConditionANDORDelete = new ObservableCollection<clsPackageServiceConditionsVO>();

        public ObservableCollection<clsPackageServiceDetailsVO> ServiceListFromParent = new ObservableCollection<clsPackageServiceDetailsVO>();

        public List<clsPackageServiceRelationsVO> PackageServiceRelationList = new List<clsPackageServiceRelationsVO>();
        public List<clsPackageServiceRelationsVO> PackageServiceRelationListDelete = new List<clsPackageServiceRelationsVO>();

        public clsPackageServiceVO objPackMasterCopy = new clsPackageServiceVO();   //Package New Changes for Procedure Added on 17042018

        public long PackageServiceID { get; set; }
        public long PackageServiceUnitID { get; set; }
        public long PackageID { get; set; }

        public long PackageDuration { get; set; }
        public bool IsFreeze { get; set; }
        public double PrevQty { get; set; }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //List<MasterListItem> lst = new List<MasterListItem>();
            //MasterListItem obj = new MasterListItem();
            //obj.Status = true;
            //lst.Add(obj);
            //dgCalender.ItemsSource = lst;

            if (ServiceListFromParent != null && ServiceListFromParent.Count > 0)
            {
                PrevQty = ServiceListFromParent[0].Quantity;
            }


            FillPackageRelations();

            ConditionANDOR = new ObservableCollection<clsPackageServiceConditionsVO>();

            //Added By CDS 
            if (IsFreeze == true)
            {
                cmdSave.IsEnabled = false;
            }
            else
            {
                cmdSave.IsEnabled = true;
            }

            rdbPercentage.IsChecked = false;
            rdbDoctorShare.IsChecked = false;
            rdbFixed.IsChecked = true;

            RateLimitTB.Visibility = Visibility.Visible;
            txtRate.Visibility = Visibility.Visible;
            txtRatePercentage.Visibility = Visibility.Collapsed;

            //rdbDoctorShare.Visibility = Visibility.Collapsed;
        }

        List<MasterListItem> PackageRelationsList;


        private void FillPackageRelations()
        {
            try
            {
                clsGetPackageRelationsBizActionVO BizAction = new clsGetPackageRelationsBizActionVO();

                BizAction.PackageServiceID = PackageServiceID;
                BizAction.PackageServiceUnitID = PackageServiceUnitID;
                BizAction.PackageRelationsList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        PackageRelationsList = new List<MasterListItem>();
                        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        PackageRelationsList.Add(new MasterListItem(0, "-- Select --"));
                        //PackageRelationsList.Add(new MasterListItem(1, "-- Select All--"));
                        PackageRelationsList.AddRange(((clsGetPackageRelationsBizActionVO)e.Result).PackageRelationsList);

                        foreach (var item in PackageRelationsList)
                        {
                            item.Status = false;
                        }

                        cmbPackageRelations.ItemsSource = null;
                        cmbPackageRelations.ItemsSource = PackageRelationsList;



                        if (PackageRelationsList != null)
                        {
                            cmbPackageRelations.SelectedItem = PackageRelationsList[0];
                        }

                        //FilRelationsCombobox(sender);

                        //if (PackageRelationsList[0].Status == true)
                        //{
                        //    foreach (var item in PackageRelationsList)
                        //    {
                        //        item.Status = false;
                        //    }
                        //}
                    }
                    FillANDOR();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (((MasterListItem)cmbPackageRelations.SelectedItem).ID == 1) // && ((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked == true)
            {

                foreach (var item in PackageRelationsList)
                {
                    item.Status = false;
                }

            }

            if (((MasterListItem)cmbPackageRelations.SelectedItem).ID == 1) // && ((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked ==false)
            {

                foreach (var item in PackageRelationsList)
                {
                    item.Status = false;
                }


            }

            if (((MasterListItem)cmbPackageRelations.SelectedItem).ID == 0) // && ((MasterListItem)cmbPackageRelations.SelectedItem).Status == false)
            {
                string flag = "";

                foreach (var item in PackageRelationsList)
                {
                    if (item.Status == true)
                    {

                        item.Status = true;
                    }
                    else
                    {
                        ((MasterListItem)cmbPackageRelations.SelectedItem).Status = false;
                        item.Status = false;
                        flag = "ClearSelectAll";
                    }
                }
                if (flag == "ClearSelectAll")
                {
                    ((MasterListItem)cmbPackageRelations.SelectedItem).ID = 1;
                    ((MasterListItem)cmbPackageRelations.SelectedItem).Status = false;
                }
            }

        }







        //void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{

        //    if (((MasterListItem)cmbPackageRelations.SelectedItem).ID == 0 && ((MasterListItem)cmbPackageRelations.SelectedItem).Status == true)
        //    {
        //        foreach (var item in PackageRelationsList)
        //        {
        //            item.Status = true;
        //        }
        //    }
        //    else if (((MasterListItem)cmbPackageRelations.SelectedItem).ID == 0 && ((MasterListItem)cmbPackageRelations.SelectedItem).Status == false)
        //    {
        //        foreach (var item in PackageRelationsList)
        //        {
        //            item.Status = false;
        //        }
        //    }

        //}


        //else if (((MasterListItem)cmbPackageRelations.SelectedItem).ID != 0 && ((MasterListItem)cmbPackageRelations.SelectedItem).Status == false)
        //{
        //    foreach (var item in PackageRelationsList)
        //    {
        //        if (item.ID > 0)
        //        {
        //            if (item.Status == true)
        //            {
        //                //((MasterListItem)cmbPackageRelations.SelectedItem).isChecked
        //                //((MasterListItem)cmbPackageRelations.SelectedItem).Status = true;
        //                item.Status = true;
        //            }
        //            else
        //            {
        //                ((MasterListItem)cmbPackageRelations.SelectedItem).Status = false;
        //                item.Status = false;
        //            }
        //        }
        //        else
        //        {
        //            item.Status = false;
        //        }
        //    }
        //}



        //if (((MasterListItem)cmbPackageRelations.SelectedItem).ID > 0)
        //{
        //    //foreach (var item in PackageRelationsList)
        //    //{
        //    //    item.Status = false;
        //    //}
        //    if(((MasterListItem)cmbPackageRelations.SelectedItem).isChecked==true)
        //    {
        //        ((MasterListItem)cmbPackageRelations.SelectedItem).Status = true;
        //    }

        //}



        public void SetString()
        {
            string str = "";
            foreach (MasterListItem item in cmbPackageRelations.ItemsSource)
            {
                if (item.Status)
                {
                    str = str + item.Description + ",";
                    //str = str + item.PackageInPackageItemList + ",";
                }
            }
            if (str != "")
                str = str.Substring(0, str.Length - 1);
            cmbPackageRelations.Text = str;
        }

        List<MasterListItem> ANDORList;

        private void FillANDOR()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ConditionsMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    ANDORList = new List<MasterListItem>();

                    ANDORList.Add(new MasterListItem(0, "- Select -"));
                    ANDORList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    FillService();
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        List<MasterListItem> objList = new List<MasterListItem>();
        List<MasterListItem> Service = new List<MasterListItem>();

        private void FillService()
        {
            clsGetSearchMasterListBizActionVO BizAction = new clsGetSearchMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ServiceMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    objList = ((clsGetSearchMasterListBizActionVO)arg.Result).MasterList;

                    clsPackageServiceConditionsVO obj = new clsPackageServiceConditionsVO();

                    obj.ServiceList.Add(new MasterListItem(0, "- Select -"));
                    obj.ServiceList.AddRange(objList);
                    obj.SelectedService = obj.ServiceList[0];  // new MasterListItem(0, "--Select--");

                    obj.ConditionList = ANDORList;

                    ConditionANDOR.Add(obj);

                    dgANDORGrid.ItemsSource = null;
                    dgANDORGrid.ItemsSource = ConditionANDOR;

                    if (ServiceListFromParent != null && ServiceListFromParent.Count > 0)
                    {

                        lblServiceName.Content = ServiceListFromParent[0].ServiceName;

                        if (ServiceListFromParent[0].IsSpecilizationGroup == true)
                        {
                            dgANDORGrid.IsEnabled = false;
                            //txtRate.IsEnabled = false;
                            txtRate.IsEnabled = true;
                            chkDiscountOnQuantity.IsEnabled = false;
                            chkFollowupNotRequired.IsEnabled = false;
                            dgCalender.IsEnabled = false;
                        }
                        else
                        {
                            dgANDORGrid.IsEnabled = true;
                            //txtRate.IsEnabled = true;
                            chkDiscountOnQuantity.IsEnabled = true;
                            chkFollowupNotRequired.IsEnabled = true;
                            dgCalender.IsEnabled = true;
                        }
                    }

                    FillGender();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        List<MasterListItem> GenderList;

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ApplicableToGenderMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    GenderList = new List<MasterListItem>();

                    GenderList.Add(new MasterListItem(0, "- Select -"));
                    GenderList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = GenderList.DeepCopy();

                    cmbGender.SelectedValue = (long)0;


                    // Commented for Package New Changes 16042018
                    //if (ServiceListFromParent != null && ServiceListFromParent[0].ID > 0)
                    //{
                    //    GetPackageConditionalServicesAndRelations();
                    //    CreateBindingDataOnView(ServiceListFromParent);
                    //}
                    //else
                    //{
                    //    CreateBindingData(ServiceListFromParent);
                    //}

                    if (chkAdjustableHead.IsChecked == true)
                    {
                        cmbGender.SelectedItem = GenderList.FirstOrDefault(p => p.ID == 3);
                    }

                    FillProcedure();    // For Package New Changes Added on 16042018
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        List<MasterListItem> ProcedureList;
        // For Package New Changes Added on 16042018
        private void FillProcedure()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ProcessMaster;    // For Package New Changes Added on 18042019
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    ProcedureList = new List<MasterListItem>();

                    ProcedureList.Add(new MasterListItem(0, "- Select -"));
                    ProcedureList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbProcedure.ItemsSource = null;
                    cmbProcedure.ItemsSource = ProcedureList.DeepCopy();

                    cmbProcedure.SelectedValue = (long)0;



                    if (ServiceListFromParent != null && ServiceListFromParent[0].ID > 0)
                    {
                        GetPackageConditionalServicesAndRelations();
                        CreateBindingDataOnView(ServiceListFromParent);
                    }
                    else
                    {
                        CreateBindingData(ServiceListFromParent);
                    }

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private StringTable StringTable { get; set; }

        public void CreateBindingData(ObservableCollection<clsPackageServiceDetailsVO> ServiceList1)
        {
            List<clsPackageServiceDetailsVO> SortedList = ServiceList1.OrderBy(o => o.ServiceName).ToList();
            //var record = SortedList.Select(c => c.Month).Max().ToString();

            ObservableCollection<PackageList> PatientList1 = new ObservableCollection<PackageList>();

            int Cnt2 = 0;

            if (dgCalender.ItemsSource != null)
            {
                ObservableCollection<PackageList> PackageServiceList = new ObservableCollection<PackageList>();
                PackageServiceList = ((ObservableCollection<PackageList>)dgCalender.ItemsSource);

                PackageList objPackList;

                foreach (var item2 in PackageServiceList)   //(var item2 in ServiceList)
                {
                    objPackList = new PackageList();
                    objPackList.Months = new ObservableCollection<PackageMonths>();

                    foreach (var item11 in dgCalender.Columns)  //(var item in item2.Months)
                    {

                        objPackList.ServiceID = item2.ServiceID;
                        objPackList.ServiceName = item2.ServiceName;

                        objPackList.GenderList = GenderList;

                        //if (item11.Header.ToString() == "Rate")
                        //    objPackList.Rate = Convert.ToDouble(((TextBlock)item11.GetCellContent(item2)).Text);

                        //if (item11.Header.ToString() == "Amount")
                        //    objPackList.Amount = Convert.ToDouble(((TextBlock)item11.GetCellContent(item2)).Text);

                        //if (item11.Header.ToString() == "Discount")
                        //    objPackList.Discount = Convert.ToDouble(((TextBlock)item11.GetCellContent(item2)).Text);

                        //if (item11.Header.ToString() == "Applicable To")
                        //    objPackList.SelectedGender = ((MasterListItem)((AutoCompleteComboBox)item11.GetCellContent(item2)).SelectedItem);

                        //if (item11.Header.ToString() == "Unlimited Quantity")
                        //    objPackList.Infinite = ((CheckBox)item11.GetCellContent(item2)).IsChecked.Value;

                        //if (item11.Header.ToString() == "Quantity")
                        //    objPackList.Quantity = Convert.ToDouble(((TextBlock)item11.GetCellContent(item2)).Text);

                        //if (item11.Header.ToString() == "Free At FollowUp")
                        //    objPackList.FreeAtFollowUp = ((CheckBox)item11.GetCellContent(item2)).IsChecked.Value;

                        objPackList.Delete = item2.Delete;
                        objPackList.DepartmentID = item2.DepartmentID;
                        objPackList.IsSpecilizationGroup = item2.IsSpecilizationGroup;

                        //if (item11.Header.ToString() != "Service Name" && item11.Header.ToString() != "Rate" && item11.Header.ToString() != "Amount" && item11.Header.ToString() != "Discount" && item11.Header.ToString() != "Applicable To" && item11.Header.ToString() != "Unlimited Quantity" && item11.Header.ToString() != "Quantity" && item11.Header.ToString() != "Free At FollowUp" && item11.Header.ToString() != "Delete")
                        //{
                        PackageMonths packmon = new PackageMonths();
                        packmon.Month = item11.Header.ToString();
                        packmon.MonthStatus = ((CheckBox)item11.GetCellContent(item2)).IsChecked.Value;

                        objPackList.Months.Add(packmon);

                        //}




                        ////}
                    }

                    PatientList1.Add(objPackList);

                }

            }

            Cnt2++;

            ////List<PackageList> PatientList1 = new List<PackageList>();
            PackageMonths list2item;

            System.Collections.ObjectModel.ObservableCollection<PackageMonths> list1item;

            var Cnt = "";
            int Cnt1 = 0;


            String SerName = "";
            long SerId = 0;

            list1item = new System.Collections.ObjectModel.ObservableCollection<PackageMonths>();

            foreach (var item in SortedList)
            {
                if (Cnt1 == 0)
                    list1item = new System.Collections.ObjectModel.ObservableCollection<PackageMonths>();

                ////list2item = new PackageMonths();

                ////if (SerName == "" || SerName != item.ServiceName)
                ////    SerName = item.ServiceName;

                if (SerId == 0 || SerId != item.ServiceID)
                    SerId = item.ServiceID;

                if (item.FromPackage == false)
                {
                    for (int k = 1; k <= PackageDuration; k++)  //for (int k = 1; k <= dgCalender.Columns.Count - 9; k++)
                    {

                        list2item = new PackageMonths();

                        list2item.Month = k.ToString();  //item.Month;
                        list2item.MonthStatus = item.MonthStatus;   //false;

                        if (SerId == item.ServiceID)  //(SerName == item.ServiceName)
                        {
                            list1item.Add(list2item);
                            Cnt1++;
                        }
                        else
                        {
                            SerId = item.ServiceID; //SerName = item.ServiceName;
                            Cnt1 = 0;
                        }

                    }
                }
                else if (item.FromPackage == true)
                {
                    //for (int k = 1; k <= Convert.ToInt32(item.Validity); k++)
                    //{

                    list2item = new PackageMonths();

                    list2item.Month = item.Month;
                    list2item.MonthStatus = item.MonthStatus;

                    if (SerId == item.ServiceID)  //(SerName == item.ServiceName)
                    {
                        list1item.Add(list2item);
                        Cnt1++;
                    }
                    else
                    {
                        SerId = item.ServiceID;  //SerName = item.ServiceName;
                        Cnt1 = 0;
                    }

                    //}
                }

                Cnt = SortedList.Count(c => c.ServiceID == SerId).ToString(); //(c => c.ServiceName == SerName).ToString();

                //if (Cnt1 == Convert.ToInt32(Cnt))
                //{

                if (item.FromPackage == false)
                {
                    var result = from r in (GenderList)
                                 where r.ID == item.ApplicableTo   //3
                                 select r;

                    if (result != null)
                    {
                        item.SelectedGender = ((MasterListItem)result.First());
                    }

                    ////var item12 = (object)null;
                    long ItemListCnt = 0;

                    if (item.IsSpecilizationGroup == false)
                    {
                        var item12 = from r2 in PatientList1
                                     where (r2.ServiceID == item.ServiceID && r2.DepartmentID == item.DepartmentID && r2.IsSpecilizationGroup == false)
                                     select r2;
                        ////select new PackageList
                        ////{
                        ////    ServiceID = r2.ServiceID
                        ////};

                        ItemListCnt = item12.ToList().Count;
                    }
                    else if (item.IsSpecilizationGroup == true)
                    {
                        var item13 = from r2 in PatientList1
                                     where (r2.ServiceID == item.ServiceID && r2.DepartmentID == item.DepartmentID && r2.IsSpecilizationGroup == true)
                                     select r2;
                        ////select new PackageList
                        ////{
                        ////    ServiceID = r2.ServiceID
                        ////};

                        ItemListCnt = item13.ToList().Count;
                    }

                    if (ItemListCnt == 0) //if (item12.ToList().Count == 0)
                        PatientList1.Add(new PackageList { ServiceID = item.ServiceID, ServiceName = item.ServiceName, Rate = item.Rate, Amount = item.Amount, Discount = item.Discount, GenderList = GenderList, SelectedGender = item.SelectedGender, Infinite = item.Infinite, Quantity = item.Quantity, FreeAtFollowUp = item.FreeAtFollowUp, Delete = item.Delete, Months = list1item, DepartmentID = item.DepartmentID, IsSpecilizationGroup = item.IsSpecilizationGroup });

                    txtRate.Text = item.Rate.ToString();

                    Cnt1 = 0;

                }
                else if (item.FromPackage == true)
                {
                    var result = from r in (GenderList)
                                 where r.ID == item.ApplicableTo
                                 select r;

                    if (result != null)
                    {
                        item.SelectedGender = ((MasterListItem)result.First());
                    }

                    int Cnt3 = SortedList.Count(c => c.ServiceID == item.ServiceID);

                    if (Cnt1 == Convert.ToInt32(item.Validity))  //if (Cnt1 == Cnt3)
                    {

                        ////var item12 = (object)null;
                        long ItemListCnt = 0;

                        if (item.IsSpecilizationGroup == false)
                        {
                            var item12 = from r2 in PatientList1
                                         where (r2.ServiceID == item.ServiceID && r2.DepartmentID == item.DepartmentID && r2.IsSpecilizationGroup == false)  //where (r2.ServiceID == item.ServiceID)
                                         select r2;
                            ////select new PackageList
                            ////{
                            ////    ServiceID = r2.ServiceID
                            ////};

                            ItemListCnt = item12.ToList().Count;
                        }
                        else if (item.IsSpecilizationGroup == true)
                        {
                            var item13 = from r2 in PatientList1
                                         where (r2.ServiceID == item.ServiceID && r2.DepartmentID == item.DepartmentID && r2.IsSpecilizationGroup == true)
                                         select r2;
                            ////select new PackageList
                            ////{
                            ////    ServiceID = r2.ServiceID
                            ////};

                            ItemListCnt = item13.ToList().Count;
                        }

                        if (ItemListCnt == 0) //if (item12.ToList().Count == 0)
                            PatientList1.Add(new PackageList { ServiceID = item.ServiceID, ServiceName = item.ServiceName, Rate = item.Rate, Amount = item.Amount, Discount = item.Discount, GenderList = GenderList, SelectedGender = item.SelectedGender, Infinite = item.Infinite, Quantity = item.Quantity, FreeAtFollowUp = item.FreeAtFollowUp, Delete = item.Delete, Months = list1item, DepartmentID = item.DepartmentID, IsSpecilizationGroup = item.IsSpecilizationGroup });

                        txtRate.Text = item.Rate.ToString();

                        Cnt1 = 0;

                    }

                }



                //}

            }


            //StringTable sampleTable = new StringTable();
            //this.StringTable = sampleTable;
            //for (int i = 0; i < PatientList1.Count; i++)
            //{
            //    StringRow newRowValues = new StringRow();

            //    if (i == 0)
            //    {
            //        this.StringTable.ColumnNames.Add("Service Name");
            //        this.StringTable.ColumnNames.Add("Rate");
            //        this.StringTable.ColumnNames.Add("Unlimited Quantity");
            //        this.StringTable.ColumnNames.Add("Quantity");
            //        this.StringTable.ColumnNames.Add("Free At FollowUp");
            //        this.StringTable.ColumnNames.Add("Delete");
            //    }



            //    //newRowValues.Add("Service Name", PatientList1[i].ServiceName);
            //    //newRowValues.Add("Rate", PatientList1[i].Rate);
            //    //newRowValues.Add("Unlimited Quantity", PatientList1[i].Infinite);
            //    //newRowValues.Add("Quantity", PatientList1[i].Quantity);
            //    //newRowValues.Add("Free At Followup", PatientList1[i].FreeAtFollowUp);
            //    //newRowValues.Add("Delete", PatientList1[i].Delete);

            //    newRowValues["Service Name"] = PatientList1[i].ServiceName;
            //    newRowValues["Rate"] = PatientList1[i].Rate;
            //    newRowValues["Unlimited Quantity"] = PatientList1[i].Infinite;
            //    newRowValues["Quantity"] = PatientList1[i].Quantity;
            //    newRowValues["Free At FollowUp"] = PatientList1[i].FreeAtFollowUp;
            //    newRowValues["Delete"] = PatientList1[i].Delete;

            //    for (int j = 0; j < PatientList1[i].Months.Count; j++)
            //    {

            //        if (i == 0)
            //        {
            //            this.StringTable.ColumnNames.Add(PatientList1[i].Months[j].Month);
            //        }

            //        //newRowValues.Add(PatientList1[i].Months[j].Month, PatientList1[i].Months[j].MonthStatus);

            //        newRowValues[PatientList1[i].Months[j].Month] = PatientList1[i].Months[j].MonthStatus;

            //    }

            //    this.StringTable.Add(this.StringTable.Count, newRowValues);
            //}

            ////dgCalender.ItemsSource = StringTable.Values;
            ////dgCalender.UpdateLayout();

            RefreshDataGrid(PatientList1);

        }

        public void CreateBindingDataOnView(ObservableCollection<clsPackageServiceDetailsVO> ServiceListOnView)
        {
            try
            {
                List<clsPackageServiceDetailsVO> SortedList = ServiceListOnView.OrderBy(o => o.ServiceName).ToList();   //ServiceList
                var record = SortedList.Select(c => c.Month).Max().ToString();

                ObservableCollection<PackageList> PatientList2 = new ObservableCollection<PackageList>();
                //List<PackageList> PatientList1 = new List<PackageList>();

                PackageMonths list2item;

                System.Collections.ObjectModel.ObservableCollection<PackageMonths> list1item;
                var Cnt = "";
                int Cnt1 = 0;
                String SerName = "";
                list1item = new System.Collections.ObjectModel.ObservableCollection<PackageMonths>();

                foreach (var item in SortedList)
                {
                    if (Cnt1 == 0)
                        list1item = new System.Collections.ObjectModel.ObservableCollection<PackageMonths>();

                    list2item = new PackageMonths();

                    if (SerName == "" || SerName != item.ServiceName)
                        SerName = item.ServiceName;

                    list2item.Month = item.Month;

                    list2item.MonthStatus = item.MonthStatus;

                    if (SerName == item.ServiceName)
                    {
                        list1item.Add(list2item);
                        Cnt1++;
                    }
                    else
                    {
                        SerName = item.ServiceName;
                        Cnt1 = 0;
                    }

                    Cnt = SortedList.Count(c => c.ServiceName == SerName).ToString();

                    var result = from r in (GenderList)
                                 where r.ID == item.ApplicableTo
                                 select r;

                    if (result != null)
                    {
                        item.SelectedGender = ((MasterListItem)result.First());
                    }

                    if (Cnt1 == 1)
                    {
                        cmbGender.SelectedItem = ((MasterListItem)result.First());
                    }

                    #region Package New Changes for Procedure Added on 16042018

                    var result1 = from r in (ProcedureList)
                                  where r.ID == item.ProcessID
                                  select r;

                    if (result1 != null)
                    {
                        item.SelectedProcess = ((MasterListItem)result1.First());
                    }

                    if (Cnt1 == 1)
                    {
                        cmbProcedure.SelectedItem = ((MasterListItem)result1.First());
                    }

                    #endregion

                    if (Cnt1 == Convert.ToInt32(Cnt))
                    {
                        //cmbGender.SelectedItem = ((MasterListItem)result.First());
                        txtDiscount.Text = item.Discount.ToString();
                        txtQuantity.Text = item.Quantity.ToString();
                        txtRate.Text = item.Rate.ToString();

                        if (item.Infinite == true)
                            chkUnlimitedQuantity.IsChecked = true;

                        chkDiscountOnQuantity.IsChecked = item.IsDiscountOnQuantity;

                        txtAgeLimit.Text = item.AgeLimit.ToString();

                        chkFollowupNotRequired.IsChecked = item.IsFollowupNotRequired;

                        chkIncludeAdjustableHead.IsChecked = item.ConsiderAdjustable;
                        // Newly Addded By CDS 



                        if (item.AdjustableHead == true)
                        {
                            chkAdjustableHead.IsChecked = item.AdjustableHead;
                            chkAdjustableHead.IsChecked = true;

                            cmbGender.IsEnabled = false;
                            txtQuantity.IsEnabled = false;
                        }
                        else
                        {
                            cmbGender.IsEnabled = true;
                            txtQuantity.IsEnabled = true;
                        }

                        if (item.AdjustableHead == true)        //Package New Changes Added on 23042018 for Procedure
                        {
                            if (item.AdjustableHeadType == 1)    // 1 = Clinical
                            {
                                rdbClinicalHead.IsChecked = true;
                            }
                            else if (item.AdjustableHeadType == 2)    // 2 = Pharmacy
                            {
                                rdbPharmacyHead.IsChecked = true;
                            }
                        }

                        if (item.IsFixed == true)
                        {
                            rdbFixed.IsChecked = true;

                            RateLimitTB.Visibility = Visibility.Visible;
                            txtRate.Visibility = Visibility.Visible;
                            txtRatePercentage.Visibility = Visibility.Collapsed;
                            //rdbDoctorShare.Visibility = Visibility.Collapsed;
                        }
                        else if (item.IsFixed == false)
                        {
                            if (item.IsDoctorSharePercentage == true)
                            {
                                rdbDoctorShare.IsChecked = true;

                                RateLimitTB.Visibility = Visibility.Collapsed;

                                txtRate.Visibility = Visibility.Collapsed;
                                txtRatePercentage.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                rdbPercentage.IsChecked = true;

                                RateLimitTB.Visibility = Visibility.Visible;
                                txtRate.Visibility = Visibility.Collapsed;
                                txtRatePercentage.Visibility = Visibility.Visible;
                            }
                            //rdbDoctorShare.Visibility = Visibility.Visible;
                        }


                        txtRatePercentage.Text = item.RatePercentage.ToString();
                        rdbDoctorShare.IsChecked = item.IsDoctorSharePercentage;

                        if (item.IsConsumables == true)        //Package New Changes Added on 23042018 for Procedure
                        {
                            chkIsConsumables.IsChecked = true;
                        }
                        else
                        {
                            chkIsConsumables.IsChecked = false;
                        }

                        //if (chkAdjustableHead.IsChecked == true)
                        //    ServiceListFromParent[cntPackServiceDetails].AdjustableHead = true;
                        //else
                        //    ServiceListFromParent[cntPackServiceDetails].AdjustableHead = false;


                        //if (rdbFixed.IsChecked == true)
                        //    ServiceListFromParent[cntPackServiceDetails].IsFixed = true;
                        //else
                        //    ServiceListFromParent[cntPackServiceDetails].IsFixed = false;

                        //if (txtRate.Text.Trim() != String.Empty && txtRate.Text.IsValueDouble())
                        //    ServiceListFromParent[cntPackServiceDetails].Rate = Convert.ToDouble(txtRate.Text.Trim());


                        //if (txtRatePercentage.Text.Trim() != String.Empty && txtRatePercentage.Text.IsValueDouble())
                        //    ServiceListFromParent[cntPackServiceDetails].RatePercentage = Convert.ToDouble(txtRatePercentage.Text.Trim());

                        //if (rdbDoctorShare.IsChecked == true)
                        //    ServiceListFromParent[cntPackServiceDetails].IsDoctorSharePercentage = true;
                        //else
                        //    ServiceListFromParent[cntPackServiceDetails].IsDoctorSharePercentage = false;

                        //


                        PatientList2.Add(new PackageList { ServiceID = item.ServiceID, ServiceName = item.ServiceName, Rate = item.Rate, Amount = item.Amount, Discount = item.Discount, GenderList = GenderList, SelectedGender = item.SelectedGender, Infinite = item.Infinite, Quantity = item.Quantity, FreeAtFollowUp = item.FreeAtFollowUp, Delete = item.Delete, Months = list1item, DepartmentID = item.DepartmentID, IsSpecilizationGroup = item.IsSpecilizationGroup });
                        Cnt1 = 0;

                        //if (ServiceListFromParent != null && ServiceListFromParent.Count > 0)
                        //{

                        //    lblServiceName.Content = ServiceListFromParent[0].ServiceName;

                        if (item.IsSpecilizationGroup == true)
                        {
                            if (item.IsSpecilizationGroup == true && this.IsFreeze == true)
                            {
                                //dgANDORGrid.IsEnabled = false;
                                //txtRate.IsEnabled = false;
                                //chkDiscountOnQuantity.IsEnabled = false;
                                //chkFollowupNotRequired.IsEnabled = false;

                                cmbGender.IsEnabled = false;
                                if (chkUnlimitedQuantity.IsChecked == true)
                                    chkUnlimitedQuantity.IsEnabled = false;
                                else
                                    chkUnlimitedQuantity.IsEnabled = true;
                                txtQuantity.IsEnabled = false;
                                dgCalender.IsEnabled = false;
                            }
                            else
                            {
                                //dgANDORGrid.IsEnabled = true;
                                //txtRate.IsEnabled = true;
                                //chkDiscountOnQuantity.IsEnabled = true;
                                //chkFollowupNotRequired.IsEnabled = true;

                                cmbGender.IsEnabled = true;
                                chkUnlimitedQuantity.IsEnabled = true;
                                txtQuantity.IsEnabled = true;
                                dgCalender.IsEnabled = true;
                            }
                        }
                        else if (item.IsSpecilizationGroup == false)
                        {
                            if (item.IsSpecilizationGroup == false && this.IsFreeze == true)
                            {
                                cmbGender.IsEnabled = false;
                                chkUnlimitedQuantity.IsEnabled = false;

                                if (item.Infinite == true)
                                {
                                    dgCalender.IsEnabled = false;
                                    txtQuantity.IsEnabled = false;
                                }

                                chkDiscountOnQuantity.IsEnabled = false;

                                //txtQuantity.IsEnabled = false;
                                //dgCalender.IsEnabled = false;
                            }
                            else
                            {

                                if (item.AdjustableHead == true)
                                {
                                    chkAdjustableHead.IsChecked = item.AdjustableHead;
                                    chkAdjustableHead.IsChecked = true;

                                    cmbGender.IsEnabled = false;
                                    txtQuantity.IsEnabled = false;
                                }
                                else
                                {
                                    cmbGender.IsEnabled = true;
                                    chkUnlimitedQuantity.IsEnabled = true;
                                    txtQuantity.IsEnabled = true;
                                    dgCalender.IsEnabled = true;
                                    chkDiscountOnQuantity.IsEnabled = true;
                                }
                            }
                        }
                        //}

                    }

                }

                //if (((clsGetPackageServiceDetailsListBizActionVO)arg.Result).ItemDetails != null)
                //{
                //    ItemList1 = new ObservableCollection<clsPackageItemMasterVO>(((clsGetPackageServiceDetailsListBizActionVO)arg.Result).ItemDetails);
                //    dgPharmaItems.ItemsSource = ItemList1;
                //}

                //StringTable sampleTable = new StringTable();
                //this.StringTable = sampleTable;
                //for (int i = 0; i < PatientList2.Count; i++)
                //{
                //    StringRow newRowValues = new StringRow();

                //    if (i == 0)
                //    {
                //        this.StringTable.ColumnNames.Add("Service Name");
                //        this.StringTable.ColumnNames.Add("Rate");
                //        this.StringTable.ColumnNames.Add("Unlimited Quantity");
                //        this.StringTable.ColumnNames.Add("Quantity");
                //        this.StringTable.ColumnNames.Add("Free At FollowUp");
                //        this.StringTable.ColumnNames.Add("Delete");
                //    }

                //    //newRowValues.Add("Service Name", PatientList1[i].ServiceName);
                //    //newRowValues.Add("Rate", PatientList1[i].Rate);
                //    //newRowValues.Add("Unlimited Quantity", PatientList1[i].Infinite);
                //    //newRowValues.Add("Quantity", PatientList1[i].Quantity);
                //    //newRowValues.Add("Free At Followup", PatientList1[i].FreeAtFollowUp);
                //    //newRowValues.Add("Delete", PatientList1[i].Delete);

                //    newRowValues["Service Name"] = PatientList2[i].ServiceName;
                //    newRowValues["Rate"] = PatientList2[i].Rate;
                //    newRowValues["Unlimited Quantity"] = PatientList2[i].Infinite;
                //    newRowValues["Quantity"] = PatientList2[i].Quantity;
                //    newRowValues["Free At FollowUp"] = PatientList2[i].FreeAtFollowUp;
                //    newRowValues["Delete"] = PatientList2[i].Delete;

                //    for (int j = 0; j < PatientList2[i].Months.Count; j++)
                //    {

                //        if (i == 0)
                //        {
                //            this.StringTable.ColumnNames.Add(PatientList2[i].Months[j].Month);
                //        }

                //        //newRowValues.Add(PatientList1[i].Months[j].Month, PatientList1[i].Months[j].MonthStatus);

                //        newRowValues[PatientList2[i].Months[j].Month] = PatientList2[i].Months[j].MonthStatus;

                //    }

                //    this.StringTable.Add(this.StringTable.Count, newRowValues);
                //}

                //CreateBindingData(ServiceList);

                RefreshDataGrid(PatientList2);

            }
            catch (Exception ex)
            {

            }
        }

        private void GetPackageConditionalServicesAndRelations()
        {
            clsGetPackageServicesAndRelationsNewBizActionVO BizAction = new clsGetPackageServicesAndRelationsNewBizActionVO();
            try
            {
                BizAction.ServiceConditionList = new List<clsPackageServiceConditionsVO>();
                BizAction.PackageServiceRelationList = new List<clsPackageServiceRelationsVO>();
                BizAction.PackageID = PackageID;
                BizAction.UnitId = PackageServiceUnitID;
                BizAction.ServiceID = ServiceListFromParent[0].ServiceID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPackageServicesAndRelationsNewBizActionVO)arg.Result).ServiceConditionList != null)
                        {
                            clsGetPackageServicesAndRelationsNewBizActionVO DetailsVO = new clsGetPackageServicesAndRelationsNewBizActionVO();
                            DetailsVO = (clsGetPackageServicesAndRelationsNewBizActionVO)arg.Result;

                            List<clsPackageServiceConditionsVO> ObjServiceList;
                            ObjServiceList = DetailsVO.ServiceConditionList;

                            if (ObjServiceList != null && ObjServiceList.Count > 0)
                                ConditionANDOR = new ObservableCollection<clsPackageServiceConditionsVO>();

                            foreach (var item4 in ObjServiceList)
                            {
                                item4.ServiceList = objList;
                                item4.SelectedService = new MasterListItem(0, "--Select--");
                                item4.ConditionList = ANDORList;
                                item4.SelectedService = objList.Where(objServ => objServ.ID == item4.ServiceID).ToList().FirstOrDefault();
                                item4.SelectedCondition = ANDORList.Where(objCondition => objCondition.ID == item4.ConditionTypeID).ToList().FirstOrDefault();
                                ConditionANDOR.Add(item4);

                            }

                            if (ConditionANDOR != null && ConditionANDOR.Count > 0)
                                dgANDORGrid.ItemsSource = ConditionANDOR;

                            //List<clsPackageServiceRelationsVO> objRelationList;
                            //objRelationList = DetailsVO.PackageServiceRelationList;

                            PackageServiceRelationList = new List<clsPackageServiceRelationsVO>();
                            PackageServiceRelationList = DetailsVO.PackageServiceRelationList;

                            foreach (var item3 in PackageRelationsList)
                            {
                                foreach (var item5 in PackageServiceRelationList)  //objRelationList
                                {
                                    if (item3.ID == item5.RelationID && item3.ID > 0)
                                        item3.Status = true;

                                }

                            }

                            cmbPackageRelations.ItemsSource = PackageRelationsList;


                            var SetAll = from r in PackageServiceRelationList
                                         where r.IsSetAllRelations == true
                                         select r;


                            if (SetAll != null && SetAll.ToList().Count > 0)
                                chkAllRelations.IsChecked = true;

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }

        ObservableCollection<PackageList> PatientList3 = new ObservableCollection<PackageList>();

        private void RefreshDataGrid(ObservableCollection<PackageList> PatientList2)
        {

            //CreateDataGrid(this.StringTable);

            dgCalender.AutoGenerateColumns = false;
            //dgCalender.ItemsSource = PatientList2;
            dgCalender.Columns.Clear();

            //dgCalender.Columns.Add(CreateTextColumn("ServiceName", "Service Name"));  //fix

            //dgCalender.Columns.Add(CreateTextTemplateColumn("Rate", "Rate", true, "0.00"));                 //change

            //dgCalender.Columns.Add(CreateTextTemplateColumn("Amount", "Amount", false, "0.00"));                 //change

            //dgCalender.Columns.Add(CreateTextTemplateColumn("Discount", "Discount", false, "0.00"));                 //change

            //dgCalender.Columns.Add(CreateComboBoxTemplateColumn(0, "Applicable To"));

            //dgCalender.Columns.Add(CreateCheckBoxNonListTemplateColumn("Infinite", "Unlimited Quantity"));  //chkbox

            //dgCalender.Columns.Add(CreateTextTemplateColumn("Quantity", "Quantity", false, "0"));            //change

            //dgCalender.Columns.Add(CreateCheckBoxNonListTemplateColumn("FreeAtFollowUp", "Free At FollowUp"));  //chkbox

            //dgCalender.Columns.Add(CreateDeleteHyperlinkTemplateColumn(0, "Delete"));                    //change


            int dayMonthCount = PatientList2[0].Months.Count;
            for (int i = 0; i < dayMonthCount; i++)
            {
                //dataGrid.Columns.Add(CreateTemplateColumn(i, "Title"));
                dgCalender.Columns.Add(CreateCheckBoxListTemplateColumn(i, "MonthStatus"));
            }

            dgCalender.ItemsSource = PatientList2;
            dgCalender.UpdateLayout();

            PatientList3 = PatientList2;

        }

        private DataGridTemplateColumn CreateCheckBoxListTemplateColumn(int i, string propName)
        {
            DataGridTemplateColumn column = new DataGridTemplateColumn();
            column.Header = String.Format("{0}", i + 1);
            column.CellTemplate = (DataTemplate)XamlReader.Load(CreateCheckBoxListColumnTemplate(i, propName)); //display template
            column.CellEditingTemplate = (DataTemplate)XamlReader.Load(CreateCheckBoxListColumnEditTemplate(i, propName)); //edit template
            return column;
        }

        private string CreateCheckBoxListColumnTemplate(int index, string propertyName)
        {
            StringBuilder CellTemp = new StringBuilder();
            CellTemp.Append("<DataTemplate ");
            CellTemp.Append("xmlns='http://schemas.microsoft.com/winfx/");
            CellTemp.Append("2006/xaml/presentation' ");
            CellTemp.Append("xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>");
            CellTemp.Append(String.Format("<CheckBox HorizontalAlignment='Center' VerticalAlignment='Center' IsChecked='{{Binding Months[{0}].{1}}}'/>", index, propertyName));
            CellTemp.Append("</DataTemplate>");
            return CellTemp.ToString();
        }

        private string CreateCheckBoxListColumnEditTemplate(int index, string propertyName)
        {
            StringBuilder CellTemp = new StringBuilder();
            CellTemp.Append("<DataTemplate ");
            CellTemp.Append("xmlns='http://schemas.microsoft.com/winfx/");
            CellTemp.Append("2006/xaml/presentation' ");
            CellTemp.Append("xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>");
            CellTemp.Append(String.Format("<CheckBox HorizontalAlignment='Center' VerticalAlignment='Center' IsChecked='{{Binding Months[{0}].{1}, Mode=TwoWay}}'/>", index, propertyName));
            CellTemp.Append("</DataTemplate>");
            return CellTemp.ToString();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ChkValidation())
            {

                int cntPackServiceDetails = 0;

                ObservableCollection<PackageList> PackageServiceList = new ObservableCollection<PackageList>();
                PackageServiceList = ((ObservableCollection<PackageList>)dgCalender.ItemsSource);

                foreach (var item2 in PackageServiceList)   //(var item2 in ServiceList)
                {
                    foreach (var item in dgCalender.Columns)  //(var item in item2.Months)
                    {
                        //if (item.Header.ToString() == "Free At FollowUp")
                        //{
                        //    FreeAtFollowup = ((CheckBox)item.GetCellContent(item2)).IsChecked.Value;
                        //}

                        //if (item.Header.ToString() != "Service Name" && item.Header.ToString() != "Rate" && item.Header.ToString() != "Amount" && item.Header.ToString() != "Discount" && item.Header.ToString() != "Applicable To" && item.Header.ToString() != "Unlimited Quantity" && item.Header.ToString() != "Quantity" && item.Header.ToString() != "Free At FollowUp" && item.Header.ToString() != "Delete")
                        //{
                        clsPackageServiceDetailsVO objPackDetils = new clsPackageServiceDetailsVO();

                        if (ServiceListFromParent[0].ID == 0 && cntPackServiceDetails > 0)   //cntPackServiceDetails
                            ServiceListFromParent.Add(objPackDetils);

                        ServiceListFromParent[cntPackServiceDetails].Month = item.Header.ToString(); //item.Month;
                        ServiceListFromParent[cntPackServiceDetails].DepartmentID = item2.DepartmentID;
                        ServiceListFromParent[cntPackServiceDetails].ServiceID = item2.ServiceID;
                        ServiceListFromParent[cntPackServiceDetails].Rate = item2.Rate;
                        ServiceListFromParent[cntPackServiceDetails].Amount = item2.Amount;


                        if (txtDiscount.Text.Trim() != String.Empty && txtDiscount.Text.IsValueDouble())
                            ServiceListFromParent[cntPackServiceDetails].Discount = Convert.ToDouble(txtDiscount.Text.Trim());  //item2.Discount;

                        ServiceListFromParent[cntPackServiceDetails].IsDiscountOnQuantity = Convert.ToBoolean(chkDiscountOnQuantity.IsChecked);

                        if (!string.IsNullOrEmpty(txtAgeLimit.Text.Trim()))
                            ServiceListFromParent[cntPackServiceDetails].AgeLimit = Convert.ToInt64(txtAgeLimit.Text.Trim());

                        if (cmbGender.SelectedItem != null) //if (txtDiscount.Text.Trim() != String.Empty)
                            ServiceListFromParent[cntPackServiceDetails].SelectedGender = (MasterListItem)cmbGender.SelectedItem;//item2.SelectedGender;

                        if (cmbProcedure.SelectedItem != null)      // Package New Changes Added on 16042018 for Procedure
                        {
                            ServiceListFromParent[cntPackServiceDetails].SelectedProcess = (MasterListItem)cmbProcedure.SelectedItem;
                            ServiceListFromParent[cntPackServiceDetails].ProcessID = ((MasterListItem)cmbProcedure.SelectedItem).ID;
                        }

                        ServiceListFromParent[cntPackServiceDetails].IsFollowupNotRequired = Convert.ToBoolean(chkFollowupNotRequired.IsChecked);  //set to consider whether followup add or not while package billing

                        ServiceListFromParent[cntPackServiceDetails].Infinite = item2.Infinite;

                        if (txtQuantity.Text.Trim() != String.Empty && txtQuantity.Text.IsValueDouble())
                            ServiceListFromParent[cntPackServiceDetails].Quantity = Convert.ToDouble(txtQuantity.Text.Trim());  //item2.Quantity;

                        ServiceListFromParent[cntPackServiceDetails].FreeAtFollowUp = false;  // FreeAtFollowup;  
                        ServiceListFromParent[cntPackServiceDetails].IsSpecilizationGroup = item2.IsSpecilizationGroup;
                        ServiceListFromParent[cntPackServiceDetails].MonthStatus = ((CheckBox)item.GetCellContent(item2)).IsChecked.Value;  //item.MonthStatus; 

                        if (txtRate.Text.Trim() != String.Empty && txtRate.Text.IsValueDouble())
                            ServiceListFromParent[cntPackServiceDetails].Rate = Convert.ToDouble(txtRate.Text.Trim());

                        // Added By CDS On 19/01/2017
                        if (chkAdjustableHead.IsChecked == true)
                            ServiceListFromParent[cntPackServiceDetails].AdjustableHead = true;
                        else
                            ServiceListFromParent[cntPackServiceDetails].AdjustableHead = false;

                        if (chkAdjustableHead.IsChecked == true)    // Package New Changes Added on 18042018 for Procedure
                        {
                            if (rdbClinicalHead.IsChecked == true)
                                ServiceListFromParent[cntPackServiceDetails].AdjustableHeadType = 1;    // Clinical
                            else if (rdbPharmacyHead.IsChecked == true)
                                ServiceListFromParent[cntPackServiceDetails].AdjustableHeadType = 2;    // Pharmacy
                        }

                        if (rdbFixed.IsChecked == true)
                            ServiceListFromParent[cntPackServiceDetails].IsFixed = true;
                        else
                            ServiceListFromParent[cntPackServiceDetails].IsFixed = false;

                        if (txtRate.Text.Trim() != String.Empty && txtRate.Text.IsValueDouble())
                            ServiceListFromParent[cntPackServiceDetails].Rate = Convert.ToDouble(txtRate.Text.Trim());


                        if (txtRatePercentage.Text.Trim() != String.Empty && txtRatePercentage.Text.IsValueDouble())
                            ServiceListFromParent[cntPackServiceDetails].RatePercentage = Convert.ToDouble(txtRatePercentage.Text.Trim());

                        if (rdbDoctorShare.IsChecked == true)
                            ServiceListFromParent[cntPackServiceDetails].IsDoctorSharePercentage = true;
                        else
                            ServiceListFromParent[cntPackServiceDetails].IsDoctorSharePercentage = false;

                        // Added By CDS On 19/01/2017
                        if (chkIncludeAdjustableHead.IsChecked == true)
                            ServiceListFromParent[cntPackServiceDetails].ConsiderAdjustable = true;
                        else
                            ServiceListFromParent[cntPackServiceDetails].ConsiderAdjustable = false;

                        //END

                        if (chkIsConsumables.IsChecked == true)         // Package New Changes Added on 25042018 for Procedure
                            ServiceListFromParent[cntPackServiceDetails].IsConsumables = true;
                        else
                            ServiceListFromParent[cntPackServiceDetails].IsConsumables = false;

                        cntPackServiceDetails++;

                        //lstPackage.Add(obj);
                        //}

                    }



                    int Totrel = 0;

                    if (PackageRelationsList != null)
                        Totrel = PackageRelationsList.Count - 1;


                    var RelStatusCount = from r in PackageRelationsList
                                         where r.Status == true && r.ID > 0
                                         select r;

                    int RelStatusTrue = RelStatusCount.Count();

                    // Code Commented Becouse the Set AllRelation Combo Box is Collapsed 
                    // So the below code is Commented by CDS 

                    if (Totrel == RelStatusTrue)
                    {

                        foreach (MasterListItem itemPackServiceRel in cmbPackageRelations.ItemsSource)
                        {
                            var RelCntDel = from r in PackageServiceRelationList
                                            where r.RelationID == itemPackServiceRel.ID
                                            select r;

                            if (RelCntDel.ToList().Count > 0)
                            {
                                PackageServiceRelationListDelete.Add((clsPackageServiceRelationsVO)RelCntDel.ToList().First());
                                PackageServiceRelationList.Remove((clsPackageServiceRelationsVO)RelCntDel.ToList().First());
                            }
                        }

                        clsPackageServiceRelationsVO objPackServRel = new clsPackageServiceRelationsVO();
                        objPackServRel.RelationID = 0;
                        objPackServRel.ServiceID = item2.ServiceID;
                        objPackServRel.SpecilizationID = item2.DepartmentID;
                        objPackServRel.IsSpecilizationGroup = item2.IsSpecilizationGroup;
                        if (cmbProcedure.SelectedItem != null)      // Package New Changes Added on 25042018 for Procedure
                            objPackServRel.ProcessID = ((MasterListItem)cmbProcedure.SelectedItem).ID;
                        objPackServRel.IsSetAllRelations = true;
                        PackageServiceRelationList.Add(objPackServRel);
                    }
                    else
                    {
                        foreach (MasterListItem itemPackServiceRel in cmbPackageRelations.ItemsSource)
                        {
                            if (itemPackServiceRel.Status == true)
                            {
                                var RelCnt = from r in PackageServiceRelationList
                                             where r.RelationID == itemPackServiceRel.ID
                                             select r;

                                if (RelCnt.ToList().Count == 0)
                                {
                                    if (itemPackServiceRel.ID > 0)
                                    {
                                        clsPackageServiceRelationsVO objPackServRel = new clsPackageServiceRelationsVO();
                                        objPackServRel.RelationID = itemPackServiceRel.ID;
                                        objPackServRel.ServiceID = item2.ServiceID;
                                        objPackServRel.SpecilizationID = item2.DepartmentID;
                                        objPackServRel.IsSpecilizationGroup = item2.IsSpecilizationGroup;
                                        if (cmbProcedure.SelectedItem != null)      // Package New Changes Added on 25042018 for Procedure
                                            objPackServRel.ProcessID = ((MasterListItem)cmbProcedure.SelectedItem).ID;
                                        objPackServRel.IsSetAllRelations = false;
                                        PackageServiceRelationList.Add(objPackServRel);
                                    }
                                }
                            }
                            else if (itemPackServiceRel.Status == false)
                            {
                                var RelCntDel = from r in PackageServiceRelationList
                                                where r.RelationID == itemPackServiceRel.ID
                                                select r;

                                if (RelCntDel.ToList().Count > 0)
                                {
                                    PackageServiceRelationListDelete.Add((clsPackageServiceRelationsVO)RelCntDel.ToList().First());
                                    PackageServiceRelationList.Remove((clsPackageServiceRelationsVO)RelCntDel.ToList().First());
                                }
                            }

                        }
                    }   // Else Closing brace 

                }


                //if (ChkValidation())
                //{
                //string msgTitle = "Palash";
                //string msgText = "Are you sure you want to save the Package Service/Group ?";

                //MessageBoxControl.MessageBoxChildWindow msgW =
                //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //msgW.Show();
                msgW_OnMessageBoxClosed();
            }
            //GC.Collect();
        }

        public int clickflg = 0;
        void msgW_OnMessageBoxClosed()  //void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            clickflg += 1;
            if (clickflg == 1)
            {
                //if (result == MessageBoxResult.Yes)  //if (CheckDuplicasy())
                //{
                //    //Save();
                this.DialogResult = true;
                OnAddButton_Click(this, new RoutedEventArgs());
                //}

                //else
                //{
                //    clickflg = 0;
                //}
            }
            else
            {
                string msgText = "Please Wait, Request To Save Package Service/Group Is Been Already Processed ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.Show();
            }
        }

        private bool ChkValidation()
        {
            bool result = true;

            if (chkAdjustableHead.IsChecked == false)
            {

                #region For Package New Changes Added on 16042018

                if ((MasterListItem)cmbProcedure.SelectedItem == null)
                {

                    cmbProcedure.TextBox.SetValidation("Please Select Procedure");
                    cmbProcedure.TextBox.RaiseValidationError();
                    cmbProcedure.Focus();
                    result = false;


                }
                else if (((MasterListItem)cmbProcedure.SelectedItem).ID == 0)
                {
                    cmbProcedure.TextBox.SetValidation("Please Select Procedure");
                    cmbProcedure.TextBox.RaiseValidationError();
                    cmbProcedure.Focus();
                    result = false;

                }
                else
                    cmbProcedure.TextBox.ClearValidationError();

                #endregion

                if ((MasterListItem)cmbGender.SelectedItem == null)
                {

                    cmbGender.TextBox.SetValidation("Please Select Gender");
                    cmbGender.TextBox.RaiseValidationError();
                    cmbGender.Focus();
                    result = false;


                }
                else if (((MasterListItem)cmbGender.SelectedItem).ID == 0)
                {
                    cmbGender.TextBox.SetValidation("Please Select Gender");
                    cmbGender.TextBox.RaiseValidationError();
                    cmbGender.Focus();
                    result = false;

                }
                else
                    cmbGender.TextBox.ClearValidationError();

                if (chkDiscountOnQuantity.IsChecked == true && (txtDiscount.Text == "0" || txtDiscount.Text == ""))
                {
                    txtDiscount.SetValidation("Discount can not be zero, while checked Discount On Quantity");
                    txtDiscount.RaiseValidationError();
                    txtDiscount.Focus();
                    result = false;
                }
                else
                    txtDiscount.ClearValidationError();

                if (rdbFixed.IsChecked == true)
                {
                    if (ServiceListFromParent != null && ServiceListFromParent[0].IsSpecilizationGroup == false && (txtRate.Text == "0" || txtRate.Text == ""))
                    {
                        txtRate.SetValidation("Rate can not be zero");
                        txtRate.RaiseValidationError();
                        txtRate.Focus();
                        result = false;
                    }
                    else
                        txtRate.ClearValidationError();
                }

                if (chkUnlimitedQuantity.IsChecked == false && (txtQuantity.Text == "0" || txtQuantity.Text == ""))
                {
                    txtQuantity.SetValidation("Quantity can not be zero");
                    txtQuantity.RaiseValidationError();
                    txtQuantity.Focus();
                    result = false;
                }
                else
                    txtQuantity.ClearValidationError();

                if (rdbPercentage.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(txtRatePercentage.Text.Trim()) && Convert.ToDouble(txtRatePercentage.Text) > 100)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("", "Percentage can not be Grater Than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        txtRatePercentage.Focus();
                        txtRatePercentage.RaiseValidationError();
                        result = false;
                        return result;
                    }
                    else
                        txtRatePercentage.ClearValidationError();
                }

                bool selectPackageRelations = false;

                if (cmbPackageRelations.ItemsSource != null && (((List<MasterListItem>)(cmbPackageRelations.ItemsSource)).Count > 0))
                {
                    foreach (MasterListItem itemPackrel in ((List<MasterListItem>)(cmbPackageRelations.ItemsSource)))
                    {
                        if (itemPackrel.ID > 0 && itemPackrel.Status == true && selectPackageRelations == false)
                        {
                            selectPackageRelations = true;
                            break;
                        }
                    }
                }

                if ((cmbPackageRelations.SelectedItem != null && ((MasterListItem)cmbPackageRelations.SelectedItem).ID == 0 && selectPackageRelations == false) || (cmbPackageRelations.SelectedItem == null && selectPackageRelations == false))
                {

                    cmbPackageRelations.TextBox.SetValidation("Please Select Applicable Relations");
                    cmbPackageRelations.TextBox.RaiseValidationError();
                    cmbPackageRelations.Focus();
                    result = false;
                }
                else
                    cmbPackageRelations.TextBox.ClearValidationError();

            }

            if (result == true)     // Package New Changes for Procedure Added on 16042018
            {

                if (ServiceListFromParent != null && ServiceListFromParent.Count > 0)
                {

                    var itemExst = from ps in objPackMasterCopy.PackageDetails
                                   where ps.ServiceID == ServiceListFromParent[0].ServiceID && ps.DepartmentID == ServiceListFromParent[0].DepartmentID
                                   && ps.IsSpecilizationGroup == ServiceListFromParent[0].IsSpecilizationGroup
                                   && ps.ProcessID == ServiceListFromParent[0].ProcessID
                                   select new clsPackageServiceDetailsVO
                                   {
                                       ServiceID = ps.ServiceID,
                                       ProcessID = ps.ProcessID,
                                       DepartmentID = ps.DepartmentID,
                                       IsSpecilizationGroup = ps.IsSpecilizationGroup
                                   };

                    if (itemExst.ToList().Count > 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW12 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Same Service with selected Procedure already added.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW12.Show();
                        dgCalender.Focus();
                        result = false;
                        return result;
                    }

                }
            }

            if (dgCalender.ItemsSource == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "You Can not save without Followup Calendar.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                dgCalender.Focus();
                result = false;
                return result;

            }

            if (dgCalender.ItemsSource != null)
            {
                if (((ObservableCollection<PackageList>)dgCalender.ItemsSource).Count == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You Can not save without Followup Calendar.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    dgCalender.Focus();
                    result = false;
                    return result;

                }
            }

            //if (rdbPercentage.IsChecked == true)
            //{
            //    if (txtRatePercentage.Text != null)
            //    {
            //        if (Convert.ToDouble(txtRatePercentage.Text) > 100)
            //        {
            //            txtRatePercentage.SetValidation("Please Select Applicable Relations");
            //            txtRatePercentage.RaiseValidationError();
            //            txtRatePercentage.Focus();
            //        }
            //        else
            //            txtRatePercentage.ClearValidationError();
            //    }
            //}

            if (result == true)
            {
                ObservableCollection<PackageList> PackageServiceList = new ObservableCollection<PackageList>();
                PackageServiceList = ((ObservableCollection<PackageList>)dgCalender.ItemsSource);

                bool selectFollowup = false;

                foreach (var item2 in PackageServiceList)
                {
                    if (dgCalender.ItemsSource != null && ((ObservableCollection<PackageList>)dgCalender.ItemsSource).Count > 0)  //&& chkFollowupNotRequired.IsChecked == false
                    {
                        foreach (var item in dgCalender.Columns)
                        {
                            if (((CheckBox)item.GetCellContent(item2)).IsChecked.Value == true && selectFollowup == false)
                            {
                                selectFollowup = true;
                                break;
                            }
                        }
                    }
                    else if (chkFollowupNotRequired.IsChecked == true)
                    {
                        selectFollowup = true;
                        break;
                    }
                }

                if (selectFollowup == false && chkAdjustableHead.IsChecked == false)     //&& chkAdjustableHead.IsChecked == false
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                       new MessageBoxControl.MessageBoxChildWindow("", "You Can not save without selecting Followup Calendar.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    dgCalender.Focus();
                    result = false;
                    return result;
                }

                long FollowCnt = 0;

                foreach (var item3 in PackageServiceList)
                {
                    if (dgCalender.ItemsSource != null && ((ObservableCollection<PackageList>)dgCalender.ItemsSource).Count > 0 && chkUnlimitedQuantity.IsChecked == false) //&& chkFollowupNotRequired.IsChecked == false 
                    {
                        foreach (var item in dgCalender.Columns)
                        {
                            if (((CheckBox)item.GetCellContent(item3)).IsChecked.Value == true && item3.IsSpecilizationGroup == false)
                            {
                                FollowCnt++;
                            }
                        }
                    }
                }

                if (dgCalender.ItemsSource != null && ((ObservableCollection<PackageList>)dgCalender.ItemsSource).Count > 0 && chkUnlimitedQuantity.IsChecked == false) //&& chkFollowupNotRequired.IsChecked == false 
                {
                    if (!String.IsNullOrEmpty(txtQuantity.Text) && txtQuantity.Text != "0" && txtQuantity.Text != "")
                    {
                        if (PackageServiceList != null && PackageServiceList.Count > 0 && PackageServiceList[0].IsSpecilizationGroup == false && FollowCnt != Convert.ToInt64(txtQuantity.Text))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                       new MessageBoxControl.MessageBoxChildWindow("", "Followup Calendar Checked Items Quantity should be same as entered Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            dgCalender.Focus();
                            result = false;
                            return result;
                        }

                        if (PackageServiceList != null && PackageServiceList.Count > 0 && PackageServiceList[0].IsSpecilizationGroup == false && PrevQty > Convert.ToInt64(txtQuantity.Text) && this.IsFreeze == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                       new MessageBoxControl.MessageBoxChildWindow("", "You can't enter Quantity less than Previous Quantity= " + PrevQty.ToString() + " .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            dgCalender.Focus();
                            result = false;
                            return result;
                        }

                    }

                }

            }

            if (result == true)
            {
                ObservableCollection<clsPackageServiceConditionsVO> PackageConditionServiceList = new ObservableCollection<clsPackageServiceConditionsVO>();
                PackageConditionServiceList = ((ObservableCollection<clsPackageServiceConditionsVO>)dgANDORGrid.ItemsSource).DeepCopy();
                //dgANDORGrid.ItemsSource = ConditionANDOR;

                //if (PackageConditionServiceList.Count > 1)
                //{
                for (int i = 0; i < PackageConditionServiceList.Count; i++)
                {
                    if (PackageConditionServiceList.Count == 1 && i == 0 && PackageConditionServiceList[i].SelectedCondition != null && PackageConditionServiceList[i].SelectedCondition.ID != 0 && PackageConditionServiceList[i].SelectedService != null && PackageConditionServiceList[i].SelectedService.ID != 0)
                    {
                        if (PackageConditionServiceList[i].Quantity == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                   new MessageBoxControl.MessageBoxChildWindow("", "Please Select Quantity for Conditional Services.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            dgANDORGrid.Focus();
                            result = false;
                            return result;
                        }
                        //else if (PackageConditionServiceList[i].Discount == 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                           new MessageBoxControl.MessageBoxChildWindow("", "Please Select Discount for Conditional Services.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        //    msgW1.Show();
                        //    dgANDORGrid.Focus();
                        //    result = false;
                        //    return result;
                        //}

                        if (ServiceListFromParent[0].IsSpecilizationGroup == false && chkUnlimitedQuantity.IsChecked == false && PackageConditionServiceList[i].SelectedCondition.ID == 2 && !string.IsNullOrEmpty(txtQuantity.Text) && CIMS.Extensions.IsItNumber(txtQuantity.Text) == true && PackageConditionServiceList[i].Quantity != Convert.ToDouble(txtQuantity.Text))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Quantity for Services with OR condition should be same as Main Service.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            dgANDORGrid.Focus();
                            result = false;
                            return result;
                        }

                    }

                    if (PackageConditionServiceList.Count > 1)
                    {
                        if (PackageConditionServiceList[i].SelectedCondition != null && PackageConditionServiceList[i].SelectedCondition.ID == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                   new MessageBoxControl.MessageBoxChildWindow("", "Please Select Condition for Conditional Services.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            dgANDORGrid.Focus();
                            result = false;
                            return result;
                        }
                        else if (PackageConditionServiceList[i].SelectedService != null && PackageConditionServiceList[i].SelectedService.ID == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                   new MessageBoxControl.MessageBoxChildWindow("", "Please Select Service for Conditional Services.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            dgANDORGrid.Focus();
                            result = false;
                            return result;
                        }
                        else if (PackageConditionServiceList[i].Quantity == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                   new MessageBoxControl.MessageBoxChildWindow("", "Please Select Quantity for Conditional Services.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            dgANDORGrid.Focus();
                            result = false;
                            return result;
                        }
                        //else if (PackageConditionServiceList[i].Discount == 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                           new MessageBoxControl.MessageBoxChildWindow("", "Please Select Discount for Conditional Services.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        //    msgW1.Show();
                        //    dgANDORGrid.Focus();
                        //    result = false;
                        //    return result;
                        //}

                        if (ServiceListFromParent[0].IsSpecilizationGroup == false && chkUnlimitedQuantity.IsChecked == false && PackageConditionServiceList[i].SelectedCondition.ID == 2 && !string.IsNullOrEmpty(txtQuantity.Text) && CIMS.Extensions.IsItNumber(txtQuantity.Text) == true && PackageConditionServiceList[i].Quantity != Convert.ToDouble(txtQuantity.Text))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Quantity for Services with OR condition should be same as Main Service.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            dgANDORGrid.Focus();
                            result = false;
                            return result;
                        }

                    }

                }

                //}
            }



            return result;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void dgCalender_LoadingRow(object sender, DataGridRowEventArgs e)
        //{

        //}

        //private void dgCalender_CellEditEnded_1(object sender, DataGridCellEditEndedEventArgs e)
        //{

        //}

        private void cmdAddService_Click(object sender, RoutedEventArgs e)
        {
            //clsPackageServiceDetailsVO obj = new clsPackageServiceDetailsVO();
            //obj.Servicelst = objList;
            //ConditionOR.Add(obj);
            //ANDGrid.ItemsSource = ConditionOR;

            clsPackageServiceConditionsVO obj = new clsPackageServiceConditionsVO();
            //**Added by Ashish Z. for remove the selected services
            //foreach (var item in ConditionANDOR.ToList())
            //{
            //    foreach(var item1 in objList.ToList())
            //    {
            //        if (item.SelectedService.ID == item1.ID)
            //        {
            //            objList.Remove(item1);
            //        }
            //    }
            //}
            //**
            obj.ServiceList = objList;
            obj.ConditionList = ANDORList;
            ConditionANDOR.Add(obj);
            dgANDORGrid.ItemsSource = ConditionANDOR;

            dgANDORGrid.UpdateLayout();
            dgANDORGrid.Focus();
            dgANDORGrid.SelectedIndex = ConditionANDOR.Count - 1;
        }

        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (dgANDORGrid.SelectedItem != null && ConditionANDOR != null && ConditionANDOR.Count > 1)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Service ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        //ANDORList.Where(objCondition => objCondition.ID == item4.ConditionTypeID).ToList().FirstOrDefault();

                        ConditionANDORDelete.Add(ConditionANDOR.Where(objCond => objCond.ServiceID == ((clsPackageServiceConditionsVO)dgANDORGrid.SelectedItem).ServiceID).ToList().First());

                        ConditionANDOR.RemoveAt(dgANDORGrid.SelectedIndex);


                        dgANDORGrid.Focus();
                        dgANDORGrid.UpdateLayout();
                        dgANDORGrid.SelectedIndex = ConditionANDOR.Count - 1;
                    }
                };
                msgWD.Show();
            }
        }

        private void textQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textQuantity_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textDiscount_KeyDown(object sender, KeyEventArgs e)
        {

        }



        private void chkUnlimitedQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (chkUnlimitedQuantity.IsChecked == true)  //&& ServiceListFromParent  != null && ServiceListFromParent[0].IsSpecilizationGroup == true
            {
                txtQuantity.IsReadOnly = true;
                txtQuantity.Text = "0";
                chkDiscountOnQuantity.IsChecked = false;
                chkDiscountOnQuantity.IsEnabled = false;
                dgANDORGrid.IsEnabled = false;
            }
            else
            {

                if (ServiceListFromParent != null && ServiceListFromParent[0].IsSpecilizationGroup == false)
                {
                    txtQuantity.IsReadOnly = false;
                    chkDiscountOnQuantity.IsEnabled = true;
                    dgANDORGrid.IsEnabled = true;
                }
            }

            if (PatientList3 != null && PatientList3.Count > 0)
                dgCalender.SelectedItem = PatientList3[0];

            dgCalender.UpdateLayout();
            dgCalender.Focus();

            if (dgCalender.SelectedItem != null)
            {
                ((PackageList)dgCalender.SelectedItem).Infinite = Convert.ToBoolean(((ToggleButton)((CheckBox)sender)).IsChecked);
                //((ToggleButton)((CheckBox)sender)).IsChecked

                if (((PackageList)dgCalender.SelectedItem).Infinite == true)  //clsPackageServiceDetailsVO
                {
                    foreach (var item in dgCalender.Columns)
                    {
                        //if (item.Header.ToString() != "Service Name" && item.Header.ToString() != "Rate" && item.Header.ToString() != "Amount" && item.Header.ToString() != "Discount" && item.Header.ToString() != "Applicable To" && item.Header.ToString() != "Unlimited Quantity" && item.Header.ToString() != "Quantity" && item.Header.ToString() != "Free At FollowUp" && item.Header.ToString() != "Delete")
                        //{
                        DataGridColumn column = dgCalender.Columns[item.DisplayIndex];
                        FrameworkElement fe = column.GetCellContent(((PackageList)dgCalender.SelectedItem));
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        if (result != null)
                        {
                            CheckBox block = fe as CheckBox;
                            if (block != null)
                            {
                                block.IsChecked = true;

                                ((PackageList)dgCalender.SelectedItem).Quantity = 0.0;
                            }
                        }
                        //}
                    }

                }
                else
                {
                    foreach (var item in dgCalender.Columns)
                    {
                        //if (item.Header.ToString() != "Service Name" && item.Header.ToString() != "Rate" && item.Header.ToString() != "Amount" && item.Header.ToString() != "Discount" && item.Header.ToString() != "Applicable To" && item.Header.ToString() != "Unlimited Quantity" && item.Header.ToString() != "Quantity" && item.Header.ToString() != "Free At FollowUp" && item.Header.ToString() != "Delete")
                        //{
                        DataGridColumn column = dgCalender.Columns[item.DisplayIndex];
                        FrameworkElement fe = column.GetCellContent(((PackageList)dgCalender.SelectedItem));  //clsPackageServiceDetailsVO
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        if (result != null)
                        {
                            CheckBox block = fe as CheckBox;
                            if (block != null)
                            {
                                block.IsChecked = false;
                            }
                        }
                        //}
                    }
                }

                dgCalender.Focus();
                dgCalender.UpdateLayout();
            }

        }

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = child.Parent;
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;
        }

        private void cmdAddORService_Click(object sender, RoutedEventArgs e)
        {
            //clsPackageServiceDetailsVO obj = new clsPackageServiceDetailsVO();
            //obj.Serviceobj = objList;
            //ConditionAND.Add(obj);
            //ORGrid.ItemsSource = ConditionAND;
        }

        private void cmbANDORService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble())
            {
                if (textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtDiscount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDiscount.Text))
            {
                if (CIMS.Extensions.IsValueDouble(txtDiscount.Text) == true)
                {
                    if (Convert.ToDecimal(txtDiscount.Text) > 100)
                    {
                        txtDiscount.SetValidation("Discount should be upto 100");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                            new MessageBoxControl.MessageBoxChildWindow("", "Discount should be upto 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        txtDiscount.Text = "0";    //PrevServiceRate.ToString();
                        return;
                    }
                }
            }
        }

        private void txtQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtQuantity.Text))
            //{
            //    if (CIMS.Extensions.IsItDecimalNo(txtQuantity.Text) == true)
            //    {
            //        txtQuantity.SetValidation("Quantity should not be in decimal");
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                                            new MessageBoxControl.MessageBoxChildWindow("", "Quantity should not be in decimal", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //        msgW1.Show();
            //        txtQuantity.Text = "0";    //PrevServiceRate.ToString();
            //        return;
            //    }
            //}
        }

        private void cmbCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dgANDORGrid.ItemsSource != null && ((ObservableCollection<clsPackageServiceConditionsVO>)dgANDORGrid.ItemsSource).Count > 0 && dgANDORGrid.SelectedItem != null)
            //{
            //    if (((AutoCompleteComboBox)sender).SelectedItem != null)
            //    {
            //        try
            //        {
            //            //foreach (var item2 in ConditionANDOR)
            //            //{

            //            clsPackageServiceConditionsVO item = new clsPackageServiceConditionsVO();
            //            item = ((clsPackageServiceConditionsVO)(dgANDORGrid.SelectedItem));

            //            //if (item2 == item)
            //            //{
            //            if (((MasterListItem)((AutoCompleteComboBox)sender).SelectedItem).ID == 2)
            //            {
            //                if (item.Quantity == 0)
            //                {
            //                    if (!string.IsNullOrEmpty(txtQuantity.Text) && CIMS.Extensions.IsItNumber(txtQuantity.Text) == true)
            //                    {
            //                        item.Quantity = Convert.ToDouble(txtQuantity.Text);

            //                        //item2.Quantity = Convert.ToDouble(txtQuantity.Text);

            //                        DataGridColumn column = dgANDORGrid.Columns[2];
            //                        FrameworkElement fe = column.GetCellContent(item);
            //                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));

            //                        if (result != null)
            //                        {
            //                            DataGridCell cell = (DataGridCell)result;
            //                            //cell.IsEnabled = false;

            //                        }

            //                        //dgANDORGrid.ItemsSource = null;
            //                        //dgANDORGrid.ItemsSource = ConditionANDOR;

            //                        dgANDORGrid.Focus();
            //                        dgANDORGrid.UpdateLayout();

            //                    }
            //                }
            //            }
            //            else
            //            {
            //                DataGridColumn column = dgANDORGrid.Columns[2];
            //                FrameworkElement fe = column.GetCellContent(item);
            //                FrameworkElement result = GetParent(fe, typeof(DataGridCell));

            //                if (result != null)
            //                {
            //                    DataGridCell cell = (DataGridCell)result;
            //                    //cell.IsEnabled = true;
            //                }

            //                //dgANDORGrid.ItemsSource = null;
            //                //dgANDORGrid.ItemsSource = ConditionANDOR;

            //                dgANDORGrid.Focus();
            //                dgANDORGrid.UpdateLayout();
            //            }

            //            //}

            //            //}

            //            //double Days = Convert.ToDouble(((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).Days);
            //            //((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).Quantity = (((FrequencyMaster)((AutoCompleteComboBox)sender).SelectedItem).Quantity * Days);
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    }
            //}

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }



        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //FilRelationsCombobox(sender);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            FilRelationsCombobox(sender);
        }

        private void FilRelationsCombobox(object sender)
        {
            try
            {
                objSpecializationList = ((List<MasterListItem>)(cmbPackageRelations.ItemsSource)).ToList();

                if (((CheckBox)sender).IsChecked == true)
                {

                    foreach (MasterListItem item in objSpecializationList)
                    {
                        if (item.ID == 0 && item.isChecked == true)
                        {
                            foreach (MasterListItem it in objSpecializationList)
                                it.isChecked = true;
                        }
                    }
                }
                else
                {
                    foreach (MasterListItem item in objSpecializationList)
                    {
                        if (item.ID == 0 && item.isChecked == false)
                        {
                            foreach (MasterListItem it in objSpecializationList)
                                it.isChecked = false;
                        }
                    }
                }
                cmbPackageRelations.ItemsSource = null;
                cmbPackageRelations.ItemsSource = objSpecializationList;
            }
            catch (Exception ex)
            { }
        }


        private void cmbPackageRelations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Code Commented Becouse the Set AllRelation Combo Box is Collapsed 
            // So the below code is Commented by CDS 

            #region Code Commented Becouse the Set AllRelation Combo Box is Collapsed So the below code is Commented by CDS
            if (cmbPackageRelations.SelectedItem != null && ((MasterListItem)cmbPackageRelations.SelectedItem).ID >= 0 && ((MasterListItem)cmbPackageRelations.SelectedItem).Status == true)
            {
                foreach (var item in PackageRelationsList)
                {
                    if (item.ID == 0)
                    {
                        foreach (var item1 in PackageRelationsList)
                        {
                            if (item1.Status == true)
                                item1.Status = true;
                            else
                                item1.Status = false;
                        }
                    }
                }
            }
            #endregion
        }



        private void cmbPackageRelations_Loaded(object sender, RoutedEventArgs e)
        {
            //objSpecializationList =new  ((List<MasterListItem>)(cmbPackageRelations.ItemsSource)).ToList();
            //cmbPackageRelations.ItemsSource = null;
            //cmbPackageRelations.ItemsSource = objSpecializationList;
            cmbPackageRelations.Text = "";
        }

        private List<DataGridRow> AllDataGridRow = new List<DataGridRow>();
        private object sender;

        private void dgCalender_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            this.AllDataGridRow.Add(e.Row);
            foreach (var item in dgCalender.Columns)
            {
                DataGridColumn column = dgCalender.Columns[item.DisplayIndex];
                FrameworkElement fe = column.GetCellContent(e.Row);
                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                if (result != null)
                {
                    CheckBox block = fe as CheckBox;
                    if (block != null)
                    {
                        block.Click += new RoutedEventHandler(block_Click);
                    }
                }
            }
        }



        void block_Click(object sender, RoutedEventArgs e)
        {
            if (chkUnlimitedQuantity.IsChecked == false)
            {
                Double qty = 0;

                foreach (var itemCol in dgCalender.Columns)
                {

                    DataGridColumn column = dgCalender.Columns[itemCol.DisplayIndex];
                    FrameworkElement fe = column.GetCellContent(((PackageList)dgCalender.SelectedItem));
                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                    if (result != null)
                    {
                        CheckBox block = fe as CheckBox;
                        if (block != null)
                        {

                            if (block.IsChecked == true)
                            {
                                qty = qty + 1;
                            }
                            else
                            {

                                //qty = qty - 1;
                            }

                            //((PackageList)dgCalender.SelectedItem).Quantity = 0.0;
                        }
                    }

                }
                txtQuantity.Text = qty.ToString();
            }
            dgCalender.UpdateLayout();
            dgCalender.Focus();
        }

        private void chkAllRelations_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                List<MasterListItem> objSpecializationList = ((List<MasterListItem>)(cmbPackageRelations.ItemsSource)).ToList();
                foreach (MasterListItem item in objSpecializationList)
                {
                    item.Status = true;
                }


                cmbPackageRelations.ItemsSource = null;
                cmbPackageRelations.ItemsSource = objSpecializationList;
            }
            else
            {
                List<MasterListItem> objSpecializationList = ((List<MasterListItem>)(cmbPackageRelations.ItemsSource)).ToList();
                foreach (MasterListItem item in objSpecializationList)
                {
                    item.Status = false;
                }

                cmbPackageRelations.ItemsSource = null;
                cmbPackageRelations.ItemsSource = objSpecializationList;

            }
        }

        private void txtAgeLimit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.IsItDecimal())
            {
                if (textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtAgeLimit_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }


        private void chkAdjustableHead_Checked(object sender, RoutedEventArgs e)
        {
            chkAllRelations.IsChecked = true;
            //chkUnlimitedQuantity.IsChecked = true;
            //chkUnlimitedQuantity_Click(null, null);


            txtAgeLimit.IsEnabled = false;
            txtDiscount.IsEnabled = false;
            chkDiscountOnQuantity.IsEnabled = false;
            cmbPackageRelations.IsEnabled = false;
            chkAllRelations.IsEnabled = false;
            txtQuantity.IsEnabled = false;
            chkUnlimitedQuantity.IsEnabled = false;

            rdbFixed.IsEnabled = false;
            rdbPercentage.IsEnabled = false;
            rdbDoctorShare.IsEnabled = false;
            //RateLimitTB.IsEnabled=false;
            txtRate.IsEnabled = false;
            txtRatePercentage.IsEnabled = false;
            Followup.IsEnabled = false;
            conditionalANDOR.IsEnabled = false;

            chkDiscountOnQuantity.IsEnabled = false;
            chkUnlimitedQuantity.IsEnabled = false;

            if (GenderList != null)
            {
                cmbGender.SelectedItem = GenderList.FirstOrDefault(p => p.ID == 3);
            }
            cmbGender.IsEnabled = false;
            chkIncludeAdjustableHead.IsEnabled = false;
            if ((PackageList)dgCalender.SelectedItem != null)
            {
                Double qty = 0;
                int i = 0;
                foreach (var itemCol in dgCalender.Columns)
                {
                    if (i == 0)
                    {
                        i++;
                        DataGridColumn column = dgCalender.Columns[itemCol.DisplayIndex];
                        FrameworkElement fe = column.GetCellContent(((PackageList)dgCalender.SelectedItem));
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        if (result != null)
                        {
                            CheckBox block = fe as CheckBox;
                            if (block != null)
                            {
                                block.IsChecked = true;
                                qty = qty + 1;
                            }
                        }
                    }
                }
                txtQuantity.Text = qty.ToString();
            }
            dgCalender.UpdateLayout();
            dgCalender.Focus();

            #region Package New Changes for Procedure Added on 18042018

            stkAdjustableHead.Visibility = Visibility.Visible;
            rdbClinicalHead.IsChecked = true;
            rdbPharmacyHead.IsChecked = false;

            #endregion
        }

        private void chkAdjustableHead_Unchecked(object sender, RoutedEventArgs e)
        {
            chkAllRelations.IsChecked = false;
            //chkUnlimitedQuantity.IsChecked = false;
            //chkUnlimitedQuantity_Click(null, null);
            cmbGender.IsEnabled = true;
            txtAgeLimit.IsEnabled = true;
            txtDiscount.IsEnabled = true;
            chkDiscountOnQuantity.IsEnabled = true;
            cmbPackageRelations.IsEnabled = true;
            chkAllRelations.IsEnabled = true;
            txtQuantity.IsEnabled = true;
            chkUnlimitedQuantity.IsEnabled = true;
            rdbFixed.IsEnabled = true;
            rdbPercentage.IsEnabled = true;
            rdbDoctorShare.IsEnabled = true;
            //RateLimitTB.IsEnabled=false;
            txtRate.IsEnabled = true;
            txtRatePercentage.IsEnabled = true;
            Followup.IsEnabled = true;
            conditionalANDOR.IsEnabled = true;
            chkDiscountOnQuantity.IsEnabled = true;
            chkUnlimitedQuantity.IsEnabled = true;
            if (GenderList != null)
            {
                cmbGender.SelectedItem = GenderList.FirstOrDefault(p => p.ID == 0);
            }
            chkIncludeAdjustableHead.IsEnabled = true;

            #region Package New Changes for Procedure Added on 18042018

            stkAdjustableHead.Visibility = Visibility.Collapsed;
            rdbClinicalHead.IsChecked = true;
            rdbPharmacyHead.IsChecked = false;

            #endregion
        }



        private void rdbFixed_Checked(object sender, RoutedEventArgs e)
        {
            //txtRate.Visibility = Visibility.Visible;

            //txtRatePercentage.Visibility = Visibility.Collapsed;
            //rdbDoctorShare.Visibility = Visibility.Collapsed;
        }

        private void rdbPercentage_Checked(object sender, RoutedEventArgs e)
        {
            //txtRate.Visibility = Visibility.Collapsed;

            //txtRatePercentage.Visibility = Visibility.Visible;
            //rdbDoctorShare.Visibility = Visibility.Visible;
        }

        private void rdbFixed_Click(object sender, RoutedEventArgs e)
        {
            if (rdbFixed.IsChecked == true)
            {
                RateLimitTB.Visibility = Visibility.Visible;
                txtRate.Visibility = Visibility.Visible;

                txtRatePercentage.Visibility = Visibility.Collapsed;
                //rdbDoctorShare.Visibility = Visibility.Collapsed;
                if (!string.IsNullOrEmpty(txtRatePercentage.Text.Trim()))
                {
                    txtRatePercentage.Text = string.Empty;
                }
            }
        }

        private void rdbPercentage_Click(object sender, RoutedEventArgs e)
        {
            if (rdbPercentage.IsChecked == true)
            {
                RateLimitTB.Visibility = Visibility.Visible;
                txtRate.Visibility = Visibility.Collapsed;

                txtRatePercentage.Visibility = Visibility.Visible;
                //rdbDoctorShare.Visibility = Visibility.Visible;
            }
        }

        private void rdbDoctorShare_Click(object sender, RoutedEventArgs e)
        {
            if (rdbDoctorShare.IsChecked == true)
            {
                RateLimitTB.Visibility = Visibility.Collapsed;
                txtRate.Visibility = Visibility.Collapsed;
                txtRatePercentage.Visibility = Visibility.Collapsed;
                if (!string.IsNullOrEmpty(txtRatePercentage.Text.Trim()))
                {
                    txtRatePercentage.Text = string.Empty;
                }
            }
        }

        private void txtRatePercentage_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtRatePercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(((TextBox)sender).Text == "100") && !((TextBox)sender).Text.IsValidTwoDigitWithTwoDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        //Package New Changes for Procedure Added on 17042018
        private void cmbProcedure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbProcedure.SelectedItem != null && ((MasterListItem)cmbProcedure.SelectedItem).ID > 0 && ServiceListFromParent != null && ServiceListFromParent.Count > 0)
            {
                foreach (clsPackageServiceDetailsVO itemPS in ServiceListFromParent)
                {
                    itemPS.ProcessID = ((MasterListItem)cmbProcedure.SelectedItem).ID;
                }
            }
        }
    }
}