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
    [SerializeField] private GameObject[] sounds, vibrations;

    private bool _isOpenSound;
    private bool _isOpenVibration;
   
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

    public void ChangeSoundState(){
        if (!_isOpenSound) {
            sounds[0].SetActive(false);
            sounds[1].SetActive(true);
            _isOpenSound = true;
        }
        else {
            sounds[1].SetActive(false);
            sounds[0].SetActive(true);
            _isOpenSound = false;
        }
    }

    public void ChangeVibrationState(){
        if (!_isOpenVibration) {
            vibrations[0].SetActive(false);
            vibrations[1].SetActive(true);
            _isOpenVibration = true;
        }
        else {
            vibrations[1].SetActive(false);
            vibrations[0].SetActive(true);
            _isOpenVibration = false;
        }
    }
}
