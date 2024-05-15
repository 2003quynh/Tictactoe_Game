

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Seed startTurn;
    [SerializeField] private Button cell;
    [SerializeField] private Transform[] table;
    [SerializeField] private SetUpXOController setUpXoController;
    [SerializeField] private DifficullyController difficullyController;
    [SerializeField] private GameObject[] hintIcons;
    [SerializeField] private Button[] functionButtons;
    [SerializeField] private GameObject[] winPopUps, winPopUpCampaigns, winningLines;
    [SerializeField] private GameObject winPopUp, winPopUpCampaign, backdrop;    
    [SerializeField] private RateUsController rateUsController;
    [SerializeField] private GameObject XPos, OPos;
    

    public bool withAI;
    public bool isCampaign;
    public int currLevelDifficulty;
    public List<bool> isWinCampaignList = new();
    public Seed currentPlayer;
    public Seed[] playerSeeds;
    public List<Button> cellButtons;
    public int turnAICount;
    public int turnXCount;
    private LinkedList<int> cellButtonIndexList;
    private int currLevel;
    private LinkedList<RectTransform> iconObjectList;
    private GameObject lastButtonBorder;
    private GameObject lastHintIcon;
    private bool aiFirst;
    private bool invokeOnce;
    private float delay;
    private float timer; 
    private int depthLength;   

    private SetUpXOController setUpXOController;
   
    
    private void Start(){       
        Init();        
        
        //set AI is X or O
        int randAIRole = UnityEngine.Random.Range(0, 20); // 0: x, 1: o
        
        //set start AI or people        
        aiFirst = randAIRole%2 == 1 && withAI;
        
        if(aiFirst){
         invokeOnce = true;
        }
        
        if (aiFirst && invokeOnce)
        {
            Debug.Log("AI First: " + aiFirst);
            AIPlay();       
            SwitchTurn();       
            invokeOnce = false;
        }
    }

    private void Init(){
        cellButtonIndexList = new LinkedList<int>();
        iconObjectList = new LinkedList<RectTransform>();
        cellButtons = new List<Button>();
        
        currLevelDifficulty = difficullyController.currDifficultyLevel;        
        startTurn = Seed.X;
        currentPlayer = startTurn;
        turnAICount = 0; //count how many AI turns to set random for AI
        turnXCount = 0; //count how many X turns to set interactable for return button
        delay = 0.5f;   
        Debug.Log("setUpXOController.WithAI" + setUpXOController.WithAI);
        withAI = setUpXOController.WithAI;
        
        //gen table
        AutoSpawnCell();
        Return();
        
        highlights[1].SetActive(false);
        highlights[0].SetActive(true);
        
        for (var i = 0; i < playerSeeds.Length; i++) playerSeeds[i] = Seed.Empty;
        
        for (var i = 0; i < cellButtons.Count; i++) {
            var index = i;
            cellButtons[index].interactable = true;
            cellButtons[index].onClick.AddListener(() => { ClickGridButton(index, cellButtons[index]); });
        }         
    }

    private void Update()
    {
        // SwitchTurn();
        if (withAI && invokeOnce)
        {            
            timer += Time.deltaTime;
            
            if (timer > delay){
                AIPlay();
                SwitchTurn();
                invokeOnce = false;
                timer = 0;
                
            }            
        }
    }

    private void ClickGridButton(int index, Button button){
       
        if (currentPlayer == Seed.X && !withAI) {
            //instantiate gameobject in a button
            DisplayIcon(index, button);
            turnXCount++;
            playerSeeds[index] = Seed.X;

            if (IsWon(currentPlayer)) {
                //Check whether the current player has won                
                DisplayWinLine(currentPlayer);
                Destroy(lastButtonBorder);
                Debug.Log("X Won!");
                DisableGridButtons();
            } else if (IsDraw()) {
                //check the game is draw
                Destroy(lastButtonBorder);
                if (isCampaign) DisplayWinPopUp(winPopUpCampaigns[1]);
                else DisplayWinPopUp(winPopUps[1]);
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
                
                DisplayWinLine(currentPlayer);
                Destroy(lastButtonBorder);
                Debug.Log("O Won!");
                DisableGridButtons();
            } else if (IsDraw()) {
                //check the game is draw
                Destroy(lastButtonBorder);
                if (isCampaign) DisplayWinPopUp(winPopUpCampaigns[1]);
                else DisplayWinPopUp(winPopUps[1]);
                Debug.Log("It's Draw!");
                DisableGridButtons();
            } else {
                // can continue play the game
                SwitchTurn();
            }
        }

        if(currentPlayer == Seed.O && aiFirst){
            DisplayIcon(index, button);
            playerSeeds[index] = Seed.O;

            if (IsWon(currentPlayer)) {
                DisplayWinLine(currentPlayer);
                OpenRateUs();
                //Check whether the is current player has won
                if (isCampaign) DisplayWinPopUp(winPopUpCampaigns[0]);
                else DisplayWinPopUp(winPopUps[0]);
                Debug.Log("Person Won!");

                DisableGridButtons();
            } else if (IsDraw()) {
                //check the game is draw
                if (isCampaign) DisplayWinPopUp(winPopUpCampaigns[1]);
                else
                    DisplayWinPopUp(winPopUps[1]);

                Debug.Log("It's Draw!");
                DisableGridButtons();
            } else {
                // can continue play the game
                SwitchTurn();
                invokeOnce = true;
            }
        } else if (currentPlayer == Seed.X && withAI) {
            DisplayIcon(index, button);
            turnXCount++;
            playerSeeds[index] = Seed.X;

            if (IsWon(currentPlayer)) {
                //Check whether the is current player has won
                DisplayWinLine(currentPlayer);  
                OpenRateUs();              
                if (isCampaign) DisplayWinPopUp(winPopUpCampaigns[0]);
                else DisplayWinPopUp(winPopUps[0]);
                Debug.Log("Person Won!");
                DisableGridButtons();

            } else if (IsDraw()) {
                //check the game is draw
                if (isCampaign) DisplayWinPopUp(winPopUpCampaigns[1]);
                else
                    DisplayWinPopUp(winPopUps[1]);

                Debug.Log("It's Draw!");
                DisableGridButtons();
            } else {
                // can continue play the game
                SwitchTurn();
                invokeOnce = true;
            }
        }
    }
      
    //add currMode
    private void AIPlay(){
        
        if (aiFirst && currLevelDifficulty == 0 && (turnAICount == 0 || turnAICount == 1))
            RandomPositionForAI();
        else if (aiFirst && currLevelDifficulty == 1 && (turnAICount == 0))
            RandomPositionForAI();
        else if(!aiFirst && currLevelDifficulty == 0 && (turnAICount == 0 || turnAICount == 1))
            RandomPositionForAI();
        else if (!aiFirst && currLevelDifficulty == 1 && turnAICount == 0)
            RandomPositionForAI();
        else{
            if (currentPlayer == Seed.X) TurnX();
            if (currentPlayer == Seed.O) TurnO();
        }        
    }

    private void TurnX(){
        Debug.Log("Turn X");
        currentPlayer = Seed.X;
        Highlight();
        
        int bestScore = 1000, bestPos = -1, value;
        var positionEmptyList = new List<int>();

        for (var i = 0; i < playerSeeds.Length; i++)
            if (playerSeeds[i] == Seed.Empty) {
                playerSeeds[i] = Seed.X;
                positionEmptyList.Add(i);
                Debug.Log("start minimax");
                value = Minimax(Seed.O,0, depthLength, playerSeeds, -1000, +1000);
                Debug.Log("end minimax");
                playerSeeds[i] = Seed.Empty;
                if (bestScore > value) {
                    bestScore = value;
                    bestPos = i;
                }
            }         
        
        if (bestPos > -1) {
            Debug.Log("bestPos: " + bestPos);
            var button = cellButtons[bestPos];
            DisplayIcon(bestPos, button);
            turnXCount++;
            playerSeeds[bestPos] = currentPlayer;

        }
         else {
            var rnd = new Random();
            var randIndex = rnd.Next(positionEmptyList.Count);
            Debug.Log("positionEmptyList: " + positionEmptyList[randIndex]==null);
            var randomPosition = positionEmptyList[randIndex];
            bestPos = randomPosition;
            var button = cellButtons[bestPos];
            DisplayIcon(bestPos, button);
            turnXCount++;
            playerSeeds[bestPos] = currentPlayer;
        }

        if (IsWon(currentPlayer)) {
            Destroy(lastButtonBorder);
            DisplayWinLine(currentPlayer);
            if (isCampaign) DisplayWinPopUp(winPopUpCampaigns[2]);
            else DisplayWinPopUp(winPopUps[2]);
            Debug.Log("AI Won!");

            DisableGridButtons();
        } else if (IsDraw()) {
            //check the game is draw
            Destroy(lastButtonBorder);
            if (isCampaign) DisplayWinPopUp(winPopUpCampaigns[1]);
            else DisplayWinPopUp(winPopUps[1]);
            Debug.Log("It's Draw!");

            DisableGridButtons();
        } 
    }

    private void TurnO(){
        Debug.Log("Turn O");
        currentPlayer = Seed.O;
        Highlight();

        int bestScore = -1, bestPos = -1, value;
        var positionEmptyList = new List<int>();

        for (var i = 0; i < playerSeeds.Length; i++)
            if (playerSeeds[i] == Seed.Empty) {
                positionEmptyList.Add(i);
                playerSeeds[i] = Seed.O;
                value = Minimax(Seed.X,0, depthLength, playerSeeds, -1000, +1000);
                playerSeeds[i] = Seed.Empty;

                if (bestScore < value) {
                    bestScore = value;
                    bestPos = i;
                }
            }

        if (bestPos > -1) {
            var button = cellButtons[bestPos];
            DisplayIcon(bestPos, button);
            turnAICount++;
            playerSeeds[bestPos] = currentPlayer;
        } else {
            var rnd = new Random();
            var randIndex = rnd.Next(positionEmptyList.Count);
            Debug.Log("positionEmptyList: " + positionEmptyList[randIndex]);
            var randomPosition = positionEmptyList[randIndex];
            bestPos = randomPosition;
            var button = cellButtons[bestPos];
            DisplayIcon(bestPos, button);
            turnAICount++;
            playerSeeds[bestPos] = currentPlayer;
        }
        if (IsWon(currentPlayer)) {                
            Destroy(lastButtonBorder);
            DisplayWinLine(currentPlayer);
            if (isCampaign) DisplayWinPopUp(winPopUpCampaigns[2]);
            else DisplayWinPopUp(winPopUps[2]);
            Debug.Log("AI Won!");

            DisableGridButtons();
        } else if (IsDraw()) {
            //check the game is draw
            Destroy(lastButtonBorder);
            if (isCampaign) DisplayWinPopUp(winPopUpCampaigns[1]);
            else DisplayWinPopUp(winPopUps[1]);
            Debug.Log("It's Draw!");

            DisableGridButtons();
        }
    }

    private void RandomPositionForAI(){
        Debug.Log("Random Position For AI");
        if(aiFirst){                       
            int bestScore = 1000, bestPos = -1, value;
            var positionEmptyList = new List<int>();

            for (var i = 0; i < playerSeeds.Length; i++){
                if (playerSeeds[i] == Seed.Empty) {
                    positionEmptyList.Add(i);
                    playerSeeds[i] = Seed.X;
                    Debug.Log("start minimax");
                    value = Minimax(Seed.O,0, depthLength, playerSeeds, -1000, +1000);
                    Debug.Log("end minimax");
                    playerSeeds[i] = Seed.Empty;

                    if (bestScore > value) {
                        bestScore = value;
                        bestPos = i;
                    }
                }
            }
            
            // Debug.Log("List Position Empty: " + positionEmptyList.Count);
            // Debug.Log("BestPos: " + bestPos);

            if (bestPos > -1) {
                positionEmptyList.Remove(bestScore);               
            } 

            var rnd = new Random();
            var randIndex = rnd.Next(positionEmptyList.Count);
            var randomPosition = positionEmptyList[randIndex];
            
            bestPos = randomPosition;
            var button = cellButtons[bestPos];
            DisplayIcon(bestPos, button);
            turnAICount++;
            turnXCount++;
            playerSeeds[bestPos] = currentPlayer;

        } else {
            int bestScore = -1, bestPos = -1, value;
            var positionEmptyList = new List<int>();

            for (var i = 0; i < playerSeeds.Length; i++){
                if (playerSeeds[i] == Seed.Empty) {
                    positionEmptyList.Add(i);
                    playerSeeds[i] = Seed.O;
                    value = Minimax(Seed.X,0, depthLength, playerSeeds, -1000, +1000);
                    playerSeeds[i] = Seed.Empty;

                    if (bestScore < value) {
                        bestScore = value;
                        bestPos = i;
                    }
                }
            }
            
            // Debug.Log("List Position Empty: " + positionEmptyList.Count);
            // Debug.Log("BestPos: " + bestPos);

            if (bestPos > -1) {
                positionEmptyList.Remove(bestScore);                
            } 
            var rnd = new Random();
            var randIndex = rnd.Next(positionEmptyList.Count);
            var randomPosition = positionEmptyList[randIndex];

            bestPos = randomPosition;
            var button = cellButtons[bestPos];
            DisplayIcon(bestPos, button);
            turnAICount++;
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
        Debug.Log("Display Icon: " + button.interactable);

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
        functionButtons[0].interactable = true;
    }

    private int Minimax(Seed currPlayer,int index, int depth, Seed[] board, int alpha, int beta){
        if(depth == 0 ){                       
            return 0;
        }
        if(currentPlayer == Seed.X){
            if(IsWon(Seed.X))
                return -1;
        }
        if(currentPlayer == Seed.O){
            if(IsWon(Seed.O))
                return 1;
        }                     
        if(!IsAnyEmpty())
            return 0; 
                
        int value;

        if (currPlayer == Seed.O) {
            for (var i = index; i < board.Length; i++)
                if (board[i] == Seed.Empty) {
                    board[i] = Seed.O;
                    value = Minimax(Seed.X, i, depth - 1, board, alpha, beta);                    
                    board[i] = Seed.Empty;
                    alpha = Math.Max(alpha, value);                    
                    if (alpha >= beta)
                        break;
                }
            return alpha;
        }

        for (var i = 0; i < board.Length; i++)
            if (board[i] == Seed.Empty) {
                board[i] = Seed.X;
                value = Minimax(Seed.O,0, depth - 1, board, alpha, beta);
                board[i] = Seed.Empty;
                beta = Math.Min(beta, value);
                if (alpha >= beta)
                    break;
            }
        return beta;        
    }
    
    private List<int[]>  winConditions= new List<int[]>();
    public List<int[]> WinCondition(int tableSize, int lineLength){
        for(int row = 0; row < tableSize; row++ ){
            for(int startCol = 0; startCol < tableSize - lineLength + 1; startCol++){
                int[] condition = new int[lineLength];
                for(int i = 0; i < lineLength; i++){
                    condition[i] = row * tableSize + startCol + i;
                }
                winConditions.Add(condition);
            }
        }   

        for(int col = 0; col < tableSize; col++ ){
            for(int startRow = 0; startRow < tableSize - lineLength + 1; startRow++){
                int[] condition = new int[lineLength];
                for(int i = 0; i < lineLength; i++){
                    condition[i] = (startRow+i)*tableSize+col;
                }
                winConditions.Add(condition);
            }
        }   
    
        for (int row = 0; row <= tableSize - lineLength; row++) {
            for (int col = 0; col <= tableSize - lineLength; col++) {
                int[] condition = new int[lineLength];
                for (int i = 0; i < lineLength; i++) {
                    condition[i] = (row + i) * tableSize + col + i;
                }
                winConditions.Add(condition);
            }
        }

        // Diagonal lines (Minor Diagonal)
        for (int row = 0; row <= tableSize - lineLength; row++) {
            for (int col = lineLength - 1; col < tableSize; col++) {
                int[] condition = new int[lineLength];
                for (int i = 0; i < lineLength; i++) {
                    condition[i] = (row + i) * tableSize + col - i;
                }
                winConditions.Add(condition);
            }
    
        }
        return winConditions;
    }

    public bool IsWon(Seed currPlayer) {
        foreach (var condition in winConditions) {
            bool win = true;
            foreach (var index in condition) {
                if (playerSeeds[index] != currPlayer) {
                    win = false;
                    break;
                }
            }
            if (win) return true;
        }
        return false;
    }

    List<int[]> relevantWinConditions = new List<int[]>();

    public void InitializeRelevantWinConditions() {
        // Dựa trên vị trí của quân cờ gần nhất đã được đặt, chỉ chọn ra các điều kiện chiến thắng có thể bị ảnh hưởng
        // từ quân cờ đó
        foreach (var condition in winConditions) {
            foreach (var index in condition) {
                if (playerSeeds[index] == Seed.Empty) {
                    relevantWinConditions.Add(condition);
                    break;
                }
            }
        }
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

    private void OpenRateUs(){
        if(IsWon(currentPlayer)){
            DisableFunctionButtons();
            rateUsController.TurnOnRateUSPopUp();
        }
    }

    private void DisplayWinPopUp(GameObject winPopUp){
        turnAICount = 0;
        backdrop.SetActive(true);
        if (isCampaign) winPopUpCampaign.SetActive(true);
        else
            this.winPopUp.SetActive(true);

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
        for (var i = 0; i < playerSeeds.Length; i++)
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
            
            highlights[0].transform.DOMove(XPos.transform.position, 0.3f)
                .From(OPos.transform.position);
        } else if (currentPlayer == Seed.O) {
            highlights[0].SetActive(false);
            highlights[1].SetActive(true);
            highlights[1].transform.DOMove(OPos.transform.position, 0.3f)
                .From(XPos.transform.position);
        }
    }

    public void Back(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Back");
    }

    public void Replay(){
        currentPlayer = startTurn;
        Highlight();
        turnAICount = 0;
        turnXCount = 0;
        DeactiveWinLine();

        for (var i = 0; i < playerSeeds.Length; i++) playerSeeds[i] = Seed.Empty;

        foreach (var button in cellButtons) {
            button.interactable = true;

            foreach (Transform child in button.transform) Destroy(child.gameObject);
        }

        if(aiFirst) {
            invokeOnce = true;
        }
        EnableFunctionButtons();
        functionButtons[0].interactable = false;
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
            if (cellButtonIndexList.Count > 0)
            lastButtonBorder = Instantiate(turnBorder, cellButtons[cellButtonIndexList.Last.Value].transform);
        }

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
            if (cellButtonIndexList.Count > 0)
            lastButtonBorder = Instantiate(turnBorder, cellButtons[cellButtonIndexList.Last.Value].transform);
        }
    }

    private void AutoSpawnCell(){
        Debug.Log("AutoSpawnCell");
        var index = setUpXoController.currLevel;
        var lengthWinLine = 0;
        switch (index) {
            case 0:
                currLevel = 3;
                lengthWinLine = 3;
                break;
            case 1:
                currLevel = 6;
                lengthWinLine = 4;
                break;
            case 2:
                currLevel = 9;
                lengthWinLine = 5;
                break;
            case 3:
                currLevel = 11;
                lengthWinLine = 6;
                break;
        }

        table[index].gameObject.SetActive(true);
        for (var i = 0; i < currLevel * currLevel; i++) {
            var item = Instantiate(cell, table[index]);
            cellButtons.Add(item);
        }

        WinCondition(currLevel, lengthWinLine);
        playerSeeds = new Seed[currLevel*currLevel];
        depthLength = 3;
    }

    public void Hint(){
        if (currentPlayer == Seed.X) {
            int bestScore = 1000, bestPos = -1, value;
            for (var i = 0; i < 9; i++)
                if (playerSeeds[i] == Seed.Empty) {
                    playerSeeds[i] = Seed.X;
                    value = Minimax(Seed.O,0, depthLength, playerSeeds, -1000, +1000);
                    playerSeeds[i] = Seed.Empty;
                    if (bestScore > value) {
                        bestScore = value;
                        bestPos = i;
                    }
                }

            if (bestPos > -1) {
                var button = cellButtons[bestPos];
                var iconObject = Instantiate(hintIcons[0], button.transform)
                    .GetComponent<RectTransform>();
                lastHintIcon = iconObject.gameObject;
            }
        } else if(currentPlayer == Seed.O)
         {
            int bestScore = -1, bestPos = -1, value;
            for (var i = 0; i < 9; i++)
                if (playerSeeds[i] == Seed.Empty) {
                    playerSeeds[i] = Seed.O;
                    value = Minimax(Seed.X,0, depthLength, playerSeeds, -1000, +1000);
                    playerSeeds[i] = Seed.Empty;
                    if (bestScore < value) {
                        bestScore = value;
                        bestPos = i;
                    }
                }

            if (bestPos > -1) {
                var button = cellButtons[bestPos];
                var iconObject = Instantiate(hintIcons[1], button.transform)
                    .GetComponent<RectTransform>();
                lastHintIcon = iconObject.gameObject;
            }
        }
    }

    private void DisplayWinLine(Seed currPlayer){       
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
                Debug.Log("Win Condition: " + winConditions[i, 1]);
                switch (winConditions[i, 1]) {                    
                    case 1:                        
                        DrawWinLine(0);
                        break;
                    case 3:                        
                        DrawWinLine(3);
                        break;    
                    case 4:
                        switch(winConditions[i, 0]){
                            case 0:                            
                                DrawWinLine(7);
                                break;
                            case 1:                            
                                DrawWinLine(4);
                                break;
                            case 2:                                
                                DrawWinLine(6);
                                break;
                            case 3:                                
                                DrawWinLine(1);
                                break;
                        } 
                        break;                       
                    case 5:                        
                        DrawWinLine(5);
                        break;
                    case 7:                        
                        DrawWinLine(2);
                        break;                    
                }
                break;
            }           
        
    }

    private void DrawWinLine(int indexWin){
        winningLines[indexWin].SetActive(true); 
        winningLines[indexWin].transform.DOScale(Vector2.one, (float)0.3).From(Vector2.zero);;     
    }

    public void DeactiveWinLine(){
        for(int i = 0; i < winningLines.Length; i++){
            winningLines[i].SetActive(false);      
        }
    }

    private void DisableFunctionButtons(){
        foreach (var button in functionButtons) button.interactable = false; // Disable the button
    }

    private void EnableFunctionButtons(){
        foreach (var button in functionButtons) button.interactable = true; // Disable the button
    }

}

