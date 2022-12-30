using KittensLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Packets
{
    internal class PacketPlayerState
    {
        [field(0)] public List<Card> Cards
    }
}
