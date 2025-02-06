using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.ImageModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Services;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("GetAllByBlindBoxId/{blindBoxId}")]
        public async Task<IActionResult> GetAllByBlindBoxId(int blindBoxId)
        {
            var images = await _imageService.GetAllImageByBlindBoxID(blindBoxId);
            if (images == null)
            {
                return NotFound();
            }
            return Ok(images);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            var images = await _imageService.GetImageById(id);
            if (images == null)
            {
                return NotFound();
            }
            return Ok(images);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(AddImageRequest request)
        {
            if (request.Files.Count != request.IsMainImage.Count)
            {
                return BadRequest("Each file must have status `isMainImage`.");
            }

            var images = await _imageService.UploadImage(request);
            if (!images)
            {
                return BadRequest("No files uploaded.");
            }
                
            return Ok(images);
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var result = await _imageService.DeleteImage(imageId);
            if (!result)
            {
                return BadRequest(new { message = "Image not found or already deleted." });
            }
            return Ok(new { message = "Image deleted successfully." });
        }
    }
}
