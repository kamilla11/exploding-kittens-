namespace Protocol;

public static class PacketTypeManager
{

    static PacketTypeManager()
    {
        RegisterType(PacketType.Handshake,1);
        RegisterType(PacketType.FailConnect, 2);
    }
    
    private static readonly Dictionary<PacketType,byte> TypeDictionary = new ();
    public static void RegisterType(PacketType type, byte btype)
    {
        if (TypeDictionary.ContainsKey(type))
        {
            throw new Exception($"Packet type {type:G} is already registered.");
        }
 
        TypeDictionary[type] = btype;
    }
    
    public static byte GetType(PacketType type)
    {
        if (!TypeDictionary.ContainsKey(type))
        {
            throw new Exception($"Packet type {type:G} is not registered.");
        }
 
        return TypeDictionary[type];
    }
    
    public static PacketType GetTypeFromPacket(Packet packet)
    {
        var type = packet.PacketType;

        foreach (var tuple in TypeDictionary)
        {
            if (tuple.Value == type )
                return tuple.Key;
        }
 
        return PacketType.Unknown;
    }
}