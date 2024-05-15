using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class TicTacToeController : MonoBehaviour
{
    
    [SerializeField] private Transform mainMenu, tictactoeMenu;
    
    [SerializeField] private RectTransform btnPvsC, btnPvsP;
    [SerializeField] private Vector3 btnPvsCPosition, btnPvsPPosition;
    [SerializeField] private float moveDuration;
    public void OpenTicTacToeGame(){
        mainMenu.GameObject().SetActive(false);
        tictactoeMenu.GameObject().SetActive(true);

        btnPvsP.DOLocalMove(btnPvsPPosition, moveDuration);
        btnPvsC.DOLocalMove(btnPvsCPosition, moveDuration);

    }
}
