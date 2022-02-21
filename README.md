# Example-INotifyDataErrorInfo
Demonstrates INotifyDataErrorInfo implementation using DataGrid and TextBox binding.

The Data Raw View demonstrates the rogue exclamation mark.

Microsoft remarks

When you use the WPF data binding model, you can associate ValidationRules with your binding object. 
To create custom rules, make a subclass of this class and implement the Validate method. 
Optionally, use the built-in ExceptionValidationRule, which catches exceptions that are thrown during source updates, 
or the DataErrorValidationRule, which checks for errors raised by the IDataErrorInfo implementation of the source object.

The binding engine checks each ValidationRule that is associated with a binding every time it transfers an input value, which is the binding target property value, to the binding source property.

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
        <DataTemplate x:Key="ShowValidationErrorsInItemsControl">
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