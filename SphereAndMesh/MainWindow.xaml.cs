using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SphereAndMesh
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        public Point3D CalculatePositionPoint(double R, double A, double B)
        {
            double sinA = Math.Sin(A * Math.PI / 180);
            double cosA = Math.Cos(A * Math.PI / 180);
            double sinB = Math.Sin(B * Math.PI / 180);
            double cosB = Math.Cos(B * Math.PI / 180);

            Point3D point = new Point3D();
            point.X = 0 + R * sinA * cosB;
            point.Y = 0 + R * sinA * sinB;
            point.Z = 0 + R * cosA;

            return point;
        }
        public static void CreateTriangle(Point3D p0, Point3D p1, Point3D p2, Viewport3D viewport)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Red;
            Material material = new DiffuseMaterial(brush);

            GeometryModel3D geometry = new GeometryModel3D(mesh, material);
            ModelUIElement3D model = new ModelUIElement3D();
            model.Model = geometry;

            viewport.Children.Add(model);
        }
        public void DrawSphere(double R, int N, int n)
        {
            if (n < 15 || N < 15)
            {
                return;
            }

            Model3DGroup sphere = new Model3DGroup();
            Point3D[,] points = new Point3D[N, n];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    points[i, j] = CalculatePositionPoint(R, i * 360 / (N - 1), j * 360 / (n - 1));
                }
            }

            Point3D[] p = new Point3D[4];
            for (int i = 0; i < N - 1; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    p[0] = points[i, j];
                    p[1] = points[i + 1, j];
                    p[2] = points[i + 1, j + 1];
                    p[3] = points[i, j + 1];
                    CreateTriangle(p[0], p[1], p[2], mainViewport);
                    CreateTriangle(p[2], p[3], p[0], mainViewport);

                }
            }
        }

        private void CalcAndDraw_Click(object sender, RoutedEventArgs e)
        {
            DrawSphere(Convert.ToDouble(BoxR.Text), Convert.ToInt32(BoxN.Text), Convert.ToInt32(Boxn.Text));

            DirectionalLight directionalLight = new DirectionalLight();
            directionalLight.Color = Colors.White;
            directionalLight.Direction = new Vector3D(-1, -1, 2);

            PerspectiveCamera perspectiveCamera = new PerspectiveCamera();
            perspectiveCamera.Position = new Point3D(10, 10, 0);
            perspectiveCamera.LookDirection = new Vector3D(-10, -10, 0);
            mainViewport.Camera = perspectiveCamera;

            ModelVisual3D myModelVisual3D = new ModelVisual3D();
            Model3DGroup myModel3DGroup = new Model3DGroup();

            myModel3DGroup.Children.Add(directionalLight);
            myModelVisual3D.Content = myModel3DGroup;
            mainViewport.Children.Add(myModelVisual3D);

        }
         
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            mainViewport.Children.Clear();
        }
    }


}
