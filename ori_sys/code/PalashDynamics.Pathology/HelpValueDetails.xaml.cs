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
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;

namespace PalashDynamics.Pathology
{
    public partial class HelpValueDetails : ChildWindow
    {
        #region Newly addded

        public bool IsNotFirstLevel;
        public bool IsFromAuthorization;
        public List<clsPathoTestParameterVO> NormalRange { get; set; }

        #endregion

        public HelpValueDetails()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler OnSaveButton_Click;
        public List<MasterListItem> HelpValueList = new List<MasterListItem>();

        private ObservableCollection<clsPathoTestParameterVO> _selectedItems;
        public ObservableCollection<clsPathoTestParameterVO> SelectedItems { get { return _selectedItems; } }
        public event RoutedEventHandler onOKButton_Click;

        public long ParameterID { get; set; }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _selectedItems = new ObservableCollection<clsPathoTestParameterVO>();
            if (IsFromAuthorization == true)
            {
                cmdSave.Visibility = Visibility.Collapsed;
            }
            else
            {
                cmdSave.Visibility = Visibility.Visible;
            }

            FillHelpValueData();
        }

        

        private void FillHelpValueData()
        {
            try
            {
                clsGetHelpValuesFroResultEntryBizActionVO BizAction = new clsGetHelpValuesFroResultEntryBizActionVO();
                BizAction.HelpValueList = new List<clsPathoTestParameterVO>();
                BizAction.ParameterID = ParameterID;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                       if(((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result !=null))
                       {
                        if (((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList != null)
                        {
                            dgHelpValueList.ItemsSource = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList;
                            NormalRange = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList; //Newet added

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
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                _selectedItems.Add((clsPathoTestParameterVO)dgHelpValueList.SelectedItem);
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void HelpValueDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgHelpValueList.SelectedItem != null)
            {
                if (_selectedItems == null)

                    _selectedItems = new ObservableCollection<clsPathoTestParameterVO>();



                CheckBox obj = (CheckBox)sender;
                if (obj != null)
                {
                    if (obj.IsChecked == true)
                        _selectedItems.Add((clsPathoTestParameterVO)dgHelpValueList.SelectedItem);
                    else
                        _selectedItems.Remove((clsPathoTestParameterVO)dgHelpValueList.SelectedItem);




                }
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

