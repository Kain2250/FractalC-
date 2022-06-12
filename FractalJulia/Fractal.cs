using System.Drawing;

namespace FractalJulia
{
    public class Fractal
    {
        public ComplexDouble Z { get; set; }
        public ComplexDouble C { get; set; }
        public int MaxIter { get; set; }
        public double Zoom { get; set; }
        public Color ColorPlenty { get; }
        public Color ColorStart { get; }
        public Color ColorEnd { get; }
        public Point Max { get; set; }
        public Point Min { get; set; }
        public Bitmap Bitmap { get; set; }
        
        public Fractal(double zRe, double zIm, double cRe, double cIm, int maxIter, double zoom, Color colorPlenty,
            Color colorStart, Color colorEnd, Point max, Point min, Bitmap bitmap)
        {
            Z = new ComplexDouble(zRe, zIm);
            C = new ComplexDouble(cRe, cIm);
            MaxIter = maxIter;
            Zoom = zoom;
            ColorPlenty = colorPlenty;
            ColorStart = colorStart;
            ColorEnd = colorEnd;
            Max = max;
            Min = min;
            Bitmap = bitmap;
        }

        public Fractal()
        {
            Z = new ComplexDouble(0, 0);
            C = new ComplexDouble(-2.1, -1.1);
            MaxIter = 100;
            Zoom = 350;
            ColorPlenty = Color.Black;
            ColorStart = Color.Orange;
            ColorEnd = Color.Purple;
            Max = Point.Empty;
            Min = Point.Empty;
            Bitmap = null;
        }
        
        public Fractal(ComplexDouble c, int maxIter, double zoom)
        {
            Z = new ComplexDouble(0, 0);
            C = new ComplexDouble(c.Re, c.Im);
            MaxIter = maxIter;
            Zoom = zoom;
            ColorPlenty = Color.Black;
            ColorStart = Color.Purple;
            ColorEnd = Color.Yellow;
            Max = Point.Empty;
            Min = Point.Empty;
            Bitmap = null;
        }

        
    }
}