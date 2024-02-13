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
    public class AboutUsOperations
    {
        DB_SchoolMgmtContext _context = new DB_SchoolMgmtContext();
        public string UploadAboutUsImage(HttpPostedFileBase file, string folderName)
        {
            var allowedextension = new[] { ".jpg", ".jpeg", ".png" };
            return UploadAboutUsFile(file, folderName, allowedextension);
        }
        public string UploadAboutUsFile(HttpPostedFileBase file, string folderName, string[] allowedextension)
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

        public async Task<IEnumerable<AboutUsDetails>> GetAboutUsMaster()
        {
            try
            {
                List<AboutUsDetails> GOpening = await _context.Get_AboutUsDetails.FromSqlRaw("EXEC USP_GetAllAboutUsData").ToListAsync();
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