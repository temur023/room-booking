using Clean.Domain.Enitities;

namespace Clean.Application.Filters;

public class RoomFilter
{
    public int? Floor { get; set; }
    public int? MinCapacity { get; set; }
    public int? MaxCapacity { get; set; }
    public bool? IsActive { get; set; }
    
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public List<string> Equipments { get; set; } = new List<string>();
}