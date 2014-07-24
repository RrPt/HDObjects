using System;
using System.Collections.Generic;
using System.Text;
using EIBDef;
using System.Net;

namespace HomeData
{
    enum HdAdrTyp
    {
        none = 0, KnxAdr=1, IP=2, TCP=3, unknown = 255
    }

    public class HdAdr
    {
        HdAdrTyp AdrTyp = HdAdrTyp.unknown;

        virtual public byte[] ToByte()
        {
            throw new Exception("Muss überschrieben werden");
        }
    }

    class KnxGroupAdr : HdAdr
    {
        public EIB_Adress adr;
    }


    class IpAdr : HdAdr
    {
        public IPAddress adr;
    }

    class TcpAdr : IpAdr
    {
        public int port;
    }



}
