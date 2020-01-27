namespace Core.Domain.Model
{
    public enum TestTypeEnum
    {
        SiteAvailable,
        RobotsTxtExists,
        RobotsHasSiteMapReference,
        SiteMapExists,
        RemoteSiteMapUsed,
        PageTest,
        SlowPage,
        UnresponsivePage
    }
}