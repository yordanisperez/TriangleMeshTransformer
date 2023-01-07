using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Xml.Linq;


namespace Geometry
{
    public class CGeometry : ICloneable, ISimpleMesh<Vector3d, Index3i>
    {
        private  DMesh3 mesh ;

        

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

            if (!pSimpleMesh.HasVertexUVs)
            {
                mesh.EnableVertexUVs(new Vector2f(0, 0.5));
            }
            computeTextureBallProyection();
        }
        public CGeometry(CSimpleMesh pSimpleMesh) {
            DMesh3 resultMesh;
            resultMesh = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(pSimpleMesh.Vertices, pSimpleMesh.Triangles);
            MeshNormals meshNormal = new MeshNormals(resultMesh);
            meshNormal.Compute();
            meshNormal.CopyTo(resultMesh);
            mesh = resultMesh;
            if (!mesh.HasVertexUVs)
            {
                mesh.EnableVertexUVs(new Vector2f(0, 0.5));
            }
            computeTextureBallProyection();
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
            DMesh3 mesh_copy = new DMesh3(mesh, false,true, true, true); ;
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

        /// <summary>
        /// get DMesh3
        /// </summary>
        public DMesh3 Mesh
        {
            get
            {
                return mesh;

            }
        }

        /// <summary>
        /// Se calcula las coordenadas UI a partir de interpolar la textura en el 
        /// area total de la geometria utilizando la proyeccion de coordenadas esfericas.
        /// </summary>
        public void computeTextureBallProyection()
        {
            Vector3d centerBall = new Vector3d(0,0,0);

            Action<int> SumAcc = delegate (int vid)
            {
                centerBall+= mesh.GetVertex(vid);
            };
            foreach (int item in mesh.VertexIndices())
            {
                SumAcc(item);
            }

            centerBall /= mesh.VertexCount;
            
            Dictionary<int,Vector3d> vCenterBall= new Dictionary<int,Vector3d>();

            Action<int> calcVectorNormCenterBall = delegate (int vid)
            {
                centerBall += mesh.GetVertex(vid);
                vCenterBall.Add(vid, ( centerBall- mesh.GetVertex(vid)).Normalized);
            };
            foreach (int item in mesh.VertexIndices())
            {
                calcVectorNormCenterBall(item);
            }

            foreach (KeyValuePair<int, Vector3d> element in vCenterBall)
            {
               double s = Math.Atan2(element.Value.y, element.Value.x)/2*Math.PI;
               double t = Math.Atan2(element.Value.z, Math.Sqrt(element.Value.x * element.Value.x + element.Value.y * element.Value.y)) / Math.PI;
               mesh.SetVertexUV(element.Key, new Vector2f(s, t));

            }

        }

    }

}
