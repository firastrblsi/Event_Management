using Event_Management.CrossCutting.Entities;

namespace Event_Management.BLL.Interfaces.Repositories
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);

    }
}
