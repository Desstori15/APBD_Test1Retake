namespace APBD_Test1Retake.Models.DTOs;

public class AddArtifactRequest
{
    public Artifact Artifact { get; set; } = null!;
    public PreservationProject Project { get; set; } = null!;
}

