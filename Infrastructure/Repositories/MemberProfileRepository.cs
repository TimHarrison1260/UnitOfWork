using Core.Domain.Model;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class MemberProfileRepository: Repository<MemberProfile, SiteMonitorDbDataContext>
    {
        public MemberProfileRepository()
        {}

        public MemberProfileRepository(SiteMonitorDbDataContext context) : base(context)
        {}
    }

}