namespace TicTacToe.GameData;

public class Grid
{
    private readonly int _rows;
    private readonly int _cols;
    private readonly char[,] _gridCells;

    public Grid(int rows, int cols, char fillValue)
    {
        _rows = rows;
        _cols = cols;
        _gridCells = new char[rows, cols];

        // Move top to bottom, left to right, filling array with empty spaces.
        for (int y = 0; y < _cols; y++)
        {
            for (int x = 0; x < _rows; x++)
            {
                _gridCells[x, y] = fillValue;
            }
        }
    }

    public override string ToString()
    {
        string output = string.Empty;

        for (int x = 0; x < _cols; x++)
        {
            if (x > 0) { output += Environment.NewLine; }
            for (int y = 0; y < _rows; y++)
            {
                output += $"[{_gridCells[x, y]}] ";
            }
        }

        return output;
    }

    public char GetValue(int row, int col)
    {
        return _gridCells[row, col];
    }

    public char[] GetValues()
    {
        char[] output = new char[_gridCells.Length];
        Buffer.BlockCopy(_gridCells, 0, output, 0, _gridCells.Length * sizeof(char));
        return output;
    }

    public void SetValue(int row, int col, char value)
    {
        _gridCells[row, col] = value;
    }
}