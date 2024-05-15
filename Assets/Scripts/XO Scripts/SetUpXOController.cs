using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SetUpXOController : MonoBehaviour{    
    [SerializeField] private GameObject[] XOScreens, XOLevelTables, LevelBanes;    
    [SerializeField] private SimpleScrollSnap simpleScrollSnap;
    [SerializeField] private DifficullyController difficultyController;

    private int currLevel; //0: 3; 1: 6; 2: 9; 3: 11
    public int currMode; //0: PC; 1: PP; 2: Campaign
    public int difficultyLevel; //0: easy; 1: medium; 2: hard

    private bool withAI, _isCampaign;

    public bool WithAI{get => withAI; set => withAI = value;}

    public bool IsCampaign{get => _isCampaign;set => _isCampaign = value;}

    public int CurrLevel{get => currLevel; set => currLevel = value;}

    void Awake(){
        _isCampaign = false;
        withAI = false;

        currLevel = 0;
        difficultyLevel = 0;
    }
    private void Start(){       
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
        difficultyController.TurnOnPopUpDifficulty();
    }

    public void ClickOnPPMode(){
        withAI = false;
        currMode = 1;
        difficultyController.TurnOnPopUpDifficulty();
    }

    public void ClickOnCampaignMode(){
        withAI = true;
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
        _isCampaign = true;
        XOScreens[0].gameObject.SetActive(false);
        XOScreens[2].gameObject.SetActive(true);
    }

    
}