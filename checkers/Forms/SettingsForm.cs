// Подключение необходимых пространств имен
using checkers.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace checkers
{
    // Класс SettingsForm для управления настройками игры в форме Windows
    public partial class SettingsForm : Form
    {
        // Свойство для определения корректности настроек
        public bool isCorrect { get; set; }

        // Конструктор для инициализации формы и установки значений по умолчанию для комбо-боксов
        public SettingsForm()
        {
            InitializeComponent();
            comboBoxBoardSizePvP.SelectedIndex = 0;
            comboBoxFirstMovePvP.SelectedIndex = 0;
            comboBoxBoardSizePvE.SelectedIndex = 0;
            comboBoxFirstMovePvE.SelectedIndex = 0;
        }

        // Обработчик события для начала игры "Игрок против Игрока"
        private void ButtonStartPvP(object sender, EventArgs e)
        {
            // Проверка корректности настроек перед началом игры
            if (CheckSettingsPvP() == true)
            {
                // Присвоение выбранных настроек свойствам класса Setting
                Setting.BoardSize = comboBoxBoardSizePvP.Text;
                Setting.ForceJump = checkBoxForceJumpPvP.Checked;
                Setting.ShowMove = checkBoxShowMovesPvP.Checked;
                Setting.FirstMove = WhichTurn(comboBoxFirstMovePvP.Text);
                Setting.Player1Name = textBoxPlayer1PvP.Text;
                Setting.Player2Name = textBoxPlayer2PvP.Text;
                Setting.isAiPlay = false;
                // Закрытие формы настроек
                this.Close();
            }
        }

        // Обработчик события для начала игры "Игрок против ИИ"
        private void ButtonStartPvE(object sender, EventArgs e)
        {
            // Проверка корректности настроек перед началом игры
            if (CheckSettingsPvE() == true)
            {
                // Присвоение выбранных настроек свойствам класса Setting
                Setting.BoardSize = comboBoxBoardSizePvE.Text;
                Setting.ForceJump = checkBoxForceJumpPvE.Checked;
                Setting.ShowMove = checkBoxShowMovesPvE.Checked;
                Setting.FirstMove = WhichTurn(comboBoxFirstMovePvE.Text);
                Setting.Player1Name = textBoxPlayer1PvE.Text;
                Setting.Player2Name = "Компьютер";
                Setting.isAiPlay = true;
                // Закрытие формы настроек
                this.Close();
            }
        }

        // Метод для проверки настроек игры "Игрок против Игрока"
        private bool CheckSettingsPvP()
        {
            isCorrect = false;
            if (CheckPlayerName(textBoxPlayer1PvP, 1, true) == true)
                isCorrect = true;
            else
                return isCorrect;
            isCorrect = false;
            if (CheckPlayerName(textBoxPlayer2PvP, 2, true) == true)
                isCorrect = true;
            else
                return isCorrect;
            return isCorrect;
        }

        // Метод для проверки настроек игры "Игрок против ИИ"
        private bool CheckSettingsPvE()
        {
            isCorrect = false;
            if (CheckPlayerName(textBoxPlayer1PvP, 1, false) == true)
                isCorrect = true;
            return isCorrect;
        }

        // Метод для проверки имени игрока
        private bool CheckPlayerName(TextBox Player, int Number, bool isPvP)
        {
            if (Player.Text == "")
            {
                if (isPvP == true)
                {
                    MessageBox.Show("Имя Игрока " + Number + " не заполнено!");
                    return false;
                }
                else
                {
                    MessageBox.Show("Имя Игрока " + Number + " не заполнено!");
                    return false;
                }
            }
            else if (Player.Text.Length > 10)
            {
                if (isPvP == true)
                {
                    MessageBox.Show("Имя Игрока " + Number + " слишком длинное!\n");
                    return false;
                }
                else
                {
                    MessageBox.Show("Имя Игрока " + Number + " слишком длинное!\n");
                    return false;
                }
            }
            return true;
        }

        // Метод для определения первого хода
        private bool WhichTurn(string firstStart)
        {
            if (firstStart == "Игрок 1" || firstStart == "Игрок")
                return true;
            else
                return false;
        }
    }
}
