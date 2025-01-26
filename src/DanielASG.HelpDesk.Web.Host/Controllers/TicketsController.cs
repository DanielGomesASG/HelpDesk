using System;
using System.IO;
using System.Linq;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using DanielASG.HelpDesk.Storage;

namespace DanielASG.HelpDesk.Web.Controllers
{
    [Authorize]
    public class TicketsController : HelpDeskControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const long MaxFilesLength = 5242880; //5MB
        private const string MaxFilesLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] FilesAllowedFileTypes = { "jpeg", "jpg", "png" };

        public TicketsController(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        public FileUploadCacheOutput UploadFilesFile()
        {
            try
            {
                //Check input
                if (Request.Form.Files.Count == 0)
                {
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                var file = Request.Form.Files.First();
                if (file.Length > MaxFilesLength)
                {
                    throw new UserFriendlyException(L("Warn_File_SizeLimit", MaxFilesLengthUserFriendlyValue));
                }

                var fileType = Path.GetExtension(file.FileName).Substring(1);
                if (FilesAllowedFileTypes != null && FilesAllowedFileTypes.Length > 0 && !FilesAllowedFileTypes.Contains(fileType))
                {
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes", FilesAllowedFileTypes));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var fileToken = Guid.NewGuid().ToString("N");
                _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

                return new FileUploadCacheOutput(fileToken);
            }
            catch (UserFriendlyException ex)
            {
                return new FileUploadCacheOutput(new ErrorInfo(ex.Message));
            }
        }

        public string[] GetFilesFileAllowedTypes()
        {
            return FilesAllowedFileTypes;
        }

    }
}