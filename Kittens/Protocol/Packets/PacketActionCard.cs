using Protocol.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Packets
{
    public class PacketActionCard
    {
        [Field(0)] public string ActionCard;
    }
}
