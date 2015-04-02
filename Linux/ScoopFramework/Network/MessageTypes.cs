using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoopFramework.Network
{
    public enum MessageTypes:byte
    {
        Null,PeerListAnswer,PeerListRequest,UserData
    }
}