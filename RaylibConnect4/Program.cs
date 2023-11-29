using Raylib_cs;

namespace RaylibConnect4
{
    static class Program
    {
        const int cellWidth = 120;
        const int cellHeight = 100;

        const int boardWidth = 7;
        const int boardHeight = 6;

        static int[,] board = new int[boardWidth, boardHeight];

        static Color[] playerColors = { Color.RAYWHITE, Color.RED, Color.GOLD };

        static int currentPlayer = 1;
        static int selectedColumn = 3;

        static bool gameOver = false;

        static int winner = 0;

        static Random rand = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Begin();
            
            while (!Raylib.WindowShouldClose())
            {
                Update();
                Draw();
            }

            End();
        }

        static void Begin()
        {
            Raylib.InitWindow(cellWidth * boardWidth, cellHeight * (boardHeight + 1), "Connect 4");
        }

        static void Update()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
            {
                selectedColumn--;
                if (selectedColumn < 0) selectedColumn = boardWidth - 1;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
            {
                selectedColumn++;
                if (selectedColumn >= boardWidth) selectedColumn = 0;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) || Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
            {
                if (board[selectedColumn, 0] == 0)
                {
                    int i = 1;
                    while (i < boardHeight && board[selectedColumn, i] == 0)
                    {
                        i++;
                    }
                    board[selectedColumn, i - 1] = currentPlayer;
                    currentPlayer = currentPlayer == 1 ? 2 : 1;
                    winner = GetWinner();
                    Console.WriteLine($"{winner} wins!");
                }
            }
        }

        static void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RAYWHITE);
            DrawCounter((int)((selectedColumn + 0.5f) * cellWidth), (int)((0.5f) * cellHeight), playerColors[currentPlayer]);
            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    Raylib.DrawRectangle(x * cellWidth, (y + 1) * cellHeight, cellWidth, cellHeight, ((x + y) & 1) == 1 ? Color.DARKBLUE : Color.BLUE);
                    DrawCounter((int)((x + 0.5f) * cellWidth), (int)((y + 1.5f) * cellHeight), playerColors[board[x, y]], board[x, y] != 0);
                }
            }
            Raylib.EndDrawing();
        }

        static void DrawCounter(int x, int y, Color col, bool lighten = true)
        {
            Raylib.DrawCircle(x, y, cellHeight * 0.4f, col);
            Raylib.DrawCircle(x, y, cellHeight * 0.3f, lighten ? col.Lighten() : col);
        }

        static void End()
        {
            Raylib.CloseWindow();
        }

        static int GetWinner()
        {
            int winner = 0;

            int x, y;

            x = 0;
            while (winner == 0 && x < boardWidth) //vertical lines
            {
                int count = 1;
                y = 1;
                while (count < 4 && y < boardHeight)
                {
                    if (board[x, y] == board[x, y - 1] && board[x, y] != 0)
                    {
                        count++;
                    }
                    else
                    {
                        count = board[x, y] == 0 ? 0 : 1;
                    }
                    y++;
                }
                if (count > 3) winner = board[x, y - 1];
                x++;
            }

            y = 0;
            while (winner == 0 && y < boardHeight) //horizontal lines
            {
                int count = 1;
                x = 1;
                while (count < 4 && x < boardWidth)
                {
                    if (board[x - 1, y] == board[x, y] && board[x, y] != 0)
                    {
                        count++;
                    }
                    else
                    {
                        count = board[x, y] == 0 ? 0 : 1;
                    }
                    x++;
                }
                if (count > 3) winner = board[x - 1, y];
                y++;
            }

            x = 0;
            y = 0;
            while (winner == 0 && y < boardHeight - 3) //diagonal lines
            {
                winner = GetLineWinner(x, y, x + 1, y + 1, x + 2, y + 2, x + 3, y + 3);
                if (winner == 0) winner = GetLineWinner(x, y + 3, x + 1, y + 2, x + 2, y + 1, x + 3, y);
                x++;
                if (x >= boardWidth - 3)
                {
                    x = 0;
                    y++;
                }
            }

            return winner;
        }

        static int GetLineWinner(int x0, int y0, int x1, int y1, int x2, int y2, int x3, int y3)
        {
            if (board[x0, y0] == 0 || board[x1, y1] == 0 || board[x2, y2] == 0 || board[x3, y3] == 0) return 0;
            else if (board[x0, y0] == board[x1, y1] && board[x0, y0] == board[x2, y2] && board[x0, y0] == board[x3, y3])
            {
                return board[x0, y0];
            }
            else return 0;
        }

        static Color Lighten(this Color c)
        {
            return new Color(Math.Min(c.R + 25, 255), Math.Min(c.G + 25, 255), Math.Min(c.B + 25, 255), 255);
        }
    }
}