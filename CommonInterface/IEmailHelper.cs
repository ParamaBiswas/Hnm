using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonInterface
{
    public interface IEmailHelper
    {
        EmailFormat GetEmailFormat(int emailID);
        void SendMail(string obj_Pk, int moduleID, string userid);
    }
}
