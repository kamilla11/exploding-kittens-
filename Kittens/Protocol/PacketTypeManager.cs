namespace Protocol;

public static class PacketTypeManager
{

    static PacketTypeManager()
    {
        RegisterType(PacketType.Handshake,1);
        RegisterType(PacketType.FailConnect, 2);
        RegisterType(PacketType.StartGame, 3);
        RegisterType(PacketType.ActionCard, 4);
        RegisterType(PacketType.SeeTheFuture, 5);
        RegisterType(PacketType.PlayerState, 6);
        RegisterType(PacketType.TakeCard, 7);
        RegisterType(PacketType.ExplodingKitten, 8);
        RegisterType(PacketType.EndGame, 9);
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