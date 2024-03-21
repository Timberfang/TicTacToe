namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Row and column size of game board
            const int GameBoardRows = 3;
            const int GameBoardCols = 3;

            Console.WriteLine("Here's the board:");

            TicTacToePlayer[,] GameBoard = new TicTacToePlayer[GameBoardRows, GameBoardCols];

            // Move top to bottom, left to right, filling array with empty spaces.
            for (int y = 0; y < GameBoardCols; y++)
            {
                for (int x = 0; x < GameBoardRows; x++)
                {
                    GameBoard[x, y] = TicTacToePlayer.Empty;
                }
            }
            
            // Display grid
            for (int y = 0;y < GameBoardCols; y++)
            {
                if (y > 0) { Console.WriteLine(); }
                for (int x = 0;x < GameBoardRows; x++)
                {
                    char CellContents;
                    switch (GameBoard[x, y])
                    {
                        case TicTacToePlayer.PlayerX:
                            CellContents = 'X';
                            break;
                        case TicTacToePlayer.PlayerO:
                            CellContents = 'O';
                            break;
                        case TicTacToePlayer.Empty:
                            CellContents = ' ';
                            break;
                        default:
                            CellContents = ' ';
                            break;

                    }
                    Console.Write("[{0}] ", CellContents);
                }
            }
        }

        enum TicTacToePlayer { PlayerX, PlayerO, Empty }
    }
}
