using Protocol.Converter;

namespace Protocol;

public class ReadyToStart
{
    [Field(0)] 
    public int Id;
    [Field(1)] 
    public string UserName;
    
}