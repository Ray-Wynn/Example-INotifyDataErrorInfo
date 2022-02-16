# Example-INotifyDataErrorInfo
Demonstrates INotifyDataErrorInfo implementation using DataGrid and TextBox binding.

The Data Raw View demonstrates the rogue exclamation mark.

Microsoft remarks

This interface enables data entity classes to implement custom validation rules and expose validation results asynchronously. 
This interface also supports custom error objects, multiple errors per property, cross-property errors, and entity-level errors. 
Cross-property errors are errors that affect multiple properties. 
You can associate these errors with one or all of the affected properties, or you can treat them as entity-level errors. 
Entity-level errors are errors that either affect multiple properties or affect the entire entity without affecting a particular property.

ValidatesOnNotifyDataErrors
	When ValidatesOnNotifyDataErrors is true, the binding checks for and reports errors that are raised by a data source that implements INotifyDataErrorInfo.

Code derived from https://kmatyaszek.github.io/wpf%20validation/2019/03/13/wpf-validation-using-inotifydataerrorinfo.html

And below snippet of code shows you how you can show to user multiple errors to single property: unfortunately this smears across controls below.
kmatyaszek
    <TextBox Text="{Binding UserName, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged, ValidatesOnNotifyDataErrors=True}">
        <Validation.ErrorTemplate>
            <ControlTemplate>
                <StackPanel>
                    <AdornedElementPlaceholder x:Name="textBox" />
                    <ItemsControl ItemsSource="{Binding}">
                        <ItemsControl.ItemTemplate>
                            <DataTemplate>
                                <TextBlock Text="{Binding ErrorContent}" Foreground="Red" />
                            </DataTemplate>
                        </ItemsControl.ItemTemplate>
                    </ItemsControl>
                </StackPanel>
            </ControlTemplate>
        </Validation.ErrorTemplate>
    </TextBox>

Derived code to display multiple errors in <DataGrid.RowDetailsTemplate>

    <DataGrid.RowDetailsTemplate>
        <DataTemplate>
            <StackPanel>
                <ItemsControl ItemsSource="{Binding RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type DataGridRow}}, Path=(Validation.Errors)}">
                    <ItemsControl.ItemTemplate>
                        <DataTemplate>
                            <TextBlock Text="{Binding ErrorContent}" Foreground="Red" Padding="4,0,4,2"/>
                        </DataTemplate>
                    </ItemsControl.ItemTemplate>
                </ItemsControl>
            </StackPanel>
        </DataTemplate>
    </DataGrid.RowDetailsTemplate>
