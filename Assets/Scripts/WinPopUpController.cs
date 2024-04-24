using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPopUpController : MonoBehaviour{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject WinPopUp, WinPopUpCampaign, backdrop, HomePage, XOPlayScreen, CampaignScreen;
    [SerializeField] private GameObject[] WinPopUps, WinPopUpCampaigns;

    [SerializeField] private CampaignController campaignController;

    private void Start(){ }

    public void Replay(){
        backdrop.SetActive(false);
        WinPopUp.SetActive(false);
        WinPopUpCampaign.SetActive(false);
        foreach (var popup in WinPopUps) popup.SetActive(false);
        foreach (var popup in WinPopUpCampaigns) popup.SetActive(false);

        // gameController.currentPlayer = gameController.startTurn == 0 ? Seed.X : Seed.O;
        // gameController.Highlight();
        // for (var i = 0; i < gameController.playerSeeds.Length; i++) gameController.playerSeeds[i] = Seed.Empty;

        // foreach (var button in gameController.cellButtons) {
        //     button.interactable = true;
        //     foreach (Transform child in button.transform) Destroy(child.gameObject);
        // }
        gameController.Replay();

        Debug.Log("Game replay.");
    }

    public void Home(){
        // XOPlayScreen.SetActive(false);
        // backdrop.SetActive(false);
        // foreach (var popup in WinPopUps) popup.SetActive(false);
        // HomePage.SetActive(true);
        // for (var i = 0; i < gameController.playerSeeds.Length; i++) gameController.playerSeeds[i] = Seed.Empty;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Back Home");
    }

    public void Next(){
        //unlock next level
        campaignController.lastIndexUnlocked++;
        campaignController.isUnlockedList[campaignController.lastIndexUnlocked] = true;

        //return campaign screen
        WinPopUpCampaign.SetActive(false);
        foreach (var popup in WinPopUpCampaigns) popup.SetActive(false);
        backdrop.SetActive(false);
        XOPlayScreen.SetActive(false);
        CampaignScreen.SetActive(true);
        campaignController.SetUpCampaign();

        gameController.currentPlayer = Seed.X;
        gameController.Highlight();
        for (var i = 0; i < gameController.playerSeeds.Length; i++) gameController.playerSeeds[i] = Seed.Empty;

        foreach (var button in gameController.cellButtons) {
            button.interactable = true;
            foreach (Transform child in button.transform) Destroy(child.gameObject);
        }
    }
}