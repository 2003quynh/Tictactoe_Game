using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SetUpXOController : MonoBehaviour{

    
    [SerializeField] private GameObject[] XOScreens, XOLevelTables, LevelBanes;    
    [SerializeField] private int maxLevel;
    [SerializeField] private SimpleScrollSnap simpleScrollSnap;
    [SerializeField] private DifficullyController difficultyController;

    public int currLevel; //0: 3; 1: 6; 2: 9; 3: 11
    public int currMode; //0: PC; 1: PP; 2: Campaign
    public int difficultyLevel; //0: easy; 1: medium; 2: hard

    private static bool withAI;

    public  bool WithAI{get;set;} = withAI;
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
        difficultyController.TurnOffPopUpDifficulty();
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
        currMode = 0;
        difficultyController.TurnOnPopUpDifficulty();
    }

    public void ClickOnPPMode(){
        currMode = 1;
        difficultyController.TurnOnPopUpDifficulty();
    }

    public void ClickOnCampaignMode(){
        currMode = 2;
        ConvertModes();
    }

    public void ConvertPCMode(){
        XOScreens[0].gameObject.SetActive(false);
        XOScreens[1].gameObject.SetActive(true);
        ChooseLevel();
        withAI = true;
    }

    public void ConvertPPMode(){
        XOScreens[0].gameObject.SetActive(false);
        XOScreens[1].gameObject.SetActive(true);
        ChooseLevel();
        withAI = false;
    }

    public void ConvertCampaignMode(){
        XOScreens[0].gameObject.SetActive(false);
        XOScreens[2].gameObject.SetActive(true);
    }

    
}