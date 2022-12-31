using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class CSimpleMesh : ISimpleMesh<Vector3d, Index3i>
    {
        private readonly List<Vector3d> vertices =null;
        private readonly List<Index3i> triangles=null;
        /// <summary>
        /// Build simple mesh
        /// </summary>
        public CSimpleMesh() {
            //vertices
            vertices = new List<Vector3d>
            {
                new Vector3d(0,0,0),
                new Vector3d(5,0,0),
                new Vector3d(5,0,5),
                new Vector3d(0,0,5),

                new Vector3d(0,5,0),
                new Vector3d(5,5,0),
                new Vector3d(5,5,5),
                new Vector3d(0,5,5),


            };
            //triangles with this index
            triangles= new List<Index3i>
            {
                new Vector3i(0,1,2),
                new Vector3i(2,3,0),
                new Vector3i(0,4,5),
                new Vector3i(0,5,1),

                new Vector3i(0,3,7),
                new Vector3i(0,7,4),
                new Vector3i(6,2,1),
                new Vector3i(6,1,5),

                new Vector3i(6,2,3),
                new Vector3i(6,3,7),
                new Vector3i(6,5,4),
                //new Vector3i(6,4,7),

            };
            

        }
        /// <summary>
        /// get Vertices
        /// </summary>
        public IEnumerable<Vector3d> Vertices {
            get
            {
                if (vertices != null)
                    return vertices;
                return new List<Vector3d>(0);

            }
        }
        /// <summary>
        /// get Triangles
        /// </summary>
        public IEnumerable<Index3i> Triangles {
            get
            {
                if (triangles!=null)
                    return triangles;
                return new List<Index3i>(0);
            }
        }

    }
}
