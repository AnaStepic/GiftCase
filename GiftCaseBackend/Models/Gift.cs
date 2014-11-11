using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftCaseBackend.Models
{
    public class Gift
    {
        public string Name { get; set; }

        public GiftStatus Status { get; set; }
    }

    public enum GiftStatus
    {
        NotReceivedYet, // gift has been sent but the user getting it has not yet seen it
        Received, // the user getting the gift has seen the gift in his app but has not claimed it yet
        Claimed // user downloaded/claimed the gift he has received
    }
}