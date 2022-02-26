# Example-INotifyDataErrorInfo
Demonstrates INotifyDataErrorInfo implementation using DataGrid and TextBox binding.
[Example project](https://github.com/Ray-Wynn/Example-INotifyDataErrorInfo)




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
