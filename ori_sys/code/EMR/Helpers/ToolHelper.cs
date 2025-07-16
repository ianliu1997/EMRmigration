using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Silverdraw.Client
{
    public class ToolHelper
    {



        public UIElement CreateShape<T>(Point prev, Point current) where T:Shape,new()
        {
            T e = new T();
            // Set the width and height of the Ellipse.
            e.Width = Math.Abs(current.X-prev.X);
            e.Height = Math.Abs(current.Y-prev.Y);

            Canvas.SetLeft(e,Math.Min(current.X, prev.X));
            Canvas.SetTop(e, Math.Min(prev.Y, current.Y));

            return e;
        }

        public UIElement CreatePen(Point prev, Point current)
        {
            Line ln = new Line();
            ln.X1 = prev.X;
            ln.Y1 = prev.Y;
            ln.X2 = current.X;
            ln.Y2 = current.Y;
            return ln;
        }

        public UIElement CreateBrush(Point prev, Point current,double size)
        {
            Ellipse e = new Ellipse();
            // Set the width and height of the Ellipse.
            e.Width = Math.Abs(size);
            e.Height = Math.Abs(size);

            Canvas.SetLeft(e, current.X - size/2);
            Canvas.SetTop(e, current.Y - size / 2);

            return e;
            
        }

       
    }
}
