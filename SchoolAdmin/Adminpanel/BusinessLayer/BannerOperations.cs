using Adminpanel.DataAccess;
using Adminpanel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Adminpanel.BusinessLayer
{
    public class BannerOperations
    {
        DB_SchoolMgmtContext _context = new DB_SchoolMgmtContext();
        public string UploadBannerImage(HttpPostedFileBase file, string folderName)
        {
            var allowedextension = new[] { ".jpg", ".jpeg", ".png" };
            return UploadBannerFile(file, folderName, allowedextension);
        }
        public string UploadBannerFile(HttpPostedFileBase file, string folderName, string[] allowedextension)
        {
            DateTime dt = DateTime.Now;
            string savedFileName = $"{Guid.NewGuid()}{dt:yyyyMMddHHmmssfff}";
            string extension = Path.GetExtension(file.FileName);
            if (!allowedextension.Contains(extension))
            {
                return "not allowed";
            }
            string directorypath = ConfigurationManager.AppSettings["FileUploadBaseDirectory"];
            string folderPath = Path.Combine(directorypath, "Uploads", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, $"{savedFileName}{extension}");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.InputStream.CopyTo(fileStream);
                //file.CopyTo(fileStream); 20240130244508786.png
            }

            return savedFileName + extension;

        }

        public async Task<IEnumerable<BannerDetail>> GetLocationMaster()
        {
            try
            {
                List<BannerDetail> GOpening = await _context.Get_BannerDetails.FromSqlRaw("EXEC USP_GetAllBanners").ToListAsync();
                return GOpening;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                throw;
            }
        }

      
    }
}