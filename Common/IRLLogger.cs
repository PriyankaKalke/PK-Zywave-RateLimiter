namespace RateLimiterWeb.Common
{
    public interface  IRLLogger
    {
        public void DEBUG(string msg);
        public void ERROR(string msg);
        public void INFO(string msg);
    }
}