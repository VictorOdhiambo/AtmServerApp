using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.Extensions.Hosting;
using RepositoryLayer;

namespace ServiceLayer
{
    public class UserService
    {
        // instance variables
        private UpesiAtmDBContext dbContext;
        private readonly IHostEnvironment env;
        private AuditTrailService auditTrail;

        public UserService(UpesiAtmDBContext dbContext, IHostEnvironment env, AuditTrailService auditTrail)
        {
            this.dbContext = dbContext;
            this.env = env;
            this.auditTrail = auditTrail;
        }

        /// <summary>
        /// Validates username and password
        /// Verify user password
        /// Sign in user (if successful) - retrieve user's account information
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>UserDetails object</returns>
        public UserDetail SignIn(string username, string password)
        {
            try
            {
                var user = dbContext.USERs.Where(u => u.USERNAME.ToLower() == username.ToLower()).FirstOrDefault();

                if (user != null)
                {
                    var correctLogins = BCrypt.Net.BCrypt.EnhancedVerify(password, user.PASSWORD);

                    // simulate ATM selection
                    var atms = dbContext.ATMs.Select(p => p.ATMID).ToList();
                    var rand = new Random();
                    var atmId = atms.ElementAt(rand.Next(atms.Count));

                    if (correctLogins && user.STATUSID == (int)Status.Active)
                    {
                        var userAccount = dbContext.ACCOUNTs.Where(a => a.USERID == user.USERID).FirstOrDefault();

                        return new UserDetail
                        {
                            UserId = user.USERID,
                            UserName = username,
                            IsActive = user.STATUSID == (int)Status.Active,
                            AccountBal = $"{userAccount.BALANCE}",
                            AccountNo = userAccount.ACCOUNTNO,
                            Token = "",
                            Message = "Login was successful",
                            AtmId = atmId,
                        };
                    }
                    else if (user != null && user.STATUSID != (int)Status.Active)
                    {
                        return new UserDetail
                        {
                            UserId = user.USERID,
                            UserName = username,
                            IsActive = false,
                            AccountBal = "",
                            AccountNo = "",
                            Token = "",
                            Message = "Login failed. Account is not active."
                        };
                    }


                    // log audit
                    auditTrail.AddAudit(new AUDITTRAIL
                    {
                        USERID = user.USERID,
                        AUDITACTION = $"Login attempt at ATM: {atmId}"
                    });

                }
                return new UserDetail
                {
                    UserId = 0,
                    UserName = username,
                    IsActive = false,
                    AccountBal = "",
                    AccountNo = "",
                    Token = "",
                    Message = "Login failed. Incorrect username or password"
                };


            }
            catch (Exception ex)
            {
                // log error to file
                LoggerService.LogException(Path.Combine(env.ContentRootPath, "/logs"), ex.ToString());
                return new UserDetail
                {
                    UserId = 0,
                    UserName = username,
                    IsActive = false,
                    AccountBal = "",
                    AccountNo = "",
                    Token = "",
                    Message = "An error occurred while attempting to login. Try again later."
                };
            }
        }
    }
}