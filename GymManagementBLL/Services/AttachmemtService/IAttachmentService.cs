using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmemtService
{
    public interface IAttachmentService
    {
        string? Upload(string FolderName, IFormFile file);

        bool Delete(string FileName, string FolderName);
    }
}
