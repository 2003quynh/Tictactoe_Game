using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class MenuController : MonoBehaviour{
    [SerializeField] private GameObject[] game;
 
    private TicTacToeController ticTacToeController;

    public void ConvertToTicTacToeGame(){
        ticTacToeController.OpenTicTacToeGame();
    }

    

    
}