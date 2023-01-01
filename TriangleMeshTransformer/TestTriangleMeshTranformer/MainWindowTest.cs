using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public TriangleMeshManager meshManag = null;
        static List<Vector3d> vertices;
        static List<Index3i> triangles;

        public DMesh3 meshTest = null;
        MainWindow mainWindow = null;

        [ClassInitialize]
        public static void InitSampleTesting(TestContext testContext)
        {
            vertices = new List<Vector3d>
              {
                  new Vector3d(0,0,0),
                  new Vector3d(5,0,0),
                  new Vector3d(5,0,5),


              };
            triangles = new List<Index3i>
              {
                  new Vector3i(0,1,2),
                  new Vector3i(2,3,0),
                  new Vector3i(0,4,5),
              };
        }

        [TestInitialize]
        public void Init()
        {
            meshManag = new TriangleMeshManager();
            meshTest = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(vertices, triangles);
            // Create a mock of the FileDialog
            var fileDialogMock = new Mock<IOpenFileDialog>();
            // Asigna un controlador de eventos al botón que llama al OpenDialog
            // Set up the mock to return a specific file path when ShowDialog() is called
            fileDialogMock.Setup(x => x.ShowDialog()).Returns(true);
            fileDialogMock.Setup(x => x.InitialDirectory).Returns("");
            fileDialogMock.Setup(x => x.Filter).Returns("");
            fileDialogMock.SetupGet(x => x.FileName).Returns(@"C:\myfile.txt");
            // fileDialogMock.SetupGet(x => x.FileNames).Returns(@"C:\myfile.txt");

            //create mock static class
            var myCallMethodStaticMock = new Mock<iCallMethodStatic>();
            //add method for create mesh
            myCallMethodStaticMock.Setup(x => x.ReadMesh(It.IsAny<string>())).Returns(meshTest);
            myCallMethodStaticMock.Setup(x => x.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>())).Returns(MessageBoxResult.Yes);
            // Create an instance of the MainWindow class
            mainWindow = new MainWindow
            {
                // Set the staticCall property to the mock
                staticCall = (iCallMethodStatic)myCallMethodStaticMock.Object,
                 // Set the FileDialog property to the mock
                openFileDialog = (IOpenFileDialog)fileDialogMock.Object
            };

        }
        [TestCleanup]
        public void ClearTest()
        {
            meshManag = null;
            meshTest = null;
            mainWindow = null;

        }

        [TestMethod]
        public void TestMiOpen_Click()
        {
            // Simulate the OpenFileButton_Click event
            mainWindow.testMiOpen_Click(new object(), new RoutedEventArgs());
            // Verify that the file path returned by the mock was used to set the FilePath property
            Assert.AreEqual(@"C:\myfile.txt", mainWindow.filePathOpen);

        }
        [TestMethod]
       public void TestAddMesh()
        {

            mainWindow.addMesh("OneMesh");
            //Is add the meh correcty
            Assert.AreEqual(true, mainWindow.existMesh("OneMesh"));
        }
    }
}
