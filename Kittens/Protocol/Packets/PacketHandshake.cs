using Protocol.Converter;

namespace Protocol;

public class PacketHandshake
{
    [Field(0)]
    public int MagicHandshakeNumber;

    [Field(1)] public string Id;
    [Field(2)] public string UserName;
}