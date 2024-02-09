using technicalevaluation.Enum;

namespace technicalevaluation.Models
{
    public class UnitInfo
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int UnitId { get; set; }

        public StatusUnit Status { get; set; }

    }
}
