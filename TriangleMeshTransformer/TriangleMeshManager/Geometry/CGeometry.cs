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
            //Calculate Normals 
            if (!pSimpleMesh.HasVertexNormals)
            {
                MeshNormals meshNormal = new MeshNormals(pSimpleMesh);
                meshNormal.Compute();
                meshNormal.CopyTo(mesh);
            }
        }
        public CGeometry(CSimpleMesh pSimpleMesh) {
            DMesh3 resultMesh;
            resultMesh = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(pSimpleMesh.Vertices, pSimpleMesh.Triangles);
            MeshNormals meshNormal = new MeshNormals(resultMesh);
            meshNormal.Compute();
            meshNormal.CopyTo(resultMesh);
            mesh = resultMesh;
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
            return new CGeometry(mesh_copy);
        }
        /// <summary>
        /// Save to file with route full pPatch
        /// </summary>
        /// <param name="pPatch"> rute sample D://myfile.stl</param>
       public void saveTo(string pPatch)
        {
            DMesh3 mesh_copy = new DMesh3(mesh)  ;
            WriteOptions w = new WriteOptions
            {
                bWriteBinary = true
            };  
            StandardMeshWriter.WriteMesh(pPatch, mesh_copy, w);
        }

        public DMesh3 mergingMeshesSDF(int pNumberCell, int pNumberCube)
        {
            DMeshAABBTree3 spatial = new DMeshAABBTree3(mesh);
            spatial.Build();           
            double cell_size = mesh.CachedBounds.MaxDim / pNumberCell;

            MeshSignedDistanceGrid sdf = new MeshSignedDistanceGrid(mesh, cell_size);
            sdf.Compute();

            var iso = new DenseGridTrilinearImplicit(sdf.Grid, sdf.GridOrigin, sdf.CellSize);

            MarchingCubes c = new MarchingCubes();
            c.Implicit = iso;
            c.Bounds = mesh.CachedBounds;
            c.CubeSize = c.Bounds.MaxDim / pNumberCube;
            c.Bounds.Expand(3 * c.CubeSize);

            c.Generate();

            return  c.Mesh;
        }

    }

}
