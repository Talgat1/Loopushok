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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.DB;
using WpfApp1.ProductWindow;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BuffPaperEntities buffPaper = new BuffPaperEntities();
        public MainWindow()
        {
            InitializeComponent();
            ListProduct prodList = new ListProduct();
            this.Close();
            prodList.Show();
           
        }

        
    }
}
