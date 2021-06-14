using System;
using System.IO;
using System.Collections.Generic;
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
using Класс_шахмат;

namespace WPF_Chess
{
    class Gui : IGui
    {
        public Gui(Canvas canvas)
        {
            this.canvas = canvas;
        }
        private Класс_шахмат.Point point = new Класс_шахмат.Point(0,0);
        private List<Image> images;
        private Board board;
        private Figuers[,] figures;
        private readonly Canvas canvas;
        private readonly Image img;
        private readonly string path = Environment.CurrentDirectory;
        private bool select = false;
        public Image CreateImg(Image img, Figuers figure, int x, int y)
        {
            img = new Image();
            img.Source = BitmapFrame.Create(new Uri(path + CreateSource(figure)));
            img.SetValue(Canvas.LeftProperty, x * canvas.ActualWidth / 8);
            img.SetValue(Canvas.TopProperty, y * canvas.ActualHeight / 8);
            img.Width = canvas.ActualWidth / 8;
            img.Height = canvas.ActualHeight / 8;
            return img;
        }
        public string CreateSource(Figuers figure)
        {
            switch (figure)
            {
                case Figuers.WhitePawn:
                    return "/images/wP.png";
                case Figuers.BlackPawn:
                    return "/images/bP.png";
                case Figuers.WhiteRook:
                    return "/images/wR.png";
                case Figuers.BlackRook:
                    return "/images/bR.png";
                case Figuers.WhiteKing:
                    return "/images/wK.png";
                case Figuers.BlackKing:
                    return "/images/bK.png";
                case Figuers.WhiteKnight:
                    return "/images/wN.png";
                case Figuers.BlackKnight:
                    return "/images/bN.png";
                case Figuers.WhiteBishop:
                    return "/images/wB.png";
                case Figuers.BlackBishop:
                    return "/images/bB.png";
                case Figuers.WhiteQueen:
                    return "/images/wQ.png";
                case Figuers.BlackQueen:
                    return "/images/bQ.png";
                default:
                    throw new Exception("Такой фигуры нету");
            }
        }
        public void SelectFigure(Класс_шахмат.Point point)
        {
            if (!select)
            {
                this.point.x = point.x;
                this.point.y = point.y;
                select = true;
            }
            else
            {
                board.MoveRequest(this.point, point);
                select = false;
            }
        }
        public void ArrangeFiguresForEnum()
        {
            board = new Board(this);
            figures = board.ArrangeFiguresForEnum();
            images = new List<Image>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (figures[i, j] != Figuers.Empty)
                    {
                        images.Add(CreateImg(img, figures[i, j], i, j));
                    }
                }
            }
        }
        public void ChildrenAdd()
        {
            foreach (var item in images)
            {
                canvas.Children.Add(item);
            }
        }
        public Класс_шахмат.Point GetCords(System.Windows.Point pointer)
        {
            Класс_шахмат.Point point = new Класс_шахмат.Point(0, 0);
            point.x = (int)(pointer.X / (canvas.ActualWidth / 8));
            point.y = (int)(pointer.Y / (canvas.ActualWidth / 8));
            return point;
        }
        public void Replace(Класс_шахмат.Point from, Класс_шахмат.Point where)
        {
            foreach (var item in images)
            {
                if ((int)(Canvas.GetLeft(item) / (canvas.ActualWidth / 8)) == from.x && (int)(Canvas.GetTop(item) / (canvas.ActualHeight / 8)) == from.y)
                {
                    item.SetValue(Canvas.LeftProperty, where.x * canvas.ActualWidth / 8);
                    item.SetValue(Canvas.TopProperty, where.y * canvas.ActualHeight / 8);
                    break;
                }
            }
        }

        public Image ReturnImage(Класс_шахмат.Point where)
        {
            foreach (var item in images)
            {
                if ((int)(Canvas.GetLeft(item) / (canvas.ActualWidth / 8)) == where.x && (int)(Canvas.GetTop(item) / (canvas.ActualHeight / 8)) == where.y)
                {
                    return item;
                }
            }
            throw new Exception("Не удалось найти картинку, которая должна удалиться");
        }

        public void DeleteImage(Класс_шахмат.Point where)
        {
            Image img = ReturnImage(where);
            images.Remove(img);
            canvas.Children.Remove(img);
        }

        public void EndGame()
        {
            MessageBox.Show("Конец игры");
        }
    }
}
