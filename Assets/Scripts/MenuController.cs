using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class MenuController : MonoBehaviour{
    [SerializeField] private RectTransform settingPopup;
    [SerializeField] private GameObject[] game;
    [SerializeField] private Transform mainMenu;
    [SerializeField] private GameObject[] sound;
    [SerializeField] private GameObject[] vibration;
    [SerializeField] private RectTransform btnPvsC, btnPvsP;
    [SerializeField] private GameObject backdrop;
    [SerializeField] private Vector3 btnPvsCPosition, btnPvsPPosition;
    [SerializeField] private float scaleDuration, moveDuration;

    private bool _isSettingShow;
    private Vector2 _originalSettingPanelPos;

    private int soundState;
    private int vibrationState;

    private void Start(){
        _originalSettingPanelPos = settingPopup.anchoredPosition;
    }

    public void TurnOnPopUpSetting(){
        if (!_isSettingShow) {
            _isSettingShow = true;
            settingPopup.gameObject.SetActive(true);
            backdrop.SetActive(true);
            settingPopup.DOScale(Vector3.one, scaleDuration).From(Vector3.zero);
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

    public void OpenTicTacToeGame(){
        mainMenu.GameObject().SetActive(false);

        btnPvsP.DOLocalMove(btnPvsPPosition, moveDuration);
        btnPvsC.DOLocalMove(btnPvsCPosition, moveDuration);

        game[0].SetActive(true);
    }

    public void ChangeSoundState(){
        if (soundState == 0) {
            sound[0].SetActive(false);
            sound[1].SetActive(true);
            soundState = 1;
        }
        else {
            sound[1].SetActive(false);
            sound[0].SetActive(true);
            soundState = 0;
        }
    }

    public void ChangeVibrationState(){
        if (vibrationState == 0) {
            vibration[0].SetActive(false);
            vibration[1].SetActive(true);
            vibrationState = 1;
        }
        else {
            vibration[1].SetActive(false);
            vibration[0].SetActive(true);
            vibrationState = 0;
        }
    }
}