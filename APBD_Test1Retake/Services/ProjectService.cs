using APBD_Test1Retake.Models.DTOs;
using APBD_Test1Retake.Repositories;

namespace APBD_Test1Retake.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public Task<ProjectDetailsResponse?> GetProjectByIdAsync(int projectId)
    {
        return _repository.GetProjectByIdAsync(projectId);
    }

    public Task<bool> AddArtifactWithProjectAsync(AddArtifactRequest request)
    {
        return _repository.AddArtifactWithProjectAsync(request);
    }
}