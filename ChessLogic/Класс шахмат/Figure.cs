using System;
namespace Класс_шахмат
{
    public interface IGui
    {
        void Replace(Point from, Point where);
        void ArrangeFiguresForEnum();
        void DeleteImage(Point where);
        void EndGame();
    }
    public class Board
    {
        public IGui gui;
        public Board(IGui gui)
        {
            this.gui = gui;
            ArrangeFigures();
        }
        public object[,] ChessBoard { get; private set; }

        Color WhoIsMove = Color.White;
        protected bool CheckBoardCapacity(Point where)
        {
            return where.x <= 7 && where.x >= 0 && where.y <= 7 && where.y >= 0;
        }
        protected virtual bool CheckBoardAndMoveMyself(Point where, Point from)
        {
            return CheckBoardCapacity(where) && CheckBoardCapacity(from);
        }
        public Figuers[,] ArrangeFiguresForEnum()
        {
            Figuers[,] enumBoard = new Figuers[8, 8];
            enumBoard[0, 0] = Figuers.BlackRook;
            enumBoard[7, 0] = Figuers.BlackRook;
            enumBoard[1, 0] = Figuers.BlackKnight;
            enumBoard[6, 0] = Figuers.BlackKnight;
            enumBoard[2, 0] = Figuers.BlackBishop;
            enumBoard[5, 0] = Figuers.BlackBishop;
            enumBoard[3, 0] = Figuers.BlackQueen;
            enumBoard[4, 0] = Figuers.BlackKing;
            for (int i = 0; i < 8; i++)
            {
                enumBoard[i, 1] = Figuers.BlackPawn;
            }

            enumBoard[0, 7] = Figuers.WhiteRook;
            enumBoard[7, 7] = Figuers.WhiteRook;
            enumBoard[1, 7] = Figuers.WhiteKnight;
            enumBoard[6, 7] = Figuers.WhiteKnight;
            enumBoard[2, 7] = Figuers.WhiteBishop;
            enumBoard[5, 7] = Figuers.WhiteBishop;
            enumBoard[3, 7] = Figuers.WhiteQueen;
            enumBoard[4, 7] = Figuers.WhiteKing;
            for (int i = 0; i < 8; i++)
            {
                enumBoard[i, 6] = Figuers.WhitePawn;
            }

            for (int i = 0; i <= 7; i++)
            {
                for (int j = 2; j <= 5; j++)
                {
                    enumBoard[i, j] = Figuers.Empty;
                }
            }
            return enumBoard;
        }
        public object[,] ArrangeFigures()
        {
            ChessBoard = new object[8, 8];
            ChessBoard[0, 0] = new BlackRook(new Point(0, 0), this);
            ChessBoard[7, 0] = new BlackRook(new Point(7, 0), this);
            ChessBoard[1, 0] = new BlackKnight(new Point(1, 0), this);
            ChessBoard[6, 0] = new BlackKnight(new Point(6, 0), this);
            ChessBoard[2, 0] = new BlackBishop(new Point(2, 0), this);
            ChessBoard[5, 0] = new BlackBishop(new Point(5, 0), this);
            ChessBoard[3, 0] = new BlackQueen(new Point(3, 0), this);
            ChessBoard[4, 0] = new BlackKing(new Point(4, 0), this);
            for (int i = 0; i < 8; i++)
            {
                ChessBoard[i, 1] = new BlackPawn(new Point(i, 1), this);
            }

            ChessBoard[0, 7] = new WhiteRook(new Point(0, 7), this);
            ChessBoard[7, 7] = new WhiteRook(new Point(7, 7), this);
            ChessBoard[1, 7] = new WhiteKnight(new Point(1, 7), this);
            ChessBoard[6, 7] = new WhiteKnight(new Point(6, 7), this);
            ChessBoard[2, 7] = new WhiteBishop(new Point(2, 7), this);
            ChessBoard[5, 7] = new WhiteBishop(new Point(5, 7), this);
            ChessBoard[3, 7] = new WhiteQueen(new Point(3, 7), this);
            ChessBoard[4, 7] = new WhiteKing(new Point(4, 7), this);
            for (int i = 0; i < 8; i++)
            {
                ChessBoard[i, 6] = new WhitePawn(new Point(i, 6), this);
            }

