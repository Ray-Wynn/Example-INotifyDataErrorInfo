# Example-INotifyDataErrorInfo
Demonstrates INotifyDataErrorInfo implementation using DataGrid and TextBox binding.
Code derived from https://kmatyaszek.github.io/wpf%20validation/2019/03/13/wpf-validation-using-inotifydataerrorinfo.html
The Data Raw View demonstrates the rogue exclamation mark.

Microsoft remarks

This interface enables data entity classes to implement custom validation rules and expose validation results asynchronously. 
This interface also supports custom error objects, multiple errors per property, cross-property errors, and entity-level errors. 
Cross-property errors are errors that affect multiple properties. 
You can associate these errors with one or all of the affected properties, or you can treat them as entity-level errors. 
Entity-level errors are errors that either affect multiple properties or affect the entire entity without affecting a particular property.

ValidatesOnNotifyDataErrors
	When ValidatesOnNotifyDataErrors is true, the binding checks for and reports errors that are raised by a data source that implements INotifyDataErrorInfo.
