using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

        for(int i=0; i < boardWidth; i++)
        {
            for(int j =0; j < boardHeight; j++)
            {
                position = new Vector3(i-boardWidth/2, j-boardHeight/2, 0) * spacing;
                GameObject newCell = Instantiate(cell, position , Quaternion.identity);
                newCell.AddComponent<Cell>();   
               // stateOfBoard[i, j] = newCell;
                newCell.GetComponent<Cell>().setState(CellState.Dead);
                newCell.gameObject.name = "cell" + i+j;

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
}
