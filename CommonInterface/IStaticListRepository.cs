using CommonModel;
using System;
using System.Collections.Generic;

namespace CommonInterface
{
    public interface IStaticListRepository
    {
        List<StaticDropDownListItem> GetStaticDropDownListByCode(int? pDropDownCode);
    }
}
