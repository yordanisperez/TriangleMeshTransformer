using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace TriangleMeshTransformer
{
    public interface IGeometryBackground
    {
        MeshGeometry3D GetGeometryWPF(Vector3d pMin, Vector3d pMax);


    }


    public interface IGeometryWPF
    {
        MeshGeometry3D GetGeometryWPF();


    }
}
