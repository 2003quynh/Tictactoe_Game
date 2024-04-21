using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// public enum Level{
//     Three,
//     Six,
//     Nine,
//     Eleven,
//     Empty
// }

public class SetUpXOController : MonoBehaviour{
    [SerializeField] private GameObject[] XOScreens, XOLevelTables, LevelBanes;
    [SerializeField] private GameController gameController;
    [SerializeField] private int maxLevel;
    [SerializeField] private SimpleScrollSnap simpleScrollSnap;
    [SerializeField] private RectTransform difficultPopup;
    [SerializeField] private float scaleDuration;
    [SerializeField] private Button[] difficultyButton; //0: easy, 1: medium, 2: hard
    [SerializeField] private DifficullyController difficultyController;
    [SerializeField] private GameObject backdrop;


    public int currLevel; //0: 3; 1: 6; 2: 9; 3: 11
    public int currMode; //0: PC; 1: PP; 2: Campaign
    public int difficultyLevel; //0: easy; 1: medium; 2: hard
    private bool isDifficultyShow;
    private Vector3 targetPos;

    private void Start(){
        currLevel = 0;
        difficultyLevel = 0;
        simpleScrollSnap.OnPanelCentered.AddListener(GetCurrentLevel);
    }

    private void GetCurrentLevel(int centeredPanel, int selectedPanel){
        currLevel = centeredPanel;
    }

    private void ChooseLevel(){
        XOLevelTables[currLevel].SetActive(true);
        LevelBanes[currLevel].SetActive(true);
    }

    public void ConvertModes(){
        TurnOffPopUpDifficulty();
        switch (currMode) {
            case 0:
                ConvertPCMode();
                break;
            case 1:
                ConvertPPMode();
                break;
            case 2:
                ConvertCampaignMode();
                break;
        }
    }

    public void ClickOnPCMode(){
        currMode = 0;
        TurnOnPopUpDifficulty();
    }

    public void ClickOnPPMode(){
        currMode = 1;
        TurnOnPopUpDifficulty();
    }

    public void ClickOnCampaignMode(){
        currMode = 2;
        ConvertModes();
    }

    public void ConvertPCMode(){
        XOScreens[0].gameObject.SetActive(false);
        XOScreens[1].gameObject.SetActive(true);
        ChooseLevel();

        gameController.withAI = true;
    }

    public void ConvertPPMode(){
        XOScreens[0].gameObject.SetActive(false);
        XOScreens[1].gameObject.SetActive(true);
        ChooseLevel();

        gameController.withAI = false;
    }

    public void ConvertCampaignMode(){
        XOScreens[0].gameObject.SetActive(false);
        XOScreens[2].gameObject.SetActive(true);
    }

    public void TurnOnPopUpDifficulty(){
        if (!isDifficultyShow) {
            isDifficultyShow = true;
            difficultPopup.gameObject.SetActive(true);
            backdrop.SetActive(true);
            difficultPopup.DOScale(Vector3.one, scaleDuration).From(Vector3.zero);
        } else {
            TurnOffPopUpDifficulty();
        }
    }


    public void TurnOffPopUpDifficulty(){
        isDifficultyShow = false;
        backdrop.SetActive(false);
        difficultPopup.gameObject.SetActive(false);
    }
}