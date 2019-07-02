using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LSCrud;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;

namespace CommonDataContext
{
    public class EmailHelper: IEmailHelper
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        
        public EmailHelper(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
            
        }
        static readonly string General_EmailFormat_TBL = "General_EmailFormat";
        public EmailFormat GetEmailFormat(int emailID)
        {
            EmailFormat objEmailFormat = new EmailFormat();
            string vComTxt = @"SELECT  [EmailCode]
                                      ,[EmailID]
                                      ,[EmailSubject]
                                      ,[EmailBody]
                                  FROM  [General_EmailFormat]
                                  WHERE [EmailID]= '" + emailID + "'";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);

            dr = objDbCommand.ExecuteReader();
            if (dr.Read())
            {
                objEmailFormat = new EmailFormat();
                objEmailFormat.EmailCode_PK = dr["EmailCode"].ToString();
                objEmailFormat.EmailID = Convert.ToInt16(dr["EmailID"].ToString());
                objEmailFormat.EmailSubject = dr["EmailSubject"].ToString();
                objEmailFormat.EmailBody = dr["EmailBody"].ToString();
            }
            dr.Close();
            connection.Close();
            return objEmailFormat;
        }
        public  string GetUserId(string userid)
        {
            string username = string.Empty;
            string sSql = @"SELECT LS_User.[UserName]                            
                            FROM [Dev_LS_SecurityDB_HNM].[dbo].[LS_User]                             
                            WHERE LS_User.[UserId] = '" + @userid+"'";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand sqluserid = new SqlCommand(sSql, connection);
            try
            {
                dr = sqluserid.ExecuteReader();
                while (dr.Read())
                {
                    username = dr["UserName"].ToString();
                }
            }
            catch (DbException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return username;
        }
        public void SendMail(string obj_Pk, int moduleID,string userid)
        {
            EmailFormat objEmailFormat = new EmailFormat();
            objEmailFormat = GetEmailFormat(moduleID);
            string username = GetUserId(userid);
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com");
            mail.From = new MailAddress("parama.biswas@leads-bd.com", "Center Point");
            mail.To.Add(username);
            mail.Subject = objEmailFormat.EmailSubject;
            var replacement = objEmailFormat.EmailBody.Replace("@PARAMETER@", obj_Pk);
            mail.Body = replacement;
            mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("parama.biswas@leads-bd.com", "Dhaka@2013");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
     }
}
