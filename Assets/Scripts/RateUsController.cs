using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RateUsController : MonoBehaviour
{
    [SerializeField] private RectTransform rateUsPopup;
    [SerializeField] private GameObject backdrop;
    [SerializeField] private float scaleDuration;

    private bool isRateUsShow;
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
    }
}
