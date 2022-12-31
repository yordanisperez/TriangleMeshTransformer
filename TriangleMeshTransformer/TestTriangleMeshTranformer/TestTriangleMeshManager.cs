using g3;
using Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Dynamic;
using TriangleMeshTransformer;


namespace TestTriangleMeshTransformer
{
    [TestClass]
    public class TestTriangleMeshManager
    {
        public TriangleMeshManager meshManag = null;
       static List<Vector3d> vertices;
       static List<Index3i> triangles;

        public DMesh3 meshTest = null;
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
        }
        [TestCleanup]
        public void ClearTest()
        {
            meshManag = null;
            meshTest = null;

        }

        [TestMethod]

        public void TestAddTrianglesMesh()
        {

           
            Assert.AreEqual(true, meshManag.AddTrianglesMesh("1erTestCaso", new CGeometry(meshTest)));
            
        }
        [TestMethod]
        public void TestDeleteTriangleMesh()
        {
            meshManag.AddTrianglesMesh("1erTestCaso", new CGeometry(meshTest));
            meshManag.deleteTriangleMesh("1erTestCaso");
            Assert.AreEqual(false, meshManag.isPathExist("1erTestCaso"));

        }
        [TestMethod]
        public void TestNotDuplicateTriangleMesh()
        {
            meshManag.AddTrianglesMesh("1erTestCaso", new CGeometry(meshTest));
            Assert.AreEqual(false, meshManag.AddTrianglesMesh("1erTestCaso", new CGeometry(meshTest)));

        }
        [TestMethod]
        public void TestExistTriangleMesh()
        {
            meshManag.AddTrianglesMesh("1erTestCaso", new CGeometry(meshTest));
            Assert.AreEqual(true, meshManag.isPathExist("1erTestCaso"));
            Assert.AreEqual(false, meshManag.isPathExist("2doTestCaso"));

        }


    }
}
