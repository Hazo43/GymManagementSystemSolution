using GymManagementBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        // ViewModel اللي موجود جوا ال  MemberViewModel دا
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMember(CreateMemberViewModel CreateMember);

        MemberViewModel? GetMemberDetails (int MemberId);

        HealthRecordViewModel? GetMemberHealthRecordDetails (int MemberId);

    }
}
