using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSession();
        SessionViewModel? GetSessionById(int sessionId);
        bool CreateSession(CreateSessionViewModel createSession);

        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        bool UpdateSession( int sessionId ,UpdateSessionViewModel updateSession);

        bool RemoveSession (int sessionId);

    }
}
