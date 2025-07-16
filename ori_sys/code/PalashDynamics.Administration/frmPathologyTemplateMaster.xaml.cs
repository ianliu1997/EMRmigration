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
using PalashDynamics.Animations;
using CIMS;
using System.Reflection;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using C1.Silverlight;
using C1.Silverlight.RichTextBox.Documents;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Administration
{
    public partial class frmPathologyTemplateMaster : UserControl
    {
        #region Variable Declaration
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        bool IsCancel = true;
        bool IsNew = false;
        int ClickedFlag = 0;
        public bool IsStatusChanged = false;
        //public Liquid.RichTextEditor Text
        //{
        //    get { return richTextEditor; }
        //}
        #endregion

        public frmPathologyTemplateMaster()
        {
            InitializeComponent();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            DataList = new PagedSortableCollectionView<clsPathoTestTemplateDetailsVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }

        private void frmTemplateMaster_Loaded(object sender, RoutedEventArgs e)
        {

            if (!IsPageLoded)
            {
                SetCommandButtonState("Load");
                this.DataContext = new clsPathoTestTemplateDetailsVO();
                FillPathologist();              
                FetchData();
                txtCode.Focus();
            }
            IsPageLoded = true;
            txtCode.Focus();
        }
        private void FetchData()
        {
            clsGetPathoTemplateMasterBizActionVO BizAction = new clsGetPathoTemplateMasterBizActionVO();
            BizAction.TemplateDetails = new List<clsPathoTestTemplateDetailsVO>();
            if (txtDescriptionFront.Text != "")
            {
                BizAction.Description = txtDescriptionFront.Text;
            }
            //if (cmbRadiologistFront.SelectedItem != null)
            //{
            //    BizAction.Pathologist = ((MasterListItem)cmbRadiologistFront.SelectedItem).ID;
            //}
            //if (cmbGenderFront.SelectedItem != null)
            //{
            //    BizAction.GenderID = ((MasterListItem)cmbGenderFront.SelectedItem).ID;
            //}
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            dgTemplateList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetPathoTemplateMasterBizActionVO)arg.Result).TemplateDetails != null)
                    {
                        clsGetPathoTemplateMasterBizActionVO result = arg.Result as clsGetPathoTemplateMasterBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.TemplateDetails != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.TemplateDetails)
                            {
                                DataList.Add(item);
                            }
                            dgTemplateList.ItemsSource = null;
                            dgTemplateList.ItemsSource = DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #region FillCombobox
        /// <summary>
        /// Purpose:Fill combobox from master table
        /// </summary>
        List<MasterListItem> ObjDoctorList = new List<MasterListItem>();
        private void FillPathologist()
        {

            clsGetPathologistBizActionVO BizAction = new clsGetPathologistBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objPathoList = new List<MasterListItem>();
                    objPathoList.Clear();
                    //objPathoList.Add(new MasterListItem(0, "-- Select --"));
                    objPathoList.AddRange(((clsGetPathologistBizActionVO)arg.Result).MasterList);
                    ObjDoctorList.Clear();
                    foreach (var item in objPathoList)
                    {
                        MasterListItem Objcls = new MasterListItem();
                        Objcls.ID = item.ID;
                        Objcls.Description = item.Description;
                        Objcls.Status = false;
                        ObjDoctorList.Add(Objcls);
                    }


                    cmbPathologist.ItemsSource = null;
                    cmbPathologist.ItemsSource = ObjDoctorList;
                   // cmbPathologist.SelectedItem = ObjDoctorList[0];

                    cmbRadiologistFront.ItemsSource = null;
                    cmbRadiologistFront.ItemsSource = objPathoList;
                    if (objPathoList.Count > 0)
                    {
                        cmbRadiologistFront.SelectedItem = objPathoList[0];
                    }

                    if (this.DataContext != null)
                    {
                        cmbPathologist.SelectedValue = ((clsPathoTestTemplateDetailsVO)this.DataContext).Pathologist;
                        cmbPathologist.SelectedValue = ((clsPathoTestTemplateDetailsVO)this.DataContext).Pathologist;
                    }

                    FillGender();


                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
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
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                    cmbGender.SelectedItem = objList[0];

                    cmbGenderFront.ItemsSource = null;
                    cmbGenderFront.ItemsSource = objList;
                    cmbGenderFront.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbGender.SelectedValue = ((clsPathoTestTemplateDetailsVO)this.DataContext).GenderID;
                        cmbGenderFront.SelectedValue = ((clsPathoTestTemplateDetailsVO)this.DataContext).GenderID;
                    }

                    foreach (var item in (List<MasterListItem>)cmbGender.ItemsSource)
                    {
                        if (item.ID == 0)
                        {
                            item.IsEnable = false;
                        }
                        else
                        {
                            item.IsEnable = true;
                        }
                    }

                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        #endregion

        #region Paging

        public PagedSortableCollectionView<clsPathoTestTemplateDetailsVO> DataList { get; private set; }

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
                // RaisePropertyChanged("DataListPageSize");
            }
        }



        #endregion
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            Validations();
            ClickedFlag = 0;
            this.SetCommandButtonState("New");
            ClearData();
            txtCode.Focus();
            cmbGender.ItemsSource = null;
            cmbPathologist.IsEnabled = true;

            FillPathologist();
            IsNew = true;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Template Details";
            objAnimation.Invoke(RotationType.Forward);

           
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                bool SaveTemplate = true;
                SaveTemplate = CheckValidations();
                if (SaveTemplate == true)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to save the Template Master?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
                else
                    ClickedFlag = 0;
            }
        }
        List<MasterListItem> GenderList = new List<MasterListItem>();
        List<MasterListItem> SelectedGenderList = new List<MasterListItem>();
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //-----added by rohini h dated 14/4/2016

                GenderList = (List<MasterListItem>)cmbGender.ItemsSource;
                MasterListItem newItem1 = new MasterListItem();
                SelectedGenderList = new List<MasterListItem>();
                foreach (var item in GenderList)
                {
                    if (item.Status == true)
                    {
                        long id1 = 0;
                        string str = "";
                        id1 = item.ID;
                        str = item.Description;                      

                        newItem1 = new MasterListItem(id1, str);
                        SelectedGenderList.Add(newItem1);
                    }
                }
                //-------------
                Save();
            }

            else
            {
                ClickedFlag = 0;
            }
        }
        int i = 1;
        public void Save()
        {
            clsAddPathoTemplateMasterBizActionVO BizAction = new clsAddPathoTemplateMasterBizActionVO();
            var SelectedDoctorList = from r in ObjDoctorList
                                     where r.Status == true
                                     select r;
            List<MasterListItem> DoctorList = new List<MasterListItem>();

            foreach (var item in ObjDoctorList)
            {
                if (item.Status == true)
                {
                    DoctorList.Add(item);
                }
            }
            //added by rohini 
            BizAction.GenderList=new List<MasterListItem>();
            BizAction.GenderList = SelectedGenderList;
            //
            if (SelectedDoctorList.Count() > 0)
            {
                foreach (var item in SelectedDoctorList)
                {
                    BizAction.TemplateDetails = (clsPathoTestTemplateDetailsVO)this.DataContext;
                    if (DoctorList.Count > 0)
                    {
                        BizAction.TemplateDetails.MultiplePathoDoctor = item.Status;

                        BizAction.TemplateDetails.Pathologist = item.ID;

                        BizAction.TemplateDetails.PathologistName = item.Description;
                    }

                    if (cmbGender.SelectedItem != null)
                        BizAction.TemplateDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

                    if (cmbPathologist.SelectedItem != null)
                    {
                        BizAction.TemplateDetails.Pathologist = item.ID;

                        BizAction.TemplateDetails.PathologistName = item.Description;
                    }
                    BizAction.TemplateDetails.Template = richTextEditor.Html;
                  
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        ClickedFlag = 0;

                        if (arg.Error == null)
                        {
                            if (((clsAddPathoTemplateMasterBizActionVO)arg.Result).SuccessStatus == 0)
                            {
                                SetCommandButtonState("New");
                                if (DoctorList.Count == i)
                                {
                                    FetchData();
                                    ClearData();
                                    objAnimation.Invoke(RotationType.Backward);
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Template Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    ClickedFlag = 0;
                                }
                                i = i + 1;
                            }
                            else if (((clsAddPathoTemplateMasterBizActionVO)arg.Result).SuccessStatus == 1)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                                ClickedFlag = 0;
                            }
                            else if (((clsAddPathoTemplateMasterBizActionVO)arg.Result).SuccessStatus == 2)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION and GENDER already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                                ClickedFlag = 0;
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Template Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                            ClickedFlag = 0;
                        }

                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                    ClickedFlag = 0;
                }
            }
            else
            {
                BizAction.TemplateDetails = (clsPathoTestTemplateDetailsVO)this.DataContext;
               
                BizAction.TemplateDetails.MultiplePathoDoctor = false;

                if (cmbGender.SelectedItem != null)
                    BizAction.TemplateDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

                BizAction.TemplateDetails.Template = richTextEditor.Html;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;

                    if (arg.Error == null)
                    {
                        if (((clsAddPathoTemplateMasterBizActionVO)arg.Result).SuccessStatus == 0)
                        {
                            SetCommandButtonState("Save");
                            //if (DoctorList.Count == i)
                            //{
                                FetchData();
                                //added by rohinee dated 7-7-2016
                                FillGender();
                                //
                                ClearData();

                                objAnimation.Invoke(RotationType.Backward);
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Template Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();                               
                                //SetCommandButtonState("New");
                                ClickedFlag = 0;
                           // }
                            i = i + 1;
                        }
                        else if (((clsAddPathoTemplateMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                            ClickedFlag = 0;
                        }
                        else if (((clsAddPathoTemplateMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                            ClickedFlag = 0;
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Template Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        ClickedFlag = 0;
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                ClickedFlag = 0;

            }
        }
       
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                bool SaveTemplate = true;
                SaveTemplate = CheckValidations();
                if (SaveTemplate == true)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to update the Template Master?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);
                    msgW.Show();
                }
                else
                    ClickedFlag = 0;
            }
        }

        void msgW2_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                GenderList = (List<MasterListItem>)cmbGender.ItemsSource;
                MasterListItem newItem1 = new MasterListItem();
                SelectedGenderList = new List<MasterListItem>();
                foreach (var item in GenderList)
                {
                    if (item.Status == true)
                    {
                        long id1 = 0;
                        string str = "";
                        id1 = item.ID;
                        str = item.Description;

                        newItem1 = new MasterListItem(id1, str);
                        SelectedGenderList.Add(newItem1);
                    }
                }
                //-------------
                Modify();
            }

            else
            {
                ClickedFlag = 0;
            }
        }

        public bool CheckValidations()
        {
            bool result = true;
            if (txtDescription.Text == "")
            {
                txtDescription.SetValidation("Please enter Description");
                txtDescription.RaiseValidationError();
                result = false;
                ClickedFlag = 0;
                txtDescription.Focus();
            }
            else
                txtDescription.ClearValidationError();

            if (txtCode.Text == "")
            {
                txtCode.SetValidation("Please enter Code");
                txtCode.RaiseValidationError();
                result = false;
                ClickedFlag = 0;
                txtCode.Focus();
            }
            else
                txtCode.ClearValidationError();

            //if ((MasterListItem)cmbGender.SelectedItem == null)
            //{
            //    cmbGender.TextBox.SetValidation("Please Select Gender");
            //    cmbGender.TextBox.RaiseValidationError();
            //    //CmbCategory.Focus();
            //    result = false;
            //}
            //else if (((MasterListItem)cmbGender.SelectedItem).ID == 0)
            //{
            //    cmbGender.TextBox.SetValidation("Please Select Gender");
            //    cmbGender.TextBox.RaiseValidationError();
            //    //CmbCategory.Focus();
            //    result = false;
            //}
            //else
                //cmbGender.TextBox.ClearValidationError();
            //commented by rohini for temp perpose
            //List<MasterListItem> SelectedDoctorList = new List<MasterListItem>();
            //foreach (var item in ObjDoctorList)
            //{
            //    if (item.Status == true)
            //    {
            //        SelectedDoctorList.Add(item);
            //    }
            //}
            //if (((MasterListItem)cmbPathologist.SelectedItem).ID > 0)
            //{
            //    if (SelectedDoctorList.Count == 0)
            //    {
            //        SelectedDoctorList.Add((MasterListItem)cmbPathologist.SelectedItem);
            //    }
            //}
            //if (SelectedDoctorList.Count == 0)
            //{
            //    if (((MasterListItem)cmbPathologist.SelectedItem).ID == 0)
            //    {
            //        cmbPathologist.TextBox.SetValidation("Please select Pathologist");
            //        cmbPathologist.TextBox.RaiseValidationError();
            //        result = false;
            //        cmbPathologist.Focus();
            //    }
            //}
            //else if (SelectedDoctorList.Count == 0)
            //{

            //    cmbPathologist.TextBox.SetValidation("Please select Radiologist");
            //    cmbPathologist.TextBox.RaiseValidationError();
            //    result = false;
            //    cmbPathologist.Focus();
            //}
            //
            if (richTextEditor.Text.Trim() == string.Empty)
            {
            //if (richTextEditor.Html.Trim() ==string.Empty)
            //{
                MessageBoxControl.MessageBoxChildWindow msgW12 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Template.", 
                   MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW12.Show();
                richTextEditor.Focus();
                result = false;
                ClickedFlag = 0;
                return result;
            }
            return result;
        }

        //by rohini dated 9.2.16 
        public bool Validations()
        {
            bool result = true;
            if (txtDescription.Text == "")
            {
                txtDescription.SetValidation("Please enter Description");
                txtDescription.RaiseValidationError();
                result = false;
                ClickedFlag = 0;
                txtDescription.Focus();
            }
            else
                txtDescription.ClearValidationError();

            if (txtCode.Text == "")
            {
                txtCode.SetValidation("Please enter Code");
                txtCode.RaiseValidationError();
                result = false;
                ClickedFlag = 0;
                txtCode.Focus();
            }
            else
                txtCode.ClearValidationError();
            
            return result;
        }
        public void Modify()
        {
            clsAddPathoTemplateMasterBizActionVO BizAction = new clsAddPathoTemplateMasterBizActionVO();
            BizAction.TemplateDetails = (clsPathoTestTemplateDetailsVO)this.DataContext;
            if (cmbGender.SelectedItem != null)
                BizAction.TemplateDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
            if (cmbPathologist.SelectedItem != null)
            {
                BizAction.TemplateDetails.Pathologist = ((MasterListItem)cmbPathologist.SelectedItem).ID;
                BizAction.TemplateDetails.PathologistName = ((MasterListItem)cmbPathologist.SelectedItem).Description;
            }
            BizAction.TemplateDetails.Template = richTextEditor.Html;
            //added by rohini 
            BizAction.TemplateDetails.GenderList = SelectedGenderList;
            //
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag = 0;
                if (arg.Error == null)
                {
                    if (((clsAddPathoTemplateMasterBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        SetCommandButtonState("Modify");
                        FetchData();
                        //added by rohinee dated 7-7-2016
                        FillGender();
                        //
                        ClearData();
                        objAnimation.Invoke(RotationType.Backward);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Template Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ClickedFlag = 0;
                    }
                    else if (((clsAddPathoTemplateMasterBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be update because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        ClickedFlag = 0;
                    }
                    else if (((clsAddPathoTemplateMasterBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be update because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        ClickedFlag = 0;

                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Template Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    ClickedFlag = 0;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
            SetCommandButtonState("Cancel");
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            SelectedGenderList = new List<MasterListItem>();

            //added by rohinee dated 7/7/2016 for gender not refreshed at the time of view record
            FillGender();
            //
           
            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Pathology Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmPathologyConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                IsCancel = true;
            }
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            Validations();
            SetCommandButtonState("View");
            ClearData();
            IsNew = false;

            if (dgTemplateList.SelectedItem != null)
            {
                this.DataContext = (clsPathoTestTemplateDetailsVO)dgTemplateList.SelectedItem;
                //txtCode.IsEnabled = false;
                cmdModify.IsEnabled = ((clsPathoTestTemplateDetailsVO)this.DataContext).Status;
                //cmbGender.SelectedValue = ((clsPathoTestTemplateDetailsVO)this.DataContext).GenderID;
                //cmbPathologist.SelectedValue = ((clsPathoTestTemplateDetailsVO)this.DataContext).Pathologist;
                if (((clsPathoTestTemplateDetailsVO)dgTemplateList.SelectedItem).Template != null && ((clsPathoTestTemplateDetailsVO)dgTemplateList.SelectedItem).Template != " ")
                {
                    richTextEditor.Html = ((clsPathoTestTemplateDetailsVO)dgTemplateList.SelectedItem).Template;
                }
                //added by rohini to get selected gender list
                clsGetPathoTemplateGenderBizActionVO BizAction = new clsGetPathoTemplateGenderBizActionVO();
                BizAction.GenderDetails = new List<MasterListItem>();

                BizAction.TemplateID = ((clsPathoTestTemplateDetailsVO)this.DataContext).ID;
                
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgTemplateList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPathoTemplateGenderBizActionVO)arg.Result).GenderDetails != null)
                        {
                            clsGetPathoTemplateGenderBizActionVO result = arg.Result as clsGetPathoTemplateGenderBizActionVO;

                            if (((clsGetPathoTemplateGenderBizActionVO)arg.Result).GenderDetails != null)
                            {
                                DataList.Clear();

                               
                                List<MasterListItem> list = new List<MasterListItem>();
                                list = (List<MasterListItem>)cmbGender.ItemsSource;

                                if (list != null && list.Count > 0 && ((clsGetPathoTemplateGenderBizActionVO)arg.Result).GenderDetails!=null)
                                {
                                    foreach (var items in list)
                                    {
                                        foreach (var item in ((clsGetPathoTemplateGenderBizActionVO)arg.Result).GenderDetails)
                                        {
                                            if (items.ID == item.ID)
                                            {
                                                items.IsDefault = true;
                                            }
                                        }
                                    }
                                }
                                foreach (var items in list)
                                {
                                    if (items.IsDefault == true)
                                    {
                                        items.Status = true;
                                    }
                                    else
                                    {
                                        items.Status = false;
                                    }
                                   
                                }
                                cmbGender.ItemsSource = null;
                                cmbGender.ItemsSource = list;
                                cmbGender.UpdateLayout(); 

                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                objAnimation.Invoke(RotationType.Forward);
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCode.Text = txtCode.Text.ToTitleCase();
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescription.Text = txtDescription.Text.ToTitleCase();
        }

        #region Clear Data
        /// <summary>
        /// Purpose:Clear controls on forms
        /// Creation Date:30/06/2011
        /// </summary>
        private void ClearData()
        {
            this.DataContext = new clsPathoTestTemplateDetailsVO();
            txtCode.Text = string.Empty;
            txtDescriptionFront.Text = string.Empty;
            cmbPathologist.SelectedValue = (long)0;
            cmbGender.SelectedValue = (long)0;

            richTextEditor.Html = string.Empty;

        }

        #endregion

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdPathologist.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdPathologist.IsEnabled = false;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdPathologist.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdPathologist.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdPathologist.IsEnabled = true;
                    break;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdPathologist.IsEnabled = false;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var oldItem = e.RemovedItems.OfType<C1TabItem>().FirstOrDefault();
                if (oldItem == null) return; // richTextBoxTab and the others are null the first time around because InitializeComponent is running.

                if (oldItem == richTextBoxTab)
                {
                    htmlBox.Text = richTextEditor.Html;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextEditor.Document);
                }
                else if (oldItem == htmlTab)
                {
                    richTextEditor.Html = htmlBox.Text;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextEditor.Document);
                }
                else if (oldItem == rtfTab)
                {
                    richTextEditor.Document = new RtfFilter().ConvertToDocument(rtfBox.Text);
                    htmlBox.Text = richTextEditor.Html;
                }
            }
                
            catch { }
        }
        
        private void UpdateTempStatus_Click(object sender, RoutedEventArgs e)
        {
            clsAddPathoTemplateMasterBizActionVO BizAction = new clsAddPathoTemplateMasterBizActionVO();
            BizAction.IsModifyStatus = true;
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == true)
                BizAction.IsStatusChanged = true;
            else
                BizAction.IsStatusChanged = false;
            if (dgTemplateList.SelectedItem != null)
                BizAction.TemplateDetails = (clsPathoTestTemplateDetailsVO)dgTemplateList.SelectedItem;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && ((clsAddPathoTemplateMasterBizActionVO)arg.Result).SuccessStatus == 1)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "STATUS UPDATED SUCCESSFULLY.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            FetchData();
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Template Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        //added by rohini
        private void cmdPathologist_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsPathoTestTemplateDetailsVO objItemVO = new clsPathoTestTemplateDetailsVO();
                objItemVO = (clsPathoTestTemplateDetailsVO)dgTemplateList.SelectedItem;
                if (objItemVO != null)
                {
                    DefinePathologist win = new DefinePathologist();
                    win.Show();
                    win.GetItemDetails(objItemVO);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtDescriptionFront_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                FetchData();
            }
        }
    }
}
