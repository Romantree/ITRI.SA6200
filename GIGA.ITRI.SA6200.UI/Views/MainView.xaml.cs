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
using TS.FW;

namespace GIGA.ITRI.SA6200.UI.Views
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void Border3D_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (AP.IsSim == false) return;
                if (e.LeftButton == MouseButtonState.Released) return;

                this.DragMove();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
