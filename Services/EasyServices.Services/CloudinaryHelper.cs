namespace EasyServices.Services
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryHelper
    {
        public static async Task<ICollection<string>> UploadFilesAsync(Cloudinary cloudinary, IEnumerable<IFormFile> files)
        {
            List<string> images = new List<string>();


            foreach (var file in files)
            {
                if (IsFileValid(file))
                {
                    break;
                }

                byte[] byteImage;

                using var memoryStream = new MemoryStream();

                await file.CopyToAsync(memoryStream);

                byteImage = memoryStream.ToArray();

                using var destinationStram = new MemoryStream(byteImage);

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, destinationStram),
                    Transformation = new Transformation().Width(200).Height(200),
                };
                var result = await cloudinary.UploadAsync(uploadParams);

                images.Add(result.Uri.AbsoluteUri);
            }

            return images;
        }

        public static async Task<string> UploadFileAsync(Cloudinary cloudinary, IFormFile file, bool isProfilePicture)
        {
            if (!IsFileValid(file))
            {
                return null;
            }

            byte[] byteImage;

            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            byteImage = memoryStream.ToArray();

            using var destinationStram = new MemoryStream(byteImage);

            ImageUploadParams uploadParams;

            if (!isProfilePicture)
            {
                uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, destinationStram),
                    Transformation = new Transformation().Width(500).Height(400).Quality("auto"),
                };
            }
            else
            {
                uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, destinationStram),
                    Transformation = new Transformation().Width(200).Height(200),
                };
            }

            var result = await cloudinary.UploadAsync(uploadParams);

            return result.Uri.AbsoluteUri;
        }

        public static bool IsFileValid(IFormFile photoFile)
        {
            if (photoFile == null)
            {
                return true;
            }

            string[] validTypes = new string[]
            {
                "image/x-png", "image/gif", "image/jpeg", "image/jpg", "image/png", "image/gif", "image/svg",
            };

            if (validTypes.Contains(photoFile.ContentType) == false)
            {
                return false;
            }

            return true;
        }

        public static async Task RemoveFileAsync(Cloudinary cloudinary, string url)
        {
            var publicId = GetIdFromUrl(url);

            var deletionParams = new DeletionParams(publicId);
            await cloudinary.DestroyAsync(deletionParams);
        }

        private static string GetIdFromUrl(string url)
        {
            var startIndex = url.IndexOf('/') + 1;
            var length = url.LastIndexOf('.') - startIndex;
            var publicId = url.Substring(startIndex, length);

            return publicId;
        }
    }
}
