using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Bishop : Piece
    {
        public Bishop(Color PieceColor) : base(PieceColor)
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

            //DIAGONALE BAS DROITE
            //Scan des déplacements sur cases vides en bas à droite du fou
            while (Coords.X + i < GameBoard.GetLength(0) && Coords.Y + i < GameBoard.GetLength(1) && GameBoard[Coords.X + i, Coords.Y + i] == null)
            {
                CoordList.Add(new Coordinates(Coords.X + i, Coords.Y + i));
                i++;
            }

            //Si une pièce est rencontrée en bas à droite du fou, peut-elle être mangée ?
            if (Coords.X + i < GameBoard.GetLength(0)
                && Coords.Y + i < GameBoard.GetLength(1)
                && GameBoard[Coords.X + i, Coords.Y + i] != null
                && GameBoard[Coords.X + i, Coords.Y + i].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X + i, Coords.Y + i));

            //DIAGONALE BAS GAUCHE
            //Scan des déplacements sur cases vides en bas à gauche du fou
            while (Coords.X + j < GameBoard.GetLength(0) && Coords.Y - j >= 0 && GameBoard[Coords.X + j, Coords.Y - j] == null)
            {
                CoordList.Add(new Coordinates(Coords.X + j, Coords.Y - j));
                j++;
            }

            //Si une pièce est rencontrée en bas à gauche du fou, peut-elle être mangée ?
            if (Coords.Y - j >= 0
                && Coords.X + j < GameBoard.GetLength(0)
                && GameBoard[Coords.X + j, Coords.Y - j] != null
                && GameBoard[Coords.X + j, Coords.Y - j].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X + j, Coords.Y - j));

            //DIAGONALE HAUT GAUCHE
            //Scan des déplacements sur cases vides en haut à gauche du fou
            while (Coords.X - k >=0 && Coords.Y - k >=0 && GameBoard[Coords.X - k, Coords.Y - k] == null)
            {
                CoordList.Add(new Coordinates(Coords.X - k, Coords.Y - k));
                k++;
            }

            //Si une pièce est rencontrée en haut à gauche du fou, peut-elle être mangée ?
            if (Coords.X - k >= 0
                && Coords.Y - k >= 0
                && GameBoard[Coords.X - k, Coords.Y - k] != null
                && GameBoard[Coords.X - k, Coords.Y - k].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X - k, Coords.Y - k));

            //DIAGONALE HAUT DROITE
            //Scan des déplacements sur cases vides en haut à droite du fou
            while (Coords.X - l >= 0 && Coords.Y + l < GameBoard.GetLength(1) && GameBoard[Coords.X - l, Coords.Y + l] == null)
            {
                CoordList.Add(new Coordinates(Coords.X - l, Coords.Y + l));
                l++;
            }

            //Si une pièce est rencontrée en haut à droite du fou, peut-elle être mangée ?
            if (Coords.X - l >= 0
                && Coords.Y + l < GameBoard.GetLength(1)
                && GameBoard[Coords.X - l, Coords.Y + l] != null
                && GameBoard[Coords.X - l, Coords.Y + l].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X - l, Coords.Y + l));

            return CoordList;
        }

        public override string ToString()
        {
            if (PieceColor == Color.White)
                return "  WB  ";
            else
                return "  BB  ";
        }
    }
}
