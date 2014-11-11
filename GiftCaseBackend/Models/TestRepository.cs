using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftCaseBackend.Models
{
    public class TestRepository
    {
        public static List<Gift> Gifts = new List<Gift>()
        {
            new Gift()
            {
                Category = GiftCategory.Book,
                LinkToTheStore = "http://www.amazon.co.uk/dp/1846573785 ",
                Name = "Fifty Shades of Grey",
                Status = GiftStatus.NotReceivedYet,
                Store = Store.Amazon
            },
            new Gift()
            {
                Category = GiftCategory.Book,
                LinkToTheStore = "http://www.amazon.co.uk/dp/1408855658 ",
                Name = "Harry Potter and the Philosopher's Stone",
                Status = GiftStatus.NotReceivedYet,
                Store = Store.Amazon
            },
            new Gift()
            {
                Category = GiftCategory.Movie,
                LinkToTheStore = "http://www.amazon.co.uk/dp/B00F3TCF7O ",
                Name = "Captain America: The First Avenger",
                Status = GiftStatus.NotReceivedYet,
                Store = Store.Amazon
            }
        };
    }
}