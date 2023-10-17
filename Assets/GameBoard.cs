using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public int boardWidth = 50;
    public int boardHeight = 25;
    public GameObject cell;
    public GameObject[,] stateOfBoard;
    public float spacing = 1.5f;
 
    void Start()
    {
        stateOfBoard = new GameObject[boardWidth, boardHeight];
        drawBoard();
        drawBackground();
    }

    // Update is called once per frame
    void Update()
    {
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
                newCell.GetComponent<Cell>().setNeighbours(findNeighbours(newCell));
            }
        }

        Debug.Log(stateOfBoard[1, 1].GetComponent<Cell>().getNeighbours());
    }
    private void drawBackground()
    {
        Vector2 position = new Vector2(0, 0);
        transform.position = position;
    }

    private void checkRules(GameObject cell)
    {

    }

    private GameObject makeNewCell(int numOfCells, Vector2 pos)
    {
        GameObject newCell = Instantiate(cell, pos, Quaternion.identity);
        newCell.GetComponent<Cell>().setState(CellState.Dead);
        newCell.gameObject.name = "cell" + numOfCells;
        return newCell;
    }

    private GameObject[] findNeighbours(GameObject cell)
    {
        int rowPos = cell.GetComponent<Cell>().getRowPos();
        int colPos = cell.GetComponent<Cell>().getColPos();
        GameObject[] neighbours = {
        stateOfBoard[colPos, rowPos - 1],
        stateOfBoard[colPos, rowPos + 1],
        stateOfBoard[colPos - 1, rowPos],
        stateOfBoard[colPos + 1, rowPos],
        stateOfBoard[colPos - 1, rowPos - 1],
        stateOfBoard[colPos + 1, rowPos + 1],
        stateOfBoard[colPos - 1, rowPos + 1],
        stateOfBoard[colPos + 1, rowPos - 1]
    };
        return neighbours;
    }
}
