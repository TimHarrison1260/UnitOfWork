using Core.Domain.Model;

namespace Core.Interfaces.Services
{
    public interface IMemberService
    {
        MemberProfile Get(int id);
    }
}