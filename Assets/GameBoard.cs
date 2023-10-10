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
    GameObject[,] stateOfBoard;
    void Start()
    {
        drawBoard();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void drawBoard()
    {
        Vector2 position;
        float spacing = 1.1f;
        for(int i=0; i < boardWidth; i++)
        {
            for(int j =0; j < boardHeight; j++)
            {
                position = new Vector3(i, j, 0) * spacing;
                Instantiate(cell, position , Quaternion.identity);
            }
        }
    }
}
