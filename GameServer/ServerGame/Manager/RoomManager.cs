using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class RoomManager:Singleton<RoomManager>
{
    private Dictionary<int, GameRoom> gameRoomDict = new Dictionary<int, GameRoom>();
    private int roomMaxNum = 100;
    private int roomStartIndex = 1000;
    public GameRoom GenRoom(GameType game_type, int totalPeopleNum) {
        int roomId = 0;
        if (gameRoomDict.Count>=100) {//max
            return null;
        }
        roomId = roomStartIndex;
        roomStartIndex++;
        GameRoom gameRoom = new GameRoom(roomId, game_type, totalPeopleNum);
        gameRoomDict.Add(roomId,gameRoom);
        return gameRoom;
    }

    public GameRoom GetRoom(int roomid) {
        if (gameRoomDict.ContainsKey(roomid))
        {
            return gameRoomDict[roomid];
        }
        else {
            return null;
        }
    }

    
}
