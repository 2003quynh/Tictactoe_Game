using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPopUpController : MonoBehaviour{

    [SerializeField] private GameObject backdrop, winPopUpScreen;    

    [SerializeField] private GameObject[] winPopUps;

    public void Home(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Back Home");
    }

    public void DisplayWinPopUp(GameObject winPopUp){
        backdrop.SetActive(true);
        winPopUpScreen.SetActive(true);
        winPopUp.SetActive(true);
    }

    public void CloseWinPopUp(){
        backdrop.SetActive(false);
        winPopUpScreen.SetActive(false);        
        foreach (var popup in winPopUps) popup.SetActive(false);
    }
}