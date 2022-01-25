using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class UdpProtobuf:Singleton<UdpProtobuf>
{
    public void ParseProtobuf(PBCommon.Csmsgid protocolId, byte[] bodyBuffer)
    {
        int uid;
        int roomid;
        GameRoom gameRoom;
        switch (protocolId) {
            case PBCommon.Csmsgid.UdpBattleReady:
                PBBattle.UdpReadyBattle udpReadyBattle = MessageData.GetDeSerializeData<PBBattle.UdpReadyBattle>(bodyBuffer);
                uid = udpReadyBattle.Uid;
                roomid = udpReadyBattle.Roomid;
                gameRoom = RoomManager.Instance.GetRoom(roomid);
                gameRoom.AddUserReady(uid);
                break;
            case PBCommon.Csmsgid.UdpUpPlayerOperations:
                PBBattle.UdpPlayerOperations playerOperations = MessageData.GetDeSerializeData<PBBattle.UdpPlayerOperations>(bodyBuffer);
                int frame = playerOperations.frameNum;
                roomid = playerOperations.Roomid;
                gameRoom = RoomManager.Instance.GetRoom(roomid);
                gameRoom.AddPlayerOperation(frame,playerOperations.Operation);
                break;
        }
    }
}

