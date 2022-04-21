using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetJoin : NetMessage
{
    public Player JoinedPlayer { set; get; }

    public NetJoin(Player joinedPlayer)
    {
        this.Code = OpCode.JOIN;
        this.JoinedPlayer = joinedPlayer;
    }

    public NetJoin(DataStreamReader reader)
    {
        this.Code = OpCode.JOIN;
        this.Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);

        Player.SerializePlayer(ref writer, this.JoinedPlayer);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        this.JoinedPlayer = Player.DeserializePlayer(reader);
    }

    public override void ReceivedOnClient()
    {
        base.ReceivedOnClient();

        NetUtility.C_JOIN?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        base.ReceivedOnServer(cnn);

        NetUtility.S_JOIN?.Invoke(this, cnn);
    }
}
