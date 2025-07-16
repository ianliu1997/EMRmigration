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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Collections.ObjectModel;

namespace PalashDynamics.IVF
{
    public partial class Lab3Detail : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public clsFemaleLabDay2CalculateDetailsVO Details { get; set; }
        public ObservableCollection<clsFemaleLabDay2CalculateDetailsVO> GetDetails { get; set; }

        public Lab3Detail()
        {
            InitializeComponent();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Details == null)
            {
                Details = new clsFemaleLabDay2CalculateDetailsVO();
            }
            else
            {
                GetCalcDetails();
            }
        }

        private void cmdCalculate_Click(object sender, RoutedEventArgs e)
        {            
            if (chkUniForm.IsChecked == true)
                Details.ZonaThickness = 1;
            else
                Details.ZonaThickness = 2;
            if (chkNormalThickness.IsChecked == true)
                Details.ZonaTexture = 1;
            else
                Details.ZonaTexture = 2;
            if (chkEqual.IsChecked == true)
                Details.BlastomereSize = 1;
            else
                Details.BlastomereSize = 2;
            if (chkRegular.IsChecked == true)
                Details.BlastomereShape = 1;
            else
                Details.BlastomereShape = 2;
            if (chkFilledZona.IsChecked == true)
                Details.BlastomeresVolume = 1;
            else
                Details.BlastomeresVolume = 2;
            if (chkSmooth.IsChecked == true)
                Details.Membrane = 1;
            else
                Details.Membrane = 2;
            if (chkClear.IsChecked == true)
                Details.Cytoplasm = 1;
            else if (chkGranular.IsChecked == true)
                Details.Cytoplasm = 2;
            else
                Details.Cytoplasm = 3;
            if (chk10.IsChecked == true)
                Details.Fragmentation = 1;
            else if (chk10to30.IsChecked == true)
                Details.Fragmentation = 2;
            else if (chk30.IsChecked == true)
                Details.Fragmentation = 3;
            if (chk6Cell.IsChecked == true)
                Details.DevelopmentRate = 1;
            else if (chk7Cell.IsChecked == true)
                Details.DevelopmentRate = 2;
            else if (chk8Cell.IsChecked == true)
                Details.DevelopmentRate = 3;
            else if (chkMoreThan8Cell.IsChecked == true)
                Details.DevelopmentRate = 4;
            else if (chkLessThan6Cell.IsChecked == true)
                Details.DevelopmentRate = 5;
            else
                Details.DevelopmentRate = 6;
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void GetCalcDetails()
        {
            var item = Details;
            if (item.ZonaThickness == 1)
                chkUniForm.IsChecked = true;
            else
                chkVariable.IsChecked = true;
            if (item.ZonaTexture == 1)
                chkNormalThickness.IsChecked = true;
            else
                chkVeryThick.IsChecked = true;
            if (item.BlastomereSize == 1)
                chkEqual.IsChecked = true;
            else
                chkUnEqual.IsChecked = true;
            if (item.BlastomereShape == 1)
                chkRegular.IsChecked = true;
            else
                chkIrRegular.IsChecked = true;
            if (item.BlastomeresVolume == 1)
                chkFilledZona.IsChecked = true;
            else
                chkSpaceZona.IsChecked = true;
            if (item.Membrane == 1)
                chkSmooth.IsChecked = true;
            else
                chkJagged.IsChecked = true;
            if (item.Cytoplasm == 1)
                chkClear.IsChecked = true;
            else if (item.Cytoplasm == 2)
                chkGranular.IsChecked = true;
            else
                chkVacuoles.IsChecked = true;
            if (item.Fragmentation == 1)
                chk10.IsChecked = true;
            else if (item.Fragmentation == 2)
                chk10to30.IsChecked = true;
            else
                chk30.IsChecked = true;
            if (item.DevelopmentRate == 1)
                chk6Cell.IsChecked = true;
            else if (item.DevelopmentRate == 2)
                chk7Cell.IsChecked = true;
            else if (item.DevelopmentRate == 3)
                chk8Cell.IsChecked = true;
            else if (item.DevelopmentRate == 4)
                chkMoreThan8Cell.IsChecked = true;
            else
                chkLessThan6Cell.IsChecked = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
      
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}
