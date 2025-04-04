namespace DataCat.Server.Domain.Identity.Enums;

public abstract class UserRole(string name, int value)
    : SmartEnum<UserRole, int>(name, value)
{
    public static readonly UserRole Admin = new AdminUserRole();
    public static readonly UserRole Viewer = new ViewerUserRole();
    public static readonly UserRole Editor = new EditorUserRole();

    private sealed class AdminUserRole() : UserRole("Admin", 1);

    private sealed class ViewerUserRole() : UserRole("Viewer", 2);

    private sealed class EditorUserRole() : UserRole("Editor", 3);
    
    public static IReadOnlyCollection<UserRole> All =>
    [
        Admin, Viewer, Editor
    ];
}
