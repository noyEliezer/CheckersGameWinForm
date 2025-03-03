using System;
using System.Collections.Generic;

namespace Ex05.CheckersGame.CheckersLogic
{
    public static class MoveValidator
    {
        private const bool k_IsCapture = true;
        private const int k_RegularMoveDistance = 1;
        private const int k_CaptureMoveDistance = 2;

        internal static List <Move> GetAllPossibleMoves(Board i_Board, Player i_Player)
        {
            List <Move> moves = new List<Move>();
            bool isCaptureMoveAvaliable = IsAnyCaptureMoves(i_Board, i_Player);

            for (int row = 0; row < i_Board.Size; row++)
            {
                for (int col = 0; col < i_Board.Size; col++)
                {
                    Position from = new Position(row, col);

                    if (i_Player.Pieces.Contains(i_Board.GameBoard[row, col]))
                    {
                        addPossibleMovesFromPosition(from, i_Board, i_Player, isCaptureMoveAvaliable, moves);
                    }
                }
            }

            return moves;
        }

        private static void addPossibleMovesFromPosition(Position i_From, Board i_Board, Player i_Player, bool i_IsMustCapture, List<Move> i_Moves)
        {
            if (i_IsMustCapture)
            {
                addCaptureMoves(i_From, i_Board, i_Player, i_Moves);
            }
            else
            {
                addRegularMoves(i_From, i_Board, i_Player, i_Moves);
            }
        }

        private static void addMoves(Position i_From, Board i_Board, Player i_Player, List <Move> i_Moves, int i_StepSize, bool i_IsCaptureMove)
        {
            int[] steps = { -i_StepSize, i_StepSize };

            foreach (int rowStep in steps)
            {
                foreach (int colStep in steps)
                {
                    Position to = new Position(i_From.Row + rowStep, i_From.Col + colStep);
                    Move move = new Move(i_From, to);
                    string message = null;
                    bool isValidMove = i_IsCaptureMove ? IsValidCaptureMove(move, i_Board, i_Player, ref message)
                                                       : IsValidRegularMove(move, i_Board, i_Player, ref message);

                    if (isValidMove)
                    {
                        i_Moves.Add(move);
                    }
                }
            }
        }

        private static void addCaptureMoves(Position i_From, Board i_Board, Player i_Player, List<Move> i_Moves)
        {
            addMoves(i_From, i_Board, i_Player, i_Moves, k_CaptureMoveDistance, k_IsCapture);
        }

        private static void addRegularMoves(Position i_From, Board i_Board, Player i_Player, List<Move> i_Moves)
        {
            addMoves(i_From, i_Board, i_Player, i_Moves, k_RegularMoveDistance, !k_IsCapture);
        }

        private static bool isValidMove(Move i_Move, Board i_Board, Player i_Player, ref string io_Message)
        {
            bool isValid = true;

            if (!isInBorders(i_Board, i_Move.From) || !isInBorders(i_Board, i_Move.To))
            {
                io_Message = "Move is out of board boundaries.";
                isValid = false;
            }
            else if (!i_Player.Pieces.Contains(i_Board.GameBoard[i_Move.From.Row, i_Move.From.Col]))
            {
                io_Message = "You can only move your own pieces.";
                isValid = false;
            }
            else if (i_Board.GameBoard[i_Move.To.Row, i_Move.To.Col] != null ||
                     i_Player.Pieces.Contains(i_Board.GameBoard[i_Move.To.Row, i_Move.To.Col]))
            {
                io_Message = "Destination position is not empty.";
                isValid = false;
            }
            else
            {
                int rowDiff = i_Move.To.Row - i_Move.From.Row;

                if (!isDirectionValid(i_Player.r_Direction, rowDiff, i_Board, i_Move))
                {
                    io_Message = "Invalid move direction for this piece type.";
                    isValid = false;
                }
            }

            return isValid;
        }

        public static bool IsValidRegularMove(Move i_Move, Board i_Board, Player i_Player, ref string io_Message)
        {
            bool isValid = isValidMove(i_Move, i_Board, i_Player, ref io_Message);

            if (isValid)
            {
                int rowDiff = i_Move.To.Row - i_Move.From.Row;
                int colDiff = i_Move.To.Col - i_Move.From.Col;

                if (!isValidRegularMove(rowDiff, colDiff))
                {
                    io_Message = "Regular move must be exactly one square diagonally.";
                    isValid = false;
                }
            }

            return isValid;
        }

