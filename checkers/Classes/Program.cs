namespace checkers.Classes
{
    internal static class Program
    {
        //Основная точка входа в приложение.
        [STAThread]
        static void Main()
        {

            ApplicationConfiguration.Initialize();  // Инициализация конфигурации приложения.
            SettingsForm FormSetting = new SettingsForm();  // Создание формы для настроек.
            Application.Run(FormSetting);  // Запуск приложения и отображение формы настроек.
            if (FormSetting.isCorrect == true)     // Проверка, были ли введены корректные настройки.
            {
                CheckersForm checkersForm = new CheckersForm(); // Если настройки корректны, создаем форму для игры в шашки.
                checkersForm.ShowDialog();    // Отображение формы для игры в шашки.
            }
        }
    }
}


//ApplicationConfiguration.Initialize(); -инициализация конфигурации приложения.установка параметров перед запуском основного окна.
//SettingsForm FormSetting = new SettingsForm(); -создание объекта формы для настроек.
//Application.Run(FormSetting); -запуск приложения и отображение формы настроек. Эта строка блокирует выполнение программы до тех пор, пока форма настроек не будет закрыта.
//if (FormSetting.isCorrect == true) -проверка, были ли введены корректные настройки в форме настроек.
//Создание и отображение формы для игры в шашки (CheckersForm), если настройки корректны.