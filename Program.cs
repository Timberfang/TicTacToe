namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Row and column size of game board
            const int GameBoardRows = 3;
            const int GameBoardCols = 3;

            Grid GameBoard = new(GameBoardRows, GameBoardCols, ' ');

            // Display grid
            Console.WriteLine("Here's the board:");
            Console.WriteLine(GameBoard.ToString());
        }

        public class Grid
        {
            private readonly int Rows;
            private readonly int Cols;
            private readonly char[,] GridCells;

            public Grid(int rows, int cols, char fillValue)
            {
                this.Rows = rows;
                this.Cols = cols;
                this.GridCells = new char[rows, cols];

                // Move top to bottom, left to right, filling array with empty spaces.
                for (int y = 0; y < this.Cols; y++)
                {
                    for (int x = 0; x < this.Rows; x++)
                    {
                        this.GridCells[x, y] = fillValue;
                    }
                }
            }

            public override string ToString()
            {
                string output = string.Empty;

                // output = "temp, please remove";

                for (int y = 0; y < this.Cols; y++)
                {
                    if (y > 0) { output += Environment.NewLine; }
                    for (int x = 0; x < this.Rows; x++)
                    {
                        output += $"[{this.GridCells[x, y]}] ";
                    }
                }

                return output;
            }

            public char GetValue(int row, int col)
            {
                return this.GridCells[row, col];
            }

            public void SetValue(int row, int col, char value)
            {
                this.GridCells[row, col] = value;
            }
        }

        enum TicTacToePlayer { PlayerX, PlayerO, Empty }
    }
}
