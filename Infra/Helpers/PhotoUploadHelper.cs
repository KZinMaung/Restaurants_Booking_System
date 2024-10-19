using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infra.Helpers
{
    public class PhotoUploadHelper
    {
        private readonly string _uploadsFolder;

        public PhotoUploadHelper(string uploadsFolder)
        {
            _uploadsFolder = uploadsFolder;
            // Ensure the uploads folder exists
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        public async Task<string> UploadPhotoAsync(IFormFile? file)
        {
            // Validate file
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            // Validate file type (optional)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!Array.Exists(allowedExtensions, e => e.Equals(fileExtension)))
            {
                throw new ArgumentException("Invalid file type.");
            }

            // Generate a unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            // Define the file path
            string filePath = Path.Combine(_uploadsFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the relative file path or the file name for database storage
            return uniqueFileName; // or return Path.Combine("uploads", uniqueFileName);
        }

        // Optional: Add methods for cloud uploads or other features here
    }
}

