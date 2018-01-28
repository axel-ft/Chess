using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    /**
    * Game
    * 
    * Classe contenant tous les mécanismes du jeu : 
    * - La création, le remplissage et l'affichage du plateau
    * - La demande, la proposition, la vérification et l'exécution de déplacements, tour par tour
    * - La détection de l'échec d'un roi et obligation de le protéger (lors du déplacement suivant)
    * - L'impossibilité de se placer en échec
    * - L'affichage d'une liste des pièces mangées
    * 
    * @author Axel Floquet-Trillot
    * */
    class Game
    {
        private Piece[,] GameBoard;
        private Piece.Color PlayingNow;
        private bool FirstTurn;
        private List<Coordinates> WhiteMateThreats;
        private List<Coordinates> BlackMateThreats;
        private List<Piece> EatenByBlack;
        private List<Piece> EatenByWhite;
        private string SpacingCase = "      ";

        //Sert à choisir les messages à afficher
        private enum CaseStatus { BeforeTest, Mine, Empty, Enemy, Mate, MateImpossibleMove, ImpossibleMove, Canceled, NoMove};

        private enum ReadWay { Row, Column};

        /**
         * Game()
         * 
         * Constructeur de la classe.
         * L'instanciation déclenche automatiquement le jeu.
         * 
         * @author Axel Floquet-Trillot
         * */
        public Game()
        {
            //Création du plateau, Blancs pour le premier tour
            GameBoard = new Piece[8,8];
            PlayingNow = Piece.Color.White;
            FirstTurn = true;
            
            //Initialisation des listes de menaces aux rois
            WhiteMateThreats = new List<Coordinates>();
            BlackMateThreats = new List<Coordinates>();

            //Initialisation des listes de pièces mangées
            EatenByBlack = new List<Piece>();
            EatenByWhite = new List<Piece>();

            StartGame();
        }

        /**
         * StartGame()
         * 
         * Remplit le plateau et lance le premier tour
         * 
         * @author Axel Floquet-Trillot
         * */
        public void StartGame()
        {
            //Remplissage du plateau
            for (int i = 0; i < GameBoard.GetLength(0); i++)
            {
                switch(i)
                {
                    case 0:
                        for (int j = 0; j < GameBoard.GetLength(1); j++)
                        {
                            switch(j)
                            {
                                case 0: case 7: GameBoard[i,j] = new Rook(Piece.Color.Black);
                                    break;

                                case 1: case 6:  GameBoard[i, j] = new Knight(Piece.Color.Black);
                                    break;

                                case 2: case 5: GameBoard[i, j] = new Bishop(Piece.Color.Black);
                                    break;

                                case 3: GameBoard[i, j] = new Queen(Piece.Color.Black);
                                    break;

                                case 4: GameBoard[i, j] = new King(Piece.Color.Black);
                                    break;
                            }
                        }
                        break;

                    case 1:
                        for (int j = 0; j < GameBoard.GetLength(1); j++)
                            GameBoard[i, j] = new Pawn(Piece.Color.Black);
                        break;

                    case 2: case 3: case 4: case 5:
                        for (int j = 0; j < GameBoard.GetLength(1); j++)
                            GameBoard[i, j] = null;
                        break;

                    case 6:
                        for (int j = 0; j < GameBoard.GetLength(1); j++)
                            GameBoard[i, j] = new Pawn(Piece.Color.White);
                        break;

                    case 7:
                        for (int j = 0; j < GameBoard.GetLength(1); j++)
                        {
                            switch (j)
                            {
                                case 0: case 7: GameBoard[i, j] = new Rook(Piece.Color.White);
                                    break;

                                case 1: case 6: GameBoard[i, j] = new Knight(Piece.Color.White);
                                    break;

                                case 2: case 5: GameBoard[i, j] = new Bishop(Piece.Color.White);
                                    break;

                                case 3: GameBoard[i, j] = new Queen(Piece.Color.White);
                                    break;

                                case 4: GameBoard[i, j] = new King(Piece.Color.White);
                                    break;
                            }
                        }
                        break;
                }
                
            }

            PlayTurn();
        }

        /**
         * PlayTurn()
         * 
         * Exécute en boucle des tours tant qu'un des deux joueurs n'est pas échec et mat
         * 
         * @author Axel Floquet-Trillot
         * */
        private void PlayTurn()
        {
            bool ExecutedOnce = false;
            Coordinates SelectedPiece;
            List<Coordinates> AllowedDestinations = null;
            Coordinates Destination;

            //Boucle exécutée tant qu'il n'y a aucun échec et mat, chaque tour de boucle correspond à un tour de joueur
            do
            {
                //Boucle exécutée tant que les coordonnées entrées ne correspondent pas à une pièce déplaçable
                do
                {
                    PrintBoard();

                    if (ExecutedOnce && AllowedDestinations != null)
                        PrintMessageCorrespondingToStatus(CaseStatus.NoMove, AllowedDestinations);

                    SelectedPiece = AskOneOfMyPiecesCoordinates();
                    AllowedDestinations = GameBoard[SelectedPiece.X, SelectedPiece.Y].GetPossibleMoves(GameBoard, SelectedPiece);
                    PrintPossibleMoves(AllowedDestinations);
                    
                    ExecutedOnce = true;
                } while (AllowedDestinations.Count() == 0);

                Destination = AskValidDestination(SelectedPiece, AllowedDestinations);
                MovePiece(SelectedPiece, Destination);
                AllowedDestinations.Clear();
                ExecutedOnce = false;
                NextTurn();
            } while (!IsCheckMate());
        }

        /**
         * NextTurn()
         * 
         * Déclenche le prochain tour en modifiant la propriété PlayingNow
         * 
         * @author Axel Floquet-Trillot
         * */
        private void NextTurn()
        {
            if (PlayingNow == Piece.Color.White)
                PlayingNow = Piece.Color.Black;
            else
                PlayingNow = Piece.Color.White;
        }

        /**
         * WhoIsPlaying()
         * 
         * Affiche la couleur du joueur en train de joueur ainsi que les pièces qu'il à mangé
         * 
         * @author Axel Floquet-Trillot
         * */
        private void WhoIsPlaying()
        {
            if (PlayingNow == Piece.Color.White)
            {
                if (FirstTurn)
                {
                    Console.Write("Les Blancs Commencent !");
                    FirstTurn = false;
                }
                else
                {
                    Console.Write("Au tour des Blancs - Pièces mangées : ");
                    if (EatenByWhite.Count() > 0)
                        foreach (Piece Piece in EatenByWhite)
                            Console.Write(Piece.ToString());
                    else
                        Console.Write("Aucune");
                }
            }
            else
            {
                Console.Write("Au tour des Noirs - Pièces mangées : ");

                if (EatenByBlack.Count() > 0)
                    foreach (Piece Piece in EatenByBlack)
                        Console.Write(Piece.ToString());
                else
                    Console.Write("Aucune");
            }

            Console.WriteLine();
        }

        /**
         * PrintPossibleMoves(AllowedDestinations)
         * 
         * Affiche les déplacements disponibles lorsqu'une pièce est choisie
         * Dans le cas d'un échec, seuls les déplacements protégeant le roi sont proposés
         * 
         * @author Axel Floquet-Trillot
         * */
        private void PrintPossibleMoves(List<Coordinates> AllowedDestinations)
        {
            Console.WriteLine("Mouvements Possibles");

            List<Coordinates> MatePossibleMoves;
            List<Coordinates> AlreadyPrinted = new List<Coordinates>();

            MatePossibleMoves = FindPiecesCoordinatesAndMovesToProtectMyKing()[1];

            if (IsMateDetected() && !IsCheckMate() && MatePossibleMoves.Count > 0)
            {
                foreach (Coordinates AllowedDestination in AllowedDestinations)
                {
                    foreach (Coordinates Coord in MatePossibleMoves)
                    {
                        if (Coord.X == AllowedDestination.X && Coord.Y == AllowedDestination.Y && !CoordinateIsInList(Coord, AlreadyPrinted))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Coord.X + 1);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(" - ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(Coord.Y + 1);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine();

                            AlreadyPrinted.Add(Coord);
                        }  
                    }
                }
            } else
            {
                foreach (Coordinates Coord in AllowedDestinations)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(Coord.X + 1);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" - ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(Coord.Y + 1);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                }
            }
        }

        /**
         * CoordinateIsInList(Element, List)
         * 
         * Teste la présence d'un élément Coordinates dans une List
         * 
         * @return bool
         * true si présence
         * false si absence
         * 
         * @author Axel Floquet-Trillot
         * */
        private bool CoordinateIsInList(Coordinates Element, List<Coordinates> List)
        {
            foreach (Coordinates Coord in List)
            {
                if (Element.IsSameAs(Coord))
                    return true;
            }

            return false;
        }

        /**
         * IsMateDetected()
         * 
         * Détecte l'absence ou la présence d'un échec sur le plateau
         * En cas d'échec, les coordonnées des pièces menaçant le roi d'une couleur sont stockées dans une liste initialisée dans le constructeur
         * Cette liste est vidée à chaque début d'exécution de la fonction
         * 
         * @return bool
         * true si échec
         * false si absence d'échec
         * 
         * @author Axel Floquet-Trillot
         * */
        private bool IsMateDetected()
        {
            Coordinates WhiteKing = King.locateKing(GameBoard, Piece.Color.White);
            Coordinates BlackKing = King.locateKing(GameBoard, Piece.Color.Black);

            WhiteMateThreats.Clear();
            BlackMateThreats.Clear();
            
            //Exception levée si un des rois n'est pas trouvé
            if (WhiteKing == null || BlackKing == null)
                throw (new Exception("Un des rois n'a pas pu être trouvé"));

            //Double boucle pour parcourir chaque case du plateau
            for (int x = 0; x < GameBoard.GetLength(0); x++)
            {
                for (int y = 0; y < GameBoard.GetLength(1); y++)
                {
                    if (GameBoard[x, y] != null)
                    {
                        //Détection des pièces pouvant manger le roi
                        List<Coordinates> PossibleMovesOfCurrentPiece = GameBoard[x, y].GetPossibleMoves(GameBoard, new Coordinates(x, y));

                        foreach (Coordinates Coord in PossibleMovesOfCurrentPiece)
                        {
                            if (Coord.X == WhiteKing.X && Coord.Y == WhiteKing.Y && GameBoard[x, y].PieceColor == Piece.Color.Black)
                                WhiteMateThreats.Add(new Coordinates(x, y));
                            if (Coord.X == BlackKing.X && Coord.Y == BlackKing.Y && GameBoard[x, y].PieceColor == Piece.Color.White)
                                BlackMateThreats.Add(new Coordinates(x, y));
                        }
                    }
                }
            }

            //Si une des deux liste contient un élement, renvoie true
            if (WhiteMateThreats.Count() > 0 || BlackMateThreats.Count() > 0)
                return true;
            else
                return false;
        }

        /**
         * FindPiecesCoordinatesAndMovesToProtectMyKing()
         * 
         * Analyse la totalité du plateau afin de trouver toutes les pièces et leur déplacement(s) associé(s) permettant de protéger le roi en cas d'échec
         * 
         * @return List<Coordinates>[]
         * FindPiecesCoordinatesAndMovesToProtectMyKing()[0] permet de récupérer les pièces pouvant protéger un roi
         * FindPiecesCoordinatesAndMovesToProtectMyKing()[1] permet de récupérer les déplacements pouvant protéger un roi
         * 
         * @author Axel Floquet-Trillot
         * */
        private List<Coordinates>[] FindPiecesCoordinatesAndMovesToProtectMyKing()
        {
            Coordinates MyKing;
            List<Coordinates> MateThreat = new List<Coordinates>();
            List<Coordinates> PiecesCoordinatesToProtectMyKing = new List<Coordinates>();
            List<Coordinates> MatePossibleMoves = new List<Coordinates>();

            if (PlayingNow == Piece.Color.White)
            {
                MyKing = King.locateKing(GameBoard, Piece.Color.White);
                MateThreat = WhiteMateThreats;
            } else
            {
                MyKing = King.locateKing(GameBoard, Piece.Color.Black);
                MateThreat = BlackMateThreats;
            }

            //Exception levée si un des rois n'est pas trouvé
            if (MyKing == null)
                throw (new Exception("Un des rois n'a pas pu être trouvé"));

            //Recherche des pièces pouvant protéger le roi
            if (MateThreat.Count() > 0 && PlayingNow == GameBoard[MyKing.X, MyKing.Y].PieceColor)
            {
                foreach (Coordinates Threat in MateThreat)
                {
                    for (int x = 0; x < GameBoard.GetLength(0); x++)
                    {
                        for (int y = 0; y < GameBoard.GetLength(1); y++)
                        {
                            if (GameBoard[x, y] != null
                                && GameBoard[x, y].PieceColor == PlayingNow)
                            {
                                foreach (Coordinates Coord in GameBoard[x, y].GetPossibleMoves(GameBoard, new Coordinates(x, y)))
                                {
                                    if (!GameBoard[x, y].Equals(GameBoard[MyKing.X, MyKing.Y]))
                                    {
                                        //Si la menace peut être mangée
                                        if (Coord.X == Threat.X && Coord.Y == Threat.Y)
                                        {
                                            PiecesCoordinatesToProtectMyKing.Add(new Coordinates(x, y));
                                            MatePossibleMoves.Add(new Coordinates(Coord.X, Coord.Y));
                                        }

                                        //Si une pièce peut s'interposer entre le roi et la menace sur la même ligne
                                        if (Threat.X == MyKing.X
                                            && Threat.X == Coord.X
                                            && ((Coord.Y > MyKing.Y && Coord.Y < Threat.Y) || (Coord.Y < MyKing.Y && Coord.Y > Threat.Y)))
                                        {
                                            PiecesCoordinatesToProtectMyKing.Add(new Coordinates(x, y));
                                            MatePossibleMoves.Add(new Coordinates(Coord.X, Coord.Y));
                                        }

                                        //Si une pièce peut s'interposer entre le roi et la menace sur la même colonne
                                        if (Threat.Y == MyKing.Y
                                            && Threat.Y == Coord.Y
                                            && ((Coord.X > MyKing.X && Coord.X < Threat.X) || (Coord.X < MyKing.X && Coord.X > Threat.X)))
                                        {
                                            PiecesCoordinatesToProtectMyKing.Add(new Coordinates(x, y));
                                            MatePossibleMoves.Add(new Coordinates(Coord.X, Coord.Y));
                                        }

                                        //Si une pièce peut s'interposer entre le roi et la menace en diagonale gauche->droite (\)
                                        if (MyKing.X - Threat.X == MyKing.Y - Threat.Y
                                            && MyKing.X - Coord.X == MyKing.Y - Coord.Y
                                            && ((MyKing.X - Coord.X < MyKing.X - Threat.X && MyKing.Y - Coord.Y < MyKing.Y - Threat.Y)
                                                || (MyKing.X - Coord.X > MyKing.X - Threat.X && MyKing.Y - Coord.Y > MyKing.Y - Threat.Y)))
                                        {
                                            PiecesCoordinatesToProtectMyKing.Add(new Coordinates(x, y));
                                            MatePossibleMoves.Add(new Coordinates(Coord.X, Coord.Y));
                                        }

                                        //Si une pièce peut s'interposer entre le roi et la menace en diagonale droite->gauche (/)
                                        if (MyKing.X - Threat.X == Threat.Y - MyKing.Y
                                            && MyKing.X - Coord.X == Coord.Y - MyKing.Y
                                            && ((MyKing.X - Coord.X < MyKing.X - Threat.X && MyKing.Y - Coord.Y > MyKing.Y - Threat.Y)
                                                || (MyKing.X - Coord.X > MyKing.X - Threat.X && MyKing.Y - Coord.Y < MyKing.Y - Threat.Y)))
                                        {
                                            PiecesCoordinatesToProtectMyKing.Add(new Coordinates(x, y));
                                            MatePossibleMoves.Add(new Coordinates(Coord.X, Coord.Y));
                                        }
                                    }
                                    //Si le roi peut bouger et sortir de l'échec
                                    if ((x == MyKing.X && y == MyKing.Y)
                                        && !VirtualMoveAndMateTest(new Coordinates(MyKing.X, MyKing.Y), new Coordinates(Coord.X, Coord.Y)))
                                    {
                                        PiecesCoordinatesToProtectMyKing.Add(new Coordinates(x, y));
                                        MatePossibleMoves.Add(new Coordinates(Coord.X, Coord.Y));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return new List<Coordinates>[] { PiecesCoordinatesToProtectMyKing, MatePossibleMoves};
        }

        /**
         * IsCheckMate()
         * 
         * Détermine la présence d'un échec et mat
         * Utilise les fonctions de détection d'échec et de mouvements de protection
         * 
         * Si il y a échec et aucun mouvement possible pour protéger le roi, il s'agit d'un échec et mat, déclenchant la fin de la partie
         * 
         * @return bool
         * true si échec et mat
         * false pour poursuivre la partie
         * 
         * @author Axel Floquet-Trillot
         * */
        private bool IsCheckMate()
        {

            if (IsMateDetected() && FindPiecesCoordinatesAndMovesToProtectMyKing()[0].Count == 0)
            {
                Console.WriteLine("Echec et mat !");
                return true;
            }
                

            return false;
        }

        /**
         * PrintBoard()
         * 
         * Affiche le plateau rempli et coloré dans la console
         * La fonction est appelée aussi souvent que l'affichage a besoin d'être mis à jour
         * 
         * @author Axel Floquet-Trillot
         * */
        public void PrintBoard()
        {
            Console.Clear();

            //Chaque case est représentée sur 4 lignes
            int CaseLineNumber = 4;

            Console.WriteLine();
            WhoIsPlaying();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("   1     2     3     4     5     6     7     8");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();

            for (int x = 0; x < GameBoard.GetLength(0); x++)
            {
                for (int y = 1; y < CaseLineNumber; y++)
                {
                    for (int k = 0; k < GameBoard.GetLength(1) + 1; k++)
                    {
                        if ((k % 2 == 0 && x % 2 == 0) || (k % 2 != 0 && x % 2 != 0))
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }

                        if (k < GameBoard.GetLength(1) && GameBoard[x, k] != null && y == 2)
                            Console.Write(GameBoard[x, k].ToString());
                        else if (k == GameBoard.GetLength(1) && y == 2)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("  " + (x + 1));
                        }
                        else if (k != GameBoard.GetLength(1))
                            Console.Write(SpacingCase);
                    }
                    
                    if (y != 0)
                        Console.WriteLine();
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }

        /**
         * AskDigitBetween1And8Minus1(RowOrColumn)
         * 
         * Demande au joueur un chiffre entre 1 et 8 grâce à un ReadKey traité
         * La lettre C (en majuscule et en minuscule) peut être utilisée pour annuler l'entrée d'une paire de coordonnées
         * 
         * @return int
         * La valeur renvoyée est  le chiffre entré diminué de 1 pour correspondre à l'index du tableau GameBoard
         * La valeur renvoyée est -2 en cas d'annulation avec c ou C
         * 
         * @author Axel Floquet-Trillot
         * */
        private int AskDigitBetween1And8Minus1(ReadWay RowOrColumn)
        {
            ConsoleKeyInfo AskDigit;
            int Digit;
            bool ExecutedOnce = false;
            
            //Boucle exécutée tant qu'une valeur incorrecte est entrée (autre qu'entre 1 et 8 ou c/C)
            do
            {
                if (ExecutedOnce)
                    Console.WriteLine("Non valide, recommencez");

                //Affiche le message correspondant à la demande d'une colonne ou d'une ligne
                if (RowOrColumn == ReadWay.Row)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Ligne (de 1 à 8) : ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (RowOrColumn == ReadWay.Column)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Colonne (de 1 à 8) : ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                AskDigit = Console.ReadKey();

                //Parse et vérification de l'entrée. Digit = -1 en cas d'entrée erronée
                if (char.IsDigit(AskDigit.KeyChar))
                {
                    Digit = int.Parse(AskDigit.KeyChar.ToString()) - 1;
                    if (Digit > 7 || Digit < 0)
                        Digit = -1;
                }
                else if (char.IsLetter(AskDigit.KeyChar) && (char.Parse(AskDigit.KeyChar.ToString()).Equals('c') || char.Parse(AskDigit.KeyChar.ToString()).Equals('C')))
                    Digit = -2;
                else
                    Digit = -1;
                
                Console.Write("    ");

                ExecutedOnce = true;
            } while (Digit == -1);

            Console.WriteLine();

            return Digit;
        }

        /**
         * PrintMessageCorrespondingToStatus(Status, AvailableMovesToDisplay)
         * 
         * Affiche en fonction du statut donné en argument le message adapté
         * Les mouvements proposés sont affichés à nouveau si besoin est
         * 
         * @author Axel Floquet-Trillot
         * */
        private void PrintMessageCorrespondingToStatus(CaseStatus Status, List<Coordinates> AvailableMovesToDisplay)
        {
            PrintBoard();
            if (AvailableMovesToDisplay != null && AvailableMovesToDisplay.Count() > 0)
                    PrintPossibleMoves(AvailableMovesToDisplay);

            switch (Status)
            {
                case CaseStatus.Empty:
                    Console.WriteLine("Cette case est vide, entrez les coordonnées d'une de vos pièces : ");
                    break;

                case CaseStatus.Enemy:
                    Console.WriteLine("C'est une pièce ennemie, impossible de la choisir");
                    Console.WriteLine("Entrez les coordonnées d'une de vos pièces : ");
                    break;

                case CaseStatus.Mate:
                    Console.WriteLine("Impossible de protéger votre roi avec cette pièce");
                    Console.WriteLine("Entrez les coordonnées d'une autre de vos pièces : ");
                    break;

                case CaseStatus.MateImpossibleMove:
                    Console.WriteLine("Impossible, ce déplacement vous place en échec");
                    Console.WriteLine("Entrez les coordonnées d'une autre de vos pièces : ");
                    break;

                case CaseStatus.Mine:
                    Console.WriteLine("C'est une de vos pièces, impossible de vous y mettre");
                    Console.WriteLine("Entrez les coordonnées de la case où vous voulez déplacer votre pièce : ");
                    break;

                case CaseStatus.ImpossibleMove:
                    Console.WriteLine("Impossible d'atteindre ces coordonnées");
                    Console.WriteLine("Entrez les coordonnées de la case où vous voulez déplacer votre pièce : ");
                    break;

                case CaseStatus.Canceled:
                    Console.WriteLine("Choix annulé, vous pouvez recommencer à choisir des coordonnées");
                    break;

                case CaseStatus.NoMove:
                    Console.WriteLine("Aucun mouvement n'est possible pour cette pièce, veuillez en choisir une autre");
                    break;
            }
        }

        /**
         * AskOneOfMyPiecesCoordinates()
         * 
         * Demande les coordonnées d'une pièce à déplacer au joueur
         * Empêche de choisir une pièce qui ne peut pas être déplacée sans déclencher un échec sur son roi
         * Ne permet de sélectionner que les pièces qui protègent le roi en cas d'échec
         * 
         * @return Coordinates
         * Les coordonnées de la pièce valide pouvant être déplacée sont renvoyées
         * 
         * @author Axel Floquet-Trillot
         * */
        private Coordinates AskOneOfMyPiecesCoordinates()
        {
            bool MateUnavoidable = false;
            bool FutureMateUnavoidable = false;
            int X, Y;
            CaseStatus Result = CaseStatus.BeforeTest;
            List<Coordinates> CoordinatesToProtectMyKing = null;

            if (IsMateDetected() && !IsCheckMate())
            {
                CoordinatesToProtectMyKing = FindPiecesCoordinatesAndMovesToProtectMyKing()[0];

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Echec : vous devez protéger votre roi");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            
            Console.WriteLine("Entrez les coordonnées d'une de vos pièces à déplacer");
            Console.WriteLine("C pour recommmencer la saisie de coordonnée");

            //Demande en boucle des coordonnées tant qu'aucun mouvement n'est possible avec la sélection. Un message est affiché à chaque tour de boucle pour chaque cas
            do
            {
                FutureMateUnavoidable = false;

                if (Result != CaseStatus.BeforeTest)
                    PrintMessageCorrespondingToStatus(Result, null);

                //Demande en boucle des coordonées tant qu'il y a annulation
                do
                {
                    if (Result != CaseStatus.BeforeTest)
                        PrintMessageCorrespondingToStatus(Result, null);

                    X = AskDigitBetween1And8Minus1(ReadWay.Row);
                    Y = AskDigitBetween1And8Minus1(ReadWay.Column);
                        
                    Result = CaseStatus.Canceled;

                } while (X == -2 || Y == -2);

                //Empecher de choisir une pièce qui ne protège pas le roi en cas d'échec
                if (IsMateDetected() && !IsCheckMate() && CoordinatesToProtectMyKing.Count > 0)
                {
                    foreach (Coordinates Coord in CoordinatesToProtectMyKing)
                    {
                        if (X == Coord.X && Y == Coord.Y)
                        {
                            MateUnavoidable = false;
                            break;
                        } else
                            MateUnavoidable = true;
                    }
                }

                //Empecher de choisir une pièce ne pouvant pas bouger sans redéclencher un échec
                if (GameBoard[X, Y] != null)
                {
                    foreach (Coordinates Coord in GameBoard[X, Y].GetPossibleMoves(GameBoard, new Coordinates(X, Y)))
                        if (!IsMateDetected() && VirtualMoveAndMateTest(new Coordinates(X, Y), Coord))
                        {
                            FutureMateUnavoidable = true;
                            Result = CaseStatus.MateImpossibleMove;
                        }
                }

                //Déterminer le message affiché pour le prochain tour de boucle
                if (GameBoard[X, Y] == null)
                    Result = CaseStatus.Empty;
                else if (GameBoard[X, Y].PieceColor != PlayingNow)
                    Result = CaseStatus.Enemy;
                else if (MateUnavoidable)
                    Result = CaseStatus.Mate;
            } while (FutureMateUnavoidable || MateUnavoidable || GameBoard[X, Y] == null || GameBoard[X, Y].PieceColor != PlayingNow);
            
            PrintBoard();

            Console.WriteLine("Vous avez choisi : " + GameBoard[X, Y].ToString());

            return new Coordinates(X, Y);
        }

        /**
         * AskValidDestination(Origin, AllowedDestinations)
         * 
         * Récupère une destination valide pour la pièce choisie
         * 
         * @return Coordinates
         * Renvoie les coordonnées de la destination choisie
         * 
         * @author Axel Floquet-Trillot
         * */
        private Coordinates AskValidDestination(Coordinates Origin, List<Coordinates> AllowedDestinations)
        {
            int X, Y;
            CaseStatus Result = CaseStatus.BeforeTest;

            Console.WriteLine("Entrez les coordonnées de la case où vous voulez déplacer votre pièce");

            //Demande en boucle tant que le déplacement n'est pas autorisé, met en échec, ou se fait sur une de ses pièces. Un message est affiché à chaque tour
            do
            {

                if (Result != CaseStatus.BeforeTest)
                    PrintMessageCorrespondingToStatus(Result, AllowedDestinations);

                //Demande en boucle tant que la demande est annulée
                do
                {
                    if (Result != CaseStatus.BeforeTest)
                        PrintMessageCorrespondingToStatus(Result, AllowedDestinations);

                    X = AskDigitBetween1And8Minus1(ReadWay.Row);
                    Y = AskDigitBetween1And8Minus1(ReadWay.Column);

                    Result = CaseStatus.Canceled;

                } while (X == -2 || Y == -2);

                if (GameBoard[X, Y] != null && GameBoard[X, Y].PieceColor == PlayingNow)
                    Result = CaseStatus.Mine;
                else if (IsNotAllowedDestination(new Coordinates(X, Y), AllowedDestinations))
                    Result = CaseStatus.ImpossibleMove;
                else if (VirtualMoveAndMateTest(Origin, new Coordinates(X, Y)))
                    Result = CaseStatus.MateImpossibleMove;
                
            } while (VirtualMoveAndMateTest(Origin, new Coordinates(X, Y)) || IsNotAllowedDestination(new Coordinates(X,Y), AllowedDestinations) || (GameBoard[X, Y] != null && GameBoard[X, Y].PieceColor == PlayingNow));
            

            return new Coordinates(X, Y);
        }

        /**
         * IsNotAllowedDestination(AskedDestination, listCoords)
         * 
         * Vérifie si le déplacement proposé est possible en fonction de la liste de déplacement prévus pour la pièce choisie
         * 
         * @return bool
         * false si autorisé
         * true si non autorisé
         * 
         * @author Axel Floquet-Trillot
         * */
        private bool IsNotAllowedDestination(Coordinates AskedDestination, List<Coordinates> listCoords)
        {
            foreach(Coordinates Coord in listCoords)
            {
                if (AskedDestination.X == Coord.X && AskedDestination.Y == Coord.Y)
                    return false;
            }

            return true;
        }

        /**
         * MovePiece(Origin, Destination)
         * 
         * Effectue le déplacement d'une pièce sur le plateau
         * Enregistre les pièces mangées dans les deux listes prévues à cet effet 
         * 
         * @author Axel Floquet-Trillot
         * */
        private void MovePiece(Coordinates Origin, Coordinates Destination)
        {
            if (GameBoard[Destination.X, Destination.Y] != null && GameBoard[Destination.X, Destination.Y].PieceColor != PlayingNow)
            {
                if (PlayingNow == Piece.Color.White)
                    EatenByWhite.Add(GameBoard[Destination.X, Destination.Y]);
                else
                    EatenByBlack.Add(GameBoard[Destination.X, Destination.Y]);
            }

            if (!GameBoard[Origin.X, Origin.Y].HasMoved) GameBoard[Origin.X, Origin.Y].HasMoved = true;

            GameBoard[Destination.X, Destination.Y] = GameBoard[Origin.X, Origin.Y];
            GameBoard[Origin.X, Origin.Y] = null;

            PrintBoard();
        }

        /**
         * MakeCopyOfGameBoard()
         * 
         * Créé une copie du GameBoard pour prévoir les conséquences d'un déplacement sans impacter le GameBoard du jeu
         * 
         * @return Piece[,]
         * Renvoie une copie de plateau
         * 
         * @author Axel Floquet-Trillot
         * */
        private Piece[,] MakeCopyOfGameBoard()
        {
            Piece[,] GameBoardCopy = new Piece[8, 8];
            for (int x = 0; x < GameBoardCopy.GetLength(0); x++)
            {
                for (int y = 0; y < GameBoardCopy.GetLength(1); y++)
                {
                    GameBoardCopy[x, y] = GameBoard[x, y];
                }
            }

            return GameBoardCopy;
        }

        /**
         * VirtualMoveAndMateTest(Origin, Destinations)
         * 
         * Effectue un mouvement fictif et détecte si ce mouvement provoque un échec, sans impacter le GameBoard principal
         * 
         * @return bool
         * true si échec détecté
         * false si mouvement sans échec provoqué
         * 
         * @author Axel Floquet-Trillot
         * */
        private bool VirtualMoveAndMateTest(Coordinates Origin, Coordinates Destination)
        {
            bool MateDetected = false;

            //Mouvement demandé effetcué sur une copie de l'échiquier
            Piece [,] GameBoardCopy = MakeCopyOfGameBoard();

            GameBoardCopy[Destination.X, Destination.Y] = GameBoardCopy[Origin.X, Origin.Y];
            GameBoardCopy[Origin.X, Origin.Y] = null;


            Coordinates WhiteKing = King.locateKing(GameBoardCopy, Piece.Color.White);
            Coordinates BlackKing = King.locateKing(GameBoardCopy, Piece.Color.Black);

            //Exception levée si un des rois n'est pas trouvé
            if (WhiteKing == null || BlackKing == null)
                throw (new Exception("Un des rois n'a pas pu être trouvé"));

            for (int x = 0; x < GameBoardCopy.GetLength(0); x++)
            {
                for (int y = 0; y < GameBoardCopy.GetLength(1); y++)
                {
                    if (GameBoardCopy[x, y] != null)
                    {
                        //Détection des pièces pouvant manger le roi avec le mouvement demandé
                        List<Coordinates> PossibleMovesOfCurrentPiece = GameBoardCopy[x, y].GetPossibleMoves(GameBoardCopy, new Coordinates(x, y));

                        foreach (Coordinates Coord in PossibleMovesOfCurrentPiece)
                        {
                            if (PlayingNow == Piece.Color.White && Coord.X == WhiteKing.X && Coord.Y == WhiteKing.Y && GameBoardCopy[x, y].PieceColor == Piece.Color.Black)
                                MateDetected = true;
                            if (PlayingNow == Piece.Color.Black && Coord.X == BlackKing.X && Coord.Y == BlackKing.Y && GameBoardCopy[x, y].PieceColor == Piece.Color.White)
                                MateDetected = true;
                        }
                    }
                }
            }

            return MateDetected;
        }
    }
}
