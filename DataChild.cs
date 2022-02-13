using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Example_INotifyDataErrorInfo
{
    public class DataChildren : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyDataErrorInfo
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        protected void OnErrorsChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(nameof(HasErrors));
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

        internal struct Child
        {
            internal string ChildName { get; set; }
            internal int ChildNumber { get; set; }
        }

        Child current;

        public string ChildName
        {
            get { return current.ChildName; }
            set
            {
                if (current.ChildName != value)
                {
                    current.ChildName = value;
                    ValidateChildName();
                    OnPropertyChanged();
                }
            }
        }

        private void ValidateChildName()
        {
            ClearErrors(nameof(ChildName));

            if (string.IsNullOrEmpty(ChildName))
            {
                AddError(nameof(ChildName), "Child name cannot be empty");
            }
        }

        public int ChildNumber
        {
            get { return current.ChildNumber; }
            set
            {
                if (current.ChildNumber != value)
                {
                    current.ChildNumber = value;
                    ValidateChildNumber();
                    OnPropertyChanged();
                }
            }
        }

        private void ValidateChildNumber()
        {
            ClearErrors(nameof(ChildNumber));

            if (ChildNumber <= 0)
            {
                AddError(nameof(ChildNumber), "Child number must be > 0");
            }
        }        
    }
}
