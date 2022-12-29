namespace Protocol.Converter;

[AttributeUsage(AttributeTargets.Field)]
public class FieldAttribute : Attribute
{
    public byte FieldID { get; }
 
    public FieldAttribute(byte fieldId)
    {
        FieldID = fieldId;
    }
}