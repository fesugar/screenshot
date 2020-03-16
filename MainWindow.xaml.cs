#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：screenshot
* 项目描述 ：屏幕截图工具
* 类 名 称 ：MainWindow
* 类 描 述 ：使用Graphics截取屏幕
* 命名空间 ：screenshot
* CLR 版本 ：4.0
* 作    者 ：fesugar
* 邮    箱 ：fesugar@fesugar.com
* 创建时间 ：12:42 2020/3/16
* 更新时间 ：12:42 2020/3/16
* 版 本 号 ：v1.0.0.0
* 参考文献 ：https://www.cnblogs.com/yang-fei/p/4029782.html
*******************************************************************
* Copyright @ fesugar 2020. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace screenshot
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private double x;
        private double y;
        private double width;
        private double height;

        private bool isMouseDown = false;
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            isMouseDown = true;
            x = e.GetPosition(null).X;
            y = e.GetPosition(null).Y;
        }
        /// <summary>
        /// 移动鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

            if (isMouseDown)
            {
                // 1. 通过一个矩形来表示目前截图区域
                System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
                double dx = e.GetPosition(null).X;
                double dy = e.GetPosition(null).Y;
                double rectWidth = Math.Abs(dx - x);
                double rectHeight = Math.Abs(dy - y);
                SolidColorBrush brush = new SolidColorBrush(Colors.White);
                rect.Width = rectWidth;
                rect.Height = rectHeight;
                rect.Fill = brush;
                rect.Stroke = brush;
                rect.StrokeThickness = 1;
                switch (dx < x)
                {
                    case true:
                        if (dy < y)
                        {
                            // 左上角
                            Canvas.SetLeft(rect, dx);
                            Canvas.SetTop(rect, dy);
                        }
                        else
                        {
                            // 左下角
                            Canvas.SetLeft(rect, dx);
                            Canvas.SetTop(rect, y);
                        }

                        break;
                    case false:
                        if (dy > y)
                        {
                            // 右上角
                            Canvas.SetLeft(rect, x);
                            Canvas.SetTop(rect, y);
                        }
                        else
                        {
                            // 右下角
                            Canvas.SetLeft(rect, x);
                            Canvas.SetTop(rect, dy);
                        }

                        break;
                }

                CaptureCanvas.Children.Clear();
                CaptureCanvas.Children.Add(rect);

                if (e.LeftButton == MouseButtonState.Released)
                {
                    CaptureCanvas.Children.Clear();
                    // 2. 获得当前截图区域
                    width = Math.Abs(e.GetPosition(null).X - x);
                    height = Math.Abs(e.GetPosition(null).Y - y);

                    if (e.GetPosition(null).X > x)
                    {
                        CaptureScreen(x, y, width, height);
                    }
                    else
                    {
                        CaptureScreen(e.GetPosition(null).X, e.GetPosition(null).Y, width, height);
                    }


                    isMouseDown = false;
                    x = 0.0;
                    y = 0.0;
                    this.Close();
                }
            }
        }
        
        private void CaptureScreen(double x, double y, double width, double height)
        {
            int ix = Convert.ToInt32(x);
            int iy = Convert.ToInt32(y);
            int iw = Convert.ToInt32(width);
            int ih = Convert.ToInt32(height);
            if (iw == 0 || ih == 0) return; // 规避 System.ArgumentException 异常
            System.Drawing.Bitmap bitmap = new Bitmap(iw, ih);
            using (System.Drawing.Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(ix, iy, 0, 0, new System.Drawing.Size(iw, ih));



                Clipboard.SetDataObject(bitmap, true);


                //  SaveFileDialog dialog = new SaveFileDialog();
                //dialog.Filter = "Png Files|*.png";
                //if (dialog.ShowDialog() ==  System.Windows.Forms.DialogResult.OK
                {
                    //    bitmap.Save(dialog.FileName, ImageFormat.Png);
                }
            }
        }

    }
}
