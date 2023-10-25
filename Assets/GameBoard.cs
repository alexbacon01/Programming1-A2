using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum Rules
{
    Underpopulation,
    NextGen,
    OverPopulation,
    Reproduction
};
public class GameBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public int boardWidth = 20;
    public int boardHeight = 10;
    public GameObject cell;
    public GameObject[,] stateOfBoard;
    public float spacing = 1.5f;
    private Rules activeRule;

    void Start()
    {
        UnityEngine.Cursor.visible = true;
        stateOfBoard = new GameObject[boardWidth, boardHeight];
        drawBoard();
        drawBackground();
    }

    // Update is called once per frame
    void Update()
    {
        changeState();
    }

    private void drawBoard()
    {
        Vector2 position;
        int numOfCells = 0; //used for naming the cells

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                position = new Vector3(i - boardWidth / 2, j - boardHeight / 2, 0) * spacing;
                numOfCells++;
                GameObject newCell = makeNewCell(numOfCells, position);
                stateOfBoard[i, j] = newCell;
                newCell.GetComponent<Cell>().setPos(i, j);
            }

        }
        for (int i = 0; i < boardWidth; i++) //loop to go and find neighbours had to be seprate so that all the neighbours are also initialized.
        {
            //Debug.Log(i + findNeighbours(stateOfBoard[19, 0])[i].name);
            for (int j = 0; j < boardHeight; j++)
            {
                GameObject currentCell = stateOfBoard[i, j];
                currentCell.GetComponent<Cell>().setNeighbours(findNeighbours(currentCell));
            }


        }


        Debug.Log(stateOfBoard[3, 4].GetComponent<Cell>().getAliveNeighbours());
    }
    private void drawBackground()
    {
        Vector2 position = new Vector2(19, 3);
        transform.position = position;
    }




    private GameObject makeNewCell(int numOfCells, Vector2 pos)
    {
        GameObject newCell = Instantiate(cell, pos, Quaternion.identity);
        newCell.GetComponent<Cell>().setState(CellState.Dead);
        newCell.gameObject.name = "cell" + numOfCells;
        return newCell;
    }

    private void checkRules(GameObject cell)
    {
        int aliveNeighbours = cell.GetComponent<Cell>().getAliveNeighbours();

        if (cell.GetComponent<Cell>().getState() == CellState.Alive)
        {
            if (aliveNeighbours < 2)
            {
                activeRule = Rules.Underpopulation;
            }
            else if (aliveNeighbours > 1 && aliveNeighbours < 4)
            {
                activeRule = Rules.NextGen;
            }
            else if (aliveNeighbours > 3)
            {
                activeRule = Rules.OverPopulation;
            }
        }
        else
        {
            if (aliveNeighbours == 3)
            {
                activeRule = Rules.Reproduction;
            }
        }

    }
    private GameObject[] findNeighbours(GameObject cell)
    {
        int rowPos = cell.GetComponent<Cell>().getRowPos();
        int colPos = cell.GetComponent<Cell>().getColPos();
        GameObject[] neighbours = new GameObject[8];
        bool colEdgeCase = false;
        bool rowEdgeCase = false;
        /*
        GameObject[] neighbours = {
        stateOfBoard[colPos, rowPos - 1], // cell under [0]
        stateOfBoard[colPos, rowPos + 1], //cell above [1]
        stateOfBoard[colPos - 1, rowPos], //cell on left [2]
        stateOfBoard[colPos + 1, rowPos], //cell on right [3]
        stateOfBoard[colPos - 1, rowPos - 1], //cell left and down [4]
        stateOfBoard[colPos + 1, rowPos + 1], //cell right and up [5]
        stateOfBoard[colPos - 1, rowPos + 1], //cell left and up [6] 
        stateOfBoard[colPos + 1, rowPos - 1] //cell right and down [7]
    };
        
        neighbours[0] = stateOfBoard[colPos, rowPos - 1]; // cell under [0]
        neighbours[1] = stateOfBoard[colPos, rowPos + 1]; //cell above [1]
        neighbours[2] = stateOfBoard[colPos - 1, rowPos]; //cell on left [2]
        neighbours[3] = stateOfBoard[colPos + 1, rowPos]; //cell on right [3]
        neighbours[4] = stateOfBoard[colPos - 1, rowPos - 1]; //cell left and down [4]
        neighbours[5] = stateOfBoard[colPos + 1, rowPos + 1]; //cell right and up [5]
        neighbours[6] = stateOfBoard[colPos - 1, rowPos + 1]; //cell left and up [6]
        neighbours[7] = stateOfBoard[colPos + 1, rowPos - 1]; //cell right and down [7]
        
        */

        //set Edge Cases
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

    private void changeState()
    {

        if (mouseClicked() != null)
        {
            CellState currentState = mouseClicked().GetComponent<Cell>().getState();
            CellState newState;
            if (currentState == CellState.Alive)
            {
                newState = CellState.Dead;
            }
            else
            {
                newState = CellState.Alive;
            }
            mouseClicked().GetComponent<Cell>().setState(newState);
        }

    }
}

