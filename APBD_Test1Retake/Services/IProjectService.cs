using APBD_Test1Retake.Models.DTOs;

namespace APBD_Test1Retake.Services;

public interface IProjectService
{
    Task<ProjectDetailsResponse?> GetProjectByIdAsync(int projectId);
    Task<bool> AddArtifactWithProjectAsync(AddArtifactRequest request);
}