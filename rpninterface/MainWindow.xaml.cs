using RpnLogic;
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

namespace rpninterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCalculate(object sender, RoutedEventArgs e)
        {
            string input = tbInput.Text;
            //RpnCalculator rpn = new RpnCalculator(input);
            if(tbInputX.Text == string.Empty)
            {
                double result = new RpnCalculator(input).Result;
                lblOutput.Content = result;
            }
            else
            {
                int inputX = int.Parse(tbInputX.Text);
                double result = new RpnCalculator(input, inputX).Result;
                lblOutput.Content = result;
            }
        }

        private void tbInputX_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
