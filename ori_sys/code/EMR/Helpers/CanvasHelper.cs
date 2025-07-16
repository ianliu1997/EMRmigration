using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Silverdraw.Client
{
    public enum ScreenObjectType
    {
        Line,
        Ellipse,
        Text,
        Shape,
        Rectangle
    }

    public class ScreenObject
    {
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public double Thickness { get; set; }
        public string StrokeColor { get; set; }
        public string FillColor { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public ScreenObjectType Type { get; set; }
        public string Text;
    }
    public class CanvasHelper
    {


        /// <summary>
        /// Constructs a screen object from a shape
        /// </summary>
        /// <param name="shp"></param>
        /// <returns></returns>
        public ScreenObject Shape2Object(Shape shp)
        {
           
            if (shp != null)
            {
                var line = shp as Line;
                if (line != null)
                {
                    return
                        (new ScreenObject()
                        {
                            StrokeColor = (line.Stroke as SolidColorBrush).Color.ToString(),
                            X1 = line.X1,
                            X2 = line.X2,
                            Y1 = line.Y1,
                            Y2 = line.Y2,
                            Thickness = line.StrokeThickness,
                            Type=ScreenObjectType.Line
 
                        }
                    );
                }
                else
                {
                   var obj=
                        (new ScreenObject()
                        {
                            X1 = Canvas.GetLeft(shp),
                            Width = shp.Width,
                            Y1 = Canvas.GetTop(shp),
                            Height = shp.Height,
                            Thickness = shp.StrokeThickness
                        }
                    );

                   if (shp is Rectangle)
                       obj.Type = ScreenObjectType.Rectangle;
                   else if (shp is Ellipse)
                       obj.Type = ScreenObjectType.Ellipse;
                   try
                   {
                     obj.FillColor=(shp.Fill as SolidColorBrush).Color.ToString();
                     obj.StrokeColor = (shp.Stroke as SolidColorBrush).Color.ToString();
                   }
                   catch { }

                   return obj;
                }
            }
            else
                return null;
        }


        /// <summary>
        /// Constructs a screen object from a shape
        /// </summary>
        /// <param name="shp"></param>
        /// <returns></returns>
        public Shape Object2Shape(ScreenObject obj)
        {

            if (obj != null)
            {
                if (obj.Type == ScreenObjectType.Line)
                {
                    return new Line()
                    {
                        X1 = obj.X1,
                        X2 = obj.X2,
                        Y1 = obj.Y1,
                        Y2 = obj.Y2,
                        StrokeThickness = obj.Thickness,
                        Stroke = new SolidColorBrush(ColorHelper.ColorFromString(obj.StrokeColor))
                    };
                }

                else if (obj.Type==ScreenObjectType.Ellipse)
                {
                    var shp = new Ellipse();
                    CopyValues(obj, shp);
                    return shp;
                }

                else if (obj.Type == ScreenObjectType.Rectangle)
                {
                    var shp = new Rectangle();
                    CopyValues(obj, shp);
                    return shp;
                }
            }

            return null;
        }

        /// <summary>
        /// Copy the values to shape
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="shp"></param>
        private void CopyValues(ScreenObject obj, Shape shp)
        {
            Canvas.SetLeft(shp, obj.X1);
            Canvas.SetTop(shp, obj.Y1);
            shp.Width = obj.Width;
            shp.Height = obj.Height;
            shp.StrokeThickness = obj.Thickness;
            try
            {
                shp.Fill = new SolidColorBrush(ColorHelper.ColorFromString(obj.FillColor));
                shp.Stroke = new SolidColorBrush(ColorHelper.ColorFromString(obj.StrokeColor));
            }
            catch { }
            
        }


       
    }
}
