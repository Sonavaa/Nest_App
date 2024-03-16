using Microsoft.CodeAnalysis.CSharp.Syntax;
using NestApp.Enums;
using NestApp.Models;

namespace NestApp.FileExtension
{
    public static class FileExtensions
    {
        public static async Task<string> SaveFilesAsync(this IFormFile file, string root, string client, string folderName)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string path = Path.Combine(root, client, folderName, uniqueFileName);

            using FileStream fs = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fs);
            return uniqueFileName;
        }

        public static bool CheckFileType(this IFormFile file, string fileType)
        {
            if (file.ContentType.StartsWith(fileType)){
                return true;
            }
            return false;
        }
        public static bool CheckFileSize(this IFormFile file, int fileSize)
        {
            if (file.Length < fileSize * 2 * 1024)
            {
                return true;
            }
            return false;
        }

        public static void DeleteFile(this IFormFile file, string root, string client, string folderName, string fileName)
        {
            string path = Path.Combine(root, client, folderName, fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

    }
}
