using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { NONE, PLAYER1, PLAYER2 }

public struct GridPosition { public int row, column; }

public class Board
{
    PlayerType[][] board;
    GridPosition currentPosition; // stores current move

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

    public void UpdateBoard(int column, bool isPlayer1Turn)
    {
        int updatePosition = 6; // we start from the bottom of a column
        for (int i = 5; i >= 0; i--)
        {
            if (board[i][column] == PlayerType.NONE)
            {
                updatePosition--;
            } else
            {
                break;
            }
        }

        board[updatePosition][column] = isPlayer1Turn ? PlayerType.PLAYER1 : PlayerType.PLAYER2;
        currentPosition = new GridPosition { row = updatePosition, column = column };
    }

    // Method for checking if a player has won in any of the following directions: horizontal, vertical, diagonal, reverse diagonal
    public bool hasWon(bool isPlayer1Turn)
    {
        PlayerType currentPlayer = isPlayer1Turn ? PlayerType.PLAYER1 : PlayerType.PLAYER2;
        return IsHorizontal(currentPlayer) || IsVertical(currentPlayer) || IsDiagonal(currentPlayer) || IsReverseDiagonal(currentPlayer);
    }

    // Method for checking if a player has won horizontally
    // It does this by first retrieving the start position (column) of the current row. This means that we will reach the edge of the board.
    // It then gathers all positions in between this start position and the current position of the player on the board and adds them to a list.
    // Finally, it checks if there are four consecutive occurences (positions) of the player's color in that list.
    bool IsHorizontal(PlayerType currentPlayer)
    {
        GridPosition start = GetEndPoint(new GridPosition { row = 0, column = -1 }); // we do not care about the row. Only about the column
        List<GridPosition> toSearchList = GetPlayerList(start, new GridPosition { row = 0, column = 1 });
        return SearchResult(toSearchList, currentPlayer);
    }

    // Method for checking if a player has won vertically
    // It does this by first retrieving the start position (row) of the current column. This means that we will reach the edge of the board.
    // It then gathers all positions in between this start position and the current position of the player on the board and adds them to a list.
    // Finally, it checks if there are four consecutive occurences (positions) of the player's color in that list.
    bool IsVertical(PlayerType currentPlayer)
    {
        GridPosition start = GetEndPoint(new GridPosition { row = -1, column = 0 }); // we do not care about the column. Only about the row
        List<GridPosition> toSearchList = GetPlayerList(start, new GridPosition { row = 1, column = 0 });
        return SearchResult(toSearchList, currentPlayer);
    }

    // Method for checking if a player has won diagonally (left to right)
    // It does this by first retrieving the start position (row and column). This means that we will reach the edge of the board.
    // It then gathers all positions in between this start position and the current position of the player on the board and adds them to a list.
    // Finally, it checks if there are four consecutive occurences (positions) of the player's color in that list.
    bool IsDiagonal(PlayerType currentPlayer)
    {
        GridPosition start = GetEndPoint(new GridPosition { row = -1, column = -1 });
        List<GridPosition> toSearchList = GetPlayerList(start, new GridPosition { row = 1, column = 1 });
        return SearchResult(toSearchList, currentPlayer);
    }

    // Method for checking if a player has won diagonally (right to left)
    // It does this by first retrieving the start position (row and column). This means that we will reach the edge of the board.
    // It then gathers all positions in between this start position and the current position of the player on the board and adds them to a list.
    // Finally, it checks if there are four consecutive occurences (positions) of the player's color in that list.
    bool IsReverseDiagonal(PlayerType currentPlayer)
    {
        GridPosition start = GetEndPoint(new GridPosition { row = -1, column = 1 });
        List<GridPosition> toSearchList = GetPlayerList(start, new GridPosition { row = 1, column = -1 });
        return SearchResult(toSearchList, currentPlayer);
    }

    // Method for retrieving the start position (which is always situated on the edge of the board) based 
    // on an accumulator and an end position.
    GridPosition GetEndPoint(GridPosition diff)
    {
        GridPosition result = new GridPosition { row = currentPosition.row, column = currentPosition.column };
        // The while ensures that we do not go over the edge of the board
        while (result.row + diff.row < 6 &&
                result.column + diff.column < 7 &&
                result.row + diff.row >= 0 &&
                result.column + diff.column >= 0)
        {
            result.row += diff.row;
            result.column += diff.column;
        }
        return result;
    }

    // Method for appending all the positions in between a start and a finish to a list.
    List<GridPosition> GetPlayerList(GridPosition start, GridPosition diff)
    {
        List<GridPosition> resList;
        resList = new List<GridPosition> { start };
        GridPosition result = new GridPosition { row = start.row, column = start.column };
        while (result.row + diff.row < 6 &&
                result.column + diff.column < 7 &&
                result.row + diff.row >= 0 &&
                result.column + diff.column >= 0)
        {
            result.row += diff.row;
            result.column += diff.column;
            resList.Add(result);
        }

        return resList;
    }

    // Method for checking if there are 4 consecutive occurences of the current player's color
    // in the list of positions generated by the GetPlayerList() method.
    bool SearchResult(List<GridPosition> searchList, PlayerType current)
    {
        int counter = 0;

        for (int i = 0; i < searchList.Count; i++)
        {
            PlayerType compare = board[searchList[i].row][searchList[i].column]; // get the value stored at this position (NONE, PLAYER1, PLAYER2)
            if (compare == current)
            {
                counter++;
                if (counter == 4)
                    break;
            }
            else
            {
                counter = 0;
            }
        }

        return counter >= 4;
    }
}
