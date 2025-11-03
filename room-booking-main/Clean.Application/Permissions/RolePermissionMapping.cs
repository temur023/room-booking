using Clean.Domain.Entities;
using Clean.Permissions;

namespace Clean.Permissions;

public static class RolePermissionMapping
{
    public static readonly Dictionary<Role, List<string>> RolePermissions = new()
    {
        {
            Role.Admin, new List<string>
            {
                PermissionConstants.Users.View,
                PermissionConstants.Users.Manage,
                PermissionConstants.Bookings.View,
                PermissionConstants.Bookings.Manage,
                PermissionConstants.Rooms.View,
                PermissionConstants.Rooms.Manage,
                PermissionConstants.RoomEquipments.View,
                PermissionConstants.RoomEquipments.Manage,
            }
        },
        {
            Role.User, new List<string>
            {
                PermissionConstants.Bookings.View,
                PermissionConstants.Company.View,
                PermissionConstants.Rooms.View,
                PermissionConstants.Users.ManageSelf,
                PermissionConstants.RoomEquipments.View,
            }
        },
        {
          Role.SuperAdmin, new List<string>()
          {
              PermissionConstants.Users.View,
              PermissionConstants.Users.Manage,
              PermissionConstants.Bookings.View,
              PermissionConstants.Bookings.Manage,
              PermissionConstants.Company.Manage,
              PermissionConstants.Company.View,
              PermissionConstants.Rooms.View,
              PermissionConstants.Rooms.Manage,
              PermissionConstants.RoomEquipments.View,
              PermissionConstants.RoomEquipments.Manage,
          }  
        },
    };
}