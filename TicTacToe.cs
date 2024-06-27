using System;
using System.Runtime.InteropServices;

namespace TicTacToe
{
    class Program
    {
        static char[,] board = {
            { '1', '2', '3' },
            { '4', '5', '6' },
            { '7', '8', '9' }
        };

        static char currentPlayer = 'X';

        static void Main(string[] args)
        {
            int turns = 0;
            bool gameWon = false;

            while (turns < 9 && !gameWon)
            {
                SafeClearConsole();
                PrintBoard();
                PlayerMove();
                gameWon = CheckWin();
                if (!gameWon)
                {
                    SwitchPlayer();
                }
                turns++;
            }

            SafeClearConsole();
            PrintBoard();

            if (gameWon)
            {
                Console.WriteLine($"Player {currentPlayer} wins!");
            }
            else
            {
                Console.WriteLine("It's a draw!");
            }
        }

        static void SafeClearConsole()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);
                if (handle == INVALID_HANDLE_VALUE || handle == IntPtr.Zero)
                {
                    // Handle is invalid, don't clear console
                    return;
                }
            }

            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                // Handle the exception or log it if needed
                Console.WriteLine("Unable to clear the console.");
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        const int STD_OUTPUT_HANDLE = -11;
        static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        static void PrintBoard()
        {
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {board[0, 0]}  |  {board[0, 1]}  |  {board[0, 2]}  ");
            Console.WriteLine("_____|_____|_____");
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {board[1, 0]}  |  {board[1, 1]}  |  {board[1, 2]}  ");
            Console.WriteLine("_____|_____|_____");
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {board[2, 0]}  |  {board[2, 1]}  |  {board[2, 2]}  ");
            Console.WriteLine("     |     |     ");
        }

        static void PlayerMove()
        {
            int choice;
            bool validMove = false;

            while (!validMove)
            {
                Console.WriteLine($"Player {currentPlayer}, enter your move (1-9): ");
                validMove = int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 9 && MakeMove(choice);
                if (!validMove)
                {
                    Console.WriteLine("Invalid move, please try again.");
                }
            }
        }

        static bool MakeMove(int choice)
        {
            int row = (choice - 1) / 3;
            int col = (choice - 1) % 3;

            if (board[row, col] != 'X' && board[row, col] != 'O')
            {
                board[row, col] = currentPlayer;
                return true;
            }

            return false;
        }

        static void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
        }

        static bool CheckWin()
        {
            // Check rows, columns, and diagonals
            for (int i = 0; i < 3; i++)
            {
                if ((board[i, 0] == currentPlayer && board[i, 1] == currentPlayer && board[i, 2] == currentPlayer) ||
                    (board[0, i] == currentPlayer && board[1, i] == currentPlayer && board[2, i] == currentPlayer))
                {
                    return true;
                }
            }

            if ((board[0, 0] == currentPlayer && board[1, 1] == currentPlayer && board[2, 2] == currentPlayer) ||
                (board[0, 2] == currentPlayer && board[1, 1] == currentPlayer && board[2, 0] == currentPlayer))
            {
                return true;
            }

            return false;
        }
    }
}