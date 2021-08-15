using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using Mirror;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;


public class GM : NetworkBehaviour
{
    [SerializeField] private GameObject canvas, chip, p1chip, p2chip, hostButton, joinButton, block, code;
    [SerializeField] private Image[] allTiles = new Image[36];
    [SerializeField] private GameObject selectorObj, textField;
    [SerializeField] private GameObject playButton, replayButton, leaveButton;
    [SerializeField] private int connectionCount;
    [SerializeField] private int[,] board = new int[6, 6];
    [SerializeField] private Color32 green, red;
    [SerializeField] private int pTurn = 1;
    [SerializeField] public int turnNumber = 1; //turn 1 will be placing first chip, turn 2 will be placing second chip.

    private Color originalColor = new Color32(50, 50, 50, 255);
    private Color p1color = new Color32(248, 148, 211, 255);
    private Color p2color = new Color32(52, 187, 228, 225);
    private Color p1colorF = new Color32(248, 148, 211, 105);
    private Color p2colorF = new Color32(52, 187, 228, 105);
    private Color victoryColor = new Color32(135, 240, 192, 225);
    private Color white = new Color32(255, 255, 255, 225);
    private TextMeshProUGUI console;
    private Selector ss;
    private Image[,] boardobj;
    private int[] numbers;
    
