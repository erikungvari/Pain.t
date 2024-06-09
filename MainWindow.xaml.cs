using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimplePaintApp
{
    public partial class MainWindow : Window
    {
        private enum Tool
        {
            Brush,
            Eraser,
            Line,
            Rectangle,
            Ellipse
        }

        private Tool currentTool = Tool.Brush;
        private bool isDrawing;
        private Point startPoint;
        private Brush selectedColor = Brushes.Black;
        private double brushSize = 2;
        private Shape currentShape;

        public MainWindow()
        {
            InitializeComponent();
            HighlightSelectedTool(BrushButton);
            HighlightSelectedColor(BlackColorButton);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(paintCanvas);

            if (e.ButtonState == MouseButtonState.Pressed && IsWithinCanvas(position))
            {
                isDrawing = true;
                startPoint = position;

                switch (currentTool)
                {
                    case Tool.Brush:
                    case Tool.Eraser:
                        currentShape = new Polyline
                        {
                            Stroke = currentTool == Tool.Eraser ? Brushes.White : selectedColor,
                            StrokeThickness = brushSize
                        };
                        ((Polyline)currentShape).Points.Add(startPoint);
                        break;
                    case Tool.Line:
                        currentShape = new Line
                        {
                            Stroke = selectedColor,
                            StrokeThickness = brushSize,
                            X1 = startPoint.X,
                            Y1 = startPoint.Y
                        };
                        break;
                    case Tool.Rectangle:
                        currentShape = new Rectangle
                        {
                            Stroke = selectedColor,
                            StrokeThickness = brushSize
                        };
                        Canvas.SetLeft(currentShape, startPoint.X);
                        Canvas.SetTop(currentShape, startPoint.Y);
                        break;
                    case Tool.Ellipse:
                        currentShape = new Ellipse
                        {
                            Stroke = selectedColor,
                            StrokeThickness = brushSize
                        };
                        Canvas.SetLeft(currentShape, startPoint.X);
                        Canvas.SetTop(currentShape, startPoint.Y);
                        break;
                }

                paintCanvas.Children.Add(currentShape);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && currentShape != null)
            {
                Point currentPoint = e.GetPosition(paintCanvas);

                if (!IsWithinCanvas(currentPoint))
                {
                    return;
                }

                switch (currentTool)
                {
                    case Tool.Brush:
                    case Tool.Eraser:
                        ((Polyline)currentShape).Points.Add(currentPoint);
                        break;
                    case Tool.Line:
                        ((Line)currentShape).X2 = currentPoint.X;
                        ((Line)currentShape).Y2 = currentPoint.Y;
                        break;
                    case Tool.Rectangle:
                        double rectWidth = Math.Abs(currentPoint.X - startPoint.X);
                        double rectHeight = Math.Abs(currentPoint.Y - startPoint.Y);
                        ((Rectangle)currentShape).Width = rectWidth;
                        ((Rectangle)currentShape).Height = rectHeight;
                        Canvas.SetLeft(currentShape, Math.Min(startPoint.X, currentPoint.X));
                        Canvas.SetTop(currentShape, Math.Min(startPoint.Y, currentPoint.Y));
                        break;
                    case Tool.Ellipse:
                        double ellipseWidth = Math.Abs(currentPoint.X - startPoint.X);
                        double ellipseHeight = Math.Abs(currentPoint.Y - startPoint.Y);
                        ((Ellipse)currentShape).Width = ellipseWidth;
                        ((Ellipse)currentShape).Height = ellipseHeight;
                        Canvas.SetLeft(currentShape, Math.Min(startPoint.X, currentPoint.X));
                        Canvas.SetTop(currentShape, Math.Min(startPoint.Y, currentPoint.Y));
                        break;
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                currentShape = null;
            }
        }

        private bool IsWithinCanvas(Point position)
        {
            return position.X >= 0 && position.X <= paintCanvas.ActualWidth
                   && position.Y >= brushSize/2 && position.Y <= paintCanvas.ActualHeight;
        }

        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            paintCanvas.Children.Clear();
        }

        private void SelectColor_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string color)
            {
                selectedColor = (Brush)new BrushConverter().ConvertFromString(color);
                HighlightSelectedColor(button);
            }
        }

        private void BrushSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            brushSize = e.NewValue;
        }

        private void HighlightSelectedTool(Button selectedButton)
        {
            ClearButton.Style = (Style)FindResource("ToolButtonStyle");
            BrushButton.Style = (Style)FindResource("ToolButtonStyle");
            EraserButton.Style = (Style)FindResource("ToolButtonStyle");
            LineButton.Style = (Style)FindResource("ToolButtonStyle");
            RectangleButton.Style = (Style)FindResource("ToolButtonStyle");
            EllipseButton.Style = (Style)FindResource("ToolButtonStyle");
            UploadButton.Style = (Style)FindResource("ToolButtonStyle");
            SaveButton.Style = (Style)FindResource("ToolButtonStyle");

            selectedButton.Style = (Style)FindResource("SelectedToolButtonStyle");
        }

        private void HighlightSelectedColor(Button selectedColorButton)
        {
            BlackColorButton.Style = (Style)FindResource("ColorButtonStyle");
            RedColorButton.Style = (Style)FindResource("ColorButtonStyle");
            GreenColorButton.Style = (Style)FindResource("ColorButtonStyle");
            BlueColorButton.Style = (Style)FindResource("ColorButtonStyle");
            YellowColorButton.Style = (Style)FindResource("ColorButtonStyle");
            OrangeColorButton.Style = (Style)FindResource("ColorButtonStyle");
            PurpleColorButton.Style = (Style)FindResource("ColorButtonStyle");

            selectedColorButton.Style = (Style)FindResource("SelectedColorButtonStyle");
        }

        private void SelectBrush_Click(object sender, RoutedEventArgs e)
        {
            currentTool = Tool.Brush;
            HighlightSelectedTool(BrushButton);
        }

        private void Eraser_Click(object sender, RoutedEventArgs e)
        {
            currentTool = Tool.Eraser;
            HighlightSelectedTool(EraserButton);
        }

        private void SelectLine_Click(object sender, RoutedEventArgs e)
        {
            currentTool = Tool.Line;
            HighlightSelectedTool(LineButton);
        }

        private void SelectRectangle_Click(object sender, RoutedEventArgs e)
        {
            currentTool = Tool.Rectangle;
            HighlightSelectedTool(RectangleButton);
        }

        private void SelectEllipse_Click(object sender, RoutedEventArgs e)
        {
            currentTool = Tool.Ellipse;
            HighlightSelectedTool(EllipseButton);
        }

        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                Image image = new Image
                {
                    Source = bitmap
                };

                paintCanvas.Children.Clear();
                paintCanvas.Children.Add(image);
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png|JPEG Image|*.jpg"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                    (int)paintCanvas.ActualWidth, (int)paintCanvas.ActualHeight,
                    96d, 96d, PixelFormats.Pbgra32);
                renderBitmap.Render(paintCanvas);

                BitmapEncoder encoder;
                if (saveFileDialog.FilterIndex == 1)
                {
                    encoder = new PngBitmapEncoder();
                }
                else
                {
                    encoder = new JpegBitmapEncoder();
                }

                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                using (FileStream file = File.Create(saveFileDialog.FileName))
                {
                    encoder.Save(file);
                }
            }
        }
    }
}
