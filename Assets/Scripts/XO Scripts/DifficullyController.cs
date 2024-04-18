using UnityEngine;
using UnityEngine.UI;

public class DifficullyController : MonoBehaviour{
    [SerializeField] private Button[] difficultButtons; //0: easy, 1: medium, 2: hard
    [SerializeField] private RectTransform[] highlights; //0: easy, 1: medium, 2: hard
    public int currDifficultyLevel;

    private void Start(){
        currDifficultyLevel = 0;
        highlights[0].gameObject.SetActive(true);
        for (var i = 0; i < difficultButtons.Length; i++) {
            var index = i;
            difficultButtons[index].onClick.AddListener(() => { ChooseDifficultyMode(index); });
        }
    }

    private void ChooseDifficultyMode(int index){
        if (currDifficultyLevel != index) TurnOffHighlight();
        currDifficultyLevel = index;
        highlights[index].gameObject.SetActive(true);
    }

    private void TurnOffHighlight(){
        highlights[currDifficultyLevel].gameObject.SetActive(false);
    }
}