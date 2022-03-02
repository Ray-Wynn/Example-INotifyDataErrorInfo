# Example-INotifyDataErrorInfo
Demonstrates INotifyDataErrorInfo implementation using DataGrid and TextBox binding.
[Example project](https://github.com/Ray-Wynn/Example-INotifyDataErrorInfo)

The file DataProducts.cs defines the data object DataProduct

	public class DataProduct : INotifyPropertyChanged, IDataErrorInfo

INotifyPropertyChanged provides the PropertyChanged event to provide the binding interface between class DataProduct and WPF.
 

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
	    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

The following structure implementation makes later additions to the data class, such as IEditableObject simple.
By declaring the structure and fields as internal hides the complexity and distraction.

	internal struct Inventory
	{         
	    internal string Product { get; set; }
	    internal int Stock { get; set; }         
	}

	Inventory current; // internal struct of backing field of properties

Then a properties set calls OnPropertyChanged() to update the WPF view.

	public string Product
	{
	    get { return current.Product; }
	    set
	    {
		    current.Product = value;
		  
Then the properties validation is performed.
		  
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
                AddError(nameof(Product), "Demo additional product message.");
            }

            if(Product.Length < 3)
            {
                AddError(nameof(Product), "Product name < 3 characters long.");
            }
        }

## INotifyDataErrorInfo Implementation
This interface enables data entity classes to implement custom validation rules and expose validation results asynchronously.
This interface also supports custom error objects, multiple errors per property, cross-property errors, and entity-level errors. 
Cross-property errors are errors that affect multiple properties. 
You can associate these errors with one or all of the affected properties, or you can treat them as entity-level errors. 
Entity-level errors are errors that either affect multiple properties or affect the entire entity without affecting a particular property.

**ErrorsChanged** event Occurs when the validation errors have changed for a property or for the entire entity.
        
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        protected void OnErrorsChanged([CallerMemberName] string propertyName = "")
        {            
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

*HasErrors is not typically implemented as a binding source, although in the example code the RawDataView binds to HasErrors by default.
In this case, calling PropertyChanged keeps the bonding current.*

            OnPropertyChanged(nameof(HasErrors)); // Make HasErrors binding current.            
        }        
        
**HasErrors** Gets a value that indicates whether the entity has validation errors.

        public bool HasErrors
        {
            get { return errorsByPropertyName.Count > 0; }
        }         

**GetErrors** Gets the validation errors for a specified property or for the entire entity.
The dictionary errorsByPropertyName holds the reported errors by property name.

	private Dictionary<string, List<string>> errorsByPropertyName = new(); 

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
