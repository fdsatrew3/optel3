﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace Braincase.GanttChart
{
    static class GDIExtention
    {
        public static void DrawRectangle(this Graphics graphics, Pen pen, RectangleF rectangle)
        {
            graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static RectangleF TextBoxAlign(this Graphics graphics, string text, ChartTextAlign align, Font font, RectangleF textbox, float margin = 0)
        {
            var size = graphics.MeasureString(text, font);
            if (align == ChartTextAlign.MiddleCenter)
            {
                return new RectangleF(new PointF(textbox.Left + (textbox.Width - size.Width) / 2, textbox.Top + (textbox.Height - size.Height) / 2), size);
            }
            else if (align == ChartTextAlign.MiddleLeft)
            {
                return new RectangleF(new PointF(textbox.Left + margin, textbox.Top + (textbox.Height - size.Height) / 2), size);
            }
            else
            {
                throw new NotImplementedException("Need to implement more alignment types");
            }
        }
    }

    /// <summary>
    /// Gantt Chart control
    /// </summary>
    public partial class Chart : UserControl
    {
        #region Public Methods

        /// <summary>
        /// Construct a gantt chart
        /// </summary>
        public Chart()
        {
            // Designer values
            InitializeComponent();

            // Factory values
            HeaderOneHeight = 32;
            HeaderTwoHeight = 20;
            BarSpacing = 32;
            BarHeight = 20;
            MajorWidth = 140;
            MinorWidth = 20;
            TimeResolution = TimeResolution.Day;
            this.DoubleBuffered = true;
            _mViewport = new ControlViewport(this) { WheelDelta = BarSpacing };
            AllowTaskDragDrop = true;
            ShowRelations = true;
            ShowSlack = false;
            AccumulateRelationsOnGroup = false;
            ShowTaskLabels = true;
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0, 0, 0, 0);
            this.Padding = new Padding(0, 0, 0, 0);
            // Formatting
            Color contentColor = Color.FromArgb(255, 61, 185, 211);
            Brush contentColorBrush = new SolidBrush(contentColor);
            Color borderColor = Color.FromArgb(255, 40, 152, 176);
            var taskFormat = new TaskFormat()
            {
                Color = Brushes.Black,
                Border = new Pen(borderColor),
                BackFill = contentColorBrush,
                ForeFill = contentColorBrush,
                SlackFill = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal, contentColor, contentColor)
            };
            TaskFormat = taskFormat;
            CriticalTaskFormat = taskFormat;
            HeaderFormat = new GanttChart.HeaderFormat()
            {
                Color = Brushes.Black,
                Border = new Pen(SystemColors.ActiveBorder),
                GradientLight = SystemColors.ButtonHighlight,
                GradientDark = SystemColors.ButtonFace
            };
        }

        /// <summary>
        /// Delegate method for creating a new Task. Creates Task by default.
        /// </summary>
        public Func<Task> CreateTaskDelegate = delegate () { return new Task(); };

        /// <summary>
        /// Get the selected tasks.
        /// Split tasks will not be in this list, only its task parts, if selected.
        /// </summary>
        public IEnumerable<Task> SelectedTasks
        {
            get
            {
                return _mSelectedTasks.ToArray();
            }
        }

        /// <summary>
        /// Get the latest selected task
        /// </summary>
        public Task SelectedTask
        {
            get
            {
                return _mSelectedTasks.LastOrDefault();
            }
        }

        /// <summary>
        /// Get or set header1 pixel height
        /// </summary>
        [DefaultValue(32)]
        public int HeaderOneHeight { get; set; }

        /// <summary>
        /// Get or set header2 pixel height
        /// </summary>
        [DefaultValue(20)]
        public int HeaderTwoHeight { get; set; }

        /// <summary>
        /// Get or set pixel distance from top of each Task to the next
        /// </summary>
        [DefaultValue(32)]
        public int BarSpacing { get; set; }

        /// <summary>
        /// Get or set pixel height of each Task
        /// </summary>
        [DefaultValue(20)]
        public int BarHeight { get; set; }

        /// <summary>
        /// Get or set the time scale display format
        /// </summary>
        [DefaultValue(TimeResolution.Day)]
        public TimeResolution TimeResolution { get; set; }

        /// <summary>
        /// Get or set the pixel width of each step of the time scale e.g. if TimeScale is TimeScale.Day, then each Day will be TimeWidth pixels apart
        /// </summary>
        [DefaultValue(20)]
        public int MinorWidth { get; set; }

        /// <summary>
        /// Get or set pixel width between major tick marks.
        /// </summary>
        [DefaultValue(140)]
        public int MajorWidth { get; set; }

        /// <summary>
        /// Get or set format for Tasks
        /// </summary>
        public TaskFormat TaskFormat { get; set; }

        /// <summary>
        /// Get or set format for critical Tasks
        /// </summary>
        public TaskFormat CriticalTaskFormat { get; set; }

        /// <summary>
        /// Get or set format for headers
        /// </summary>
        public HeaderFormat HeaderFormat { get; set; }

        /// <summary>
        /// Get or set format for relations
        /// </summary>
        public RelationFormat RelationFormat { get; set; }

        /// <summary>
        /// Get or set whether dragging of Tasks is allowed. Set to false when not dragging to skip drag(drop) tracking.
        /// </summary>
        [DefaultValue(true)]
        public bool AllowTaskDragDrop { get; set; }

        /// <summary>
        /// Get or set whether to show relations
        /// </summary>
        [DefaultValue(true)]
        public bool ShowRelations { get; set; }

        /// <summary>
        /// Get or set whether to show task labels
        /// </summary>
        [DefaultValue(true)]
        public bool ShowTaskLabels { get; set; }

        /// <summary>
        /// Get or set whether to accumulate relations on group tasks and show relations even when group is collapsed. (Not working well; still improving on it)
        /// </summary>
        [DefaultValue(false)]
        public bool AccumulateRelationsOnGroup { get; set; }

        /// <summary>
        /// Get or set whether to show slack
        /// </summary>
        [DefaultValue(false)]
        public bool ShowSlack { get; set; }

        /// <summary>
        /// Occurs when the mouse is moving over a Task
        /// </summary>
        public event EventHandler<TaskMouseEventArgs> TaskMouseOver = null;

        /// <summary>
        /// Occurs when the mouse leaves a Task
        /// </summary>
        public event EventHandler<TaskMouseEventArgs> TaskMouseOut = null;

        /// <summary>
        /// Occurs when a Task is clicked
        /// </summary>
        public event EventHandler<TaskMouseEventArgs> TaskMouseClick = null;

        /// <summary>
        /// Occurs when a Task is double clicked by the mouse
        /// </summary>
        public event EventHandler<TaskMouseEventArgs> TaskMouseDoubleClick = null;

        /// <summary>
        /// Occurs when a Task is being dragged by the mouse
        /// </summary>
        public event EventHandler<TaskDragDropEventArgs> TaskMouseDrag = null;

        /// <summary>
        /// Occurs when a dragged Task is being dropped by releasing any previously pressed mouse button.
        /// </summary>
        public event EventHandler<TaskDragDropEventArgs> TaskMouseDrop = null;

        /// <summary>
        /// Occurs when a task is selected.
        /// </summary>
        public event EventHandler<TaskMouseEventArgs> TaskSelected = null;

        /// <summary>
        /// Occurs before one or more tasks are being deselected. All Task in Chart.SelectedTasks will be deselected.
        /// </summary>
        public event EventHandler<TaskMouseEventArgs> TaskDeselecting = null;

        /// <summary>
        /// Occurs before a Task gets painted
        /// </summary>
        public event EventHandler<TaskPaintEventArgs> PaintTask = null;

        /// <summary>
        /// Occurs before overlays get painted
        /// </summary>
        public event EventHandler<ChartPaintEventArgs> PaintOverlay = null;

        /// <summary>
        /// Occurs before the header gets painted
        /// </summary>
        public event EventHandler<HeaderPaintEventArgs> PaintHeader = null;

        /// <summary>
        /// Occurs before the header date tick mark gets painted
        /// </summary>
        public event EventHandler<TimelinePaintEventArgs> PaintTimeline = null;

        /// <summary>
        /// Get the line number of the specified task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool TryGetRow(Task task, out int row)
        {
            row = 0;
            if (_mChartTaskHitRects.ContainsKey(task))
            {
                // collection contains parts
                row = _ChartCoordToChartRow(_mChartTaskHitRects[task].Top);
                return true;
            }
            else if (_mChartTaskRects.ContainsKey(task))
            {
                // collectino contains splits
                row = _ChartCoordToChartRow(_mChartTaskRects[task].Top);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the task at the specified line number
        /// </summary>
        /// <param name="row"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool TryGetTask(int row, out Task task)
        {
            task = null;
            if (row > 0 && row < _mProject.Tasks.Count())
            {
                task = _mChartTaskRects.ElementAtOrDefault(row - 1).Key;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Initialize this Chart with a Project
        /// </summary>
        /// <param name="project"></param>
        public void Init(ProjectManager<Task, object> project)
        {
            _mProject = project;
            _GenerateModels();
        }

        /// <summary>
        /// Print the Chart to the specified PrintDocument.
        /// </summary>
        public void Print(PrintDocument document, float scale = 1.0f)
        {
            // save a copy of the current viewport and swap it with PrintViewport
            var viewport = _mViewport;

            float x = 0; // viewport world x, y coords
            float y = 0;
            int pageCount = 0;

            document.PrintPage += (s, e) =>
            {
                e.HasMorePages = false;
                pageCount++;

                // create a PrintViewport to navigate the world
                var printViewport = new PrintViewport(e.Graphics,
                    viewport.WorldWidth, viewport.WorldHeight,
                    e.MarginBounds.Width, e.MarginBounds.Height,
                    e.PageSettings.Margins.Left, e.PageSettings.Margins.Right)
                { Scale = scale };
                _mViewport = printViewport;

                // move the viewport
                printViewport.X = x;
                printViewport.Y = y;

                // set clip and draw
                e.Graphics.SetClip(e.MarginBounds);
                _Draw(e.Graphics, e.PageBounds);

                // check if reached end of printing
                if (printViewport.Rectangle.Right < printViewport.WorldWidth)
                {
                    // continue horizontally
                    x += printViewport.Rectangle.Width;
                    e.HasMorePages = true;
                }
                else
                {
                    // reached end of worldwidth so we go down vertically once
                    x = 0;
                    if (printViewport.Rectangle.Bottom < printViewport.WorldHeight)
                    {
                        y += printViewport.Rectangle.Height;
                        e.HasMorePages = true;
                    }
                }
            };
            document.Print();

            // restore the viewport 
            _mViewport = viewport;
        }

        /// <summary>
        /// Print the Chart to an Image
        /// </summary>
        /// <param name="scale">Scale to print the image at.</param>
        public Bitmap Print(float scale = 1.0f)
        {
            var viewport = _mViewport;
            _mViewport = new ImageViewport(scale, viewport.WorldWidth, viewport.WorldHeight);

            Bitmap image = new Bitmap((int)Math.Ceiling(viewport.WorldWidth * scale), (int)Math.Ceiling(viewport.WorldHeight * scale));
            var graphics = Graphics.FromImage(image);

            _Draw(graphics, Rectangle.Ceiling(viewport.Rectangle));

            _mViewport = viewport;

            return image;
        }

        /// <summary>
        /// Get information about the chart area at the mouse coordinate of the chart
        /// </summary>
        /// <param name="mouse"></param>
        /// <returns></returns>
        public ChartInfo GetChartInfo(Point mouse)
        {
            var row = _ChartCoordToChartRow(mouse.Y);
            var col = _GetDeviceColumnUnderMouse(mouse);
            var task = _GetTaskUnderMouse(mouse);
            return new ChartInfo(row, _mHeaderInfo.DateTimes[col], task);
        }

        /// <summary>
        /// Set tool tip for the specified task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="text"></param>
        public void SetToolTip(Task task, string text)
        {
            if (task != null && text != string.Empty)
                _mTaskToolTip[task] = text;
        }

        /// <summary>
        /// Get tool tip currently set for the specified task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public string GetToolTip(Task task)
        {
            if (task != null)
                return _mTaskToolTip[task];
            else
                return string.Empty;
        }

        /// <summary>
        /// Clear tool tip for the specified task
        /// </summary>
        /// <param name="task"></param>
        public void ClearToolTip(Task task)
        {
            if (task != null)
                _mTaskToolTip.Remove(task);
        }

        /// <summary>
        /// Clear all tool tips
        /// </summary>
        public void ClearToolTips()
        {
            _mTaskToolTip.Clear();
        }

        /// <summary>
        /// Scroll to the specified DateTime
        /// </summary>
        /// <param name="datetime"></param>
        public void ScrollTo(DateTime datetime)
        {
            TimeSpan span = datetime - _mProject.Start;
            _mViewport.X = GetSpan(span);
        }

        /// <summary>
        /// Scroll to the specified task
        /// </summary>
        /// <param name="task"></param>
        public void ScrollTo(Task task)
        {
            if (_mChartTaskRects.ContainsKey(task))
            {
                var rect = _mChartTaskRects[task];
                _mViewport.X = rect.Left - this.MinorWidth;
                _mViewport.Y = rect.Top - this.HeaderOneHeight - this.HeaderTwoHeight;
            }
        }

        /// <summary>
        /// Begin billboard mode. Graphics must orginate from Chart and be same as that used in EndBillboardMode.
        /// </summary>
        /// <param name="graphics"></param>
        public void BeginBillboardMode(Graphics graphics)
        {
            graphics.Transform = ControlViewport.Identity;
        }

        /// <summary>
        /// End billboard mode. Graphics must orginate from Chart and be same as that used in BeginBillboardMode.
        /// </summary>
        /// <param name="graphics"></param>
        public void EndBillboardMode(Graphics graphics)
        {
            graphics.Transform = _mViewport.Projection;
        }

        /// <summary>
        /// Convert the specified timespan to pixels units of the Chart x-coordinates
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public float GetSpan(TimeSpan span)
        {
            double pixels = 0;
            switch (TimeResolution)
            {
                case TimeResolution.Day:
                    pixels = span.TotalDays * (double)MinorWidth;
                    break;
                case TimeResolution.Week:
                    pixels = span.TotalDays / 7f * (double)MinorWidth;
                    break;
                case TimeResolution.Hour:
                    pixels = span.TotalHours * (double)MinorWidth;
                    break;
            }

            return (float)pixels;
        }

        /// <summary>
        /// Convert the pixel units of the Chart x-coordinates to TimeSpan
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        public TimeSpan GetSpan(float dx)
        {
            TimeSpan span = TimeSpan.MinValue;
            switch (TimeResolution)
            {
                case TimeResolution.Day:
                    span = TimeSpan.FromDays(dx / MinorWidth);
                    break;
                case TimeResolution.Week:
                    span = TimeSpan.FromDays(dx / MinorWidth * 7f);
                    break;
                case TimeResolution.Hour:
                    span = TimeSpan.FromHours(dx / MinorWidth);
                    break;
            }
            return span;
        }

        #endregion Public Methods


        #region UserControl Events

        /// <summary>
        /// Raises the System.Windows.Forms.Control.Paint event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!this.DesignMode)
                this._Draw(e.Graphics, e.ClipRectangle);
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.MouseMove event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Hot tracking
            var task = _GetTaskUnderMouse(e.Location);
            if (_mMouseEntered != null && task == null)
            {
                OnTaskMouseOut(new TaskMouseEventArgs(_mMouseEntered, RectangleF.Empty, e.Button, e.Clicks, e.X, e.Y, e.Delta));
                _mMouseEntered = null;

            }
            else if (_mMouseEntered == null && task != null)
            {
                _mMouseEntered = task;
                OnTaskMouseOver(new TaskMouseEventArgs(_mMouseEntered, _mChartTaskHitRects[task], e.Button, e.Clicks, e.X, e.Y, e.Delta));
            }

            // Dragging
            if (AllowTaskDragDrop && _mDraggedTask != null)
            {
                Task target = task;
                if (target == _mDraggedTask) target = null;
                RectangleF targetRect = target == null ? RectangleF.Empty : _mChartTaskHitRects[target];
                int row = _DeviceCoordToChartRow(e.Location.Y);
                OnTaskMouseDrag(new TaskDragDropEventArgs(_mDragTaskStartLocation, _mDragTaskLastLocation, _mDraggedTask, _mChartTaskHitRects[_mDraggedTask], target, targetRect, row, e.Button, e.Clicks, e.X, e.Y, e.Delta));
                _mDragTaskLastLocation = e.Location;
            }
            else if (_mDraggedTask == null && e.Button == MouseButtons.Middle) // panning mode
            {
                this.Cursor = Cursors.SizeAll;
                _mViewport.X -= e.X - _mPanViewLastLocation.X;
                _mViewport.Y -= e.Y - _mPanViewLastLocation.Y;
                _mPanViewLastLocation = e.Location;
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.MouseClick event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            var task = _GetTaskUnderMouse(e.Location);
            if (task != null)
            {
                OnTaskMouseClick(new TaskMouseEventArgs(task, _mChartTaskHitRects[task], e.Button, e.Clicks, e.X, e.Y, e.Delta));
            }
            else
            {
                OnTaskDeselecting(new TaskMouseEventArgs(task, RectangleF.Empty, e.Button, e.Clicks, e.X, e.Y, e.Delta));
            }
            base.OnMouseClick(e);
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.MouseDown event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Begin Drag
            _mDragTaskStartLocation = e.Location;
            _mDragTaskLastLocation = e.Location;
            _mPanViewLastLocation = e.Location;

            if (AllowTaskDragDrop)
            {
                _mDraggedTask = _GetTaskUnderMouse(e.Location);
                //if (_mDragSource != null)
                //{
                //    _mDragStartLocation = e.Location;
                //    _mDragLastLocation = e.Location;
                //}
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.MouseUp event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // reset cursor to handle end of panning mode;
            this.Cursor = Cursors.Default;

            // Drop task
            if (AllowTaskDragDrop && _mDraggedTask != null)
            {
                var target = _GetTaskUnderMouse(e.Location);
                if (target == _mDraggedTask) target = null;
                var targetRect = target == null ? RectangleF.Empty : _mChartTaskHitRects[target];
                int row = _DeviceCoordToChartRow(e.Location.Y);
                OnTaskMouseDrop(new TaskDragDropEventArgs(_mDragTaskStartLocation, _mDragTaskLastLocation, _mDraggedTask, _mChartTaskHitRects[_mDraggedTask], target, targetRect, row, e.Button, e.Clicks, e.X, e.Y, e.Delta));
                _mDraggedTask = null;
                _mDragTaskLastLocation = Point.Empty;
                _mDragTaskStartLocation = Point.Empty;
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.MouseDoubleClick event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            var task = _GetTaskUnderMouse(e.Location);
            if (task != null)
            {
                OnTaskMouseDoubleClick(new TaskMouseEventArgs(task, _mChartTaskHitRects[task], e.Button, e.Clicks, e.X, e.Y, e.Delta));
            }

            base.OnMouseDoubleClick(e);
        }

        #endregion UserControl Events

        #region Chart Events

        /// <summary>
        /// Raises the TaskMouseOver event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTaskMouseOver(TaskMouseEventArgs e)
        {
            TaskMouseOver?.Invoke(this, e);

            this.Cursor = Cursors.Hand;

            var task = e.Task;
            if (_mProject.IsPart(e.Task)) task = _mProject.SplitTaskOf(task);
            if (_mTaskToolTip.ContainsKey(task))
            {
                _mOverlay.ShowToolTip(_mViewport.DeviceToWorldCoord(e.Location), _mTaskToolTip[task]);
                this.Invalidate();
            }
        }
        /// <summary>
        /// Raises the TaskMouseOver event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTaskMouseOut(TaskMouseEventArgs e)
        {
            TaskMouseOut?.Invoke(this, e);

            this.Cursor = Cursors.Default;

            _mOverlay.HideToolTip();
            this.Invalidate();
        }
        /// <summary>
        /// Raises the TaskMouseDrag( event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTaskMouseDrag(TaskDragDropEventArgs e)
        {
            // fire listeners
            TaskMouseDrag?.Invoke(this, e);
            if (AllowTaskDragDrop == false)
            {
                return;
            }
            // Default drag behaviors **********************************
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                var complete = e.Source.Complete + (float)(e.X - e.PreviousLocation.X) / GetSpan(e.Source.Duration);
                _mProject.SetComplete(e.Source, complete);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (e.Target == null)
                {
                    var delta = (e.PreviousLocation.X - e.StartLocation.X);
                    _mOverlay.DraggedRect = e.SourceRect;
                    _mOverlay.DraggedRect.Width += delta;
                }
                else // drop targetting (join)
                {
                    _mOverlay.DraggedRect = e.TargetRect;
                    _mOverlay.Row = int.MinValue;
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _mOverlay.Clear();

                if (e.Target == null)
                {
                    if (Control.ModifierKeys.HasFlag(Keys.Shift))
                    {
                        // insertion line
                        _mOverlay.Row = e.Row;
                    }
                    else
                    {
                        // displacing horizontally
                        _mOverlay.DraggedRect = e.SourceRect;
                        _mOverlay.DraggedRect.Offset((e.X - e.StartLocation.X), 0);
                    }
                }
                else // drop targetting (subtask / predecessor)
                {
                    _mOverlay.DraggedRect = e.TargetRect;
                    _mOverlay.Row = int.MinValue;
                }
            }
            this.Invalidate();
        }
        /// <summary>
        /// Raises the TaskMouseDrop event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTaskMouseDrop(TaskDragDropEventArgs e)
        {
            // Fire event
            TaskMouseDrop?.Invoke(this, e);
            if (AllowTaskDragDrop == false)
            {
                return;
            }
            var delta = (e.PreviousLocation.X - e.StartLocation.X);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.Target == null)
                {
                    if (Control.ModifierKeys.HasFlag(Keys.Shift))
                    {
                        // insert
                        Task source = e.Source;
                        if (_mProject.IsPart(source)) source = _mProject.SplitTaskOf(source);
                        if (this.TryGetRow(source, out int from))
                            _mProject.Move(source, e.Row - from);
                    }
                    else
                    {
                        // displace horizontally
                        var start = e.Source.Start + GetSpan(delta);
                        _mProject.SetStart(e.Source, start);
                    }
                }
                else // have drop target
                {
                    if (Control.ModifierKeys.HasFlag(Keys.Shift))
                    {
                        _mProject.Relate(e.Target, e.Source);
                    }
                    else if (Control.ModifierKeys.HasFlag(Keys.Alt))
                    {
                        var source = e.Source;
                        if (_mProject.IsPart(source)) source = _mProject.SplitTaskOf(source);
                        if (_mProject.DirectGroupOf(source) == e.Target)
                        {
                            _mProject.Ungroup(e.Target, e.Source);
                        }
                        else
                        {
                            _mProject.Unrelate(e.Target, source);
                        }
                    }
                    else
                    {
                        _mProject.Group(e.Target, e.Source);
                    }
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (e.Target == null)
                {
                    var duration = e.Source.Duration + GetSpan(delta);
                    _mProject.SetDuration(e.Source, duration);
                }
                else // have target then we do a join
                {
                    _mProject.Join(e.Target, e.Source);
                }
            }

            _mOverlay.Clear();
            this.Invalidate();
        }
        /// <summary>
        /// Raises the TaskMouseClick event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTaskMouseClick(TaskMouseEventArgs e)
        {
            TaskMouseClick?.Invoke(this, e);
            if (AllowTaskDragDrop == false)
            {
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (ModifierKeys.HasFlag(Keys.Shift)) // activate multi-select
                {
                    if (!_mSelectedTasks.Remove(e.Task))
                    {
                        _mSelectedTasks.Add(e.Task);
                    }
                }
                else
                {
                    OnTaskDeselecting(e);
                    _mSelectedTasks.Add(e.Task);
                }
                OnTaskSelected(e);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                if (ModifierKeys.HasFlag(Keys.Shift))
                {
                    var newtask = CreateTaskDelegate();
                    _mProject.Add(newtask);
                    _mProject.SetStart(newtask, e.Task.Start);
                    _mProject.SetDuration(newtask, new TimeSpan(5, 0, 0, 0));
                    if (_mProject.IsPart(e.Task)) _mProject.Move(newtask, _mProject.IndexOf(_mProject.SplitTaskOf(e.Task)) + 1 - _mProject.IndexOf(newtask));
                    else _mProject.Move(newtask, _mProject.IndexOf(e.Task) + 1 - _mProject.IndexOf(newtask));
                }
                else if (Control.ModifierKeys.HasFlag(Keys.Alt))
                    _mProject.Delete(e.Task);
            }
            this.Invalidate();
        }
        /// <summary>
        /// Raises the TaskMouseDoubleClick event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTaskMouseDoubleClick(TaskMouseEventArgs e)
        {
            TaskMouseDoubleClick?.Invoke(this, e);
            if (AllowTaskDragDrop == false)
            {
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                e.Task.IsCollapsed = !e.Task.IsCollapsed;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                TimeSpan duration = GetSpan(_mViewport.DeviceToWorldCoord(e.Location).X - e.Rectangle.Left);
                if (_mProject.IsPart(e.Task)) _mProject.Split(e.Task, CreateTaskDelegate(), duration);
                else _mProject.Split(e.Task, CreateTaskDelegate(), CreateTaskDelegate(), duration);
            }

            this.Invalidate();
        }
        /// <summary>
        /// Raises the TaskSelected event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTaskSelected(TaskMouseEventArgs e)
        {
            TaskSelected?.Invoke(this, e);
        }
        /// <summary>
        /// Raises the TaskDeselecting event and then clear all the selected tasks
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTaskDeselecting(TaskMouseEventArgs e)
        {
            TaskDeselecting?.Invoke(this, e);

            // deselect all tasks
            _mSelectedTasks.Clear();
        }
        /// <summary>
        /// Raises the PaintOverlay event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPaintOverlay(ChartPaintEventArgs e)
        {
            if (this.PaintOverlay != null)
                PaintOverlay(this, e);
        }
        /// <summary>
        /// Raises the PaintTickMark event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPaintTimeline(TimelinePaintEventArgs e)
        {
            PaintTimeline?.Invoke(this, e);
        }
        /// <summary>
        /// Raises the PaintHeader event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPaintHeader(HeaderPaintEventArgs e)
        {
            PaintHeader?.Invoke(this, e);
        }

        #endregion Chart Events

        #region OverlayPainter
        private ChartOverlay _mOverlay = new ChartOverlay();
        class ChartOverlay
        {
            public void Paint(ChartPaintEventArgs e)
            {
                var g = e.Graphics;
                var chart = e.Chart;

                // dragging outline / trail
                if (DraggedRect != RectangleF.Empty)
                    g.DrawRectangle(Pens.Red, DraggedRect);

                // insertion indicator line
                if (Row != int.MinValue)
                {
                    float y = e.Chart._ChartRowToChartCoord(Row) + e.Chart.BarSpacing / 2.0f;
                    g.DrawLine(Pens.CornflowerBlue, new PointF(0, y), new PointF(e.Chart.Width, y));
                }

                // tool tip
                if (_mToolTipMouse != Point.Empty && _mToolTipText != string.Empty)
                {
                    var size = g.MeasureString(_mToolTipText, chart.Font).ToSize();
                    var tooltiprect = new RectangleF(_mToolTipMouse, size);
                    tooltiprect.Offset(0, -tooltiprect.Height);
                    var textstart = new PointF(tooltiprect.Left, tooltiprect.Top);
                    tooltiprect.Inflate(5, 5);
                    g.FillRectangle(Brushes.LightYellow, tooltiprect);
                    g.DrawString(_mToolTipText, chart.Font, Brushes.Black, textstart);
                }
            }

            public void ShowToolTip(PointF worldcoord, string text)
            {
                _mToolTipMouse = worldcoord;
                _mToolTipText = text;
            }

            public void HideToolTip()
            {
                _mToolTipMouse = Point.Empty;
                _mToolTipText = string.Empty;
            }

            public void Clear()
            {
                DraggedRect = RectangleF.Empty;
                Row = int.MinValue;
            }

            private PointF _mToolTipMouse = PointF.Empty;
            private string _mToolTipText = string.Empty;
            public RectangleF DraggedRect = RectangleF.Empty;
            public int Row = int.MinValue;
        }
        #endregion

        #region Private Helper Methods

        private Task _GetTaskUnderMouse(Point mouse)
        {
            var chartcoord = _mViewport.DeviceToWorldCoord(mouse);

            if (!_mHeaderInfo.H1Rect.Contains(chartcoord)
                && !_mHeaderInfo.H2Rect.Contains(chartcoord))
            {
                foreach (var task in _mChartTaskHitRects.Keys)
                {
                    if (_mChartTaskHitRects[task].Contains(chartcoord))
                        return task;
                }
            }

            return null;
        }

        private int _GetDeviceColumnUnderMouse(Point mouse)
        {
            var worldcoord = _mViewport.DeviceToWorldCoord(mouse);

            return _mHeaderInfo.Columns.Select((x, i) => new { x, i }).FirstOrDefault(x => x.x.Contains(worldcoord)).i;
        }

        /// <summary>
        /// Convert view Y coordinate to zero based row number
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private int _DeviceCoordToChartRow(float y)
        {
            y = _mViewport.DeviceToWorldCoord(new PointF(0, y)).Y;
            var row = (int)((y - this.BarSpacing - this.HeaderOneHeight) / this.BarSpacing);
            return row < 0 ? 0 : row;
        }

        /// <summary>
        /// Convert world Y coordinate to zero-based row number
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private int _ChartCoordToChartRow(float y)
        {
            var row = (int)((y - this.HeaderTwoHeight - this.HeaderOneHeight) / this.BarSpacing);
            return row < 0 ? 0 : row;
        }

        /// <summary>
        /// Convert zero based row number to client Y coordinates
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private float _ChartRowToChartCoord(int row)
        {
            return row * this.BarSpacing + this.HeaderTwoHeight + this.HeaderOneHeight;
        }

        /// <summary>
        /// Draw the Chart using the specified graphics. Only object within the clipRect are drawn, the rest are culled away.
        /// </summary>
        private void _Draw(Graphics graphics, Rectangle clipRect)
        {
            graphics.Clear(Color.White);

            int row = 0;
            if (_mProject != null)
            {
                // generate rectangles
                _GenerateModels();
                _GenerateHeaders();

                // set model view matrix
                graphics.Transform = _mViewport.Projection;

                // draw columns in the background
                _DrawColumns(graphics);

                // draw predecessor arrows
                if (this.ShowRelations) this._DrawPredecessorLines(graphics);

                // draw bar charts
                row = this._DrawTasks(graphics, clipRect);

                // draw the header
                _DrawHeader(graphics, clipRect);

                // Paint overlays
                ChartPaintEventArgs paintargs = new ChartPaintEventArgs(graphics, clipRect, this);
                OnPaintOverlay(paintargs);
                _mOverlay.Paint(paintargs);
            }
            else
            {
                // nothing to draw
            }

            // flush
            graphics.Flush();
        }

        /// <summary>
        /// Generate the task models and resize the world accordingly
        /// </summary>
        private void _GenerateModels()
        {
            // Clear Models
            _mChartTaskRects.Clear();
            _mChartTaskHitRects.Clear();
            _mChartSlackRects.Clear();
            _mChartTaskPartRects.Clear();

            var pHeight = this.Parent == null ? this.Height : this.Parent.Height;
            var pWidth = this.Parent == null ? this.Width : this.Parent.Width;

            // loop over the tasks and pick up items
            var end = TimeSpan.MinValue;
            int row = 0;
            foreach (var task in _mProject.Tasks)
            {
                if (!_mProject.GroupsOf(task).Any(x => x.IsCollapsed))
                {
                    int y_coord = row * this.BarSpacing + this.HeaderTwoHeight + this.HeaderOneHeight + (this.BarSpacing - this.BarHeight) / 2;
                    RectangleF taskRect;

                    // Compute task rectangle
                    taskRect = new RectangleF(GetSpan(task.Start), y_coord, GetSpan(task.Duration), this.BarHeight);
                    _mChartTaskRects.Add(task, taskRect); // also add groups and split tasks (not just task parts)

                    if (!_mProject.IsSplit(task))
                    {
                        // Add normal Task Rectangles to hitRect collection for hit testing
                        _mChartTaskHitRects.Add(task, taskRect);
                    }
                    else // Compute task part rectangles if task is a split task
                    {
                        var parts = new List<KeyValuePair<Task, RectangleF>>();
                        _mChartTaskPartRects.Add(task, parts);
                        foreach (var part in _mProject.PartsOf(task))
                        {
                            taskRect = new RectangleF(GetSpan(part.Start), y_coord, GetSpan(part.Duration), this.BarHeight);
                            parts.Add(new KeyValuePair<Task, RectangleF>(part, taskRect));

                            // Parts are mouse enabled, add to hitRect collection
                            _mChartTaskHitRects.Add(part, taskRect);
                        }
                    }

                    // Compute Slack Rectangles
                    if (this.ShowSlack)
                    {
                        var slackRect = new RectangleF(GetSpan(task.End), y_coord, GetSpan(task.Slack), this.BarHeight);
                        _mChartSlackRects.Add(task, slackRect);
                    }

                    // Find maximum end time
                    if (task.End > end) end = task.End;

                    row++;
                }
            }
            row += 5;
            _mViewport.WorldHeight = Math.Max(pHeight, row * this.BarSpacing + this.BarHeight);
            _mViewport.WorldWidth = Math.Max(pWidth, GetSpan(end) + 200);
        }

        /// <summary>
        /// Generate Header rectangles and dates
        /// </summary>
        private void _GenerateHeaders()
        {
            // only generate the necessary headers by determining the current viewport location
            var h1Rect = new RectangleF(_mViewport.X, _mViewport.Y, _mViewport.Rectangle.Width, this.HeaderOneHeight);
            var h2Rect = new RectangleF(h1Rect.Left, h1Rect.Bottom, _mViewport.Rectangle.Width, this.HeaderTwoHeight);
            var labelRects = new List<RectangleF>();
            var columns = new List<RectangleF>();
            var datetimes = new List<DateTime>();

            // generate columns across the viewport area           
            var minorDate = __CalculateViewportStart(); // start date of chart
            var minorInterval = GetSpan(MinorWidth);
            // calculate coordinates of rectangles
            var labelRect_Y = _mViewport.Y + this.HeaderOneHeight;
            var labelRect_X = (int)(_mViewport.X / MinorWidth) * MinorWidth;
            var columns_Y = labelRect_Y + this.HeaderTwoHeight;

            // From second column onwards,
            // loop over the number of <TimeScaleDisplay> each with width of MajorWidth,
            // creating the Major and Minor header rects and generating respective date time information
            while (labelRect_X < _mViewport.Rectangle.Right) // keep creating H1 labels until we are out of the viewport
            {
                datetimes.Add(minorDate);
                labelRects.Add(new RectangleF(labelRect_X, labelRect_Y, MinorWidth, HeaderTwoHeight));
                columns.Add(new RectangleF(labelRect_X, columns_Y, MinorWidth, _mViewport.Rectangle.Height));
                minorDate += minorInterval;
                labelRect_X += MinorWidth;
            }

            _mHeaderInfo.H1Rect = h1Rect;
            _mHeaderInfo.H2Rect = h2Rect;
            _mHeaderInfo.LabelRects = labelRects;
            _mHeaderInfo.Columns = columns;
            _mHeaderInfo.DateTimes = datetimes;
        }

        /// <summary>
        /// Calculate the date in the first visible column in the viewport
        /// </summary>
        /// <returns></returns>
        private DateTime __CalculateViewportStart()
        {
            float vpTime = (int)(_mViewport.X / this.MinorWidth);
            if (this.TimeResolution == GanttChart.TimeResolution.Week)
            {
                return _mProject.Start.AddDays(vpTime * 7);
            }
            else if (this.TimeResolution == GanttChart.TimeResolution.Day)
            {
                return _mProject.Start.AddDays(vpTime);
            }
            else if (this.TimeResolution == GanttChart.TimeResolution.Hour)
            {
                return _mProject.Start.AddHours(vpTime);
            }
            else if (this.TimeResolution == TimeResolution.Minute)
            {
                return _mProject.Start.AddMinutes(vpTime);
            }

            throw new NotImplementedException("Unable to determine TimeResolution.");
        }

        private void _DrawColumns(Graphics graphics)
        {
            // draw column lines
            graphics.DrawRectangles(this.HeaderFormat.Border, _mHeaderInfo.Columns.ToArray());

            // fill weekend columns
            for (int i = 0; i < _mHeaderInfo.DateTimes.Count; i++)
            {
                var date = _mHeaderInfo.DateTimes[i];
                // highlight weekends for day time scale
                if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                {
                    var pattern = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Percent20, this.HeaderFormat.Border.Color, Color.Transparent);
                    graphics.FillRectangle(pattern, _mHeaderInfo.Columns[i]);
                }
            }
        }

        private void _DrawHeader(Graphics graphics, Rectangle clipRect)
        {
            var info = _mHeaderInfo;
            var viewRect = _mViewport.Rectangle;

            // Draw header backgrounds
            var e = new HeaderPaintEventArgs(graphics, clipRect, this, this.Font, this.HeaderFormat);
            OnPaintHeader(e);
            var gradient = new System.Drawing.Drawing2D.LinearGradientBrush(info.H1Rect, e.Format.GradientLight, e.Format.GradientDark, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            graphics.FillRectangles(gradient, new RectangleF[] { info.H1Rect, info.H2Rect });
            graphics.DrawRectangles(e.Format.Border, new RectangleF[] { info.H1Rect, info.H2Rect });

            // Draw the header scales
            __DrawScale(graphics, clipRect, e.Font, e.Format, info.LabelRects, info.DateTimes);

            // draw "Now" line
            float xf = GetSpan(_mProject.Now);
            var pen = new Pen(e.Format.Border.Color) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
            graphics.DrawLine(pen, new PointF(xf, _mViewport.Y), new PointF(xf, _mViewport.Rectangle.Bottom));
        }

        private void __DrawScale(Graphics graphics, Rectangle clipRect, Font font, HeaderFormat headerformat, List<RectangleF> labelRects, List<DateTime> dates)
        {
            TimelinePaintEventArgs e = null;
            DateTime datetime = dates[0]; // these initialisation values matter
            DateTime datetimeprev = dates[0]; // these initialisation values matter
            for (int i = 0; i < labelRects.Count; i++)
            {
                // Give user a chance to format the tickmark that is to be drawn
                // https://blog.nicholasrogoff.com/2012/05/05/c-datetime-tostring-formats-quick-reference/
                datetime = dates[i];
                ___GetLabelFormat(datetime, datetimeprev, out LabelFormat minor, out LabelFormat major);
                e = new TimelinePaintEventArgs(graphics, clipRect, this, datetime, datetimeprev, minor, major);
                OnPaintTimeline(e);

                // Draw the label if not already handled by the user
                if (!e.Handled)
                {
                    if (!string.IsNullOrEmpty(minor.Text))
                    {
                        // Draw minor label
                        var textbox = graphics.TextBoxAlign(minor.Text, minor.TextAlign, minor.Font, labelRects[i], minor.Margin);
                        graphics.DrawString(minor.Text, minor.Font, minor.Color, textbox);
                    }

                    if (!string.IsNullOrEmpty(major.Text))
                    {
                        // Draw major label
                        var majorLabelRect = new RectangleF(labelRects[i].X, _mViewport.Y, this.MajorWidth, this.HeaderOneHeight);
                        var textbox = graphics.TextBoxAlign(major.Text, major.TextAlign, major.Font, majorLabelRect, major.Margin);
                        graphics.DrawString(major.Text, major.Font, major.Color, textbox);
                        __DrawMarker(graphics, labelRects[i].X + MinorWidth / 2f, _mViewport.Y + HeaderOneHeight - 2f);
                    }
                }

                // set prev datetime
                datetimeprev = datetime;
            }
        }

        private void ___GetLabelFormat(DateTime datetime, DateTime datetimeprev, out LabelFormat minor, out LabelFormat major)
        {
            minor = new LabelFormat() { Text = string.Empty, Font = this.Font, Color = HeaderFormat.Color, Margin = 3, TextAlign = ChartTextAlign.MiddleCenter };
            major = new LabelFormat() { Text = string.Empty, Font = this.Font, Color = HeaderFormat.Color, Margin = 3, TextAlign = ChartTextAlign.MiddleLeft };

            System.Globalization.GregorianCalendar calendar = new System.Globalization.GregorianCalendar();
            switch (TimeResolution)
            {
                case TimeResolution.Week:
                    minor.Text = calendar.GetWeekOfYear(datetime, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday).ToString();
                    if (datetime.Month != datetimeprev.Month) major.Text = datetime.ToString("MMMM");
                    break;
                case TimeResolution.Hour:
                    minor.Text = datetime.Hour.ToString();
                    if (datetime.Day != datetimeprev.Day) major.Text = datetime.ToString("dd MMMM yyyy");
                    break;
                default: // case TimeResolution.Day: -- to implement other TimeResolutions, add to this function or listen to the the PaintTimeline event
                    minor.Text = ShortDays[datetime.DayOfWeek]; // datetime.ToString("dddd").Substring(0, 1).ToUpper();
                    if (datetime.DayOfWeek == DayOfWeek.Sunday) major.Text = datetime.ToString("dd MMMM yyyy");
                    break;
            }
        }

        private void __DrawMarker(Graphics graphics, float offsetX, float offsetY)
        {
            var marker = _Marker.Select(p => new PointF(p.X + offsetX, p.Y + offsetY)).ToArray();
            graphics.FillPolygon(Brushes.LightGoldenrodYellow, marker);
            graphics.DrawPolygon(new Pen(SystemColors.ButtonShadow), marker);
        }
        private static GraphicsPath RoundedRect(RectangleF bounds, float radius)
        {
            float diameter = radius * 2;
            SizeF size = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private int _DrawTasks(Graphics graphics, Rectangle clipRect)
        {
            var viewRect = _mViewport.Rectangle;
            int row = 0;
            var crit_task_set = new HashSet<Task>(_mProject.CriticalPaths.SelectMany(x => x));
            var pen = new Pen(Color.Gray);
            float labelMargin = this.MinorWidth / 2.0f + 3.0f;
            pen.DashStyle = DashStyle.Dot;
            TaskPaintEventArgs e;
            foreach (var task in _mChartTaskRects.Keys)
            {
                // Get the taskrect
                var taskrect = _mChartTaskRects[task];

                // Only begin drawing when the taskrect is to the left of the clipRect's right edge
                if (taskrect.Left <= viewRect.Right)
                {
                    // Crtical Path
                    bool critical = crit_task_set.Contains(task);
                    if (critical) e = new TaskPaintEventArgs(graphics, clipRect, this, task, row, critical, this.Font, this.CriticalTaskFormat);
                    else e = new TaskPaintEventArgs(graphics, clipRect, this, task, row, critical, this.Font, this.TaskFormat);
                    PaintTask?.Invoke(this, e);

                    if (viewRect.IntersectsWith(taskrect))
                    {
                        if (_mProject.IsSplit(task))
                        {
                            __DrawTaskParts(graphics, e, task, pen);
                        }
                        else
                        {
                            __DrawRegularTaskAndGroup(graphics, e, task, taskrect);
                        }
                    }

                    // write text
                    if (this.ShowTaskLabels && task.Name != string.Empty)
                    {
                        var name = task.Name;
                        var txtrect = graphics.TextBoxAlign(name, ChartTextAlign.MiddleLeft, e.Font, taskrect, labelMargin);
                        txtrect.Offset(taskrect.Width, 0);
                        if (viewRect.IntersectsWith(txtrect))
                        {
                            graphics.DrawString(name, e.Font, e.Format.Color, txtrect);
                        }
                    }

                    // draw slack
                    if (this.ShowSlack && task.Complete < 1.0f)
                    {
                        var slackrect = _mChartSlackRects[task];
                        if (viewRect.IntersectsWith(slackrect))
                            graphics.FillRectangle(e.Format.SlackFill, slackrect);
                    }
                }

                row++;
            }

            return row;
        }

        /// <summary>
        /// Only draw lines for all Precedents which are visible on the chart
        /// TODO: draw lines for all collapsed Groups which has precendents
        /// TODO: draw lines for all collapsed Groups which has dependants
        /// </summary>
        /// <param name="graphics"></param>
        private void _DrawPredecessorLines(Graphics graphics)
        {
            var viewRect = _mViewport.Rectangle;
            RectangleF clipRectF = new RectangleF(viewRect.X, viewRect.Y, viewRect.Width, viewRect.Height);
            foreach (var precedent in _mProject.Precedents)
            {
                foreach (var dependant in _mProject.DirectDependantsOf(precedent))
                {
                    var pvisible = _mChartTaskRects.ContainsKey(precedent);
                    var dvisible = _mChartTaskRects.ContainsKey(dependant);
                    RectangleF prect, drect;
                    PointF p1, p2, p3, p4, p5, p6;
                    var arrowSize = 6f;
                    var minLineLength = 10f;
                    var maxLineLength = 10f;
                    // case where both precedent and dependant are visible, just connect line between them
                    if (!pvisible && !dvisible)
                    {
                        continue; //next dependant please!
                    }
                    else
                    {
                        prect = _mChartTaskRects[precedent];
                        drect = _mChartTaskRects[dependant];

                        // plot and draw lines
                        p1 = new PointF(prect.Right, prect.Top + prect.Height / 2.0f);
                        p2 = new PointF(p1.X + Math.Min(Math.Max(((drect.Right - prect.Right) / 2.0f), minLineLength), maxLineLength), p1.Y);
                        p3 = new PointF(p2.X, prect.Bottom + (drect.Top - prect.Bottom) / 2.0f);
                        p4 = new PointF(drect.Left - (p2.X - p1.X) - (arrowSize * 1.5f), p3.Y);
                        p5 = new PointF(p4.X, drect.Bottom - (drect.Height / 2.0f));
                        p6 = new PointF(drect.Left, p5.Y);
                    }

                    // prepare and draw the lines
                    var size = new SizeF(Math.Abs(p3.X - p1.X), Math.Abs(p3.Y - p1.Y));
                    var linerect = p1.Y < p3.Y ? new RectangleF(p1, size) : new RectangleF(new PointF(p1.X, p1.Y - size.Height), size);
                    if (clipRectF.IntersectsWith(linerect))
                    {
                        Pen linePen = new Pen(Color.Orange, 2.0f);
                        graphics.DrawLines(linePen, new PointF[] { p1, p2, p3, p4, p5, p6 });
                        // draw arrowhead
                        var p7 = new PointF(p6.X - arrowSize, p6.Y - arrowSize);
                        var p8 = new PointF(p6.X - arrowSize, p6.Y + arrowSize);
                        graphics.FillPolygon(Brushes.Orange, new PointF[] { p6, p7, p8 });
                    }

                }
            }
        }

        private void __DrawRegularTaskAndGroup(Graphics graphics, TaskPaintEventArgs e, Task task, RectangleF taskRect)
        {
            var fill = taskRect;
            fill.Width = (int)(fill.Width * task.Complete);
            //graphics.FillRectangle(e.Format.BackFill, taskRect);
            //graphics.FillRectangle(e.Format.ForeFill, fill);
            using (GraphicsPath path = RoundedRect(taskRect, 4))
            {
                graphics.FillPath(e.Format.BackFill, path);
                //graphics.FillPath(e.Format.ForeFill, fill);
                graphics.DrawPolygon(e.Format.Border, path.PathPoints);
                // check if this is a parent task / group task, then draw the bracket
                if (_mProject.IsGroup(task))
                {
                    //var rod = new RectangleF(taskRect.Left, taskRect.Top, taskRect.Width, taskRect.Height);
                    //graphics.FillRectangle(Brushes.Red, rod);
                    graphics.FillPath(e.Format.BackFill, path);
                    graphics.DrawPolygon(e.Format.Border, path.PathPoints);
                    /*
                    if (!task.IsCollapsed)
                    {
                        // left bracket
                        graphics.FillPolygon(Brushes.Black, new PointF[] {
                                    new PointF() { X = taskRect.Left, Y = taskRect.Top },
                                    new PointF() { X = taskRect.Left, Y = taskRect.Top + BarHeight },
                                    new PointF() { X = taskRect.Left + MinorWidth / 2f, Y = taskRect.Top } });
                        // right bracket
                        graphics.FillPolygon(Brushes.Black, new PointF[] {
                                    new PointF() { X = taskRect.Right, Y = taskRect.Top },
                                    new PointF() { X = taskRect.Right, Y = taskRect.Top + BarHeight },
                                    new PointF() { X = taskRect.Right - MinorWidth / 2f, Y = taskRect.Top } });
                    } */
                }
            }
        }

        private void __DrawTaskParts(Graphics graphics, TaskPaintEventArgs e, Task task, Pen pen)
        {
            var parts = _mChartTaskPartRects[task];

            // Draw line indicator
            var firstRect = parts[0].Value;
            var lastRect = parts[parts.Count - 1].Value;
            var y_coord = (firstRect.Top + firstRect.Bottom) / 2.0f;
            var point1 = new PointF(firstRect.Right, y_coord);
            var point2 = new PointF(lastRect.Left, y_coord);
            graphics.DrawLine(pen, point1, point2);

            // Draw Part Rectangles
            var taskRects = parts.Select(x => x.Value).ToArray();
            graphics.FillRectangles(e.Format.BackFill, taskRects);

            // Draw % complete indicators
            graphics.FillRectangles(e.Format.ForeFill, parts.Select(x => new RectangleF(x.Value.X, x.Value.Y, x.Value.Width * x.Key.Complete, x.Value.Height)).ToArray());

            // Draw border
            graphics.DrawRectangles(e.Format.Border, taskRects);
        }

        #endregion Private Helper Methods

        #region Private Helper Variables'
        /// <summary>
        /// Printing labels for header
        /// </summary>
        private static readonly SortedDictionary<DayOfWeek, string> ShortDays = new SortedDictionary<DayOfWeek, string>
        {
            {DayOfWeek.Sunday, "S"},
            {DayOfWeek.Monday, "M"},
            {DayOfWeek.Tuesday, "T"},
            {DayOfWeek.Wednesday, "W"},
            {DayOfWeek.Thursday, "T"},
            {DayOfWeek.Friday, "F"},
            {DayOfWeek.Saturday, "S"}
        };

        /// <summary>
        /// Polygon points for Header markers
        /// </summary>
        private static readonly PointF[] _Marker = new PointF[] {
            new PointF(-4, 0),
            new PointF(4, 0),
            new PointF(4, 4),
            new PointF(0, 8),
            new PointF(-4f, 4)
        };

        class HeaderInfo
        {
            public RectangleF H1Rect;
            public RectangleF H2Rect;
            public List<RectangleF> LabelRects;
            public List<RectangleF> Columns;
            public List<DateTime> DateTimes;
        }

        ProjectManager<Task, object> _mProject = null; // The project to be visualised / rendered as a Gantt Chart
        IViewport _mViewport = null;
        Task _mDraggedTask = null; // The dragged source Task
        Point _mDragTaskLastLocation = Point.Empty; // Record the task dragging mouse offset
        Point _mDragTaskStartLocation = Point.Empty;
        Point _mPanViewLastLocation = Point.Empty;
        List<Task> _mSelectedTasks = new List<Task>(); // List of selected tasks
        Dictionary<Task, RectangleF> _mChartTaskHitRects = new Dictionary<Task, RectangleF>(); // list of hitareas for Task Rectangles
        Dictionary<Task, RectangleF> _mChartTaskRects = new Dictionary<Task, RectangleF>();
        Dictionary<Task, List<KeyValuePair<Task, RectangleF>>> _mChartTaskPartRects = new Dictionary<Task, List<KeyValuePair<Task, RectangleF>>>();
        Dictionary<Task, RectangleF> _mChartSlackRects = new Dictionary<Task, RectangleF>();
        HeaderInfo _mHeaderInfo = new HeaderInfo();
        Task _mMouseEntered = null; // flag whether the mouse has entered a Task rectangle or not
        Dictionary<Task, string> _mTaskToolTip = new Dictionary<Task, string>();
        #endregion Private Helper Variables
    }

    #region Chart Formatting

    /// <summary>
    /// Time resolution for the minor tick marks which are spaced Chart.TimeWidth apart
    /// </summary>
    public enum TimeResolution
    {
        Week,
        Day,
        Hour,
        Minute,
        Second,
    }

    /// <summary>
    /// Format for painting tasks
    /// </summary>
    public struct TaskFormat
    {
        /// <summary>
        /// Get or set Task outline color
        /// </summary>
        public Pen Border { get; set; }

        /// <summary>
        /// Get or set Task background color
        /// </summary>
        public Brush BackFill { get; set; }

        /// <summary>
        /// Get or set Task foreground color
        /// </summary>
        public Brush ForeFill { get; set; }

        /// <summary>
        /// Get or set Task font color
        /// </summary>
        public Brush Color { get; set; }

        /// <summary>
        /// Get or set the brush for slack bars
        /// </summary>
        public Brush SlackFill { get; set; }
    }

    /// <summary>
    /// Format for painting relations
    /// </summary>
    public struct RelationFormat
    {
        /// <summary>
        /// Get or set the line pen
        /// </summary>
        public Pen Line { get; set; }
    }

    /// <summary>
    /// Format for painting chart header
    /// </summary>
    public struct HeaderFormat
    {
        /// <summary>
        /// Font color
        /// </summary>
        public Brush Color { get; set; }
        /// <summary>
        /// Border and line colors
        /// </summary>
        public Pen Border { get; set; }
        /// <summary>
        /// Get or set the lighter color in the gradient
        /// </summary>
        public Color GradientLight { get; set; }
        /// <summary>
        /// Get or set the darker color in the gradient
        /// </summary>
        public Color GradientDark { get; set; }
    }

    public struct LabelFormat
    {
        public string Text;
        public Font Font;
        public Brush Color;
        public ChartTextAlign TextAlign;
        public float Margin;
    }
    #endregion Chart Formatting

    #region EventAgrs
    /// <summary>
    /// Provides data for TaskMouseEvent
    /// </summary>
    public class TaskMouseEventArgs : MouseEventArgs
    {
        /// <summary>
        /// Subject Task of the event
        /// </summary>
        public Task Task { get; private set; }
        /// <summary>
        /// Rectangle bounds of the Task
        /// </summary>
        public RectangleF Rectangle { get; private set; }
        /// <summary>
        /// Initialize a new instance of TaskMouseEventArgs with the MouseEventArgs parameters and the Task involved.
        /// </summary>
        public TaskMouseEventArgs(Task task, RectangleF rectangle, MouseButtons buttons, int clicks, int x, int y, int delta)
            : base(buttons, clicks, x, y, delta)
        {
            this.Task = task;
            this.Rectangle = rectangle;
        }
    }
    /// <summary>
    /// Provides data for TaskDragDropEvent
    /// </summary>
    public class TaskDragDropEventArgs : MouseEventArgs
    {
        /// <summary>
        /// Get the previous mouse location
        /// </summary>
        public Point PreviousLocation { get; private set; }
        /// <summary>
        /// Get the starting mouse location of this drag drop event
        /// </summary>
        public Point StartLocation { get; private set; }
        /// <summary>
        /// Get the source task that is being dragged
        /// </summary>
        public Task Source { get; private set; }
        /// <summary>
        /// Get the target task that is being dropped on
        /// </summary>
        public Task Target { get; private set; }
        /// <summary>
        /// Get the rectangle bounds of the source task in chart coordinates
        /// </summary>
        public RectangleF SourceRect { get; private set; }
        /// <summary>
        /// Get the rectangle bounds of the target task in chart coordinates
        /// </summary>
        public RectangleF TargetRect { get; private set; }
        /// <summary>
        /// Get the chart row number that the mouse is current at.
        /// </summary>
        public int Row { get; private set; }
        /// <summary>
        /// Initialize a new instance of TaskDragDropEventArgs with the MouseEventArgs parameters and the Task involved and the previous mouse location.
        /// </summary>
        public TaskDragDropEventArgs(Point startLocation, Point prevLocation, Task source, RectangleF sourceRect, Task target, RectangleF targetRect, int row, MouseButtons buttons, int clicks, int x, int y, int delta)
            : base(buttons, clicks, x, y, delta)
        {
            this.Source = source;
            this.SourceRect = sourceRect;
            this.Target = target;
            this.TargetRect = targetRect;
            this.PreviousLocation = prevLocation;
            this.StartLocation = startLocation;
            this.Row = row;
        }
    }

    /// <summary>
    /// Provides data for ChartPaintEvent
    /// </summary>
    public class ChartPaintEventArgs : PaintEventArgs
    {
        /// <summary>
        /// Get the chart that for this event
        /// </summary>
        public Chart Chart { get; private set; }

        /// <summary>
        /// Initialize a new instance of ChartPaintEventArgs with the PaintEventArgs graphics and clip rectangle, and the chart itself.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipRect"></param>
        /// <param name="chart"></param>
        public ChartPaintEventArgs(Graphics graphics, Rectangle clipRect, Chart chart)
            : base(graphics, clipRect)
        {
            this.Chart = chart;
        }
    }

    /// <summary>
    /// Provides data for ChartPaintEvent
    /// </summary>
    public class HeaderPaintEventArgs : ChartPaintEventArgs
    {
        /// <summary>
        /// Get or set the font to use for drawing the text on the header
        /// </summary>
        public Font Font { get; set; }
        /// <summary>
        /// Get or set the header formatting
        /// </summary>
        public HeaderFormat Format { get; set; }

        /// <summary>
        /// Initialize a new instance of HeaderPaintEventArgs with the editable default font and header format
        /// </summary>
        public HeaderPaintEventArgs(Graphics graphics, Rectangle clipRect, Chart chart, Font font, HeaderFormat format)
            : base(graphics, clipRect, chart)
        {
            this.Font = font;
            this.Format = format;
        }
    }

    /// <summary>
    /// Provides data for TaskPaintEvent
    /// </summary>
    public class TaskPaintEventArgs : ChartPaintEventArgs
    {
        /// <summary>
        /// Get the task to be painted
        /// </summary>
        public Task Task { get; private set; }
        /// <summary>
        /// Get the rectangle bounds of the task
        /// </summary>
        public RectangleF Rectangle
        {
            get
            {
                return new RectangleF(this.Chart.GetSpan(this.Task.Start), this.Row * this.Chart.BarSpacing + this.Chart.BarSpacing + this.Chart.HeaderOneHeight, this.Chart.GetSpan(this.Task.Duration), this.Chart.BarHeight);
            }
        }
        /// <summary>
        /// Get the row number of the task
        /// </summary>
        public int Row { get; private set; }
        /// <summary>
        /// Get or set the font to be used to draw the task label
        /// </summary>
        public Font Font { get; set; }
        /// <summary>
        /// Get or set the formatting of the task
        /// </summary>
        public TaskFormat Format { get; set; }
        /// <summary>
        /// Get whether the task is in a critical path
        /// </summary>
        public bool IsCritical { get; private set; }
        /// <summary>
        /// Initialize a new instance of TaskPaintEventArgs with the editable default font and task paint format
        /// </summary>
        public TaskPaintEventArgs(Graphics graphics, Rectangle clipRect, Chart chart, Task task, int row, bool critical, Font font, TaskFormat format) // need to create a paint event for each task for custom painting
            : base(graphics, clipRect, chart)
        {
            this.Task = task;
            this.Row = row;
            this.Font = font;
            this.Format = format;
            this.IsCritical = critical;
        }
    }

    /// <summary>
    /// Provides data for RelationPaintEvent
    /// </summary>
    public class RelationPaintEventArgs : ChartPaintEventArgs
    {
        /// <summary>
        /// Get the precedent task in the relation
        /// </summary>
        public Task Precedent { get; private set; }

        /// <summary>
        /// Get the dependant task in the relation
        /// </summary>
        public Task Dependant { get; private set; }

        /// <summary>
        /// Get or set the formatting to use for drawing the relation
        /// </summary>
        public RelationFormat Format { get; set; }

        /// <summary>
        /// Initialize a new instance of RelationPaintEventArgs with the editable default font and relation paint format
        /// </summary>
        public RelationPaintEventArgs(Graphics graphics, Rectangle clipRect, Chart chart, Task before, Task after, RelationFormat format)
            : base(graphics, clipRect, chart)
        {
            this.Precedent = before;
            this.Dependant = after;
            this.Format = format;
        }
    }

    /// <summary>
    /// Provides data for ScalePaintEvent
    /// </summary>
    public class TimelinePaintEventArgs : ChartPaintEventArgs
    {
        /// <summary>
        /// Get the datetime value of the tick mark
        /// </summary>
        public DateTime DateTime { get; private set; }
        /// <summary>
        /// Get the dateimte value of the preview mark
        /// </summary>
        public DateTime DateTimePrev { get; private set; }
        /// <summary>
        /// Get or set whether painting of the tick mark has already been handled. If it is already handled, Chart will not paint the tick mark.
        /// </summary>
        public bool Handled { get; private set; }
        /// <summary>
        /// Get or set the label for the minor scale
        /// </summary>
        LabelFormat Minor { get; set; }
        /// <summary>
        /// Get or set the label for the major scale
        /// </summary>
        LabelFormat Major { get; set; }

        public TimelinePaintEventArgs(Graphics graphics, Rectangle clipRect, Chart chart, DateTime datetime, DateTime datetimeprev, LabelFormat minor, LabelFormat major)
            : base(graphics, clipRect, chart)
        {
            Handled = false;
            DateTime = datetime;
            DateTimePrev = datetimeprev;
            Minor = minor;
            Major = major;
        }
    }

    #endregion EventArgs

    /// <summary>
    /// Provides information about the chart at a specific row and date/time.
    /// </summary>
    public struct ChartInfo
    {
        /// <summary>
        /// Get or set the chart row number
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// Get or set the chart date/time
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Get or set the task
        /// </summary>
        public Task Task { get; set; }
        /// <summary>
        /// Construct a passive data structure to hold chart information
        /// </summary>
        /// <param name="row"></param>
        /// <param name="dateTime"></param>
        /// <param name="task"></param>
        public ChartInfo(int row, DateTime dateTime, Task task)
            : this()
        {
            Row = row;
            DateTime = dateTime;
            Task = task;
        }
    }

    public class Row
    {
        public int Index { get; set; }
        public float Height { get; set; }
    }

    public class Column
    {
        public int Index { get; set; }
        public DateTime DateTime { get; set; }
    }

    public enum ChartTextAlign
    {
        TopLeft, TopCenter, TopRight,
        MiddleLeft, MiddleCenter, MiddleRight,
        BottomLeft, BottomCenter, BottomRight
    }
}
