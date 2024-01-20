using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.Extensions.Hosting;
using RepositoryLayer;

namespace ServiceLayer
{
    public class AtmService
    {
        private readonly IHostEnvironment env;
        private UpesiAtmDBContext dbContext;
        private AuditTrailService auditTrail;

        public AtmService(UpesiAtmDBContext dbContext, IHostEnvironment env, AuditTrailService auditTrail)
        {
            this.dbContext = dbContext;
            this.env = env;
            this.auditTrail = auditTrail;
        }

        /// <summary>
        /// Transfer funds to a given account number
        /// Validates the account no if it is valid
        /// Validates the amount against user's account balance
        /// </summary>
        /// <param name="accountTo"></param>
        /// <param name="amount"></param>
        /// <param name="userId"></param>
        /// <returns>Success | Error messages</returns>
        public ApiResponse TransferFunds(string accountTo, decimal amount, int userId)
        {
            try
            {
                // log audit
                auditTrail.AddAudit(new AUDITTRAIL
                {
                    USERID = userId,
                    AUDITACTION = $"Funds Transfer attempt of Amount: {amount} to Account no: {accountTo}"
                });

                // get current user account
                var userAccount = dbContext.ACCOUNTs.Where(a => a.USERID == userId).FirstOrDefault();

                // get the recipient account
                var recipientAcc = dbContext.ACCOUNTs.Where(a => a.ACCOUNTNO.ToUpper() == accountTo.ToUpper()).FirstOrDefault();

                // check that recipient account exists
                if (recipientAcc == null)
                {
                    return new ApiResponse
                    {
                        Message = "Funds transfer failed: Invalid recipient account."
                    };
                }

                // check that account is active
                if (userAccount?.STATUSID == (int)Status.Active)
                {
                    // check that account has sufficient balance
                    if (userAccount.BALANCE >= amount)
                    {
                        // transfer amount
                        recipientAcc.BALANCE += amount;
                        dbContext.ACCOUNTs.Update(recipientAcc);

                        // update user account
                        userAccount.BALANCE -= amount;
                        dbContext.ACCOUNTs.Update(userAccount);

                        dbContext.SaveChanges();

                        // log audit
                        auditTrail.AddAudit(new AUDITTRAIL
                        {
                            USERID = userId,
                            AUDITACTION = $"Successful Funds Transfer of Amount: {amount} to Account no: {accountTo}"
                        });

                        return new ApiResponse
                        {
                            Message = "Funds transfer was successful"
                        };
                    }
                    else
                    {
                        return new ApiResponse
                        {
                            Message = "Funds transfer failed: Insufficient balance."
                        };
                    }
                }
                else
                {
                    return new ApiResponse
                    {
                        Message = "Funds transfer failed: Account inactive"
                    };
                }


            }catch (Exception ex)
            {
                LoggerService.LogException(Path.Combine(env.ContentRootPath, "/logs"), ex.ToString());
                return new ApiResponse
                {
                    Message = "An error occurred while processing request. Try again later."
                };
            }
        }

        /// <summary>
        /// Withdraw funds from ATM
        /// Validates the amount against user's account balance
        /// Validate the amount against ATM balance
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="atmId"></param>
        /// <param name="amount"></param>
        /// <returns>Success | Failed messages</returns>
        public ApiResponse WithdrawFunds(int userId, int atmId, decimal amount)
        {
            try
            {

                // log audit
                auditTrail.AddAudit(new AUDITTRAIL
                {
                    USERID = userId,
                    AUDITACTION = $"Funds Withdrawal attempt of Amount: {amount} at ATM {atmId}"
                });

                // get current user account
                var userAccount = dbContext.ACCOUNTs.Where(a => a.USERID == userId).FirstOrDefault();
                
                // get current logged in atm
                var atm = dbContext.ATMs.Where(t => t.ATMID == atmId).FirstOrDefault();

                // check that user account is active
                if (userAccount?.STATUSID == (int)Status.Active)
                {
                    // validate user balance against amount for withdrawal
                    if (userAccount.BALANCE >= amount)
                    {
                        // check that the ATM has enough funds to dispense
                        if (atm?.BALANCE >= amount)
                        {
                            // update ATM amount
                            atm.BALANCE -= amount;
                            dbContext.Update(atm);

                            // update user account
                            userAccount.BALANCE -= amount;
                            dbContext.Update(userAccount);

                            dbContext.SaveChanges();

                            // log audit
                            auditTrail.AddAudit(new AUDITTRAIL
                            {
                                USERID = userId,
                                AUDITACTION = $"Successful Funds Withdrawal of Amount: {amount} at ATM {atmId}"
                            });

                            return new ApiResponse
                            {
                                Message = "Funds withdrawal was successful."
                            };

                        }
                        else
                        {
                            return new ApiResponse
                            {
                                Message = "Funds withdrawal failed: ATM can only dispense Ksh. " + atm.BALANCE.ToString("##.##") + " at the moment."
                            };
                        }
                    }
                    else
                    {
                        return new ApiResponse
                        {
                            Message = "Funds withdrawal failed: Insufficient balance"
                        };
                    }
                }
                else
                {
                    return new ApiResponse
                    {
                        Message = "Funds withdrawal failed: Account inactive"
                    };
                }

                

            }
            catch (Exception ex)
            {
                LoggerService.LogException(Path.Combine(env.ContentRootPath, "/logs"), ex.ToString());
                return new ApiResponse
                {
                    Message = "An error occurred while processing request. Try again later."
                };
            }
        }
    }
}
