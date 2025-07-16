using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Silverdraw.Client
{
    public class DrawingArea
    {
        #region Private Variables

        private Canvas _canvas;
        ToolHelper toolHelper = new ToolHelper();
        List<Shape> tempHolder = new List<Shape>();
        CanvasHelper canHelper = new CanvasHelper();
        Line _virtualLine = new Line() { Visibility = Visibility.Collapsed, Stroke = new SolidColorBrush(Colors.LightGray), StrokeThickness = 2 };
        CurrentTool tool = CurrentTool.Pencil;
        ObservableCollection<TalkItem> messages = new ObservableCollection<TalkItem>();

        private List<Shape> redoList;
        private List<Shape> UndoList;
        private Dictionary<Shape, List<Shape>> UnShapeDrawingItems = new Dictionary<Shape, List<Shape>>();
        private int redoTop;
        private int undoTop;

        #endregion

        #region Properties

        public ObservableCollection<TalkItem> MessageCollection
        {
            get { return messages; }
        }


        /// <summary>
        /// Current tool to perform draw
        /// </summary>
        public CurrentTool Tool
        {
            get { return tool; }
            set { tool = value; }
        }


        /// <summary>
        /// Temporary holder to hold list of objects
        /// </summary>
        public List<Shape> TempHolder
        {
            get { return tempHolder; }
            set { tempHolder = value; }
        }


        /// <summary>
        /// Previous point
        /// </summary>
        public Point PrevPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Starting Point
        /// </summary>
        public Point StartPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Stroke color
        /// </summary>
        public SolidColorBrush StrokeColor { get; set; }

        /// <summary>
        /// Stroke width
        /// </summary>
        public double StrokeWidth { get; set; }

        /// <summary>
        /// Filling color
        /// </summary>
        public SolidColorBrush FillColor { get; set; }

        #endregion

        #region Events


        public event Action<string> DataAdded;

        #endregion

        #region Ctor and Methods

        /// <summary>
        /// Drawing area constructor
        /// </summary>
        /// <param name="c"></param>
        public DrawingArea(Canvas c)
        {
            _canvas = c;
            _canvas.Children.Add(_virtualLine);
            redoList = new List<Shape>();
            UndoList = new List<Shape>();

        }


        /// <summary>
        /// Draws item on complete
        /// </summary>
        public Point DrawOnComplete(Point cupt)
        {
            switch (tool)
            {
                case CurrentTool.Pen:
                    {
                        var pen = toolHelper.CreatePen(PrevPoint, cupt);
                        ApplyAttributes(pen as Shape);
                        _canvas.Children.Add(pen);
                        PrevPoint = cupt;
                        AddToUndoShape(pen as Shape);
                        break;

                    }

                case CurrentTool.Rectangle:
                    {
                        var shp = toolHelper.CreateShape<Rectangle>(PrevPoint, cupt);
                        ApplyAttributes(shp as Shape);
                        _canvas.Children.Add(shp);
                        AddToUndoShape(shp as Shape);
                        PrevPoint = cupt;
                        break;
                    }
                case CurrentTool.Ellipse:
                    {
                        var shp = toolHelper.CreateShape<Ellipse>(PrevPoint, cupt);
                        ApplyAttributes(shp as Shape);
                        _canvas.Children.Add(shp);
                        //tempHolder.Add(shp as Shape);
                        AddToUndoShape(shp as Shape);


                        PrevPoint = cupt;
                        break;
                    }
                case CurrentTool.Brush:
                case CurrentTool.EraseBrush:
                case CurrentTool.Pencil:
                    {
                        var shp = toolHelper.CreatePen(PrevPoint, cupt);
                        List<Shape> UnshapeList = new List<Shape>();
                        tempHolder.ForEach(p => UnshapeList.Add(p));
                        UnShapeDrawingItems.Add(shp as Shape, UnshapeList);
                        tempHolder.Clear();
                        AddToUndoShape(shp as Shape);
                    }
                    break;
            }



            return cupt;
        }

        /// <summary>
        /// Draws the item on move
        /// </summary>
        public Point DrawOnMove(Point cupt)
        {
            switch (tool)
            {
                case CurrentTool.Brush:
                    {
                        HideVirtualLine();
                        var spot = toolHelper.CreateBrush(PrevPoint, cupt, StrokeWidth);
                        (spot as Shape).StrokeThickness = 0;
                        (spot as Shape).Fill = FillColor;
                        _canvas.Children.Add(spot);
                        tempHolder.Add(spot as Shape);
                        PrevPoint = cupt;
                        break;

                    }

                case CurrentTool.EraseBrush:
                    {
                        HideVirtualLine();
                        var spot = toolHelper.CreateBrush(PrevPoint, cupt, 25);
                        (spot as Shape).StrokeThickness = 0;
                        (spot as Shape).Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                        _canvas.Children.Add(spot);
                        tempHolder.Add(spot as Shape);
                        PrevPoint = cupt;
                        break;

                    }
                case CurrentTool.Pencil:
                    {
                        var pen = toolHelper.CreatePen(PrevPoint, cupt);
                        ApplyAttributes(pen as Shape);
                        (pen as Shape).StrokeThickness = 3;
                        _canvas.Children.Add(pen);
                        tempHolder.Add(pen as Shape);
                        PrevPoint = cupt;
                        break;
                    }
                default:
                    {
                        ShowVirtualLine(StartPoint.X, StartPoint.Y, cupt.X, cupt.Y);
                        break;
                    }

            }
            return cupt;
        }

        /// <summary>
        /// Apply shape attribute
        /// </summary>
        public void ApplyAttributes(Shape e)
        {
            e.Stroke = StrokeColor;
            //e.Fill = FillColor;
            e.StrokeThickness = 2;
            e.StrokeThickness = StrokeWidth;
        }


        /// <summary>
        /// Hide virtual line
        /// </summary>
        public void HideVirtualLine()
        {
            _virtualLine.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows the virtual line
        /// </summary>
        public void ShowVirtualLine(double x1, double y1, double x2, double y2)
        {
            if (!_canvas.Children.Contains(_virtualLine))
                _canvas.Children.Add(_virtualLine);
            _virtualLine.Visibility = Visibility.Visible;
            Canvas.SetZIndex(_virtualLine, 0);
            _virtualLine.X1 = x1;
            _virtualLine.X2 = x2;
            _virtualLine.Y1 = y1;
            _virtualLine.Y2 = y2;
        }


        public void UndoShape()
        {
            if (undoTop > 0)
            {
                undoTop--;
                Shape shape = UndoList[undoTop];
                if (shape is Line && UnShapeDrawingItems.ContainsKey(shape))
                {
                    foreach (var unShapeDrawingItem in UnShapeDrawingItems[shape])
                    {
                        _canvas.Children.Remove(unShapeDrawingItem);
                    }
                }
                else
                    _canvas.Children.Remove(shape);
                UndoList.RemoveAt(undoTop);

                AddToRedoList(shape);
            }
        }
        public void RedoShape()
        {
            if (redoTop > 0)
            {
                redoTop--;
                Shape shape = redoList[redoTop];
                if (shape is Line && UnShapeDrawingItems.ContainsKey(shape))
                {
                    foreach (var unShapeDrawingItem in UnShapeDrawingItems[shape])
                    {
                        _canvas.Children.Add(unShapeDrawingItem);
                    }
                }
                else
                    _canvas.Children.Add(shape);
                redoList.RemoveAt(redoTop);
                AddToUndoShape(shape);
            }
        }

        private void AddToRedoList(Shape shape)
        {
            if (redoTop >= 400)
            {
                RemoveRedoBottom();
            }
            redoTop++;
            redoList.Add(shape);
        }

        private void RemoveRedoBottom()
        {
            redoTop--;
            redoList.RemoveAt(0);
        }

        public void AddToUndoShape(Shape shape)
        {
            if (undoTop >= 400)
            {
                RemoveUndoBottom();
            }
            undoTop++;
            UndoList.Add(shape);

        }

        private void RemoveUndoBottom()
        {
            undoTop--;
            UndoList.RemoveAt(0);
        }


        /// <summary>
        /// Add an object to canvas
        /// </summary>
        /// <param name="drawingCanvas"></param>
        /// <param name="objdata"></param>
        public void AddObjects(string objdata, string from)
        {
            try
            {
                var objects = objdata.JsonDeserialize<ScreenObject[]>();

                foreach (var obj in objects)
                {
                    if (obj.Type == ScreenObjectType.Text)
                    {
                        this.messages.Insert(0, new TalkItem()
                        {
                            Text = obj.Text,
                            Time = DateTime.Now.ToShortTimeString(),
                            From = from
                        });
                    }
                    else
                    {
                        var shp = canHelper.Object2Shape(obj);
                        if (shp != null)
                        {
                            _canvas.Children.Add(shp);
                        }
                    }
                }

            }
            catch { }
        }

        #endregion


    }


    #region TalkItem

    public class TalkItem
    {
        public string Text { get; set; }
        public string From { get; set; }
        public string Time { get; set; }
    }

    #endregion

    #region Enum
    public enum CurrentTool
    {
        Pen,
        Pencil,
        Brush,
        EraseBrush,
        Sticky,
        Ellipse,
        Rectangle
    }
    #endregion
}