    void Start()
    {
        console = textField.GetComponent<TextMeshProUGUI>();
        textField.SetActive(true);

        //initialize board numbers.
        numbers = new int[36] {1, 2, 3, 4, 5, 6,
                               7, 8, 9, 10, 12, 14,
                               15, 16, 18, 20, 21, 24,
                               25, 27, 28, 30, 32, 35,
                               36, 40, 42, 45, 48, 49,
                               54, 56, 63, 64, 72, 81};
        
        ss = selectorObj.GetComponent<Selector>();
        boardobj = new Image[6, 6];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                boardobj[i, j] = allTiles[i * 6 + j];
            }
        }
        ss.Start(); // call select start 
    }

    private void Update()
    {
        textField.SetActive(true);
    }
    private void NiceTry()
    {
        if (pTurn == 1)
        {
            console.SetText("P1, that tile is taken!");
            console.color = p1color;
        }
        else if (pTurn == 2)
        {
            console.SetText("P2, that tile is taken!");
            console.color = p2color;
        }
    }
    private void Draw()
    {
        string msg = "it is a draw!";
        console.color = white;
        console.SetText(msg);
    }

    [Command (ignoreAuthority = true)]
    public void restartGame() { resetThings(); }

    [ClientRpc]
    public void resetThings() {
        //reset variables:
        pTurn = 1;
        turnNumber = 1; 
        playButton.SetActive(true);
        replayButton.SetActive(false);
        p1chip.GetComponent<Image>().color = p1color;
        p2chip.GetComponent<Image>().color = p2colorF;
        if (isServer && connectionCount == 2) { block.SetActive(false); }
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                boardobj[i, j].color = originalColor;
                board[i, j] = 0;
            }
        }
        if (connectionCount == 2)
        {
            console.SetText("game restarted");
        }
        console.color = white;
        ss.choice1 = 1;
        ss.choice2 = 9;
        ss.IWannaMoveADude(ss.s1, 1, 1);
        ss.IWannaMoveADude(ss.s2, 9, 2);
        ss.prevchoice1 = 1;
        ss.prevchoice2 = 9;
    }

    public void gameResetNoNetwork()
    {
        pTurn = 1;
        turnNumber = 1;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                boardobj[i, j].color = originalColor;
                board[i, j] = 0;
            }
        }
        ss.choice1 = 1;
        ss.choice2 = 9;
        Vector3 pos1 = ss.s1.GetComponent<RectTransform>().position;
        ss.s1.GetComponent<RectTransform>().position = new Vector3(ss.tiles[0].GetComponent<RectTransform>().position.x, pos1.y, pos1.z);
        Vector3 pos2 = ss.s2.GetComponent<RectTransform>().position;
        ss.s2.GetComponent<RectTransform>().position = new Vector3(ss.tiles[8].GetComponent<RectTransform>().position.x, pos2.y, pos2.z);
        ss.prevchoice1 = 1;
        ss.prevchoice2 = 9;
        p1chip.GetComponent<Image>().color = p1color;
        p2chip.GetComponent<Image>().color = p2color;
    }

    [Command (ignoreAuthority = true)]
    public void playMove()
    {
        if(ss.p1down) Debug.Log("ss.p1down is tru");
        else Debug.Log("ss.p1down is fals");
        if(ss.p2down) Debug.Log("ss.p2down is tru");
        else Debug.Log("ss.p2down is fals");
        if(ss.p1down || ss.p2down) return; //don't move if they have selected selector, but no tile!
        //calculate tile to mark
        NChangeVars(ss.choice1, ss.choice2); //update all vars;

        //disable/enable people to play
        NChangeBlock();

        //revert the colour
        buttonExit(GameObject.Find("playMove"));
    }

    [ClientRpc]
    public void NChangeBlock()
    {
        if (pTurn == 1)
        {
            block.SetActive(!isServer);
        }
        else if (pTurn == 2)
        {
            block.SetActive(isServer);
        }
        else
        {
            block.SetActive(true);
        }
    }

    [ClientRpc]
    public void NChangeVars(int c1, int c2)
    {
        int num = c1 * c2;
        int index = 0;
        for (int i = 0; i < 36; i++) { if (numbers[i] == num) { index = i; break; } }

        int r = index / 6;
        int c = index % 6;
        if (board[r, c] == 0)
        {
            //make move if its valid!
            turnNumber++;
            Color currColor;
            if (pTurn == 1) { currColor = p1color; }
            else { currColor = p2color; }
            boardobj[r, c].color = currColor;
            board[r, c] = pTurn;
            console.SetText("");
            if (draw()) 
            {
                p1chip.GetComponent<Image>().color = p1color;
                p2chip.GetComponent<Image>().color = p2color;
                Draw();
                pTurn = 3;
            }
            else if (won(r, c))
            {
                p1chip.GetComponent<Image>().color = p1color;
                p2chip.GetComponent<Image>().color = p2color;
                Win(pTurn);
                pTurn = 3;
                return;
            }

            if (pTurn == 1)
            {
                pTurn = 2;
                p1chip.GetComponent<Image>().color = p1colorF;
                p2chip.GetComponent<Image>().color = p2color;
            }
            else
            {
                pTurn = 1;
                p2chip.GetComponent<Image>().color = p2colorF;
                p1chip.GetComponent<Image>().color = p1color;
            }
            ss.prevchoice1 = ss.choice1; // so that you can't move both on same turn
            ss.prevchoice2 = ss.choice2;
        }
        else
        {
            NiceTry();
        }
    }
    public bool draw() {
        for(int i = 0; i < 6; i++) {
            for(int j = 0; j < 6; j++) {
                if(board[i, j] == 0) return false;
            }
        }
        DrawRoutine();
        return true;
    }
    [Command(ignoreAuthority = true)]
    public void Win(int turn)
    {
        WinChangeText(turn);
    }

    [ClientRpc]
    public void WinChangeText(int turn)
    {
        string msg = "P" + turn.ToString() + " won!";
        console.color = victoryColor;
        console.SetText(msg);
        playButton.SetActive(false); //disable the Place Tile button
        replayButton.SetActive(true); //enable replay button.

    }

    [Command(ignoreAuthority = true)]
    public void WinRoutine(List<int[]> list, bool winByFourPlus)
    {
        WinRoutineAnimated(list, winByFourPlus);
    }

    [ClientRpc]
    public void WinRoutineAnimated(List<int[]> list, bool winByFourPlus)
    {
        //first parameter is the list of coordinates of 4+ in a row
        //second parameter is whether victory is by 4+ in a row, or no more moves for opponent.
        //if second parameter is false, then list is emtpy.

        if (winByFourPlus)
        {
            foreach (var coord in list) boardobj[coord[0], coord[1]].color = victoryColor;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (board[i, j] != 0 && boardobj[i, j].color != victoryColor) boardobj[i, j].color *= 0.5f;
                }
            }
        }
        else
        {
            //do stuff for win by no more moves left

            //nvm just win XD
        }
        //playButton.SetActive(false); //disable the Place Tile button
        //replayButton.SetActive(true); //enable replay button.
    }
    public void DrawRoutine()
    {
        playButton.SetActive(false); //disable the Place Tile button
        replayButton.SetActive(true); //enable replay button.
    }
    public bool won(int r, int c) {
        //win by no more moves for opponent:
        Debug.Log(ss.choice1);
        Debug.Log(ss.choice2);

        bool moveExists = false;
        for(int i = 1; i <= 9; i++) {
            int index1 = 69, index2 = 69;
            for(int j = 0; j < 36; j++) {
                if(numbers[j] == ss.choice1 * i) index1 = j;
                if(numbers[j] == ss.choice2 * i) index2 = j;
            }
            int r1 = index1 / 6, r2 = index2 / 6;
            int c1 = index1 % 6, c2 = index2 % 6;
            if(board[r1, c1] == 0 || board[r2, c2] == 0) {
                moveExists = true;
                break;
            }
        }
        if(!moveExists) {
            WinRoutine(new List<int[]>(), false);
            return true;
        }

        //win by 4+ in a row
        int curr = board[r, c];
        List<int[]> adj = new List<int[]>();

        //horizontal case:
        adj.Add(new int[] {r, c});
        for(int i = c - 1; i >= 0; i--) {
            if(board[r, i] == curr) adj.Insert(0, new int[] {r, i});
            else break;
        }
        for(int i = c + 1; i < 6; i++) {
            if(board[r, i] == curr) adj.Add(new int[] {r, i});
            else break;
        }
        if(adj.Count >= 4) {
            WinRoutine(adj, true);
            return true;
        }

        //vertical case:
        adj.Clear();
        adj.Add(new int[] {r, c});
        for(int i = r - 1; i >= 0; i--) {
            if(board[i, c] == curr) adj.Insert(0, new int[] {i, c});
            else break;
        }
        for(int i = r + 1; i < 6; i++) {
            if(board[i, c] == curr) adj.Add(new int[] {i, c});
            else break;
        }
        if(adj.Count >= 4) {
            WinRoutine(adj, true);
            return true;
        }

        // \diag cases:
        adj.Clear();
        adj.Add(new int[] {r, c});
        for(int i = r - 1, j = c - 1; i >= 0 && j >= 0; i--, j--) {
            if(board[i, j] == curr) adj.Insert(0, new int[] {i, j});
            else break;
        }
        for(int i = r + 1, j = c + 1; i < 6 && j < 6; i++, j++) {
            if(board[i, j] == curr) adj.Add(new int[] {i, j});
            else break;
        }
        if(adj.Count >= 4) {
            WinRoutine(adj, true);
            return true;
        }

        adj.Clear();
        adj.Add(new int[] {r, c});
        for(int i = r - 1, j = c + 1; i >= 0 && j < 6; i--, j++) {
            if(board[i, j] == curr) adj.Insert(0, new int[] {i, j});
            else break;
        }
        for(int i = r + 1, j = c - 1; i < 6 && j >= 0; i++, j--) {
            if(board[i, j] == curr) adj.Add(new int[] {i, j});
            else break;
        }
        if(adj.Count >= 4) {
            WinRoutine(adj, true);
            return true;
        }
        return false;
    }

    public void buttonEnter(GameObject button) {
        if (!block.activeSelf)
        {
            Color col = button.GetComponent<Image>().color;
            button.GetComponent<Image>().color = new Color(col.r, col.g, col.b, 0.75f);
        }
    }

    public void buttonExit(GameObject button)
    {
        Color col = button.GetComponent<Image>().color;
        button.GetComponent<Image>().color = new Color(col.r, col.g, col.b, 1f);
    }

    //lobby stuff update CC here too
    //NetoworkServer.connections.Count
    public void startGame()
    {
        Start();
        try
        {
            NetworkManager.singleton.StartHost();
        }
        catch
        {
            console.SetText("Error when starting game");
            console.color = red;
        }
    }
    public void joinGame()
    {
        Start();
        string ipText = code.GetComponent<TMP_InputField>().text;
        if (ipText.Length > 0)
        {
            NetworkManager.singleton.networkAddress = ipText;
            if (ValidIP(ipText))
            {
                try
                {
                    console.color = white;
                    console.SetText("Trying to connect...");
                    NetworkManager.singleton.StartClient();
                }
                catch
                {
                    console.color = red;
                    console.SetText("Error invalid IP");
                }
            }
            else
            {
                console.color = red;
                console.SetText("Error invalid IP");
            }
        }
        else
        {
            console.color = red;
            console.SetText("Error invalid IP");
        }
    }

    public void leaveGame()
    {
        if (!isServer) // someone left
        {
            DecCount();
            restartGame();
            ClientLeft();
            NetworkManager.singleton.StopClient();
            gameResetNoNetwork();
            console.SetText("you left");
            console.color = white;
        }
        else
        {
            //make all clients leave
            ClientDisc();
            NetworkManager.singleton.StopHost();
        }
    }

    public override void OnStopClient() //client left
    {
        base.OnStopClient();
        gameResetNoNetwork();
        console.SetText("disconnected");
        console.color = green;
        //if (!isServer)
        //{
        //    Debug.Log("client left");
        //}
        HomeScreenButtons();
        connectionCount = 0;
        code.GetComponent<TMP_InputField>().text = "";
    }
    public override void OnStartClient() //client joined
    {
        base.OnStartClient();
        GameSceneButtons();
        if (!isServer)
        {
            console.SetText("connected");
            console.color = green;
            ClientArrivedStart();
        }
        IncCount();
        TryStart();

    }

    public override void OnStartServer() //server started
    {
        base.OnStartServer();
        GameSceneButtons();
        gameResetNoNetwork();
        connectionCount = 0;
        console.SetText("server started\nIP: " + getHostIP());
        console.color = green;
    }


    public override void OnStopServer() //server stopped
    {
        base.OnStopServer();
        gameResetNoNetwork();
        connectionCount = 0;
        HomeScreenButtons();
        console.SetText("server stopped");
        console.color = green;
    }

    [Command (ignoreAuthority =true)]
    public void ClientArrivedStart()
    {
        console.color = green;
        console.SetText("start!");
    }

    [Command(ignoreAuthority = true)]
    public void ClientLeft()
    {
        console.color = red;
        console.SetText("player disconnected!");

    }

    public string getHostIP()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "";
    }

    void OnFailedToConnect()
    {
        console.text = "failed to connect";
        console.color = red;
    }

    public void HomeScreenButtons() //show lobby (disconnected from game)
    {
        block.SetActive(true);
        hostButton.SetActive(true);
        joinButton.SetActive(true);
        playButton.SetActive(false);
        leaveButton.SetActive(false);
        replayButton.SetActive(false);
        code.SetActive(true); ;
    }

    public void GameSceneButtons()
    {
        block.SetActive(true);
        hostButton.SetActive(false);
        joinButton.SetActive(false);
        playButton.SetActive(true);
        leaveButton.SetActive(true);
        replayButton.SetActive(false);
        code.SetActive(false);
    }

    public bool ValidIP(string x)
    {
        if (x == "localhost") return true;
        string[] ipTextSections = x.Split('.');
        if (ipTextSections.Length != 4) return false;
        foreach (var str in ipTextSections)
        {
            if (int.TryParse(str, out int blob) == false) return false;
        }
        return true;
    }

    [ClientRpc]
    public void ClientDisc() //server stops and kicks client
    {
        if (!isServer)
        {
            NetworkManager.singleton.StopClient();
            connectionCount = 0;
            HomeScreenButtons();
        }
    }

    [Command(ignoreAuthority = true)]
    public void IncCount()
    {
        connectionCount++;
    }

    [Command(ignoreAuthority = true)]
    public void DecCount()
    {
        connectionCount--;
        if (connectionCount == 1)
        { 
            block.SetActive(true);
        }
    }

    [Command(ignoreAuthority = true)]
    public void TryStart()
    {
        if (connectionCount == 2)
        {
            if (isServer)
            {
                block.SetActive(false);
                UpdateBoardClients();
            }
        }
        else
        {
            Debug.Log("welcome");
        }
    }

    [ClientRpc]
    public void UpdateBoardClients()
    {
        p1chip.GetComponent<Image>().color = p1color;
        p2chip.GetComponent<Image>().color = p2colorF;
    }
}
