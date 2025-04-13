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
            var planId = _context.SubscriptionPlans
                .Where(p => p.PlanName == serviceName)
                .Select(p => p.PlanId)
                .FirstOrDefault();

            if (planId == 0)
                return false;

            return _context.CompanySubscriptions
                .Any(cs => cs.CompanyId == companyId && cs.PlanId == planId && cs.EndDate >= DateTime.Now);
        }
    }

}
