using Spectre.Console;

namespace TicTacToe.GameData;

internal class TicTacToeGame
{
    private TicTacToePlayer _player = TicTacToePlayer.PlayerX;
    private readonly GameData.Grid _board = new(3, 3, ' ');
    private int _rounds = 1;
    private GameState _gameStatus = GameState.Ongoing;

    public void Start()
    {
        while (_gameStatus == GameState.Ongoing)
        {
            DisplayBoard();
            GetPlayerInput();
            if (_rounds >= 3) { _gameStatus = GetGameState(); }
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
                        _ => throw new ArgumentOutOfRangeException($"Unrecognized player '{_player}'")
                    };
                    Console.WriteLine($"Congratulations, {playerName} won!");
                    break;
                case GameState.Draw:
                    Console.WriteLine("Sorry, we have a draw!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unrecognized game state '{_gameStatus}'");
            }
        }
        AnsiConsole.WriteLine("Press any key to exit.");
        AnsiConsole.Console.Input.ReadKey(false);
    }

    private void DisplayBoard()
    {
        // Display grid
        Console.Clear();
        Console.WriteLine("Here's the board:");
        Console.WriteLine(_board.ToString());
    }

    private void GetPlayerInput()
    {
        char currentPlayer = _player switch
        {
            TicTacToePlayer.PlayerX => 'X',
            TicTacToePlayer.PlayerO => 'O',
            _ => ' '
        };
        Console.WriteLine($"Player {currentPlayer}, it is your turn.");
        string playerInput =
            AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your move as X and Y coordinates, separated by a comma:")
                    .Validate(ValidateInput));
        var parsedInput = ParseInput(playerInput);
        _board.SetValue(parsedInput.Item1, parsedInput.Item2, currentPlayer);
        DisplayBoard();
    }

    private GameState GetGameState()
    {
        char[] cellCheck = new char[3];

        // Columns
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                cellCheck[x] = _board.GetValue(x, y);
                if (!cellCheck.All(c => c.Equals(cellCheck[0]) && !c.Equals(' '))) { continue; }
                return GameState.Win;
            }
            Array.Clear(cellCheck, 0, cellCheck.Length);
        }

        // Rows
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                cellCheck[y] = _board.GetValue(x, y);
                if (!cellCheck.All(c => c.Equals(cellCheck[0]) && !c.Equals(' '))) { continue; }
                return GameState.Win;
            }
            Array.Clear(cellCheck, 0, cellCheck.Length);
        }

        // Diagonal - top-left to bottom-right
        for (int x = 0; x < 3; x++)
        {
            cellCheck[x] = _board.GetValue(x, x);
            if (!cellCheck.All(c => c.Equals(cellCheck[0]) && !c.Equals(' '))) { continue; }
            return GameState.Win;
        }
        Array.Clear(cellCheck, 0, cellCheck.Length);

        // Diagonal - top-right to bottom-left
        for (int x = 0; x < 3; x++)
        {
            cellCheck[x] = _board.GetValue(x, 2 - x);
            if (!cellCheck.All(c => c.Equals(cellCheck[0]) && !c.Equals(' '))) { continue; }
            return GameState.Win;
        }
        Array.Clear(cellCheck, 0, cellCheck.Length);
        return !_board.GetValues().Contains(' ') ? GameState.Draw : GameState.Ongoing;
    }

    private bool ValidateInput(string input)
    {
        if (!GameRegex.InputPattern().IsMatch(input)) { return false; }
        string[] inputs = input.Split(',');
        int x = int.Parse(inputs[0]);
        int y = int.Parse(inputs[1]);
        return _board.GetValue(x - 1, y - 1) == ' ';
    }

    private static (int, int) ParseInput(string input)
    {
        int[] output = GameRegex.CleaningPattern().Replace(input, " ").Split(',').Select(int.Parse).ToArray();
        if (output.Length != 2) { throw new ArgumentOutOfRangeException(nameof(input)); }
        return (output[0] - 1, output[1] - 1);
    }
}