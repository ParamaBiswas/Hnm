using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonModel
{
    public class CommonValidation
    {
        static string DATEFORMAT;
        static string DBSERVER;
        public static bool ValidateDate(string pInputDateTime)
        {
            if (String.IsNullOrEmpty(pInputDateTime))
                return false;
            else if (pInputDateTime.Length != 10)
                return false;

            string[] vDateParts; char vSeparator;

            if (pInputDateTime.IndexOf('-') != -1)
                vSeparator = '-';
            else if (pInputDateTime.IndexOf('/') != -1)
                vSeparator = '/';
            else
                return false;

            vDateParts = pInputDateTime.Split(vSeparator);
            if (vDateParts.Length != 3)//If it would not have 3 date parts
                return false;

            //Sorting Date parts temporarily to dd-MM-YYYY style for simplification
            vDateParts = OrderDateParts(vDateParts);

            int vDay, vMonth, vYear, vMaxDay;
            vDay = Convert.ToInt32(vDateParts[0]);
            vMonth = Convert.ToInt32(vDateParts[1]);
            vYear = Convert.ToInt32(vDateParts[2]);

            //Validating year
            if (vYear < 1900)
                return false;
            //Validating month
            if (vMonth > 0 && vMonth < 13)
            {
                if (vMonth == 2)//For February
                {
                    //Checking Leap year
                    if (DateTime.IsLeapYear(vYear))
                        vMaxDay = 29;
                    else
                        vMaxDay = 28;
                }
                else if (vMonth == 1 || vMonth == 3 || vMonth == 5 || vMonth == 7 || vMonth == 8 || vMonth == 10 || vMonth == 12)
                    vMaxDay = 31;
                else
                    vMaxDay = 30;

                //Validating Day
                if (!ValidateDay(vDay, vMaxDay))
                    return false;
                else
                    return true;
            }
            else
                return false;

        }
        public static string FormatDate(string pInputDateTime, bool pInOut, bool pUIReport)
        {
            //Check date and validate as a date
            if (!ValidateDate(pInputDateTime))
                return null;

            string pOutputDateTime = null;
            string[] vDateParts;
            char vSeparator;

            //Checking is there any separator for input date string. If not then return null.
            if (pInputDateTime.IndexOf('-') != -1)
                vSeparator = '-';
            else if (pInputDateTime.IndexOf('/') != -1)
                vSeparator = '/';
            else
                return null;

            vDateParts = pInputDateTime.Split(vSeparator);
            if (string.Compare(DATEFORMAT, "dd-MM-yyyy", true) == 0)
            {
                if (pUIReport)
                {
                    if (pInOut)//Input to DB
                    {
                        if (string.Compare(DBSERVER, "SQLSERVER", true) == 0)//Checking DB SERVER type
                            pOutputDateTime = vDateParts[2] + "-" + vDateParts[1] + "-" + vDateParts[0]; // YYYY-MM-DD
                    }
                    else//Output from DB
                        pOutputDateTime = vDateParts[0] + "-" + vDateParts[1] + "-" + vDateParts[2]; // DD-MM-YYYY
                }
                else//Input to Report
                    pOutputDateTime = vDateParts[1] + "/" + vDateParts[0] + "/" + vDateParts[2]; // MM/DD/YYYY
            }
            if (string.Compare(DATEFORMAT, "MM-dd-yyyy", true) == 0)
            {
                if (pUIReport)
                {
                    if (pInOut)//Input to DB
                    {
                        if (string.Compare(DBSERVER, "SQLSERVER", true) == 0)//Checking DB SERVER type
                            pOutputDateTime = vDateParts[2] + "-" + vDateParts[0] + "-" + vDateParts[1]; // yyyy-mm-DD
                    }
                    else//Output from DB
                        pOutputDateTime = vDateParts[0] + "-" + vDateParts[1] + "-" + vDateParts[2]; // mm-DD-yyyy
                }
                else//Input to Report
                    pOutputDateTime = vDateParts[1] + "/" + vDateParts[0] + "/" + vDateParts[2]; // MM/DD/YYYY
            }
            return pOutputDateTime;
        }
        public static string FormatDate(string pInputDateTime, string pInFormat, string pOutFormat)
        {
            //if (!ValidateDate(pInputDateTime))
            //    return null;
            if (String.IsNullOrEmpty(pInputDateTime))
                return null;
            string pOutputDateTime = null;
            string[] vDateParts;
            char vSeparator;

            //Checking is there any separator for input date string. If not then return false
            if (pInputDateTime.IndexOf('-') != -1)
                vSeparator = '-';
            else if (pInputDateTime.IndexOf('/') != -1)
                vSeparator = '/';
            else
                return null;

            vDateParts = pInputDateTime.Split(vSeparator);
            //Order the Date Parts to day, month, year
            if (String.IsNullOrEmpty(pInFormat))
                vDateParts = OrderDateParts(vDateParts);
            else
                vDateParts = OrderDateParts(vDateParts, pInFormat);
            if (string.Compare(pOutFormat, "dd-MM-yyyy", true) == 0)
            {
                pOutputDateTime = vDateParts[0] + "-" + vDateParts[1] + "-" + vDateParts[2]; // dd-MM-yyyy
            }
            else if (string.Compare(pOutFormat, "MM-dd-yyyy", true) == 0)
            {
                pOutputDateTime = vDateParts[1] + "/" + vDateParts[0] + "/" + vDateParts[2]; // MM/DD/YYYY
            }
            else if (string.Compare(pOutFormat, "yyyy-MM-dd", true) == 0)
            {
                pOutputDateTime = vDateParts[2] + "/" + vDateParts[1] + "/" + vDateParts[0]; // YYYY/MM/DD
            }
            return pOutputDateTime;
        }
        private static string[] OrderDateParts(string[] vDateParts)
        {
            string vTEMP = null;
            if (string.Compare(DATEFORMAT, "dd-MM-yyyy", true) == 0)
                return vDateParts;
            else if (string.Compare(DATEFORMAT, "MM-dd-yyyy", true) == 0)
            {
                vTEMP = vDateParts[0];
                vDateParts[0] = vDateParts[1];
                vDateParts[1] = vTEMP;
            }
            else if (string.Compare(DATEFORMAT, "yyyy-MM-dd", true) == 0)
            {
                vTEMP = vDateParts[0];
                vDateParts[0] = vDateParts[2];
                vDateParts[2] = vTEMP;
            }
            return vDateParts;
        }
        private static string[] OrderDateParts(string[] vDateParts, string pInFormat)
        {
            string vTEMP = null;
            if (string.Compare(pInFormat, "dd-MM-yyyy", true) == 0)
                return vDateParts;
            if (string.Compare(pInFormat, "MM-dd-yyyy", true) == 0)
            {
                vTEMP = vDateParts[0];
                vDateParts[0] = vDateParts[1];
                vDateParts[1] = vTEMP;
            }
            else if (string.Compare(pInFormat, "yyyy-MM-dd", true) == 0)
            {
                vTEMP = vDateParts[0];
                vDateParts[0] = vDateParts[2];
                vDateParts[2] = vTEMP;
            }
            return vDateParts;
        }
        private static bool ValidateDay(int pDay, int pMaxDay)
        {
            if (pDay <= 0)
                return false;
            else if (pDay > pMaxDay)
                return false;
            else
                return true;
        }
        public static string DateDifference(DateTime pDT1, DateTime pDT2)
        {
            DateTime vDT = new DateTime();
            //Get the difference
            //To get the actual value it is need to subtract 1 year, 1 month, 1 day from the 
            //returned value as the returned value is the addition of (01/01/0001) with the difference
            vDT = GetDateDifferenceTemp(pDT1, pDT2);
            //Add 1 day, because bydafault it is considered that difference includes 
            //the startdate itself
            vDT = vDT.AddDays(1);
            return ReturnDateDifferenceString(vDT);
        }
        public static int[] DateDifferenceValue(DateTime pDT1, DateTime pDT2)
        {
            DateTime vDT = new DateTime();
            //Get the difference
            //To get the actual value it is need to subtract 1 year, 1 month, 1 day from the 
            //returned value as the returned value is the addition of (01/01/0001) with the
            //difference
            vDT = GetDateDifferenceTemp(pDT1, pDT2);
            //Add 1 day, because bydafault it is considered that difference includes 
            //the startdate itself
            vDT = vDT.AddDays(1);
            return ReturnDateDifferenceValue(vDT);
        }
        private static string ReturnDateDifferenceString(DateTime pDT1)
        {
            StringBuilder vSB = new StringBuilder();
            vSB.Append(pDT1.Year - 1);
            vSB.Append(" Years ");
            vSB.Append(pDT1.Month - 1);
            vSB.Append(" Months ");
            vSB.Append(pDT1.Day - 1);
            vSB.Append(" Days.");
            return vSB.ToString();
        }
        private static int[] ReturnDateDifferenceValue(DateTime pDT1)
        {
            int[] vOut = { pDT1.Day - 1, pDT1.Month - 1, pDT1.Year - 1 };
            return vOut;
        }
        private static DateTime GetDateDifferenceTemp(DateTime pDT1, DateTime pDT2)
        {
            TimeSpan vTS = pDT1.Subtract(pDT2);
            //If pDT1 < pDT2 then the subtracted value will be negative. Therefore
            //it is needed to make the negative value Negative to make positive.
            if (DateTime.Compare(pDT1, pDT2) < 0)//pDT1 < pDT2
                vTS = vTS.Negate();
            //Return the Difference as a datetime object with adding DateTime.MinValue(01/01/0001)
            //So to get the actual value it is need to subtract 1 year, 1 month, 1 day from the 
            //returned value.
            return DateTime.MinValue.Add(vTS);
        }
        private static int CountLeapYear(DateTime pDT1, DateTime pDT2)
        {
            int vLYCounter = 0;
            if (DateTime.Compare(pDT1, pDT2) < 0)//pDT1 < pDT2
            {
                for (int i = pDT1.Year; i <= pDT2.Year; i++)
                {
                    if (DateTime.IsLeapYear(i))
                        vLYCounter++;
                }
            }
            else if (DateTime.Compare(pDT1, pDT2) == 0)
            {
                if (DateTime.IsLeapYear(pDT1.Year))
                    vLYCounter = 1;
            }
            else//pDT1 > pDT2
            {
                for (int i = pDT2.Year; i <= pDT1.Year; i++)
                {
                    if (DateTime.IsLeapYear(i))
                        vLYCounter++;
                }
            }
            return vLYCounter;
        }
        public static bool CompareDateIsInSpecificDuration(DateTime dt1, int durationInMonth, bool compareBackDateDuration, bool compareFutureDateDuration)
        {
            DateTime dt2 = DateTime.Now.Date;
            dt1 = dt1.Date;

            bool isValid = false;


            if (DateTime.Compare(dt1, dt2) == 0)
            {
                isValid = true;
            }
            else if (compareBackDateDuration && (DateTime.Compare(dt1, dt2) < 0))
            {
                dt1 = dt1.AddMonths(durationInMonth);

                if (DateTime.Compare(dt1, dt2) >= 0)
                {
                    isValid = true;
                }
            }
            else if (compareFutureDateDuration && (DateTime.Compare(dt1, dt2) > 0))
            {
                dt2 = dt2.AddMonths(durationInMonth);

                if (DateTime.Compare(dt2, dt1) >= 0)
                {
                    isValid = true;
                }
            }

            return isValid;
        }//End of CompareDateIsInSpecificDuration(...)


        public static bool IsValidEmailAddress(string emailAddress)
        {
            emailAddress = emailAddress.Trim();
            Regex re = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);
            return re.IsMatch(emailAddress);
        }
    }
}
