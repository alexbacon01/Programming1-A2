using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLineUp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GameBoard;
    void Start()
    {
        AlignWithCells();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AlignWithCells()
    {
        int height = GameBoard.GetComponent<GameBoard>().boardHeight;
        int width = GameBoard.GetComponent<GameBoard>().boardWidth;

        Vector3 position = new Vector3(0, height , 0);
        transform.position = position;
    }
}
