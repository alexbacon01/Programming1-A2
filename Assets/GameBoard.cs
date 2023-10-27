using UnityEngine;
using UnityEngine.UI;

//enums for the rules of life in simulation
public enum Rules
{
    Underpopulation,
    NextGen,
    OverPopulation,
    Reproduction,
    NoChange
};

public enum Stamps
{
    Oscillator,
    Spaceship
};
public class GameBoard : MonoBehaviour
{
    [Header("Game settings: ")]
    [Range(10, 50)]
    public int boardWidth = 20; //board width / number of columns in game, in a range between 10 and 50
    [Range(1, 4)]
    private int frameRate = 1; //framerate for game is in a range between 1 and 4
    private int boardHeight; //board height is dependant on boardWidth
    private float cellSize;
    private float spacing = 1.5f;

    [Header("Game objects: ")]
    public GameObject cell;
    public GameObject[,] stateOfBoard;
    public GameObject mainCamera;
    public UnityEngine.UI.Slider FPSSlider;

    [Header("Variables for state of game: ")]
    private bool gameRunning = false;
    private int generation;
    public Text generationText;
    private bool stamp = false;
    Stamps stampShape;


    void Start()
    {
        //inital game settings
        boardHeight = boardWidth / 2;
        UnityEngine.Cursor.visible = true;
        stateOfBoard = new GameObject[boardWidth, boardHeight];
        cellSize = ((mainCamera.GetComponent<Camera>().orthographicSize) / boardWidth) * 2;

        Debug.Log(cellSize);

        drawBoard(); //draw the initial board

    }

    void Update()
    {
        changeState(); //change state of cells manually using mouse clicks
        generationText.text = "Current Generation: " + generation.ToString(); //set text on screen to match current generation

        if (gameRunning)  //if game is running move to next generation
        {
            nextGen(); //changes the rule state and updates the generation count
            Application.targetFrameRate = frameRate; //update the target frame rate to match the slider framerate
        }

    }

    //game settings methods

    /* public accessible method for start button to be able to toggle the gameRunning variable */
    public void toggleGameRunning()
    {
        gameRunning = !gameRunning;
    }

    /* A method to update frame rate with the slider in game */
    public void changeFPS()
    {
        frameRate = (int)FPSSlider.value;
        Debug.Log(frameRate);
    }

    /* updates the generation by rechecking the rule state and adding to the generation count */
    private void nextGen()
    {
        changeRuleState();
        generation++;
    }

