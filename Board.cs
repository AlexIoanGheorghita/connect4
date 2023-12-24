using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { NONE, PLAYER1, PLAYER2 }

public struct GridPosition { public int row, col; }

public class Board
{
    PlayerType[][] board;
    GridPosition currentPosition;

    public Board()
    {
        // create an empty 2D array that will only be able to contain values of the enum type PlayerType.
        // We initially make it have 6 rows, with no columns.
        board = new PlayerType[6][];
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new PlayerType[7];
            for (int j = 0;  j < board[i].Length; j++)
            {
                board[i][j] = PlayerType.NONE; // populate the 2D array with the NONE value (i.e., no one did a move yet)
            }
        }
    }
}
