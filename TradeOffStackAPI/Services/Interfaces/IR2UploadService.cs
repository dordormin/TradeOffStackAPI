using Microsoft.AspNetCore.Http;

namespace TradeOffStackAPI.Services.Interfaces;

public interface IR2UploadService
{
    Task<string> UploadImageAsync(IFormFile file, string folder);
}
