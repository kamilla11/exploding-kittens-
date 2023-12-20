using KittensLibrary;
using Protocol.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Packets
{
    public class PacketExplodingKitten
    {
        [Field(0)] public State State;
    }
}
