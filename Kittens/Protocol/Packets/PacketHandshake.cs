using Protocol.Converter;

namespace Protocol;

public class PacketHandshake
{
    [Field(0)] public string UserName;
    [Field(1)] public string Email;
}