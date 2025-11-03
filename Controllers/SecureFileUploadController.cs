using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Text;

namespace GCBQuotationSystem.Controllers;

/// <summary>
/// Secure API endpoint for CSV file uploads
/// Requires X-API-Key header for authentication
/// </summary>
[ApiController]
[Route("api/secure")]
public class SecureFileUploadController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<SecureFileUploadController> _logger;

    public SecureFileUploadController(
        IWebHostEnvironment environment,
        ILogger<SecureFileUploadController> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    /// <summary>
    /// Upload a CSV file
    /// Requires X-API-Key header with valid API key
    /// </summary>
    [HttpPost("upload-csv")]
    [RequestSizeLimit(10_000_000)] // 10MB max file size
    [EnableRateLimiting("ApiRateLimit")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        try
        {
            // Validate file
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "No file uploaded or file is empty." });
            }

            // Validate file extension
            var allowedExtensions = new[] { ".csv" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { error = "Only CSV files are allowed." });
            }

            // Validate file size (10MB max)
            if (file.Length > 10_000_000)
            {
                return BadRequest(new { error = "File size exceeds 10MB limit." });
            }

            // Ensure the secure uploads directory exists
            var uploadsDirectory = Path.Combine(_environment.ContentRootPath, "SecureUploads", "CSV");
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
                _logger.LogInformation("Created SecureUploads/CSV directory");
            }

            // Generate unique filename with timestamp
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var sanitizedFileName = SanitizeFileName(file.FileName);
            var uniqueFileName = $"{timestamp}_{sanitizedFileName}";
            var filePath = Path.Combine(uploadsDirectory, uniqueFileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation($"File uploaded successfully: {uniqueFileName} (Size: {file.Length} bytes)");

            return Ok(new
            {
                message = "File uploaded successfully.",
                fileName = uniqueFileName,
                originalFileName = file.FileName,
                fileSize = file.Length,
                uploadedAt = DateTime.UtcNow,
                filePath = $"SecureUploads/CSV/{uniqueFileName}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file");
            return StatusCode(500, new { error = "An error occurred while uploading the file." });
        }
    }

    /// <summary>
    /// Get list of uploaded CSV files (metadata only)
    /// </summary>
    [HttpGet("list-csv")]
    public IActionResult ListCsvFiles()
    {
        try
        {
            var uploadsDirectory = Path.Combine(_environment.ContentRootPath, "SecureUploads", "CSV");
            
            if (!Directory.Exists(uploadsDirectory))
            {
                return Ok(new { files = Array.Empty<object>() });
            }

            var files = Directory.GetFiles(uploadsDirectory, "*.csv")
                .Select(filePath =>
                {
                    var fileInfo = new FileInfo(filePath);
                    return new
                    {
                        fileName = Path.GetFileName(filePath),
                        size = fileInfo.Length,
                        uploadedAt = fileInfo.CreationTimeUtc,
                        modifiedAt = fileInfo.LastWriteTimeUtc
                    };
                })
                .OrderByDescending(f => f.uploadedAt)
                .ToList();

            return Ok(new { count = files.Count, files });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing files");
            return StatusCode(500, new { error = "An error occurred while listing files." });
        }
    }

    /// <summary>
    /// Download a specific CSV file by filename
    /// </summary>
    [HttpGet("download-csv/{fileName}")]
    public IActionResult DownloadCsv(string fileName)
    {
        try
        {
            // Security: Prevent directory traversal attacks
            var sanitizedFileName = SanitizeFileName(fileName);
            
            var filePath = Path.Combine(_environment.ContentRootPath, "SecureUploads", "CSV", sanitizedFileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { error = "File not found." });
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file");
            return StatusCode(500, new { error = "An error occurred while downloading the file." });
        }
    }

    /// <summary>
    /// Sanitize filename to prevent directory traversal attacks
    /// </summary>
    private string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = new StringBuilder();
        
        foreach (char c in fileName)
        {
            if (!invalidChars.Contains(c))
            {
                sanitized.Append(c);
            }
        }
        
        return sanitized.ToString();
    }
}