            return ChessBoard;
        }
        public void MoveRequest(Point from, Point where)
        {
            if (CheckBoardAndMoveMyself(where, from))
                if (ChessBoard[from.x, from.y] != null)
                    if (ChessBoard[from.x, from.y] is Figure figure && figure.Color == WhoIsMove)
                        if (figure.CheckMove(where))
                            Replace(from, where);
        }
        internal void Replace(Point from, Point where)
        {
            if (ChessBoard[where.x, where.y] != null)
            {
                gui.DeleteImage(where);
                if (ChessBoard[where.x, where.y] is King)
                {
                    gui.EndGame();
                }
            }
            gui.Replace(from, where);
            ChessBoard[where.x, where.y] = ChessBoard[from.x, from.y];
            ChessBoard[from.x, from.y] = null;
            if (WhoIsMove == Color.White)
            {
                WhoIsMove = Color.Black;
            }
            else WhoIsMove = Color.White;
        }
    }
    enum Color
    {
        Black,
        White
    }
    public enum Figuers
    {
        WhitePawn, BlackPawn, WhiteRook, BlackRook, WhiteKing, BlackKing, WhiteKnight, BlackKnight, WhiteBishop, BlackBishop, WhiteQueen, BlackQueen, Empty
    }
    public class Point
    {
        public int x, y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    abstract class Figure
    {
        private readonly Board board;
        public Figure(Point position, Board board)
        {
            myPosition = position;
            this.board = board;
        }
        public abstract Color Color { get; }
        protected Point myPosition;
        public virtual bool CheckMove(Point move)
        {
            if (CheckEat(move) && CheckMoveCorrect(move) && CheckObstackles(move))
            {
                myPosition.x = move.x;
                myPosition.y = move.y;
                return true;
            }
            return false;
        }
        protected abstract bool CheckMoveCorrect(Point where);
        protected virtual bool CheckObstackles(Point where)
        {
            int xDir = where.x - myPosition.x < 0 ? -1 : 1;
            int yDir = where.y - myPosition.y < 0 ? -1 : 1;
            Point currentPosition = new Point(myPosition.x, myPosition.y);
            do
            {
                if (currentPosition.x != where.x)
                    currentPosition.x += xDir;
                if (currentPosition.y != where.y)
                    currentPosition.y += yDir;
                if (currentPosition.x != where.x && currentPosition.y != where.y && board.ChessBoard[currentPosition.x, currentPosition.y] != null)
                    return false;
            } while (currentPosition.x != where.x && currentPosition.y != where.y);
            return true;
        }
            protected bool CheckEat(Point where)
        {
            if (board.ChessBoard[where.x, where.y] != null)
                if (board.ChessBoard[where.x, where.y] is Figure figure)
                    return figure.Color != this.Color;
                else
                    return false;
            else
                return true;
        }
    }
    internal abstract class King : Figure
    {
        public King(Point position, Board board) : base(position, board) { }
        protected override bool CheckMoveCorrect(Point where)
        {
            return Math.Abs(where.x - myPosition.x) <= 1 && Math.Abs(where.y - myPosition.y) <= 1;
        }
    }
    
    class WhiteKing : King
    {
        public WhiteKing(Point position, Board board) : base(position, board) { }

        public override Color Color => Color.White;
    }
    class BlackKing : King
    {
        public BlackKing(Point position, Board board) : base(position, board) { }

        public override Color Color => Color.Black;
    }
    abstract class Pawn : Figure
    {
        public Pawn(Point position, Board board) : base(position, board) { }
        protected bool isMoved = false;
        public override bool CheckMove(Point move)
        {
            if (base.CheckMove(move))
            {
                isMoved = true;
                return true;
            }
            else return false;
        }
    }
    class WhitePawn : Pawn
    {
        public WhitePawn(Point position, Board board) : base(position, board) { }
        protected override bool CheckMoveCorrect(Point where)
        {
            if (where.y - myPosition.y == -2 && !isMoved)
                return true;
            else
                return where.y - myPosition.y == -1 && Math.Abs(where.x - myPosition.x) <= 1;
        }
        public override Color Color => Color.White;
    }
    class BlackPawn : Pawn
    {
        public BlackPawn(Point position, Board board) : base(position, board) { }
        protected override bool CheckMoveCorrect(Point where)
        {
            if (where.y - myPosition.y == 2 && !isMoved)
                return true;
            else
                return where.y - myPosition.y == 1 && Math.Abs(where.x - myPosition.x) <= 1;
        }
        public override Color Color => Color.Black;
    }
    abstract class Knight : Figure
    {
        public Knight(Point position, Board board) : base(position, board) { }
        protected override bool CheckMoveCorrect(Point where)
        {
            return Math.Pow(myPosition.x - where.x, 2) + Math.Pow(myPosition.y - where.y, 2) == 5;
        }
        protected override bool CheckObstackles(Point point)
        {
            return true;
        }
    }
    class WhiteKnight : Knight
    {
        public WhiteKnight(Point position, Board board) : base(position, board) { }
        public override Color Color => Color.White;
    }
    class BlackKnight : Knight
    {
        public BlackKnight(Point position, Board board) : base(position, board) { }
        public override Color Color => Color.Black;
    }
    abstract class Queen : Figure
    {
        public Queen(Point position, Board board) : base(position, board) { }
        protected override bool CheckMoveCorrect(Point where)
        {
            return Math.Abs(where.x - myPosition.x) == 0 || Math.Abs(where.y - myPosition.y) == 0 || Math.Abs(where.x - myPosition.x) == Math.Abs(where.y - myPosition.y);
        }
    }
    class WhiteQueen : Queen
    {
        public WhiteQueen(Point position, Board board) : base(position, board) { }
        public override Color Color => Color.White;
    }
    class BlackQueen : Queen
    {
        public BlackQueen(Point position, Board board) : base(position, board) { }
        public override Color Color => Color.Black;
    }
    abstract class Bishop : Figure
    {
        public Bishop(Point position, Board board) : base(position, board) { }
        protected override bool CheckMoveCorrect(Point where)
        {
            return Math.Abs(where.x - myPosition.x) == Math.Abs(where.y - myPosition.y);
        }
    }
    class WhiteBishop : Bishop
    {
        public WhiteBishop(Point position, Board board) : base(position, board) { }
        public override Color Color => Color.White;
    }
    class BlackBishop : Bishop
    {
        public BlackBishop(Point position, Board board) : base(position, board) { }
        public override Color Color => Color.Black;
    }
    abstract class Rook : Figure
    {
        public Rook(Point position, Board board) : base(position, board) { }
        protected override bool CheckMoveCorrect(Point where)
        {
            return Math.Abs(where.x - myPosition.x) == 0 || Math.Abs(where.y - myPosition.y) == 0;
        }
    }
    class WhiteRook : Rook
    {
        public WhiteRook(Point position, Board board) : base(position, board) { }
        public override Color Color => Color.White;
    }
    class BlackRook : Rook
    {
        public BlackRook(Point position, Board board) : base(position, board) { }
        public override Color Color => Color.Black;
    }
}
