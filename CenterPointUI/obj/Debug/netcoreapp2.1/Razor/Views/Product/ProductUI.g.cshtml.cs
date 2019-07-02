#pragma checksum "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0221845d3fee65eddd2e7e9a9f564ade8f8d5fc4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Product_ProductUI), @"mvc.1.0.view", @"/Views/Product/ProductUI.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Product/ProductUI.cshtml", typeof(AspNetCore.Views_Product_ProductUI))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\_ViewImports.cshtml"
using CenterPointUI;

#line default
#line hidden
#line 2 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\_ViewImports.cshtml"
using CenterPointUI.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0221845d3fee65eddd2e7e9a9f564ade8f8d5fc4", @"/Views/Product/ProductUI.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2edc7cc0f3e77f68ee09f57f8aa10aed8980b104", @"/Views/_ViewImports.cshtml")]
    public class Views_Product_ProductUI : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "0", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "1", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
  
    ViewData["Title"] = "Product Information";

#line default
#line hidden
            BeginContext(57, 12615, true);
            WriteLiteral(@"
<script>

    var server = ""http://192.168.20.29/"";
    var requestUrl;

    var pUserCode = """";
    var pCompanyCode = 1;

    window.onload = function () {
        CheckUserIsLogedIn();
        
    }

    function CheckUserIsLogedIn() {
        debugger
        GetCookie();

        if (pUserCode != ""NotLogedIn"") {
            ProductCategory();
            Brand();
            Country();
            Mou();
        }
        else {
            requestUrl = server + ""/centerpoint/employee/createemployee"";
            Cookies.set('RequestURL', requestUrl, { expires: 1 });
            window.location.href = ""../Login/LoginPageUI"";
        }
    }

    function GetCookie() {
        debugger
        pUserCode = Cookies.get('UserID');

        if (pUserCode == null || pUserCode == """") {
            // do cookie doesn't exist stuff;
            pUserCode = ""NotLogedIn"";
        }
        else {
            // do cookie exists stuff
            pUserCode = Cookies.get('Us");
            WriteLiteral(@"erID');
        }
    }

    function ClickCheckBox() {

        if ($(""#chkSampleProduct"").prop('checked') == true) {
            alert();
        }
        else
            alert(false);

    }
    function ProductCategory() {

        $.ajax({
            url: server + ""CoreCommon/api/Common/GetCodeFile?FileTypeCode=33&LevelCode=143&COMPANY_CODE="" + pCompanyCode,
            type: ""Get"",
            datatype: ""JSON"",
            contentType: ""application/json"",
            crossDomain: ""true"",

            error: function () {
                alert("" An error occurred."");
            },
            success: function (data) {

                $(""#FileCodeForProductCategory"").get(0).options.length = 0;
                $(""#FileCodeForProductCategory"").get(0).options[0] = new Option(""Select Type"", ""-1"");
                for (var i = 1; i < data.listGeneralCodeFile.length + 1; i++) {

                    $(""#FileCodeForProductCategory"").get(0).options[i] = new Option(data.listGene");
            WriteLiteral(@"ralCodeFile[i - 1].fileName, data.listGeneralCodeFile[i - 1].fileCode_PK, i - 1);

                }
           

            },
            error: function () {
                alert(""Failed to load Product Category"");


            }
        });

    }
    function Brand() {

        $.ajax({
            url: server + ""CoreCommon/api/Common/GetCodeFile?FileTypeCode=36&LevelCode=63&COMPANY_CODE="" + pCompanyCode,
            type: ""Get"",
            datatype: ""JSON"",
            contentType: ""application/json"",
            crossDomain: ""true"",

            error: function () {
                alert("" An error occurred."");
            },
            success: function (data) {

                $(""#FileCodeForBrand"").get(0).options.length = 0;
                $(""#FileCodeForBrand"").get(0).options[0] = new Option(""Select Type"", ""-1"");
                for (var i = 1; i < data.listGeneralCodeFile.length + 1; i++) {

                    $(""#FileCodeForBrand"").get(0).options[i] = new O");
            WriteLiteral(@"ption(data.listGeneralCodeFile[i - 1].fileName, data.listGeneralCodeFile[i - 1].fileCode_PK, i - 1);

                }


            },
            error: function () {
                alert(""Failed to load Brand"");


            }
        });

    }
    function Country() {

        $.ajax({
            url: server + ""CoreCommon/api/Common/GetCodeFile?FileTypeCode=18&LevelCode=17&COMPANY_CODE="" + pCompanyCode,
            type: ""Get"",
            datatype: ""JSON"",
            contentType: ""application/json"",
            crossDomain: ""true"",

            error: function () {
                alert("" An error occurred."");
            },
            success: function (data) {
                debugger
                $(""#CountryofManufactur"").get(0).options.length = 0;
                $(""#CountryofManufactur"").get(0).options[0] = new Option(""Select Type"", ""-1"");
                $(""#CountryofOrigin"").get(0).options.length = 0;
                $(""#CountryofOrigin"").get(0).options[0] ");
            WriteLiteral(@"= new Option(""Select Type"", ""-1"");
                for (var i = 1; i < data.listGeneralCodeFile.length + 1; i++) {

                    $(""#CountryofOrigin"").get(0).options[i] = new Option(data.listGeneralCodeFile[i - 1].fileName, data.listGeneralCodeFile[i - 1].fileCode_PK, i - 1);
                    $(""#CountryofManufactur"").get(0).options[i] = new Option(data.listGeneralCodeFile[i - 1].fileName, data.listGeneralCodeFile[i - 1].fileCode_PK, i - 1);

                }


            },
            error: function () {
                alert(""Failed to load Country"");


            }
        });

    }
    function Mou() {

        $.ajax({
            url: server + ""CoreCommon/api/Common/GetCodeFile?FileTypeCode=20&LevelCode=19&COMPANY_CODE="" + pCompanyCode,
            type: ""Get"",
            datatype: ""JSON"",
            contentType: ""application/json"",
            crossDomain: ""true"",

            error: function () {
                alert("" An error occurred."");
            }");
            WriteLiteral(@",
            success: function (data) {
                debugger
  
                $(""#FileCodeForMou"").get(0).options.length = 0;
                $(""#FileCodeForMou"").get(0).options[0] = new Option(""Select Type"", ""-1"");
                for (var i = 1; i < data.listGeneralCodeFile.length + 1; i++) {

                    $(""#FileCodeForMou"").get(0).options[i] = new Option(data.listGeneralCodeFile[i - 1].fileName, data.listGeneralCodeFile[i - 1].fileCode_PK, i - 1);

                }


            },
            error: function () {
                alert(""Failed to load MOU"");


            }
        });

    }
    function SaveProduct() {
       

        var jObjMain = {
            ""ProductID"": """",
            ""RepairingStatus"": 0,
            ""chkSampleProduct"": """",
            ""chkServiceProduct"": 0,
            ""chkMaintenanceServiceProduct"": 0,
            ""Status"": """",
            ""PackCCSize"": """",
            ""ProductName"": """",
            ""FileCodeForProductCategor");
            WriteLiteral(@"y"": 0,
            ""chkSerialNoManage"": 0,
            ""VAT"": """",
            ""Description"": """",
            ""FileCodeForMou"": 0,
            ""CountryofManufactur"": 0,
            ""CountryofOrigin"": """",
            ""PartNumber"": """",
            ""ProductSeries"": """",
            ""FileCodeForBrand"": """",
            ""WorkingUnitCode"": 0,
            ""ProductCode_PK"": 0,
            ""TPRate"": """",
            ""CompanyCode_FK"": """",
            ""ActionType"": """",
            ""UserCode"": """",
            ""ActionDate"": """",
        };
     
        jObjMain.ProductID = $(""#ProductID"").val();
        jObjMain.RepairingStatus = $(""#RepairingStatus"").val();
        jObjMain.chkSampleProduct = Number(document.getElementById(""chkSampleProduct"").checked);
        jObjMain.chkServiceProduct = Number(document.getElementById(""chkServiceProduct"").checked);
        jObjMain.chkMaintenanceServiceProduct = Number(document.getElementById(""chkMaintenanceServiceProduct"").checked);
        jObjMain.Status = $(""#Sta");
            WriteLiteral(@"tus"").val();
        jObjMain.PackCCSize = $(""#PackCCSize"").val();
        jObjMain.ProductName = $(""#ProductName"").val();
        jObjMain.FileCodeForProductCategory = $(""#FileCodeForProductCategory"").val();
        jObjMain.chkSerialNoManage = Number(document.getElementById(""chkSerialNoManage"").checked);
        jObjMain.VAT = $(""#VAT"").val();
        jObjMain.Description = $(""#Description"").val();
        jObjMain.FileCodeForMou = $(""#FileCodeForMou"").val();
        jObjMain.CountryofManufactur = $(""#CountryofManufactur"").val();
        jObjMain.CountryofOrigin = $(""#CountryofOrigin"").val();
        jObjMain.PartNumber = $(""#PartNumber"").val();
        jObjMain.ProductSeries = $(""#ProductSeries"").val();
        jObjMain.FileCodeForBrand = $(""#FileCodeForBrand"").val();
        jObjMain.WorkingUnitCode = $(""#WorkingUnitCode"").val();
        jObjMain.ProductCode_PK = $(""#ProductCode_PK"").val();
        jObjMain.TPRate = $(""#TPRate"").val();
        jObjMain.CompanyCode_FK = pCompanyCode;
     ");
            WriteLiteral(@"   jObjMain.ActionType = """";
        jObjMain.UserCode = pUserCode;
        jObjMain.ActionDate = """";
        var datas = JSON.stringify(jObjMain);

        $.ajax(
            {
                url: server + ""/Procurement/api/Product/SaveProduct"",
                dataType: ""json"",
                processData: false,
                contentType: 'application/json',
                type: ""POST"",
                data: datas,
                success: function (data) {
                   
                    alert(""Save Succesfull!"");
                },
                error: function () {
                    alert(""There was error saving data!"");
                }
            }
        );


    }
    function PopUpforProduct() {
          
        var vURL = encodeURIComponent(server +""/procurement/api/Product/GetProductsList"");

        var vColHeaderName = 'Product Id, Product Name , TP Rate';
            var vTitle = 'Product PopUp';
        var vColumn = 'ProductID,ProductName,");
            WriteLiteral(@"TPRate';
        var vPKValue = 'ProductCode_PK';

        callModal(""../Product/GetData?pDynColumns="" + vColHeaderName + ""&pTitle="" + vTitle + ""&pURL="" + vURL + ""&pColumns="" + vColumn + ""&pPkCode="" + vPKValue);
       
    }
    function AssignValueCommonPopUPNewListUI() {
        if (vCalleeVar1 != null && vCalleeVar1 != undefined) {
            $(""#ProductCode"").val(vCalleeVar1);
            GetProduct(vCalleeVar1);

        }
        var vSpan = document.getElementsByClassName(""close"")[0];
        vSpan.onclick();
    }
    function GetProduct(vCalleeVar1) {
       
        $.ajax({
            url: server + ""procurement/api/Product/GetProductByCode?productcode="" + vCalleeVar1,
            type: ""Get"",
            datatype: ""JSON"",
            contentType: ""application/json"",
            crossDomain: ""true"",

            error: function () {
                alert("" An error occurred."");
            },
            success: function (data) {
                debugger
            ");
            WriteLiteral(@"    $(""#ProductId"").val(data.products.ProductID);
                $(""#ProductName"").val(data.products.ProductName);
                $(""#Description"").val(data.products.Description);
                $(""#PackCCSize"").val(data.products.PackCCSize);
                $(""#PartNumber"").val(data.products.PartNumber);
                $(""#ProductSeries"").val(data.products.ProductSeries);
                $(""#TPRate"").val(data.products.TPRate);
                $(""#VAT"").val(data.products.VAT);
                $(""#FileCodeForProductCategory"").val(data.products.FileCodeForProductCategory).change();
                $(""#FileCodeForBrand"").val(data.products.FileCodeForBrand).change();
                $(""#FileCodeForMou"").val(data.products.FileCodeForMou).change();
                $(""#CountryofOrigin"").val(data.products.CountryofOrigin).change();
                $(""#CountryofManufactur"").val(data.products.CountryofManufactur).change();
                $(""#Status"").get(0).selectedIndex = data.products.Status;
     ");
            WriteLiteral(@"           $(""#RepairingStatus"").get(0).selectedIndex = data.products.RepairingStatus;
                if (data.products.chkSampleProduct == 1) {
                    $(""#chkSampleProduct"").prop('checked', true);
                }
                if (data.products.chkServiceProduct == 1) {
                    $(""#chkServiceProduct"").prop('checked', true);
                }
                if (data.products.chkMaintenanceServiceProduct == 1) {
                    $(""#chkMaintenanceServiceProduct"").prop('checked', true);
                }
                if (data.products.chkSerialNoManage == 1) {
                    $(""#chkSerialNoManage"").prop('checked', true);
                }
            },
            error: function () {
                alert(""Failed to load Product Info"");


            }
        });
    }
    function NewPage() {
        window.location.href = ""../Product/ProductUI"";
    }
</script>

<div class=""container"">
    <fieldset>
        <legend>Product</legend>
   ");
            WriteLiteral(@"     <div class=""row"">
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-5"">
                        <label class=""col-md-15 control-label"">Product Id</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(12673, 126, false);
#line 355 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.TextBox("ProductId", null, new { @class = "form-control", ondblclick = "PopUpforProduct()", PlaceHolder="Double click" }));

#line default
#line hidden
            EndContext();
            BeginContext(12799, 26, true);
            WriteLiteral("\r\n                        ");
            EndContext();
            BeginContext(12826, 32, false);
#line 356 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.Hidden("ProductCode", null));

#line default
#line hidden
            EndContext();
            BeginContext(12858, 381, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-5"">
                        <label class=""col-md-15 control-label"">Product Name</label>

                    </div>
                    <div class=""col-md-5"">

                        ");
            EndContext();
            BeginContext(13240, 66, false);
#line 368 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.TextBox("ProductName", null, new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(13306, 2047, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
        </div>

    </fieldset>
    <fieldset>
        <legend>Product Information</legend>
        <div class=""row"" style=""margin-left:10px"">
            <div class=""col-md-2"">
                <div class=""form-group"">
                    <div class=""col-md-13"">
                        <input type=""checkbox"" id=""chkSampleProduct"" name=""chkSampleProduct"" onclick=""ClickCheckBox();"" value=""""> Sample<br>
                    </div>
                </div>
            </div>
            <div class=""col-md-2"">
                <div class=""form-group"">
                    <div class=""col-md-13"">
                        <input type=""checkbox"" id=""chkSerialNoManage"" name=""chkSerialNoManage"" onclick=""ClickCheckBox();"" value=""""> BarCode Manageable<br>
                    </div>
                </div>
            </div>
        </div>
        <div class=""row"" style=""margin-left:10px"">
            <div class=""col-md-2"">
         ");
            WriteLiteral(@"       <div class=""form-group"">
                    <div class=""col-md-13"">
                        <input type=""checkbox"" id=""chkServiceProduct"" name=""chkServiceProduct"" onclick=""ClickCheckBox();"" value=""""> Service<br>
                    </div>
                </div>
            </div>
            <div class=""col-md-2"">
                <div class=""form-group"">
                    <div class=""col-md-13"">
                        <input type=""checkbox"" id=""chkMaintenanceServiceProduct"" name=""chkMaintenanceServiceProduct"" onclick=""ClickCheckBox();"" value=""""> Maintenance Service<br>
                    </div>
                </div>
            </div>

        </div>
        <div class=""row"">
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-6"">
                        <label class=""col-md-15 control-label"">Product Category</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(15354, 117, false);
#line 418 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.DropDownList("FileCodeForProductCategory", new SelectList("", "Value", "Text"), new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(15471, 584, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-7"">
                        <label class=""col-md-15 control-label"">Repairing Status</label>

                    </div>
                    <div class=""col-md-5"">
                        
                        <select id=""RepairingStatus"" style=""width: 160px;height: 24px;padding-left: 4px;border: 1px solid #ccc !important;border-radius: 4px !important"">
                            ");
            EndContext();
            BeginContext(16055, 30, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "844a32222bff452cb656dc98f7e33d05", async() => {
                BeginContext(16073, 3, true);
                WriteLiteral("RSP");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(16085, 30, true);
            WriteLiteral("\r\n                            ");
            EndContext();
            BeginContext(16115, 31, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b8b0dce0ecce4840a9ce93fcde618e29", async() => {
                BeginContext(16133, 4, true);
                WriteLiteral("NRSP");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(16146, 467, true);
            WriteLiteral(@"

                        </select>
                    </div>
                </div>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-6"">
                        <label class=""col-md-15 control-label"">Product Series/Model</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(16614, 68, false);
#line 447 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.TextBox("ProductSeries", null, new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(16682, 377, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-7"">
                        <label class=""col-md-15 control-label"">Brand Name</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(17060, 107, false);
#line 458 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.DropDownList("FileCodeForBrand", new SelectList("", "Value", "Text"), new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(17167, 427, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-6"">
                        <label class=""col-md-15 control-label"">Country of Origin</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(17595, 106, false);
#line 471 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.DropDownList("CountryofOrigin", new SelectList("", "Value", "Text"), new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(17701, 378, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-7"">
                        <label class=""col-md-15 control-label"">Part Number</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(18080, 65, false);
#line 482 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.TextBox("PartNumber", null, new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(18145, 413, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-6"">
                        <label class=""col-md-15 control-label"">MOU</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(18559, 105, false);
#line 495 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.DropDownList("FileCodeForMou", new SelectList("", "Value", "Text"), new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(18664, 391, true);
            WriteLiteral(@"

                    </div>
                </div>
            </div>
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-7"">
                        <label class=""col-md-15 control-label"">Country of Manufacture</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(19056, 110, false);
#line 507 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.DropDownList("CountryofManufactur", new SelectList("", "Value", "Text"), new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(19166, 421, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-6"">
                        <label class=""col-md-15 control-label"">Description</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(19588, 67, false);
#line 520 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.TextArea("Description", null, new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(19655, 539, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-7"">
                        <label class=""col-md-15 control-label"">Status</label>

                    </div>
                    <div class=""col-md-5"">
                        <select id=""Status"" style=""width: 160px;height: 24px;padding-left: 4px;border: 1px solid #ccc !important;border-radius: 4px !important"">
                            ");
            EndContext();
            BeginContext(20194, 33, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "86085a34e70f4a1baa5b588dd5eb512a", async() => {
                BeginContext(20212, 6, true);
                WriteLiteral("Active");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(20227, 30, true);
            WriteLiteral("\r\n                            ");
            EndContext();
            BeginContext(20257, 35, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f0781a1e3eb845ecb737e5eaf6b83175", async() => {
                BeginContext(20275, 8, true);
                WriteLiteral("InActive");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(20292, 506, true);
            WriteLiteral(@"
                            


                        </select>                    
                    </div>
                </div>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-6"">
                        <label class=""col-md-15 control-label"">TP Rate</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(20799, 125, false);
#line 550 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.TextBox("TPRate", null, new { @class = "form-control" , onkeypress = "InputOnlyDecimalNumber(event);", maxlength = 18 }));

#line default
#line hidden
            EndContext();
            BeginContext(20924, 372, true);
            WriteLiteral(@"

                    </div>
                </div>
            </div>
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-7"">
                        <label class=""col-md-15 control-label"">VAT</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(21297, 121, false);
#line 562 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.TextBox("VAT", null, new { @class = "form-control", onkeypress = "InputOnlyDecimalNumber(event);", maxlength = 18 }));

#line default
#line hidden
            EndContext();
            BeginContext(21418, 511, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
        </div>

    </fieldset>
    <fieldset>
        <legend>Product Packing Information</legend>
        <div class=""row"">
            <div class=""col-md-4"">
                <div class=""form-group"">
                    <div class=""col-md-6"">
                        <label class=""col-md-15 control-label"">Pack/CC Size</label>

                    </div>
                    <div class=""col-md-5"">
                        ");
            EndContext();
            BeginContext(21930, 65, false);
#line 579 "E:\CenterPoint\CenterPointPortal\CenterPointUI\Views\Product\ProductUI.cshtml"
                   Write(Html.TextBox("PackCCSize", null, new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(21995, 816, true);
            WriteLiteral(@"

                    </div>
                </div>
            </div>


        </div>

    </fieldset>
    
        <div class=""box"">
            <button type=""button"" id=""addBtn"" class=""btn-primary"" onclick=""SaveProduct()"" style=""width:100px"">Save</button>
            <button type=""button"" id=""addBtn"" class=""btn-info"" onclick=""NewPage()"" style=""width:100px"">New</button>

        </div>
    
</div>
<div id=""ModalView"" class=""modal"">

    <!-- Modal content -->
    <div class=""modal-content"">
        <div class=""modal-header"">

            <span class=""close"">×</span>
        </div>
        <div class=""modal-body"">
            <div class=""modal-body"">
                <div id=""DivNew"">



                </div>

            </div>
        </div>

    </div>

</div>
");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591