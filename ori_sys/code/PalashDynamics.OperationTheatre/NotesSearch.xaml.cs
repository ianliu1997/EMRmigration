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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using CIMS;

namespace PalashDynamics.OperationTheatre
{
    public partial class NotesSearch : ChildWindow
    {
        public event RoutedEventHandler OnAddButton_Click;
        public string labelOfdesc { get; set; }
        public Notes UserSelectionNotes { get; set; }
        public List<MasterListItem> NotesList = new List<MasterListItem>();
        public enum Notes
        {
            Anestehsia, Surgery

        };

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        public NotesSearch()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Fetches Notes(Anesthesia/Surg)
        /// </summary>
        private void FetchData()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                if(UserSelectionNotes==Notes.Anestehsia)
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_AnesthesiaNotes;
                else
                    BizAction.MasterTable = MasterTableNameList.M_SurgeryNotes;

                if( txtSearch.Text!="")
                 BizAction.Parent = new KeyValue { Value = "Description", Key = txtSearch.Text };
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
                        dgNotess.ItemsSource = null;
                        dgNotess.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        NotesList = ((clsGetMasterListBizActionVO)e.Result).MasterList;



                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void chkStatus_Checked(object sender, RoutedEventArgs e)
        {
            if (dgNotess.SelectedItem != null)
                ((MasterListItem)dgNotess.SelectedItem).Status = true;
        }

        private void chkStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dgNotess.SelectedItem != null)
                ((MasterListItem)dgNotess.SelectedItem).Status = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OnAddButton_Click != null)
                {
                    this.DialogResult = true;
                    OnAddButton_Click(this, new RoutedEventArgs());

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

