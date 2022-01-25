using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
 
public class MessageData 
{
    public static byte[] GetSendMessage<T>(T pbMsg, PBCommon.Csmsgid id)
    {
        byte[] msgBody = SerializeData<T>(pbMsg);
        byte byte_MsgId = (byte)id;
        int totalLen = msgBody.Length + 1;
        byte[] byte_totalLen = BitConverter.GetBytes(totalLen);
        List<byte> messageList = new List<byte>();
        messageList.AddRange(byte_totalLen);
        messageList.Add(byte_MsgId);
        messageList.AddRange(msgBody);
        return messageList.ToArray();
    }

    public static byte[] SerializeData<T>(T obj)
    {
        byte[] buffer;
        using (MemoryStream ms = new MemoryStream())
        {
            ProtoBuf.Serializer.Serialize(ms,obj);
            buffer = new byte[ms.Position];
            byte[] bb = ms.GetBuffer();
            Array.Copy(bb, buffer, buffer.Length);
        }
        return buffer;
    }

    public static T GetDeSerializeData<T>(byte [] buffer) {
        using (Stream ms = new MemoryStream(buffer)) {
            return ProtoBuf.Serializer.Deserialize<T>(ms);
        }
    }
}
