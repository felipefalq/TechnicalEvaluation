using technicalevaluation.Models;

public class CollaboratorInfo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? UnitId { get; set; }
    public int? UserId { get; set; }
    public virtual UnitInfo Unit { get; set; }
    public virtual UserInfo User { get; set; }
}
