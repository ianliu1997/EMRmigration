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
using PalashDynamics;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.UserControls;
using PalashDynamics.Administration;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.Pharmacy;
namespace PalashDynamics.Administration
{

    public partial class DefineRadiologist : ChildWindow
    {
        Boolean IsPageLoded = false;
        clsRadiologyVO objMasterVO = null;
     
        Boolean IsRowSelected = false;
        public bool IsFromSubTest = false;

        public List<clsRadiologyVO> ItemSupplierList;
        public List<clsRadiologyVO> _ItemSupplier;
        public clsAddRadiologistToTempbizActionVO BizActionobj;

      
        public Boolean IsEditMode { get; set; }
        public long pkID { get; set; }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 BizActionobj = new clsAddRadiologistToTempbizActionVO();
                    
               // PalashDynamics.Service.PalashServiceReferance.clsRadioTestTemplateDetailsVO sdg=new PalashDynamics.Service.PalashServiceReferance.clsRadioTestTemplateDetailsVO();

                BizActionobj.ItemSupplier = new clsRadiologyVO();
                                                //clsRadioTestTemplateDetailsVO

                BizActionobj.ItemSupplierList = (List<clsRadiologyVO>)dgRadiologist.ItemsSource;

                BizActionobj.ItemSupplier.TemplateID = objMasterVO.TemplateID;
                BizActionobj.ItemSupplier.Radiologist1 = objMasterVO.Radiologist1;
                BizActionobj.ItemSupplier.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                string msgTitle = "";

                string msgText = "";

                if (IsEditMode == false)
                {
                    msgText = "Are you sure you want to Save the  Details?";

                }
                MessageBoxControl.MessageBoxChildWindow msgW =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
               // BizActionobj.ItemSupplierList = (List<clsRadioTestTemplateDetailsVO>)dgRadiologist.ItemsSource;
                //BizActionobj.ItemSupplier.CreatedUnitID = 1;
                //BizActionobj.ItemSupplier.UpdatedUnitID = 1;
                //BizActionobj.ItemSupplier.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID; ;
                //BizActionobj.ItemSupplier.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //BizActionobj.ItemSupplier.AddedDateTime = DateTime.Now;
                //BizActionobj.ItemSupplier.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

              //  BizActionobj.ItemSupplier.TemplateID = objMasterVO.ID;
               // BizActionobj.ItemSupplier.Radiologist = objMasterVO.RadiologistName;
            //    BizActionobj.ItemSupplier.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


            //    string msgTitle = "";

            //    string msgText = "";

            //    if (IsEditMode == false)
            //    {
            //        msgText = "Are you sure you want to Save the  Details?";

            //    }
            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //          new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

            //    msgW.Show();
            }
            catch (Exception)
            {

                throw;
            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessAsync(BizActionobj, new clsUserVO());
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {

                            //IF DATA IS SAVED
                            if (((clsAddRadiologistToTempbizActionVO)(arg.Result)).SuccessStatus == 1)
                            {
                                if (IsEditMode == true)
                                {
                                    string msgTitle = "";
                                    string msgText = "Radiologist defined successfully";

                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


                                    msgW.Show();
                                    IsEditMode = false;
                                    pkID = 0;
                                }
                                else
                                {
                                    string msgTitle = "";
                                    string msgText = "Radiologist defined successfully";

                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


                                    msgW.Show();

                                }
                                this.Close();
                            }
                        }
                    };
                    client.CloseAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {

        }
        public void GetItemDetails(clsRadiologyVO objItemVO)
        {
            objMasterVO = objItemVO;
        }
        public DefineRadiologist()
        {
            InitializeComponent();//clsPathoTestTemplateDetailsVO  clsRadioTestTemplateDetailsVO

            this.DataContext = new clsRadiologyVO();
        }

       
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                lblTempName.Text = "";

                if (objMasterVO.Description != null)
                {
                    lblTempName.Text = objMasterVO.Description.ToString();
                    lbl.Text = "Template Name :";
                }

