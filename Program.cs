namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Row and column size of game board
            const int GameBoardRows = 3;
            const int GameBoardCols = 3;

            TicTacToe Game = new(TicTacToePlayer.PlayerX, GameBoardRows, GameBoardCols);
            // Game.Start();
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

				for (int x = 0; x < this.Cols; x++)
                {
					if (x > 0) { output += Environment.NewLine; }
					for (int y = 0; y < this.Rows; y++)
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

        public class TicTacToe
        {
            private readonly TicTacToePlayer Player;
            private readonly Grid Board;
            private readonly int Rounds;

            public TicTacToe(TicTacToePlayer player, int rows, int cols, int rounds = 1)
            {
                this.Player = player;
                this.Board = new Grid(rows, cols, ' ');
                this.Rounds = rounds;
            }

            private void DisplayBoard()
            {
                // Display grid
                Console.WriteLine("Here's the board:");
                Console.WriteLine(Board.ToString());
            }

            private void PlayerInput()
            {
                Console.Clear();
                DisplayBoard();

                char CurrentPlayer = Player switch
                {
                    TicTacToePlayer.PlayerX => 'X',
                    TicTacToePlayer.PlayerO => 'O',
                    _ => ' '
                };
                Console.WriteLine($"Player {CurrentPlayer}, it is your turn.");
                Console.Write("Enter your move as X and Y coordinates. (e.g. '1,1' is the upper-left corner): ");
                string[] PlayerInput = Console.ReadLine().Split(','); // TODO: Implement error-handling for this

                // Subtract 1 to convert human-readable numbers to array-equivalents
                Board.SetValue(int.Parse(PlayerInput[0]) - 1, int.Parse(PlayerInput[1]) - 1, CurrentPlayer);

                DisplayBoard();
            }
        }
        
        public enum TicTacToePlayer { PlayerX, PlayerO }
    }
}
