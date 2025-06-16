using APBD_Test1Retake.Models;
using APBD_Test1Retake.Models.DTOs;
using System.Data.SqlClient;

namespace APBD_Test1Retake.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IConfiguration _configuration;

    public ProjectRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ProjectDetailsResponse?> GetProjectByIdAsync(int projectId)
    {
        var response = new ProjectDetailsResponse();
        var staff = new List<StaffAssignment>();

        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await connection.OpenAsync();

        var command = new SqlCommand(@"
            SELECT 
                p.ProjectId, p.Objective, p.StartDate, p.EndDate,
                a.Name AS ArtifactName, a.OriginDate, 
                i.InstitutionId, i.Name AS InstitutionName, i.FoundedYear,
                s.FirstName, s.LastName, s.HireDate, sa.Role
            FROM Preservation_Project p
            JOIN Artifact a ON p.ArtifactId = a.ArtifactId
            JOIN Institution i ON a.InstitutionId = i.InstitutionId
            LEFT JOIN Staff_Assignment sa ON p.ProjectId = sa.ProjectId
            LEFT JOIN Staff s ON sa.StaffId = s.StaffId
            WHERE p.ProjectId = @projectId
        ", connection);

        command.Parameters.AddWithValue("@projectId", projectId);

        using var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
            return null;

        while (await reader.ReadAsync())
        {
            if (response.ProjectId == 0)
            {
                response.ProjectId = reader.GetInt32(0);
                response.Objective = reader.GetString(1);
                response.StartDate = reader.GetDateTime(2);
                response.EndDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3);

                response.Artifact = new ArtifactDetails
                {
                    Name = reader.GetString(4),
                    OriginDate = reader.GetDateTime(5),
                    Institution = new Institution
                    {
                        InstitutionId = reader.GetInt32(6),
                        Name = reader.GetString(7),
                        FoundedYear = reader.GetInt32(8)
                    }
                };
            }

            if (!reader.IsDBNull(9)) // if Staff exists
            {
                var assignment = new StaffAssignment
                {
                    FirstName = reader.GetString(9),
                    LastName = reader.GetString(10),
                    HireDate = reader.GetDateTime(11),
                    Role = reader.GetString(12)
                };
                staff.Add(assignment);
            }
        }

        response.StaffAssignments = staff;
        return response;
    }

    public async Task<bool> AddArtifactWithProjectAsync(AddArtifactRequest request)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            // Insert Artifact
            var insertArtifactCmd = new SqlCommand(@"
                INSERT INTO Artifact (ArtifactId, Name, OriginDate, InstitutionId)
                VALUES (@id, @name, @date, @institutionId)", connection, transaction);
            
            insertArtifactCmd.Parameters.AddWithValue("@id", request.Artifact.ArtifactId);
            insertArtifactCmd.Parameters.AddWithValue("@name", request.Artifact.Name);
            insertArtifactCmd.Parameters.AddWithValue("@date", request.Artifact.OriginDate);
            insertArtifactCmd.Parameters.AddWithValue("@institutionId", request.Artifact.InstitutionId);

            await insertArtifactCmd.ExecuteNonQueryAsync();

            // Insert Project
            var insertProjectCmd = new SqlCommand(@"
                INSERT INTO Preservation_Project (ProjectId, ArtifactId, StartDate, EndDate, Objective)
                VALUES (@projectId, @artifactId, @start, @end, @objective)", connection, transaction);

            insertProjectCmd.Parameters.AddWithValue("@projectId", request.Project.ProjectId);
            insertProjectCmd.Parameters.AddWithValue("@artifactId", request.Artifact.ArtifactId);
            insertProjectCmd.Parameters.AddWithValue("@start", request.Project.StartDate);
            insertProjectCmd.Parameters.AddWithValue("@end", (object?)request.Project.EndDate ?? DBNull.Value);
            insertProjectCmd.Parameters.AddWithValue("@objective", request.Project.Objective);

            await insertProjectCmd.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}
