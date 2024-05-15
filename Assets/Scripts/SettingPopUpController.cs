using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SettingPopUpController : MonoBehaviour
{   
    private bool _isSettingShow;
    [SerializeField] private float scaleDuration;
    [SerializeField] private GameObject backdrop;
    [SerializeField] private RectTransform settingPopup;
   
    void Awake(){
        _isSettingShow = true;
    }
    
    public void TurnOnPopUpSetting(){
        if (!_isSettingShow) {
            _isSettingShow = true;
            settingPopup.gameObject.SetActive(true);
            backdrop.SetActive(true);
            settingPopup.transform.DOScale(Vector3.one, scaleDuration).From(Vector3.zero);
        }
        else {
            TurnOffPopUpSetting();
        }
    }

    public void TurnOffPopUpSetting(){
        _isSettingShow = false;
        backdrop.SetActive(false);
        settingPopup.gameObject.SetActive(false);
    }
}
