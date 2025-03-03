using System;
using System.Collections.Generic;

namespace Ex05.CheckersGame.CheckersLogic
{
    public class Player
    {
        private const int k_MaxNameLength = 20;
        public readonly ePlayerDirection r_Direction;
        private readonly string r_PlayerName;
        private readonly List<Piece> r_Pieces;
        private int m_Score;

        public Player(string i_PlayerName, ePlayerDirection i_Direction)
        {
            ValidatePlayerName(i_PlayerName);
            r_PlayerName = i_PlayerName;
            r_Direction = i_Direction;
            r_Pieces = new List<Piece>();
            m_Score = 0;
        }

        public ePlayerDirection Direction
        {
            get
            {
                return r_Direction;
            }
        }

        public string PlayerName
        {
            get
            {
                return r_PlayerName;
            }
        }

        public List <Piece> Pieces
        {
            get
            {
                return r_Pieces;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
            }
        }

        public static void ValidatePlayerName(string i_PlayerName)
        {
            if (string.IsNullOrEmpty(i_PlayerName))
            {
                throw new ArgumentException("Player name cannot be empty.");
            }

            if (i_PlayerName.Length > k_MaxNameLength)
            {
                throw new ArgumentException($"Player name cannot be longer than {k_MaxNameLength} characters.");
            }

            if (i_PlayerName.Contains(" "))
            {
                throw new ArgumentException("Player name cannot contain spaces.");
            }
        }

        internal void InitializePlayerPieces(int i_BoardSize)
        {
            Pieces.Clear();
            int piecesPerPlayer = i_BoardSize == 6 ? 6 : (i_BoardSize == 8 ? 12 : 20);
            int startRow = Direction == ePlayerDirection.Up ? 0 : i_BoardSize / 2 + 1;
            int endRow = Direction == ePlayerDirection.Up ? i_BoardSize / 2 : i_BoardSize;

            for (; startRow < endRow; startRow++)
            {
                for (int col = (startRow % 2 == 0 ? 1 : 0); col < i_BoardSize; col += 2)
                {
                    if (Pieces.Count < piecesPerPlayer)
                    {
                        Pieces.Add(new Piece(Direction, new Position(startRow, col)));
                    }
                }
            }
        }

        internal int CalculatePlayerScore()
        {
            int score = 0;
            foreach (Piece piece in Pieces)
            {
                score += piece.IsKing ? 4 : 1;
            }

            return score;
        }

        internal void UpdateScore(int i_AdditionalScore)
        {
            Score += i_AdditionalScore;
        }
    }
}
