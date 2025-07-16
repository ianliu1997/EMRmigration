
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;


namespace PalashDynamics.Pharmacy.ViewModels
{
    
        public class ViewModelBase : INotifyPropertyChanged
        {

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            protected bool DelayNotification { get; set; }

            protected virtual void OnPropertyChanged(string propertyName)
            {
                if (DelayNotification) return;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            protected virtual void RaisePropertyChanged(string propertyName)
            {
                OnPropertyChanged(propertyName);
            }

            #endregion

        }
   

}
