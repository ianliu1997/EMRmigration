using System;
using System.ComponentModel;

namespace PalashDynamics.Collections
{
    /// <summary>
    /// Refresh Event Arguments, provides indication of need for data refresh
    /// </summary>
    public class RefreshEventArgs : EventArgs
    {
        public SortDescriptionCollection SortDescriptions { get; set; }
    }

}
