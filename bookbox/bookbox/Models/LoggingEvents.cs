using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public class LoggingEvents
    {
        public const int GenerateItems = 1000;
        public const int ListItems = 1001;
        public const int GetItem = 1002;
        public const int InsertItem = 1003;
        public const int UpdateItem = 1004;
        public const int DeleteItem = 1005;
        public const int Logout = 1006;

        public const int CreateUpdateItemFailed = 2000;
        public const int DeleteItemFailed = 2001;

        public const int GetItemNotFound = 4000;
        public const int UpdateItemNotFound = 4001;
    }
}
