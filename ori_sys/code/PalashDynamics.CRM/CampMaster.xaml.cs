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
using PalashDynamics.ValueObjects.CRM;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using System.Windows.Controls.Data;
using System.ComponentModel;
using System.Windows.Browser;
using System.Globalization;
using System.Windows.Data;
using PalashDynamics.Animations;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Collections;


namespace PalashDynamics.CRM
{
    public partial class CampMaster : UserControl
    {
 

        public CampMaster()
        {
            InitializeComponent();


            DataList = new PagedSortableCollectionView<clsCampMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;


            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

        }

        #region Paging

        public PagedSortableCollectionView<clsCampMasterVO> DataList { get; private set; }

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

        #region Variable Declaration
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        private SwivelAnimation objAnimation;
        bool IsNew = false;
        int ClickedFlag = 0;

        #endregion

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }

        private void CampMaster_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.DataContext = new clsCampMasterVO();
                SetCommandButtonState("New");
                CheckValidations();
                FetchData();
                
                txtCampName.Focus();
                txtCode.Focus();
                Indicatior.Close();
            }
            IsPageLoded = true;
            txtCampName.Focus();
            txtCampName.UpdateLayout();
            txtCode.Focus();
            txtCode.UpdateLayout();
            
           
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("Save");

            ClearControl();
            this.DataContext = new clsCampMasterVO();
            IsNew = true;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Camp";
            objAnimation.Invoke(RotationType.Forward);
           
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            ClearControl();
            txtDescription.ClearValidationError();
            txtCode.ClearValidationError();


            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);
          
        }
     
        /// <summary>
        /// Purpose:Save new Camp.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                bool SaveCamp = true;

                SaveCamp = CheckValidations();

                if (SaveCamp == true)
                {

                    string msgTitle = "";
                    string msgText = "Are you sure you want to save the Camp Master?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();

                }
                else
                    ClickedFlag = 0;

                
            }
        }
        
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {

                    SaveCampMaster();
                }
                else
                {
                    ClickedFlag = 0;
                }

            }

            else
            {
                ClickedFlag = 0;
            }
        }
                    
        private void SaveCampMaster()
        {
            //Indicatior.Show();
            clsAddCampMasterBizActionVO BizAction = new clsAddCampMasterBizActionVO();
            BizAction.CampMasterDetails = (clsCampMasterVO)this.DataContext;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
               {
                   ClickedFlag = 0;
                   if (arg.Error == null)
                   {
                       FetchData();
                       ClearControl();
                       objAnimation.Invoke(RotationType.Backward);
                       //Indicatior.Close();
                       
                       MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Camp Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                       msgW1.Show();
                   }
                   else
                   {
                       MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Camp  Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                       msgW1.Show();
                   }

                   SetCommandButtonState("New");

               };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        
        /// <summary>
        /// Purpose:Modify existing camp.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if(ClickedFlag==1)
            {
            bool ModifySchedule = true;
            ModifySchedule = CheckValidations();
            if (ModifySchedule == true)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update the Camp Master?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();
            }
            else

                ClickedFlag = 0;
        }
       
        /// <summary>
        /// Purpose:Update existing camp details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
       

    


        }   

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {
                    UpdateCampMaster();
                }
                else
                {
                    ClickedFlag = 0;
                }
            }
            else
            {
                ClickedFlag = 0;
            }

        }
        private void UpdateCampMaster()
        {
            clsUpdateCampMasterBizActionVO BizAction = new clsUpdateCampMasterBizActionVO();
            BizAction.CampMasterDetails = (clsCampMasterVO)this.DataContext;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag = 0;
                SetCommandButtonState("New");
                if (arg.Error == null)
                {
                    FetchData();
                    objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Camp Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while updating Camp  Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
                
        /// <summary>
        /// Purpose:Search Camp Details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void FetchData()
        {
            clsGetCampMasterListBizActionVO BizAction = new clsGetCampMasterListBizActionVO();
            BizAction.CampMasterList = new List<clsCampMasterVO>();

            if (txtCampName.Text != null)
                BizAction.Description = txtCampName.Text;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgCampList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {

                    if (((clsGetCampMasterListBizActionVO)arg.Result).CampMasterList != null)
                    {
                        //dgCampList.ItemsSource = ((clsGetCampMasterListBizActionVO)arg.Result).CampMasterList;

                        clsGetCampMasterListBizActionVO result = arg.Result as clsGetCampMasterListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.CampMasterList != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.CampMasterList)
                            {
                                DataList.Add(item);
                            }

                            dgCampList.ItemsSource = null;
                            dgCampList.ItemsSource = DataList;

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
       
        /// <summary>
        /// Purpose:View camp details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public clsCampMasterVO objCampMaster = new clsCampMasterVO();

        private void hlbViewCampMaster_Click(object sender, RoutedEventArgs e)
        {

            SetCommandButtonState("Modify");
            IsNew = false;

            if ((clsCampMasterVO)dgCampList.SelectedItem != null) ;
            {
                objCampMaster = ((clsCampMasterVO)dgCampList.SelectedItem);
                FillCampMaster(objCampMaster.ID);
            }


            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + objCampMaster.Description;
                                           
            
            objAnimation.Invoke(RotationType.Forward);

        }

        private void FillCampMaster(long iID)
        {   
            clsGetCampMasterByIDBizActionVO BizAction = new clsGetCampMasterByIDBizActionVO();
            BizAction.CampMasterDetails = (clsCampMasterVO)this.DataContext;
            BizAction.CampMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            BizAction.ID = iID;
             
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    SetCommandButtonState("Modify");
                    if (dgCampList.SelectedItem != null)
                        objAnimation.Invoke(RotationType.Forward);

                    ((clsCampMasterVO)this.DataContext).ID = ((clsGetCampMasterByIDBizActionVO)arg.Result).CampMasterDetails.ID;
                    ((clsCampMasterVO)this.DataContext).Description = ((clsGetCampMasterByIDBizActionVO)arg.Result).CampMasterDetails.Description;
                    ((clsCampMasterVO)this.DataContext).Code = ((clsGetCampMasterByIDBizActionVO)arg.Result).CampMasterDetails.Code;
                    ((clsCampMasterVO)this.DataContext).Description = ((clsGetCampMasterByIDBizActionVO)arg.Result).CampMasterDetails.Description;
                    ((clsCampMasterVO)this.DataContext).Status = ((clsGetCampMasterByIDBizActionVO)arg.Result).CampMasterDetails.Status;
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

        #region Validations
        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCode.Text = txtCode.Text.ToTitleCase();
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescription.Text = txtDescription.Text.ToTitleCase();
        }

        private void txtCampName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCampName.Text = txtCampName.Text.ToTitleCase();
        }
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

           
            return result;
        }
        #endregion

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion
        
        #region Reset All controls
        private void ClearControl()
        {
            txtCode.Text = string.Empty;
            txtDescription.Text = string.Empty;
        }
        #endregion


        private bool CheckDuplicasy()
        {
            clsCampMasterVO CampItem;
            clsCampMasterVO CampItem1;
            if (IsNew)
            {
                CampItem = ((PagedSortableCollectionView<clsCampMasterVO>)dgCampList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                CampItem1 = ((PagedSortableCollectionView<clsCampMasterVO>)dgCampList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()));
            }
            else
            {
                CampItem = ((PagedSortableCollectionView<clsCampMasterVO>)dgCampList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.ID != ((clsCampMasterVO)dgCampList.SelectedItem).ID);
                CampItem1 = ((PagedSortableCollectionView<clsCampMasterVO>)dgCampList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()) && p.ID != ((clsCampMasterVO)dgCampList.SelectedItem).ID);
            }

            if (CampItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (CampItem1 != null)
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



    }
}
