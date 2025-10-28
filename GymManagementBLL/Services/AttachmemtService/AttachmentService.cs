using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmemtService
{
    public class AttachmentService : IAttachmentService
    {


        private readonly string[] allowedExtentions = { ".jpg", ".jpeg", ".png" };
        private readonly long MaxSizeFile = 5 * 1024 * 1024; // 5 MB
        private readonly IWebHostEnvironment webHost;
        public AttachmentService(IWebHostEnvironment _webHost)
        {
            webHost = _webHost;
        }

        public string? Upload(string FolderName, IFormFile file)
        {
            try
            {         
                if(FolderName is null || file is  null || file.Length == 0) return null;
                
                //Check Size
                if(file.Length > MaxSizeFile) return null;

                // Check Extention  
                var extention = Path.GetExtension(file.FileName).ToLower();
                if(!allowedExtentions.Contains(extention)) return null;
               
                //  Folderpath  =>                 wwwroot        /   images   / Member مثلا
                var FolderPath = Path.Combine(webHost.WebRootPath, "images", FolderName); 
                if(!Directory.Exists(FolderPath)) 
                {
                    // روح اعمل واحد FolderPath دي معناها لو مفيش 
                    Directory.CreateDirectory(FolderPath);
                }

                // ghjhs.jpg   <= عشان الاسم يبقي فريد من نوعو 
                var fileName = Guid.NewGuid().ToString() + extention;
                
                // FolderPath=>     wwwroot/images/member      fileName => hsjkh.jpg
                var filePath = Path.Combine(FolderPath, fileName);

                using var fileStream = new FileStream( filePath , FileMode.Create);
                {
                    file.CopyTo(fileStream);
                }

                return fileName;

            }
            catch( Exception ex) 
            {
                Console.WriteLine($" Failed To Upload File To Folder = {FolderName} : {ex}");
                return null;
            }
        }


        public bool Delete(string FileName, string FolderName)
        {
            try
            {
                if(string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FolderName)) return false;

                var fullPath = Path.Combine(webHost.WebRootPath, "images"  , FolderName , FileName);

                if(File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                
                    return false;
                

            }
            catch( Exception ex )
            {
                Console.WriteLine($"Failed To Delete File With Name {FileName} : {ex}");
                return false;
            }
        }

      
    }
}
