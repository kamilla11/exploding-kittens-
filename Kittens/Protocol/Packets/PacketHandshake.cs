using Protocol.Converter;

namespace Protocol;

public class PacketHandshake
{

    [Field(0)] public string Id;
    [Field(1)] public string UserName;
}