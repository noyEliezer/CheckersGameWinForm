namespace Ex05.CheckersGame.CheckersLogic
{
    public class Piece
    {
        private readonly ePlayerDirection r_Direction;
        private Position m_Position;
        private bool m_IsKing;

        public Piece(ePlayerDirection i_Direction, Position i_CurrentPosition)
        {
            m_IsKing = false;
            r_Direction = i_Direction;
            m_Position = i_CurrentPosition;
        }

        public ePlayerDirection Direction
        {
            get
            {
                return r_Direction;
            }
        }

        public Position Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        public bool IsKing
        {
            get
            {
                return m_IsKing;
            }
            set
            {
                m_IsKing = value;
            }
        }

        public ePieceType GetPieceType()
        {
            return Direction == ePlayerDirection.Up ?
                  (IsKing ? ePieceType.KingPlayer1 : ePieceType.RegularPlayer1) :
                  (IsKing ? ePieceType.KingPlayer2 : ePieceType.RegularPlayer2);
        }
    }
}