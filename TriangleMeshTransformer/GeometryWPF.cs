using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace TriangleMeshTransformer
{
  public   class GeometryWPF
    {
       public Vector3DCollection NormalCollection = new Vector3DCollection();
       public Point3DCollection  PositionCollection = new Point3DCollection();
       public PointCollection    TextureCoordinatesCollection = new PointCollection();
       public Int32Collection    TriangleIndicesCollection = new Int32Collection();
       public GeometryWPF() { 
        }
    }
}
