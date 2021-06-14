using System;
using System.IO;
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
using Класс_шахмат;

namespace WPF_Chess
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] images;
        private Gui gui;
        private Класс_шахмат.Point from;
        private Класс_шахмат.Point to;
        private readonly string path = Environment.CurrentDirectory;
        public MainWindow()
        {
            InitializeComponent();
            imgBoard.ImageSource = BitmapFrame.Create(new Uri(path + "/images/background.png"));
        }
        private void Move(object sender, MouseEventArgs e)
        {
            gui.SelectFigure(gui.GetCords(e.GetPosition(canvas)));
        }
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            borderBoard.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Hidden;
            gui = new Gui(canvas);
            gui.ArrangeFiguresForEnum();
            gui.ChildrenAdd();
        }
        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SettingsGame_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
        }
    }
}
