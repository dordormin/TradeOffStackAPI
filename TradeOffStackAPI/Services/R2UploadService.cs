using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Services;

public class R2UploadService : IR2UploadService
{
    private readonly string _bucketName;
    private readonly string _publicUrl;
    private readonly IAmazonS3 _s3Client;

    public R2UploadService(IConfiguration configuration)
    {
        var accessKey = configuration["CloudflareR2:AccessKey"] ?? throw new ArgumentNullException("CloudflareR2:AccessKey");
        var secretKey = configuration["CloudflareR2:SecretKey"] ?? throw new ArgumentNullException("CloudflareR2:SecretKey");
        var serviceUrl = configuration["CloudflareR2:EndpointUrl"] ?? throw new ArgumentNullException("CloudflareR2:EndpointUrl");
        _bucketName = configuration["CloudflareR2:BucketName"] ?? throw new ArgumentNullException("CloudflareR2:BucketName");
        _publicUrl = configuration["CloudflareR2:PublicUrl"] ?? ""; // Optional custom domain

        var s3Config = new AmazonS3Config
        {
            ServiceURL = serviceUrl,
            ForcePathStyle = true // Important for R2
        };

        _s3Client = new AmazonS3Client(accessKey, secretKey, s3Config);
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var objectKey = string.IsNullOrEmpty(folder) ? uniqueFileName : $"{folder}/{uniqueFileName}";

        using var newMemoryStream = new MemoryStream();
        await file.CopyToAsync(newMemoryStream);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = objectKey,
            BucketName = _bucketName,
            ContentType = file.ContentType,
            DisablePayloadSigning = true
        };

        var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        // Generate the URL to return to the client
        // If a public URL (like https://assets.tradeoffstack.com) is configured, use it.
        // Otherwise, return the standard R2 dev URL structure (which is usually not public by default, but we'll try to form it).
        if (!string.IsNullOrEmpty(_publicUrl))
        {
            return $"{_publicUrl.TrimEnd('/')}/{objectKey}";
        }
        
        // Note: For R2, if you don't have a public custom domain set up, you cannot access objects via a public URL directly
        // unless you configure a public bucket policy or enable Public R2.dev domain in the Cloudflare dashboard.
        // We'll return the S3 URL format as fallback.
        var endpointUrl = ((AmazonS3Client)_s3Client).Config.ServiceURL;
        return $"{endpointUrl.TrimEnd('/')}/{_bucketName}/{objectKey}";
    }
}
