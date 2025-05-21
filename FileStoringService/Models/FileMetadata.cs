namespace FileStoringService.Models;

public record FileMetadata(
    Guid     FileId,
    string   FileName,
    long     FileSize,
    DateTime UploadedAt
);
