using System.ComponentModel;
using System;
using System.Net;
using System.Globalization;
using System.Windows.Resources;
using System.Windows;
using System.IO;
using System.Threading;
using System.Xml;

namespace PalashDynamics
{
    public class ObservableResources<T> : INotifyPropertyChanged
    {
        public static T _resources;

        public T LocalizationResources
        {
            get { return _resources; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableResources(T resources)
        {
            _resources = resources;
        }

        public void UpdateBindings()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("LocalizationResources"));
        }
    }
}
