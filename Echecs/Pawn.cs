using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Pawn : Piece
    {
        public Pawn(Color PieceColor) : base(PieceColor)
        {
            
        }

        public override List<Coordinates> GetPossibleMoves(Piece[,] GameBoard, Coordinates Coords)
        {
            List<Coordinates> CoordList = new List<Coordinates>();
            Color OppositeColor;
            int MoveDirection;

            if (PieceColor == Color.Black)
            {
                OppositeColor = Color.White;
                MoveDirection = 1;
             }
            else
            {
                OppositeColor = Color.Black;
                MoveDirection = -1;
            }

            /*Si aucune pièce n'est présente devant le pion, 
             * et que le pion ne se trouve pas au bord du plateau, 
             * ajouter le mouvement à la liste*/
            if (!HasMoved
                &&Coords.X + MoveDirection * 2 >= 0 
                && Coords.X + MoveDirection * 2 < GameBoard.GetLength(0) 
                && GameBoard[Coords.X + MoveDirection * 2, Coords.Y] == null)
                CoordList.Add(new Coordinates(Coords.X + MoveDirection * 2, Coords.Y));

            /*Si aucune pièce n'est présente devant le pion, 
             * et que le pion ne se trouve pas au bord du plateau, 
             * ajouter le mouvement à la liste*/
            if (Coords.X + MoveDirection >= 0
                && Coords.X + MoveDirection < GameBoard.GetLength(0)
                && GameBoard[Coords.X + MoveDirection, Coords.Y] == null)
                CoordList.Add(new Coordinates(Coords.X + MoveDirection, Coords.Y));

            /*Si une pièce de la couleur opposée est présente en diagonale haute gauche, 
              * et que le pion ne se trouve pas au bord du plateau, 
              * ajouter le mouvement à la liste*/
            if (Coords.Y - 1 >= 0 
                && Coords.X + MoveDirection >= 0 
                && Coords.X + MoveDirection < GameBoard.GetLength(0) 
                && GameBoard[Coords.X + MoveDirection, Coords.Y - 1] != null 
                && GameBoard[Coords.X + MoveDirection, Coords.Y - 1].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X + MoveDirection, Coords.Y - 1));

            /*Si une pièce de la couleur opposée est présente en diagonale haute droite, 
             * et que le pion ne se trouve pas au bord du plateau, 
             * ajouter le mouvement à la liste*/
            if (Coords.Y + 1 < GameBoard.GetLength(1)
                && Coords.X + MoveDirection >= 0
                && Coords.X + MoveDirection < GameBoard.GetLength(0)
                && GameBoard[Coords.X + MoveDirection, Coords.Y + 1] != null 
                && GameBoard[Coords.X + MoveDirection, Coords.Y + 1].PieceColor == OppositeColor)
                CoordList.Add(new Coordinates(Coords.X + MoveDirection, Coords.Y + 1));

            return CoordList;
        }

        public override string ToString()
        {
            if (PieceColor == Color.White)
                return "  WP  ";
            else
                return "  BP  ";
        }
    }
}
