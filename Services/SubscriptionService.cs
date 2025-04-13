using webchatBTL.Models;

namespace webchatBTL.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly WebchatBTLDbContext _context;

        public SubscriptionService(WebchatBTLDbContext context)
        {
            _context = context;
        }

        public bool IsCompanySubscribedToService(int companyId, string serviceName)
        {
            int planId = _context.SubscriptionPlans
                .Where(sp => sp.PlanName == serviceName)
                .Select(sp => sp.PlanId)
                .FirstOrDefault();

            if (planId == 0) return false;

            return _context.CompanySubscriptions
                .Any(cs => cs.CompanyId == companyId && cs.PlanId == planId && cs.EndDate >= DateTime.Now);
        }
    }
}
