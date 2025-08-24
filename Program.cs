/**
*   namespace to group together the Knight's Tour code and the 
*/
namespace KnightsTour
{
    enum Squares
    {
        UNVISITED,
        KNIGHT,
        VISITED
    }
    class Solver
    {
        /**
        *   A function to find a solution to the current board
        */
        private static bool SolveBoard(Board board)
        {
            try
            {
                if (board.IsSolved())
                {
                    return true;
                }
                PriorityQueue<Board, int> boards = new();
                boards.Enqueue(board, 0);
                while (boards.Count > 0)
                {
                    Board current = boards.Dequeue();
                    List<Board> neighbors = current.GetNeighbors();
                    foreach (Board neighbor in neighbors)
                    {
                        if (neighbor.IsSolved())
                        {
                            return true;
                        }
                        boards.Enqueue(neighbor, neighbor.GetNeighborCount());
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /**
        *   A function to make the next move in a solution if one exists as a hint
        */
        public static bool MakeNextMove(ref Board board)
        {
            try
            {
                List<Board> neighbors = board.GetNeighbors();
                foreach (Board neighbor in neighbors)
                {
                    if (SolveBoard(neighbor) == true)
                    {
                        board.Move(neighbor.GetCurrentRow(), neighbor.GetCurrentCol());
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
    class Board
    {
        private int dim;
        private Squares[,] board;
        private int currentRow = -1;
        private int currentCol = -1;

        /**
        *   Constructor for a new board configuration
        *   If board is not provided, create an empty board,
        *   otherwise copy the squares from the board provided
        */
        public Board(int dim, Squares[,]? board = null)
        {
            this.dim = dim;
            this.board = new Squares[this.dim, this.dim];
            if (board == null)
            {
                for (int i = 0; i < this.dim; i++)
                {
                    for (int j = 0; j < this.dim; j++)
                    {
                        this.board[i, j] = Squares.UNVISITED;
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.dim; i++)
                {
                    for (int j = 0; j < this.dim; j++)
                    {
                        this.board[i, j] = board[i, j];
                        if (this.board[i, j] == Squares.KNIGHT)
                        {
                            this.currentRow = i;
                            this.currentCol = j;
                        }
                    }
                }
            }
        }

        /**
        *   Returns true if no squares on the board are unvisited, false otherwise
        */
        public bool IsSolved()
        {
            for (int i = 0; i < this.dim; i++)
            {
                for (int j = 0; j < this.dim; j++)
                {
                    if (this.board[i, j] == Squares.UNVISITED)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /**
        *   Returns true if no "neighbor" configurations exist and no moves can be made, false otherwise
        */
        public bool NoMoves()
        {
            return this.GetNeighbors().Count == 0;
        }

        /**
        *   A function to reset the board and current row and column
        *   Returns true if reset successfully, false otherwise
        */
        public bool ResetBoard()
        {
            try
            {
                this.currentRow = -1;
                this.currentCol = -1;
                for (int i = 0; i < this.dim; i++)
                {
                    for (int j = 0; j < this.dim; j++)
                    {
                        this.board[i, j] = Squares.UNVISITED;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /**
        *   A function to check if moving to the square at the row and column
        *   provided is valid
        */
        public bool ValidMove(int row, int col)
        {
            try
            {
                if (row < 0 || row >= this.dim || col < 0 || col >= this.dim)
                {
                    return false; // row and/or column value provided is out of range
                }
                else if (this.currentRow == -1 && this.currentCol == -1)
                {
                    return true; // first piece hasn't been placed yet, any space on the board is valid
                }
                int rowDiff = int.Abs(row - this.currentRow);
                int colDiff = int.Abs(col - this.currentCol);
                if ((((rowDiff == 1) && (colDiff == 2)) || ((rowDiff == 2) && (colDiff == 1))) && this.board[row, col] == Squares.UNVISITED)
                {
                    return true; // moved in L-shape to space on the board that hasn't been visited yet
                }
                else
                {
                    return false; // didn't move in the correct shape
                }
            }
            catch
            {
                return false;
            }
        }

        /**
        *   Function to check if moving the knight to the square at the specified
        *   row and column is valid, then make the move
        *   Returns 1 if the move is valid, 0 if invalid, and -1 on error
        */
        public int Move(int row, int col)
        {
            try
            {
                if (ValidMove(row, col))
                {
                    if (this.currentRow != -1 && this.currentCol != -1)
                    {
                        this.board[this.currentRow, this.currentCol] = Squares.VISITED;
                    }
                    this.currentRow = row;
                    this.currentCol = col;
                    this.board[this.currentRow, this.currentCol] = Squares.KNIGHT;
                    return (1);
                }
                return (0);
            }
            catch
            {
                return (-1);
            }
        }

        /**
        *   Function to return the number of "neighbor" configurations
        *   (valid spaces to move to)
        */
        public int GetNeighborCount()
        {
            try
            {
                int count = 0;
                if (this.currentRow == -1 || this.currentCol == -1)
                {
                    return this.dim * this.dim; // all squares
                }
                if (ValidMove(this.currentRow - 2, this.currentCol - 1)) count++;
                if (ValidMove(this.currentRow - 2, this.currentCol + 1)) count++;
                if (ValidMove(this.currentRow - 1, this.currentCol - 2)) count++;
                if (ValidMove(this.currentRow - 1, this.currentCol + 2)) count++;
                if (ValidMove(this.currentRow + 2, this.currentCol - 1)) count++;
                if (ValidMove(this.currentRow + 2, this.currentCol + 1)) count++;
                if (ValidMove(this.currentRow + 1, this.currentCol - 2)) count++;
                if (ValidMove(this.currentRow + 1, this.currentCol + 2)) count++;
                return count;
            }
            catch
            {
                return 0;
            }
        }

        /**
        *   Function to return a list of "neighbor" configurations where
        *   the knight has been moved to one of the valid spaces
        */
        public List<Board> GetNeighbors()
        {
            try
            {
                List<Board> neighbors = [];
                if (this.currentRow == -1 || this.currentCol == -1)
                {
                    // don't need to check "repeat" cells (cell checked but with the board rotated)
                    for (int i = 0; i <= this.dim / 2; i++)
                    {
                        for (int j = 0; j <= i; j++)
                        {
                            Board neighbor = new(this.dim);
                            neighbor.Move(i, j);
                            neighbors.Add(neighbor);
                        }
                    }
                }
                else
                {
                    Board neighbor = new(this.dim, this.board);
                    if (neighbor.Move(this.currentRow + 1, this.currentCol + 2) == 1)
                    {
                        neighbors.Add(neighbor);
                    }
                    neighbor = new(this.dim, this.board);
                    if (neighbor.Move(this.currentRow + 2, this.currentCol + 1) == 1)
                    {
                        neighbors.Add(neighbor);
                    }
                    neighbor = new(this.dim, this.board);
                    if (neighbor.Move(this.currentRow - 2, this.currentCol + 1) == 1)
                    {
                        neighbors.Add(neighbor);
                    }
                    neighbor = new(this.dim, this.board);
                    if (neighbor.Move(this.currentRow + 2, this.currentCol - 1) == 1)
                    {
                        neighbors.Add(neighbor);
                    }
                    neighbor = new(this.dim, this.board);
                    if (neighbor.Move(this.currentRow - 1, this.currentCol + 2) == 1)
                    {
                        neighbors.Add(neighbor);
                    }
                    neighbor = new(this.dim, this.board);
                    if (neighbor.Move(this.currentRow + 1, this.currentCol - 2) == 1)
                    {
                        neighbors.Add(neighbor);
                    }
                    neighbor = new(this.dim, this.board);
                    if (neighbor.Move(this.currentRow - 2, this.currentCol - 1) == 1)
                    {
                        neighbors.Add(neighbor);
                    }
                    neighbor = new(this.dim, this.board);
                    if (neighbor.Move(this.currentRow - 1, this.currentCol - 2) == 1)
                    {
                        neighbors.Add(neighbor);
                    }
                }
                return neighbors;
                /*
                if (ValidMove(this.currentRow - 2, this.currentCol - 1))
                {
                    Board neighbor = new(this.dim, this.board);
                    neighbor.Move(this.currentRow - 2, this.currentCol - 1);
                    neighbors.Add(neighbor);
                }
                if (ValidMove(this.currentRow - 2, this.currentCol + 1))
                {
                    Board neighbor = new(this.dim, this.board);
                    neighbor.Move(this.currentRow - 2, this.currentCol + 1);
                    neighbors.Add(neighbor);
                }
                if (ValidMove(this.currentRow - 1, this.currentCol - 2))
                {
                    Board neighbor = new(this.dim, this.board);
                    neighbor.Move(this.currentRow - 1, this.currentCol - 2);
                    neighbors.Add(neighbor);
                }
                if (ValidMove(this.currentRow - 1, this.currentCol + 2))
                {
                    Board neighbor = new(this.dim, this.board);
                    neighbor.Move(this.currentRow - 1, this.currentCol + 2);
                    neighbors.Add(neighbor);
                }
                if (ValidMove(this.currentRow + 2, this.currentCol - 1))
                {
                    Board neighbor = new(this.dim, this.board);
                    neighbor.Move(this.currentRow + 2, this.currentCol - 1);
                    neighbors.Add(neighbor);
                }
                if (ValidMove(this.currentRow + 2, this.currentCol + 1))
                {
                    Board neighbor = new(this.dim, this.board);
                    neighbor.Move(this.currentRow + 2, this.currentCol + 1);
                    neighbors.Add(neighbor);
                }
                if (ValidMove(this.currentRow + 1, this.currentCol - 2))
                {
                    Board neighbor = new(this.dim, this.board);
                    neighbor.Move(this.currentRow + 1, this.currentCol - 2);
                    neighbors.Add(neighbor);
                }
                if (ValidMove(this.currentRow + 1, this.currentCol + 2))
                {
                    Board neighbor = new(this.dim, this.board);
                    neighbor.Move(this.currentRow + 1, this.currentCol + 2);
                    neighbors.Add(neighbor);
                }
                
                return neighbors;
                */
            }
            catch
            {
                return [];
            }
        }

        public override string ToString()
        {
            try
            {
                string boardStr = "";
                string border = "-";
                for (int i = 0; i < this.dim; i++)
                {
                    border += "----";
                }
                boardStr += border + "\n";
                for (int i = 0; i < this.dim; i++)
                {
                    boardStr += "|";
                    for (int j = 0; j < this.dim; j++)
                    {
                        switch (this.board[i, j])
                        {
                            case Squares.UNVISITED:
                                boardStr += "   |";
                                break;
                            case Squares.KNIGHT:
                                boardStr += " \u265E |";
                                break;
                            case Squares.VISITED:
                                boardStr += " x |";
                                break;
                        }
                    }
                    boardStr += "\n" + border + "\n";
                }

                return boardStr;
            }
            catch
            {
                return "";
            }
        }

        public int GetCurrentRow()
        {
            return this.currentRow;
        }

        public int GetCurrentCol()
        {
            return this.currentCol;
        }

        public int GetDim()
        {
            return this.dim;
        }
    }
    class KnightsTour
    {
        /**
        *   The main function of the KnightsTour program
        */
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.WriteLine("Welcome to \u2658 The Knight's Tour \u2658 !");
                Console.Write("Enter the length of the game board (5-9): ");
                int dim = Console.Read() - 48; // subtract to go from ascii value to actual dimension
                if (dim < 5 || dim > 9)
                {
                    throw new Exception("Invalid dimension entered.");
                }
                Board board = new(dim);
                bool gameOver = false; // can ask about playing again if game is won/lost
                Console.Clear();
                Console.Write(board.ToString());
                Console.Write(">> ");
                while (!gameOver)
                {
                    string command = Console.ReadLine() ?? "";
                    if (command == "")
                    {
                        continue;
                    }
                    else if (command == "help")
                    {
                        Console.WriteLine("The Knight's Tour is a sequence of moves on a square board such that the knight visits each cell once.");
                        Console.WriteLine("To play this game, the following actions are allowed:");
                        Console.WriteLine("\tmove r c - move the knight to the square at the specified row and column");
                        Console.WriteLine("\t\tr and c should be an integer representing the row and column to move to");
                        Console.WriteLine("\thint - move the knight to the next step in a solution, if one still exists");
                        Console.WriteLine("\treset - resets the current puzzle");
                        Console.WriteLine("\tquit - exits the game");
                    }
                    else if (command.StartsWith("move"))
                    {
                        //parse rest of command to get r, c
                        string[] nums = command.Split(' ');
                        if (nums.Length != 3)
                        {
                            // wrong number of params given for command
                            Console.WriteLine("usage: move r c - r and c should be integers representing the row and column on the board");
                        }
                        else
                        {
                            int r, c;
                            if (int.TryParse(nums[1], out r) && int.TryParse(nums[2], out c))
                            {
                                if (r < 0 || r >= dim || c < 0 || c >= dim)
                                {
                                    Console.WriteLine("usage: move r c - r and c should be integers representing the row and column on the board");
                                }
                                else
                                {
                                    int moved = board.Move(r, c);
                                    if (moved == 1)
                                    {
                                        Console.WriteLine("Successfully moved to ({0}, {1})", r, c);
                                        Console.WriteLine(board.ToString());
                                        if (board.IsSolved())
                                        {
                                            // congratulate player, ask if they want to play again
                                            Console.WriteLine("Congratulations, you won The Knight's Tour! Play again? (y/n)");
                                            int playAgain = Console.Read();
                                            if (playAgain == 'y')
                                            {
                                                Console.Clear();
                                                board.ResetBoard();
                                                Console.WriteLine(board.ToString());
                                            }
                                            else if (playAgain == 'n')
                                            {
                                                Console.WriteLine("Exiting the game...");
                                                gameOver = true;
                                                continue;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid option provided. Exiting the game...");
                                                gameOver = true;
                                                continue;
                                            }
                                        }
                                        else if (board.NoMoves())
                                        {
                                            // print no moves msg, ask if they want to play again
                                            Console.WriteLine("Game over: no more valid moves! Play again? (y/n)");
                                            int playAgain = Console.Read();
                                            if (playAgain == 'y')
                                            {
                                                Console.Clear();
                                                board.ResetBoard();
                                                Console.WriteLine(board.ToString());
                                            }
                                            else if (playAgain == 'n')
                                            {
                                                Console.WriteLine("Exiting the game...");
                                                gameOver = true;
                                                continue;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid option provided. Exiting the game...");
                                                gameOver = true;
                                                continue;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Moving to ({0}, {1}) is an invalid move.", r, c);
                                        Console.WriteLine(board.ToString());
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("usage: move r c - r and c should be integers representing the row and column on the board");
                            }
                        }
                    }
                    else if (command == "hint")
                    {
                        if (Solver.MakeNextMove(ref board) == true)
                        {
                            Console.WriteLine("Moved to ({0}, {1}).", board.GetCurrentRow(), board.GetCurrentCol()); // write that next move was made successfully
                            Console.WriteLine(board.ToString());
                            if (board.IsSolved())
                            {
                                Console.WriteLine("Congratulations, you won The Knight's Tour! Play again? (y/n)");
                                int playAgain = Console.Read();
                                if (playAgain == 'y')
                                {
                                    Console.Clear();
                                    board.ResetBoard();
                                    Console.WriteLine(board.ToString());
                                }
                                else if (playAgain == 'n')
                                {
                                    Console.WriteLine("Exiting the game...");
                                    gameOver = true;
                                    continue;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option provided. Exiting the game...");
                                    gameOver = true;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No more valid moves. Play again? (y/n)");
                            int playAgain = Console.Read();
                            if (playAgain == 'y')
                            {
                                Console.Clear();
                                board.ResetBoard();
                                Console.WriteLine(board.ToString());
                            }
                            else if (playAgain == 'n')
                            {
                                Console.WriteLine("Exiting the game...");
                                gameOver = true;
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Invalid option provided. Exiting the game...");
                                gameOver = true;
                                continue;
                            }
                        }
                    }
                    else if (command == "reset")
                    {
                        Console.WriteLine("Resetting the game...");
                        board.ResetBoard();
                        Console.WriteLine(board.ToString());
                    }
                    else if (command == "quit")
                    {
                        Console.WriteLine("Exiting the game...");
                        gameOver = true;
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Enter 'help' to see a list of valid commands.");
                    }
                    Console.Write(">> ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}