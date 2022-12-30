using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;
using System.Windows;
using TriangleMeshTransformer;


namespace TestTriangleMeshTransformer
{
    
    [TestClass]
    public class MainWindowsTest
    {
        [TestMethod]
        public void testMiOpen_Click()
        {
            
            // Create a mock of the FileDialog
            var fileDialogMock = new Mock<IOpenFileDialog>();
            // Asigna un controlador de eventos al botón que llama al OpenDialog
            // Set up the mock to return a specific file path when ShowDialog() is called
            fileDialogMock.Setup(x => x.ShowDialog()).Returns(true);
            fileDialogMock.Setup(x => x.InitialDirectory).Returns("");
            fileDialogMock.Setup(x => x.Filter).Returns("");
            fileDialogMock.SetupGet(x => x.FileName).Returns(@"C:\myfile.txt");
           // fileDialogMock.SetupGet(x => x.FileNames).Returns(@"C:\myfile.txt");
            // Create an instance of the MainWindow class
            MainWindow mainWindow = new MainWindow();

            // Set the FileDialog property to the mock
            mainWindow.openFileDialog = (IOpenFileDialog) fileDialogMock.Object;


            // Simulate the OpenFileButton_Click event
            mainWindow.miOpen_Click(new object(), new RoutedEventArgs());

            // Verify that the file path returned by the mock was used to set the FilePath property
            Assert.AreEqual(@"C:\myfile.txt", mainWindow.filePathOpen);

        }
    }
}
