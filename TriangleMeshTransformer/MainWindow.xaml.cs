using Microsoft.Win32;
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
            }
        }
    }
}
