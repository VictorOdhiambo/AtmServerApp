using DataAccessLayer.Models;
using Microsoft.Extensions.Hosting;
using RepositoryLayer;

namespace ServiceLayer
{
    public class AuditTrailService
    {
        private readonly IHostEnvironment env;
        private UpesiAtmDBContext dbContext;

        public AuditTrailService(UpesiAtmDBContext dbContext, IHostEnvironment env)
        {
            this.dbContext = dbContext;
            this.env = env;
        }

        public void AddAudit(AUDITTRAIL audit)
        {
            try
            {
                audit.AUDITDATE = DateTime.Now;
                dbContext.Add(audit);
                dbContext.SaveChanges();
                
            }catch (Exception ex)
            {
                LoggerService.LogException(Path.Combine(env.ContentRootPath, "/logs"), ex.ToString());
            }
        }
    }
}
