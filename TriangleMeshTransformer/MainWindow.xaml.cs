using g3;
using Geometry;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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


    internal static class ResourceAccessor
    {
        public static Uri Get(string resourcePath)
        {
            var uri = string.Format(
                "pack://application:,,,/{0};component/{1}"
                , Assembly.GetExecutingAssembly().GetName().Name
                , resourcePath
            );

            return new Uri(uri);
        }
    }
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
       public OpenFileDialog OpenFileDialog { get; set; }
        public string Filter
        {
            get => OpenFileDialog.Filter; set
            {
                OpenFileDialog.Filter =value;
            }
        }
        public string FileName { 
            get => OpenFileDialog.FileName; set  {
                OpenFileDialog.FileName = value;
            } 
        }
        public string InitialDirectory { 
            get => OpenFileDialog.InitialDirectory; set {
               OpenFileDialog.InitialDirectory=value;
            }
        }

        public OpenFileDialogContainer(OpenFileDialog pOpenfile)
        {
            OpenFileDialog=pOpenfile;
        }

        public bool? ShowDialog()
        {
            return OpenFileDialog.ShowDialog();
        }
    }

    public interface ICallMethodStatic
    {
        /// <summary>
        /// Read un file from path pFile
        /// </summary>
        /// <param name="pFile"></param>
        /// <returns></returns>
         DMesh3 ReadMesh(String pFile);
        // Summary:
        //     Displays a message box that has a message, title bar caption, and button; and
        //     that returns a result.
        //
        // Parameters:
        //   messageBoxText:
        //     A System.String that specifies the text to display.
        //
        //   caption:
        //     A System.String that specifies the title bar caption to display.
        //
        //   button:
        //     A System.Windows.MessageBoxButton value that specifies which button or buttons
        //     to display.
        //
        // Returns:
        //     A System.Windows.MessageBoxResult value that specifies which message box button
        //     is clicked by the user.        MessageBoxResult Show(string message, string caption, MessageBoxButton button);

        MessageBoxResult Show(string message, string caption, MessageBoxButton button);
    }
    /// <summary>
    /// CallMethodStatic is need for Testing the call and can mock
    /// </summary>
    /// 
    public class CallMethodStatic: ICallMethodStatic
    {
        public CallMethodStatic() { }
     public   DMesh3 ReadMesh(String pFile)
        {
            return StandardMeshReader.ReadMesh(pFile);
        }
     public MessageBoxResult Show(string message, string caption, MessageBoxButton button)
        {
            return MessageBox.Show(message, caption, button);
        }
    }

    /// <summary>
    /// filenameMesh: is name file associad to mesh
    /// parentMesh: is the object container 
    /// </summary>
    public struct ShowVisualMesh{
       public string filenameMesh;
       public Object parentMesh;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      private readonly TriangleMeshManager managerMesh =new TriangleMeshManager();
      public  string filePathOpen=@"C:\";
      public IOpenFileDialog openFileDialog;
      public ICallMethodStatic staticCall ;
        private int countMesh = 0;

        public MainWindow()
        {
            InitializeComponent();
            // Create an instance of the OpenFileDialog
            openFileDialog = new OpenFileDialogContainer(new OpenFileDialog()) ;
            staticCall = new CallMethodStatic();

        }

        /// <summary>
        /// Determines  whether the diccionary contain with pHash specified
        /// </summary>
        /// <param name="pHash"> string key of diccionary</param>
        /// <returns> Return true if pPatch in meshs diccionary, otherwise false</returns>
        public bool ExistMesh(string pHash)
        {
            return managerMesh.isPathExist(pHash);
          
        }

        public void TestMiOpen_Click(object sender, RoutedEventArgs e)
        {
            MiOpen_Click(sender, e);
        }

        public void TestDeleteMeshButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteMeshButton_Click(sender, e);
        }
        protected void DeleteMeshButton_Click(object sender, RoutedEventArgs e)
        {
            Button btnDelete=(Button) sender;
            ShowVisualMesh showComponent = (ShowVisualMesh) btnDelete.Tag;
            bool resultDelete= managerMesh.deleteTriangleMesh(showComponent.filenameMesh);
            
            if (resultDelete)
            {
                //delete the container of MainForm
                string message = string.Format("The mesh {0} deleted", showComponent.filenameMesh);
                spMesh.Children.Remove((Label)showComponent.parentMesh);
                staticCall.Show(message, "Mesh Deleted", MessageBoxButton.OK);

            }
            
        }

        protected void TranformerMeshButton_Click(object sender, RoutedEventArgs e)
        {
            Button btnTranf = (Button)sender;
            //get data of mesh 
            ShowVisualMesh showComponent = (ShowVisualMesh)btnTranf.Tag;
            
           //instance windows tranformer and pass data mesh
            WindowsTransformerMesh winTranf = new WindowsTransformerMesh(((CGeometry)managerMesh.getSimpleMesh(showComponent.filenameMesh)));
           if ((bool)winTranf.ShowDialog())
            {
                
                // Find the position of the delimiter in the original string
                int index = showComponent.filenameMesh.IndexOf(@".");
                // Extract the substring before the delimiter
                
                string nameFile = showComponent.filenameMesh.Substring(0, index);


                AddMesh(nameFile + "_" +countMesh.ToString()+".stl",false, new CGeometry(winTranf.meshTranf));
            }
        }

        protected void SaveMeshButton_Click(object sender, RoutedEventArgs e)
        {
            Button btnSave = (Button)sender;
            //get data of mesh 
            ShowVisualMesh showComponent = (ShowVisualMesh)btnSave.Tag;
            try
            {
                ((CGeometry)managerMesh.getSimpleMesh(showComponent.filenameMesh)).saveTo(showComponent.filenameMesh);

            }
            catch (Exception)
            {
                staticCall.Show("Error saving file", "Save", MessageBoxButton.OK);
                return;
            }
            btnSave.IsEnabled = false;
            
        }

        protected void ShowMeshButton_Click(object sender, RoutedEventArgs e)
        {
            staticCall.Show("Aun falta esta caracteristica", "Show Mesh", MessageBoxButton.OK);
        }

        public void AddMesh(string pPath, bool pSaveFile =true, CGeometry meshTranf=null)
        {
 
            //If pPath  is null or empty
            if (string.IsNullOrEmpty(pPath))
            {
                staticCall.Show("You should select a valid file","Add Mesh", MessageBoxButton.OK);   
                return;
            }
            //if pPath is in diccionary 
            if (managerMesh.isPathExist(pPath) ==true)
            {
                staticCall.Show("The file select is loading", "Add Mesh", MessageBoxButton.OK);
                return;
            }
            //Loading mesh
            CGeometry mesh;
            if (pSaveFile) {
                mesh = new CGeometry( staticCall.ReadMesh(pPath));
            }
            else
            {
                mesh = meshTranf;
            }
           
            if (!managerMesh.AddTrianglesMesh(pPath, mesh))
                return;

            countMesh++;
            //Creation of component
            Label lbWrapper = new Label
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
            };
            //bingMesh to set Tags each Button
            ShowVisualMesh bingMesh = new ShowVisualMesh
            {
                filenameMesh = pPath,
                parentMesh = lbWrapper,
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
                Tag = bingMesh,
                ToolTip="Show Geometry",
            };
            btnShow.Click += new RoutedEventHandler(ShowMeshButton_Click);
            Button btnTranf = new Button
            {
                Tag = bingMesh,
                ToolTip = "Merging Meshes with Signed Distance Fields",
            };
            btnTranf.Click += new RoutedEventHandler(TranformerMeshButton_Click);
            Button btnDelete = new Button
            {
                Tag = bingMesh,
                ToolTip = "Delete mesh from app",
            };
            btnDelete.Click += new RoutedEventHandler(DeleteMeshButton_Click);

            Button btnSave = new Button()
            {
                Tag = bingMesh,
                IsEnabled= !pSaveFile,
                ToolTip = "Save mesh to hard disk",
            };
            btnSave.Click += new RoutedEventHandler(SaveMeshButton_Click);

            BitmapImage bitmapShow = new BitmapImage();
            bitmapShow.BeginInit();
            bitmapShow.UriSource = ResourceAccessor.Get("images/show.png");// new Uri("https://pic.onlinewebfonts.com/svg/img_416768.png");
            bitmapShow.EndInit();
            Image imgShow = new Image
            {
                Width = 20,
                Source = bitmapShow,
            };

            BitmapImage bitmapTranf  = new BitmapImage();
            bitmapTranf.BeginInit();
            bitmapTranf.UriSource = ResourceAccessor.Get("images/transformer.png");//new Uri("https://pic.onlinewebfonts.com/svg/img_335840.png");
            bitmapTranf.EndInit();
            Image imgTranf = new Image
            {
                Width = 20,
                Source = bitmapTranf,
            };

            BitmapImage bitmapDel = new BitmapImage();
            bitmapDel.BeginInit();
            bitmapDel.UriSource = ResourceAccessor.Get("images/delete.png");//new Uri("https://pic.onlinewebfonts.com/svg/img_659.png");
            bitmapDel.EndInit();
            Image imgDel = new Image
            {
                Width = 20,
                Source = bitmapDel,
            };
           
            BitmapImage bitmapSave = new BitmapImage();
            bitmapSave.BeginInit();
            bitmapSave.UriSource = ResourceAccessor.Get("images/save.png"); //new Uri("https://pic.onlinewebfonts.com/svg/img_115260.png");
            bitmapSave.EndInit();
            //TriangleMeshTransformer
           // var uriSource = new Uri("pack://application:,,,TriangleMeshTransformer;/save.png");
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

        private void MiOpen_Click(object sender, RoutedEventArgs e)
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
                AddMesh(filePathOpen);
            }
        }

    }
}
