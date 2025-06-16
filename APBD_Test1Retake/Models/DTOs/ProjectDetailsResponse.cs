namespace APBD_Test1Retake.Models.DTOs;

public class ProjectDetailsResponse
{
    public int ProjectId { get; set; }
    public string Objective { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ArtifactDetails Artifact { get; set; } = null!;
    public List<StaffAssignment> StaffAssignments { get; set; } = new();
}

public class ArtifactDetails
{
    public string Name { get; set; } = null!;
    public DateTime OriginDate { get; set; }
    public Institution Institution { get; set; } = null!;
}