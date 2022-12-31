using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;
using System.Windows;
using TriangleMeshTransformer;
using g3;
using System.Collections.Generic;

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
            mainWindow.testMiOpen_Click(new object(), new RoutedEventArgs());

            
            // Verify that the file path returned by the mock was used to set the FilePath property
            Assert.AreEqual(@"C:\myfile.txt", mainWindow.filePathOpen);

        }
        [TestMethod]
       public void testAddMesh()
        {
            List<Vector3d> vertices = new List<Vector3d>
            {
                new Vector3d(0,0,0),
                new Vector3d(5,0,0),
                new Vector3d(5,0,5),
            

            };
            List<Index3i> triangles = new List<Index3i>
            {
                new Vector3i(0,1,2),
                new Vector3i(2,3,0),
                new Vector3i(0,4,5),
            };
          DMesh3 meshTest= DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(vertices, triangles);
            //create mock static class
            var myClassMock = new Mock<iCallMethodStatic>();
            //add method for create mesh
            myClassMock.Setup(x => x.ReadMesh(It.IsAny<string>())).Returns(meshTest);

            MainWindow mainWindow = new MainWindow();
            mainWindow.staticCall =(iCallMethodStatic) myClassMock.Object;

            mainWindow.addMesh("OneMesh");
            //Is add the meh correcty
            Assert.AreEqual(true, mainWindow.existMesh("OneMesh"));

           
           

        }
    }
}