        internal static bool IsValidCaptureMove(Move i_Move, Board i_Board, Player i_Player, ref string io_Message)
        {
            bool isValid = isValidMove(i_Move, i_Board, i_Player, ref io_Message);

            if (isValid)
            {
                int rowDiff = i_Move.To.Row - i_Move.From.Row;
                int colDiff = i_Move.To.Col - i_Move.From.Col;

                if (!isValidCaptureMove(rowDiff, colDiff))
                {
                    io_Message = "Capture move is available. you must make it.";
                    isValid = false;
                }
                else
                {
                    Position midPosition = new Position((i_Move.From.Row + i_Move.To.Row) / 2,
                                                        (i_Move.From.Col + i_Move.To.Col) / 2);

                    if (!isInBorders(i_Board, midPosition) || i_Board.GameBoard[midPosition.Row, midPosition.Col] == null ||
                        i_Player.Pieces.Contains(i_Board.GameBoard[midPosition.Row, midPosition.Col]))
                    {
                        io_Message = "No opponent piece to capture in the middle.";
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        internal static bool IsAnyCaptureMoves(Board i_Board, Player i_Player)
        {
            bool isCapture = false;

            for (int row = 0; row < i_Board.Size; row++)
            {
                for (int col = 0; col < i_Board.Size; col++)
                {
                    Position pos = new Position(row, col);

                    if (i_Player.Pieces.Contains(i_Board.GameBoard[row, col]) &&
                        IsCaptureMoveFromCurrentPosition(i_Board, i_Player, pos))
                    {
                        isCapture = true;
                        break;
                    }
                }

                if (isCapture)
                {
                    break;
                }
            }

            return isCapture;
        }

        internal static bool IsCaptureMoveFromCurrentPosition(Board i_Board, Player i_Player, Position i_FromPosition)
        {
            bool isCapture = false;

            if (isInBorders(i_Board, i_FromPosition) &&
                i_Board.GameBoard[i_FromPosition.Row, i_FromPosition.Col] != null &&
                i_Player.Pieces.Contains(i_Board.GameBoard[i_FromPosition.Row, i_FromPosition.Col]))
            {
                int[] steps = { -k_CaptureMoveDistance, k_CaptureMoveDistance };

                foreach (int rowStep in steps)
                {
                    foreach (int colStep in steps)
                    {
                        Position to = new Position(i_FromPosition.Row + rowStep, i_FromPosition.Col + colStep);

                        if (isInBorders(i_Board, to))
                        {
                            Move move = new Move(i_FromPosition, to);
                            string message = null;

                            if (IsValidCaptureMove(move, i_Board, i_Player, ref message))
                            {
                                isCapture = true;
                            }
                        }
                    }
                }
            }

            return isCapture;
        }

        private static bool isDirectionValid(ePlayerDirection i_Piece, int i_RowDiff, Board i_Board, Move i_Move)
        {
            Piece piece = i_Board.GameBoard[i_Move.From.Row, i_Move.From.Col];

            return piece.IsKing || (i_Piece == ePlayerDirection.Up && i_RowDiff > 0) ||
                   (i_Piece == ePlayerDirection.Down && i_RowDiff < 0);
        }

        private static bool isValidRegularMove(int i_RowDiff, int i_ColDiff)
        {
            return Math.Abs(i_RowDiff) == k_RegularMoveDistance && Math.Abs(i_ColDiff) == k_RegularMoveDistance;
        }

        private static bool isValidCaptureMove(int i_RowDiff, int i_ColDiff)
        {
            return Math.Abs(i_RowDiff) == k_CaptureMoveDistance && Math.Abs(i_ColDiff) == k_CaptureMoveDistance;
        }

        private static bool isInBorders(Board i_Board, Position i_Position)
        {
            return i_Position.Row >= 0 && i_Position.Row < i_Board.Size &&
                   i_Position.Col >= 0 && i_Position.Col < i_Board.Size;
        }
    }
}