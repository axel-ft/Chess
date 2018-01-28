using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chess
{
    abstract class Piece
    {
        //Booléen mis à true après le premier mouvement de la pièce
        private bool _HasMoved;

        public enum Color {White, Black};

        protected Color pieceColor;

        public Color PieceColor { get => pieceColor;}

        public Piece(Color BlackOrWhite)
        {
            pieceColor = BlackOrWhite;
            HasMoved = false;
        }

        public bool HasMoved
        {
            get => _HasMoved;
            set
            {
                if (value) _HasMoved = value;
            }
        }

        /**
         * GetPossibleMoves(Gameboard, Coords)
         * 
         * Calcule les mouvements possibles en fonction du type de pièce
         * Chaque classe héritée développe cette fonction en fonction des règles du jeu
         * 
         * @return List<Coordinates>
         * Renvoie les coordonnées de destinations possibles
         * 
         * @author Axel Floquet-Trillot
         * */
        public virtual List<Coordinates> GetPossibleMoves(Piece[,] GameBoard, Coordinates Coords)
        {
            return new List<Coordinates>();
        }
    }
}