using GIGA.ITRI.SA6200.UI.Models.Setup;
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

namespace GIGA.ITRI.SA6200.UI.Views.Page.Setup
{
    /// <summary>
    /// SetupInOutView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SetupInOutView : UserControl
    {
        public SetupInOutView()
        {
            InitializeComponent();
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (AP.IsSim == false) return;

                var model = (sender as Ellipse).DataContext as InOutModel;
                if (model == null || string.IsNullOrEmpty(model.Name)) return;

                AP.IO.WriteX(!model.OnOff, model.Key);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
