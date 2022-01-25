using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/// <summary>
/// Match Mgr
/// </summary>
class MatchManager:Singleton<MatchManager>
{
    private int MAXMatchNum = 100;
    private int startMatchNum = 1000;
    private Dictionary<int,GameMatch> gameMatches = new Dictionary<int, GameMatch>();
    Mutex mutex = new Mutex();
    public void JoinMatch(int gametype,int peopleNum,Client cc) {
        mutex.WaitOne();
        if (gameMatches.Count <= 0)
        {
            int getMatchNum = startMatchNum;
            //no match ,create match
            startMatchNum++;
            GameMatch gameMatch = new GameMatch(getMatchNum, peopleNum, GetGameType(gametype),cc);
            gameMatches.Add(getMatchNum,gameMatch);
        }
        else {
            foreach (GameMatch gg in gameMatches.Values) {
                GameMatch gameMatch = gg;
                if (gameMatch.GameType == GetGameType(gametype)&& gameMatch.UserList.Count< gameMatch.PeopleNum) {
                    gg.AddClient(cc);
                    break;
                } 
            }
        }
        mutex.ReleaseMutex();
    }

    public void CancelMatch(Client cc) {
        mutex.WaitOne();
        if (cc.matchingNum == -1&& !gameMatches.ContainsKey(cc.matchingNum))
        {
            PBHall.TcpResponseCancelMatch tcpResponseCancelMatch = new PBHall.TcpResponseCancelMatch();
            tcpResponseCancelMatch.Result = false;
            tcpResponseCancelMatch.Content = "Can't Cancel!";
        }
        else {
            gameMatches[cc.matchingNum].RemoveClient(cc);
        }
        mutex.ReleaseMutex();
    }

    private GameType GetGameType(int type) {
        if (type == 1) {
            return GameType.MatchGame;
        } else if (type==2) {
            return GameType.RankGame;
        }
        return GameType.MatchGame;
    }
}
public enum GameType { 
    MatchGame,
    RankGame
}