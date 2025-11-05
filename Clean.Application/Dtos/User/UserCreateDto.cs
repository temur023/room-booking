
using Clean.Domain.Entities;

namespace Clean.Application.Dtos.User;

public class UserCreateDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; } // We Create Them base password which they can then change
    public Role Role { get; set; }
}