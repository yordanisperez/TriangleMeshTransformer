using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    /// <summary>
    /// Generic ISimpleMesh that soports generic type
    /// </summary>
    /// <typeparam name="VType"> VType soport Vector3d and Vector3f</typeparam>
    /// <typeparam name="TType"> TType soport Vector3i and Index3i</typeparam>
    internal interface ISimpleMesh<VType, TType>
    {
        /// <summary>
        /// All the vertex of geometry
        /// </summary>
        IEnumerable<VType> Vertices { get; }
        /// <summary>
        /// All index triangle of geometry
        /// </summary>
        IEnumerable<TType> Triangles { get; }
 

    }
}
