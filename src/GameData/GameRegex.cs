using System.Text.RegularExpressions;

namespace TicTacToe.GameData;

public static partial class GameRegex
{
    [GeneratedRegex(@"\s+")]
    public static partial Regex CleaningPattern();

    [GeneratedRegex(@"[1-3],\s*[1-3]")]
    public static partial Regex InputPattern();
}