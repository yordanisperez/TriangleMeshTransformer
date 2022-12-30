using g3;
using Geometry;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
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

namespace TriangleMeshTransformer
{
    /// <summary>
    /// This class is for can mock to OpenDialog from MSTest
    /// </summary>
    public interface IOpenFileDialog
    {
        string Filter { get; set; }
        bool? ShowDialog();
        string FileName { get; set; }
        string InitialDirectory { get; set; }
    }
    /// <summary>
    /// Class container wrapper for OpenFileDialog need for testing mock MSTest
    /// </summary>
    public class OpenFileDialogContainer : IOpenFileDialog
    {
       public OpenFileDialog openFileDialog { get; set; }
        public string Filter
        {
            get => openFileDialog.Filter; set
            {
                openFileDialog.Filter =value;
            }
        }
        public string FileName { 
            get => openFileDialog.FileName; set  {
                openFileDialog.FileName = value;
            } 
        }
        public string InitialDirectory { 
            get => openFileDialog.InitialDirectory; set {
               openFileDialog.InitialDirectory=value;
            }
        }

        public OpenFileDialogContainer(OpenFileDialog pOpenfile)
        {
            openFileDialog=pOpenfile;
        }

        public bool? ShowDialog()
        {
            return openFileDialog.ShowDialog();
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      private TriangleMeshManager managerMesh =new TriangleMeshManager();
      public  string filePathOpen=@"C:\";
      public IOpenFileDialog openFileDialog;
        public MainWindow()
        {
            InitializeComponent();
            // Create an instance of the OpenFileDialog
            openFileDialog = new OpenFileDialogContainer(new OpenFileDialog()) ;

        }

        public void miOpen_Click(object sender, RoutedEventArgs e)
        {

            // Set the filter for the file extension
            openFileDialog.Filter = "Text files (*.stl)|*.stl";

            // Find the position of the delimiter in the original string
            int index = filePathOpen.IndexOf(@"\");
            // Extract the substring before the delimiter
            // Set the initial directory
            openFileDialog.InitialDirectory = filePathOpen.Substring(0, index);

            // Show the OpenFileDialog and get the result
            bool? result = openFileDialog.ShowDialog();

            // If the user selected a file and clicked "Open"
            if (result == true)
            {
                // Get the path of the selected file
                filePathOpen = openFileDialog.FileName;
                //LoadingFile and create Structure mesh
                // MessageBox.Show(filePathOpen);
                addMesh(filePathOpen);
            }
        }

        public void addMesh(string pPath)
        {
            //If pPath  is null or empty
            if (string.IsNullOrEmpty(pPath))
            {
                MessageBox.Show("You shold select a valid file");
                return;
            }
            //if pPath is in diccionary 
            if (managerMesh.getSimpleMesh(pPath) != null)
            {
                MessageBox.Show("The file select is loading");
                return;
            }
            //Loading mesh
            DMesh3 mesh = StandardMeshReader.ReadMesh(pPath);
            if (!managerMesh.AddTrianglesMesh(pPath, new CGeometry(mesh)))
                return;


            //Creation of component
            Label lbWrapper = new Label
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
            };
            StackPanel spWrapper = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };
            TextBlock tbNameFile = new TextBlock
            {
                Width = 130,
                TextWrapping= TextWrapping.NoWrap,
                TextTrimming=TextTrimming.CharacterEllipsis,
                FontWeight=FontWeights.DemiBold,
                FontSize=14,
                VerticalAlignment=VerticalAlignment.Center,
                TextAlignment=TextAlignment.Justify,
                Text= pPath,
            };

            Label lbBtns = new Label
            {
                Width = 150,
                HorizontalContentAlignment = HorizontalAlignment.Right,
            };
            WrapPanel wrapBtns = new WrapPanel();

            Button btnShow = new Button {
                Tag = pPath,
            };
            Button btnTranf = new Button
            {
                Tag = pPath,
            };
            Button btnDelete = new Button
            {
                Tag = pPath,
            };
            Button btnSave = new Button()
            {
                Tag = pPath,
            };

            BitmapImage bitmapShow = new BitmapImage();
            bitmapShow.BeginInit();
            bitmapShow.UriSource = new Uri("https://pic.onlinewebfonts.com/svg/img_416768.png");
            bitmapShow.EndInit();
            Image imgShow = new Image
            {
                Width = 20,
                Source = bitmapShow,
            };

            BitmapImage bitmapTranf  = new BitmapImage();
            bitmapTranf.BeginInit();
            bitmapTranf.UriSource = new Uri("https://pic.onlinewebfonts.com/svg/img_335840.png");
            bitmapTranf.EndInit();
            Image imgTranf = new Image
            {
                Width = 20,
                Source = bitmapTranf,
            };

            BitmapImage bitmapDel = new BitmapImage();
            bitmapDel.BeginInit();
            bitmapDel.UriSource = new Uri("https://pic.onlinewebfonts.com/svg/img_659.png");
            bitmapDel.EndInit();
            Image imgDel = new Image
            {
                Width = 20,
                Source = bitmapDel,
            };

            BitmapImage bitmapSave = new BitmapImage();
            bitmapSave.BeginInit();
            bitmapSave.UriSource = new Uri("https://pic.onlinewebfonts.com/svg/img_115260.png");
            bitmapSave.EndInit();
            Image imgSave = new Image
            {
                Width = 20,
                Source = bitmapSave,
            };


            //Nesting component
            btnShow.Content = imgShow;
            btnDelete.Content = imgDel;
            btnSave.Content = imgSave;
            btnTranf.Content = imgTranf;

            wrapBtns.Children.Add(btnShow);
            wrapBtns.Children.Add(btnTranf);
            wrapBtns.Children.Add(btnDelete);
            wrapBtns.Children.Add(btnSave);

            lbBtns.Content = wrapBtns;
            spWrapper.Children.Add(tbNameFile);
            spWrapper.Children.Add(lbBtns);
            lbWrapper.Content = spWrapper;
            spMesh.Children.Add(lbWrapper);

            /*< Label BorderBrush = "Gray" BorderThickness = "1" >
               < StackPanel Orientation = "Horizontal" >
                   < TextBlock Width = "130" TextWrapping = "NoWrap" TextTrimming = "CharacterEllipsis" FontWeight = "DemiBold" FontSize = "14" VerticalAlignment = "Center" TextAlignment = "Justify" > Esta es mi promera lista</ TextBlock >
                   < Label  Width = "150" HorizontalContentAlignment = "Right" >
                       < WrapPanel >
                           < Button >
                               < Image Width = "20" Source = "https://pic.onlinewebfonts.com/svg/img_416768.png" ></ Image >
                           </ Button >


                           < Button >
                               < Image Width = "20" Source = "https://pic.onlinewebfonts.com/svg/img_335840.png" ></ Image >
                           </ Button >


                           < Button >
                               < Image Width = "20" Source = "https://pic.onlinewebfonts.com/svg/img_659.png" ></ Image >
                           </ Button >
                           < Button >
                               < Image Width = "20" Source = "https://pic.onlinewebfonts.com/svg/img_115260.png" ></ Image >
                           </ Button >
                       </ WrapPanel >
                   </ Label >

               </ StackPanel >
           </ Label >*/

        }
    }
}
