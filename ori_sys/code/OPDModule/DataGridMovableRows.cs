using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Windows.Data;

namespace CIMS
{
    /// <summary>
    /// A DataGrid control with movable rows. Select rows(s) and move the mouse over the row header of the first
    /// selection to start dragging.
    /// </summary>
    /// <remarks>
    /// The row headers are made visible in the constructor and the RowHeight is set to 22 as the default Double.NaN will not work here.
    /// </remarks>
    public class DataGridMovableRows : DataGrid, IMoveRowsTarget
    {
        private RowDragDropTracker _dndTracker;

        /// <summary>
        /// Gets or sets a value that indicates whether the end-user can reorder the rows by
        /// drag and drop. Default is true.
        /// </summary>
        /// <remarks>
        /// If you do allow reordering of rows, and when you data-bind the GanttControl to a
        /// collection, make sure to specify the SortOrderBinding to
        /// persist the new order of activites.
        /// </remarks>
        public bool CanUserReorderRows
        {
            get { return (bool)GetValue(CanUserReorderRowsProperty); }
            set { SetValue(CanUserReorderRowsProperty, value); }
        }

        public static readonly DependencyProperty CanUserReorderRowsProperty =
            DependencyProperty.Register("CanUserReorderRows", typeof(bool), typeof(DataGridMovableRows), new PropertyMetadata(true, CanUserReorderRowsChanged));

        public DataGridMovableRows()
        {
            this.HeadersVisibility = DataGridHeadersVisibility.All;
            this.Loaded += new RoutedEventHandler(DataGridMovableRows_Loaded);
            this.RowHeight = 22;
        }

        #region ROW MOVING LOGIC
        /// <summary>
        /// The Vertical ScrollBar of this control.
        /// </summary>
        public ScrollBar VerticalScrollBar { get; private set; }

        bool _loaded = false;
        void DataGridMovableRows_Loaded(object sender, RoutedEventArgs e)
        
