using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Geometry
{
    public class CGeometry : ICloneable, ISimpleMesh<Vector3d, Index3i>
    {
        private readonly DMesh3 mesh;

        public CGeometry(DMesh3 pSimpleMesh)
        {
            mesh = pSimpleMesh;
        }
            public CGeometry(CSimpleMesh pSimpleMesh) {
            DMesh3 resultMesh;

            resultMesh = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(pSimpleMesh.Vertices, pSimpleMesh.Triangles);
            MeshNormals meshNormal = new MeshNormals(resultMesh);
            meshNormal.Compute();
            meshNormal.CopyTo(resultMesh);

            DMeshAABBTree3 spatial = new DMeshAABBTree3(resultMesh);
            spatial.Build();
            Vector3d rayDir = new Vector3d(0.331960519038825, 0.462531727525156, 0.822111072077288);
            rayDir.Normalize();

            Console.WriteLine("spatial.TotalExtentSum() {0} spatial.TotalVolume() {1}", spatial.TotalExtentSum(), spatial.TotalVolume());

           

            spatial.FastWindingNumber(Vector3d.Zero);
            List<Vector3d> list_of_points = new List<Vector3d>
            {
                new Vector3d(2,2,2),
                new Vector3d(0,2,0),
                new Vector3d(0,2,2),
            };


            int num_cells = 2;
            double cell_size = resultMesh.CachedBounds.MaxDim / num_cells;

           

            MeshSignedDistanceGrid sdf = new MeshSignedDistanceGrid(resultMesh, cell_size);
            sdf.Compute();

            var iso = new DenseGridTrilinearImplicit(sdf.Grid, sdf.GridOrigin, sdf.CellSize);

            MarchingCubes c = new MarchingCubes();
            c.Implicit = iso;
            c.Bounds = resultMesh.CachedBounds;
            c.CubeSize = c.Bounds.MaxDim / 4;
            c.Bounds.Expand(3 * c.CubeSize);

            c.Generate();

            mesh=c.Mesh;
           // mesh = resultMesh;
            //spatial.IsInside
            //Vector3d rayOrigin = new Vector3d(0, 0, -2.0);
            //Ray3d ray = new Ray3d(rayOrigin,rayDir );
            //int hit_tid = spatial.FindNearestHitTriangle(ray);

            //if (hit_tid != DMesh3.InvalidID)
            //{
            //    IntrRay3Triangle3 intr = MeshQueries.TriangleIntersection(mesh, hit_tid, ray);
            //    double hit_dist = rayOrigin.Distance(ray.PointAt(intr.RayParameter));
            //    Console.WriteLine("Distancia Intercepts {0} count intercept triangle {1}", hit_dist, hit_tid);

            //}
            //else
            //{
            //    Console.WriteLine("No se intercepto");
            //}





        } 

        IEnumerable<Vector3d> ISimpleMesh<Vector3d, Index3i>.Vertices  {
            get
            {
                return mesh.Vertices();

            }
              

    }

        IEnumerable<Index3i> ISimpleMesh<Vector3d, Index3i>.Triangles {
            get
            {
                return mesh.Triangles();
            }

        }
        /// <summary>
        ///  Make a copy of object DMesh3
        /// </summary>
        /// <returns>object DMesh3 </returns>  
        public object Clone()
        {
            DMesh3 mesh_copy = new DMesh3(mesh);
            return mesh;
        }
        /// <summary>
        /// Save to file with route full pPatch
        /// </summary>
        /// <param name="pPatch"> rute sample D://myfile.stl</param>
       public void saveTo(string pPatch)
        {
            DMesh3 mesh_copy = (DMesh3)  Clone();
            WriteOptions w = new WriteOptions
            {
                bWriteBinary = true
            };  
            StandardMeshWriter.WriteMesh(pPatch, mesh_copy, w);
        }
    }

}
