using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public enum Seed{
    X,
    O,
    Empty
}

public class GameController : MonoBehaviour{
    [SerializeField] private GameObject[] turnIcons; // 0 is X, 1 is O 
    [SerializeField] private GameObject turnBorder; // 0 is X, 1 is O 
    [SerializeField] private GameObject[] highlights; // 0 is X, 1 is O 
    [SerializeField] public int startTurn;
    [SerializeField] private Button cell;
    [SerializeField] private Transform[] table;
    [SerializeField] private SetUpXOController setUpXoController;
    [SerializeField] private DifficullyController difficullyController;

    [SerializeField] private GameObject hintIcon;
    [SerializeField] private Button[] functionButtons;
    [SerializeField] private GameObject[] WinPopUps, WinPopUpCampaigns;
    [SerializeField] private GameObject WinPopUp, WinPopUpCampaign;

    [SerializeField] private GameObject backdrop;
    // [SerializeField] private GameObject XOPlayScreen;


    public bool withAI;
    public bool isCampaign;

    public int currLevelDifficulty;

    public List<bool> isWinCampaignList = new();
    public Seed currentPlayer;
    public Seed[] playerSeeds = new Seed[9];
    public List<Button> cellButtons;

    public int turnOCount;

    private LinkedList<int> cellButtonIndexList;

    private int currLevel;
    private LinkedList<RectTransform> iconObjectList;

    private GameObject lastButtonBorder;
    private GameObject lastHintIcon;

    private Vector2 pos1, pos2;

    private void Start(){
        // XOPlayScreenOrigin = XOPlayScreen;
        cellButtonIndexList = new LinkedList<int>();
        iconObjectList = new LinkedList<RectTransform>();
        cellButtons = new List<Button>();
        currLevelDifficulty = difficullyController.currDifficultyLevel;
        turnOCount = 0;

        //gen table
        AutoSpawnCell();

        currentPlayer = startTurn == 0 ? Seed.X : Seed.O;
        Highlight();

        for (var i = 0; i < cellButtons.Count; i++) {
            var index = i;
            cellButtons[index].interactable = true;
            if (currentPlayer == Seed.O && withAI) {
                AIPlay();
                SwitchTurn();
            }

            cellButtons[index].onClick.AddListener(() => { ClickGridButton(index, cellButtons[index]); });
        }

        for (var i = 0; i < playerSeeds.Length; i++) playerSeeds[i] = Seed.Empty;
    }

    private void ClickGridButton(int index, Button button){
        if (currentPlayer == Seed.X) {
            //instantiate gameobject in a button
            DisplayIcon(index, button);
            playerSeeds[index] = Seed.X;

            if (IsWon(currentPlayer)) {
                //Check whether the is current player has won
                if (isCampaign) DisplayWinPopUp(WinPopUpCampaigns[0]);
                else DisplayWinPopUp(WinPopUps[0]);
                Debug.Log("X Won!");

                DisableGridButtons();
            } else if (IsDraw()) {
                //check the game is draw
                if (isCampaign) DisplayWinPopUp(WinPopUpCampaigns[1]);
                else
                    DisplayWinPopUp(WinPopUps[1]);

                Debug.Log("It's Draw!");
                DisableGridButtons();
            } else {
                // can continue play the game
                SwitchTurn();
            }
        } else if (currentPlayer == Seed.O && !withAI) {
            //instantiate gameobject in a button
            DisplayIcon(index, button);
            playerSeeds[index] = Seed.O;

            if (IsWon(currentPlayer)) {
                //Check whether the current player has won
                Destroy(lastButtonBorder);
                Debug.Log("O Won!");
                DisableGridButtons();
            } else if (IsDraw()) {
                //check the game is draw
                Destroy(lastButtonBorder);
                if (isCampaign) DisplayWinPopUp(WinPopUpCampaigns[1]);
                else DisplayWinPopUp(WinPopUps[1]);
                Debug.Log("It's Draw!");
                DisableGridButtons();
            } else {
                // can continue play the game
                SwitchTurn();
            }
        }

        StartCoroutine(AITurn());
    }

