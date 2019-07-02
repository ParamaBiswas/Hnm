/// *************************************************************************************************
///	|| Creation History ||
///	-------------------------------------------------------------------------------------------------
///	Copyright		:	Copyright© LEADSOFT 2009. All rights reserved.
/// File Type       :   JavaScript file
///	Author			:	A. K. M. Aminul Bari
///	Purpose			:	This js file holds the common js works
///	Creation Date	:	06/07/2009
/// ==================================================================================================
///  || Modification History ||
///  -------------------------------------------------------------------------------------------------
///  Sl No.	Date:		    Author:			    Ver:	Area of Change:
///	**************************************************************************************************
///     01  04-Aug-09       Aminul Bari         1.1     Add function ShowAlertScript
///     02  17-Aug-09       Aminul Bari         1.2     Add variable vRewriteURL for rewriting url.
///     03  08-Oct-09       Azharul Sharif      1.3     Add function RightButton & KeyCtrlAlt
///     04  27-Jan-10       Md. Mahabubul Alam  1.4     Add variable vCalleeVar6 and vCalleeVar7
///     05  22-Mar-10       Aminul Bari         1.5     Update ShowConfirmation(..)
///     06  21-Jun-10       Md. Abu Sayem       1.6     Add variable vCalleeVar8, vCalleeVar9 and vCalleeVar10
///     07  06-October      Md. Jakaria Hossain 1.7     Add function GetAjaxErrorAlert for showing ajax error alert
///     08  04-Nov-10       Shikha Anirban      1.8     Add functions FormatDate and CompareDate.
///     09  04-Jul-12       Shahrima Sultana    1.3.1.11 Add function IsNumeric.
///     10  23-Apr-2013     Md. Masudur Rahman          Add new function to get offset position of an element
///	**************************************************************************************************

///-----------------Variables----------------
//For each control at runtime may be masterpage content place holder id is tagged. So it
//globally declared here
var vCtrlPrx = 'ctl00_contPlcHdrMasterHolder_';
//When calling a popup window then use this variable in the parameter so that all over
//the application a fixed size popup window will appear.
//For example: window.showModalDialog('XYZ',window, vPopupShape);
var vPopupShape = 'status:no;help:no;dialogWidth:470px;dialogHeight:490px;scrolling=no';
var vPopupShapeReportInfo = 'status:no;help:no;dialogWidth:720px;dialogHeight:490px;scrolling=no';
//When calling a popup window then use this variable to hold the caller window object
var vCallerWindowObject;
//When using urls in code it should add the following variable after controller name. It is necessary for URL routing
//For example: window.showModalDialog("../Common"+vRewriteURL+"/GetItemHierarchy, window, vPopupShape);
var vRewriteURL = "";
//When calling a popup window then use the following variables to callee window values
var vCalleeVarPK; //added by Shahrima on 22/09/2014
var vCalleeVar1;
var vCalleeVar2;
var vCalleeVar3;
var vCalleeVar4;
var vCalleeVar5;
var vCalleeVar6;
var vCalleeVar7;
var vCalleeVar8;
var vCalleeVar9;
var vCalleeVar10;
var vCalleeVar11;
var vCalleeVar12;
var vCalleeVar13;
var vCalleeVar14;
var vCalleeVar15;//Add For Challan Integration with Fixed Asset
var vCalleeVar16;//Add For Challan Integration with Fixed Asset
var vCalleeVar17;//Add For Challan Integration with Fixed Asset

var vConfirm;
var vCalleeVarArray = new Array();
var CommasFormat = "2;3";
var gDigitDecimals = 3;
var vServer = "http://192.168.20.29/";

