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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.Collections;
using System.Reflection;
using Liquid;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using C1.Silverlight.RichTextBox.Documents;
using C1.Silverlight;



namespace PalashDynamics.Administration
{
    public partial class RadiologyTemplateMaster : UserControl
    {
        #region Variable Declaration
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        bool IsCancel = true;
        bool IsNew = false;
        int ClickedFlag = 0;
        public List<MasterListItem> objList { get; set; }
        //public Liquid.RichTextEditor Text
        //{
        //    get { return richTextEditor; }
        //}
        #endregion

        #region Paging

        public PagedSortableCollectionView<clsRadiologyVO> DataList { get; private set; }

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
       
        public RadiologyTemplateMaster()
        {
            InitializeComponent();
          
            //richTextEditor.Html = "<p>This is some sample <strong>Rich</strong> text.</p>";
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
           
            DataList = new PagedSortableCollectionView<clsRadiologyVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

        /// <summary>
        /// Purpose:Used when refresh event get called.
        /// Creation Date:29/06/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }
       
        private void RadiologyTemplateMaster_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                SetCommandButtonState("Load");
                this.DataContext = new clsRadiologyVO();
               // FillRadiologist();
                FillRadiologistFront();//Added By Yogesh
                FillGender();
                FillTemplateResult();
                FetchData();
                CheckValidations();
                txtCode.Focus();
                cmdRadiologist.IsEnabled = true;
            }
            IsPageLoded = true;
            txtCode.Focus();
        }

