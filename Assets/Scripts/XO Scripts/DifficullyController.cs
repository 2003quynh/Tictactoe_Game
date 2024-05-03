using UnityEngine;
using UnityEngine.UI;

public class DifficullyController : MonoBehaviour{
    [SerializeField] private Button[] difficultButtons; //0: easy, 1: medium, 2: hard
    [SerializeField] private RectTransform[] highlights; //0: easy, 1: medium, 2: hard
    public int currDifficultyLevel;

    private void Start(){
        Debug.Log("Diffi 1");
        currDifficultyLevel = 0;
        Debug.Log("Diffi 2");
        highlights[0].gameObject.SetActive(true);
        Debug.Log("Diffi 3");
        for (var i = 0; i < difficultButtons.Length; i++) {
            var index = i;
            difficultButtons[index].onClick.AddListener(() => { ChooseDifficultyMode(index); });
        }
        Debug.Log("Diffi 4");
    }

    private void ChooseDifficultyMode(int index){
       Debug.Log("Diffi 5");
        if (currDifficultyLevel != index) TurnOffHighlight();
        Debug.Log("Diffi 6");
        currDifficultyLevel = index;
        highlights[index].gameObject.SetActive(true);
        Debug.Log("Diffi 7");
    }

    private void TurnOffHighlight(){
        highlights[currDifficultyLevel].gameObject.SetActive(false);
    }
}