//Datepicker
//$(document).ready(function(){
//    $(".selector").datepicker("option", "dateFormat", 'yy-mm-dd');
//});
///-----------------Functions----------------
//pMessage=For any customize message from caller
//pLevel=Folder level from the caller. It will need for custom popup
//pCustom=true for customization
function ShowConfirmation(pMessage, pLevel, pCustom) {
    if (pCustom == true) {
        //Code for customize popup ui and call according to pLevel and show message
        //according to pMessge
    }
    else {
        if (pMessage == null || pMessage == "")
            vConfirm = window.confirm("Are you sure you want to Remove/Delete?");
        else
            vConfirm = window.confirm(pMessage);
        return vConfirm;
    }
} //End of function ShowConfirmation
//------------------------------------------------------------------------------


////added by saima hossain bcz InputOnlyDecimalNumberWithPoint() is not working properly
function InputOnlyDecimalNumberWithTwoPlaces(event, obj) {
   
    if (!event) var event = event || window.event;
    if (event) {
        var keyCode = event.charCode || event.keyCode;

        if (keyCode < 48 || keyCode > 57) {
            
            if (event.keyCode != 46) {/////46 means "."
                event.preventDefault();
                return false;
            }
        }
    }
    var vValue = obj.value;
    if (vValue == '')
        vValue = 0;

    var decimalnum = /^[0-9]+(\.[0-9]{0,2})?$/;
    if (decimalnum.test(vValue))
        return true
    else {
        event.preventDefault();
        return false;
    }
}







//------------------------------------------------------------------------------
//Only takes number as input. call this function on the event 'onkeypress'
function InputOnlyNumber(event) {
    //debugger
    if (!event) var event = event || window.event;
    if (event) {
        var keyCode = event.charCode || event.keyCode;
        if (keyCode == 46) {
            event.preventDefault();

            return true;

        }
        else if ((keyCode < 48 || keyCode > 57) && keyCode != 8 && keyCode != 119 && keyCode != 9) {
            event.preventDefault();
          
            keyCode = 000;
            //alert("Input Decimal Number");
            return false;
        }
        else if((keyCode == 17 && keyCode == 99) || (keyCode == 17 && keyCode == 119))
        {
          
            return true;
        }
    }
    else
        return true;
}
//------------------------------------------------------------------------------
//Only takes decimal number as input. call this function on the event 'onkeypress'
function InputOnlyDecimalNumber(event) {
 
    var event = event || window.event;
    if (event) {
        var keyCode = event.charCode || event.keyCode;
        if (keyCode < 48 || keyCode > 57) {

            if (event.keyCode != 46) {/////46 means "."
                event.preventDefault();
                return false;
            }
        }
    }
    
}


/*
* @Params
* pValue       : which value you want to format
* pControlId   : It expects a html controlID in which you want to show
* pDecimalPoint: number of digits you want to show after the decimal point(Ex .02)
* pIsCeling    : true/false
* @example     : FormatNumber('2346.768', 'txtAmount', 2, true);  Output: 2346.77
*
* @return value: If there is any Control ID, it sets the resultant value in the 
*                respected Control. Otherwise, it returns the Formatted Value
*/
function FormatNumber(pValue, pControlId, pDecimalPoint, pIsCeling) {
    var vResult;
    var vNumber = (isNaN(pValue)) ? 0 : pValue;
    var vPow = Math.pow(10, pDecimalPoint);
    vResult = (pIsCeling == true)
			  ? Math.ceil((parseInt((Math.abs(vNumber) * vPow) * 10) / 10.0)) / vPow
			  : Math.floor(Math.abs(vNumber) * vPow) / vPow;
    vResult = (vNumber < 0) ? (-1 * vResult) : vResult;

    vResult = new String(vResult);
    var sec = vResult.split('.');
    var dot;
    var zero = '';
    if (sec.length > 1) {
        dot = (sec[1].length < pDecimalPoint) ? (pDecimalPoint - sec[1].length) : 0;
        while (dot) { zero += '0'; dot--; }
        vResult = (zero != '') ? (sec[0] + '.' + (sec[1] + zero)) : vResult;
    }
    else {
        dot = pDecimalPoint;
        while (dot) { zero += '0'; dot--; }
        vResult = (zero != '') ? (vResult + '.' + zero) : vResult;
    }
    if (pControlId != '' && pControlId != null)
        document.getElementById(pControlId).value = vResult;
    else
        return vResult;
}
function callModal(vURL, vDATA) {
    var vModal = document.getElementById('ModalView');
    var vSpan = document.getElementsByClassName("close")[0];

    $.ajax({
        type: "GET",
        url: vURL,
        data: vDATA,

        dataType: "html",
        success: function (data) {
            $('#DivNew').html(data);

            vModal.style.display = "block";

        }

    });
    vSpan.onclick = function () {
        vModal.style.display = "none";
    }
}
//----------------------------------------------------------------------------------------------