        #region FillCombobox
        /// <summary>
        /// Purpose:Fill combobox from master table
        /// </summary>
        private void FillRadiologist()
        {

            clsGetRadiologistBizActionVO BizAction = new clsGetRadiologistBizActionVO();            
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetRadiologistBizActionVO)arg.Result).MasterList);

                    cmbRadiologist.ItemsSource = null;
                    cmbRadiologist.ItemsSource = objList;
                    cmbRadiologist.SelectedItem = objList[0];

                    cmbRadiologistFront.ItemsSource = null;
                    cmbRadiologistFront.ItemsSource = objList;
                    cmbRadiologistFront.SelectedItem = objList[0];


                    if (this.DataContext != null)
                    {
                        cmbRadiologist.SelectedValue = ((clsRadiologyVO)this.DataContext).Radiologist;
                        cmbRadiologistFront.SelectedValue = ((clsRadiologyVO)this.DataContext).Radiologist;
                    }



                    

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillRadiologistFront()
        {
            clsGetRadiologistBizActionVO BizAction = new clsGetRadiologistBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetRadiologistBizActionVO)arg.Result).MasterList);

                    //cmbRadiologist.ItemsSource = null;
                    //cmbRadiologist.ItemsSource = objList;
                    //cmbRadiologist.SelectedItem = objList[0];

                    cmbRadiologistFront.ItemsSource = null;
                    cmbRadiologistFront.ItemsSource = objList;
                    cmbRadiologistFront.SelectedItem = objList[0];
                    //cmbRadiologistFront

                    if (this.DataContext != null)
                    {
                        cmbRadiologist.SelectedValue = ((clsRadiologyVO)this.DataContext).Radiologist;
                        cmbRadiologistFront.SelectedValue = ((clsRadiologyVO)this.DataContext).Radiologist;
                    }





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

                    //SelectedGenderList = new List<MasterListItem>();

                    //SelectedGenderList.Add(new MasterListItem(0, "- Select -"));
                    //SelectedGenderList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);


                    //foreach (var item in SelectedGenderList)
                    //{
                    //    item.Status = false;


                    //}

                    //cmbGender.ItemsSource = null;
                    //cmbGender.ItemsSource = SelectedGenderList;
                    //cmbGender.SelectedItem = SelectedGenderList[0];

                    //cmbGenderFront.ItemsSource = null;
                    //cmbGenderFront.ItemsSource = SelectedGenderList;
                    //cmbGenderFront.SelectedItem = SelectedGenderList[0];

                    //if (this.DataContext != null)
                    //{
                    //    cmbGender.SelectedValue = ((clsRadiologyVO)this.DataContext).GenderID;
                    //    cmbGenderFront.SelectedValue = ((clsRadiologyVO)this.DataContext).GenderID;
                    //}

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
                        cmbGender.SelectedValue = ((clsRadiologyVO)this.DataContext).GenderID;
                        cmbGenderFront.SelectedValue = ((clsRadiologyVO)this.DataContext).GenderID;
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

        private void FillTemplateResult()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_RadTemplateResult;
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
                   
                    cmbTemplateResult.ItemsSource = null;
                    cmbTemplateResult.ItemsSource = objList;
                    cmbTemplateResult.SelectedItem = objList[0];

                    cmbTemplateResultFront.ItemsSource = null;
                    cmbTemplateResultFront.ItemsSource = objList;
                    cmbTemplateResultFront.SelectedItem = objList[0];
                    if (this.DataContext != null)
                    {
                        cmbTemplateResult.SelectedValue = ((clsRadiologyVO)this.DataContext).TemplateResultID;
                        cmbTemplateResultFront.SelectedValue = ((clsRadiologyVO)this.DataContext).TemplateResultID;
                    }
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FetchData()
        {
            clsGetRadTemplateBizActionVO BizAction = new clsGetRadTemplateBizActionVO();
            BizAction.TemplateList = new List<clsRadiologyVO>();

            if (txtDescriptionFront.Text != "")
            {
                BizAction.Description = txtDescriptionFront.Text;
            }
            if (cmbRadiologistFront.SelectedItem != null)
            {
                BizAction.Radiologist = ((MasterListItem)cmbRadiologistFront.SelectedItem).ID;
            }
            if (cmbGenderFront.SelectedItem != null )
            {
                BizAction.GenderID = ((MasterListItem)cmbGenderFront.SelectedItem).ID;
            }
            if (cmbTemplateResultFront.SelectedItem != null )
            {
                BizAction.TemplateResultID = ((MasterListItem)cmbTemplateResultFront.SelectedItem).ID;
            }
           
           
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

                    if (((clsGetRadTemplateBizActionVO)arg.Result).TemplateList != null)
                    {
                        clsGetRadTemplateBizActionVO result = arg.Result as clsGetRadTemplateBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.TemplateList != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.TemplateList)
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

        #endregion
      
        /// <summary>
        /// Purpose:For Add new template 
        /// Creation Date:29/06/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {

            this.SetCommandButtonState("New");
            ClearData();
            txtCode.Focus();
            IsNew = true;
            
            cmdRadiologist.IsEnabled = false;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Template Details";
          
            objAnimation.Invoke(RotationType.Forward);
            
                if (cmbGender.ItemsSource != null)
                {
                    foreach (MasterListItem item in SelectedGenderList)
                    {
                        item.Status = false;
                    }
                    //cmbGender.SelectedItem = SelectedGenderList[0];
                }
           
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {            

            FetchData();
            SetCommandButtonState("Cancel");
            cmdRadiologist.IsEnabled = true;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";
            SelectedGenderList = new List<MasterListItem>();
            //SelectedGenderList = new List<MasterListItem>();
            //GenderList = new List<MasterListItem>();

            FillGender();
            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Radiology Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmRadiologyConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
          
            else
            {
                IsCancel = true;
            }
          

        }
        
        #region Save data
        /// <summary>
        /// Purpose:Save data in database
        /// Creation Date:29/06/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                //if (CheckDuplicasy())
                //{

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

                    Save();
                //}
                //else
                //{
                //    ClickedFlag = 0;
                //}

            }

            else
            {
                ClickedFlag = 0;
            }
        }
       
        private void Save()
        {
            clsAddRadTemplateMasterBizActionVO BizAction = new clsAddRadTemplateMasterBizActionVO();
            BizAction.TemplateDetails = (clsRadiologyVO)grdTemplate.DataContext;

            
            
            //byte [] dBytes;
            //code by yogesh vss
            //String str=Tt.Html;
            //System.Text.UTF8Encoding  enc = new System.Text.UTF8Encoding();
            //dBytes = enc.GetBytes(str);



            //BinaryFormatter bf = new BinaryFormatter(); 
            //byte[] bytes; 
            //MemoryStream ms = new MemoryStream(); 
            //string orig = Tt.Html; 
            //bf.Serialize(ms, orig); 
            //ms.Seek(0, 0); 
            //bytes = ms.ToArray();        

           // BizAction.TemplateDetails.Template = dBytes;
            BizAction.TemplateDetails.TemplateDesign = richTextEditor.Html;  //BizAction.TemplateDetails.TemplateDesign = Text.Html;
      
            
            if (cmbRadiologist.SelectedItem != null)
                BizAction.TemplateDetails.Radiologist = ((MasterListItem)cmbRadiologist.SelectedItem).ID;
            
            if (cmbGender.SelectedItem != null)
                BizAction.TemplateDetails.GenderList = SelectedGenderList.DeepCopy();

            if (cmbTemplateResult.SelectedItem != null)
                BizAction.TemplateDetails.TemplateResultID = ((MasterListItem)cmbTemplateResult.SelectedItem).ID;

            BizAction.GenderList = new List<MasterListItem>();
            BizAction.GenderList = SelectedGenderList;
            
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag = 0;
               
                if (arg.Error == null)
                {
                    if (((clsAddRadTemplateMasterBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        SetCommandButtonState("Save");
                        FetchData();
                        ClearData();
                        objAnimation.Invoke(RotationType.Backward);
                        //Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Template Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                    else if (((clsAddRadTemplateMasterBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    else if (((clsAddRadTemplateMasterBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();

                    }
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

        #endregion

        #region Modify Data
        /// <summary>
        /// Purpose:For modify exisiting template 
        /// Creation Date:30/06/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            cmdRadiologist.IsEnabled = true;
            if (ClickedFlag == 1)
            {

                bool ModifyTemplate = true;
                ModifyTemplate = CheckValidations();
                if (ModifyTemplate == true)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to Modify the Template Master?";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                    msgW1.Show();
                }
                else
                    ClickedFlag = 0;
            }


            
        

        }
        
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            //if (result == MessageBoxResult.Yes)
            //{
            //    //if (CheckDuplicasy())
            //    //{
            //        Modify();
            //    //}
            //    //else
            //    //{
            //    //    ClickedFlag = 0;
            //    //}
            //}
            //else
            //{
            //    ClickedFlag = 0;
            //}




            if (result == MessageBoxResult.Yes)
            {
                //if (CheckDuplicasy())
                //{

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

                Modify();
                //}
                //else
                //{
                //    ClickedFlag = 0;
                //}

            }

            else
            {
                ClickedFlag = 0;
            }
        }
        
        private void Modify()
        {
            clsAddRadTemplateMasterBizActionVO BizAction = new clsAddRadTemplateMasterBizActionVO();
            BizAction.TemplateDetails = (clsRadiologyVO)grdTemplate.DataContext;

            if (cmbRadiologist.SelectedItem != null)
                BizAction.TemplateDetails.Radiologist = ((MasterListItem)cmbRadiologist.SelectedItem).ID;

            if (cmbGender.SelectedItem != null)
               // BizAction.TemplateDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

            if (cmbTemplateResult.SelectedItem != null)
                BizAction.TemplateDetails.TemplateResultID = ((MasterListItem)cmbTemplateResult.SelectedItem).ID;

            //BizAction.TemplateDetails.TemplateDesign = Text.Html;
            BizAction.TemplateDetails.TemplateDesign = richTextEditor.Html;
            BizAction.TemplateDetails.GenderList =  SelectedGenderList;
               

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
              
                ClickedFlag = 0;
                if (arg.Error == null)
                {
                    if (((clsAddRadTemplateMasterBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        SetCommandButtonState("Modify");
                        FetchData();
                        ClearData();
                        objAnimation.Invoke(RotationType.Backward);
                        //Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Template Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        ClickedFlag = 0;
                    }
                    else if (((clsAddRadTemplateMasterBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be update because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        ClickedFlag = 0;
                    }
                    else if (((clsAddRadTemplateMasterBizActionVO)arg.Result).SuccessStatus == 2)
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
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while updating Template Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        #endregion

        #region search Data

        /// <summary>
        /// Purpose:For search existing template using different criteria.
        /// Creation Date:30/06/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
            dgDataPager.PageIndex = 0;
        }

        #endregion

        #region View Details
        /// <summary>
        /// Purpose:For view selected template  details.
        /// Creation Date:30/06/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    
        private void View_Click(object sender, RoutedEventArgs e)
        {
             
            SetCommandButtonState("View");
            ClearData();
            if (dgTemplateList.SelectedItem != null)
            {
                IsNew = false;
                cmdRadiologist.IsEnabled = false;
                grdTemplate.DataContext = ((clsRadiologyVO)dgTemplateList.SelectedItem);
               // cmbGender.SelectedValue = ((clsRadiologyVO)dgTemplateList.SelectedItem).GenderID;
                cmbRadiologist.SelectedValue = ((clsRadiologyVO)dgTemplateList.SelectedItem).Radiologist;
                cmbTemplateResult.SelectedValue = ((clsRadiologyVO)dgTemplateList.SelectedItem).TemplateResultID;
                
                if (((clsRadiologyVO)dgTemplateList.SelectedItem).TemplateDesign != " ")
                {
                    richTextEditor.Html = ((clsRadiologyVO)dgTemplateList.SelectedItem).TemplateDesign;   //Text.Html = ((clsRadiologyVO)dgTemplateList.SelectedItem).TemplateDesign;
                }
              
               
              //  clsGetRadioTemplateGenderBizAction
                //added by Yogesh K to get selected gender list
                //clsAddRadTemplateMasterBizActionVO
              
              
                
                clsGetRadioTemplateGenderBizActionVO BizAction = new clsGetRadioTemplateGenderBizActionVO();
                BizAction.GenderDetails = new List<MasterListItem>();
                SelectedGenderList = new List<MasterListItem>();

                //   BizAction.TemplateID = ((clsRadiologyVO)this.DataContext).ID;   



                BizAction.TemplateID = ((clsRadiologyVO)dgTemplateList.SelectedItem).TemplateID;

              //  ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ReferredDoctor;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgTemplateList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetRadioTemplateGenderBizActionVO)arg.Result).GenderDetails != null)
                        {
                            clsGetRadioTemplateGenderBizActionVO result = arg.Result as clsGetRadioTemplateGenderBizActionVO;

                            if (((clsGetRadioTemplateGenderBizActionVO)arg.Result).GenderDetails != null)
                            {
                                DataList.Clear();

                                List<MasterListItem> list = new List<MasterListItem>();
                                list = (List<MasterListItem>)cmbGender.ItemsSource;

                                if (list != null && list.Count > 0 && ((clsGetRadioTemplateGenderBizActionVO)arg.Result).GenderDetails != null)
                                {
                                    foreach (var items in list)
                                    {
                                        foreach (var item in ((clsGetRadioTemplateGenderBizActionVO)arg.Result).GenderDetails)
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
                              //  List<MasterListItem> list = new List<MasterListItem>();

                                //SelectedGenderList = (List<MasterListItem>)cmbGender.ItemsSource;

                                //if (SelectedGenderList != null && SelectedGenderList.Count > 0 && ((clsGetRadioTemplateGenderBizActionVO)arg.Result).GenderDetails != null)
                                //{
                                //    foreach (var items in SelectedGenderList)
                                //    {
                                //        foreach (var item in ((clsGetRadioTemplateGenderBizActionVO)arg.Result).GenderDetails)
                                //        {
                                //            if (items.ID == item.ID)
                                //            {
                                //                items.IsDefault = true;
                                //            }
                                //        }
                                //    }
                                //}
                                //foreach (var items in SelectedGenderList)
                                //{
                                //    if (items.IsDefault == true)
                                //    {
                                //        items.Status = true;
                                //    }
                                //    else
                                //    {
                                //        items.Status = false;
                                //    }

                                //}
                                //cmbGender.ItemsSource = null;
                                //cmbGender.ItemsSource = SelectedGenderList;
                             

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

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
          //  mElement.Text = " : " + ((clsRadiologyVO)dgTemplateList.SelectedItem).Description;
        }

        #endregion
       
        #region Clear Data
        /// <summary>
        /// Purpose:Clear controls on forms
        /// Creation Date:30/06/2011
        /// </summary>
        private void ClearData()
        {
            grdTemplate.DataContext = new clsRadiologyVO();
            txtCode.Text = string.Empty;
            txtDescriptionFront.Text = string.Empty;
            cmbRadiologist.SelectedValue = (long)0;
            cmbGender.SelectedValue = (long)0;
            cmbTemplateResult.SelectedValue = (long)0;
            richTextEditor.Html = string.Empty;

        }

        #endregion

        #region Validation
        /// <summary>
        /// Purpose:Checks & assigns validations for the controls.
        /// Creation Date:30/06/2011
        /// </summary>
        /// <returns></returns>
        private bool CheckValidations()
        {
            bool result = true;

            if (txtDescription.Text == "")
            {

                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();

                result = false;
            }
            else
                txtDescription.ClearValidationError();


            if (txtCode.Text == "")
            {

                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();

                result = false;
            }
            else
                txtCode.ClearValidationError();

            if (IsPageLoded)
            {
                if (richTextEditor.Html == "")
                {
                    MessageBoxControl.MessageBoxChildWindow msgW12 =
                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Design Template.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW12.Show();
                    richTextEditor.Focus();
                    result = false;
                    return result;

                }

                if (cmbRadiologist.SelectedItem == null)
                {
                   //commented by Yogesh K
                    //cmbRadiologist.TextBox.SetValidation("Please select Radiologist");
                    //cmbRadiologist.TextBox.RaiseValidationError();
                    //result = false;
                    //cmbRadiologist.Focus();
                    
                }
                else if (((MasterListItem)cmbRadiologist.SelectedItem).ID == 0)
                {
                    //commented by Yogesh K
                    //cmbRadiologist.TextBox.SetValidation("Please select Radiologist");
                    //cmbRadiologist.TextBox.RaiseValidationError();
                    //result = false;
                    //cmbRadiologist.Focus();
                }
                else
                    cmbRadiologist.ClearValidationError();



                if (cmbTemplateResult.SelectedItem == null)
                {
                    cmbTemplateResult.TextBox.SetValidation("Please select Template Result");
                    cmbTemplateResult.TextBox.RaiseValidationError();
                    result = false;
                    cmbTemplateResult.Focus();
                }
                else if (((MasterListItem)cmbTemplateResult.SelectedItem).ID == 0)
                {
                    cmbTemplateResult.TextBox.SetValidation("Please select Template Result");
                    cmbTemplateResult.TextBox.RaiseValidationError();
                    result = false;
                    cmbTemplateResult.Focus();
                }
                else
                    cmbTemplateResult.ClearValidationError();
            }


            return result;
        }
        /// <summary>
        /// Purpose:Check entered Code and Description available or not
        /// </summary>
        /// <returns></returns>
        private bool CheckDuplicasy()
        {
            clsRadiologyVO Item;
            clsRadiologyVO Item1;
            if (IsNew)
            {
               

                Item = ((PagedSortableCollectionView<clsRadiologyVO>)dgTemplateList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                Item1 = ((PagedSortableCollectionView<clsRadiologyVO>)dgTemplateList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()));
            }
            else
            {
              
                Item = ((PagedSortableCollectionView<clsRadiologyVO>)dgTemplateList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.TemplateID != ((clsRadiologyVO)dgTemplateList.SelectedItem).TemplateID);
                Item1 = ((PagedSortableCollectionView<clsRadiologyVO>)dgTemplateList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()) && p.TemplateID != ((clsRadiologyVO)dgTemplateList.SelectedItem).TemplateID);
            }

            if (Item != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (Item1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Description already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCode.Text = txtCode.Text.ToTitleCase();
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescription.Text = txtDescription.Text.ToTitleCase();
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
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cmdRadiologist_Click(object sender, RoutedEventArgs e)
        {
            try
            { 

               // clsRadOrderBookingDetailsVO objItemVO = new clsRadOrderBookingDetailsVO();

               clsRadiologyVO objItemVO = new clsRadiologyVO();
              //  clsPathoTestTemplateDetailsVO objItemVO = new clsPathoTestTemplateDetailsVO();
              //  objItemVO = (clsPathoTestTemplateDetailsVO)dgTemplateList.SelectedItem;
                objItemVO = (clsRadiologyVO)dgTemplateList.SelectedItem;
                if (objItemVO != null)
                {
                    DefineRadiologist win = new DefineRadiologist();
                    win.Show();
                   win.GetItemDetails(objItemVO);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
