using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class King : Piece
    {
        private Coordinates[] Moves;

        public King(Color PieceColor) : base(PieceColor)
        {
            Moves = new Coordinates[8];
            Moves[0] = new Coordinates(-1, 0);
            Moves[1] = new Coordinates(-1, 1);
            Moves[2] = new Coordinates(0, 1);
            Moves[3] = new Coordinates(+1, 1);
            Moves[4] = new Coordinates(+1, 0);
            Moves[5] = new Coordinates(+1, -1);
            Moves[6] = new Coordinates(0, -1);
            Moves[7] = new Coordinates(-1, -1);
        }

        public override List<Coordinates> GetPossibleMoves(Piece[,] GameBoard, Coordinates Coords)
        {
            List<Coordinates> CoordList = new List<Coordinates>();
            Color OppositeColor;

            if (PieceColor == Color.Black)
                OppositeColor = Color.White;
            else
                OppositeColor = Color.Black;

            /*Si aucune pièce n'est présente en [X, Y] ou qu'une pièce peut être mangée, 
             * et que le roi ne se trouve pas au bord du plateau, 
             * ajouter le mouvement à la liste*/
            foreach (Coordinates Move in Moves)
            {
                if ((Coords.X + Move.X >= 0 && Coords.X + Move.X < GameBoard.GetLength(0))
                    && (Coords.Y + Move.Y >= 0 && Coords.Y + Move.Y < GameBoard.GetLength(1))
                    && (GameBoard[Coords.X + Move.X, Coords.Y + Move.Y] == null || GameBoard[Coords.X + Move.X, Coords.Y + Move.Y].PieceColor == OppositeColor))
                {
                    CoordList.Add(new Coordinates(Coords.X + Move.X, Coords.Y + Move.Y));
                }
            }

            return CoordList;
        }

        /**
         * locateKing(GameBoard, WhichKing)
         * 
         * Récupère la position d'un des deux rois en fonction de sa couleur
         * 
         * @return Coordinates
         * Renvoie les coordonnées du roi demandé
         * 
         * @author Axel Floquet-Trillot
         * */
        public static Coordinates locateKing(Piece[,] GameBoard, Color WhichKing)
        {
            //Détection de la position d'un roi
            for (int x = 0; x < GameBoard.GetLength(0); x++)
            {
                for (int y = 0; y < GameBoard.GetLength(1); y++)
                {
                    if (GameBoard[x, y] != null && WhichKing == Color.White && GameBoard[x, y].ToString() == "  WK  ")
                        return new Coordinates(x, y);
                    if (GameBoard[x, y] != null && WhichKing == Color.Black && GameBoard[x, y].ToString() == "  BK  ")
                        return new Coordinates(x, y);
                }
            }

            return null;
        }

        public override string ToString()
        {
            if (PieceColor == Color.White)
                return "  WK  ";
            else
                return "  BK  ";
        }
    }
}
