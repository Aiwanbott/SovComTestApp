using SovComTestApp.Models;
using System.Threading.Tasks;

namespace SovComTestApp.Interfaces
{
    public interface IInvitationService
    {
        ResponseObject SendInvite(Invites invite);
        int CountRequests(int apiId);
        bool IsSentToday(string phoneNumber, int apiId);
    }
}
