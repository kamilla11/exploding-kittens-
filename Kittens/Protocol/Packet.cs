using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Protocol;

public class Packet
{
    public byte PacketType { get; private set; }
    public List<PacketField> Fields { get; set; } = new List<PacketField>();
    
    
    private Packet() {}

    public static Packet Create(byte type)
    {
        return new Packet
        {
            PacketType = type
        };
    }
    
    public byte[] ToPacket()
    {
        var packet = new MemoryStream();
 
        packet.Write(
            new byte[] {0xAF, 0xAA, 0xAF, PacketType}, 0, 4);
 
        var fields = Fields.OrderBy(field => field.FieldID);
 
        foreach (var field in fields)
        {
            packet.Write(new[] {field.FieldID, field.FieldSize}, 0, 2);
            packet.Write(field.Contents, 0, field.Contents.Length);
        }
 
        packet.Write(new byte[] {0xFF, 0x00}, 0, 2);
 
        return packet.ToArray();
    }
    
    public static Packet Parse(byte[] packet)
    {
        if (packet.Length < 6)
        {
            return null;
        }
 
        if (packet[0] != 0xAF ||
            packet[1] != 0xAA ||
            packet[2] != 0xAF)
            return null;

 
        var mIndex = packet.Length - 1;
 
        if (packet[mIndex - 1] != 0xFF ||
            packet[mIndex] != 0x00)
            return null;

        var type = packet[3];

        var Packet = Create(type);
        
        var fields = packet.Skip(4).ToArray();
 
        while (true)
        {
            if (fields.Length == 2)
                return Packet;

            var id = fields[0];
            var size = fields[1];
 
            var contents = size != 0 ?
                fields.Skip(2).Take(size).ToArray() : null;
 
            Packet.Fields.Add(new PacketField
            {
                FieldID = id,
                FieldSize = size,
                Contents = contents!
            });
 
            fields = fields.Skip(2 + size).ToArray();
        }
    }
    
    public byte[] FixedObjectToByteArray(object value)
    {
        var json = JsonSerializer.Serialize(value);
        return Encoding.UTF8.GetBytes(json);
    }
    
    private T ByteArrayToFixedObject<T>(byte[] bytes)
    {
        var json = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<T>(json)!;
    }
    
    public PacketField GetField(byte id)
    {
        foreach (var field in Fields)
        {
            if (field.FieldID == id)
            {
                return field;
            }
        }
 
        return null;
    }
    
    public bool HasField(byte id)
    {
        return GetField(id) != null;
    }
    
    public T GetValue<T>(byte id) 
    {
        var field = GetField(id);

        if (field == null)
        {
            throw new Exception($"Field with ID {id} wasn't found.");
        }
        
        return ByteArrayToFixedObject<T>(field.Contents);
    }
    
    public void SetValue(byte id, object structure)
    {
       
        var field = GetField(id);
 
        if (field == null)
        {
            field = new PacketField
            {
                FieldID = id
            };
 
            Fields.Add(field);
        }
 
        var bytes = FixedObjectToByteArray(structure);
 
        if (bytes.Length > byte.MaxValue)
        {
            throw new Exception("Object is too big. Max length is 255 bytes.");
        }
 
        field.FieldSize = (byte) bytes.Length;
        field.Contents = bytes;
    }
}

