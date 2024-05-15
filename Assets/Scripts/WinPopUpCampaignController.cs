using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPopUpCampaignController : MonoBehaviour{

    [SerializeField] private GameObject backdrop;   
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject  xoPlayScreen, campaignScreen, winPopUpCampaignScreen;
    [SerializeField] private GameObject[]  winPopUpCampaigns;
    [SerializeField] private CampaignController campaignController;

    public void Home(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Back Home");
    }

    public void Next(){
        //unlock next level
        campaignController.lastIndexUnlocked++;
        campaignController.isUnlockedList[campaignController.lastIndexUnlocked] = true;

        //return campaign screen
        winPopUpCampaignScreen.SetActive(false);
        foreach (var popup in winPopUpCampaigns) popup.SetActive(false);
        backdrop.SetActive(false);
        xoPlayScreen.SetActive(false);
        campaignScreen.SetActive(true);
        campaignController.SetUpCampaign();

        gameController.currentPlayer = Seed.X;
        gameController.Highlight();
        for (var i = 0; i < gameController.playerSeeds.Length; i++) gameController.playerSeeds[i] = Seed.Empty;

        foreach (var button in gameController.cellButtons) {
            button.interactable = true;
            foreach (Transform child in button.transform) Destroy(child.gameObject);
        }

        gameController.DeactiveWinLine();
    }

    public void DisplayWinCampaignPopUp(GameObject winCampaignPopUp){
        backdrop.SetActive(true);
        winPopUpCampaignScreen.SetActive(true);
        winCampaignPopUp.SetActive(true);    
    }

    public void CloseWinCampaignPopUp(){
        backdrop.SetActive(false);       
        winPopUpCampaignScreen.SetActive(false);
        foreach (var popup in winPopUpCampaigns) popup.SetActive(false);
    }
}