/*
if (document.layers) window.captureEvents(Event.KEYPRESS);
if (document.layers) window.captureEvents(Event.MOUSEDOWN);
if (document.layers) window.captureEvents(Event.MOUSEUP);
document.onkeydown = KeyCtrlAlt;
document.onmousedown = RightButton;
document.onmouseup = RightButton;
window.document.layers = RightButton;
*/

function setObjectStatus(obj) {
    obj.text = "val";
}

/*
* @Params
* pXHRResponseText: Containing the ajax responseText
* @example        : GetAjaxErrorAlert(pXHRResponseText);  Output: "Access denied."
* @return value   : If pXHRResponseText is empty or undefined then it return false
*                   else show alert message.
*/
function GetAjaxErrorAlert(pXHRResponseText) {
    if (pXHRResponseText == '' || pXHRResponseText == undefined)
        return false;
    else
        ShowAlert(pXHRResponseText.substring(pXHRResponseText.indexOf('<title>') + 7, pXHRResponseText.indexOf('</title>')) + '.', null, false);
}

//  Formate the date in a specific format
//  pInputDate = The Date to be formated
//  pCurrent Format of the date Specified such as "dd-MM-yyyy"
//  pFormatedDateFormat=The output date format.
function FormatDate(pInputDate, pCurrentDateFormat, pFormatedDateFormat) {
    //debugger
    var vSeparator;
    var vOutputDate;
    if (pInputDate.indexOf('-') != -1)
        vSeparator = '-';
    else if (pInputDate.indexOf('/') != -1)
        vSeparator = '/';
    else
        return null;
    var vDateParts = pInputDate.split(vSeparator);
    if (pFormatedDateFormat == "MM-dd-yyyy") {
        if (pCurrentDateFormat == "dd-MM-yyyy") {
            vOutputDate = vDateParts[1] + vSeparator + vDateParts[0] + vSeparator + vDateParts[2];
        }
        else if (pCurrentDateFormat == "MM-dd-yyyy") {
            vOutputDate = InputDateTime;
        }
        else if (pCurrentDateFormat == "yyyy-MM-dd") {
            vOutputDate = vDateParts[1] + vSeparator + vDateParts[2] + vSeparator + vDateParts[0];
        };
    };
    return vOutputDate;
};

// This function compare two dates Which returns -1 if first date is smaller than second date,
//  returns 0 if two dates are equal and returns 1 if first date is larger than the second date.

function CompareDate(FirstDate, SecondDate, CurrentDateFormat) {
    //debugger
    FirsDate = FormatDate(FirstDate, CurrentDateFormat, "MM-dd-yyyy");
    SecondDate = FormatDate(SecondDate, CurrentDateFormat, "MM-dd-yyyy");
    if (Date.parse(FirsDate) < Date.parse(SecondDate)) {
        return -1;
    }
    else if (Date.parse(FirsDate.replace(/-/g, '/')) == Date.parse(SecondDate.replace(/-/g, '/'))) {
        return 0;
    }
    else if (Date.parse(FirsDate) > Date.parse(SecondDate)) {
        return 1;
    };
};

