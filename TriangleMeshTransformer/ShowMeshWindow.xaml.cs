using Geometry;
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

namespace TriangleMeshTransformer
{
    /// <summary>
    /// Interaction logic for ShowMeshWindow.xaml
    /// </summary>
    public partial class ShowMeshWindow : Window
    {
        Basic3DShapeExample pgCode;
        
        public ShowMeshWindow(CGeometry pGeometry)
        {
            InitializeComponent();
            pgCode = new Basic3DShapeExample(pGeometry);
            pgCode.Title = "asas";
            FmPage.Content= pgCode;
            


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
