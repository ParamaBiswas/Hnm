using PMSModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSInterface
{
    public interface IItemReceiveRepository
    {
        string SaveItemReceive(ItemReceive objItemReceive);
        List<ItemReceive> GetItemReceiveByPO(string pPOCode);
    }
}
