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

        for(int i=0; i < boardWidth; i++)
        {
            for(int j =0; j < boardHeight; j++)
            {
                position = new Vector3(i - boardWidth / 2, j - boardHeight / 2, 0) * spacing;
                numOfCells++;
                stateOfBoard[i, j] = makeNewCell(numOfCells, position) ;
            }
        }
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
}
