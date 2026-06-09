using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UploadController : ControllerBase
{
    private readonly IR2UploadService _uploadService;

    public UploadController(IR2UploadService uploadService)
    {
        _uploadService = uploadService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string folder = "Equipments")
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded." });
        }

        // Validate file extension
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new { message = "Invalid file type. Only JPG, JPEG, PNG, GIF, and WEBP are allowed." });
        }

        try
        {
            var fileUrl = await _uploadService.UploadImageAsync(file, folder);

            return Ok(new
            {
                image_url = fileUrl,
                filename = file.FileName
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }
}
