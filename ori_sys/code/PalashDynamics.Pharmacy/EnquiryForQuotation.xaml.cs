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

namespace PalashDynamics.Pharmacy
{
    public partial class EnquiryForQuotation : UserControl
    {
        
     
        public EnquiryForQuotation()
        {
            InitializeComponent();
           
        }

       

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Supplier obj = new Supplier();

            lstSuppName.ItemsSource = obj.GetListOfSupp();
        }
    }
    public class Supplier
    {
        public string supname { get; set; }
        public List<Supplier> QtnEnq { get; set; }
        public List<Supplier> GetListOfSupp()
        {
            List<Supplier> retList = new List<Supplier>();
            Supplier newobj = null;
            retList.Clear();
            newobj = new Supplier();
            newobj.supname = "ABC";
            retList.Add(newobj);
            return retList;
        }
    }
}
