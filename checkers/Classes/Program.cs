namespace checkers.Classes
{
    internal static class Program
    {
        //�������� ����� ����� � ����������.
        [STAThread]
        static void Main()
        {

            ApplicationConfiguration.Initialize();  // ������������� ������������ ����������.
            SettingsForm FormSetting = new SettingsForm();  // �������� ����� ��� ��������.
            Application.Run(FormSetting);  // ������ ���������� � ����������� ����� ��������.
            if (FormSetting.isCorrect == true)     // ��������, ���� �� ������� ���������� ���������.
            {
                CheckersForm checkersForm = new CheckersForm(); // ���� ��������� ���������, ������� ����� ��� ���� � �����.
                checkersForm.ShowDialog();    // ����������� ����� ��� ���� � �����.
            }
        }
    }
}


//ApplicationConfiguration.Initialize(); -������������� ������������ ����������.��������� ���������� ����� �������� ��������� ����.
//SettingsForm FormSetting = new SettingsForm(); -�������� ������� ����� ��� ��������.
//Application.Run(FormSetting); -������ ���������� � ����������� ����� ��������. ��� ������ ��������� ���������� ��������� �� ��� ���, ���� ����� �������� �� ����� �������.
//if (FormSetting.isCorrect == true) -��������, ���� �� ������� ���������� ��������� � ����� ��������.
//�������� � ����������� ����� ��� ���� � ����� (CheckersForm), ���� ��������� ���������.