//This function stop entering single quotation
function SkipSingleQuotation(event) {

    if (event.keyCode == 39) {
        window.event.keyCode = 000;
        return false;
    }
    else {
        return true;
    }
};
//Add by Abdul Mannan, Date: 31-May-2012.
//Only takes AlphaNumeric(A-Z a-z  0-1 _ - . Speace Key ).
function InputOnlyAlphaNumeric(pEvent) {
    var vKeycode = pEvent.keyCode;
    if (vKeycode > 64 && vKeycode < 91) {//'A Z'
        return true;
    }
    if (vKeycode > 96 && vKeycode < 123) {//'a z'
        return true;
    }
    if (vKeycode > 47 && vKeycode < 58) { //'0 9'
        return true;
    }
    if (vKeycode == 45) {//'-'
        return true;
    }
    if (vKeycode == 46) {//'.'
        return true;
    }
    if (vKeycode == 95) {//'_'
        return true;
    }
    if (vKeycode == 32) {//Speace Key
        return true;
    }
    window.event.keyCode = 000;
    return false;
}
//Add by Abdullah, Date: 29-October-2014.
//Only takes AlphaNumeric(A-Z a-z  0-1 _ - . Speace Key ).
function InputOnlyAlphaNumericWithBracket(pEvent) {
    var vKeycode = pEvent.keyCode;
    if (vKeycode > 64 && vKeycode < 91) {//'A Z'
        return true;
    }
    if (vKeycode > 96 && vKeycode < 123) {//'a z'
        return true;
    }
    if (vKeycode > 47 && vKeycode < 58) { //'0 9'
        return true;
    }
    if (vKeycode == 45) {//'-'
        return true;
    }
    if (vKeycode == 40) {//'('
        return true;
    }
    if (vKeycode == 41) {//')'
        return true;
    }
    if (vKeycode == 44) {//','
        return true;
    }
    if (vKeycode == 47) {//'/'
        return true;
    }
    if (vKeycode == 46) {//'.'
        return true;
    }
    if (vKeycode == 95) {//'_'
        return true;
    }
    if (vKeycode == 32) {//Speace Key
        return true;
    }
    window.event.keyCode = 000;
    return false;
}
//Add by Md.Rafiqul Islam, Date: 22-Oct-2013.
//Only takes AlphaNumeric(A-Z a-z  _ - . Speace Key ).
function InputOnlyAlphabetic(pEvent) {
    var vKeycode = pEvent.keyCode;
    if (vKeycode > 64 && vKeycode < 91) {//'A Z'
        return true;
    }
    if (vKeycode > 96 && vKeycode < 123) {//'a z'
        return true;
    }
    if (vKeycode == 45) {//'-'
        return true;
    }
    if (vKeycode == 46) {//'.'
        return true;
    }
    if (vKeycode == 95) {//'_'
        return true;
    }
    if (vKeycode == 32) {//Speace Key
        return true;
    }
    window.event.keyCode = 000;
    return false;
}
// Added by Shahrima , Date : 4-Jul-2012.
// Takes object. If value of object is only numeric type returns true. If not refreshes object value.
function IsNumeric(object) {
    var vInput = object.value;
    var vText = object.value - 0;
    var vName = object.name;
    if (vInput == vText && vText != NaN) {
        return true;
    }
    else {
        alert("Please enter numeric value. ");
        $('#' + vName + '').val('');
    }

}
//Add by Abdul Mannan,Date:04-july-2012
// return true if format of pString varify the Regular Expression. Else false
function CompareString(pString) {
    var check = /^[A-Za-z0-9]\w*/;
    if (!check.test(pString)) {
        return false;
    }
    else
        return true;
}

//Added by Md. Tajul Islam. Date: 12 November,2012
// All popup will close by ESC key. 
//Prerequisite: Common.js.
//document.onkeydown = function(evt) {
//    if (window.dialogArguments != null) {//checking is there any open modaldialogue exist.
//        evt = evt || window.event;
//        if (evt.keyCode == 27) {
//            window.close();
//        }
//    }
//};

