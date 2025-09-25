using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class GroceryListItemsView : ContentPage
{
    public GroceryListItemsView(GroceryListItemsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is GroceryListItemsViewModel vm)
        {
            vm.SearchCommand.Execute(e.NewTextValue);
        }
    }
}