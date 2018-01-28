using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Rook : Piece
    {
        public Rook(Color PieceColor) : base(PieceColor)
        {

        }

        public override List<Coordinates> GetPossibleMoves(Piece[,] GameBoard, Coordinates Coords)
        {
            List<Coordinates> CoordList = new List<Coordinates>();
            Color OppositeColor;
            int i = 1, j = 1, k = 1, l = 1;

            if (PieceColor == Color.Black)
                OppositeColor = Color.White;
            else
                OppositeColor = Color.Black;

            //BAS
            //Scan des déplacements sur cases vides en dessous de la tour
            while (Coords.X + i < GameBoard.GetLength(0) && GameBoard[Coords.X + i, Coords.Y] == null)
            {
                CoordList.Add(new Coordinates(Coords.X + i, Coords.Y));
                i++;
            }

            //Si une pièce est rencontrée en dessous de la tour, peut-elle être mangée ?
            if (Coords.X + i < GameBoard.GetLength(0) 
                && GameBoard[Coords.X + i, Coords.Y] != null
                && GameBoard[Coords.X + i, Coords.Y].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X + i, Coords.Y));

            //HAUT
            //Scan des déplacements sur cases vides au dessus de la tour
            while (Coords.X - j >= 0 && GameBoard[Coords.X - j, Coords.Y] == null)
            {
                CoordList.Add(new Coordinates(Coords.X - j, Coords.Y));
                j++;
            }

            //Si une pièce est rencontrée au dessus de la tour, peut-elle être mangée ?
            if (Coords.X - j >= 0
                && GameBoard[Coords.X - j, Coords.Y] != null 
                && GameBoard[Coords.X - j, Coords.Y].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X - j, Coords.Y));

            
            //DROITE
            //Scan des déplacements sur cases vides à droite de la tour
            while (Coords.Y + k < GameBoard.GetLength(1) && GameBoard[Coords.X, Coords.Y + k] == null)
            {
                CoordList.Add(new Coordinates(Coords.X, Coords.Y + k));
                k++;
            }

            //Si une pièce est rencontrée à droite de la tour, peut-elle être mangée ?
            if (Coords.Y + k < GameBoard.GetLength(1)
                && GameBoard[Coords.X, Coords.Y + k] != null
                && GameBoard[Coords.X, Coords.Y + k].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X, Coords.Y + k));

            
            //GAUCHE
            //Scan des déplacements sur cases vides à gauche de la tour
            while (Coords.Y - l >= 0 && GameBoard[Coords.X, Coords.Y - l] == null)
            {
                CoordList.Add(new Coordinates(Coords.X, Coords.Y - l));
                l++;
            }

            //Si une pièce est rencontrée à gauche de la tour, peut-elle être mangée ?
            if (Coords.Y - l >= 0
                && GameBoard[Coords.X, Coords.Y - l] != null
                && GameBoard[Coords.X, Coords.Y - l].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X, Coords.Y - l));

            return CoordList;
        }

        public override string ToString()
        {
            if (PieceColor == Color.White)
                return "  WR  ";
            else
                return "  BR  ";
        }
    }
}
