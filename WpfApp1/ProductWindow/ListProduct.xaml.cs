using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.ProductWindow;
using WpfApp1.DB;

namespace WpfApp1.ProductWindow
{
    /// <summary>
    /// Логика взаимодействия для ListProduct.xaml
    /// </summary>
    public partial class ListProduct : Window
    {
        int pageNumber;
        public ListProduct()
        {
            InitializeComponent();
            ListProd.ItemsSource = MainWindow.buffPaper.Product.ToList();
            FellCombBox();
            ImageNull();
            FilterCB.ItemsSource = MainWindow.buffPaper.ProductType.ToList();
            FilterCB.DisplayMemberPath = "Title";
            RefreshPagination();
            RefreshButtons();           
            SortInfo();
        }

        private void RefreshPagination()
        {
            ListProd.ItemsSource = null;
            if (SortCB.Text != null)
            {
                SortInfo();
            }
            else
            {
                ListProd.ItemsSource = MainWindow.buffPaper.Product.OrderBy(x => x.Title).Skip(pageNumber * 20).Take(20).ToList();
            }
        }
        private void BLeft_Click(object sender, RoutedEventArgs e)
        {
            if (pageNumber == 0)
                return;
            pageNumber--;
            RefreshPagination();
        }

        private void BRight_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.buffPaper.Product.ToList().Count % 20 == 0)
            {
                if (pageNumber == (MainWindow.buffPaper.Product.ToList().Count / 20) - 1)
                    return;
            }
            else
            {
                if (pageNumber == (MainWindow.buffPaper.Product.ToList().Count / 20))
                    return;
            }
            pageNumber++;
            RefreshPagination();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            pageNumber = Convert.ToInt32(button.Content) - 1;
            RefreshPagination();
        }

        private void RefreshButtons()
        {
            WPButtons.Children.Clear();
            if (MainWindow.buffPaper.Product.ToList().Count % 40 == 0)
            {
                for (int i = 0; i < MainWindow.buffPaper.Product.ToList().Count / 20; i++)
                {
                    Button button = new Button();
                    button.Content = i + 1;
                    button.Click += Button_Click;
                    button.Margin = new Thickness(5);
                    button.Width = 20;
                    button.Height = 20;
                    button.FontSize = 14;
                    WPButtons.Children.Add(button);
                }
            }
            else
            {
                for (int i = 0; i < MainWindow.buffPaper.Product.ToList().Count / 20 + 1; i++)
                {
                    Button button = new Button();
                    button.Content = i + 1;
                    button.Click += Button_Click;
                    button.Margin = new Thickness(5);
                    button.Width = 20;
                    button.Height = 20;
                    button.FontSize = 14;
                    WPButtons.Children.Add(button);
                }
            }
        }
        private void ImageNull()
        {
            foreach (var item in MainWindow.buffPaper.Product)
            {
                if (item.Image == null)
                    item.Image = @"\products\picture.png";
            }
            MainWindow.buffPaper.SaveChanges();
        }
        private void Poisk_TextChanged(object sender, TextChangedEventArgs e)
        {
            var res = MainWindow.buffPaper.Product.ToList();
            res = res.Where(x => x.Title.ToLower().Contains(Poisk.Text.ToLower())).ToList();
            ListProd.ItemsSource = res.ToList();
        }
        private void FellCombBox()
        {
            SortCB.Items.Add("Наименование от А до Я");
            SortCB.Items.Add("Наименование от Я до А");
        }
        private void SortInfo()
        {
            switch (SortCB.SelectedItem)
            {
                case "Наименование от А до Я":
                    ListProd.ItemsSource = null;
                    ListProd.ItemsSource = MainWindow.buffPaper.Product.OrderBy(x => x.Title).ToList();
                    break;
                case "Наименование от Я до А":
                    ListProd.ItemsSource = null;
                    ListProd.ItemsSource = MainWindow.buffPaper.Product.OrderByDescending(x => x.Title).ToList();
                    break;
                default:
                    ListProd.ItemsSource = null;
                    ListProd.ItemsSource = MainWindow.buffPaper.Product.ToList();
                    break;
            }
        }
        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortInfo();
        }
        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedType = FilterCB.SelectedItem;
            var type = ((ProductType)selectedType).ID;
            ListProd.ItemsSource = MainWindow.buffPaper.Product.Where(x => x.ProductTypeID == type).ToList();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //WinAdd winAdd = new WinAdd();
            //winAdd.Show();
        }
        private void ListProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = MessageBox.Show("Вы точно хотите удалить элемент?", "Удаление элемента", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (MessageBoxResult.Yes == a)
            {
                try
                {
                    var item = ListProd.SelectedItem as Product;
                    MainWindow.buffPaper.ProductCostHistory.RemoveRange(MainWindow.buffPaper.ProductCostHistory.Where(x => x.ProductID == item.ID).ToList());
                    MainWindow.buffPaper.ProductMaterial.RemoveRange(MainWindow.buffPaper.ProductMaterial.Where(x => x.ProductID == item.ID).ToList());
                    MainWindow.buffPaper.ProductSale.RemoveRange(MainWindow.buffPaper.ProductSale.Where(x => x.ProductID == item.ID).ToList());
                    MainWindow.buffPaper.Product.Remove(item);
                    MainWindow.buffPaper.SaveChanges();
                    MessageBox.Show("Успешное удаление!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex}");
                }
            }
        }
    }
}
