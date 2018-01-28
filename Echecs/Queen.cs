using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Queen : Piece
    {
        public Queen(Color PieceColor) : base(PieceColor)
        {

        }

        public override List<Coordinates> GetPossibleMoves(Piece[,] GameBoard, Coordinates Coords)
        {
            List<Coordinates> CoordList = new List<Coordinates>();
            Color OppositeColor;
            int i = 1, j = 1, k = 1, l = 1, m = 1, n = 1, o = 1, p = 1;

            if (PieceColor == Color.Black)
                OppositeColor = Color.White;
            else
                OppositeColor = Color.Black;

            //BAS
            //Scan des déplacements sur cases vides en dessous de la dame
            while (Coords.X + i < GameBoard.GetLength(0) && GameBoard[Coords.X + i, Coords.Y] == null)
            {
                CoordList.Add(new Coordinates(Coords.X + i, Coords.Y));
                i++;
            }

            //Si une pièce est rencontrée en dessous de la dame, peut-elle être mangée ?
            if (Coords.X + i < GameBoard.GetLength(0) 
                && GameBoard[Coords.X + i, Coords.Y] != null
                && GameBoard[Coords.X + i, Coords.Y].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X + i, Coords.Y));

            //DIAGONALE BAS GAUCHE
            //Scan des déplacements sur cases vides en bas à gauche de la dame
            while (Coords.X + j < GameBoard.GetLength(0) && Coords.Y - j >= 0 && GameBoard[Coords.X + j, Coords.Y - j] == null)
            {
                CoordList.Add(new Coordinates(Coords.X + j, Coords.Y - j));
                j++;
            }

            //Si une pièce est rencontrée en bas à gauche de la dame, peut-elle être mangée ?
            if (Coords.Y - j >= 0
                && Coords.X + j < GameBoard.GetLength(0)
                && GameBoard[Coords.X + j, Coords.Y - j] != null
                && GameBoard[Coords.X + j, Coords.Y - j].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X + j, Coords.Y - j));
            
            //GAUCHE
            //Scan des déplacements sur cases vides à gauche de la dame
            while (Coords.Y - k >= 0 && GameBoard[Coords.X, Coords.Y - k] == null)
            {
                CoordList.Add(new Coordinates(Coords.X, Coords.Y - k));
                k++;
            }

            //Si une pièce est rencontrée à gauche de la dame, peut-elle être mangée ?
            if (Coords.Y - k >= 0
                && GameBoard[Coords.X, Coords.Y - k] != null
                && GameBoard[Coords.X, Coords.Y - k].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X, Coords.Y - k));

            //DIAGONALE HAUT GAUCHE
            //Scan des déplacements sur cases vides en haut à gauche de la dame
            while (Coords.Y - l >= 0 && Coords.X - l >= 0 && GameBoard[Coords.X - l, Coords.Y - l] == null)
            {
                CoordList.Add(new Coordinates(Coords.X - l, Coords.Y - l));
                l++;
            }

            //Si une pièce est rencontrée en haut à gauche de la dame, peut-elle être mangée ?
            if (Coords.X - l >= 0
                && Coords.Y - l >= 0
                && GameBoard[Coords.X - l, Coords.Y - l] != null
                && GameBoard[Coords.X - l, Coords.Y - l].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X - l, Coords.Y - l));

            //HAUT
            //Scan des déplacements sur cases vides au dessus de la dame
            while (Coords.X - m >= 0 && GameBoard[Coords.X - m, Coords.Y] == null)
            {
                CoordList.Add(new Coordinates(Coords.X - m, Coords.Y));
                m++;
            }

            //Si une pièce est rencontrée au dessus de la dame, peut-elle être mangée ?
            if (Coords.X - m >= 0
                && GameBoard[Coords.X - m, Coords.Y] != null
                && GameBoard[Coords.X - m, Coords.Y].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X - m, Coords.Y));

            //DIAGONALE HAUT DROITE
            //Scan des déplacements sur cases vides en haut à droite du fou
            while (Coords.X - n >= 0 && Coords.Y + n < GameBoard.GetLength(1) && GameBoard[Coords.X - n, Coords.Y + n] == null)
            {
                CoordList.Add(new Coordinates(Coords.X - n, Coords.Y + n));
                n++;
            }

            //Si une pièce est rencontrée en haut à droite de la dame, peut-elle être mangée ?
            if (Coords.X - n >= 0
                && Coords.Y + n < GameBoard.GetLength(1)
                && GameBoard[Coords.X - n, Coords.Y + n] != null
                && GameBoard[Coords.X - n, Coords.Y + n].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X - n, Coords.Y + n));

            //DROITE
            //Scan des déplacements sur cases vides à droite de la dame
            while (Coords.Y + o < GameBoard.GetLength(1) && GameBoard[Coords.X, Coords.Y + o] == null)
            {
                CoordList.Add(new Coordinates(Coords.X, Coords.Y + o));
                o++;
            }

            //Si une pièce est rencontrée à droite de la dame, peut-elle être mangée ?
            if (Coords.Y + o < GameBoard.GetLength(1)
                && GameBoard[Coords.X, Coords.Y + o] != null
                && GameBoard[Coords.X, Coords.Y + o].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X, Coords.Y + o));

            //DIAGONALE BAS DROITE
            //Scan des déplacements sur cases vides en bas à droite de la dame
            while (Coords.X + p < GameBoard.GetLength(0) && Coords.Y + p < GameBoard.GetLength(1) && GameBoard[Coords.X + p, Coords.Y + p] == null)
            {
                CoordList.Add(new Coordinates(Coords.X + p, Coords.Y + p));
                p++;
            }

            //Si une pièce est rencontrée en bas à droite de la dame, peut-elle être mangée ?
            if (Coords.X + p < GameBoard.GetLength(0)
                && Coords.Y + p < GameBoard.GetLength(1)
                && GameBoard[Coords.X + p, Coords.Y + p] != null
                && GameBoard[Coords.X + p, Coords.Y + p].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X + p, Coords.Y + p));

            return CoordList;
        }

        public override string ToString()
        {
            if (PieceColor == Color.White)
                return "  WQ  ";
            else
                return "  BQ  ";
        }
    }
}
