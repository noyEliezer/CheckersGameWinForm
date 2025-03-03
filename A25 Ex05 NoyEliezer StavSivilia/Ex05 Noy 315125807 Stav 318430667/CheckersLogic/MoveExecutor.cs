using System;
using System.Collections.Generic;

namespace Ex05.CheckersGame.CheckersLogic
{
    public static class MoveExecutor
    {
        private const bool k_IsCaptureMove = true;
        private static readonly Random s_Random = new Random();
        private static Position? s_LastCapturingPiecePosition = null;
        private static bool s_IsInMultiCapture = false;

        internal static bool ExecuteMove(Move i_Move, Board i_Board, Player i_CurrentPlayer, ref bool io_IsGetsAnotherTurn, ref string io_Message)
        {
            bool isMoveMade = false;
            io_IsGetsAnotherTurn = false;

            if (s_IsInMultiCapture)
            {
                if (!i_Move.From.Equals(s_LastCapturingPiecePosition))
                {
                    io_Message = "Must continue capture sequence with the same piece";
                }
                else if (MoveValidator.IsValidCaptureMove(i_Move, i_Board, i_CurrentPlayer, ref io_Message))
                {
                    executeCaptureMove(i_Move, i_Board, i_CurrentPlayer, ref io_IsGetsAnotherTurn);
                    isMoveMade = true;
                }
            }
            else if (MoveValidator.IsAnyCaptureMoves(i_Board, i_CurrentPlayer))
            {
                if (MoveValidator.IsValidCaptureMove(i_Move, i_Board, i_CurrentPlayer, ref io_Message))
                {
                    executeCaptureMove(i_Move, i_Board, i_CurrentPlayer, ref io_IsGetsAnotherTurn);
                    isMoveMade = true;
                }
            }
            else if (MoveValidator.IsValidRegularMove(i_Move, i_Board, i_CurrentPlayer, ref io_Message))
            {
                executeRegularMove(i_Move, i_Board, i_CurrentPlayer);
                isMoveMade = true;
            }

            if (!isMoveMade || (!io_IsGetsAnotherTurn))
            {
                s_LastCapturingPiecePosition = null;
                s_IsInMultiCapture = false;
            }

            return isMoveMade;
        }

        private static void executeCaptureMove(Move i_Move, Board i_Board, Player i_CurrentPlayer, ref bool io_IsGetsAnotherTurn)
        {
            executeMoveOnBoard(i_Move, i_Board, i_CurrentPlayer, k_IsCaptureMove);
            io_IsGetsAnotherTurn = MoveValidator.IsCaptureMoveFromCurrentPosition(i_Board, i_CurrentPlayer, i_Move.To);
            if (io_IsGetsAnotherTurn)
            {
                s_LastCapturingPiecePosition = i_Move.To;
                s_IsInMultiCapture = true;
            }
            else
            {
                s_LastCapturingPiecePosition = null;
                s_IsInMultiCapture = false;
            }
        }

        private static void executeRegularMove(Move i_Move, Board i_Board, Player i_CurrentPlayer)
        {
            executeMoveOnBoard(i_Move, i_Board, i_CurrentPlayer, !k_IsCaptureMove);
        }

        internal static bool ExecuteComputerMove(Board i_Board, Player i_ComputerPlayer, ref bool io_IsGetsAnotherTurn, ref Move io_ComputerMove)
        {
            bool isMoveMade = false;
            List<Move> possibleMoves = MoveValidator.GetAllPossibleMoves(i_Board, i_ComputerPlayer);
            List<Move> filteredMoves = new List<Move>();
            string errorMessage = string.Empty;

            if (s_IsInMultiCapture && s_LastCapturingPiecePosition != null)
            {
                foreach (Move move in possibleMoves)
                {
                    if (move.From.Equals(s_LastCapturingPiecePosition) &&
                        MoveValidator.IsValidCaptureMove(move, i_Board, i_ComputerPlayer, ref errorMessage))
                    {
                        filteredMoves.Add(move);
                    }
                }

                possibleMoves = filteredMoves;
            }

            if (possibleMoves.Count == 0)
            {
                io_IsGetsAnotherTurn = false;
                s_LastCapturingPiecePosition = null;
                s_IsInMultiCapture = false;
                io_ComputerMove = null;
            }
            else
            {
                io_ComputerMove = possibleMoves[s_Random.Next(possibleMoves.Count)];
                isMoveMade = ExecuteMove(io_ComputerMove, i_Board, i_ComputerPlayer, ref io_IsGetsAnotherTurn, ref errorMessage);
            }

            return isMoveMade;
        }

        private static void executeMoveOnBoard(Move i_Move, Board i_Board, Player i_Player, bool i_IsCapture)
        {
            if (i_IsCapture)
            {
                i_Board.CapturePiece(i_Move);
            }
            else
            {
                i_Board.MovePiece(i_Move);
            }

            checkAndPromoteToKing(i_Move, i_Board, i_Player);
        }

        private static void checkAndPromoteToKing(Move i_Move, Board i_Board, Player i_CurrentPlayer)
        {
            Piece piece = i_Board.GameBoard[i_Move.To.Row, i_Move.To.Col];

            if (piece != null)
            {
                bool isShouldPromote = (i_CurrentPlayer.Direction == ePlayerDirection.Up && i_Move.To.Row == i_Board.Size - 1) ||
                                   (i_CurrentPlayer.Direction == ePlayerDirection.Down && i_Move.To.Row == 0);

                piece.IsKing = piece.IsKing || isShouldPromote;
            }
        }
    }
}
