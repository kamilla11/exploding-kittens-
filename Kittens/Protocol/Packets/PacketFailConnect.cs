using Protocol.Converter;

namespace Protocol;

public class PacketFailConnect
{
    [Field(0)] public string Exception;
}