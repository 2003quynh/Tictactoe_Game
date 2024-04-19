using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignController : MonoBehaviour{
    public List<CampaignData> campaignList;
    [SerializeField] private GameObject XOPlayScreen;
    [SerializeField] private GameObject CampaignScreen;
    [SerializeField] private GameObject[] XOLevelTables, LevelBanes;

    public List<bool> isUnlockedList = new();


    [SerializeField] private GameController gameController;
    private readonly List<GameObject> campaignListLocked = new();
    private readonly List<GameObject> campaignListUnlocked = new();

    private void Start(){
        for (var i = 0; i < campaignList.Count; i++) {
            if (i == 0) {
                isUnlockedList.Add(true);

                var iconObject = Instantiate(campaignList[i].prefabButtonLevelUnlocked,
                    campaignList[i].position.transform);
                iconObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                campaignListLocked.Add(iconObject);
            }

            isUnlockedList.Add(false);
        }

        for (var i = 0; i < campaignList.Count; i++) {
            var index = i;
            var campaign = campaignList[index];
            if (isUnlockedList[index]) {
                Destroy(campaignListLocked[index]);
                var iconObject = Instantiate(campaign.prefabButtonLevelUnlocked, campaign.position.transform);
                iconObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                campaignListUnlocked.Add(iconObject);
            } else {
                var iconObject = Instantiate(campaign.prefabButtonLevelLocked, campaign.position.transform);
                iconObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                campaignListLocked.Add(iconObject);
            }
        }

        for (var i = 0; i < campaignListUnlocked.Count; i++) {
            var index = i;
            Debug.Log(index);
            if (campaignListUnlocked[index].GetComponent<Button>() != null) {
                var campaignUnlocked = campaignListUnlocked[index].GetComponent<Button>();
                campaignUnlocked.onClick.AddListener(() => { ChooseLevelCampaign(index, campaignUnlocked); });
            }
        }
    }

    public void ChooseLevelCampaign(int index, Button button){
        CampaignScreen.SetActive(false);
        XOPlayScreen.SetActive(true);

        XOLevelTables[campaignList[index].level].SetActive(true);
        LevelBanes[campaignList[index].level].SetActive(true);

        gameController.withAI = true;
    }
}