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
			Game.Start();
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

			public char[] GetValues()
			{
				char[] output = new char[GridCells.Length];
                Buffer.BlockCopy(GridCells, 0, output, 0, GridCells.Length * sizeof(char));
                return output;
			}

			public void SetValue(int row, int col, char value)
			{
				this.GridCells[row, col] = value;
			}
		}

		public class TicTacToe
		{
			private TicTacToePlayer Player;
			private readonly Grid Board;
			private int Rounds;
			private GameState GameStatus;

			public TicTacToe(TicTacToePlayer player, int rows, int cols, int rounds = 1)
			{
				this.Player = player;
				this.Board = new Grid(rows, cols, ' ');
				this.Rounds = rounds;
				GameStatus = GameState.Ongoing;
			}

			public void Start()
			{
				while (GameStatus == GameState.Ongoing)
				{
					DisplayBoard();
					PlayerInput();
					if (Rounds >= 3) { UpdateGameStatus(); }
					switch (GameStatus)
					{
						case GameState.Ongoing:
                            if (Player == TicTacToePlayer.PlayerX) { Player = TicTacToePlayer.PlayerO; }
                            else { Player = TicTacToePlayer.PlayerX; }
							Rounds++;
							break;
						case GameState.Win:
							string PlayerName;
							switch (Player)
							{
								case TicTacToePlayer.PlayerX:
									PlayerName = "Player X";
									break;
								case TicTacToePlayer.PlayerO:
									PlayerName = "Player O";
									break;
								default:
									throw new ArgumentOutOfRangeException($"Unrecognized player {Player}");
							}
							Console.WriteLine($"Congratulations, '{PlayerName}' won!");
							break;
						case GameState.Draw:
							Console.WriteLine("No one won, this game is a draw!");
							break;
						default:
							throw new ArgumentOutOfRangeException($"Unrecognized game state '{GameStatus}'");
                    }
				}
			}

			private void DisplayBoard()
			{
				// Display grid
				Console.Clear();
				Console.WriteLine("Here's the board:");
				Console.WriteLine(Board.ToString());
			}

			private void PlayerInput()
			{
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

			private void UpdateGameStatus()
			{
				char[] CheckedChars = new char[3];

				// Columns
				for (int y = 0; y < 3; y++)
				{
					for (int x = 0; x < 3; x++)
					{
						CheckedChars[x] = Board.GetValue(x, y);
						if (CheckedChars.All(c => c.Equals(CheckedChars[0]) && !c.Equals(' '))) { GameStatus = GameState.Win; break; }
					}
                    Array.Clear(CheckedChars, 0, CheckedChars.Length);
                }

				// Rows
				for (int x = 0; x < 3; x++)
				{
					for (int y = 0; y < 3; y++)
					{
						// CheckedChars[x] = Board.GetValue(x, y);
						CheckedChars[y] = Board.GetValue(x, y);
						if (CheckedChars.All(c => c.Equals(CheckedChars[0]) && !c.Equals(' '))) { GameStatus = GameState.Win; break; }
					}
                    Array.Clear(CheckedChars, 0, CheckedChars.Length);
                }
                // Top-left to bottom-right
                CheckedChars[0] = Board.GetValue(0, 0);
				CheckedChars[1] = Board.GetValue(1, 1);
				CheckedChars[2] = Board.GetValue(2, 2);
				if (CheckedChars.All(c => c.Equals(CheckedChars[0]) && !c.Equals(' '))) { GameStatus = GameState.Win; }
                Array.Clear(CheckedChars, 0, CheckedChars.Length);
                // Top-right to bottom-left
                CheckedChars[0] = Board.GetValue(0, 2);
				CheckedChars[1] = Board.GetValue(1, 1);
				CheckedChars[2] = Board.GetValue(2, 0);
				if (CheckedChars.All(c => c.Equals(CheckedChars[0]) && !c.Equals(' '))) { GameStatus = GameState.Win; }
                Array.Clear(CheckedChars, 0, CheckedChars.Length);

                if (!Board.GetValues().Contains(' ') && GameStatus != GameState.Win) { GameStatus = GameState.Draw; }
			}
		}
		
		public enum TicTacToePlayer { PlayerX, PlayerO }
		private enum GameState { Win, Draw, Ongoing }
	}
}