//Added By Anamul Hoque
// TO disable function Key (F1) Keypress 

if ("onhelp" in window) {
    window.onhelp = function () {
        return false;
    }
}
// TO disable function Key (F3,F4,F6,F7,F12) Keypress
document.onkeydown = function (evt) {
    var vKeyCode = window.event.keyCode;
    if (window.event && (vKeyCode == 114 || vKeyCode == 115 || vKeyCode == 117 || vKeyCode == 118 || vKeyCode == 123)) { // Capture and remap F5
        window.event.keyCode = 505;
    }

    if (window.dialogArguments != null) {//checking is there any open modaldialogue exist.
        evt = evt || window.event;
        if (evt.keyCode == 27) {
            window.close();
        }
    }

    if (window.event && window.event.keyCode == 505) { // New action for F5
        return false;
        // Must return false or the browser will refresh anyway
    }
}

// Added by Shahrima , Date : 17-Apr-2013.
// Takes object, Length of object value, No of digit after decimal point.
// If total no of digit before decimal point is greater than difference between  ValueLength and NoOfDecimalDigit it refreshes object value else returns true. 

function ValidateDecimalValue(obj, ValueLength, NoOfDecimalDigit) {
    var vId = obj.id;
    var vInt = "";
    var vDec = "";
    var vDevidedBy = obj.value.split('.');
    var vMSG = "Before decimal point more than " + (ValueLength - NoOfDecimalDigit) + " digit is not allowed.";
    if (vDevidedBy.length === 2) {
        vInt = vDevidedBy[0];
        vDec = vDevidedBy[1];
        if (vInt.length > (ValueLength - NoOfDecimalDigit)) {
            alert(vMSG);
            $('#' + vId + '').val("");
        }
    }
    if (vDevidedBy.length === 1) {
        if (obj.value.length > (ValueLength - NoOfDecimalDigit)) {
            alert(vMSG);
            $('#' + vId + '').val("");
        }
    }
}
////////////////////////////////////////////////////////////////////////////////////
// Get offset position of an element relative to parent element
///////////////////////////////////////////////////////////////////////////////////
function getOffset(el) {
    var _x = 0;
    var _y = 0;
    while (el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop)) {
        _x += el.offsetLeft - el.scrollLeft;
        _y += el.offsetTop - el.scrollTop;
        el = el.offsetParent;
    }
    return { top: _y, left: _x };
}


//Add By Abdul Mannan at 04-July-2013
//obj=Object, pDecimalPoint=Total Digit after ".";
function InputOnlyDecimalNumberWithPoint(event,obj, pDecimalPoint) {

    var vValue = obj.value;
    var vIndex = vValue.indexOf(".");
    var vLength = vValue.length;
    if (event.keyCode == 46) {
        if (vIndex != -1) {
            window.event.keyCode = 000;
            return false;
        }
        else {
            return true;
        }
    }
    if (pDecimalPoint == '' || pDecimalPoint == undefined) {
        pDecimalPoint = 2;
    }
    if (vIndex >= 0 && vIndex < vValue.length - pDecimalPoint) {//if ((vLength - vIndex > pDecimalPoint) && vIndex != -1) {
        window.event.keyCode = 000;
        return false;
    }
    if (event.keyCode < 48 || event.keyCode > 57) {
        window.event.keyCode = 000;
        return false;
    }
    else
        return true;
}
//Add by Abdul Mannan
//To Sub String up to max value.
///ControlName=Control Name, MaxLength=Max Length
function CustomSubStr(ControlName, MaxLength) {
    var ControlValue = $('#' + ControlName).val();
    if (ControlValue.length > MaxLength) {
        alert("System can not allow to input more than " + MaxLength + " Character.\nSystem remove over Character.");
        ControlValue = ControlValue.substr(0, MaxLength);
        $('#' + ControlName).val(ControlValue);
    }
}
//Add by Abdul Mannan
// convert '',null, undefined to Zero(0) of working Control.
//Event onblur
//"For example: ChangeNullToZero(this);"
function ChangeNullToZero(pControl) {
    var pControlID = pControl.id;
    var vValue = $('#' + pControlID + '').val();
    if (vValue == '' || vValue == undefined || vValue == null) {
        $('#' + pControlID + '').val('0');
    }
}
//Add by Abdul Mannan
// convert  Zero(0) to '' of working Control.
//Event: onfocus
//For example: "ChangeZeroToNull(this);"
function ChangeZeroToNull(pControl) {
    var pControlID = pControl.id;
    var vValue = $('#' + pControlID + '').val();
    if (Number(vValue) == 0) {
        $('#' + pControlID + '').val('');
    }
}

