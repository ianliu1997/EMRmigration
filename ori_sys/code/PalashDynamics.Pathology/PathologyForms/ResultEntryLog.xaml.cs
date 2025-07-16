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
using System.ComponentModel;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Pathology;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class ResultEntryLog : UserControl
    {
        #region Variables and List Declaration
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public PagedSortableCollectionView<clsPathoTestParameterVO> MasterList { get; private set; }
        public int DataListPageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
            }
        }
        PagedCollectionView collection;
        clsAppConfigVO myAppConfig;
        SolidColorBrush NormalColorCode = new SolidColorBrush();
        SolidColorBrush MinColorCode = new SolidColorBrush();
        SolidColorBrush MaxColorCode = new SolidColorBrush();
        #endregion
        #region Constructor
        public ResultEntryLog()
        {
            InitializeComponent();
            myAppConfig = new clsAppConfigVO();
            myAppConfig = ((IApplicationConfiguration)App.Current).ApplicationConfigurations;
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            MasterList = new PagedSortableCollectionView<clsPathoTestParameterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            DataListPageSize = 15;
            
            DataPager.PageSize = DataListPageSize;
            DataPager.Source = MasterList;

            this.DataPager.DataContext = MasterList;
            dgTestList.DataContext = MasterList;
            GetTestWiseParameters();
           // SetColorCode();
        }
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetTestWiseParameters();
        }

        #endregion

        #region Button Click Events
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            GetTestWiseParameters();
        }
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).OpenMainContent(new NewPathologyWorkOrderGeneration());
        }
        #endregion

        #region Private Methods
        private void GetTestWiseParameters()
        {
            try
            {
                clsGetPathoTestParameterAndSubTestDetailsBizActionVO bizActionObj = new clsGetPathoTestParameterAndSubTestDetailsBizActionVO();
                bizActionObj.ParameterDetails = new clsPathoTestParameterVO();
                bizActionObj.TestList = new List<clsPathoTestParameterVO>();
                if (!String.IsNullOrEmpty(txtFrontFirstName.Text))
                    bizActionObj.ParameterDetails.FirstName = txtFrontFirstName.Text;
                if (!String.IsNullOrEmpty(txtFrontLastName.Text))
                    bizActionObj.ParameterDetails.LastName = txtFrontLastName.Text;
                if (!String.IsNullOrEmpty(txtTestName.Text))
                    bizActionObj.ParameterDetails.PathoTestName = txtTestName.Text;
                if (!String.IsNullOrEmpty(txtFrontMRNO.Text))
                    bizActionObj.ParameterDetails.MrNo = txtFrontMRNO.Text;
                if (dtpFromDate.SelectedDate != null)
                    bizActionObj.ParameterDetails.FromDate = ((DateTime)dtpFromDate.SelectedDate).Date;
                if (dtpToDate.SelectedDate != null)
                    bizActionObj.ParameterDetails.ToDate = ((DateTime)dtpToDate.SelectedDate).Date.AddDays(1);
                bizActionObj.IsPagingEnabled = true;
                bizActionObj.StartIndex = MasterList.PageIndex * MasterList.PageSize;
                bizActionObj.MaximumRows = MasterList.PageSize;
                bizActionObj.IsForResultEntryLog = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetPathoTestParameterAndSubTestDetailsBizActionVO result = args.Result as clsGetPathoTestParameterAndSubTestDetailsBizActionVO;
                        MasterList.Clear();
                        if (result.TestList != null && result.TestList.Count > 0)
                        {
                            MasterList.TotalItemCount = (int)((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)args.Result).TotalRows;
                            foreach (clsPathoTestParameterVO item in result.TestList)
                            {
                                if (item.IsNumeric == true)
                                {
                                    Color C = new Color();
                                    C.R = 198;
                                    C.B = 24;
                                    C.G = 15;
                                    C.A = 255;

                                    string[] NoramlRange1 = null;
                                    char[] Splitchar = { '-' };
                                    NoramlRange1 = item.NormalRange.Split(Splitchar);

                                    if (!String.IsNullOrEmpty(item.ResultValue))
                                    {
                                        if ((Convert.ToDouble(item.ResultValue) > (Convert.ToDouble(NoramlRange1[1]))))
                                        {
                                            //item.ApColor = myAppConfig.pathomaxColorCode;
                                        }
                                        else if ((Convert.ToDouble(item.ResultValue) < (Convert.ToDouble(NoramlRange1[0]))))
                                        {
                                           // item.ApColor = myAppConfig.pathominColorCode;
                                        }
                                        else
                                        {
                                          //  item.ApColor = myAppConfig.pathonormalColorCode;
                                        }
                                    }
                                }
                                MasterList.Add(item);
                            }
                            dgTestList.ItemsSource = null;
                            collection = new PagedCollectionView(MasterList);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("PatientName"));
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("PathoTestName"));
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("AddedDateTime"));
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("UserName"));
                            dgTestList.ItemsSource = collection;
                            DataPager.Source = null;
                            DataPager.PageSize = MasterList.PageSize;
                            DataPager.Source = MasterList;
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "ERROR OCCURRED WHILE PROCESSING.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {

            }
            return;
        }

       //private void SetColorCode()
       // {

       //     #region Normal Color
       //     string NormalColor = myAppConfig.pathonormalColorCode;
       //     switch (NormalColor)
       //     {
       //         case "Black":
       //             NormalColorCode = new SolidColorBrush(Colors.Black);
       //             break;
       //         case "Blue":
       //             NormalColorCode = new SolidColorBrush(Colors.Blue);
       //             break;
       //         case "Brown":
       //             NormalColorCode = new SolidColorBrush(Colors.Brown);
       //             break;
       //         case "Cyan":
       //             NormalColorCode = new SolidColorBrush(Colors.Cyan);
       //             break;
       //         case "DarkGray":
       //             NormalColorCode = new SolidColorBrush(Colors.DarkGray);
       //             break;
       //         case "Gray":
       //             NormalColorCode = new SolidColorBrush(Colors.Gray);
       //             break;
       //         case "Green":
       //             NormalColorCode = new SolidColorBrush(Colors.Green);
       //             break;
       //         case "LightGray":
       //             NormalColorCode = new SolidColorBrush(Colors.LightGray);
       //             break;
       //         case "Magenta":
       //             NormalColorCode = new SolidColorBrush(Colors.Magenta);
       //             break;
       //         case "Orange":
       //             NormalColorCode = new SolidColorBrush(Colors.Orange);
       //             break;
       //         case "Purple":
       //             NormalColorCode = new SolidColorBrush(Colors.Purple);
       //             break;
       //         case "Red":
       //             NormalColorCode = new SolidColorBrush(Colors.Red);
       //             break;
       //         case "Transparent":
       //             NormalColorCode = new SolidColorBrush(Colors.Transparent);
       //             break;
       //         case "White":
       //             NormalColorCode = new SolidColorBrush(Colors.White);
       //             break;
       //         case "Yellow":
       //             NormalColorCode = new SolidColorBrush(Colors.Yellow);
       //             break;
       //     }
       //     txtNormalValue.Background = NormalColorCode;
       //     #endregion

       //     #region Min ValueColor
       //     string MinColor = myAppConfig.pathominColorCode;
       //     switch (MinColor)
       //     {
       //         case "Black":
       //             MinColorCode = new SolidColorBrush(Colors.Black);
       //             break;
       //         case "Blue":
       //             MinColorCode = new SolidColorBrush(Colors.Blue);
       //             break;
       //         case "Brown":
       //             MinColorCode = new SolidColorBrush(Colors.Brown);
       //             break;
       //         case "Cyan":
       //             MinColorCode = new SolidColorBrush(Colors.Cyan);
       //             break;
       //         case "DarkGray":
       //             MinColorCode = new SolidColorBrush(Colors.DarkGray);
       //             break;
       //         case "Gray":
       //             MinColorCode = new SolidColorBrush(Colors.Gray);
       //             break;
       //         case "Green":
       //             MinColorCode = new SolidColorBrush(Colors.Green);
       //             break;
       //         case "LightGray":
       //             MinColorCode = new SolidColorBrush(Colors.LightGray);
       //             break;
       //         case "Magenta":
       //             MinColorCode = new SolidColorBrush(Colors.Magenta);
       //             break;
       //         case "Orange":
       //             MinColorCode = new SolidColorBrush(Colors.Orange);
       //             break;
       //         case "Purple":
       //             MinColorCode = new SolidColorBrush(Colors.Purple);
       //             break;
       //         case "Red":
       //             MinColorCode = new SolidColorBrush(Colors.Red);
       //             break;
       //         case "Transparent":
       //             MinColorCode = new SolidColorBrush(Colors.Transparent);
       //             break;
       //         case "White":
       //             MinColorCode = new SolidColorBrush(Colors.White);
       //             break;
       //         case "Yellow":
       //             MinColorCode = new SolidColorBrush(Colors.Yellow);
       //             break;
       //     }

       //     txtMinValue.Background = MinColorCode;
       //     #endregion

       //     #region MAXColor
       //     string MaxColor = myAppConfig.pathomaxColorCode;
       //     switch (MaxColor)
       //     {
       //         case "Black":
       //             MaxColorCode = new SolidColorBrush(Colors.Black);
       //             break;
       //         case "Blue":
       //             MaxColorCode = new SolidColorBrush(Colors.Blue);
       //             break;
       //         case "Brown":
       //             MaxColorCode = new SolidColorBrush(Colors.Brown);
       //             break;
       //         case "Cyan":
       //             MaxColorCode = new SolidColorBrush(Colors.Cyan);
       //             break;
       //         case "DarkGray":
       //             MaxColorCode = new SolidColorBrush(Colors.DarkGray);
       //             break;
       //         case "Gray":
       //             MaxColorCode = new SolidColorBrush(Colors.Gray);
       //             break;
       //         case "Green":
       //             MaxColorCode = new SolidColorBrush(Colors.Green);
       //             break;
       //         case "LightGray":
       //             MaxColorCode = new SolidColorBrush(Colors.LightGray);
       //             break;
       //         case "Magenta":
       //             MaxColorCode = new SolidColorBrush(Colors.Magenta);
       //             break;
       //         case "Orange":
       //             MaxColorCode = new SolidColorBrush(Colors.Orange);
       //             break;
       //         case "Purple":
       //             MaxColorCode = new SolidColorBrush(Colors.Purple);
       //             break;
       //         case "Red":
       //             MaxColorCode = new SolidColorBrush(Colors.Red);
       //             break;
       //         case "Transparent":
       //             MaxColorCode = new SolidColorBrush(Colors.Transparent);
       //             break;
       //         case "White":
       //             MaxColorCode = new SolidColorBrush(Colors.White);
       //             break;
       //         case "Yellow":
       //             MaxColorCode = new SolidColorBrush(Colors.Yellow);
       //             break;
       //     }
       //     txtMaxValue.Background = MaxColorCode;
       //     #endregion
       // }
        #endregion
    }
}
