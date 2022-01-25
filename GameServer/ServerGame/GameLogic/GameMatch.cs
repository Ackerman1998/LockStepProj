using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Game Match Queue
/// </summary>
class GameMatch
{
    private int matchNum = 0;
    private int peopleNum = 0;
    private GameType gameType;
    private List<Client> userList = new List<Client>();
    private bool gameIsStart = false;
    public GameMatch(int matchNum,int peopleNum, GameType gameType, Client cc)
    {
        this.matchNum = matchNum;
        this.gameType = gameType;
        this.peopleNum = peopleNum;
        gameIsStart = false;
        cc.matchingNum = matchNum;
        Debug.Log("[Create Match] : " + cc.GetUser().account + " Create Success,Match Number:" + matchNum);
        AddClient(cc,true);
       
    }
    public void AddClient(Client cc,bool self=false) {
        userList.Add(cc);
        cc.matchingNum = matchNum;
        if (userList.Count >= peopleNum)
        {
            gameIsStart = true;
            GameRoom gameRoom =  RoomManager.Instance.GenRoom(gameType, peopleNum);
            int roomId = gameRoom.roomId;
            Debug.Log("[Create Room] : " + cc.GetUser().account + " Create Success,Room Number:" + roomId);
            //send message,create room
            PBHall.TcpResponseMatch tcpResponseMatch = new PBHall.TcpResponseMatch();
            tcpResponseMatch.Result = true;
            tcpResponseMatch.roomId = roomId;
            //broadcast match all client
            foreach (Client _client in userList) {
                gameRoom.AddClient(_client);
                _client.SendMessage(MessageData.GetSendMessage<PBHall.TcpResponseMatch>(tcpResponseMatch,PBCommon.Scmsgid.TcpResponseRequestMatch));
            }
            gameRoom.Broadcast_EnterBattle();
            //解散队列
        }
        else {
            if (!self) {
                //刷新,有人加入进来了
                PBHall.TcpResponseMatching tcpResponseMatching = new PBHall.TcpResponseMatching();
                tcpResponseMatching.currentPeopleNum = userList.Count;
                tcpResponseMatching.Des = "a player join matching";
                foreach (Client _client in userList)
                {
                    _client.SendMessage(MessageData.GetSendMessage<PBHall.TcpResponseMatching>(tcpResponseMatching, PBCommon.Scmsgid.TcpResponseUpdateMatching));
                }
            }
        }
    }

    public void RemoveClient(Client cc) {
        if (!gameIsStart)
        {
            cc.matchingNum = -1;
            userList.Remove(cc);
            PBHall.TcpResponseCancelMatch tcpResponseCancelMatch = new PBHall.TcpResponseCancelMatch();
            tcpResponseCancelMatch.Result = true;
            tcpResponseCancelMatch.Content = "Cancel Success!";
            cc.SendMessage(MessageData.GetSendMessage<PBHall.TcpResponseCancelMatch>(tcpResponseCancelMatch, PBCommon.Scmsgid.TcpResponseUpdateMatching));
        }
        else {
            //cancel failed,can't cancel,start game
            PBHall.TcpResponseCancelMatch tcpResponseCancelMatch = new PBHall.TcpResponseCancelMatch();
            tcpResponseCancelMatch.Result = false;
            tcpResponseCancelMatch.Content = "Game is Start! Can't Start Game";
        }
    }

    public int MatchNum { get => matchNum; set => matchNum = value; }
    public int PeopleNum { get => peopleNum; set => peopleNum = value; }
    public GameType GameType { get => gameType; set => gameType = value; }
    public List<Client> UserList { get => userList; set => userList = value; }
}
