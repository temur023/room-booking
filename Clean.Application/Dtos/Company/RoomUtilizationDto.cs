namespace Clean.Application.Dtos.Company;

public class RoomUtilizationDto
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public double UtilizationPercent { get; set; }
}