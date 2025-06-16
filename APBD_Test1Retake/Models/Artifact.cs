namespace APBD_Test1Retake.Models;

public class Artifact
{
    public int ArtifactId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime OriginDate { get; set; }
    public int InstitutionId { get; set; }
}