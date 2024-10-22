using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace api.Services
{
    public class CloudinaryService : ICloudinaryService

    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account=new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary=new Cloudinary(account);
            
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
          var uploadResult=new ImageUploadResult();
          if(file.Length>0){
            using(var stream =file.OpenReadStream()){
                var uploadParams=new ImageUploadParams{
                    File=new FileDescription(file.FileName,stream),
                    // Transformation=new Transformation().Width(500).Height(500)

                };
                try{
                    uploadResult=await _cloudinary.UploadAsync(uploadParams);

                }catch(Exception ex){
                    Console.WriteLine($"Cloudinary Upload Error: {ex.Message}");
                } 
               
            }
          }
          return uploadResult;
        }

        public async Task<DeletionResult> DeletetPhotoAsync(string publicId)
        {
           var deleteParams=new DeletionParams(publicId);
           var result=await _cloudinary.DestroyAsync(deleteParams);
           return result;
        }
    }
}