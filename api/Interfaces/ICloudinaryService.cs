using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace api.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult>AddPhotoAsync(IFormFile file);
        Task<DeletionResult>DeletetPhotoAsync(string publicId);
    }
}