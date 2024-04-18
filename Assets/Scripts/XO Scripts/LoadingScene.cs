using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour{
    [SerializeField] private GameObject loadingScreen, mainMenu;
    [SerializeField] private Image loadingBarFill;
    [SerializeField] private int sceneId;


    public void Start(){
        LoadScene();
    }

    public void LoadScene(){
        StartCoroutine(LoadSceneAsync());
        // loadingScreen.SetActive(false);
        // mainMenu.SetActive(true);
    }

    private IEnumerator LoadSceneAsync(){
        var operation = SceneManager.LoadSceneAsync(sceneId);
        loadingScreen.SetActive(true);
        while (!operation.isDone) {
            var progressValue = Mathf.Clamp01(operation.progress / 0.5f);
            loadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }
}