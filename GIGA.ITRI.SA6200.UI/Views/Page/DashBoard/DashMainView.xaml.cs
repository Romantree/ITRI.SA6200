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
using TS.FW;

namespace GIGA.ITRI.SA6200.UI.Views.Page.DashBoard
{
    /// <summary>
    /// DashMainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DashMainView : UserControl
    {
        public DashMainView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                (sender as TextBox).ScrollToEnd();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
