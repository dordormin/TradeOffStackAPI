namespace TradeOffStackAPI.Models;

public class CloudflareR2Settings
{
    public const string SectionName = "CloudflareR2";
    
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string EndpointUrl { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string PublicUrl { get; set; } = string.Empty;
}
