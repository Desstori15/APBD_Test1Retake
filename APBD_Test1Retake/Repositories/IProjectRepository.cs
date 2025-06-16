using APBD_Test1Retake.Models.DTOs;

namespace APBD_Test1Retake.Repositories;

public interface IProjectRepository
    {
        Task<ProjectDetailsResponse?> GetProjectByIdAsync(int projectId);
        Task<bool> AddArtifactWithProjectAsync(AddArtifactRequest request);
    }
