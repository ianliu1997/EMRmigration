

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Radiology;
using CIMS;
using PalashDynamics.Pharmacy;
using System.Collections.ObjectModel;
using PalashDynamics.Pharmacy.ItemSearch;
using PalashDynamics.Radiology.ItemSearch;
using System.Windows.Browser;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.Radiology
{
    public partial class RadiologyTechnicianEntry : UserControl, IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder == null)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Radiology work order is not selected.\nPlease Select a work order then click on Technician Entry.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        IsPatientExist = false;
                        break;
                    }
                    else if (((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.TestID == 0 && ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.ServiceID == 0)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please Select the test", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        IsPatientExist = false;
                    }
                    else if (((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.ServiceID == 0)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Test is not assigned to the selected service.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        IsPatientExist = false;
                        //break;
                    }
                    else if (((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.IsCancelled == true)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Test is not Canceled.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        IsPatientExist = false;
                        //break;
                    }

                    else if (((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder != null && ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.PatientID != 0 && ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.IsTechnicianEntryFinalized == true)
                    {
                        //MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Technician Entry already done.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        //msgbox.Show();
                        //IsPatientExist = false;
                        cmdSave.IsEnabled = false;
                        FillTechnicianEntryList();
                        IsPatientExist = true;
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                        mElement.Text = " : " + ((clsRadOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder).PatientName;

                    }
                    else if (((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder != null && ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.PatientID != 0 && ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.TestID == 0)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Select the test for technician entry", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbox.Show();
                        IsPatientExist = false;
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    }

                    else if (((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder != null && ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.PatientID != 0 && ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.IsTechnicianEntryFinalized == false)
                    {
                        // FillItemCategory();
                        cmdSave.IsEnabled = true;
                        FillTechnicianEntryList();
                        IsPatientExist = true;
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                        mElement.Text = " : " + ((clsRadOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder).PatientName;

                    }
                    break;
            }
        }

        #endregion
        public RadiologyTechnicianEntry()
        {
            InitializeComponent();
        }

        #region Variable Declaration
        public ObservableCollection<clsRadItemDetailMasterVO> ItemList { get; set; }
        public ObservableCollection<clsRadItemDetailMasterVO> OldItemList { get; set; }
        public ObservableCollection<clsRadItemDetailMasterVO> OldItemList1 { get; set; }
        long TemplateID { get; set; }
        long OrderID { get; set; }
        int ClickedFlag = 0;
        bool IsPageLoded = false;
        long ResultEntryID { get; set; }
        bool IsUpdate = false;

        clsRadOrderBookingVO SelectedOrder;
        bool IsPatientExist = false;


        #endregion


        private void ResultEntry_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder = null;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(new New_RadiologyWorkOrderGeneration());
                    UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleHeader");
                    mElement1.Text = "Radiology Work Order";
                }
                else
                {
                    if (!IsPageLoded)
                    {

                        this.DataContext = new clsRadResultEntryVO()
                        {
                            StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologyStoreID
                        };
                        ItemList = new ObservableCollection<clsRadItemDetailMasterVO>();

                        FillFilm();
                        FillStore();
                      // FillRefernceDoctor();
                       FillDoctor();
                        FillTest();
                    }
                    IsPageLoded = true;
                    cmbFilm.Focus();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        List<MasterListItem> ItemCategoryList = new List<MasterListItem>();

        private void FillItemCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ItemCategory;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        ItemCategoryList = objList;
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

        private void FillTechnicianEntryList()
        {
            try
            {
                clsGetRadTechnicianEntryListBizActionVO BizAction = new clsGetRadTechnicianEntryListBizActionVO();
                BizAction.BookingList = new List<clsRadOrderBookingVO>();
                BizAction.FromDate = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.Date.Value.Date.Date;
                BizAction.ToDate = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.Date.Value.Date.Date.AddDays(1);
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.PatientUnitId;
                BizAction.RadOrderID = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.ID;
                BizAction.OrderDetailID = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.OrderDetailID;
                BizAction.BillID = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.BillID;
                BizAction.ChargeID = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.ChargeID;
                BizAction.ServiceID = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.ServiceID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetRadTechnicianEntryListBizActionVO)arg.Result).BookingList != null)
                        {
                            if (((clsGetRadTechnicianEntryListBizActionVO)arg.Result).BookingList.Count > 0)
                            {
                                SelectedOrder = ((clsGetRadTechnicianEntryListBizActionVO)arg.Result).BookingList[0];
                            }
                            else
                            {
                                SelectedOrder = new clsRadOrderBookingVO();
                            }
                            if (SelectedOrder != null)
                            {
                                if (SelectedOrder.TestID > 0)
                                {
                                    if (SelectedOrder.IsTechnicianEntry == false && SelectedOrder.IsTechnicianEntryFinalized == false)
                                    {
                                        txtReferenceDoctor.Text = SelectedOrder.ReferredDoctor;
                                        cmbTestName.SelectedValue = SelectedOrder.TestID;
                                        FillItemList();
                                        IsUpdate = false;
                                    }
                                    else
                                    {
                                        if (SelectedOrder.IsTechnicianEntry == true)
                                        {
                                            cmbTestName.SelectedValue = SelectedOrder.TestID;
                                            txtReferenceDoctor.Text = SelectedOrder.ReferredDoctor;
                                            FillTest();
                                            GetResultEntry();
                                            IsUpdate = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
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

        private void FillItemList()
        {
            try
            {
                clsGetRadTemplateAndItemByTestIDBizActionVO BizAction = new clsGetRadTemplateAndItemByTestIDBizActionVO();
                BizAction.ItemList = new List<clsRadItemDetailMasterVO>();
                BizAction.TestID = SelectedOrder.TestID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetRadTemplateAndItemByTestIDBizActionVO)arg.Result).ItemList != null)
                        {
                            ItemList = new ObservableCollection<clsRadItemDetailMasterVO>();
                            List<clsRadItemDetailMasterVO> ObjItem;
                            ObjItem = ((clsGetRadTemplateAndItemByTestIDBizActionVO)arg.Result).ItemList; ;
                            foreach (clsRadItemDetailMasterVO item4 in ObjItem)
                            {
                                item4.BatchCode = String.Empty;
                                item4.ExpiryDate = null;
                                ItemList.Add(item4);
                            }
                            dgItemDetailsList.ItemsSource = null;
                            dgItemDetailsList.ItemsSource = ItemList;
                            OldItemList1 = ItemList;
                            OldItemList = OldItemList1.DeepCopy();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region Fillcombobox


        private void FillFilm()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_RadFilmSize;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbFilm.ItemsSource = null;
                    cmbFilm.ItemsSource = objList;
                    cmbFilm.SelectedItem = objList[0];


                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillTest()
        {
            clsFillTestComboBoxBizActionVO BizAction = new clsFillTestComboBoxBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsFillTestComboBoxBizActionVO)arg.Result).MasterList);

                    cmbTestName.ItemsSource = null;
                    cmbTestName.ItemsSource = objList;
                    if (SelectedOrder != null && SelectedOrder.TestID != 0)
                    {
                        cmbTestName.SelectedValue = SelectedOrder.TestID;
                    }
                    else
                    {
                        cmbTestName.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.TestID;
                    }




                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillRefernceDoctor()
        {
            clsGetDoctorMasterListBizActionVO BizAction = new clsGetDoctorMasterListBizActionVO();
            BizAction.ComboList = new List<clsComboMasterBizActionVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {

                if (e.Error == null && e.Result != null)
                {
                    if (((clsGetDoctorMasterListBizActionVO)e.Result).ComboList != null)
                    {

                        clsGetDoctorMasterListBizActionVO result = (clsGetDoctorMasterListBizActionVO)e.Result;
                        List<MasterListItem> objList = new List<MasterListItem>();

                        if (result.ComboList != null)
                        {
                            foreach (var item in result.ComboList)
                            {
                                MasterListItem Objmaster = new MasterListItem();
                                Objmaster.ID = item.ID;
                                Objmaster.Description = item.Value;
                                objList.Add(Objmaster);

                            }
                        }
                        txtReferenceDoctor.ItemsSource = null;
                        txtReferenceDoctor.ItemsSource = objList;

                        if (SelectedOrder != null && SelectedOrder.ReferredDoctor != null)
                        {
                            txtReferenceDoctor.Text = SelectedOrder.ReferredDoctor;
                        }
                        else
                        {
                            txtReferenceDoctor.Text = ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.ReferredDoctor;
                        }
                    }
                }


            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillStore()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();
            //Added By Somewhat
            long ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (ClinicID > 0)
                BizAction.Parent = new KeyValue { Key = ClinicID, Value = "ClinicId" };
            //End


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;

                    if (this.DataContext != null)
                    {
                        //if (((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId == 2)
                        //{
                        //    cmbStore.SelectedItem = objList[1];
                        //}
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologyStoreID != null)
                        {
                            cmbStore.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologyStoreID;
                        }
                        else
                        {
                            cmbStore.SelectedItem = objList[0];
                        }
                        // cmbStore.SelectedValue = ((clsRadResultEntryVO)this.DataContext).StoreID;
                    }
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void txtReferenceDoctor_GotFocus(object sender, RoutedEventArgs e)
        {
           // FillRefernceDoctor();
            FillDoctor();
        }

        #endregion

        #region Get Result Entry details for Update
        private void GetResultEntry()
        {
            clsGetRadResultEntryBizActionVO BizAction = new clsGetRadResultEntryBizActionVO();
            BizAction.ID = SelectedOrder.ID;
            BizAction.DetailID = SelectedOrder.OrderDetailID;
            BizAction.UnitID = SelectedOrder.UnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetRadResultEntryBizActionVO)arg.Result).ResultDetails != null)
                    {
                        clsRadResultEntryVO ObjDetails;
                        ObjDetails = ((clsGetRadResultEntryBizActionVO)arg.Result).ResultDetails;
                        if (ObjDetails != null)
                        {
                            this.DataContext = ObjDetails;
                            cmbFilm.SelectedValue = ObjDetails.FilmID;
                        }
                        if (ObjDetails.TestItemList != null)
                        {
                            List<clsRadItemDetailMasterVO> ObjItem;
                            ObjItem = ((clsGetRadResultEntryBizActionVO)arg.Result).TestItemList; ;
                            foreach (var item4 in ObjItem)
                            {
                                cmbStore.SelectedValue = item4.StoreID;
                                if (item4.IsFinalized ==true)
                                    item4.BatchesRequired = false;
                                //else
                                //    item4.BatchesRequired = true;
                                ItemList.Add(item4);
                            }
                            dgItemDetailsList.ItemsSource = null;
                            dgItemDetailsList.ItemsSource = ItemList;
                            if (ObjItem[0].IsFinalized == true)
                            {
                                chkFinalize.IsEnabled = false;
                                chkFinalize.IsChecked = true;
                            }
                            else
                            {
                                chkFinalize.IsEnabled = true;
                                chkFinalize.IsChecked = false;
                            }

                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        #region Save and Update data
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            Flag = false;
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                bool SaveTemplate = true;

                List<clsRadItemDetailMasterVO> RadItemList = new List<clsRadItemDetailMasterVO>();
                RadItemList = ((ObservableCollection<clsRadItemDetailMasterVO>)dgItemDetailsList.ItemsSource).ToList();
                int item = (from r in RadItemList
                            where r.WastageQantity == 0
                            select r).Count();


                SaveTemplate = CheckValidation();

                int itemFlim = (from r in ItemList
                              //  where r.ItemCategoryID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FilmCategory
                                select r).Count();
                int itemContrast = (from r in ItemList
                                    //where r.ItemCategoryID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Contrast
                                    select r).Count();

                int itemSedetion = (from r in ItemList
                                    where r.ItemCategoryID == 9//((IApplicationConfiguration)App.Current).ApplicationConfigurations.Contrast
                                    select r).Count();

                if (chkContrast.IsChecked == true && chkFilmWastage.IsChecked == true && itemFlim == 0 && itemContrast == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW14 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter at least one contrast and Film item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW14.Show();
                    SaveTemplate = false;
                    ClickedFlag = 0;
                }
                else if (chkContrast.IsChecked == true && itemContrast == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW14 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter at least one contrast item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW14.Show();
                    SaveTemplate = false;
                    ClickedFlag = 0;
                }
                else if (chkFilmWastage.IsChecked == true && itemFlim == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW14 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter at least one Film item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW14.Show();
                    SaveTemplate = false;
                    ClickedFlag = 0;
                }
                if (ItemList.Count != 0)
                {
                    # region For zero Wastage Qty Validation
                    foreach (var i in ItemList)
                    {
                        //    if (item > 0 && chkContrast.IsChecked == true && i.ItemCategoryID == 2)
                        //    {
                        //        if (i.WastageQantity == 0)
                        //        {
                        //            MessageBoxControl.MessageBoxChildWindow msgW14 =
                        //            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Contrast Wastage Quantity for " + i.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //            msgW14.Show();
                        //            SaveTemplate = false;
                        //            ClickedFlag = 0;
                        //        }
                        //    }
                        if (item > 0 && chkFilmWastage.IsChecked == true && i.ItemCategoryID == 3)
                        {
                            if (i.WastageQantity == 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW14 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Film Wastage Quantity for " + i.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW14.Show();
                                SaveTemplate = false;
                                ClickedFlag = 0;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    //if (chkFilmWastage.IsChecked == true && chkContrast.IsChecked == true)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgW14 =
                    //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter at least one contrast and film item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    msgW14.Show();
                    //    SaveTemplate = false;
                    //    ClickedFlag = 0;
                    //}
                    //else if (chkFilmWastage.IsChecked == false && chkContrast.IsChecked == true)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgW14 =
                    //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter at least one contrast item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    msgW14.Show();
                    //    SaveTemplate = false;
                    //    ClickedFlag = 0;
                    //}
                    //else if (chkFilmWastage.IsChecked == true && chkContrast.IsChecked == false)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgW14 =
                    //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter at least one film item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    msgW14.Show();
                    //    SaveTemplate = false;
                    //    ClickedFlag = 0;
                    //}
                }

                if (itemContrast > 0)
                    chkContrast.IsChecked = true;
                if (itemFlim > 0)
                    chkFilmWastage.IsChecked = true;
                if (itemSedetion > 0)
                    chkSedation.IsChecked = true;

                if (SaveTemplate == true)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to save the Technician Entry?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
                else
                {
                    ClickedFlag = 0;
                }

                #region unused Validations
                //if (chkFilmWastage.IsChecked == false && chkContrast.IsChecked == false && ItemList.Count == 0)
                //{
                //    if (SaveTemplate == true)
                //    {
                //        string msgTitle = "Palash";
                //        string msgText = "Are you sure you want to save the Technician Entry?";

                //        MessageBoxControl.MessageBoxChildWindow msgW =
                //            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //        msgW.Show();
                //    }
                //    else
                //    {
                //        ClickedFlag = 0;
                //    }
                //}
                //else if (chkContrast.IsChecked == true)
                //{
                //    foreach (var item1 in ItemList)
                //    {
                //        if (item1.ItemCategoryID != 2)
                //        {
                //            string msgTitle = "Palash";
                //            string msgText = "Please Add at least one contrast item ";
                //            MessageBoxControl.MessageBoxChildWindow msgW =
                //                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                //            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //            msgW.Show();
                //            break;
                //        }

                //    }

                //}
                //else if (chkFilmWastage.IsChecked == true)
                //{
                //    foreach (var item1 in ItemList)
                //    {
                //        if (item1.ItemCategoryString != "FILM")
                //        {
                //            string msgTitle = "Palash";
                //            string msgText = "Please Add at least one film item ";
                //            MessageBoxControl.MessageBoxChildWindow msgW =
                //                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                //            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //            msgW.Show();
                //        }
                //    }
                //}
                //else
                //{
                //    if (SaveTemplate == true)
                //    {
                //        string msgTitle = "Palash";
                //        string msgText = "Are you sure you want to save the Technician Entry?";

                //        MessageBoxControl.MessageBoxChildWindow msgW =
                //            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //        msgW.Show();
                //    }
                //    else
                //    {
                //        ClickedFlag = 0;
                //    }
                //}
                #endregion
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (IsUpdate == false)
                {
                    Save();
                }
                else
                {
                    Update();
                }
            }

            else
            {
                ClickedFlag = 0;
            }
        }
       // long Films = 0;
        private void Save()
        {
        }

        private void Update()
        {      
        }
        #endregion

        #region Item details
        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((MasterListItem)cmbStore.SelectedItem).ID > 0)
                {
                    cmbStore.ClearValidationError();
                    ItemListNew ItemsWin = new ItemListNew();
                    ItemsWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                    ItemsWin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                    ItemsWin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                    ItemsWin.ShowBatches = true;
                    ItemsWin.cmbStore.IsEnabled = false;
                    ItemsWin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                    ItemsWin.Show();
                }
                else
                {
                    if (cmbStore.SelectedItem == null)
                    {
                        cmbStore.TextBox.SetValidation("Please select the store");
                        cmbStore.TextBox.RaiseValidationError();
                    }
                    else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                    {
                        cmbStore.TextBox.SetValidation("Please select the store");
                        cmbStore.TextBox.RaiseValidationError();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            if (((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem) != null)
            {
                if (cmbStore.SelectedItem != null)
                {
                    if (((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ItemID > 0 && ((MasterListItem)cmbStore.SelectedItem).ID > 0)
                    {
                        cmbStore.ClearValidationError();
                        AssignBatch BatchWin = new AssignBatch();
                        BatchWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                        BatchWin.SelectedItemID = ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ItemID;
                        BatchWin.ItemName = ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ItemName;

                        BatchWin.OnAddButton_Click += new RoutedEventHandler(OnAddBatchButton_Click);
                        BatchWin.Show();
                    }
                }
                else
                {
                    if (cmbStore.SelectedItem == null)
                    {
                        cmbStore.TextBox.SetValidation("Please select the store");
                        cmbStore.TextBox.RaiseValidationError();
                    }
                    else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                    {
                        cmbStore.TextBox.SetValidation("Please select the store");
                        cmbStore.TextBox.RaiseValidationError();
                    }
                }
            }
        }

        void OnAddBatchButton_Click(object sender, RoutedEventArgs e)
        {
            AssignBatch AssignBatchWin = (AssignBatch)sender;

            if (AssignBatchWin.DialogResult == true)
            {
                if (AssignBatchWin.SelectedBatches != null)
                {
                    foreach (var item in AssignBatchWin.SelectedBatches)
                    {
                        var item1 = from r in ItemList
                                    where (r.BatchID == item.BatchID)
                                    select new clsRadItemDetailMasterVO
                                    {
                                        Status = r.Status,
                                        ItemID = r.ItemID,
                                        ItemName = r.ItemName,

                                    };
                        if (item1.ToList().Count == 0)
                        {

                            foreach (var BatchItems in ItemList.Where(x => x.ItemID == item.ItemID))
                            {
                                BatchItems.ItemID = item.ItemID;
                                BatchItems.ItemCode = item.ItemCode;
                                BatchItems.BatchID = item.BatchID;
                                BatchItems.BatchCode = item.BatchCode;
                                BatchItems.ExpiryDate = item.ExpiryDate;
                                BatchItems.BalanceQuantity = item.AvailableStock;
                                BatchItems.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                            }
                            dgItemDetailsList.ItemsSource = ItemList;
                            dgItemDetailsList.Focus();
                            dgItemDetailsList.UpdateLayout();
                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                }
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemDetailsList.SelectedItem != null)
            {
                clsRadItemDetailMasterVO obj;
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Item ?";
                clsRadItemDetailMasterVO objItem = dgItemDetailsList.SelectedItem as clsRadItemDetailMasterVO;
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        //ItemList.RemoveAt(dgItemDetailsList.SelectedIndex);
                        obj = ItemList.Where(z => z.ItemID == objItem.ItemID).FirstOrDefault();
                        ItemList.Remove(obj);
                        dgItemDetailsList.Focus();
                        dgItemDetailsList.UpdateLayout();
                        dgItemDetailsList.ItemsSource = ItemList;
                    }
                };
                msgWD.Show();
            }
        }

        #endregion

        #region Clear UI
        private void ClearData()
        {
            this.DataContext = new clsRadResultEntryVO();

            cmbTestName.SelectedValue = (long)0;
            cmbFilm.SelectedValue = (long)0;
            //cmbRadiologist.SelectedValue = (long)0;
            cmbStore.SelectedValue = (long)0;
            txtReferenceDoctor.Text = string.Empty;

            ItemList = new ObservableCollection<clsRadItemDetailMasterVO>();

            chkContrast.IsChecked = false;
            chkFilmWastage.IsChecked = false;
            chkSedation.IsChecked = false;

            IsUpdate = false;

            SelectedOrder = new clsRadOrderBookingVO();

        }

        #endregion

        #region validation
        bool Flag = false;

        private bool CheckValidation()
        {
            bool result = true;
            if (cmbTestName.SelectedItem == null)
            {
                cmbTestName.TextBox.SetValidation("Please select the Test Name");
                cmbTestName.TextBox.RaiseValidationError();
                cmbTestName.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbTestName.SelectedItem).ID == 0)
            {
                cmbTestName.TextBox.SetValidation("Please select the Test Name");
                cmbTestName.TextBox.RaiseValidationError();
                cmbTestName.Focus();
                result = false;
            }
            else
                cmbTestName.TextBox.ClearValidationError();
            if (txtReferenceDoctor.Text == "")
            {
                txtReferenceDoctor.SetValidation("Please enter Referred By");
                txtReferenceDoctor.RaiseValidationError();
                txtReferenceDoctor.Focus();
                result = false;
            }
            else
                txtReferenceDoctor.ClearValidationError();

            if (IsPageLoded)
            {
                foreach (var item in ItemList)
                {
                    if (item.BatchID == 0 && item.BatchesRequired == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW13 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please assign Batch for item :" + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW13.Show();
                        result = false;
                        return result;
                    }
                  
                    

                   
                   
                }
            }
            return result;
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {

            }
            else if (!((TextBox)sender).Text.IsNumberValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        private void txtTexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
        private void txtContrastDetails_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }


        #endregion

        #region Print Report
        private void PrintReport(long ResultID)
        {
            if (ResultID > 0)
            {
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                string URL = "../Reports/Radiology/ResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");


            }
        }


        #endregion


        private void ChkContrast_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkFilmWastage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

            SelectedOrder = new clsRadOrderBookingVO();
            ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder = null;
            ((IApplicationConfiguration)App.Current).OpenMainContent(new New_RadiologyWorkOrderGeneration());
            UserControl rootPage1 = Application.Current.RootVisual as UserControl;
            TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleHeader");
            mElement1.Text = "Radiology Work Order";

        }

        double qty;
        double wastageQty;

        private void dgItemDetailsList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            DataGrid dgItems = (DataGrid)sender;
            clsRadItemDetailMasterVO obj = (clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem;
            if (dgItemDetailsList.SelectedItem != null)
            {
                if (e.Column.Header != null)
                {
                    if (e.Column.Header.ToString().Equals("Actual Qty"))
                    {
                        if (obj.ActualQantity < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("PALASH", "Actual Quantity should not be in Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();

                            foreach (var item in OldItemList)
                            {
                                if (item.ID == ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ID)
                                {
                                    ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ActualQantity = item.ActualQantity;
                                }
                            }

                        }
                        if (obj.ItemCategoryID == 3)
                        {
                            if (!((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ActualQantity.ToString().IsNumberValid())
                            {
                                string msgText = "You can not Enter Actual Quantity in Decimal For FILM Category ";
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();
                                ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ActualQantity = Math.Ceiling(((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ActualQantity);
                            }
                        }
                        if (obj.ActualQantity > obj.BalanceQuantity)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                          new MessageBoxControl.MessageBoxChildWindow("PALASH", "Actual quantity should not be more than Balance Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                            if (OldItemList.ToList() != null)
                            {
                                foreach (var item in OldItemList)
                                {
                                    if (item.ID == ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ID)
                                    {
                                        ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ActualQantity = item.ActualQantity;
                                    }
                                }
                            }
                        }

                        if ((obj.ActualQantity + obj.WastageQantity) > obj.BalanceQuantity)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("PALASH", "Sum of Actual quantity and Wastage quantity should not be more than Balance Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();

                            foreach (var item in OldItemList)
                            {
                                if (item.ID == ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ID)
                                {
                                    ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ActualQantity = item.ActualQantity;
                                    ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).WastageQantity = 0;
                                }
                            }
                        }
                    }
                }
                if (e.Column.Header != null)
                {
                    if (e.Column.Header.ToString().Equals("Wastage Qty"))
                    {
                        if (obj.WastageQantity < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("PALASH", "Wastage Quantity should not be in Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                            ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).WastageQantity = 0;
                        }
                        if (obj.ItemCategoryID == 3)
                        {
                            if (!((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).WastageQantity.ToString().IsNumberValid())
                            {
                                string msgText = "You can not Enter Wastage Quantity in Decimal for FILM Category ";
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();
                                ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).WastageQantity = Math.Ceiling(((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).WastageQantity);
                            }
                        }
                        if (obj.WastageQantity > obj.BalanceQuantity)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                          new MessageBoxControl.MessageBoxChildWindow("PALASH", "Wastage quantity should not be more than Balance Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                            ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).WastageQantity = 0;
                        }

                        if ((obj.ActualQantity + obj.WastageQantity) > obj.BalanceQuantity)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("PALASH", "Sum of Actual quantity and Wastage quantity should not be more than Balance Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();

                            foreach (var item in OldItemList)
                            {
                                if (item.ID == ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ID)
                                {
                                    ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ActualQantity = item.ActualQantity;
                                    ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).WastageQantity = 0;
                                }
                            }
                        }
                    }
                }
                dgItemDetailsList.SelectedItem = null;
            }
        }

        private void dgItemDetailsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtReferenceDoctor_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;

        }

        private void txtReferenceDoctor_LostFocus(object sender, RoutedEventArgs e)
        {
            txtReferenceDoctor.Text = txtReferenceDoctor.Text.ToTitleCase();
        }

        private void txtReferenceDoctor_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!((AutoCompleteBox)sender).Text.IsOnlyCharacters())
            {
                ((AutoCompleteBox)sender).Text = textBefore;
                //((AutoCompleteBox)sender).SelectionStart = selectionStart;
                //((AutoCompleteBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void FillDoctor()
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

                    txtReferenceDoctor.ItemsSource = null;
                    txtReferenceDoctor.ItemsSource = objList;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }



        #region CommentedCode
        //private void FillOrderBooking(bolo Status)
        //{
        //    clsGetOrderBookingListBizActionVO BizAction = new clsGetOrderBookingListBizActionVO();
        //    try
        //    {
        //        BizAction.BookingList = new List<clsRadOrderBookingVO>();

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, rag) =>
        //        {
        //            if (arg.Error == null)
        //            {
        //                if (((clsGetOrderBookingListBizActionVO)arg.Result).BookingList != null)
        //                {
        //                    clsGetOrderBookingListBizActionVO result = arg.Result as clsGetOrderBookingListBizActionVO;

        //                    List<clsRadOrderBookingVO> ObjList = new List<clsRadOrderBookingVO>();
        //                    if (result.BookingList != null)
        //                    {
        //                        ObjList.Clear();


        //                        foreach (var item in result.BookingList)
        //                        {
        //                            ObjList.Add(item);

        //                        }

        //                        var results = from r in ObjList
        //                                      where r.OrderDetailID == ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.OrderDetailID
        //                                      select r;
        //                        if (results.ToList().Count > 0)
        //                        {
        //                            ObjList = results.ToList();

        //                        }

        //                        dgOrdertList.ItemsSource = null;
        //                        dgOrdertList.ItemsSource = ObjList;





        //                    }
        //                }
        //            }
        //            else
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //                msgW1.Show();
        //            }
        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        #endregion
    }
}
