namespace Ex05.CheckersGame.CheckersUI
{
    public static class StartCheckersGame
    {
        internal static void RunCheckersGame()
        {
            FormGameSettings formGameSettings = new FormGameSettings();

            formGameSettings.ShowDialog();
            if (formGameSettings.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                FormCheckersGameBoard formGameBoard = new FormCheckersGameBoard(
                    formGameSettings.GetSelectedBoardSize(),
                    formGameSettings.GetPlayer1Name(),
                    formGameSettings.GetPlayer2Name());

                formGameBoard.ShowDialog();
            }
        }
    }
}
