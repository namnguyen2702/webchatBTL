namespace webchatBTL.Services
{
    public interface ISubscriptionService
    {
        bool IsCompanySubscribedToService(int companyId, string serviceName);
    }

}
