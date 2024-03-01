using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using static Google.Apis.Drive.v3.DriveService;
using Microsoft.Extensions.Logging;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Application;

namespace Infrastructure.Account.Services;

public class GoogleDriveService : IGoogleDriveService
{
    private ILogger<GoogleDriveService> _logger;
    private IConfiguration _configuration;
    private const string FolderName = "StudentProjects";

    public GoogleDriveService(ILogger<GoogleDriveService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<(bool status, string linkToProject)> TryUpload(string fileName, Stream file)
    {
        _logger.LogInformation("Uploading project to Google Drive");
        try
        {
            var service = GetService();
            var folder = await GetFolderIdAsync(FolderName);
            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = fileName;
            driveFile.Parents = new string[] { folder };

            var filePath = ((FileStream)file).Name;
            var fileExtension = new FileInfo(filePath).Extension;
            var fileMime = "application/x-zip-compressed";

            var createProject = service.Files.Create(driveFile, file, fileMime);

            var response = await createProject.UploadAsync();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                throw response.Exception;

            return (true, createProject.ResponseBody.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return (false, string.Empty);
        }
    }

    private async Task<string> GetFolderIdAsync(string folderName)
    {
        var service = GetService();
        var fileList = service.Files.List();
        fileList.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{folderName}'";

        var result = await fileList.ExecuteAsync();

        return result.Files.Select(x => x.Id).FirstOrDefault();
    }

    private DriveService GetService()
    {
        var tokenResponse = new TokenResponse
        {
            AccessToken = _configuration[ApplicationConstants.GOOGLE_DRIVE_ACCESS_TOKEN],
            RefreshToken = _configuration[ApplicationConstants.GOOGLE_DRIVE_REFRESH_TOKEN]
        };


        var applicationName = "StudentProjects";
        var username = "silkroadprofessionals.academy@gmail.com";

        var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _configuration[ApplicationConstants.GOOGLE_DRIVE_CLIENT_ID],
                ClientSecret = _configuration[ApplicationConstants.GOOGLE_DRIVE_CLIENT_SECRET]
            },
            Scopes = new[] { Scope.Drive },
            DataStore = new FileDataStore(applicationName)
        });


        var credential = new UserCredential(apiCodeFlow, username, tokenResponse);

        var service = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = applicationName
        });
        return service;
    }

}