               FillDataGrid();
            }
            IsPageLoded = true;
        }


        private void GetItemSupplierlist()
        {

            clsGetRadiologistToTempBizActionVO objBizActionVO = new clsGetRadiologistToTempBizActionVO();
           // clsGetPathologistToTempBizActionVO objBizActionVO = new clsGetPathologistToTempBizActionVO();
            objBizActionVO.ItemSupplier = new clsRadiologyVO();

            objBizActionVO.ItemSupplier.TemplateID = objMasterVO.TemplateID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    _ItemSupplier = null;
                    _ItemSupplier = new List<clsRadiologyVO>();
                    _ItemSupplier = ((clsGetRadiologistToTempBizActionVO)args.Result).ItemSupplierList;
                    CheckForExistingSupplier();

                }

            };

            client.ProcessAsync(objBizActionVO, new clsUserVO());
            client.CloseAsync();
        }




        //private void GetItemSupplierlist()
        //{


        //    clsGetRadiologistToTempBizActionVO objBizActionVO = new clsGetRadiologistToTempBizActionVO();
        //  //  clsAddRadTestMasterBizActionVO objBizActionVO = new clsAddRadTestMasterBizActionVO();
        //    objBizActionVO.ItemSupplier = new clsRadioTestTemplateDetailsVO();

        //    objBizActionVO.ItemSupplier = new clsRadioTestTemplateDetailsVO();

        //    objBizActionVO.ItemSupplier = new clsPathoTestTemplateDetailsVO();

        //    objBizActionVO.ItemSupplier.TemplateID = objMasterVO1.ID;


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {

        //            _ItemSupplier = null;
        //            _ItemSupplier = new List<clsRadioTestTemplateDetailsVO>();
        //            _ItemSupplier = ((clsGetRadiologistToTempBizActionVO)args.Result).ItemSupplierList;
        //            CheckForExistingSupplier();

        //        }

        //    };

        //    client.ProcessAsync(objBizActionVO, new clsUserVO());
        //    client.CloseAsync();
        //}
        private void CheckForExistingSupplier()
        {
            
            List<clsRadiologyVO> objList = (List<clsRadiologyVO>)dgRadiologist.ItemsSource;


            if (objList != null && objList.Count > 0)
            {
                if (_ItemSupplier != null && _ItemSupplier.Count > 0)
                {
                    foreach (var item in _ItemSupplier)
                    {
                        foreach (var items in objList)
                        {
                            if (items.ID == item.Radiologist)
                            {
                                items.Status = item.Status;
                                // items.SelectedHPLevel = item.SelectedHPLevel;
                            }

                        }
                    }

                }
                dgRadiologist.ItemsSource = objList;
            }

        }
        List<MasterListItem> ObjDoctorList = new List<MasterListItem>();
        private void FillDataGrid()
        {
            try
            {

                clsGetRadiologistBizActionVO BizAction = new clsGetRadiologistBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objRadioList = new List<MasterListItem>();
                        objRadioList.Clear();
                        //objPathoList.Add(new MasterListItem(0, "-- Select --"));
                        objRadioList.AddRange(((clsGetRadiologistBizActionVO)arg.Result).MasterList);
                        ObjDoctorList.Clear();
                        foreach (var item in objRadioList)
                        {
                            MasterListItem Objcls = new MasterListItem();
                            Objcls.ID = item.ID;
                            Objcls.Description = item.Description;
                            Objcls.Status = false;
                            ObjDoctorList.Add(Objcls);
                        }

                        dgRadiologist.ItemsSource = null;
                        dgRadiologist.ItemsSource = objRadioList;
                         GetItemSupplierlist();


                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList = ((clsGetRadiologistBizActionVO)arg.Result).MasterList;
                        ItemSupplierList = new List<PalashDynamics.ValueObjects.Radiology.clsRadiologyVO>();
                        clsRadiologyVO obj = new clsRadiologyVO();

                        foreach (var item in objList)
                        {

                            ItemSupplierList.Add(new clsRadiologyVO { ID = item.ID, RadiologistName = item.Description, Status = item.Status });
                        }


                        dgRadiologist.ItemsSource = null;
                        dgRadiologist.ItemsSource = ItemSupplierList;
                        GetItemSupplierlist();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }

            //   try
            //   {

            //       clsRadiologyVO BizAction = new clsRadiologyVO();
            //// BizAction.MasterList = new List<MasterListItem>();

            //   Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //   PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //   client.ProcessCompleted += (s, arg) =>
            //   {
            //       if (arg.Error == null && arg.Result != null)
            //       {
            //           List<MasterListItem> objRadioList = new List<MasterListItem>();
            //           objRadioList.Clear();
            //           objRadioList.Add(new MasterListItem(0, "-- Select --"));
            //           objRadioList.AddRange(((clsRadiologyVO)arg.Result));
            //           //ObjDoctorList.Clear();
            //           //foreach (var item in objPathoList)
            //           //{
            //           //    MasterListItem Objcls = new MasterListItem();
            //           //    Objcls.ID = item.ID;
            //           //    Objcls.Description = item.Description;
            //           //    Objcls.Status = false;
            //           //    ObjDoctorList.Add(Objcls);
            //           //}

            //           //dgItemSupplier.ItemsSource = null;
            //           //dgItemSupplier.ItemsSource = objPathoList;
            //           //GetItemSupplierlist();
            //          dgRadiologist.ItemsSource = objRadioList;

            //          // List<MasterListItem> objList = new List<MasterListItem>();
            //           //objList = ((clsRadiologyVO)arg.Result).MasterList;
            //           //ItemSupplierList = new List<clsPathoTestTemplateDetailsVO>();
            //           //clsPathoTestTemplateDetailsVO obj = new clsPathoTestTemplateDetailsVO();

            //           //foreach (var item in objList)
            //           //{

            //           //    ItemSupplierList.Add(new clsPathoTestTemplateDetailsVO { ID = item.ID, SupplierName = item.Description, HPLevelList = obj.HPLevelList, Status = item.Status });
            //           //}


            //           //dgItemSupplier.ItemsSource = null;
            //           //dgItemSupplier.ItemsSource = ItemSupplierList;
            //           //GetItemSupplierlist();
            //       }

            //   };
            //   client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //   client.CloseAsync();
            //    }
            //   catch (Exception)
            //   {

            //       throw;
            //   }

        }

        private void dgItemSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
           
            var chk = sender as CheckBox;
            List<clsRadiologyVO> obj = new List<clsRadiologyVO>();
            obj = (List<clsRadiologyVO>)dgRadiologist.ItemsSource;
            int ind = dgRadiologist.SelectedIndex;
        }

        private void dgRadiologist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

