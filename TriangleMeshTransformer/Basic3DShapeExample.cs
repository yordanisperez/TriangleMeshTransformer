using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using Geometry;
using g3;
using System.Security.Cryptography;
using System.Threading;
using HelixToolkit.Wpf;
using System.Diagnostics;

namespace TriangleMeshTransformer
{
    
    public partial class Basic3DShapeExample :Page
    {
        bool startTranf = false;
        Vector2d startClick;
        GeometryModel3D myGeometryModel;
        DMesh3 mesh;
        public Func<bool> CancelF = () => false;
        Stopwatch stopwatch;
        Label lbFPS;
        Viewport3D myViewport3D;
        StackPanel stPanel;
         int countFPS = 0;
        double currentFps = 0;
        public Basic3DShapeExample(CGeometry pGeometry)
        {
             mesh = pGeometry.Mesh;
            // Declare scene objects.
            myViewport3D = new Viewport3D();
            Model3DGroup myModel3DGroup = new Model3DGroup();
            myGeometryModel = new GeometryModel3D();
            ModelVisual3D myModelVisual3D = new ModelVisual3D();
            // Defines the camera used to view the 3D object. In order to view the 3D object,
            // the camera must be positioned and pointed such that the object is within view
            // of the camera.
            PerspectiveCamera myPCamera = new PerspectiveCamera();

            // Specify where in the 3D scene the camera is.
            AxisAlignedBox3d axis= mesh.GetBounds();
            //Set Camera in position central to max depth
           

            Vector3d ptPositionCamera = new Vector3d(axis.Center[0], axis.Center[1], axis.Depth*2);
            myPCamera.Position = new Point3D(ptPositionCamera[0], ptPositionCamera[1], ptPositionCamera[2]);
            Vector3d ptLook = new Vector3d(axis.Center[0], axis.Center[1], axis.Center[2]);
            
            // Specify the direction that the camera is pointing. 
            Vector3d directionCamera = ptLook - ptPositionCamera;
            myPCamera.LookDirection = new Vector3D(directionCamera[0], directionCamera[1], directionCamera[2]);

            // Define camera's horizontal field of view in degrees.
            myPCamera.FieldOfView = 90;

            // Asign the camera to the viewport
            myViewport3D.Camera = myPCamera;
            // Define the lights cast in the scene. Without light, the 3D object cannot
            // be seen. Note: to illuminate an object from additional directions, create
            // additional lights.
            DirectionalLight myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = Colors.White;
            //Hay que valorar cual será la direccion de la luz mas adecuada***************
            myDirectionalLight.Direction = new Vector3D(-0.61, -0.5, -0.61);

            myModel3DGroup.Children.Add(myDirectionalLight);

            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet
            // is created.
            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry =  pGeometry.GetGeometryWPF();

            // The material specifies the material applied to the 3D object. In this sample a
            // linear gradient covers the surface of the 3D object.

            // Create a horizontal linear gradient with four stops.
            LinearGradientBrush myHorizontalGradient = new LinearGradientBrush();
            myHorizontalGradient.StartPoint = new Point(0, 0.5);
            myHorizontalGradient.EndPoint = new Point(1, 0.5);
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myHorizontalGradient);
            myGeometryModel.Material = myMaterial;


            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);

            // Add the group of models to the ModelVisual3d.
            myModelVisual3D.Content = myModel3DGroup;


            CBackground background = new CBackground();
             

            ModelVisual3D visualBackgroud = new ModelVisual3D();
            visualBackgroud.Content = new GeometryModel3D(background.GetGeometryWPF(axis.Min, axis.Max), new DiffuseMaterial(Brushes.Black));
            myViewport3D.Children.Add(visualBackgroud);
            //
            myViewport3D.Children.Add(myModelVisual3D);
            myViewport3D.MouseDown += MyViewport3D_MouseDown;
            myViewport3D.MouseMove += MyViewport3D_MouseMove;
           // myViewport3D.MouseMove += QuaternionsMouseMove;
            myViewport3D.MouseUp += MyViewport3D_MouseUp;
            // Apply the viewport to the page so it will be rendered.

             lbFPS = new Label
            {
                Content = "FPS : ",

            };
            // this.Chil
            myViewport3D.Width = Width;
            myViewport3D.Height = 500;