    /* public method for clearing the game on the clear button, destroys old board, resets generation back to 0, and draws a new board */
    public void clearGame()
    {
        generation = 0;
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                Destroy(stateOfBoard[i, j].gameObject);
            }
        }
        drawBoard();
    }

    /* method for finding what object the mouse clicked */
    private GameObject mouseClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;

            }
        }
        return null;
    }

    //creating game and cells methods
    /* method for drawing the board and adding it to the stateOfBoard array also calls find neighbours to set the cells neighbours*/
    private void drawBoard()
    {
        Vector2 position;
        int numOfCells = 0; //used for naming the cells
        float spacing = cellSize * 1.2f; //sets spacing for the cells
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                position = new Vector3(i - boardWidth / 2, j - boardHeight / 2, 0) * spacing; //sets position for each cell
                numOfCells++; //variable to keep track of number of cells (for naming)
                GameObject newCell = makeNewCell(numOfCells, position); //creates a new gameObject of a cell using the makeNewCell function
                stateOfBoard[i, j] = newCell; //adds the new cell to the stateOfBoard array
                newCell.GetComponent<Cell>().setPos(i, j); //sets position of cell.
            }
        }

        for (int i = 0; i < boardWidth; i++) //loop to go and find neighbours had to be seprate so that all the neighbours are already initialized.
        {
            for (int j = 0; j < boardHeight; j++)
            {
                GameObject currentCell = stateOfBoard[i, j];
                currentCell.GetComponent<Cell>().setNeighbours(findNeighbours(currentCell)); //set neighbours for every cell in array
            }
        }
    }

    /*a function to instantiate a new cell and set state, size and name*/
    private GameObject makeNewCell(int numOfCells, Vector2 pos)
    {
        GameObject newCell = Instantiate(cell, pos, Quaternion.identity);
        newCell.GetComponent<Cell>().setState(CellState.Dead);
        newCell.GetComponent<Transform>().localScale = new Vector3(cellSize, cellSize, cellSize);
        newCell.gameObject.name = "cell" + numOfCells;
        return newCell;
    }


    //change board state methods

    /* a method to check rules and returns which rule is currrently active */
    private Rules checkRules(GameObject cell)
    {
        int aliveNeighbours = cell.GetComponent<Cell>().getAliveNeighbours(); //number of alive neighbours
        Rules activeRule = Rules.NoChange; //set initially to no change

        if (cell.GetComponent<Cell>().getState() == CellState.Alive) //if cell is alive
        {
            if (aliveNeighbours < 2) //if less than 2 alive neighbours
            {
                activeRule = Rules.Underpopulation; //underpopulation
            }
            if (aliveNeighbours > 1 && aliveNeighbours < 4) //if more than 1 neighbour and less than 4 
            {
                activeRule = Rules.NextGen; //Next gen
            }
            if (aliveNeighbours > 3) //if more than 3 neighbours
            {
                activeRule = Rules.OverPopulation; //Overpopulation
            }
        }
        else //if cell is dead
        {
            if (aliveNeighbours == 3) //if 3 neighbours
            {
                activeRule = Rules.Reproduction; //Reproduction
            }
        }
        return activeRule; //return active rule
    }

    /*a method to stamp a preset onto the board*/
    private void stampPreset()
    {
        if (stamp)
        {
            if(stampShape == Stamps.Oscillator)
            {

            } else if (stampShape == Stamps.Spaceship)
            {

            }
        }
    }

    /* method to changeState of cell manually with mouse clicks*/
    private void changeState()
    {
        CellState newState;
        if (mouseClicked() != null)
        {
            CellState currentState = mouseClicked().GetComponent<Cell>().getState(); //gets current state
            if (currentState == CellState.Alive) //sets cell state to opposite of whatever it is
            {
                newState = CellState.Dead;
            }
            else
            {
                newState = CellState.Alive;
            }

            mouseClicked().GetComponent<Cell>().setState(newState); //set the new state of the cell

            if (gameRunning == false)
            {
                recountNeighbours(); //recount alive neighbours
            }
        }
    }

    /* method to change the state of the baord based on the rule set by check rules */
    private void changeRuleState()
    {
        Rules currentRule = Rules.NoChange;

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                currentRule = checkRules(stateOfBoard[i, j]);

                //changes state of cell based on the current rule 
                if (currentRule == Rules.Underpopulation)
                {
                    stateOfBoard[i, j].GetComponent<Cell>().setState(CellState.Dead);

                }
                else if (currentRule == Rules.NextGen)
                {
                    stateOfBoard[i, j].GetComponent<Cell>().setState(CellState.Alive);
                }
                else if (currentRule == Rules.OverPopulation)
                {
                    stateOfBoard[i, j].GetComponent<Cell>().setState(CellState.Dead);
                }
                else if (currentRule == Rules.Reproduction)
                {
                    stateOfBoard[i, j].GetComponent<Cell>().setState(CellState.Alive);
                }
            }

        }
        recountNeighbours(); //recount alive neighbours 
    }

    //neighbour methods

    /* method to recount alive neighbours */
    private void recountNeighbours()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                stateOfBoard[i, j].GetComponent<Cell>().setAliveNeighbours();
            }
        }
    }
    /* Method for finding the array of neighbours for each cell, cells wrap to other side of the board*/
    private GameObject[] findNeighbours(GameObject cell)
    {
        int rowPos = cell.GetComponent<Cell>().getRowPos(); //get row position from cell
        int colPos = cell.GetComponent<Cell>().getColPos(); //get col position from cell

        GameObject[] neighbours = new GameObject[8]; //array of neighbours

        //bools for edge cases
        bool colEdgeCase = false;
        bool rowEdgeCase = false;

        if (colPos == 0 || colPos == boardWidth - 1)
        {
            colEdgeCase = true;
        }
        if (rowPos == 0 || rowPos == boardHeight - 1)
        {
            rowEdgeCase = true;
        }

        if (colEdgeCase && colPos == 0 && !rowEdgeCase)
        {
            neighbours[2] = stateOfBoard[boardWidth - 1, rowPos]; // cell left [2] wraps around to other side of board //YES
            neighbours[3] = stateOfBoard[colPos + 1, rowPos]; //cell on right [3] YES
            neighbours[4] = stateOfBoard[boardWidth - 1, rowPos - 1]; //cell left and down [4] wraps to other side //YES
            neighbours[5] = stateOfBoard[colPos + 1, rowPos + 1]; //cell right and up [5]
            neighbours[6] = stateOfBoard[boardWidth - 1, rowPos + 1]; //cell left and up [6] wraps to other side
            neighbours[7] = stateOfBoard[colPos + 1, rowPos - 1]; //cell right and down [7]
        }
        else if (colEdgeCase && colPos == boardWidth - 1 && !rowEdgeCase)
        {

            neighbours[2] = stateOfBoard[colPos - 1, rowPos]; //cell on left [2]
            neighbours[3] = stateOfBoard[0, rowPos]; //cell on right [3] YES
            neighbours[4] = stateOfBoard[colPos - 1, rowPos - 1]; //cell left and down [4]
            neighbours[5] = stateOfBoard[0, rowPos + 1]; //cell right and up [5]
            neighbours[6] = stateOfBoard[colPos - 1, rowPos + 1]; //cell left and up [6]
            neighbours[7] = stateOfBoard[0, rowPos - 1]; //cell right and down [7]

        }

        if (rowEdgeCase && rowPos == 0 && !colEdgeCase)
        {
            neighbours[0] = stateOfBoard[colPos, boardHeight - 1]; // cell under [0] wraps to top of board
            neighbours[1] = stateOfBoard[colPos, rowPos + 1]; //cell above [1]
            neighbours[4] = stateOfBoard[colPos - 1, boardHeight - 1]; //cell left and down [4]
            neighbours[5] = stateOfBoard[colPos + 1, rowPos + 1]; //cell right and up [5]
            neighbours[6] = stateOfBoard[colPos - 1, rowPos + 1]; //cell left and up [6]
            neighbours[7] = stateOfBoard[colPos + 1, boardHeight - 1]; //cell right and down [7]


        }
        else if (rowEdgeCase && rowPos == boardHeight - 1 && !colEdgeCase)
        {
            neighbours[0] = stateOfBoard[colPos, rowPos - 1]; // cell under [0]
            neighbours[1] = stateOfBoard[colPos, 0]; // cell above[1] wraps around to bottom of the board
            neighbours[4] = stateOfBoard[colPos - 1, rowPos - 1]; //cell left and down [4]
            neighbours[5] = stateOfBoard[colPos + 1, 0]; //cell right and up [5]
            neighbours[6] = stateOfBoard[colPos - 1, 0]; //cell left and up [6]
            neighbours[7] = stateOfBoard[colPos + 1, rowPos - 1]; //cell right and down [7]
        }

        if (!colEdgeCase)
        {
            neighbours[2] = stateOfBoard[colPos - 1, rowPos]; //cell on left [2]
            neighbours[3] = neighbours[3] = stateOfBoard[colPos + 1, rowPos]; //cell on right [3]

            if (!rowEdgeCase)
            {
                neighbours[4] = stateOfBoard[colPos - 1, rowPos - 1]; //cell left and down [4] 
                neighbours[5] = stateOfBoard[colPos + 1, rowPos + 1]; //cell right and up [5]
                neighbours[6] = stateOfBoard[colPos - 1, rowPos + 1]; //cell left and up [6]
                neighbours[7] = stateOfBoard[colPos + 1, rowPos - 1]; //cell right and down [7]
            }
        }

        if (!rowEdgeCase)
        {
            neighbours[0] = stateOfBoard[colPos, rowPos - 1]; // cell under [0] YES
            neighbours[1] = stateOfBoard[colPos, rowPos + 1]; //cell above [1] YES

        }

        if (rowEdgeCase && colEdgeCase)
        {
            if (colPos == 0 && rowPos == 0)
            {
                neighbours[0] = stateOfBoard[colPos, boardHeight - 1]; // cell under [0] wraps to top of board
                neighbours[1] = stateOfBoard[colPos, rowPos + 1]; //cell above [1]
                neighbours[2] = stateOfBoard[boardWidth - 1, rowPos]; // cell left [2] wraps around to other side of board //YES
                neighbours[3] = stateOfBoard[colPos + 1, rowPos]; //cell on right [3] YES
                neighbours[4] = stateOfBoard[boardWidth - 1, boardHeight - 1]; //cell left and down [4]
                neighbours[5] = stateOfBoard[colPos + 1, rowPos + 1]; //cell right and up [5]
                neighbours[6] = stateOfBoard[boardWidth - 1, rowPos + 1]; //cell left and up [6]
                neighbours[7] = stateOfBoard[colPos + 1, boardHeight - 1]; //cell right and down [7]
            }
            else if (colPos == boardWidth - 1 && rowPos == boardHeight - 1)
            {
                neighbours[0] = stateOfBoard[colPos, rowPos - 1]; // cell under [0] YES
                neighbours[1] = stateOfBoard[colPos, 0]; // cell above[1] wraps around to bottom of the board
                neighbours[2] = stateOfBoard[colPos - 1, rowPos]; //cell on left [2]
                neighbours[3] = stateOfBoard[0, rowPos]; //cell on right [3]
                neighbours[4] = stateOfBoard[colPos - 1, rowPos - 1]; //cell left and down [4]
                neighbours[5] = stateOfBoard[0, 0]; //cell right and up [5]
                neighbours[6] = stateOfBoard[colPos - 1, 0]; //cell left and up [6]
                neighbours[7] = stateOfBoard[0, rowPos - 1]; //cell right and down [7]
            }
            else if (colPos == 0 && rowPos == boardHeight - 1)
            {
                neighbours[0] = stateOfBoard[colPos, rowPos - 1]; // cell under [0] YES
                neighbours[1] = stateOfBoard[colPos, 0]; // cell above[1] wraps around to bottom of the board
                neighbours[2] = stateOfBoard[boardWidth - 1, rowPos]; // cell left [2] wraps around to other side of board //YES
                neighbours[3] = stateOfBoard[colPos + 1, rowPos]; //cell on right [3] YES
                neighbours[4] = stateOfBoard[boardWidth - 1, rowPos - 1]; //cell left and down [4] wraps to other side //YES
                neighbours[5] = stateOfBoard[colPos + 1, 0]; //cell right and up [5]
                neighbours[6] = stateOfBoard[boardWidth - 1, 0]; //cell left and up [6] wraps to other side
                neighbours[7] = stateOfBoard[colPos + 1, rowPos - 1]; //cell right and down [7]
            }
            else if (colPos == boardWidth - 1 && rowPos == 0)
            {
                neighbours[0] = stateOfBoard[colPos, boardHeight - 1]; // cell under [0] wraps to top of board
                neighbours[1] = stateOfBoard[colPos, rowPos + 1]; //cell above [1]
                neighbours[2] = stateOfBoard[colPos - 1, rowPos]; //cell on left [2]
                neighbours[3] = stateOfBoard[0, rowPos]; //cell on right [3] YES
                neighbours[4] = stateOfBoard[colPos - 1, boardHeight - 1]; //cell left and down [4]
                neighbours[5] = stateOfBoard[0, rowPos + 1]; //cell right and up [5]
                neighbours[6] = stateOfBoard[colPos - 1, rowPos + 1]; //cell left and up [6]
                neighbours[7] = stateOfBoard[0, boardHeight - 1]; //cell right and down [7]
            }
        }
        return neighbours;
    }
}