    private IEnumerator AITurn(){
        DisableFunctionButtons();
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.5f);

        //After we have waited 5 seconds print the time again.
        EnableFunctionButtons();
        if (currentPlayer == Seed.O && withAI) {
            if (currLevelDifficulty == 0 && (turnOCount == 0 || turnOCount == 1))
                RandomPositionForO();
            else if (currLevelDifficulty == 1 && turnOCount == 0)
                RandomPositionForO();
            else
                AIPlay();
            if (IsWon(currentPlayer)) {
                Destroy(lastButtonBorder);

                if (isCampaign) DisplayWinPopUp(WinPopUpCampaigns[2]);
                else DisplayWinPopUp(WinPopUps[2]);
                Debug.Log("AI Won!");

                DisableGridButtons();
            } else if (IsDraw()) {
                //check the game is draw
                Destroy(lastButtonBorder);
                if (isCampaign) DisplayWinPopUp(WinPopUpCampaigns[1]);
                else DisplayWinPopUp(WinPopUps[1]);
                Debug.Log("It's Draw!");

                DisableGridButtons();
            } else {
                // can continue play the game
                SwitchTurn();
            }
        }
    }

    private void DisableFunctionButtons(){
        foreach (var button in functionButtons) button.interactable = false; // Disable the button
    }

    private void EnableFunctionButtons(){
        foreach (var button in functionButtons) button.interactable = true; // Disable the button
    }

    //add currMode
    private void AIPlay(){
        int bestScore = -1, bestPos = -1, value;
        var positionEmptyList = new List<int>();

        for (var i = 0; i < 9; i++)
            if (playerSeeds[i] == Seed.Empty) {
                positionEmptyList.Add(i);
                playerSeeds[i] = Seed.O;
                value = Minimax(Seed.X, playerSeeds, -1000, +1000);
                playerSeeds[i] = Seed.Empty;

                if (bestScore < value) {
                    bestScore = value;
                    bestPos = i;
                }
            }

        if (bestPos > -1) {
            var button = cellButtons[bestPos];
            DisplayIcon(bestPos, button);
            turnOCount++;
            playerSeeds[bestPos] = currentPlayer;
        } else {
            var rnd = new Random();
            var randIndex = rnd.Next(positionEmptyList.Count);
            var randomPosition = positionEmptyList[randIndex];
            bestPos = randomPosition;
            var button = cellButtons[bestPos];
            DisplayIcon(bestPos, button);
            turnOCount++;
            playerSeeds[bestPos] = currentPlayer;
        }
    }

    private void RandomPositionForO(){
        Debug.Log("Random Position For O");
        int bestScore = -1, bestPos = -1, value;
        var positionEmptyList = new List<int>();

        for (var i = 0; i < 9; i++)
            if (playerSeeds[i] == Seed.Empty) {
                positionEmptyList.Add(i);
                playerSeeds[i] = Seed.O;
                value = Minimax(Seed.X, playerSeeds, -1000, +1000);
                playerSeeds[i] = Seed.Empty;

                if (bestScore < value) {
                    bestScore = value;
                    bestPos = i;
                }
            }

        Debug.Log("List Position Empty: " + positionEmptyList.Count);
        Debug.Log("BestPos: " + bestPos);

        if (bestPos > -1) {
            positionEmptyList.Remove(bestScore);
            var rnd = new Random();
            var randIndex = rnd.Next(positionEmptyList.Count);
            var randomPosition = positionEmptyList[randIndex];
            bestPos = randomPosition;
            var button = cellButtons[bestPos];
            DisplayIcon(bestPos, button);
            turnOCount++;
            playerSeeds[bestPos] = currentPlayer;
        } else {
            var rnd = new Random();
            var randIndex = rnd.Next(positionEmptyList.Count);
            var randomPosition = positionEmptyList[randIndex];
            bestPos = randomPosition;
            var button = cellButtons[bestPos];
            DisplayIcon(bestPos, button);
            turnOCount++;
            playerSeeds[bestPos] = currentPlayer;
        }
    }


    private void DisplayIcon(int index, Button button){
        //instantiate iconObject in a button
        var iconObject = Instantiate(turnIcons[currentPlayer.GetHashCode()], button.transform)
            .GetComponent<RectTransform>();
        iconObject.DOScale(0.9f * Vector2.one, (float)0.3).From(Vector2.zero);
        iconObject.anchoredPosition = Vector2.zero;
        button.interactable = false;

        //instantiate iconObjectBorder in a button
        if (lastButtonBorder != null) Destroy(lastButtonBorder);
        if (lastHintIcon != null) Destroy(lastHintIcon);
        var iconObjectBorder = Instantiate(turnBorder, button.transform)
            .GetComponent<RectTransform>();
        iconObjectBorder.DOScale(1.1f * Vector2.one, (float)0.3).From(Vector2.zero);
        iconObjectBorder.anchoredPosition = Vector2.zero;
        lastButtonBorder = iconObjectBorder.gameObject;

        //Add iconObject and index of cellButton to list
        iconObjectList.AddLast(iconObject);
        cellButtonIndexList.AddLast(index);
    }

    private int Minimax(Seed currPlayer, Seed[] board, int alpha, int beta){
        if (IsDraw())
            return 0;

        if (IsWon(Seed.O))
            return +1;

        if (IsWon(Seed.X))
            return -1;

        int value;

        if (currPlayer == Seed.O) {
            for (var i = 0; i < 9; i++)
                if (board[i] == Seed.Empty) {
                    board[i] = Seed.O;
                    value = Minimax(Seed.X, board, alpha, beta);
                    board[i] = Seed.Empty;

                    if (value > alpha)
                        alpha = value;

                    if (alpha > beta)
                        break;
                }

            return alpha;
        }

        for (var i = 0; i < 9; i++)
            if (board[i] == Seed.Empty) {
                board[i] = Seed.X;
                value = Minimax(Seed.O, board, alpha, beta);
                board[i] = Seed.Empty;

                if (value < beta)
                    beta = value;

                if (alpha > beta)
                    break;
            }

        return beta;
    }

    private bool IsWon(Seed currPlayer){
        var hasWon = false;

        var winConditions = new int[8, 3] {
            { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
            { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
            { 0, 4, 8 }, { 2, 4, 6 }
        };

        // check win conditions
        for (var i = 0; i < 8; i++)
            if ((playerSeeds[winConditions[i, 0]] == currPlayer) &
                (playerSeeds[winConditions[i, 1]] == currPlayer) &
                (playerSeeds[winConditions[i, 2]] == currPlayer)) {
                hasWon = true;
                break;
            }

        return hasWon;
    }

    private bool IsDraw(){
        bool XWon, OWon, anyEmpty;

        XWon = IsWon(Seed.X);

        OWon = IsWon(Seed.O);

        // check if there is any empty cell or not
        anyEmpty = IsAnyEmpty();

        var isDraw = false;

        if ((XWon == false) & (OWon == false) & (anyEmpty == false))
            isDraw = true;

        return isDraw;
    }

    private void DisplayWinPopUp(GameObject winPopUp){
        turnOCount = 0;
        backdrop.SetActive(true);
        if (isCampaign) WinPopUpCampaign.SetActive(true);
        else
            WinPopUp.SetActive(true);

        winPopUp.SetActive(true);
    }

    private void SwitchTurn(){
        //change the current player after each turn
        if (currentPlayer == Seed.X)
            currentPlayer = Seed.O;
        else if (currentPlayer == Seed.O) currentPlayer = Seed.X;
        Highlight();
    }

    private void DisableGridButtons(){
        foreach (var button in cellButtons) button.interactable = false;
    }

    private bool IsAnyEmpty(){
        var empty = false;
        for (var i = 0; i < 9; i++)
            if (playerSeeds[i] == Seed.Empty) {
                empty = true;
                break;
            }

        return empty;
    }

    public void Highlight(){
        if (currentPlayer == Seed.X) {
            highlights[1].SetActive(false);
            highlights[0].SetActive(true);
        } else if (currentPlayer == Seed.O) {
            highlights[0].SetActive(false);
            highlights[1].SetActive(true);
        }
    }

    public void Back(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Back");
    }

    public void Replay(){
        currentPlayer = startTurn == 0 ? Seed.X : Seed.O;
        Highlight();
        turnOCount = 0;
        for (var i = 0; i < playerSeeds.Length; i++) playerSeeds[i] = Seed.Empty;

        foreach (var button in cellButtons) {
            button.interactable = true;

            foreach (Transform child in button.transform) Destroy(child.gameObject);
        }

        Debug.Log("Game reset for a new replay.");
    }

    public void Return(){
        if (cellButtonIndexList.Count == 0) {
            functionButtons[0].interactable = false;
            Debug.Log("Can not return!");
        } else {
            functionButtons[0].interactable = true;
            Destroy(lastButtonBorder);
            playerSeeds[cellButtonIndexList.Last.Value] = Seed.Empty;
            Destroy(iconObjectList.Last.Value.gameObject);
            cellButtons[cellButtonIndexList.Last.Value].interactable = true;
            cellButtonIndexList.RemoveLast();
            iconObjectList.RemoveLast();


            playerSeeds[cellButtonIndexList.Last.Value] = Seed.Empty;
            Destroy(iconObjectList.Last.Value.gameObject);
            cellButtons[cellButtonIndexList.Last.Value].interactable = true;
            cellButtonIndexList.RemoveLast();
            iconObjectList.RemoveLast();
            if (cellButtonIndexList.Count > 0)
                lastButtonBorder = Instantiate(turnBorder, cellButtons[cellButtonIndexList.Last.Value].transform);
        }
    }

    private void AutoSpawnCell(){
        Debug.Log("AutoSpawnCell");
        var index = setUpXoController.currLevel;
        switch (index) {
            case 0:
                currLevel = 3;
                break;
            case 1:
                currLevel = 6;
                break;
            case 2:
                currLevel = 9;
                break;
            case 3:
                currLevel = 11;
                break;
        }

        table[index].gameObject.SetActive(true);
        for (var i = 0; i < currLevel * currLevel; i++) {
            var item = Instantiate(cell, table[index]);
            cellButtons.Add(item);
        }
    }

    public void Hint(){
        Debug.Log("Hint");

        if (currentPlayer == Seed.X) {
            int bestScore = 1000, bestPos = -1, value;
            for (var i = 0; i < 9; i++)
                if (playerSeeds[i] == Seed.Empty) {
                    playerSeeds[i] = Seed.X;
                    value = Minimax(Seed.O, playerSeeds, -1000, +1000);
                    playerSeeds[i] = Seed.Empty;
                    if (bestScore > value) {
                        bestScore = value;
                        bestPos = i;
                    }
                }

            if (bestPos > -1) {
                var button = cellButtons[bestPos];
                var iconObject = Instantiate(hintIcon, button.transform)
                    .GetComponent<RectTransform>();
                lastHintIcon = iconObject.gameObject;
            }
        } else {
            int bestScore = -1, bestPos = -1, value;
            for (var i = 0; i < 9; i++)
                if (playerSeeds[i] == Seed.Empty) {
                    playerSeeds[i] = Seed.O;
                    value = Minimax(Seed.X, playerSeeds, -1000, +1000);
                    playerSeeds[i] = Seed.Empty;
                    if (bestScore < value) {
                        bestScore = value;
                        bestPos = i;
                    }
                }

            if (bestPos > -1) {
                var button = cellButtons[bestPos];
                var iconObject = Instantiate(hintIcon, button.transform)
                    .GetComponent<RectTransform>();
                lastHintIcon = iconObject.gameObject;
            }
        }
    }
}