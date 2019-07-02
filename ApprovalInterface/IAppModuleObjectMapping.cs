using ApprovalModel;
using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalInterface
{
    public interface IAppModuleObjectMapping
    {
        List<StaticItem> GetModuleName();
        string SaveMappedBusinessObject(List<AppModuleObjectMapping> objIAppModuleObjectMappingList);
        List<AppModuleObjectMapping> GetMappingBusinessObjectDetails();
    }
}
