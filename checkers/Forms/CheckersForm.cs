using checkers.AI;
using checkers.Classes;
using Microsoft.VisualBasic.Devices;
using System.Drawing;
using System.Net.NetworkInformation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace checkers
{
    public partial class CheckersForm : Form // представление формы игры в шашки
    {
        private Board _board; // Объект для представления игровой доски
        private PictureBox[,] _places = new PictureBox[8, 8];  // Двумерный массив PictureBox для представления клеток на доске
        private Point _selectedPieceLocation { get; set; } // Координаты выбранной фигуры
        private Point[] _selectedMoves = new Point[4]; // Массив для хранения выбранных ходов

        private bool _isAITurn = false;         // Флаг, указывающий, чей ход (игрока или искусственного интеллекта)


        public CheckersForm() // Конструктор формы
        {
            InitializeComponent();
            InitializeGameBoard(Setting.FirstMove);
            NewTurn();
        }

        private void InitializeGameBoard(bool FirstMove)   // Инициализация игровой доски
        {
            _board = new Board(FirstMove); // Создание новой доски с указанным параметром первого хода

            int xLoc = 0, yLoc = 0;
            Color[] colors = new Color[] { Color.White, Color.Gray };
            int white = 0;

            for (int x = 0; x < 8; x++)// Перебор строк и столбцов доски
            {
                for (int y = 0; y < 8; y++)
                {
                    _places[x, y] = new PictureBox();   // Инициализация нового PictureBox для каждой позиции на доске
                    _places[x, y].Location = new Point(xLoc, yLoc);
                    _places[x, y].BackColor = colors[white % 2];
                    _places[x, y].AccessibleDescription = "" + x.ToString() + "," + y.ToString();
                    _places[x, y].Size = new Size(75, 75);
                    mainBoard.Controls.Add(_places[x, y]);
                    xLoc += 75;
                    white++;
                    SetPiece(new Point(x, y));  // Установка фигуры на доске
                }
                white++;
                xLoc = 0;
                yLoc += 75;
            }
            PlayerWhiteNameLabel.Text = Setting.Player1Name;
            PlayerBlackNameLabel.Text = Setting.Player2Name;
        }

        private void UpdateBoardUI() // Обновление интерфейса игровой доски
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (_board.IsWin == false) // Обновление фигур на доске, если игра не завершена
                        SetPiece(new Point(x, y));
                    else
                        break;
                }
            }
        }

        private void UpdatePlayerUI() // Обновление информации об игроках
        {
            PlayerBlackScoreLabel.Text = _board.PlayerBlack.Score.ToString(); // Обновление счетчиков очков для игроков
            PlayerWhiteScoreLabel.Text = _board.PlayerWhite.Score.ToString();
            if (_board.IsWin == true) // Проверка завершения игры и отображение соответствующего сообщения
            {
                PlayerWin();
            }

            else if (_board.IsWhiteTurn == true)
            {
                PlayerWhiteTurn.Visible = true; // Переключение видимости текстовых меток в зависимости от хода белых
                Player2Turn.Visible = false;
            }
            else
            {
                PlayerWhiteTurn.Visible = false; // Переключение видимости текстовых меток в зависимости от хода черных
                Player2Turn.Visible = true;
            }
        }

        private void PlayerWin() // Отображение сообщения о победе
        {
            mainBoard.Enabled = false; // Отключение игровой доски
            string name;
            if (_board.PlayerBlack.Score == 12) // Определение победителя и отображение соответствующего сообщения
                name = _board.PlayerBlack.Name;
            else
                name = _board.PlayerWhite.Name;
            Player2Turn.Visible = false;
            PlayerWhiteTurn.Visible = false;
            MessageBox.Show(name + " is win!");
        }
        private void NewTurn() // Новый игровой ход
        {
            UpdatePlayerUI();
            if (Setting.isAiPlay == false)     // человек-человек
            {
                if (_board.IsWhiteTurn)
                    _board.checkAllMoves(2);
                else
                    _board.checkAllMoves(1);
            }
            else    // человек-компьютер
            {
                if (_board.IsWhiteTurn)
                    _board.checkAllMoves(2);
                else
                {
                    _isAITurn = true;
                    AITurn();
                    //_board.changePlayerTurn();
                    _isAITurn = false;
                    _board.checkAllMoves(2);
                    UpdatePlayerUI();
                }
            }
        }

        private void DisplayAvailableMovesForSelectedPiece()         // Отображение доступных ходов для выбранной фигуры

        {
            if (_places[_selectedPieceLocation.X, _selectedPieceLocation.Y].BackColor == Color.Gray)
            {
                foreach (var move in _board.ListMoves)
                {
                    if (move[0] == _selectedPieceLocation)
                    {
                        _places[_selectedPieceLocation.X, _selectedPieceLocation.Y].BackColor = Color.DarkBlue;
                        _places[move[1].X, move[1].Y].AccessibleName = "green";
                        if (Setting.ShowMove == true)
                            _places[move[1].X, move[1].Y].BackColor = Color.Green;
                    }
                }
            }
        }

        private void MouseClickPlace(PictureBox selectedPlace) // Обработчик событий для клика на клетке доски
        {
            selectedPlace.MouseClick += (sender2, e2) => // Проверка первого клика по доске
            {
                if (_isAITurn == false)
                {
                    PictureBox piece = sender2 as PictureBox;
                    if (piece.Image != null)
                    {
                        int[] placeLocation = piece.AccessibleDescription.Split(',').Select(int.Parse).ToArray();// Получение координат выбранной фигуры
                        RemoveGreenPlaceFromBoard(); // Удаление подсветки клеток
                        _selectedPieceLocation = new Point(placeLocation[0], placeLocation[1]); // Установка координат выбранной фигуры
                        DisplayAvailableMovesForSelectedPiece();  // Отображение доступных ходов для выбранной фигуры
                    }
                }
            };

            selectedPlace.MouseClick += (sender3, e3) => // Проверка второго клика по доске
            {
                if (_isAITurn == false)
                {
                    PictureBox piece = sender3 as PictureBox;
                    if (selectedPlace.AccessibleName == "green" || selectedPlace.BackColor == Color.Green)
                    {
                        int[] placeLocation = piece.AccessibleDescription.Split(',').Select(int.Parse).ToArray(); // Получение координат клетки, куда будет произведен ход
                        Point GreenMove = new Point(placeLocation[0], placeLocation[1]);  // Установка координат для перемещения выбранной фигуры
                        MoveSelectedPiece(_selectedPieceLocation, GreenMove); // Выполнение хода выбранной фигурой
                        NewTurn();
                    }
                }
            };
        }

        private void SetPiece(Point piece) // Установка фигуры на доску
        {
            if (_board.Gameboard[piece.X, piece.Y] == 1) // Установка черной шашки
            {
                _places[piece.X, piece.Y].Image = Properties.Resources.blackPiece;
                _places[piece.X, piece.Y].Image.Tag = "black";
            }
            else if (_board.Gameboard[piece.X, piece.Y] == 2) // Установка белой шашки
            {
                _places[piece.X, piece.Y].Image = Properties.Resources.whitePiece;
                _places[piece.X, piece.Y].Image.Tag = "white";
            }
            else if (_board.Gameboard[piece.X, piece.Y] == 3) // Установка черной дамки
            {
                _places[piece.X, piece.Y].Image = Properties.Resources.blackPieceKing;
                _places[piece.X, piece.Y].Image.Tag = "black";
            }
            else if (_board.Gameboard[piece.X, piece.Y] == 4) // Установка белой дамки
            {
                _places[piece.X, piece.Y].Image = Properties.Resources.whitePieceKing;
                _places[piece.X, piece.Y].Image.Tag = "white";
            }
            else if (_places[piece.X, piece.Y].BackColor == Color.Green)  // Удаление зеленой клетки
            {
                _places[piece.X, piece.Y].BackColor = Color.Gray;
                _places[piece.X, piece.Y].AccessibleName = null;
            }
            else
            {
                _places[piece.X, piece.Y].Image = null; // Очистка клетки (удаление фигуры)
            }
            _places[piece.X, piece.Y].SizeMode = PictureBoxSizeMode.CenterImage;
            UpdatePlayerUI();
        }

        private void MoveSelectedPiece(Point selectedPiece, Point move) // Перемещение выбранной фигуры 
        {
            _board.MakeMove(selectedPiece, move);
            UpdateBoardUI();
            RemoveGreenPlaceFromBoard();
        }

        private void RemoveGreenPlaceFromBoard() // Удаление зеленых клеток с доски
        {
            foreach (Point[] move in _board.ListMoves)
            {
                for (int i = 0; i < move.Length; i++)
                {
                    _places[move[i].X, move[i].Y].BackColor = Color.Gray;
                    _places[move[i].X, move[i].Y].AccessibleName = null;
                }
            }
        }

        private void AITurn() // игра ии
        {
            MinMax AI = new MinMax(_board, 3); // Создание экземпляра класса MinMax с текущим состоянием доски и глубиной поиска 3
            AI.Calculate();  // Расчет ИИ лучшего хода
            MoveSelectedPiece(AI.BestMove[0], AI.BestMove[1]); // Выполнение хода, выбранного ИИ
        }

        private void UpdateGameBoard(object sender, EventArgs e) // Сканирование доски для проверки, был ли клик по клетке
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (_board.IsWin == false)
                        MouseClickPlace(_places[x, y]);
                    else
                        break;
                }
            }
        }
    }
}