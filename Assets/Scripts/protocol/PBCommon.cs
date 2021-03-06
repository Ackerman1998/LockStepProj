// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: PBCommon.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace PBCommon
{

    [global::ProtoBuf.ProtoContract(Name = @"CSMSGID")]
    public enum Csmsgid
    {
        [global::ProtoBuf.ProtoEnum(Name = @"TCP_REQUEST_Connect")]
        TCPREQUESTConnect = 0,
        [global::ProtoBuf.ProtoEnum(Name = @"TCP_REQUEST_LOGIN")]
        TcpRequestLogin = 1,
        [global::ProtoBuf.ProtoEnum(Name = @"TCP_REQUEST_MATCH")]
        TcpRequestMatch = 10,
        [global::ProtoBuf.ProtoEnum(Name = @"TCP_CANCEL_MATCH")]
        TcpCancelMatch = 11,
        [global::ProtoBuf.ProtoEnum(Name = @"UDP_BATTLE_READY")]
        UdpBattleReady = 51,
        [global::ProtoBuf.ProtoEnum(Name = @"UDP_UP_PLAYER_OPERATIONS")]
        UdpUpPlayerOperations = 53,
        [global::ProtoBuf.ProtoEnum(Name = @"UDP_UP_DELTA_FRAMES")]
        UdpUpDeltaFrames = 55,
        [global::ProtoBuf.ProtoEnum(Name = @"UDP_UP_GAME_OVER")]
        UdpUpGameOver = 57,
    }

    [global::ProtoBuf.ProtoContract(Name = @"SCMSGID")]
    public enum Scmsgid
    {
        [global::ProtoBuf.ProtoEnum(Name = @"TCP_RESPONSE_LOGIN")]
        TcpResponseLogin = 1,
        [global::ProtoBuf.ProtoEnum(Name = @"TCP_RESPONSE_REQUEST_MATCH")]
        TcpResponseRequestMatch = 10,
        [global::ProtoBuf.ProtoEnum(Name = @"TCP_RESPONSE_CANCEL_MATCH")]
        TcpResponseCancelMatch = 11,
        [global::ProtoBuf.ProtoEnum(Name = @"TCP_RESPONSE_UPDATE_MATCHING")]
        TcpResponseUpdateMatching = 12,
        [global::ProtoBuf.ProtoEnum(Name = @"TCP_ENTER_BATTLE")]
        TcpEnterBattle = 50,
        [global::ProtoBuf.ProtoEnum(Name = @"UDP_BATTLE_START")]
        UdpBattleStart = 51,
        [global::ProtoBuf.ProtoEnum(Name = @"UDP_DOWN_FRAME_OPERATIONS")]
        UdpDownFrameOperations = 53,
        [global::ProtoBuf.ProtoEnum(Name = @"UDP_DOWN_DELTA_FRAMES")]
        UdpDownDeltaFrames = 55,
        [global::ProtoBuf.ProtoEnum(Name = @"UDP_DOWN_GAME_OVER")]
        UdpDownGameOver = 57,
    }

    [global::ProtoBuf.ProtoContract()]
    public enum TeamType
    {
        [global::ProtoBuf.ProtoEnum(Name = @"NO")]
        No = 0,
        [global::ProtoBuf.ProtoEnum(Name = @"TEAM1")]
        Team1 = 1,
        [global::ProtoBuf.ProtoEnum(Name = @"TEAM2")]
        Team2 = 2,
        [global::ProtoBuf.ProtoEnum(Name = @"TEAM3")]
        Team3 = 3,
        [global::ProtoBuf.ProtoEnum(Name = @"TEAM4")]
        Team4 = 4,
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
