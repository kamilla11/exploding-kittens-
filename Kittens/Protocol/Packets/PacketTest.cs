using Protocol.Converter;

namespace Protocol;

public class PacketTest
{
    [Field(0)]
    public int TestNumber;

    [Field(1)]
    public double TestDouble;

    [Field(2)]
    public bool TestBoolean;
}