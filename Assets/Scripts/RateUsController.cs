using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RateUsController : MonoBehaviour
{
    [SerializeField] private RectTransform rateUsPopup;
    [SerializeField] private GameObject backdrop;
    [SerializeField] private float scaleDuration;

    [SerializeField] private Button[] rateStarButton;
    [SerializeField] private Button[] ratedStarButton;

    private int rateStar;
    private bool isRateUsShow;
    private void Start(){
        DisableRatedStar();
        for (var i = 0; i < rateStarButton.Length; i++) {
            var index = i;
            rateStarButton[index].interactable = true;
            rateStarButton[index].onClick.AddListener(() => {RateUs(index, rateStarButton[index]); });
        }
        for (var i = 0; i < ratedStarButton.Length; i++) {
            var index = i;
            ratedStarButton[index].interactable = true;
            ratedStarButton[index].onClick.AddListener(() => {UnRateUs(index, rateStarButton[index]); });
        }
    }

    private void DisableRatedStar(){
        for(int i = 0; i < ratedStarButton.Length; i++){
            ratedStarButton[i].gameObject.SetActive(false);
        }
    }
    public void TurnOnRateUSPopUp(){
        if (!isRateUsShow) {
            isRateUsShow = true;
            rateUsPopup.gameObject.SetActive(true);
            backdrop.SetActive(true);
            rateUsPopup.DOScale(Vector3.one, scaleDuration).From(Vector3.zero);
        } else {
            TurnOffRateUsPopUp();
        }
    }

    public void TurnOffRateUsPopUp(){
        isRateUsShow = false;
        backdrop.SetActive(false);
        rateUsPopup.gameObject.SetActive(false);
        DisableRatedStar()  ;
    }

    private void RateUs(int index, Button button){
        Debug.Log("Rate Us");
        
        for(int i = 0; i <= index; i++){
            ratedStarButton[i].gameObject.SetActive(true);
        }

        rateStar = index;                    
    }

    private void UnRateUs(int index, Button button){
        Debug.Log("Rate Us");
        
        DisableRatedStar()  ;
        
        RateUs(index, rateStarButton[index]);

        rateStar = index;                 
    }
    public void ClickRateUs(){
        if(rateStar == 3 || rateStar == 4){
           
            Application.OpenURL("https://img.freepik.com/free-vector/thank-you-placard-concept-illustration_114360-13436.jpg");
        }
        
        TurnOffRateUsPopUp();
    }

}