            // Frame fmviewport = new Frame();
            //fmviewport.Content = myViewport3D;
            stPanel = new StackPanel {
             Width = Width,
             Height = Height,
            };
            stPanel.Children.Add(lbFPS);
            stPanel.Children.Add(myViewport3D);
            this.Content = stPanel;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (countFPS < 1000)
            {
                countFPS++;
                return;
            }
            double elapsedTime = stopwatch.ElapsedMilliseconds;
            double fps = 1000*countFPS / elapsedTime;
            countFPS = 0;
            currentFps = fps;
            lbFPS.Content = String.Format("FPS:{0}",((int)fps));
            stopwatch.Restart();
        }

      private void MyViewport3D_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            startTranf = false;
           
        }

        private void MyViewport3D_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!startTranf)
                return;
            AxisAlignedBox3d axis = mesh.GetBounds();
            Vector3d ptCenter = new Vector3d(axis.Center[0], axis.Center[1], axis.Center[2]);
            Vector2d mauseMoveClick = new Vector2d(e.GetPosition(myViewport3D).X, e.GetPosition(myViewport3D).Y);



            // Apply a transform to the object. In this sample, a rotation transform is applied,
            // rendering the 3D object rotated.

               Transform3DGroup transformGroup = new Transform3DGroup();
               Transform3D tranfCurrent = myGeometryModel.Transform;
               transformGroup.Children.Add(tranfCurrent);

                RotateTransform3D myRotateTransform3D = new RotateTransform3D();
               myRotateTransform3D.CenterX = ptCenter[0];
               myRotateTransform3D.CenterY = ptCenter[1];
               myRotateTransform3D.CenterZ = ptCenter[2];
               AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();


               myAxisAngleRotation3d.Axis = new Vector3D(mauseMoveClick[1]-startClick[1]  , mauseMoveClick[0]-startClick[0] , 0);
               myAxisAngleRotation3d.Angle = 5.0;
               myRotateTransform3D.Rotation = myAxisAngleRotation3d;
               transformGroup.Children.Add(myRotateTransform3D);
               MatrixTransform3D matrixTransform = new MatrixTransform3D(transformGroup.Value);
               myGeometryModel.Transform = matrixTransform;




        }

        private void QuaternionsMouseMove(object sender, System.Windows.Input.MouseEventArgs  e)
        {

            if (!startTranf)
                return;
            AxisAlignedBox3d axis = mesh.GetBounds();
            Vector3d ptCenter = new Vector3d(axis.Center[0], axis.Center[1], axis.Center[2]);
            Vector2d mauseMoveClick = new Vector2d(e.GetPosition(myViewport3D).X, e.GetPosition(myViewport3D).Y);


            /*Prueba con quaternions*/
            Vector3D rotationAxis = new Vector3D(mauseMoveClick[1] - startClick[1], mauseMoveClick[0] - startClick[0], 0);
            if (rotationAxis.Length == 0)
                return;
            //double rotationAngle += 1;
            Quaternion rotation = new Quaternion(rotationAxis, 5.0);
            // rotation.

            // Actualiza el quaternion de la QuaternionRotation3D
            QuaternionRotation3D quaternionRotation = new QuaternionRotation3D(rotation);
            // quaternionRotation.Quaternion = rotation;



            // Aplica la transformación al elemento 3D
            Transform3DGroup transformGroup = new Transform3DGroup();
            RotateTransform3D rotateTranf = new RotateTransform3D(quaternionRotation);
            rotateTranf.CenterX = ptCenter[0];
            rotateTranf.CenterY = ptCenter[1];
            rotateTranf.CenterZ = ptCenter[2];
            Transform3D tranfCurrent = myGeometryModel.Transform;
           
            transformGroup.Children.Add(tranfCurrent);
            transformGroup.Children.Add(rotateTranf);
            MatrixTransform3D matrixTransform = new MatrixTransform3D(transformGroup.Value);
            myGeometryModel.Transform = matrixTransform;

        }
        private void MyViewport3D_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            startTranf = true;
            startClick = new Vector2d(e.GetPosition(myViewport3D).X, e.GetPosition(myViewport3D).Y);
            
        }
    }
}