        {
            _loaded = true;
            this.ProcessTemplate();
        }
        private bool _templateApplied = false;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._templateApplied = true;
            this.ProcessTemplate();
        }
        private void ProcessTemplate()
        {
            if (this._loaded == false || this._templateApplied == false)
                return;

            // To update scrollbar bindings
            List<ScrollBar> sbars = new List<ScrollBar>();
            Extensions.GetChildren<ScrollBar>(this, ref sbars, false, false);

            ScrollBar vertSB = null;
            foreach (ScrollBar sbar in sbars)
            {
                if (sbar.Orientation == Orientation.Vertical)
                {
                    vertSB = sbar; break;
                }
            }
            if (vertSB != null)
            {
                this.VerticalScrollBar = vertSB;
            }

            this.UpdateRowDragDropTracker();
        }
        private static void CanUserReorderRowsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((DataGridMovableRows)sender).ProcessTemplate();
        }
        private void UpdateRowDragDropTracker()
        {
            if (this.CanUserReorderRows)
            {
                if (this._dndTracker == null)
                    this._dndTracker = new RowDragDropTracker(this);
            }
            else
            {
                if (this._dndTracker != null)
                {
                    this._dndTracker.Dispose();
                    this._dndTracker = null;
                }
            }
        }
        #endregion

        #region IMoveRowsTarget Members

        public void MoveRows(int start, int count, int dest)
        {
            RowsMovedEventArgs args = new RowsMovedEventArgs(start, count, dest);
            this.OnRowsMoved(args);
        }

        public bool OnBeforeDragStart(int start, int count)
        {
            CancelEventArgs args = new CancelEventArgs();
            this.OnBeforeMovingRows(args);
            return args.Cancel;
        }

        public event RowsMovedEventHandler RowsMoved;

        protected void OnRowsMoved(RowsMovedEventArgs args)
        {
            if (this.RowsMoved != null)
                this.RowsMoved(this, args);
        }

        public event EventHandler<CancelEventArgs> BeforeMovingRows;

        protected void OnBeforeMovingRows(CancelEventArgs args)
        {
            if (this.BeforeMovingRows != null)
                this.BeforeMovingRows(this, args);
        }
        #endregion
    }
    public delegate void RowsMovedEventHandler(object sender, RowsMovedEventArgs args);

    public class RowsMovedEventArgs : EventArgs
    {
        public int StartIndex { get; private set; }
        public int Count { get; private set; }
        public int DestinationIndex { get; private set; }
        public RowsMovedEventArgs(int startIndex, int count, int endIndex)
        {
            this.StartIndex = startIndex;
            this.Count = count;
            this.DestinationIndex = endIndex;
        }
    }

    interface IMoveRowsTarget
    {
        void MoveRows(int start, int count, int dest);
        bool OnBeforeDragStart(int start, int count);
    }
    //TODO: Autoscroll during d&d
    internal class RowDragDropTracker : IDisposable
    {
        DataGridMovableRows _source;
        Grid _gridHost;
        int _gridCol, _gridRow;
        Thumb _handleThumb;
        Border _dropCue;
        DataGridRowsPresenter _presenter;
        public RowDragDropTracker(DataGridMovableRows source)
        {
            this._source = source;
            this._source.SelectionChanged += new SelectionChangedEventHandler(_source_SelectionChanged);

            this.IsHandleUnderMouse = false;

            List<DataGridRowsPresenter> presntrs = new List<DataGridRowsPresenter>();
            Extensions.GetChildren<DataGridRowsPresenter>(_source, ref presntrs, false, false);
            if (presntrs.Count > 0)
            {
                this._presenter = presntrs[0];
                this._gridCol = Grid.GetColumn(presntrs[0]);
                this._gridRow = Grid.GetRow(presntrs[0]);
            }
            else
                throw new NotSupportedException("Cannot find DataGridRowsPresenter within DataGrid. Make sure to call the RowDragDropTracker constructor after the tracked DataGrid is loaded.");

            _gridHost = Extensions.FindAncestor<Grid>(presntrs[0]);

            if (_gridHost == null)
                throw new NotSupportedException("Cannot find the Grid parent of the DataGridRowsPresenter within DataGrid. Make sure to call the RowDragDropTracker constructor after the tracked DataGrid is loaded.");

            this._source.VerticalScrollBar.Scroll += new ScrollEventHandler(VerticalScrollBar_Scroll);

            //this._handleThumb = new Ellipse() { Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 2, 
            //    Fill = new SolidColorBrush(Colors.Black), Height = 10, Width = 10, Visibility= Visibility.Collapsed ,
            //    HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top};
            this._handleThumb = new Thumb() { Background = new SolidColorBrush(Colors.Transparent), Visibility = Visibility.Collapsed };
            this._handleThumb.Foreground = new SolidColorBrush(Colors.Transparent);
            this._handleThumb.Template = (ControlTemplate)XamlReader.Load(@"<ControlTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" TargetType=""Thumb"">
                                                                        <Canvas Background=""Transparent"" IsHitTestVisible=""true"">
                                                                        <Path Stroke=""{TemplateBinding Foreground}"" HorizontalAlignment=""Left"" VerticalAlignment=""Top""
                                                                                  Data=""M 0,8 L 6,8      M 8,6 L 8,0      M 10,8 L 16,8     M 8,10 L 8,16 
                                                                                         M 3,5 0,8 L 3,12 M 5,3 8,0 L 11,3 M 13,5 16,8 13,11 M 5,13 8,16 11,13""></Path>
                                                                        </Canvas>
                                                                    </ControlTemplate>"
                                                                );
            //            <Ellipse Stroke=""Black"" StrokeThickness=""2"" Fill=""Black"" Height=""10"" Width=""10""
            //         HorizontalAlignment=""Left"" VerticalAlignment=""Top"">
            //</Ellipse>
            this._handleThumb.MouseEnter += new MouseEventHandler(_handleThumb_MouseEnter);
            this._handleThumb.MouseLeave += new MouseEventHandler(_handleThumb_MouseLeave);
            this._handleThumb.DragStarted += new DragStartedEventHandler(_handleThumb_DragStarted);
            this._handleThumb.DragDelta += new DragDeltaEventHandler(_handleThumb_DragDelta);
            this._handleThumb.DragCompleted += new DragCompletedEventHandler(_handleThumb_DragCompleted);
            this._handleThumb.Height = this._source.RowHeight;
            Grid.SetColumn(this._handleThumb, this._gridCol);
            Grid.SetRow(this._handleThumb, this._gridRow);
            this._handleThumb.VerticalAlignment = VerticalAlignment.Top;
            this._gridHost.Children.Add(this._handleThumb);

            this._dropCue = new Border()
            {
                Height = 2,
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Collapsed
            };

            Grid.SetColumn(this._dropCue, this._gridCol + 1);
            Grid.SetRow(this._dropCue, this._gridRow);

            this._gridHost.Children.Add(this._dropCue);

            this._source_SelectionChanged(null, null);
        }


        #region IDisposable Members

        public void Dispose()
        {
            this._source.SelectionChanged -= new SelectionChangedEventHandler(_source_SelectionChanged);
            this._source.VerticalScrollBar.Scroll -= new ScrollEventHandler(VerticalScrollBar_Scroll);

            this._gridHost.Children.Remove(this._handleThumb);
            this._gridHost.Children.Remove(this._dropCue);

            if (this._scrollValueTracker != null)
            {
                this._scrollValueTracker.ValueChanged -= new EventHandler(scrollValueTracker_ValueChanged);
                this._scrollValueTracker.Dispose();
            }
        }

        #endregion
        bool IsHandleUnderMouse { get; set; }
        void _handleThumb_MouseLeave(object sender, MouseEventArgs e)
        {
            this.IsHandleUnderMouse = false;
            this.UpdateThumbForeground();
        }

        void _handleThumb_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this._source.IsReadOnly)
                return;

            this.IsHandleUnderMouse = true;
            this.UpdateThumbForeground();
        }

        private void UpdateThumbForeground()
        {
            if (this.IsHandleUnderMouse && !this._handleThumb.IsDragging)
                this._handleThumb.Foreground = new SolidColorBrush(Colors.Black);
            else
                this._handleThumb.Foreground = new SolidColorBrush(Colors.Transparent);
        }

        void scrollValueTracker_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateCueRow();
            this.UpdateDragHandle();
        }

        ScrollValueTracker _scrollValueTracker = null;
        void VerticalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            // Hack to listen to Value changed.
            // This also requires to be done repeatedly, for some reason, to keep the binding in sync.
            double rowsScrolled = this._source.VerticalScrollBar.Value / this._source.RowHeight;
            if (rowsScrolled != Math.Floor(rowsScrolled))
            {
                this.SetupScrollBarValueTracker();
            }

            this.UpdateCueRow();
            this.UpdateDragHandle();
        }
        void SetupScrollBarValueTracker()
        {
            if (this._scrollValueTracker != null)
            {
                this._scrollValueTracker.ValueChanged -= new EventHandler(scrollValueTracker_ValueChanged);
                this._scrollValueTracker.Dispose();
            }
            Binding binding = new Binding("Value");
            //_scrollValueTracker = new ValueBinder<double>(this._source.VerticalScrollBar, binding, BindingMode.OneWay);
            _scrollValueTracker = new ScrollValueTracker(this._source.VerticalScrollBar);
            _scrollValueTracker.ValueChanged += new EventHandler(scrollValueTracker_ValueChanged);
        }

        void _handleThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (this._source.IsReadOnly)
                return;

            this.UpdateThumbForeground();
            //&& this.CurrentCueRowIndex != this.TopSelectedIndex + 1
            if (e.Canceled == false && this.CurrentCueRowIndex != -1
                // Possible when user started but didn't drag at all
                && this.CurrentCueRowIndex != this.TopSelectedIndex)
            {
                // Cache selected items.
                List<object> sel_items = new List<object>();
                foreach (object o in this._source.SelectedItems)
                    sel_items.Add(o);

                int dest = this.CurrentCueRowIndex;
                double scrollbarMax = this._source.VerticalScrollBar.Maximum;
                try
                {
                    this._ignoreSelection = true;
                    // Notify Move

                    this._source.MoveRows(this.TopSelectedIndex, this._source.SelectedItems.Count,
                        dest);

                    this.TopSelectedIndex = -1;
                    // Reselect selected items.
                    this._source.SelectedItems.Clear();
                    foreach (object o in sel_items)
                        this._source.SelectedItems.Add(o);
                }
                finally
                {
                    //this._source.VerticalScrollBar.Maximum = scrollbarMax;
                    this._ignoreSelection = false;
                    this.TopSelectedIndex = dest;
                    this.SetupScrollBarValueTracker();
                    this._source_SelectionChanged(null, null);
                }
            }
            this.CurrentCueRowIndex = -1;
        }
        
        void _handleThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this._source.IsReadOnly)
                return;

            this._cumDelta += e.VerticalChange;
            int inRows = (int)(_cumDelta / this._source.RowHeight);

            //While moving down:
            if (_cumDelta > 0)
                inRows++;

            System.Diagnostics.Debug.WriteLine("TopSelectedIndex: " + this.TopSelectedIndex
                + "; inRows=" + inRows);
            // If moving into the DataGrid's white space at the bottom:
            if (this._handleThumb.Margin.Top + this._cumDelta > _presenter.DesiredSize.Height)
                return;

            int newCueRowIndex = this.TopSelectedIndex + inRows;

            if (newCueRowIndex < 0)
                newCueRowIndex = 0;

            if (newCueRowIndex >= this.TopSelectedIndex &&
                newCueRowIndex <= (this.TopSelectedIndex + this._source.SelectedItems.Count)
                || newCueRowIndex < 0)
                // Insetead of -1, pointing to the original position.
                this.CurrentCueRowIndex = this.TopSelectedIndex;
            else
                this.CurrentCueRowIndex = newCueRowIndex;

            System.Diagnostics.Debug.WriteLine("CurrentCueRowIndex: " + this.CurrentCueRowIndex.ToString());
        }
        private double _cumDelta = 0;
        void _handleThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (this._source.IsReadOnly)
                return;

            this.UpdateThumbForeground();
            this._cumDelta = 0;
            this.CurrentCueRowIndex = this.TopSelectedIndex;
            bool cancel = this._source.OnBeforeDragStart(this.TopSelectedIndex, this._source.SelectedItems.Count);
            if (cancel)
            {
                this._handleThumb.CancelDrag();
            }
        }

        private bool _ignoreSelection = false;
        private int _selAnchor = -1;
        void _source_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this._ignoreSelection || this._source.IsReadOnly)
                return;

            // multiple selection
            if (this._source.SelectedItems.Count > 1)
            {

                int curSel = this._source.SelectedIndex;
                if (this.TopSelectedIndex < curSel)
                    this.TopSelectedIndex = curSel > _selAnchor ? _selAnchor : curSel;
                else
                    this.TopSelectedIndex = curSel;

            }
            else if (this._source.SelectedItems.Count == 1)// single selection
            {
                this._selAnchor = this._source.SelectedIndex;
                this.TopSelectedIndex = this._source.SelectedIndex;
            }
            else // no selection
            {
                this._selAnchor = -1;
                this.TopSelectedIndex = -1;
            }
        }

        private int _topSelIndex = -1;
        private int TopSelectedIndex
        {
            get { return this._topSelIndex; }
            set
            {
                if (this._topSelIndex != value)
                {
                    this._topSelIndex = value;
                    this.UpdateDragHandle();
                }
            }
        }

        private void UpdateDragHandle()
        {
            if (this.TopSelectedIndex != -1)
            {

                double rowtop = (this._source.RowHeight * this.TopSelectedIndex);

                this._handleThumb.Margin = new Thickness(2, rowtop + 3 - this._source.VerticalScrollBar.Value, 0, 0);

                // Don't show if it's out of view on top.
                if (this._handleThumb.Margin.Top < 0)
                    this._handleThumb.Visibility = Visibility.Collapsed;
                else
                    this._handleThumb.Visibility = Visibility.Visible;

                System.Diagnostics.Debug.WriteLine("ScrollBar.Value: " + this._source.VerticalScrollBar.Value);
            }
            else
            {
                this._handleThumb.Visibility = Visibility.Collapsed;
            }
        }
        private int _curCueRowIndex = -1;
        private int CurrentCueRowIndex
        {
            get { return this._curCueRowIndex; }
            set
            {
                if (this._curCueRowIndex != value)
                {
                    this._curCueRowIndex = value;
                    this.UpdateCueRow();
                }
            }
        }
        private void UpdateCueRow()
        {
            if (this.CurrentCueRowIndex != -1)
            {
                double rowtop = (this._source.RowHeight * this.CurrentCueRowIndex);

                // -1 to draw it right at the row borders to take care of border conditions.
                this._dropCue.Margin = new Thickness(0, rowtop - this._source.VerticalScrollBar.Value - 1, 0, 0);
                this._dropCue.Visibility = Visibility.Visible;
            }
            else
            {
                this._dropCue.Visibility = Visibility.Collapsed;
            }
        }

    }

}
