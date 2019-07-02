using ApprovalDataContext;
using ApprovalInterface;
using ApprovalModel;
using CommonModel;
using ConnectionGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalApproverSelectionController : ControllerBase
    {
        IAppLevelDefDetAppType _appLevelDefDetAppType;
        ISupplierDbContext _supplierDbContext;
        IAppObjInfoMap_Logic _appObjInfoMap_Logic;
        IAppLevelDefinition _appLevelDefinition;
        public static List<StaticItem> Value1List_VW;
        public static List<StaticItem> Value2List_VW;
        public static List<StaticItem> Value3List_VW;
        public ApprovalApproverSelectionController(ISupplierDbContext supplierDbContext, IAppLevelDefDetAppType appLevelDefDetAppType, IAppObjInfoMap_Logic appObjInfoMap_Logic,IAppLevelDefinition appLevelDefinition)
        {
            _appLevelDefDetAppType = appLevelDefDetAppType;
            _supplierDbContext = supplierDbContext;
            _appObjInfoMap_Logic = appObjInfoMap_Logic;
            _appLevelDefinition = appLevelDefinition;
        }
        [HttpGet]
        [Route("ROK")]
        public IActionResult ROk()
        {
            string s = "its ok";
            return Ok(new
            {
                s   
            });
        }
        // [HttpPost]
        //[Route("SaveApproverSelectionInformations")]
        //public IActionResult SaveApproverSelectionInformations(AppLevelDefDetAppType appLevelDefDet)
        //{
        //    string vMSG = "";
        //    int vResult = 0;
        //    List<AppLevelDefDetAppType> appLevelDefDetAppTypes = new List<AppLevelDefDetAppType>();


        //        foreach (AppLevelDetAppTypeApprover objdtl in appLevelDefDet.objAppLevelDetAppTypeApproverList_VW)
        //        {
        //            AppLevelDefDetAppType appLevelDefDetnew = new AppLevelDefDetAppType();
        //            appLevelDefDetnew = appLevelDefDet;
        //            appLevelDefDetnew.ApproverLevelNo = objdtl.LevelNo;
        //            appLevelDefDetnew.ApproverType = objdtl.ApproverTypeVal_VW;

        //            if (objdtl.IsNew)
        //            {
        //                objdtl.AppLvDetAppTypeApprover_PK = Guid.NewGuid().ToString();
        //                appLevelDefDetnew.AppLvDefDetAppTypeCode_PK = Guid.NewGuid().ToString();

        //            }
        //            if (string.IsNullOrEmpty(objdtl.AppLvDefDetAppTypeCode_FK))
        //            {
        //                objdtl.AppLvDefDetAppTypeCode_FK = appLevelDefDetnew.AppLvDefDetAppTypeCode_PK;

        //            }

        //            objdtl.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
        //            objdtl.UserCode = "abcd";
        //            objdtl.CompanyCode_FK = 1;
        //            appLevelDefDetnew.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
        //            appLevelDefDetnew.UserCode = "abcd";
        //            appLevelDefDetnew.CompanyCode_FK = 1;
        //        appLevelDefDetnew.objAppLevelDetAppTypeApproverList_VW = new List<AppLevelDetAppTypeApprover>();
        //        appLevelDefDetnew.objAppLevelDetAppTypeApproverList_VW.Add(objdtl);
        //        appLevelDefDetAppTypes.Add(appLevelDefDetnew);
        //    }



        //    vMSG = _appLevelDefDetAppType.SaveApproverSelection(appLevelDefDetAppTypes);


        //    if (vMSG.Contains("Information Saved Successfully"))
        //    {
        //        vResult = 1;
        //        vMSG = "Data Saved Successfully";
        //    }

        //    else
        //    {
        //        vMSG = "Cannot save data.";

        //    }

        //    return Ok(new
        //    {
        //        vResult,
        //        vMSG
        //        //ID = appLevelDefDet.AppLvDefDetAppTypeCode_PK
        //    });

        //}
        [HttpPost]
        [Route("SaveApproverSelectionInformations")]
        public IActionResult SaveApproverSelectionInformations(ListModel objList)
        {
            string vMSG = "";
            int vResult = 0;
            foreach (AppLevelDefDetAppType obj in objList.appLevelDefDet)
            {
                if (obj.IsNew)
                {
                    obj.AppLvDefDetAppTypeCode_PK = Guid.NewGuid().ToString();

                }
                foreach (AppLevelDetAppTypeApprover objdtl in obj.objAppLevelDetAppTypeApproverList_VW)
                {
                    if (objdtl.IsNew)
                    {
                        objdtl.AppLvDetAppTypeApprover_PK = Guid.NewGuid().ToString();

                    }

                    if (string.IsNullOrEmpty(objdtl.AppLvDefDetAppTypeCode_FK))
                    {
                        objdtl.AppLvDefDetAppTypeCode_FK = obj.AppLvDefDetAppTypeCode_PK;

                    }
                    objdtl.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                    objdtl.UserCode = "abcd";
                    objdtl.CompanyCode_FK = 1;
                }
                obj.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                //appLevelDefDet.ActionType = ;
                obj.UserCode = "abcd";
                obj.CompanyCode_FK = 1;
            }





            vMSG = _appLevelDefDetAppType.SaveApproverSelection(objList.appLevelDefDet);


            if (vMSG.Contains("Information Saved Successfully"))
            {
                vResult = 1;
                vMSG = "Data Saved Successfully";
            }

            else
            {
                vMSG = "Cannot save data.";
                //if (appLevelDefDet.IsNew == true)
                //{
                //    appLevelDefDet.AppLvDefDetAppTypeCode_PK = null;

                //    foreach (AppLevelDetAppTypeApprover objIApp in objApproverSelectionViewData.AppLevelDetAppTypeApproverList)
                //    {
                //        if (objIApp.IsNew == true)
                //        {
                //            objIApp.IS_NEW_Test_VW = "1";
                //        }

                //    }
                //}

            }

            return Ok(new
            {
                vResult,
                vMSG
                // ID = appLevelDefDet.AppLvDefDetAppTypeCode_PK
            });

        }

        //[HttpPost]
        //[Route("DeleteApproverSelectionInformations")]
        //public ActionResult DeleteApproverSelectionInformations(List<AppLevelDefDetAppType> appLevelDefDetList)
        //{
        //    string vMSG = "";
        //    bool vResult = false;
        //    foreach (AppLevelDefDetAppType obj in appLevelDefDetList)
        //    {
        //        foreach (AppLevelDetAppTypeApprover objdtl in obj.objAppLevelDetAppTypeApproverList_VW)
        //        {
        //            objdtl.IsNew = false;
        //            objdtl.IsDeleted = true;
        //            objdtl.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
        //            objdtl.UserCode = "abcd";
        //            objdtl.CompanyCode_FK = 1;
        //        }

        //        obj.IsNew = false;
        //        obj.IsDeleted = true;
        //        obj.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
        //        obj.UserCode = "abcd";
        //        obj.CompanyCode_FK = 1;
        //    }
               
        //    int vIsTransactionFound = 0;

        //    vIsTransactionFound = _appLevelDefDetAppType.IsTransactionFound(appLevelDefDetList[0].AppLvDefinitionDetCode_FK);
        //    //not sure why this block should be done; this is done following the previous code
        //    if (string.IsNullOrEmpty(appLevelDefDetList[0].AppLvDefDetAppTypeCode_PK) && vIsTransactionFound == 0)
        //    {
        //        vMSG = _appLevelDefDetAppType.SaveApproverSelection(appLevelDefDetList);

        //        if (vMSG.Contains("Information Saved Successfully"))
        //        {
        //            vMSG = "Data Deleted Successfully";
        //            vResult = true;
        //        }
        //    }
        //    else
        //    {
        //        if (vIsTransactionFound == 1)
        //            vMSG = "Unapproved Data found.So can not be deleted";
        //        else
        //            vMSG = vMSG + "Data cant be deleted";

        //    }

        //    return Ok(new
        //    {
        //        vResult,
        //        vMSG
        //    });

        //}

        //[HttpPost]
        //[Route("GetApproverSelectionInformations")]
        //public ActionResult GetApproverSelectionInformations(string pAppLvDefDetAppTypeCode_PK, string pModuleObjId_VW, string pBusinessObjectName_VW, string pModuleName_VW, int? pSerialNo, string pAppLevelType_VW, int? pNoOfAppLevel_VW, string pApproverEmpName_VW) //, string pApproverEmpName_VW
        //{

        //    string vMSG = "";
        //    int vResult = 0;
        //    string ObjectmapCode = string.Empty;


        //    if (SessionUtility.SessionContainer.OBJ_CLASS != null)
        //    {
        //        objApproverSelectionViewData = (ApproverSelectionViewData)SessionUtility.SessionContainer.OBJ_CLASS;
        //        if (objApproverSelectionViewData.objIAppLevelDefDetAppType != null)
        //        {
        //            ObjectmapCode = objApproverSelectionViewData.objIAppLevelDefDetAppType.ModuleObjMapCode_VW;
        //        }
        //    }
        //    objApproverSelectionViewData.AppLevelDetAppTypeApproverList = new List<IAppLevelDetAppTypeApprover>();

        //    objApproverSelectionViewData.objIAppLevelDefDetAppType = objIAppLevelDefDetAppType;
        //    objApproverSelectionViewData.objAppLevelDetAppTypeApprover = objIAppLevelDetAppTypeApprover;
        //    objApproverSelectionViewData.objIBusinessObject = objOfModuleObjectMapping;

        //    //value Assign becoz of page refresh  === First pop up

        //    objApproverSelectionViewData.objIAppLevelDefDetAppType.ModuleObjId_VW = pModuleObjId_VW;
        //    objApproverSelectionViewData.objIAppLevelDefDetAppType.BusinessObjectName_VW = pBusinessObjectName_VW;
        //    objApproverSelectionViewData.objIAppLevelDefDetAppType.ModuleName_VW = pModuleName_VW;
        //    objApproverSelectionViewData.objIAppLevelDefDetAppType.ModuleObjMapCode_VW = ObjectmapCode;

        //    //value Assign becoz of page refresh  === 2nd pop up

        //    objApproverSelectionViewData.objIAppLevelDefDetAppType.SLNo_VW = pSerialNo;
        //    objApproverSelectionViewData.objIAppLevelDefDetAppType.NoOfAppLevel_VW = pNoOfAppLevel_VW;
        //    objApproverSelectionViewData.objIAppLevelDefDetAppType.AppLevelType_VW = pAppLevelType_VW;

        //    //dynamic pop up making

        //    objApproverSelectionViewData.objIAppLevelDefDetAppType.ApproverLevelNo_VW = new List<StaticItem>();
        //    objIAppLevelDefDetAppType.ApproverLevelNo_VW = new List<StaticItem>();

        //    StaticItem obj1 = new StaticItem();

        //    for (int i = 1; i <= pNoOfAppLevel_VW; i++)
        //    {
        //        obj1 = new StaticItem();
        //        obj1.DataValue = i.ToString();
        //        obj1.TextValue = i.ToString();
        //        objApproverSelectionViewData.objIAppLevelDefDetAppType.ApproverLevelNo_VW.Add(obj1);

        //    }

        //    //call DC Get Method

        //    if (!String.IsNullOrEmpty(pAppLvDefDetAppTypeCode_PK))
        //    {
        //        objIAppLevelDefDetAppType.AppLvDefinitionDetCode_FK = pAppLvDefDetAppTypeCode_PK;
        //        objApproverSelectionViewData.AppLevelDefDetAppTypeList = objIAppLevelDefDetAppType.GetApproverSelection(objIAppLevelDefDetAppType);
        //    }


        //    //Assign && Find values

        //    if (objApproverSelectionViewData.AppLevelDefDetAppTypeList != null)
        //    {
        //        foreach (IAppLevelDefDetAppType obj in objApproverSelectionViewData.AppLevelDefDetAppTypeList)
        //        {
        //            if (obj.objIAppLevelDetAppTypeApproverList_VW != null && obj.objIAppLevelDetAppTypeApproverList_VW.Count > 0)
        //            {

        //                foreach (IAppLevelDetAppTypeApprover objdtl in obj.objIAppLevelDetAppTypeApproverList_VW)
        //                {

        //                    objdtl.ApproverType_VW = obj.ApproverType_VW.Find(a => a.DataValue == obj.ApproverType.ToString()).TextValue;

        //                    //   objdtl.LevelNo = obj.ApproverLevelNo.ToString();
        //                    objdtl.LevelNo = Convert.ToInt32(obj.ApproverLevelNo);



        //                    if (objdtl.DesignationType == -1)
        //                    {
        //                        objdtl.DesignationType_Name_VW = "";
        //                    }
        //                    else if (objdtl.DesignationType != 0)
        //                    {
        //                        objdtl.DesignationType_Name_VW = objdtl.DesignationType_VW.Find(c => c.DataValue == objdtl.DesignationType.ToString()).TextValue;
        //                    }
        //                    else
        //                    {
        //                        objdtl.DesignationType_Name_VW = "";
        //                    }

        //                    objApproverSelectionViewData.AppLevelDetAppTypeApproverList.Add(objdtl);
        //                }
        //            }
        //            else
        //            {
        //                IAppLevelDetAppTypeApprover objdtl = ApprovalFactory.InitiateAppLevelDetAppTypeApprover();
        //                objdtl.ApproverType_VW = obj.ApproverType_VW.Find(a => a.DataValue == obj.ApproverType.ToString()).TextValue;
        //                //  objdtl.ApproverLevelNo_VW = obj.ApproverLevelNo.ToString();
        //                objdtl.LevelNo = Convert.ToInt32(obj.ApproverLevelNo);

        //                objApproverSelectionViewData.AppLevelDetAppTypeApproverList.Add(objdtl);
        //            }
        //        }
        //    }


        //    SessionUtility.SessionContainer.OBJ_CLASS = objApproverSelectionViewData;
        //    ViewData["Title"] = "Approver Selection";
        //    return View("ApproverSelectionUI", objApproverSelectionViewData);

        //}
        [HttpGet]
        [Route("GetObjAppInfoMapLogicValue")]
        public ActionResult GetObjAppInfoMapLogicValue(string pModuleObjMapCode)
        {
            bool vResult = true;
            string vMSG="";
            AppObjInfoMap_Logic objIAppObjInfoMap_Logic = new AppObjInfoMap_Logic();
            AppLevelDefinition objAppLevelDefinition = new AppLevelDefinition();
            AppLevelDefinitionDet objAppLevelDefinitionDet = new AppLevelDefinitionDet();
            objIAppObjInfoMap_Logic.ModuleObjMapCode_FK = pModuleObjMapCode;
            try
            {
                //AppLevelDefinition objAppLevelDefinition = new AppLevelDefinition();
                objIAppObjInfoMap_Logic = _appObjInfoMap_Logic.GetIAppObjInfoMap_Logic(objIAppObjInfoMap_Logic);
                    if (objIAppObjInfoMap_Logic == null)
                    {
                        vMSG = "This object is not mapping for approval";
                        vResult = false;
                    }
                    else
                    {
                        if (objIAppObjInfoMap_Logic.IsFieldPolicy == 1 && objIAppObjInfoMap_Logic.AppObjInfoMap_LogicValues_List_VW != null && objIAppObjInfoMap_Logic.AppObjInfoMap_LogicValues_List_VW.Count > 0)
                        {
                            GetLogicvalues(objIAppObjInfoMap_Logic);
                        }
                         //AppLevelDefinition objAppLevelDefinition = new AppLevelDefinition();
                         objAppLevelDefinition.ModuleObjMapCode_FK = objIAppObjInfoMap_Logic.ModuleObjMapCode_FK;
                         objAppLevelDefinition = _appLevelDefinition.GetAppLevelDefinition(objAppLevelDefinition);

                        if (objAppLevelDefinition == null || string.IsNullOrEmpty(objAppLevelDefinition.AppLvDefinitionCode_PK))
                        {

                            objAppLevelDefinition.objIAppLevelDefinitionDetList_VW = new List<AppLevelDefinitionDet>();

                            objAppLevelDefinitionDet.ObjAppInfoMapLogicCode_FK = objIAppObjInfoMap_Logic.ObjAppInfoMapLogicCode_PK;
                            objAppLevelDefinition.ModuleObjMapCode_FK = objIAppObjInfoMap_Logic.ModuleObjMapCode_FK;
                            objAppLevelDefinition.AppLevelType = objIAppObjInfoMap_Logic.IsFieldPolicy;
                            objAppLevelDefinition.objIAppLevelDefinitionDetList_VW.Add(objAppLevelDefinitionDet);
                        }

                        if (objAppLevelDefinition.objIAppLevelDefinitionDetList_VW.Count< 1)
                        {
                            vMSG = "Approval Level Definition Data Is incorrect";
                            vResult = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    vMSG = ex.Message;
                    vResult = false;
                }

                return Ok(new
                {
                    vResult,
                    List = objAppLevelDefinition,
                    LogicList = objIAppObjInfoMap_Logic,
                    vMSG
                });
            }

        private void GetLogicvalues(AppObjInfoMap_Logic objAppObjInfoMap_Logic)
        {

            StaticItem si1 = new StaticItem();
            si1.DataValue = "-1";
            si1.TextValue = "Select";

            if (objAppObjInfoMap_Logic.AppObjInfoMap_LogicValues_List_VW != null)
            {
                foreach (AppObjInfoMap_LogicValues obj in objAppObjInfoMap_Logic.AppObjInfoMap_LogicValues_List_VW)
                {
                    StaticItem si = new StaticItem();
                    si.DataValue = obj.OptionValue;
                    si.TextValue = obj.OptionText;

                    if (obj.OptionFor == 1)
                    {
                        Value1List_VW.Add(si);
                    }

                    if (obj.OptionFor == 2)
                    {
                        Value2List_VW.Add(si);
                    }

                    if (obj.OptionFor == 3)
                    {
                        Value3List_VW.Add(si);
                    }
                }
            }
        }

        [HttpGet]
        [Route("GetApproverSelectionInformations")]
        public ActionResult GetApproverSelectionInformations(string pAppLvDefDetAppTypeCode_PK)
        {
            List<AppLevelDefDetAppType> appLevelDefDetAppTypes = new List<AppLevelDefDetAppType>();
            List<AppLevelDefDetAppType> vlist = new List<AppLevelDefDetAppType>();
            if (!String.IsNullOrEmpty(pAppLvDefDetAppTypeCode_PK))
            {
                appLevelDefDetAppTypes = _appLevelDefDetAppType.GetApproverSelection(pAppLvDefDetAppTypeCode_PK);
            }

            string check = "Successful";
            return Ok(new
            {
                check,
                vlist=appLevelDefDetAppTypes,
                appLevelDefDetAppTypes
            });
        }


    }
}

