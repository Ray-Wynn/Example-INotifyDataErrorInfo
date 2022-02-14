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

        public DataChildren(DataProduct parent)
        {
            Parent = parent;
        }

        internal struct Child
        {
            internal DataProduct Parent {get; set; }
            internal string ChildName { get; set; }
            internal int ChildAge { get; set; }
        }

        Child current;

        public DataProduct Parent
        {
            get { return current.Parent; }
            set
            {
                if(current.Parent != value)
                {
                    current.Parent = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public int ChildAge
        {
            get { return current.ChildAge; }
            set
            {
                if (current.ChildAge != value)
                {
                    current.ChildAge = value;
                    ValidateChildAge();
                    OnPropertyChanged();
                }
            }
        }

        private void ValidateChildAge()
        {
            ClearErrors(nameof(ChildAge));

            if (ChildAge > Parent.ProductPrice - 16)
            {
                AddError(nameof(ChildAge), "Child's age must be greater than parent's");
            }
        }        
    }
}