String.prototype.trim = function () {
    try {
        return this.replace(/^\s+|\s+$/g, "");
    } catch (e) {
        return this;
    }
}
function AjaxErrorHandle(jqXHR, exception) {
    var msg = '';
    if (jqXHR.status === 0) {
        msg = 'Not connect.\n Verify Network.';
    } else if (jqXHR.status == 404) {
        msg = 'Requested page not found.';
    } else if (jqXHR.status == 500) {
        msg = 'Internal Server Error.';
    } else if (exception === 'parsererror') {
        msg = 'Requested JSON parse failed.';
    } else if (exception === 'timeout') {
        msg = 'Time out error.';
    } else if (exception === 'abort') {
        msg = 'Ajax request aborted.';
    } else {
        msg = 'Uncaught Error.\n' + jqXHR.responseText;
    }
    alert(msg + '\nPlease try again.');
}

function NumberWithCommas(pNumber) {
    if (CommasFormat == '') return pNumber;
    if (pNumber == undefined || pNumber == null || pNumber == '' || Number(pNumber) == Number('0.00') || Number(pNumber) == Number('0.0') || Number(pNumber) == Number('0')) return pNumber;

    var CommasFormatList = CommasFormat.split(";");
    var NumberCharForFirstComma;
    var NumberCharForOtherComma;


    if (CommasFormatList.length == 1) {
        NumberCharForOtherComma = CommasFormatList[0];
        NumberCharForFirstComma = CommasFormatList[1];
    }
    else {
        NumberCharForOtherComma = CommasFormatList[0];
        NumberCharForFirstComma = CommasFormatList[1];
    }

    pNumber = pNumber.replace(/,/g, "");
    pNumber = pNumber.replace(/\s/g, "");
    var res = pNumber.split(".");
    var DecemalPart = '';
    if (res.length == 2) {
        DecemalPart = '.' + res[1];
    }

    var IntegerPart = res[0];
    if (IntegerPart.length <= NumberCharForFirstComma) {
        return pNumber;
    }
    else {
        var FirstThreePart = IntegerPart.substr(IntegerPart.length - NumberCharForFirstComma);
        var OtherPart = IntegerPart.substring(0, IntegerPart.length - NumberCharForFirstComma);
        if (NumberCharForOtherComma == 1)
            OtherPart = OtherPart.toString().replace(/\B(?=(?:\d{1})+(?!\d))/g, ",");
        if (NumberCharForOtherComma == 2)
            OtherPart = OtherPart.toString().replace(/\B(?=(?:\d{2})+(?!\d))/g, ",");
        if (NumberCharForOtherComma == 3)
            OtherPart = OtherPart.toString().replace(/\B(?=(?:\d{3})+(?!\d))/g, ",");
        if (NumberCharForOtherComma == 4)
            OtherPart = OtherPart.toString().replace(/\B(?=(?:\d{4})+(?!\d))/g, ",");
        pNumber = OtherPart + ',' + FirstThreePart + DecemalPart;
    }
    return pNumber;
}
