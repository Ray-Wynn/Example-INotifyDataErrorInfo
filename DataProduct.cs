﻿using System;
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
    public class DataProducts : ObservableCollection<DataProduct>
    {

    }

    public class DataProduct : INotifyPropertyChanged, INotifyDataErrorInfo
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

        internal struct Products
        {
            internal string Product { get; set; }
            internal int Stock { get; set; }           
        }

        Products current;

        public string Product
        {
            get { return current.Product; }
            set
            {
                if (current.Product != value)
                {
                    current.Product = value;
                    ValidateProduct();
                    OnPropertyChanged();
                }
            }
        }

        private void ValidateProduct()
        {
            ClearErrors(nameof(Product));

            if (string.IsNullOrEmpty(Product))
            {
                AddError(nameof(Product), "Product name required");
            }
        }

        public int Stock
        {
            get { return current.Stock; }
            set
            {
                if (current.Stock != value)
                {
                    current.Stock = value;
                    ValidateStock();
                    OnErrorsChanged();
                }
            }
        }

        private void ValidateStock()
        {
            ClearErrors(nameof(Stock));

            if (Stock < 0)
            {
                AddError(nameof(Stock), "Stock error!");
            }
        }
    }
}