using System;
using System.Drawing;
using System.Windows.Forms;
using Ex05.CheckersGame.CheckersLogic;
using Ex05.Properties;

namespace Ex05.CheckersGame.CheckersUI
{
    public class FormCheckersGameBoard : Form
    {
        private Button[,] m_ButtonsBoard;
        private Label m_LabelPlayer1Score;
        private Label m_LabelPlayer2Score;
        private Image m_ImageRegularPlayer1;
        private Image m_ImageRegularPlayer2;
        private Image m_ImageKingPlayer1;
        private Image m_ImageKingPlayer2;
        private GameManager m_GameManager;
        private Position? m_SelectedPiecePosition = null;

        public FormCheckersGameBoard(int i_BoardSize, string i_Player1Name, string i_Player2Name)
        {
            initializeComponent();
            loadImages();
            initializeFormProperties();
            initializeGameManager(i_BoardSize, i_Player1Name, i_Player2Name);
            initializeScoreLabels();
            initializeGameBoard(i_BoardSize);
            updateBoardDisplay();
        }

        public Button[,] ButtonsBoard
        {
            get
            {
                return m_ButtonsBoard;
            }
            set
            {
                m_ButtonsBoard = value;
            }
        }

        public Label LabelPlayer1Score
        {
            get
            {
                return m_LabelPlayer1Score;
            }
            set
            {
                m_LabelPlayer1Score = value;
            }
        }

        public Label LabelPlayer2Score
        {
            get
            {
                return m_LabelPlayer2Score;
            }
            set
            {
                m_LabelPlayer2Score = value;
            }
        }

        public Image ImageRegularPlayer1
        {
            get
            {
                return m_ImageRegularPlayer1;
            }
            set
            {
                m_ImageRegularPlayer1 = value;
            }
        }

        public Image ImageRegularPlayer2
        {
            get
            {
                return m_ImageRegularPlayer2;
            }
            set
            {
                m_ImageRegularPlayer2 = value;
            }
        }

        public Image ImageKingPlayer1
        {
            get
            {
                return m_ImageKingPlayer1;
            }
            set
            {
                m_ImageKingPlayer1 = value;
            }
        }

        public Image ImageKingPlayer2
        {
            get
            {
                return m_ImageKingPlayer2;
            }
            set
            {
                m_ImageKingPlayer2 = value;
            }
        }

        public GameManager GameManager
        {
            get
            {
                return m_GameManager;
            }
            set
            {
                m_GameManager = value;
            }
        }

        public Position? SelectedPiecePosition
        {
            get
            {
                return m_SelectedPiecePosition;
            }
            set
            {
                m_SelectedPiecePosition = value;
            }
        }

        private void initializeFormProperties()
        {
            this.Text = "Damka";
            this.Icon = Resources.CheckersIcon;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.WhiteSmoke;
        }

        private void initializeComponent() { }

