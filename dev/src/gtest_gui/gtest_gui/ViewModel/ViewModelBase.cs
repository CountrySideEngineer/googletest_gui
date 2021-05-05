using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace gtest_gui.ViewModel
{
    /// <summary>
    /// Common base class for all ViewModel class.
    /// Inherits and implements INotifyPropertyChanged interface.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify property changed.
        /// </summary>
        /// <param name="propertyName">Name property whose value changed.</param>
        protected virtual void RaisePropertyChanged(String propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
