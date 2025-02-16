using System.Text.RegularExpressions;
using Spectre.Console;

namespace TicTacToe
{
	internal static partial class Program
	{
		private static void Main()
		{
			TicTacToe game = new();
			game.Start();
		}

		private class Grid
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

		private partial class TicTacToe
		{
			private TicTacToePlayer _player = TicTacToePlayer.PlayerX;
			private readonly Grid _board = new(3, 3, ' ');
			private int _rounds = 1;
			private GameState _gameStatus = GameState.Ongoing;

			public void Start()
			{
				while (_gameStatus == GameState.Ongoing)
				{
					DisplayBoard();
					PlayerInput();
					if (_rounds >= 3) { UpdateGameStatus(); }
					switch (_gameStatus)
					{
						case GameState.Ongoing:
                            _player = _player == TicTacToePlayer.PlayerX ? TicTacToePlayer.PlayerO : TicTacToePlayer.PlayerX;
							_rounds++;
							break;
						case GameState.Win:
							string playerName = _player switch
							{
								TicTacToePlayer.PlayerX => "Player X",
								TicTacToePlayer.PlayerO => "Player O",
								_ => throw new ArgumentOutOfRangeException($"Unrecognized player {_player}")
							};
							Console.WriteLine($"Congratulations, '{playerName}' won!");
							break;
						case GameState.Draw:
							Console.WriteLine("No one won, this game is a draw!");
							break;
						default:
							throw new ArgumentOutOfRangeException($"Unrecognized game state '{_gameStatus}'");
                    }
				}
			}

			private void DisplayBoard()
			{
				// Display grid
				Console.Clear();
				Console.WriteLine("Here's the board:");
				Console.WriteLine(_board.ToString());
			}

			private void PlayerInput()
			{
				char currentPlayer = _player switch
				{
					TicTacToePlayer.PlayerX => 'X',
					TicTacToePlayer.PlayerO => 'O',
					_ => ' '
				};
				Console.WriteLine($"Player {currentPlayer}, it is your turn.");
				Console.Write("Enter your move as X and Y coordinates. (e.g. '1,1' is the upper-left corner): ");
                string playerInput =
	                AnsiConsole.Prompt(
						new TextPrompt<string>("Enter your move as X and Y coordinates. (e.g. '1,1' is the upper-left corner)")
							.Validate(ValidateInput));

                // Subtract 1 to convert human-readable numbers to array-equivalents
				var cleanedInput = CleanInput(playerInput);
                _board.SetValue(cleanedInput.Item1 - 1, cleanedInput.Item2 - 1, currentPlayer);
                DisplayBoard();
            }

			private void UpdateGameStatus()
			{
				char[] checkedChars = new char[3];

				// Columns
				for (int y = 0; y < 3; y++)
				{
					for (int x = 0; x < 3; x++)
					{
						checkedChars[x] = _board.GetValue(x, y);
						if (!checkedChars.All(c => c.Equals(checkedChars[0]) && !c.Equals(' '))) { continue; } 
						_gameStatus = GameState.Win; break;
					}
                    Array.Clear(checkedChars, 0, checkedChars.Length);
                }

				// Rows
				for (int x = 0; x < 3; x++)
				{
					for (int y = 0; y < 3; y++)
					{
						// CheckedChars[x] = Board.GetValue(x, y);
						checkedChars[y] = _board.GetValue(x, y);
						if (!checkedChars.All(c => c.Equals(checkedChars[0]) && !c.Equals(' '))) { continue; }
						_gameStatus = GameState.Win; break;
					}
                    Array.Clear(checkedChars, 0, checkedChars.Length);
                }
                // Top-left to bottom-right
                checkedChars[0] = _board.GetValue(0, 0);
				checkedChars[1] = _board.GetValue(1, 1);
				checkedChars[2] = _board.GetValue(2, 2);
				if (checkedChars.All(c => c.Equals(checkedChars[0]) && !c.Equals(' '))) { _gameStatus = GameState.Win; }
                Array.Clear(checkedChars, 0, checkedChars.Length);
                // Top-right to bottom-left
                checkedChars[0] = _board.GetValue(0, 2);
				checkedChars[1] = _board.GetValue(1, 1);
				checkedChars[2] = _board.GetValue(2, 0);
				if (checkedChars.All(c => c.Equals(checkedChars[0]) && !c.Equals(' '))) { _gameStatus = GameState.Win; }
                Array.Clear(checkedChars, 0, checkedChars.Length);

                if (!_board.GetValues().Contains(' ') && _gameStatus != GameState.Win) { _gameStatus = GameState.Draw; }
			}

			private bool ValidateInput(string input)
			{
				if (!InputPattern().IsMatch(input)) { return false; }
				string[] inputs = input.Split(',');
				int x = int.Parse(inputs[0]);
				int y = int.Parse(inputs[1]);
				return _board.GetValue(x - 1, y - 1) == ' ';
			}

			private static (int, int) CleanInput(string input)
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
