using KittensLibrary;
using Protocol.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Packets
{
    public class PacketStartGame
    {
        [Field(0)] public Player Player;
        [Field(1)] public int OtherPlayerCardsCount;
    }
}
