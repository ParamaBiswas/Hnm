using CommonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SupplierModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using SupplierInterface;
using ConnectionGateway;
using Microsoft.AspNetCore.Authorization;

namespace Supplier.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {

        IUploadedFileRepository _uploadedFile;
        ISupplierDbContext _supplierDbContext;

        public FileUploadController(ISupplierDbContext supplierDbContext, IUploadedFileRepository uploadedFileRepository)
        {
            _supplierDbContext = supplierDbContext;
            _uploadedFile = uploadedFileRepository;
        }
        [HttpPost]
        [Route("Upload")]
        public async  Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), @"E:\Books", file.FileName);
                var stream = new FileStream(path, FileMode.Create);
                 await file.CopyToAsync(stream);
                return Ok(new
                {
                    length = file.Length,
                    name = file.FileName,

                });
            }
            catch
            {
                return BadRequest();

            }
        }
        [HttpPost]
        [Route("UploadMultipleFile")]
        public async Task<IActionResult> Uploads(List<IFormFile> files)
        {
            try
            {
                var result = new List<FileUploadResult>();
                foreach (var file in files)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"E:\Books", file.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);
                    result.Add(new FileUploadResult() { Name = file.FileName, Length = file.Length });
                }
                return Ok(result);

            }
            catch
            {
                return BadRequest();

            }
        }

        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            try
            {
                string file = Path.Combine(
                  Path.Combine(Directory.GetCurrentDirectory(), @"E:\Supplier Documents"), filename);

                var memory = new MemoryStream();
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;
                return File(memory, GetMimeType(file), filename);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        private string GetMimeType(string file)
        {
            string extension = Path.GetExtension(file).ToLowerInvariant();
            switch (extension)
            {
                case ".txt": return "text/plain";
                case ".pdf": return "application/pdf";
                case ".doc": return "application/vnd.ms-word";
                case ".docx": return "application/vnd.ms-word";
                case ".xls": return "application/vnd.ms-excel";
                case ".png": return "image/png";
                case ".jpg": return "image/jpeg";
                case ".jpeg": return "image/jpeg";
                case ".gif": return "image/gif";
                case ".csv": return "text/csv";
                default: return "";
            }
        }
        [HttpDelete]
        [Route("DeleteFile")]
        public void DeleteFile(string filename)
        {
            string file = Path.Combine(Directory.GetCurrentDirectory(), @"E:\Books");


            IFileProvider physicalFileProvider = new PhysicalFileProvider(file);

            DeleteFiles(physicalFileProvider, filename);
        }

        private void DeleteFiles(IFileProvider physicalFileProvider, string filename)
        {

                var files = Path.Combine(
                      Path.Combine(Directory.GetCurrentDirectory(), @"E:\Books"), filename);
                if (physicalFileProvider is PhysicalFileProvider)
                {
                    var directory = physicalFileProvider.GetDirectoryContents(string.Empty);
                    foreach (var file in directory)
                    {
                        if (!file.IsDirectory)
                        {
                            var fileInfo = new System.IO.FileInfo(files);
                            fileInfo.Delete();

                        }
                    }
                }
                
            
        }

        [HttpPost]
        [Route("SaveFiles")]
        public IActionResult SaveUploadedFiles(ListModel objList)
        {
            foreach (UploadedFile objUploadedFile in objList.objUploadedFiles)
            {
               
                objUploadedFile.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
           }
           

            string Vmsg = _uploadedFile.SaveUploadedFiles(objList.objUploadedFiles);

            return Ok(new
            {
                message = Vmsg
            });

        }
        [HttpGet]
        [Route("GetUploadedDocuments")]
        public IActionResult GetUploadedDocumentsBySupplierCode(string suppliercode)
        {
            List<UploadedFile> objUploadedFileslist = new List<UploadedFile>();
            objUploadedFileslist = _uploadedFile.GetUploadedFileBySupplierCode(suppliercode);
            return Ok(new
            {
                objUploadedFileslist
            });
        }

    }
}