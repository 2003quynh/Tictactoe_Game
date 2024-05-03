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
        Debug.Log("Set up 1");
        currLevel = 0;
        difficultyLevel = 0;
        Debug.Log("Set up 2");
        simpleScrollSnap.OnPanelCentered.AddListener(GetCurrentLevel);
        Debug.Log("Set up 3");
    }

    private void GetCurrentLevel(int centeredPanel, int selectedPanel){
        Debug.Log("Set up 4");
        currLevel = centeredPanel;
    }

    private void ChooseLevel(){
        Debug.Log("Set up 5");

        XOLevelTables[currLevel].SetActive(true);
        LevelBanes[currLevel].SetActive(true);
        Debug.Log("Set up 6");

    }

    public void ConvertModes(){
        Debug.Log("Set up 7");

        TurnOffPopUpDifficulty();
        Debug.Log("Set up 8");
        switch (currMode) {
            case 0:
                ConvertPCMode();
                Debug.Log("Set up 9");
                break;
            case 1:
                ConvertPPMode();
                Debug.Log("Set up 10");
                break;
            case 2:
                ConvertCampaignMode();
                Debug.Log("Set up 11");
                break;
        }
    }

    public void ClickOnPCMode(){
        Debug.Log("Set up 12");

        currMode = 0;
        TurnOnPopUpDifficulty();
        Debug.Log("Set up 13");

    }

    public void ClickOnPPMode(){
        Debug.Log("Set up 14");

        currMode = 1;
        TurnOnPopUpDifficulty();
        Debug.Log("Set up 15");

    }

    public void ClickOnCampaignMode(){
        currMode = 2;
        ConvertModes();
    }

    public void ConvertPCMode(){
        Debug.Log("Set up 16");

        XOScreens[0].gameObject.SetActive(false);
        XOScreens[1].gameObject.SetActive(true);
        ChooseLevel();
        Debug.Log("Set up 17");

        gameController.withAI = true;
    }

    public void ConvertPPMode(){
        Debug.Log("Set up 18");

        XOScreens[0].gameObject.SetActive(false);
        XOScreens[1].gameObject.SetActive(true);
        ChooseLevel();
        Debug.Log("Set up 19");

        gameController.withAI = false;
    }

    public void ConvertCampaignMode(){
        XOScreens[0].gameObject.SetActive(false);
        XOScreens[2].gameObject.SetActive(true);
    }

    public void TurnOnPopUpDifficulty(){
        Debug.Log("Set up 20");

        if (!isDifficultyShow) {
        Debug.Log("Set up 21");

            isDifficultyShow = true;
            difficultPopup.gameObject.SetActive(true);
            backdrop.SetActive(true);
            difficultPopup.DOScale(Vector3.one, scaleDuration).From(Vector3.zero);
        Debug.Log("Set up 22");

        } else {
            TurnOffPopUpDifficulty();
        }
    }


    public void TurnOffPopUpDifficulty(){
        Debug.Log("Set up 23");

        isDifficultyShow = false;
        backdrop.SetActive(false);
        difficultPopup.gameObject.SetActive(false);
        Debug.Log("Set up 24");

    }
}