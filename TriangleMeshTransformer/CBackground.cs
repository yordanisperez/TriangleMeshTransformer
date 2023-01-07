using g3;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TriangleMeshTransformer
{
    public class CBackground : IGeometryBackground
    {
        public MeshGeometry3D GetGeometryWPF(Vector3d pMin, Vector3d pMax)
        {
            MeshGeometry3D geo = new MeshGeometry3D();

            Vector3DCollection NormalCollection = new Vector3DCollection();
            Point3DCollection PositionCollection = new Point3DCollection();
            PointCollection TextureCoordinatesCollection = new PointCollection();
            Int32Collection TriangleIndicesCollection = new Int32Collection();

            double lim = 1000;
            PositionCollection.Add(new Point3D(Math.Abs(pMin[0])+lim * -1, Math.Abs(pMin[1] )+lim * -1, Math.Abs(pMin[3]) * -1));
            PositionCollection.Add(new Point3D(Math.Abs(pMax[0])+lim, Math.Abs(pMin[1] )+lim * -1, Math.Abs(pMin[3]) * -1));
            PositionCollection.Add(new Point3D(Math.Abs(pMax[0] )+lim, Math.Abs(pMax[1])+lim, Math.Abs(pMin[3]) * -1));
            PositionCollection.Add(new Point3D(Math.Abs(pMin[0] )+lim * -1, Math.Abs(pMax[1])+lim, Math.Abs(pMin[3]) * -1));



            geo.Positions = PositionCollection;

            NormalCollection.Add(new Vector3D(0, 0, 1));
            NormalCollection.Add(new Vector3D(0, 0, 1));
            NormalCollection.Add(new Vector3D(0, 0, 1));
            NormalCollection.Add(new Vector3D(0, 0, 1));

            geo.Normals = NormalCollection;

            TriangleIndicesCollection.Add(0);
            TriangleIndicesCollection.Add(1);
            TriangleIndicesCollection.Add(2);
            TriangleIndicesCollection.Add(0);
            TriangleIndicesCollection.Add(2);
            TriangleIndicesCollection.Add(3);

            geo.TriangleIndices = TriangleIndicesCollection;

            TextureCoordinatesCollection.Add(new Point(0, 0));
            TextureCoordinatesCollection.Add(new Point(1, 0));
            TextureCoordinatesCollection.Add(new Point(1, 1));
            TextureCoordinatesCollection.Add(new Point(0, 1));

            geo.TextureCoordinates = TextureCoordinatesCollection;

            return geo;
        }
    }
}
