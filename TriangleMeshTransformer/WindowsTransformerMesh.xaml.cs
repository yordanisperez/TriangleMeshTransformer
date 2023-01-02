using g3;
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
    /// Interaction logic for WindowsTransformerMesh.xaml
    /// </summary>
    public partial class WindowsTransformerMesh : Window
    {
        private readonly CGeometry geometry;
        public ICallMethodStatic staticCall;
        public DMesh3 meshTranf=null;
        public WindowsTransformerMesh(CGeometry pGeometry)
        {
            geometry =(CGeometry) pGeometry.Clone();
            InitializeComponent();
            staticCall = new CallMethodStatic();
        }

        private void TBNumberCell_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (Int32.TryParse(textBox.Text, out _) == false)
            {
                TextChange textChange = e.Changes.ElementAt<TextChange>(0);
                int iAddedLength = textChange.AddedLength;
                int iOffset = textChange.Offset;

                textBox.Text = textBox.Text.Remove(iOffset, iAddedLength);
            }
        }

        private void BTTranformer_Click(object sender, RoutedEventArgs e)
        {
            if (geometry == null)
            {
                DialogResult = false;
                Close();
            }
            int numbCells;
            int numbCubes;
             try {
                numbCells = Int32.Parse(tbNumberCell.Text);
                numbCubes = Int32.Parse(tbCubeNumber.Text);
            }
            catch (FormatException)
            {
                staticCall.Show(string.Format("{0} , {1}: Bad Format", tbNumberCell.Text, tbCubeNumber.Text), "Tranform Mesh", MessageBoxButton.OK);
                return;
            }
            catch (OverflowException)
            {
                staticCall.Show(string.Format("{0} , {1}: Overflow", tbNumberCell.Text, tbCubeNumber.Text), "Tranform Mesh", MessageBoxButton.OK);
                return;
             
            }
            catch (ArgumentNullException)
            {
                staticCall.Show(string.Format("{0} , {1}: Empty Params", tbNumberCell.Text, tbCubeNumber.Text), "Add Mesh", MessageBoxButton.OK);
                return;
            }

            meshTranf= geometry.mergingMeshesSDF(numbCells, numbCubes);
            DialogResult = true;
            Close();
        }
    }
}
