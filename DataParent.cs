using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Example_INotifyDataErrorInfo
{
    public class DataItems : ObservableCollection<DataItem>
    {

    }

    public class DataItem : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged
        /// <summary>
        /// Notifies property binding of property change
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        { // Raise PropertyChanged
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyDataErrorInfo
        /// <summary>
        /// Notifies property binding of property errors
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        protected void OnErrorsChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(nameof(HasErrors)); // Update HasErrors
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private readonly Dictionary<string, List<string>> errorsByPropertyName = new();

        public bool HasErrors
        {
            get { return errorsByPropertyName.Count > 0; }
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && errorsByPropertyName.ContainsKey(propertyName))
            {
                return errorsByPropertyName[propertyName];
            }

            return new List<string>();
        }

        private void AddError(string propertyName, string error)
        {
            if (!errorsByPropertyName.ContainsKey(propertyName))
            {
                errorsByPropertyName[propertyName] = new List<string>();
            }

            if (!errorsByPropertyName[propertyName].Contains(error))
            {
                errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (errorsByPropertyName.ContainsKey(propertyName))
            {
                errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        #endregion

        internal struct Parent
        {
            internal string ParentName { get; set; }
            internal int ParentNumber { get; set; }
            internal ObservableCollection<DataChildren> Children { get; set; }
        }

        Parent current;

        public string ParentName
        {
            get { return current.ParentName; }
            set
            {
                if (current.ParentName != value)
                {
                    current.ParentName = value;
                    ValidateParentName();
                    OnPropertyChanged();
                }
            }
        }

        private void ValidateParentName()
        {
            ClearErrors(nameof(ParentName));

            if (string.IsNullOrEmpty(ParentName))
            {
                AddError(nameof(ParentName), "Parent name cannot be empty");
            }
        }

        public int ParentNumber
        {
            get { return current.ParentNumber; }
            set
            {
                if (current.ParentNumber != value)
                {
                    current.ParentNumber = value;
                    ValidateParentNumber();
                    OnErrorsChanged();
                }
            }
        }

        private void ValidateParentNumber()
        {
            ClearErrors(nameof(ParentNumber));

            if (ParentNumber <= 0)
            {
                AddError(nameof(ParentNumber), "Child number must be > 0");
            }
        }

        public ObservableCollection<DataChildren> Children
        {
            get { return current.Children; }
            set
            {
                if (current.Children != value)
                {
                    current.Children = value;
                    ValidateChildren();
                    OnPropertyChanged();
                }
            }
        }

        private void ValidateChildren()
        {
            foreach (var child in Children)
            {
                ClearErrors(nameof(child.ChildName));

                if (string.IsNullOrEmpty(child.ChildName))
                {
                    AddError(nameof(child.ChildName), "Child name cannot be empty");
                }

                if (child.ChildNumber <= 0)
                {
                    AddError(nameof(child.ChildNumber), "Child number must be > 0");
                }
            }
        }
    }
}
