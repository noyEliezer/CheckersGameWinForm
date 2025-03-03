using System;

namespace Ex05.CheckersGame.CheckersLogic
{
    public class GameManager
    {
        private Board m_Board;
        private readonly int r_BoardSize;
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private Player m_CurrentPlayer;
        private Player m_LastPlayer;
        private Position? m_LastPosition = null;
        private bool m_IsGameOver;

        public GameManager(string i_Player1Name, string i_Player2Name, int i_BoardSize)
        {
            r_BoardSize = i_BoardSize;
            r_Player1 = new Player(i_Player1Name, ePlayerDirection.Down);
            r_Player2 = new Player(i_Player2Name, ePlayerDirection.Up);
            InitializeNewGame();
        }

        public Board Board
        {
            get
            {
                return m_Board;
            }
            set
            {
                m_Board = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }
            set
            {
                m_CurrentPlayer = value;
            }
        }

        public Player LastPlayer
        {
            get
            {
                return m_LastPlayer;
            }
            set
            {
                m_LastPlayer = value;
            }
        }

        public Position? LastPosition
        {
            get
            {
                return m_LastPosition;
            }
            set
            {
                m_LastPosition = value;
            }
        }

        public bool IsGameOver
        {
            get
            {
                return m_IsGameOver;
            }
            set
            {
                m_IsGameOver = value;
            }
        }

        public Player Player1
        {
            get
            {
                return r_Player1;
            }
        }

        public Player Player2
        {
            get
            {
                return r_Player2;
            }
        }

        public bool IsPieceAtPosition(Position i_Position)
        {
            return Board.GameBoard[i_Position.Row, i_Position.Col] != null;
        }

        public void InitializeNewGame()
        {
            Board = new Board(r_BoardSize, Player1, Player2);
            CurrentPlayer = Player1;
            LastPlayer = null;
            LastPosition = null;
            IsGameOver = false;
        }

        private void handleMoveExecution(Position i_LastPosition, bool i_IsGetsAnotherTurn)
        {
            LastPosition = i_LastPosition;
            LastPlayer = CurrentPlayer;

            if (!i_IsGetsAnotherTurn)
            {
                SwitchPlayer();
                checkGameStatus();
            }
        }

        public bool IsExecuteMove(Move i_Move, out string o_ErrorMessage, out bool o_IsGetsAnotherTurn)
        {
            o_ErrorMessage = null;
            o_IsGetsAnotherTurn = false;
            bool isValidMove = false;

            if (!IsGameOver)
            {
                if (MoveExecutor.ExecuteMove(i_Move, Board, CurrentPlayer, ref o_IsGetsAnotherTurn, ref o_ErrorMessage))
                {
                    handleMoveExecution(i_Move.To, o_IsGetsAnotherTurn);
                    isValidMove = true;
                }
            }

            return isValidMove;
        }

        public void ExecuteComputerMove(out Move o_ComputerMove, out bool o_IsGetsAnotherTurn)
        {
            o_IsGetsAnotherTurn = false;
            o_ComputerMove = null;
            if (!IsGameOver)
            {
                if (MoveExecutor.ExecuteComputerMove(Board, CurrentPlayer, ref o_IsGetsAnotherTurn, ref o_ComputerMove))
                {
                    handleMoveExecution(o_ComputerMove.To, o_IsGetsAnotherTurn);
                }
                else
                {
                    IsGameOver = true;
                }
            }
        }

        internal void SwitchPlayer()
        {
            LastPlayer = CurrentPlayer;
            CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;
        }

        private void checkGameStatus()
        {
            IsGameOver = IsGameOver || !isOnGoingGame(Board, CurrentPlayer, CurrentPlayer == Player1 ? Player2 : Player1);
        }

        private static bool isOnGoingGame(Board i_Board, Player i_CurrentPlayer, Player i_OppositePlayer)
        {
            bool isCurrentPlayerMoves = MoveValidator.GetAllPossibleMoves(i_Board, i_CurrentPlayer).Count > 0;

            return isCurrentPlayerMoves && i_CurrentPlayer.Pieces.Count != 0;
        }

        private static void calculateGameResult(Board i_Board, Player i_Player1, Player i_Player2, out string o_Result)
        {
            bool isPlayer1HasMoves = MoveValidator.GetAllPossibleMoves(i_Board, i_Player1).Count > 0;
            bool isPlayer2HasMoves = MoveValidator.GetAllPossibleMoves(i_Board, i_Player2).Count > 0;

            if (!isPlayer1HasMoves && !isPlayer2HasMoves)
            {
                o_Result = "Tie";
            }
            else if (!isPlayer1HasMoves || i_Player1.Pieces.Count == 0)
            {
                o_Result = i_Player2.PlayerName;
                i_Player2.UpdateScore(calculateScore(i_Player1, i_Player2));
            }
            else if (!isPlayer2HasMoves || i_Player2.Pieces.Count == 0)
            {
                o_Result = i_Player1.PlayerName;
                i_Player1.UpdateScore(calculateScore(i_Player1, i_Player2));
            }
            else
            {
                o_Result = "Ongoing Game";
            }
        }

        private static int calculateScore(Player i_Player1, Player i_Player2)
        {
            return Math.Abs(i_Player1.CalculatePlayerScore() - i_Player2.CalculatePlayerScore());
        }

        public void GetGameResult(out string o_Result)
        {
            calculateGameResult(Board, CurrentPlayer, CurrentPlayer == Player1 ? Player2 : Player1, out o_Result);
        }
    }
}