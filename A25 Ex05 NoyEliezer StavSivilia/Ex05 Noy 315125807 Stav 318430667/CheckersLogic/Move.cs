namespace Ex05.CheckersGame.CheckersLogic
{
    public class Move
    {
        private Position m_From;
        private Position m_To;

        public Move(Position i_From, Position i_To)
        {
            m_From = i_From;
            m_To = i_To;
        }

        public Position From
        {
            get
            {
                return m_From;
            }
            set
            {
                m_From = value;
            }
        }

        public Position To
        {
            get
            {
                return m_To;
            }
            set
            {
                m_To = value;
            }
        }
    }
}
