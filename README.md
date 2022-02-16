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

<Window.Resources>
        <!-- Textbox Validation Style
                Tooltip single validation error reporting -->
        <Style x:Key="Example-TextBoxValidationError" TargetType="{x:Type TextBox}">
            <Style.Triggers>
                <Trigger Property="Validation.HasError" Value="true">
                    <Setter Property="ToolTip" Value="{Binding RelativeSource={x:Static RelativeSource.Self}, Path=(Validation.Errors)/ErrorContent}"/>
                </Trigger>
            </Style.Triggers>
        </Style>
        
        <!-- DataGrid.RowValidationErrorTemplate
                Display yellow exclamation mark within a red circle on row containing validation errors. -->
        <ControlTemplate x:Key="Example-RowValidationErrorTemplate">
            <Grid Margin="0,-2,0,-2">
                <Ellipse StrokeThickness="0" Fill="Red" Width="{TemplateBinding FontSize}" Height="{TemplateBinding FontSize}" />
                <TextBlock Text="!" FontSize="{TemplateBinding FontSize}" FontWeight="Bold" Foreground="Yellow" HorizontalAlignment="Center" VerticalAlignment="Center" />
            </Grid>
        </ControlTemplate>

        <!-- DataGrid.RowDetailsTemplate
                Display one or more row validation errors in RowDetails when row is selected. -->
        <DataTemplate x:Key="Example-RowDetailsTemplate">
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
    </Window.Resources>