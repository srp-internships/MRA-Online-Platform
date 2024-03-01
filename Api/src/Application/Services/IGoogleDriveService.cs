using Google.Apis.Drive.v3;

namespace Application.Services;

public interface IGoogleDriveService
{
    public Task<(bool status, string linkToProject)> TryUpload(string fileName, Stream file);
}
