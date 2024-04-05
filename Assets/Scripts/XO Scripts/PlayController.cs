using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayController : MonoBehaviour
{
    public int whoTurn;
    public int turnCount;
    public GameObject[] turnIcons;
    public Material[] materials;
    public Button[] tictactoeSpaces;
    // Start is called before the first frame update
    void Start()
    {
        whoTurn = 0;
        turnCount = 0;
        turnIcons = new GameObject[turnCount];
               
    }

    // Update is called once per frame
    void Update()
    {
        tictactoeSpaces[1].image.material = materials[whoTurn];
    }
}
