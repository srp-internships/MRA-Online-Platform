using Application.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.IntegrationTest;

public class TestGoogleDriveService : IGoogleDriveService
{
    public async Task<(bool status, string linkToProject)> TryUpload(string fileName, Stream file)
    {
        if (fileName.Contains("TestUploading"))
        {
            return await Task.FromResult((false, ""));
        }
        return await Task.FromResult((true, Guid.NewGuid().ToString()));
    }
}
