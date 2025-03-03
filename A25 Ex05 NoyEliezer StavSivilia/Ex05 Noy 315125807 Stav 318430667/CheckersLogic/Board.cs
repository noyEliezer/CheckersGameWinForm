namespace Ex05.CheckersGame.CheckersLogic
{
    public class Board
    {
        private static readonly int[] sr_ValidBoardSizes = { 6, 8, 10 };
        private readonly int r_Size;
        private readonly Piece[,] r_GameBoard;
        private readonly Player r_Player1;
        private readonly Player r_Player2;

        public Board(int i_Size, Player i_Player1, Player i_Player2)
        {
            r_Size = i_Size;
            r_Player1 = i_Player1;
            r_Player2 = i_Player2;
            r_GameBoard = new Piece[r_Size, r_Size];
            initializeBoard(i_Player1, i_Player2);
        }

        public int Size
        {
            get
            {
                return r_Size;
            }
        }

        public Piece[,] GameBoard
        {
            get
            {
                return r_GameBoard;
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

        public static int[] ValidBoardSizes
        {
            get
            {
                return (int[])sr_ValidBoardSizes.Clone();
            }
        }

        private void initializeBoard(Player i_Player1, Player i_Player2)
        {
            i_Player1.InitializePlayerPieces(Size);
            i_Player2.InitializePlayerPieces(Size);
            placePiecesOnBoard(i_Player1);
            placePiecesOnBoard(i_Player2);
        }

        private void placePiecesOnBoard(Player i_Player)
        {
            foreach (Piece piece in i_Player.Pieces)
            {
                GameBoard[piece.Position.Row, piece.Position.Col] = piece;
            }
        }

        internal void MovePiece(Move i_Move)
        {
            Piece currentPiece = GameBoard[i_Move.From.Row, i_Move.From.Col];
            GameBoard[i_Move.To.Row, i_Move.To.Col] = currentPiece;
            GameBoard[i_Move.From.Row, i_Move.From.Col] = null;
            currentPiece.Position = i_Move.To;
        }

        internal void CapturePiece(Move i_Move)
        {
            MovePiece(i_Move);
            int capturedRow = (i_Move.From.Row + i_Move.To.Row) / 2;
            int capturedCol = (i_Move.From.Col + i_Move.To.Col) / 2;
            Piece capturedPiece = GameBoard[capturedRow, capturedCol];
            Player opponent = capturedPiece.Direction == Player1.Direction ? Player1 : Player2;

            opponent.Pieces.Remove(capturedPiece);
            GameBoard[capturedRow, capturedCol] = null;
        }
    }
}
