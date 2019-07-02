using CenterPointCoreSecurity.Model;
using Security.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CenterPointCoreSecurity.Data
{
    public class UserInfoDC
    {
        private static string ConnectionString { get; set; }
        public static string GetDefaultConnectionString()
        {
            ConnectionString= Startup.ConnectionString;
            return ConnectionString;
        }
        
        public static string SaveUser(UserInfo objUserInfo)
        {
        
        int vResult = 0;
        int vResult1 = 0;
        int vResult2 = 0;
        int vResult3 = 0;
            int vResult4 = 0;
            string connectionstring = GetDefaultConnectionString();
            //string providerUserKey = Guid.NewGuid().ToString();
         string vOut = EnumMessageId.LS251.ToString() + ": Exception Occured !";
        StringBuilder vComText = new StringBuilder();
            string vComText1 = string.Empty;

        Hashtable htExistingRoles = new Hashtable();
        Hashtable htNewUserRoleList = new Hashtable();
        SqlConnection connection = new SqlConnection(connectionstring);
        connection.Open();
            LsMembershipProvider lsMembershipProvider = new LsMembershipProvider();
            string passwordSaltedText = lsMembershipProvider.GeneratePasswordSaltingText();
            string saltedPassword = lsMembershipProvider.SaltText(objUserInfo.Password, passwordSaltedText);
            string saltedPasswordAnswer = lsMembershipProvider.SaltText(objUserInfo.PasswordAnswer, passwordSaltedText);
            string sSqlUser = @"INSERT INTO LS_User 
                                    (UserId,UserName,
                                    UserCode,ActionDate,ActionType)
                                    VALUES
                                    (@UserId,@UserName,
                                    @UserCode,@ActionDate,@ActionType)";

            Guid userId = Guid.NewGuid();
            SqlCommand sqluser = new SqlCommand(sSqlUser, connection);
            sqluser.Parameters.AddWithValue("UserId", userId);
            sqluser.Parameters.AddWithValue("UserName", objUserInfo.UserName);
            sqluser.Parameters.AddWithValue("UserCode", Guid.NewGuid());
            sqluser.Parameters.AddWithValue("ActionDate", objUserInfo.ActionDate);
            sqluser.Parameters.AddWithValue("ActionType", "Insert");
            


            string sSqlMembership = @"INSERT INTO LS_Membership
                                        (UserId,IsLockedOut,IsFirstLogin,
                                        LastLoginDate,LastPasswordChangeDate,FailedPassAtmptCount,
                                        LastLockoutDate,FailedPassAnsAtmptCount,PasswordSalt,Email,
                                        PasswordQuestion,PasswordAnswer,
                                        UserCode,ActionDate,ActionType)
                                        VALUES
                                        (@UserId,@IsLockedOut,@IsFirstLogin,
                                        @LastLoginDate,@LastPasswordChangeDate,@FailedPassAtmptCount,
                                        @LastLockoutDate,@FailedPassAnsAtmptCount,@PasswordSalt,@Email,
                                        @PasswordQuestion,@PasswordAnswer,
                                        @UserCode,@ActionDate,@ActionType)
                                        ";

            SqlCommand sqlmembership = new SqlCommand(sSqlMembership, connection);
            sqlmembership.Parameters.AddWithValue("UserId", userId.ToString());
            sqlmembership.Parameters.AddWithValue("IsLockedOut", 0);
            sqlmembership.Parameters.AddWithValue("IsFirstLogin", 1);
            sqlmembership.Parameters.AddWithValue("LastLoginDate", new DateTime(1800, 1, 1));
            sqlmembership.Parameters.AddWithValue("LastPasswordChangeDate", new DateTime(1800, 1, 1));
            sqlmembership.Parameters.AddWithValue("FailedPassAtmptCount",0);
            sqlmembership.Parameters.AddWithValue("LastLockoutDate", new DateTime(1800, 1, 1));
            sqlmembership.Parameters.AddWithValue("FailedPassAnsAtmptCount", 0);
            sqlmembership.Parameters.AddWithValue("PasswordSalt", lsMembershipProvider.EncodeToBase64String(passwordSaltedText));
            sqlmembership.Parameters.AddWithValue("Email", objUserInfo.Email);
            sqlmembership.Parameters.AddWithValue("PasswordQuestion", "abc");
            sqlmembership.Parameters.AddWithValue("PasswordAnswer",lsMembershipProvider.EncodeText(saltedPasswordAnswer));
            sqlmembership.Parameters.AddWithValue("UserCode", Guid.NewGuid());
            sqlmembership.Parameters.AddWithValue("ActionDate",DateTime.Now);
            sqlmembership.Parameters.AddWithValue("ActionType", "Insert");

            string sSqlUserPassword = @"INSERT INTO LS_UserPassword
                                            (UserId,Password,
                                            UserCode,ActionDate,ActionType)
                                            VALUES
                                            (@UserId,@Password,
                                            @UserCode,@ActionDate,@ActionType)";
            SqlCommand sqlpassword = new SqlCommand(sSqlUserPassword, connection);
            sqlpassword.Parameters.AddWithValue("UserId", userId.ToString());
            sqlpassword.Parameters.AddWithValue("Password",lsMembershipProvider.EncodeText(saltedPassword));
            sqlpassword.Parameters.AddWithValue( "UserCode", Guid.NewGuid());
            sqlpassword.Parameters.AddWithValue( "ActionDate", DateTime.Now);
            sqlpassword.Parameters.AddWithValue( "ActionType", "Insert");

            vComText.Append("INSERT INTO LS_UserInfo (UserId,UserCode,ActionDate,ActionType,CompanyCode,WorkingUnitCode,UserFullName)");
            vComText.Append(" VALUES");
            vComText.Append("(@UserId,@UserCode,@ActionDate,@ActionType,@CompanyCode,@WorkingUnitCode,@UserFullName)");
            
            
            SqlCommand sqlCommand = new SqlCommand(vComText.ToString(), connection);

            sqlCommand.Parameters.AddWithValue("@UserId", userId);
            sqlCommand.Parameters.AddWithValue("UserCode", userId.ToString());
            sqlCommand.Parameters.AddWithValue("ActionDate", objUserInfo.ActionDate);
            sqlCommand.Parameters.AddWithValue("ActionType", "Insert");
            sqlCommand.Parameters.AddWithValue("CompanyCode", objUserInfo.CompanyCode);
            sqlCommand.Parameters.AddWithValue("WorkingUnitCode", objUserInfo.WorkingUnitCode);
            sqlCommand.Parameters.AddWithValue( "UserFullName", objUserInfo.UserFullName);

            string sSqlUserRole = @"INSERT INTO LS_UserInRole
                                            (UserCode,ActionDate,
                                            ActionType,UserId,RoleId,UserRoleId,IsDeleted)
                                            VALUES
                                            (@UserCode,@ActionDate,@ActionType,@UserId,@RoleID,@UserRoleId,
                                            @IsDeleted)";
            SqlCommand sqlrole = new SqlCommand(sSqlUserRole, connection);
            sqlrole.Parameters.AddWithValue("UserCode", userId.ToString());
            sqlrole.Parameters.AddWithValue("ActionDate", DateTime.Now);
            sqlrole.Parameters.AddWithValue("ActionType", "Insert");
            sqlrole.Parameters.AddWithValue("UserId", userId.ToString());
            sqlrole.Parameters.AddWithValue("RoleID", "f65b77cf-26be-4451-9980-d5c5ca735514");
            sqlrole.Parameters.AddWithValue("UserRoleId", Guid.NewGuid());
            sqlrole.Parameters.AddWithValue("IsDeleted", 0);

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                sqluser.Transaction = transaction;
            sqlCommand.Transaction = transaction;
            sqlmembership.Transaction = transaction;
            sqlpassword.Transaction = transaction;
            sqlrole.Transaction = transaction;
            try
                {

                    vResult1 = sqluser.ExecuteNonQuery();


                    if (vResult1 > 0)
                    {
                        vResult = sqlCommand.ExecuteNonQuery();
                        if (vResult > 0)
                        {
                            vResult2 = sqlmembership.ExecuteNonQuery();
                            if (vResult2 > 0)
                            {
                                vResult3 = sqlpassword.ExecuteNonQuery();
                                if (vResult3 > 0)
                                {
                                    vResult4 = sqlrole.ExecuteNonQuery();
                                    transaction.Commit();
                                    vOut = "User Created Successfully";
                                }
                            }
                        }



                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    vOut = "User not created Successfully";
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }

            return vOut;



    }
        public static string GetPasswordSalt(string username)
        {
            string vOut = EnumMessageId.LS251.ToString() + ": Exception Occured !";
            string saltedText = string.Empty;
            string connectionstring = GetDefaultConnectionString();
            LsMembershipProvider lsMembershipProvider = new LsMembershipProvider();
            string sSqlsalt = @"SELECT LS_Membership.PasswordSalt
                            FROM LS_User
                            INNER Join LS_Membership
                            ON LS_User.UserId = LS_Membership.UserId
                            WHERE UserName = @UserName";
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlDataReader dr;
            SqlCommand command = new SqlCommand(sSqlsalt, connection);
            command.Parameters.AddWithValue("UserName", username);
            try
            {
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    saltedText = Convert.ToString(dr["PasswordSalt"]);
                }
            }
            catch (Exception ex)
            {

                vOut = "Password salt not found";
                throw ex;
            }
            saltedText = lsMembershipProvider.DecodeFromBase64String(saltedText);
            return saltedText;
        }
        private static bool ValidateEmail(string username, int? pEmailStatus)
        {
            bool vOut = false;
            int vEmailStatus = 0;
            int vResult = 0;
            string connectionstring = GetDefaultConnectionString();
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlDataReader dr;
            if (pEmailStatus == 1)
            {
                string sSql = @"Update LS_User set EmailVerified=1 where UserName = @UserName";
                SqlCommand sSqlemail = new SqlCommand(sSql, connection);
                sSqlemail.Parameters.AddWithValue("UserName", username);
                SqlTransaction transaction = connection.BeginTransaction();
                sSqlemail.Transaction = transaction;
                try
                {
                    vResult = sSqlemail.ExecuteNonQuery();
                    if (vResult > 0)
                    {
                        vOut = true;
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    vOut = false;
                    throw ex;
                }
            }
            else
            {
                string sSql = @"SELECT isnull(EmailVerified,0)EmailVerified
                            FROM LS_User
                            WHERE UserName = @UserName";
                SqlCommand command = new SqlCommand(sSql, connection);
                command.Parameters.AddWithValue("UserName", username);
                try
                {
                    dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        vEmailStatus = Convert.ToInt16(dr["EmailVerified"]);
                        if (vEmailStatus == 1)
                        {
                            vOut = true;
                        }
                        else
                        {
                            vOut = false;
                        }
                    }
                }
                catch (Exception ex)
                {

                    vOut = false;
                    throw ex;
                }

            }
            return vOut;
        }
        public static string GetUserId(string username)
        {
            string userId = string.Empty;
            string sSql = @"SELECT LS_User.UserId                            
                            FROM LS_User                             
                            WHERE LS_User.UserName = @UserName";
            string connectionstring = GetDefaultConnectionString();
            SqlConnection connection = new SqlConnection(connectionstring);
            SqlDataReader dr;
            connection.Open();
            SqlCommand sqluserid = new SqlCommand(sSql, connection);
            sqluserid.Parameters.AddWithValue("UserName", username);
            try
            {
                dr = sqluserid.ExecuteReader();
                while (dr.Read())
                {
                    userId = Convert.ToString(dr["UserId"]);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return userId;
        }
        public static List<RoleInfo> GetAllRolls()
        {
            List<RoleInfo> objRoleInfoList = new List<RoleInfo>();
            RoleInfo objRoleInfo;
            string sSql = @"Select RoleId,RoleName from LS_Role";
            string connectionstring = GetDefaultConnectionString();
            SqlConnection connection = new SqlConnection(connectionstring);
            SqlDataReader dr;
            connection.Open();
            SqlCommand sqlRole = new SqlCommand(sSql, connection);
            try
            {
                dr = sqlRole.ExecuteReader();
                while (dr.Read())
                {
                    objRoleInfo = new RoleInfo();
                    objRoleInfo.RoleId_PK = dr["RoleId"].ToString();
                    objRoleInfo.RoleName = dr["RoleName"].ToString();
                    objRoleInfoList.Add(objRoleInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
                return objRoleInfoList;
        }
            public static List<RoleInfo> GetUserRoleList(string username)
        {
            List<RoleInfo> objRoleInfoList = new List<RoleInfo>();
            RoleInfo objRoleInfo;
            string sSql = @"SELECT DISTINCT LS_Role.RoleId, LS_Role.RoleName FROM LS_Role INNER JOIN LS_UserInRole ON LS_Role.RoleId = LS_UserInRole.RoleId INNER JOIN LS_USER ON LS_UserInRole.UserId = @UserId and LS_UserInRole.IsDeleted ='0'  ORDER BY RoleName";

            string connectionstring = GetDefaultConnectionString();
            SqlConnection connection = new SqlConnection(connectionstring);
            SqlDataReader dr;
            connection.Open();
            SqlCommand sqlRole = new SqlCommand(sSql, connection);
            sqlRole.Parameters.AddWithValue("UserId", GetUserId(username));

            dr = sqlRole.ExecuteReader();
            while (dr.Read())
                {
                    objRoleInfo = new RoleInfo();
                    objRoleInfo.RoleId_PK = dr["RoleId"].ToString();
                    objRoleInfo.RoleName = dr["RoleName"].ToString();
                    objRoleInfoList.Add(objRoleInfo);
                }
            
            return objRoleInfoList;
        }
        public static string GetSupplierCode(string usercode)
        {
            string suppliercode = string.Empty;
            string sSql = @"SELECT [SupplierCode]                         
                            FROM [DB_H&M].[dbo].[LSP_PMS_SupplierInfo]                            
                            WHERE [UserCode] = @UserCode";
            string connectionstring = GetDefaultConnectionString();
            SqlConnection connection = new SqlConnection(connectionstring);
            SqlDataReader dr;
            connection.Open();
            SqlCommand sqlsupplierCode = new SqlCommand(sSql, connection);
            sqlsupplierCode.Parameters.AddWithValue("UserCode", usercode);
            try
            {
                dr = sqlsupplierCode.ExecuteReader();
                while (dr.Read())
                {
                    suppliercode = Convert.ToString(dr["SupplierCode"]);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return suppliercode;

        }
        public static string ValidateUser(string username, string password, int? pEmailStatus)
        {
            bool EmailStatus = true;
            string vOut = EnumMessageId.LS251.ToString() + ": Exception Occured !";
            string saltedText = string.Empty;
            string saltpassword = string.Empty;
            string concatpassword = string.Empty;
            string checkpassword = string.Empty;
            string connectionstring = GetDefaultConnectionString();
            LsMembershipProvider lsMembershipProvider = new LsMembershipProvider();
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlDataReader dr;
            saltedText = GetPasswordSalt(username);
            if(pEmailStatus!=null)
            {
                EmailStatus = ValidateEmail(username, pEmailStatus);
            }
            else
            {
                EmailStatus = true;
            }
            
            if (EmailStatus == true)
            {
                string sSql = @"SELECT Password
                            FROM LS_UserPassword
                            INNER JOIN LS_Membership
                            ON LS_UserPassword.UserId = LS_Membership.UserId
                            INNER JOIN LS_User
                            ON LS_UserPassword.UserId = LS_User.UserId
                            WHERE UserName = @UserName";

                SqlCommand sqlPassword = new SqlCommand(sSql, connection);
                sqlPassword.Parameters.AddWithValue("UserName", username);

                try
                {
                    dr = sqlPassword.ExecuteReader();
                    while (dr.Read())
                    {
                        saltpassword = Convert.ToString(dr["Password"]);
                    }
                    checkpassword = lsMembershipProvider.EncodeText(lsMembershipProvider.SaltText(password, saltedText));

                    if (saltpassword == checkpassword)
                    {

                        vOut = "Login Successfully";
                    }
                    else
                    {
                        vOut = "Invalid user";
                    }
                }
                catch (Exception ex)
                {

                    vOut = "Password not found";
                    throw ex;
                }

                 finally
                {
                    connection.Close();
                }

                

            }
            else
            {
                vOut = "Verify Email";
            }

            return vOut;

        }
    }
}
