using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;
using Geometry;

namespace TriangleMeshTransformer
{
    public class TriangleMeshManager
    {
        private Dictionary<string, ISimpleMesh<Vector3d, Index3i>> meshs =null;
       public TriangleMeshManager()
        {
            meshs = new Dictionary<string, ISimpleMesh<Vector3d, Index3i>>();
        }
        /// <summary>
        /// Add One ISimpleMesh to Diccionary
        /// </summary>
        /// <param name="pPath">filename associado to file</param>
        /// <param name="pMesh">ISimpleMesh</param>
        /// <returns></returns>
        public bool AddTrianglesMesh(string pPath, ISimpleMesh<Vector3d, Index3i> pMesh)
        {
            if (!meshs.ContainsKey(pPath))
            {
                meshs.Add(pPath, pMesh);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Delete one mesh of diccionary
        /// </summary>
        /// <param name="pPath">Filename</param>
        /// <returns></returns>
        public bool deleteTriangleMesh(string pPath)
        {
            if (!meshs.ContainsKey(pPath))
            {
                return true;
            }

            return meshs.Remove(pPath);
        }
        /// <summary>
        /// Get a iSimple Mesh from diccionary 
        /// </summary>
        /// <param name="pPath"> filename</param>
        /// <returns> ISimpleMesh<Vector3d, Index3i></returns>
        public ISimpleMesh<Vector3d, Index3i> getSimpleMesh(string pPath)
        {
            if (meshs.ContainsKey(pPath))
            {
                return meshs[pPath];
            }
            return null;
        }
        /// <summary>
        /// Determines  whether the diccionary contain with pPath specified
        /// </summary>
        /// <param name="pPath"> string key of diccionary</param>
        /// <returns> Return true if pPatch in meshs diccionary, otherwise false</returns>
        public bool isPathExist(string pPath)
        {
            return meshs.ContainsKey(pPath);
        }
    }
}