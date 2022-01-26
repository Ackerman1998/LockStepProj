using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
class GameRoom
{
    public List<Client> client_List = new List<Client>();
    private Dictionary<int, Udp_Client> udpClients = new Dictionary<int, Udp_Client>();
    public int roomId;
    private int game_frame = 0;
    private bool startGame = false;
    public GameType type_match;
    private bool gameIsOver = false;
    //private int[] playerFrameNum;
    private Dictionary<int, int> playerFrameNum;
    private Dictionary<int, PBBattle.PlayerOperation> playerOperations;
    public int totalPeopleNumber;
    public GameRoom(int rooId, GameType type,int totalPeopleNumber) {
        this.roomId = rooId;
        this.type_match = type;
        this.totalPeopleNumber = totalPeopleNumber;
        startGame = false;
    }
    ~GameRoom() {
        this.roomId = 0;
        this.type_match = 0;
        this.totalPeopleNumber = 0;
        startGame = false;
        client_List.Clear();
    }
    public void AddClient(Client cc) {
        client_List.Add(cc);
    }
    public void RemoveClient() { 
    
    }
    /// <summary>
    /// 广播事件-EnterBattle
    /// </summary>
    public void Broadcast_EnterBattle() {
        Debug.Log("[Create Battle] : " + roomId + " Create Success,Room Number:" + roomId);
        Debug.Log("client_List.count="+client_List.Count);
        PBBattle.TcpEnterBattle tcpEnterBattle = new PBBattle.TcpEnterBattle();
        Random random = new Random();
        int seed = random.Next(0,100);
        tcpEnterBattle.randSeed = seed;
        foreach (Client cc in client_List) {
            PBBattle.BattleUserInfo battleUserInfo = new PBBattle.BattleUserInfo();
            battleUserInfo.Uid = int.Parse(cc.GetUser().uid);
            battleUserInfo.roleID = 1;//默认为1
            tcpEnterBattle.battleUserInfoes.Add(battleUserInfo);
            Udp_Client udp_Client = new Udp_Client(cc.GetIpAddress(),cc.GetUdpPort());
            udpClients.Add(battleUserInfo.Uid,udp_Client);
        }
        foreach (Client cc in client_List) {
            cc.SendMessage(MessageData.GetSendMessage<PBBattle.TcpEnterBattle>(tcpEnterBattle,PBCommon.Scmsgid.TcpEnterBattle));
        }
    }

    public void AddUserReady(int uid) {
        if (startGame) return;
        if (udpClients.ContainsKey(uid)) {
            udpClients[uid].Ready = true;
        }
        int readyPeople = 0;
        //check ready state
        foreach (Udp_Client udp_Client in udpClients.Values)
        {
            if (udp_Client.Ready) {
                readyPeople++;
            }
        }
        if (readyPeople >= totalPeopleNumber)
        {
            //ok,start game
            startGame = true;
            StartBattle();
        }
        else {
            
        }
    }
    /// <summary>
    /// start battle and init data
    /// </summary>
    private void StartBattle() {
        game_frame = 0;
        playerOperations = new Dictionary<int, PBBattle.PlayerOperation>();
        //playerFrameNum = new int[totalPeopleNumber];
        playerFrameNum = new Dictionary<int, int>();
        foreach (Client cc in client_List) {
            playerFrameNum.Add(int.Parse(cc.GetUser().uid),0);
            playerOperations.Add(int.Parse(cc.GetUser().uid),null);
        }
        Thread thread = new Thread(BattleOperation);
        thread.Start();
    }
    private void BattleOperation() {
        PBBattle.UdpReadyBattleResponse udpReadyBattleResponse = new PBBattle.UdpReadyBattleResponse();
        udpReadyBattleResponse.Result = true;
        bool sendReadyBattleResponse = false;
        while (!sendReadyBattleResponse) {
            foreach (Udp_Client udp_Client in udpClients.Values) {
                udp_Client.SendMessage(MessageData.GetSendMessage<PBBattle.UdpReadyBattleResponse>(udpReadyBattleResponse,PBCommon.Scmsgid.UdpBattleStart));
            }
            bool receiveAllClientResponse = true;
            //接收第一次操作
            foreach (PBBattle.PlayerOperation playerOperation in playerOperations.Values) {
                if (playerOperation==null) {
                    receiveAllClientResponse = false;
                    break;
                }
            }
            if (!receiveAllClientResponse)
            {
                sendReadyBattleResponse = false;
            }
            else {
                sendReadyBattleResponse = true;
                game_frame = 1;
            }

            Thread.Sleep(500);
        }
        //start send frame data to all users
        Debug.Log("All Client Response Complete,Start Send FrameData to AllUsers");
        while (startGame) {
            PBBattle.UdpAllPlayerOperations udpAllPlayerOperations = new PBBattle.UdpAllPlayerOperations();
            if (gameIsOver) {

            }
            else {
                udpAllPlayerOperations.frameID = game_frame;
                game_frame++;
            }
            udpAllPlayerOperations.Operations = GetAllPlayerOperations();
            foreach (Udp_Client udp_Client in udpClients.Values) {
                udp_Client.SendMessage(MessageData.GetSendMessage<PBBattle.UdpAllPlayerOperations>(udpAllPlayerOperations,PBCommon.Scmsgid.UdpDownFrameOperations));
            }
            Debug.Log("Send Frame Data : [FrameID :"+ game_frame +" ],Success");
            Thread.Sleep(ServerConfig.frameMillSenconds);
        }
    }

    public void AddPlayerOperation(int frameNum,PBBattle.PlayerOperation playerOperation) {
        int uid = playerOperation.Uid;
        if (frameNum > playerFrameNum[uid])
        {
            playerOperations[uid] = playerOperation;
            playerFrameNum[uid] = frameNum;
        }
        else { 
        
        }
    }

    private PBBattle.AllPlayerOperation GetAllPlayerOperations() {
        PBBattle.AllPlayerOperation allPlayerOperation = new PBBattle.AllPlayerOperation();
        allPlayerOperation.Operations.AddRange(playerOperations.Values.ToList());
        return allPlayerOperation;
    }
}
