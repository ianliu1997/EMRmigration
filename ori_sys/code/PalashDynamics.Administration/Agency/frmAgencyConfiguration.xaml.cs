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
using CIMS;
using System.Reflection;

namespace PalashDynamics.Administration.Agency
{
    public partial class frmAgencyConfiguration : UserControl
    {
        #region Class Constructor
        public frmAgencyConfiguration()
        {
            InitializeComponent();
            TreeView1.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(TreeView1_SelectedItemChanged);
        }
        #endregion

        #region Selection Changed Evets
        void TreeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView1.CollapseAllButSelectedPath();
            TreeView1.ExpandSelectedPath();
        }

        public string Action { get; set; }
        private void CreatedForm_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton HB = (HyperlinkButton)e.OriginalSource;
            if (HB.Tag != null)
            {
                Action = HB.Tag.ToString();
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = HB.Content.ToString();
                UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance(Action) as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(mydata);
            }
        }
        #endregion
    }
}
