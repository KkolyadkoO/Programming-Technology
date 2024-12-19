using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;

namespace Lab10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDragging = false;
        private bool isResizing = false;
        private bool isTopLeftResizing = false;
        private bool isBottomRightResizing = false;
        private bool isMoving = false;
        private Point lastMousePosition;
        private SolidColorBrush graphColor = Brushes.Black;
        private string graphText = "График";
        private FontWeight graphFontWeight = FontWeights.Normal;
        private FontStyle graphFontStyle = FontStyles.Normal;
        private double parameterA = 20;

        delegate void GraphParametersChanged();
        event GraphParametersChanged NotifyGraphParametersChanged;

        public MainWindow()
        {
            InitializeComponent();

            NotifyGraphParametersChanged += ChangeText;
            NotifyGraphParametersChanged += UpdateGraph;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)GraphContainer.ActualWidth,
                (int)GraphContainer.ActualHeight,
                96,
                96,
                PixelFormats.Pbgra32
            );

            renderBitmap.Render(GraphContainer);

            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    pngEncoder.Save(fileStream);
                }
            }
        }

        private void ChangeText()
        {
            graphText = GraphTextBox.Text;
            string selectedStyle = ((ComboBoxItem)FontStyleComboBox.SelectedItem).Content.ToString();
            graphFontWeight = selectedStyle.Contains("Жирный") ? FontWeights.Bold : FontWeights.Normal;
            graphFontStyle = selectedStyle.Contains("Курсив") ? FontStyles.Italic : FontStyles.Normal;
        }

        private void ChangeColorButton_Click(object sender, RoutedEventArgs e)
        {
            var colorDialog = new System.Windows.Forms.ColorDialog
            {
                Color = System.Drawing.Color.FromArgb(graphColor.Color.A, graphColor.Color.R, graphColor.Color.G, graphColor.Color.B)
            };

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                graphColor = new SolidColorBrush(
                    Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B)
                );

                NotifyGraphParametersChanged();
            }
        }

        private void LoadBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Выберите изображение",
                Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var imageBrush = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(openFileDialog.FileName))
                };
                GraphCanvas.Background = imageBrush;
            }
        }

        private void DrawGraphButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ParameterATextBox.Text, out double a))
            {
                parameterA = a;
                NotifyGraphParametersChanged();
            }
            else
            {
                MessageBox.Show("Введите корректное значение параметра a.");
            }
        }

        private void UpdateGraph()
        {
            GraphCanvas.Visibility = Visibility.Visible;
            GraphCanvas.Children.Clear();

            double xc = GraphCanvas.ActualWidth / 2;
            double yc = GraphCanvas.ActualHeight / 2;

            // Добавление текста на график
            TextBlock textBlock = new TextBlock
            {
                Text = graphText,
                Foreground = graphColor,
                FontWeight = graphFontWeight,
                FontStyle = graphFontStyle,
                FontSize = 24,
                Margin = new Thickness(xc , 0, 0, 0)
            };
            GraphCanvas.Children.Add(textBlock);


            Polyline polyline = new Polyline
            {
                Stroke = graphColor,
                StrokeThickness = 1
            };

            double scaleX = GraphCanvas.ActualWidth / 300;
            double scaleY = GraphCanvas.ActualHeight / 300;

            for (double t = 0; t <= 2 * Math.PI; t += 0.01)
            {
                double x = parameterA * (2 * Math.Cos(t) - Math.Cos(2 * t));
                double y = parameterA * (2 * Math.Sin(t) - Math.Sin(2 * t));

                polyline.Points.Add(new Point(xc + x * scaleX, yc - y * scaleY));
            }

            // Добавление управляющих элементов
            GraphCanvas.Children.Add(BottomRightResizer);
            GraphCanvas.Children.Add(TopLeftResizer);
            GraphCanvas.Children.Add(MoveHandle);
            GraphCanvas.Children.Add(polyline);

            PositionElements();

        }


        private void PositionElements()
        {
            Canvas.SetLeft(TopLeftResizer, 0);
            Canvas.SetTop(TopLeftResizer, 0);

            Canvas.SetLeft(BottomRightResizer, GraphCanvas.ActualWidth - 10);
            Canvas.SetTop(BottomRightResizer, GraphCanvas.ActualHeight - 10);

            Canvas.SetLeft(MoveHandle, GraphCanvas.ActualWidth / 2 - 7.5);
            Canvas.SetTop(MoveHandle, GraphCanvas.ActualHeight / 2 - 7.5);
        }

        private void GraphContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == TopLeftResizer || e.OriginalSource == BottomRightResizer)
            {
                isResizing = true;
            }
            else if (e.OriginalSource == MoveHandle)
            {
                isMoving = true;
            }
            lastMousePosition = e.GetPosition(GraphContainer);
            GraphContainer.CaptureMouse();
            e.Handled = true;
        }

        private void GraphContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isResizing && !isMoving) return;

            Point currentPosition = e.GetPosition(GraphContainer);
            double deltaX = currentPosition.X - lastMousePosition.X;
            double deltaY = currentPosition.Y - lastMousePosition.Y;

            if (isResizing)
            {
                if (GraphCanvas.ActualWidth + deltaX > 50 && GraphCanvas.ActualHeight + deltaY > 50)
                {
                    if (isTopLeftResizing)
                    {
                        GraphCanvas.Width = Math.Max(GraphCanvas.ActualWidth - deltaX, 50);
                        GraphCanvas.Height = Math.Max(GraphCanvas.ActualHeight - deltaY, 50);
                    }
                    else if (isBottomRightResizing)
                    {
                        GraphCanvas.Width = Math.Max(GraphCanvas.ActualWidth + deltaX, 50);
                        GraphCanvas.Height = Math.Max(GraphCanvas.ActualHeight + deltaY, 50);
                    }
                }

                lastMousePosition = currentPosition;
                NotifyGraphParametersChanged();
            }
            else if (isMoving)
            {
                GraphCanvas.Margin = new Thickness(
                    GraphCanvas.Margin.Left + deltaX,
                    GraphCanvas.Margin.Top + deltaY,
                    0, 0);

                lastMousePosition = currentPosition;
            }

            e.Handled = true;
        }

        private void GraphContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isResizing = false;
            isMoving = false;
            GraphContainer.ReleaseMouseCapture();

            // Проверка, если canvas выехал за границы окна
            var windowBounds = new Rect(0, 0, GraphContainer.ActualWidth, GraphContainer.ActualHeight);
            var canvasBounds = GraphCanvas.TransformToAncestor(GraphContainer).TransformBounds(new Rect(0, 0, GraphCanvas.ActualWidth, GraphCanvas.ActualHeight));

            double offsetX = 0;
            double offsetY = 0;

            if (canvasBounds.Left < windowBounds.Left)
                offsetX = windowBounds.Left - canvasBounds.Left;
            else if (canvasBounds.Right > windowBounds.Right)
                offsetX = windowBounds.Right - canvasBounds.Right;

            if (canvasBounds.Top < windowBounds.Top)
                offsetY = windowBounds.Top - canvasBounds.Top;
            else if (canvasBounds.Bottom > windowBounds.Bottom)
                offsetY = windowBounds.Bottom - canvasBounds.Bottom;

            if (offsetX != 0 || offsetY != 0)
            {
                GraphCanvas.Margin = new Thickness(
                    GraphCanvas.Margin.Left + offsetX,
                    GraphCanvas.Margin.Top + offsetY,
                    0, 0
                );
            }
            e.Handled = true;
        }

        private void TopLeftResizer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isResizing = true;
            isTopLeftResizing = true;
            lastMousePosition = e.GetPosition(GraphContainer);
            e.Handled = true;
        }

        private void BottomRightResizer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isResizing = true;
            isBottomRightResizing = true;
            lastMousePosition = e.GetPosition(GraphContainer);
            e.Handled = true;
        }

        private void Resizer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            isResizing = false;
            isTopLeftResizing = false;
            isBottomRightResizing = false;
            e.Handled = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FontStyleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NotifyGraphParametersChanged?.Invoke();
        }

        private void GraphTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NotifyGraphParametersChanged?.Invoke();
        }
        private void OpenGraphSettingsWindow_Click(object sender, RoutedEventArgs e)
        {
            // Передаем текущие параметры в окно настроек
            int fontStyleIndex = FontStyleComboBox.SelectedIndex;
            var settingsWindow = new GraphSettingsWindow(graphText, parameterA, fontStyleIndex);

            // Показываем окно настроек
            if (settingsWindow.ShowDialog() == true)
            {
                // Применяем изменения из окна настроек
                GraphTextBox.Text = settingsWindow.GraphText;
                ParameterATextBox.Text = settingsWindow.ParameterA.ToString();
                graphText = settingsWindow.GraphText;
                parameterA = settingsWindow.ParameterA;
                FontStyleComboBox.SelectedIndex = settingsWindow.FontStyleIndex;

                NotifyGraphParametersChanged?.Invoke(); // Вызываем событие обновления
            }
        }

        private void ParameterATextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получаем текущее значение TextBox
            string currentText = ParameterATextBox.Text;

            // Пытаемся преобразовать значение в double
            if (double.TryParse(currentText, out double a))
            {
                // Успешно: обновляем параметр и вызываем событие
                parameterA = a;
                NotifyGraphParametersChanged?.Invoke();
            }
            else
            {
                // Не удалось: сохраняем предыдущий корректный текст
                if (!string.IsNullOrEmpty(currentText)) // Проверяем, что текст не пустой
                {
                    int caretPosition = ParameterATextBox.SelectionStart;
                    ParameterATextBox.TextChanged -= ParameterATextBox_TextChanged; // Отписываемся временно
                    ParameterATextBox.Text = currentText.Remove(currentText.Length - 1);
                    ParameterATextBox.SelectionStart = Math.Max(caretPosition - 1, 0); // Защита от отрицательных значений
                    ParameterATextBox.TextChanged += ParameterATextBox_TextChanged; // Подписываемся снова
                }
            }
        }

    }
}