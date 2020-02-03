using System.Linq;
using Core.Domain.Model;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Interfaces.Factories;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class MemberService : UnitOfWork<SiteMonitorDbDataContext>, IMemberService
    {
        private readonly MemberProfileRepository _memberRepository;

        public MemberService(SiteMonitorDbDataContext context, IRepositoryFactory<SiteMonitorDbDataContext> repositoryFactory) : base(context, repositoryFactory)
        {
            _memberRepository = base.GetRepository<MemberProfileRepository>();
        }


        public MemberProfile Get(int id)
        {
            var profile = _memberRepository.Get(p => p.Id == id).FirstOrDefault();
            return profile;
        }
    }
}