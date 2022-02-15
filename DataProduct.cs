using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors)); // Make HasErrors binding current.
            Debug.WriteLine("Property {0} Errors={1}", propertyName, errorsByPropertyName.Count);
        }

        private Dictionary<string, List<string>> errorsByPropertyName = new();        

        public bool HasErrors
        {
            get { return errorsByPropertyName.Count > 0; }
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && errorsByPropertyName.ContainsKey(propertyName))
            {
                IEnumerable errors = errorsByPropertyName[propertyName];
                return errors;
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

        internal struct Inventory
        { // Boilerplate ready to implement IEditableObject interface if required
            internal string Product { get; set; }
            internal int Stock { get; set; }         
        }

        Inventory current;

        public string Product
        {
            get { return current.Product; }
            set
            {       
                current.Product = value;
                ValidateProduct();
                OnPropertyChanged();             
            }
        }

        private void ValidateProduct()
        {
            ClearErrors(nameof(Product));

            if (string.IsNullOrEmpty(Product))
            {
                AddError(nameof(Product), "Product name required.");
                AddError(nameof(Product), "Product Error multiple message.");
            }
        }

        public int Stock
        {
            get { return current.Stock; }
            set
            {               
                current.Stock = value;
                ValidateStock();
                OnPropertyChanged();             
            }
        }

        private void ValidateStock()
        {
            ClearErrors(nameof(Stock));
            
            if (Stock <= 0)        
            {
                AddError(nameof(Stock), "Stock <= 0 !");
            }
        }
    }
}
