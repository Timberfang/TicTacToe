using System.Text.RegularExpressions;
using Spectre.Console;

namespace TicTacToe
{
	internal partial class Program
	{
		private static void Main(string[] args)
		{
			TicTacToe Game = new();
			Game.Start();
		}

		private class Grid
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

		private partial class TicTacToe
		{
			private TicTacToePlayer Player = TicTacToePlayer.PlayerX;
			private readonly Grid Board = new(3, 3, ' ');
			private int Rounds = 1;
			private GameState GameStatus = GameState.Ongoing;

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
                            Player = Player == TicTacToePlayer.PlayerX ? TicTacToePlayer.PlayerO : TicTacToePlayer.PlayerX;
							Rounds++;
							break;
						case GameState.Win:
							string PlayerName = Player switch
							{
								TicTacToePlayer.PlayerX => "Player X",
								TicTacToePlayer.PlayerO => "Player O",
								_ => throw new ArgumentOutOfRangeException($"Unrecognized player {Player}")
							};
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
                string PlayerInput =
	                AnsiConsole.Prompt(
						new TextPrompt<string>("Enter your move as X and Y coordinates. (e.g. '1,1' is the upper-left corner)")
							.Validate(ValidateInput));

                // Subtract 1 to convert human-readable numbers to array-equivalents
				var CleanedInput = CleanInput(PlayerInput);
                Board.SetValue(CleanedInput.Item1 - 1, CleanedInput.Item2 - 1, CurrentPlayer);
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
						if (!CheckedChars.All(c => c.Equals(CheckedChars[0]) && !c.Equals(' '))) { continue; }
						GameStatus = GameState.Win; break;
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

			private bool ValidateInput(string input)
			{
				if (!InputPattern().IsMatch(input)) { return false; }
				string[] inputs = input.Split(',');
				int x = int.Parse(inputs[0]);
				int y = int.Parse(inputs[1]);
				return Board.GetValue(x - 1, y - 1) == ' ';
			}

			private (int, int) CleanInput(string input)
			{
				int[] output = CleaningPattern().Replace(input, " ").Split(',').Select(int.Parse).ToArray();
				if (output.Length != 2) { throw new ArgumentOutOfRangeException(nameof(input)); }
				return (output[0], output[1]);
			}

			[GeneratedRegex(@"\s+")]
			private static partial Regex CleaningPattern();
			
            [GeneratedRegex(@"[1-3],\s*[1-3]")]
            private static partial Regex InputPattern();
        }

		private enum TicTacToePlayer { PlayerX, PlayerO }
		private enum GameState { Win, Draw, Ongoing }
	}
}
