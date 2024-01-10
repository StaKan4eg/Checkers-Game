namespace checkers.Classes
{
    public class Board : ICloneable     // Класс, представляющий шахматную доску
    {
        public int[,] Gameboard { get; private set; } // Массив, представляющий состояние игровой доски
        public bool IsWhiteTurn { get; private set; } // Флаг, указывающий на ход белых фигур
        public bool IsJump { get; private set; } // Флаг, указывающий на наличие прыжка
        public bool IsNextJump { get; private set; } // Флаг, указывающий на возможность следующего прыжка
        public bool IsWin { get; private set; }// Флаг, указывающий на победу одного из игроков

        private int[] _whitePieces = new int[2] { 2, 4 };// Инициализация цветов фигур
        private int[] _blackPieces = new int[2] { 1, 3 };
        private static int _boardSize = 8;// Размер доски

        public List<Point[]> ListMoves = new List<Point[]>();         // Массив доступных ходов
        public Player PlayerWhite;  // Игроки.
        public Player PlayerBlack;
        enum Pieces // Перечисление для представления типов фигур на доске
        {
            Empty,
            BlackPawn,
            WhitePawn,
            BlackKing,
            WhiteKing
        }

        public Board(bool isWhiteTurn) // Стандартный коструктор
        {
            PlayerWhite = new Player(Setting.Player1Name, _whitePieces);
            PlayerBlack = new Player(Setting.Player2Name, _blackPieces);

            Gameboard = new int[8, 8] {   // Инициализация стандартной доски.
                { 0,1,0,1,0,1,0,1 },
                { 1,0,1,0,1,0,1,0 },
                { 0,1,0,1,0,1,0,1 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 2,0,2,0,2,0,2,0 },
                { 0,2,0,2,0,2,0,2 },
                { 2,0,2,0,2,0,2,0 }
            };
            IsWhiteTurn = isWhiteTurn;
            IsJump = false;

        }
        // Конструктор для создания доски с определенными параметрами
        public Board(Player playerWhite, Player playerBlack)         // Конструктор для создания глубокой копии объекта
        {
            PlayerWhite = playerWhite;
            PlayerBlack = playerBlack;
        }

        public void changePlayerTurn() // смена хода текущего игрока
        {
            IsWhiteTurn ^= true;
            IsJump = false;
        }

        public bool isPLayerWin(Player player)//  проверка, является ли игрок победителем
        {
            if (player.Score == 12)
                return true;
            else
                return false;
        }

        public void checkAllMoves(int color) // проверка всех возможных ходов для определенного цвета фигур
        {
            int[] playerColors = GetColors(color, false);
            ListMoves.Clear();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (playerColors.Contains(Gameboard[x, y]))
                    {
                        Point[] availableMoves = checkAvailableMoves(new Point(x, y));
                        if (availableMoves[0].X + availableMoves[0].Y > 0)
                        {
                            foreach (Point p in availableMoves)
                            {
                                if (p.X + p.Y > 0)
                                {
                                    Point[] piece = new Point[2];
                                    piece[0] = new Point(x, y);
                                    piece[1] = p;
                                    ListMoves.Add(piece);
                                }
                            }
                        }
                    }
                }
            }
            if (Setting.ForceJump == true)
                checkJump();
        }

        private void checkJump() //проверка наличия прыжка
        {
            List<Point[]> tmpList = new List<Point[]>();
            foreach (Point[] move in ListMoves)
            {
                Point[] tmpMoves = new Point[2];
                if (Math.Pow(move[0].X - move[1].X, 2) > 2)
                {
                    tmpMoves[0] = move[0];
                    tmpMoves[1] = move[1];
                    tmpList.Add(tmpMoves);
                    IsJump = true;
                }
            }
            if (IsJump == true)
                ListMoves = tmpList;
        }

        private void NextJump(Point selectedPiece) // обработка следующего прыжка после выполнения первого
        {
            IsNextJump = false;
            List<Point[]> tmpList = new List<Point[]>();
            Point[] availableMoves = checkAvailableMoves(new Point(selectedPiece.X, selectedPiece.Y));
            foreach (var move in availableMoves)
            {
                if (move.IsEmpty == false && Math.Pow(move.X - selectedPiece.X, 2) > 2)
                {
                    IsNextJump = true;
                    Point[] piece = new Point[2];
                    piece[0] = selectedPiece;
                    piece[1] = move;
                    tmpList.Add(piece);
                }
            }
            if (IsNextJump == true)
                ListMoves = tmpList;
            else
                IsNextJump = false;
        }

        public void MakeMove(Point selectedPiece, Point newPosition) // осуществление хода на доске.
        {
            if (Math.Pow(newPosition.X - selectedPiece.X, 2) > 2) // прыжок
            {
                addScore(Gameboard[(newPosition.X + selectedPiece.X) / 2, (newPosition.Y + selectedPiece.Y) / 2]);
                Gameboard[newPosition.X, newPosition.Y] = Gameboard[selectedPiece.X, selectedPiece.Y];
                Gameboard[selectedPiece.X, selectedPiece.Y] = (int)Pieces.Empty;
                Gameboard[(newPosition.X + selectedPiece.X) / 2, (newPosition.Y + selectedPiece.Y) / 2] = (int)Pieces.Empty;
                NextJump(newPosition);
            }
            else // перемещение
            {
                Gameboard[newPosition.X, newPosition.Y] = Gameboard[selectedPiece.X, selectedPiece.Y];
                Gameboard[selectedPiece.X, selectedPiece.Y] = (int)Pieces.Empty;
            }
            UpgradeToKing(new Point(newPosition.X, newPosition.Y));
            if (IsNextJump == false)
                changePlayerTurn();
        }

        private void UpgradeToKing(Point actualPosition) // проверка на возможность изменения из обычной пешки в короля
        {
            int color = Gameboard[actualPosition.X, actualPosition.Y];
            if (color == (int)Pieces.WhitePawn && actualPosition.X == 0)
            {
                Gameboard[actualPosition.X, actualPosition.Y] = (int)Pieces.WhiteKing;
                PlayerWhite.AddKing();
            }
            else if (color == (int)Pieces.BlackPawn && actualPosition.X == _boardSize - 1)
            {
                Gameboard[actualPosition.X, actualPosition.Y] = (int)Pieces.BlackKing;
                PlayerWhite.AddKing();
            }
        }

        private void addScore(int piece) // добавить очко за съеденую фигуру противника
        {
            if (piece == (int)Pieces.BlackPawn || piece == (int)Pieces.BlackKing)
            {
                if (piece == (int)Pieces.BlackKing)
                    PlayerBlack.CaptureKing();
                else
                    PlayerBlack.CapturePawns();
                PlayerWhite.IncreaseScore();
            }
            else
            {
                if (piece == (int)Pieces.WhiteKing)
                    PlayerWhite.CaptureKing();
                else
                    PlayerWhite.CapturePawns();
                PlayerBlack.IncreaseScore();
            }
            if (PlayerWhite.Score == 12 || PlayerBlack.Score == 12)
                IsWin = true;
        }


        private Point[] addMove(Point[] moves, Point move) // добавление хода в массив ходов
        {
            Point[] tmp = moves;
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i].X + moves[i].Y == 0)
                {
                    moves[i] = move;
                    break;
                }
            }
            return moves;
        }

        private bool IsKing(int color) // проверка, является ли фигура королем
        {
            if (color == (int)Pieces.BlackKing || color == (int)Pieces.WhiteKing)
                return true;
            else
                return false;
        }

        private int[] GetColors(int color, bool isOppnent) //получить цвета игрока или противника
        {
            if (isOppnent == false)
            {
                if (PlayerWhite.PlayerColors.Contains(color))
                    return PlayerWhite.PlayerColors;
                else
                    return PlayerBlack.PlayerColors;
            }
            else
            {
                if (PlayerWhite.PlayerColors.Contains(color))
                    return PlayerBlack.PlayerColors;
                else
                    return PlayerWhite.PlayerColors;
            }
        }

        private bool checkIsMoveOutOfBounds(int X, int Y) //проерка границ доски
        {
            bool result = false;
            if (X < 0 || X > _boardSize - 1) // верх || низ
                result = true;
            if (Y < 0 || Y > _boardSize - 1) // лево || право
                result = true;
            return result;
        }

        private Point[] checkAvailableMoves(Point SelectedPlace)
        {
            int color = Gameboard[SelectedPlace.X, SelectedPlace.Y];        // получение id цвета фигуры;
            bool king = IsKing(color);
            int[] opponent = GetColors(color, true);

            Point[] moves = king == true ? new Point[4] : new Point[2]; // если фигура == король, то ходов = 4, иначе ходов = 2

            if (color == (int)Pieces.WhitePawn || king == true)     // проверить место хода для белой фигуры или королевской фигуры
            {
                if (checkIsMoveOutOfBounds(SelectedPlace.X - 1, SelectedPlace.Y - 1) == false)      // проверка слева вверху место за пределами доски
                {
                    if (Gameboard[SelectedPlace.X - 1, SelectedPlace.Y - 1] == (int)Pieces.Empty)       // проверка на пустую клетку
                        moves = addMove(moves, new Point(SelectedPlace.X - 1, SelectedPlace.Y - 1));
                    else if (opponent.Contains(Gameboard[SelectedPlace.X - 1, SelectedPlace.Y - 1]))       // проверка на налочии в ней противника
                    {
                        if (checkIsMoveOutOfBounds(SelectedPlace.X - 2, SelectedPlace.Y - 2) == false)      // проверка на "прыжок" за пределы доски
                        {
                            if (Gameboard[SelectedPlace.X - 2, SelectedPlace.Y - 2] == (int)Pieces.Empty)       //проверка на наличие пустой клетке для прыжка за фигурой противника
                                moves = addMove(moves, new Point(SelectedPlace.X - 2, SelectedPlace.Y - 2));
                        }
                    }
                }
                if (checkIsMoveOutOfBounds(SelectedPlace.X - 1, SelectedPlace.Y + 1) == false)        // проверяем, находится ли верхний-правый угол за пределами доски
                {
                    if (Gameboard[SelectedPlace.X - 1, SelectedPlace.Y + 1] == (int)Pieces.Empty)       // проверяем, свободен ли верхний-правый угол
                        moves = addMove(moves, new Point(SelectedPlace.X - 1, SelectedPlace.Y + 1));
                    else if (opponent.Contains(Gameboard[SelectedPlace.X - 1, SelectedPlace.Y + 1]))       // проверяем, является ли верхний-правый угол оппонентом
                    {
                        if (checkIsMoveOutOfBounds(SelectedPlace.X - 2, SelectedPlace.Y + 2) == false)      // проверяем, находится ли прыжок верхний-правый за пределами доски
                        {
                            if (Gameboard[SelectedPlace.X - 2, SelectedPlace.Y + 2] == (int)Pieces.Empty)       // проверяем, свободно ли место за оппонентом верхний-правый
                                moves = addMove(moves, new Point(SelectedPlace.X - 2, SelectedPlace.Y + 2));
                        }
                    }
                }
            }
            if (color == (int)Pieces.BlackPawn || king == true)     // проверяем место для черной шашки или короля
            {
                if (checkIsMoveOutOfBounds(SelectedPlace.X + 1, SelectedPlace.Y - 1) == false)      // проверяем, находится ли левый-нижний угол за пределами доски
                {
                    if (Gameboard[SelectedPlace.X + 1, SelectedPlace.Y - 1] == (int)Pieces.Empty)       // проверяем, свободен ли левый-нижний угол
                        moves = addMove(moves, new Point(SelectedPlace.X + 1, SelectedPlace.Y - 1));
                    else if (opponent.Contains(Gameboard[SelectedPlace.X + 1, SelectedPlace.Y - 1]))       // проверяем, является ли левый-нижний угол оппонентом
                    {
                        if (checkIsMoveOutOfBounds(SelectedPlace.X + 2, SelectedPlace.Y - 2) == false)      // проверяем, находится ли прыжок левый-нижний за пределами доски
                        {
                            if (Gameboard[SelectedPlace.X + 2, SelectedPlace.Y - 2] == (int)Pieces.Empty)       // проверяем, свободно ли место за оппонентом левый-нижний
                                moves = addMove(moves, new Point(SelectedPlace.X + 2, SelectedPlace.Y - 2));
                        }
                    }
                }
                if (checkIsMoveOutOfBounds(SelectedPlace.X + 1, SelectedPlace.Y + 1) == false)       // проверяем, находится ли правый-нижний угол за пределами доски
                {
                    if (Gameboard[SelectedPlace.X + 1, SelectedPlace.Y + 1] == (int)Pieces.Empty)       // проверяем, свободен ли правый-нижний угол
                        moves = addMove(moves, new Point(SelectedPlace.X + 1, SelectedPlace.Y + 1));
                    else if (opponent.Contains(Gameboard[SelectedPlace.X + 1, SelectedPlace.Y + 1]))      // проверяем, является ли правый-нижний угол оппонентом
                    {
                        if (checkIsMoveOutOfBounds(SelectedPlace.X + 2, SelectedPlace.Y + 2) == false)      // проверяем, находится ли прыжок правый-нижний за пределами доски
                        {
                            if (Gameboard[SelectedPlace.X + 2, SelectedPlace.Y + 2] == (int)Pieces.Empty)        // проверяем, свободно ли место за оппонентом правый-нижний
                                moves = addMove(moves, new Point(SelectedPlace.X + 2, SelectedPlace.Y + 2));
                        }
                    }
                }
            }
            return moves;
        }

        public object Clone() // создание глубокой копии объекта.
        {
            // Клонирование объектов игроков
            Player playerWhite = (Player)PlayerWhite.Clone();
            Player playerBlack = (Player)PlayerBlack.Clone();
            Board cloned = new Board(playerWhite, playerBlack); // Создание нового клонированного объекта с клонированными игроками
            cloned.Gameboard = new int[8, 8]; // Инициализация нового массива для игровой доски

            for (int i = 0; i < Gameboard.GetLength(0); i++) // Копирование значений из текущей игровой доски в клонированную
            {
                for (int j = 0; j < Gameboard.GetLength(1); j++)
                {
                    cloned.Gameboard[i, j] = Gameboard[i, j];
                }
            }
            return cloned; // Возвращение клонированного объекта
        }
    }
}