        private void loadImages()
        {
            try
            {
                ImageRegularPlayer1 = Resources.RegularPlayer1Image;
                ImageRegularPlayer2 = Resources.RegularPlayer2Image;
                ImageKingPlayer1 = Resources.KingPlayer1Image;
                ImageKingPlayer2 = Resources.KingPlayer2Image;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading game images: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void initializeGameManager(int i_BoardSize, string i_Player1Name, string i_Player2Name)
        {
            GameManager = new GameManager(i_Player1Name, i_Player2Name, i_BoardSize);
        }

        private void initializeScoreLabels()
        {
            int boardWidth = (GameManager.Board.Size * 70) + (50 * 2);
            int oneThirdWidth = boardWidth / 3;

            initializePlayerLabel(ref m_LabelPlayer1Score, GameManager.Player1.PlayerName, new Point(oneThirdWidth, 20), true);
            initializePlayerLabel(ref m_LabelPlayer2Score, GameManager.Player2.PlayerName, new Point(2 * oneThirdWidth, 20), false);
        }

        private void initializePlayerLabel(ref Label io_Label, string i_PlayerName, Point i_Location, bool i_IsPlayer1)
        {
            io_Label = new Label();
            io_Label.Text = $"{i_PlayerName}: 0";
            io_Label.AutoSize = true;
            io_Label.Font = new Font("Arial", 14, FontStyle.Bold);
            io_Label.ForeColor = i_IsPlayer1 ? Color.DeepPink : Color.Black;
            int centeredX = i_Location.X - (io_Label.Text.Length * 10 / 2);

            io_Label.Location = new Point(centeredX, i_Location.Y);
            this.Controls.Add(io_Label);
        }

        private void initializeGameBoard(int i_BoardSize)
        {
            ButtonsBoard = new Button[i_BoardSize, i_BoardSize];
            initializeButtons(i_BoardSize, 70, 60, 50);
            initializeFormSize(i_BoardSize, 70, 60, 50);
        }

        private void initializeButtons(int i_BoardSize, int i_ButtonSize, int i_TopMargin, int i_SideMargin)
        {
            for (int row = 0; row < i_BoardSize; row++)
            {
                for (int col = 0; col < i_BoardSize; col++)
                {
                    initializeSingleButton(row, col, i_ButtonSize, i_TopMargin, i_SideMargin);
                }
            }
        }

        private void initializeSingleButton(int i_Row, int i_Col, int i_ButtonSize, int i_TopMargin, int i_SideMargin)
        {
            Button button = new Button();

            button.Size = new Size(i_ButtonSize, i_ButtonSize);
            button.Location = new Point(i_Col * i_ButtonSize + i_SideMargin, i_Row * i_ButtonSize + i_TopMargin);
            button.BackColor = (i_Row + i_Col) % 2 == 0 ? Color.White : Color.Black;
            button.Enabled = (i_Row + i_Col) % 2 != 0;
            button.FlatStyle = FlatStyle.Flat;
            button.ForeColor = Color.White;
            button.Click += button_Click;
            button.MouseEnter += button_MouseEnter;
            button.MouseLeave += button_MouseLeave;
            ButtonsBoard[i_Row, i_Col] = button;
            this.Controls.Add(button);
        }

        private void initializeFormSize(int i_BoardSize, int i_ButtonSize, int i_TopMargin, int i_SideMargin)
        {
            this.ClientSize = new Size((i_BoardSize * i_ButtonSize) + (i_SideMargin * 2),
                                       (i_BoardSize * i_ButtonSize) + i_TopMargin + i_SideMargin);
        }

        private void button_MouseEnter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Position position = getButtonPosition(button);

            if (isButtonHighlightable(button, position))
            {
                button.BackColor = Color.LightBlue;
            }
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Position position = getButtonPosition(button);

            if (isButtonHighlightable(button, position))
            {
                button.BackColor = (position.Row + position.Col) % 2 == 0 ? Color.White : Color.Black;
            }
        }

        private bool isButtonHighlightable(Button i_Button, Position i_Position)
        {
            return i_Button.Enabled && i_Button.BackgroundImage != null &&
                   (!SelectedPiecePosition.HasValue ||
                   !isClickedSamePosition(i_Position));
        }

        private Position getButtonPosition(Button i_Button)
        {
            Position position = new Position(-1, -1);

            for (int row = 0; row < GameManager.Board.Size; row++)
            {
                for (int col = 0; col < GameManager.Board.Size; col++)
                {
                    if (ButtonsBoard[row, col] == i_Button)
                    {
                        position = new Position(row, col);
                        break;
                    }
                }
            }

            return position;
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (!GameManager.IsGameOver)
            {
                Button clickedButton = (Button)sender;
                Position clickedPosition = getButtonPosition(clickedButton);

                if (SelectedPiecePosition.HasValue)
                {
                    handleSelectedPieceMove(clickedButton, clickedPosition);
                }
                else
                {
                    handlePieceSelection(clickedButton, clickedPosition);
                }
            }
        }

        private void handleSelectedPieceMove(Button i_ClickedButton, Position i_ClickedPosition)
        {
            if (isClickedSamePosition(i_ClickedPosition))
            {
                cancelSelection(i_ClickedButton);
            }
            else
            {
                executePlayerMove(i_ClickedPosition);
            }
        }

        private bool isClickedSamePosition(Position i_ClickedPosition)
        {
            return SelectedPiecePosition.Value.Equals(i_ClickedPosition);
        }

        private void cancelSelection(Button i_Button)
        {
            SelectedPiecePosition = null;
            i_Button.BackColor = Color.White;
        }

        private void executePlayerMove(Position i_ClickedPosition)
        {
            Move move = new Move(SelectedPiecePosition.Value, i_ClickedPosition);
            bool isGetsAnotherTurn = false;
            string message = string.Empty;

            if (GameManager.IsExecuteMove(move, out message, out isGetsAnotherTurn))
            {
                handleSuccessfulMove(isGetsAnotherTurn);
            }
            else
            {
                MessageBox.Show(message, "Invalid Move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            resetMoveState();
        }

        private void handleSuccessfulMove(bool i_IsGetsAnotherTurn)
        {
            updateBoardDisplay();
            updateTurnDisplay(GameManager.CurrentPlayer.PlayerName);
            if (i_IsGetsAnotherTurn)
            {
                MessageBox.Show("You get another turn!", "Extra Turn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                checkGameOver();
                if (!GameManager.IsGameOver && isComputerTurn())
                {
                    executeComputerTurn();
                }
            }
        }
       
        private bool isComputerTurn()
        {
            return GameManager.CurrentPlayer == GameManager.Player2 && GameManager.Player2.PlayerName == "Computer";
        }

        private void executeComputerTurn()
        {
            if (!GameManager.IsGameOver)
            {
                Move computerMove;
                bool isComputerGetAnotherTurn;

                GameManager.ExecuteComputerMove(out computerMove, out isComputerGetAnotherTurn);
                updateBoardDisplay();
                updateTurnDisplay(GameManager.CurrentPlayer.PlayerName);
                if (isComputerGetAnotherTurn)
                {
                    executeComputerTurn();
                }
                else
                {
                    checkGameOver();
                }
            }
        }

        private void resetMoveState()
        {
            SelectedPiecePosition = null;
            clearSelection();
        }

        private void handlePieceSelection(Button i_ClickedButton, Position i_ClickedPosition)
        {
            if (GameManager.IsPieceAtPosition(i_ClickedPosition))
            {
                selectPiece(i_ClickedButton, i_ClickedPosition);
            }
        }

        private void selectPiece(Button i_Button, Position i_Position)
        {
            SelectedPiecePosition = i_Position;
            i_Button.BackColor = Color.LightBlue;
        }

        private void updateTurnDisplay(string i_PlayerName)
        {
            LabelPlayer1Score.ForeColor = Color.Black;
            LabelPlayer2Score.ForeColor = Color.Black;
            if (i_PlayerName == GameManager.Player1.PlayerName)
            {
                LabelPlayer1Score.ForeColor = Color.DeepPink;
            }
            else
            {
                LabelPlayer2Score.ForeColor = Color.DeepPink;
            }
        }

        private void updateBoardDisplay()
        {
            for (int row = 0; row < GameManager.Board.Size; row++)
            {
                for (int col = 0; col < GameManager.Board.Size; col++)
                {
                    Button button = ButtonsBoard[row, col];
                    Piece piece = GameManager.Board.GameBoard[row, col];

                    resetButtonState(button, row, col);
                    if (piece != null)
                    {
                        setPieceDisplay(button, piece);
                    }
                }
            }
        }

        private void resetButtonState(Button i_Button, int i_Row, int i_Col)
        {
            i_Button.Image = null;
            i_Button.BackgroundImage = null;
            i_Button.Text = string.Empty;
            i_Button.BackColor = (i_Row + i_Col) % 2 == 0 ? Color.White : Color.Black;
        }

        private void setPieceDisplay(Button i_Button, Piece i_Piece)
        {
            i_Button.BackgroundImage = getPieceImage(i_Piece.GetPieceType());
            i_Button.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private Image getPieceImage(ePieceType i_PieceType)
        {
            Image result = null;

            switch (i_PieceType)
            {
                case ePieceType.RegularPlayer1:
                    result = ImageRegularPlayer1;
                    break;
                case ePieceType.KingPlayer1:
                    result = ImageKingPlayer1;
                    break;
                case ePieceType.RegularPlayer2:
                    result = ImageRegularPlayer2;
                    break;
                case ePieceType.KingPlayer2:
                    result = ImageKingPlayer2;
                    break;
            }

            return result;
        }

        private void checkGameOver()
        {
            if (GameManager.IsGameOver)
            {
                string result;

                GameManager.GetGameResult(out result);
                string gameOverMessage = result == "Tie" ? "Tie!" : $"{result} Won!";
                LabelPlayer1Score.Text = $"{GameManager.Player1.PlayerName}: {GameManager.Player1.Score}";
                LabelPlayer2Score.Text = $"{GameManager.Player2.PlayerName}: {GameManager.Player2.Score}";
                DialogResult playAgain = MessageBox.Show($"{gameOverMessage}{Environment.NewLine}Another Round?", "Damka",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                handlePlayAgainResponse(playAgain);
            }
        }

        private void handlePlayAgainResponse(DialogResult i_PlayAgain)
        {
            if (i_PlayAgain == DialogResult.Yes)
            {
                GameManager.InitializeNewGame();
                updateBoardDisplay();
                updateTurnDisplay(GameManager.Player1.PlayerName);
            }
            else
            {
                this.Close();
            }
        }

        private void clearSelection()
        {
            for (int row = 0; row < GameManager.Board.Size; row++)
            {
                for (int col = 0; col < GameManager.Board.Size; col++)
                {
                    if ((row + col) % 2 != 0)
                    {
                        ButtonsBoard[row, col].BackColor = Color.Black;
                    }
                }
            }
        }
    }
}