using Core.ControllerBases;
using Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using PhotoAPI.Dtos;

namespace PhotoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", photo.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream, cancellationToken);
                }

                var returnPath = "images/" + photo.FileName;

                var photoDto = new PhotoDto() { URL = returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }

            return CreateActionResultInstance(Response<PhotoDto>.Fail("Photo is null or empty.", 400));
        }

        [HttpGet]
        public IActionResult PhotoDelete(string photoName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", photoName);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("URL is not exist.", 404));
            }
            System.IO.File.Delete(path);

            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}
