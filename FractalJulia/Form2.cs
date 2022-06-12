using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace FractalJulia
{
    public partial class Form2 : Form
    {
        private static int _maxThreads = 32;
        private ComplexDouble _c = new ComplexDouble(-2.1, -1.1);
        private int _maxIter = 30;
        private double _zoom = 340d;
        private double _speedZoom = 2.1;
        
        private Thread[] _threads = new Thread[_maxThreads];
        private Fractal[] _fractals = new Fractal[_maxThreads];
        private Bitmap[] _bitmaps = new Bitmap[_maxThreads];
        private int[] _yStartPoints = new int[_maxThreads];
        

        private Graphics _g;
        private ParameterizedThreadStart _threadDraw;
        
        
        public Form2()
        {
            InitializeComponent();
            InitFractals();
        }

        private void InitFractals()
        {
            _threadDraw = CalculateFractal;
            
            var panel1Height = (int) (1.00 / _maxThreads * panel1.Height);

            for (var index = 0; index < _fractals.Length; index++)
            {
                _bitmaps[index] = new Bitmap(panel1.Width, panel1Height);
                
                var min = new Point(0, panel1Height * index);
                var max = new Point(panel1.Width, panel1Height * (index + 1));
                
                _fractals[index] = new Fractal(_c, _maxIter, _zoom);
                _fractals[index].Max = max;
                _fractals[index].Min = min;
                _fractals[index].Bitmap = _bitmaps[index];
                
                _threads[index] = new Thread(_threadDraw);
                _yStartPoints[index] = min.Y;
            }
        }
        
        private double Percent(int current, int start, int end)
        {
            double placement = current - start;
            double distance = end - start;
            
            if (distance == 0)
                return 1.0;
            return placement / distance;
        }
        
        private void CalculateFractal(object o)
        {
            var currentPlenty = (Fractal)o;
            
            var y = currentPlenty.Min.Y;
            while (y != currentPlenty.Max.Y)
            {
                var x = currentPlenty.Min.X;
                while (x != currentPlenty.Max.X)
                {
                    var z = currentPlenty.Z;
                    var c = new ComplexDouble(x / currentPlenty.Zoom + currentPlenty.C.Re,
                        y / currentPlenty.Zoom + currentPlenty.C.Im);

                    Mandelbrot(currentPlenty, z, c, x, y);
                    x++;
                }
                y++;
            }
        }

        private void Mandelbrot(Fractal currentPlenty, ComplexDouble z, ComplexDouble c, int x, int y)
        {
            double lenVector = 1;
            ComplexDouble z2;
            int i = 0;

            while (i <= currentPlenty.MaxIter && lenVector <= 2.3)
            {
                z2 = z * z;
                z = z2 + c;
                lenVector = z.Abs;
                i++;

                if (i >= currentPlenty.MaxIter)
                {
                    currentPlenty.Bitmap.SetPixel(x, y - currentPlenty.Min.Y, currentPlenty.ColorPlenty);
                    return;
                }
            }
            
            Color currentColor = ColorInterpolator.InterpolateBetween(
                currentPlenty.ColorStart,
                currentPlenty.ColorEnd,
                Percent(i, 0, _maxIter));
            
            currentPlenty.Bitmap.SetPixel(x, y - currentPlenty.Min.Y, currentColor);
        }
        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            _g = e.Graphics;

            for (var index = 0; index < _threads.Length; index++)
            {
                var thread = _threads[index];
                thread.Start(_fractals[index]);
            }
            
            foreach (var thread in _threads)
                thread.Join();

            for (var j = 0; j < _maxThreads; j++)
                _g.DrawImageUnscaled(_bitmaps[j], new Point(0, _yStartPoints[j]));
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            var click = e.Location;

            if ((e.Button & MouseButtons.Left) != 0)
            {
                ComplexDouble m = new ComplexDouble();

                m.Re = click.X / _zoom + _fractals[0].C.Re;
                m.Im = click.Y / _zoom + _fractals[0].C.Im;
                _zoom *= _speedZoom;
                _c.Re = m.Re - click.X / _zoom;
                _c.Im = m.Im - click.Y / _zoom;
                _maxIter += 2;
                if (_maxIter < 10)
                    _maxIter = 10;
                if (_maxIter >= Int32.MaxValue)
                    _maxIter = Int32.MaxValue;

            }
            else
                _maxIter += 10;
            
            InitFractals();
            panel1.Invalidate();
        }
    }
}