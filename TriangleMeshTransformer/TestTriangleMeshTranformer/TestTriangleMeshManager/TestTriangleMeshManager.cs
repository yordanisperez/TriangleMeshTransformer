using g3;
using Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TriangleMeshTransformer;


namespace TestTriangleMeshTransformer.TestTriangleMeshManager
{
    [TestClass]
    public class TestTriangleMeshManager
    {
       public TriangleMeshManager meshManag ;
       public DMesh3 meshTest;
        [ClassInitialize]
      public void  createInstanceManager()
        {
            meshManag = new TriangleMeshManager();
        }

        [TestInitialize]
        public void init()
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
            meshTest = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(vertices, triangles);
           

        }
        /// <summary>
        /// Testing add one mesh to diccionary
        /// </summary>
        [TestMethod]
       public void AddTrianglesMesh()
        {
            meshManag.AddTrianglesMesh("1erTestCaso", new CGeometry(meshTest));
            Assert.AreEqual(true, meshManag.isPathExist("1erTestCaso"));

        }


    }
}
