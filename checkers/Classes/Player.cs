using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers.Classes
{
    public class Player : ICloneable    // Класс, представляющий игрока в шашках
    {
        public string Name { get; private set; } // Имя игрока
        public int Score { get; private set; } // Очки игрока
        public int PawnsLeft { get; private set; }// Количество пешек, оставшихся у игрока
        public int KingsLeft { get; private set; } // Количество дамок, оставшихся у игрока
        public int[] PlayerColors { get; private set; }// Цвета фишек игрока
        public Player(string name, int[] playerColors)// Конструктор для создания нового игрока с заданным именем и цветами фишек
        {
            Name = name;
            PlayerColors = playerColors;
            Score = 0;
            PawnsLeft = 12;// Изначально 12 пешек у каждого игрока
            KingsLeft = 0;// Изначально ни одной дамки
        }
        public Player(string name, int[] playerColors, int pawnsLeft, int kingsLeft)         // Конструктор для создания нового игрока с заданными параметрами
        {
            Name = name;
            PlayerColors = playerColors;
            Score = 0;
            PawnsLeft = 12;
            KingsLeft = 0;
        }
        public Player()   // Конструктор по умолчанию.
        {

        }
        public void CapturePawns() // захват пешки
        {
            PawnsLeft -= 1;
        }
        public void AddKing()       // добавление дамки
        {
            PawnsLeft -= 1;// После превращения пешки в дамку, количество пешек уменьшается
            KingsLeft += 1;
        }
        public void CaptureKing()  // захват дамки.
        {
            KingsLeft -= 1;
        }
        public void IncreaseScore()  // увеличение счета игрока.
        {
            Score += 1;
        }

        public object Clone() // Реализация интерфейса ICloneable для возможности клонирования объекта Player.
        {
            Player cloned = new Player();
            cloned.PawnsLeft = PawnsLeft;
            cloned.KingsLeft = KingsLeft;
            cloned.PlayerColors = PlayerColors;
            cloned.Score = Score;
            return cloned;
        }
    }
}
