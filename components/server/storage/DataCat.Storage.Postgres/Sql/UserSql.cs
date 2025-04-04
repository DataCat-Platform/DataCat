namespace DataCat.Storage.Postgres.Sql;

public static class UserSql
{
    public static class Select
    {
        public const string FindById = $"""
          SELECT
              u.{Public.Users.Id}               {nameof(UserSnapshot.UserId)}
              ,u.{Public.Users.IdentityId}      {nameof(UserSnapshot.IdentityId)}
              ,u.{Public.Users.Name}            {nameof(UserSnapshot.Name)}
              ,u.{Public.Users.Email}           {nameof(UserSnapshot.Name)}
              ,u.{Public.Users.CreatedAt}       {nameof(UserSnapshot.Name)}
              ,u.{Public.Users.UpdatedAt}       {nameof(UserSnapshot.Name)}
          
              ,url.{Public.UsersRolesLink.RoleId}        {nameof(AssignedUserRoleSnapshot.RoleId)}
              ,url.{Public.UsersRolesLink.NamespaceId}   {nameof(AssignedUserRoleSnapshot.NamespaceId)}
              ,url.{Public.UsersRolesLink.IsManual}      {nameof(AssignedUserRoleSnapshot.IsManual)}
          
              ,upl.{Public.UsersPermissionsLink.PermissionId}    {nameof(AssignedUserPermissionSnapshot.PermissionId)}
              ,upl.{Public.UsersPermissionsLink.NamespaceId}     {nameof(AssignedUserPermissionSnapshot.NamespaceId)}
              ,upl.{Public.UsersPermissionsLink.IsManual}        {nameof(AssignedUserPermissionSnapshot.IsManual)}
          
          FROM 
              {Public.UserTable} u
          LEFT JOIN
              {Public.UserRoleLinkTable} url ON url.{Public.UsersRolesLink.UserId} = u.{Public.Users.Id}
          LEFT JOIN
              {Public.UserPermissionLinkTable} upl ON upl.{Public.UsersPermissionsLink.UserId} = u.{Public.Users.Id}
          WHERE {Public.Users.Id} = @p_user_id
        """;
        
        public const string FindByEmail = $"""
           SELECT
               u.{Public.Users.Id}               {nameof(UserSnapshot.UserId)}
               ,u.{Public.Users.IdentityId}      {nameof(UserSnapshot.IdentityId)}
               ,u.{Public.Users.Name}            {nameof(UserSnapshot.Name)}
               ,u.{Public.Users.Email}           {nameof(UserSnapshot.Name)}
               ,u.{Public.Users.CreatedAt}       {nameof(UserSnapshot.Name)}
               ,u.{Public.Users.UpdatedAt}       {nameof(UserSnapshot.Name)}
           
               ,url.{Public.UsersRolesLink.RoleId}        {nameof(AssignedUserRoleSnapshot.RoleId)}
               ,url.{Public.UsersRolesLink.NamespaceId}   {nameof(AssignedUserRoleSnapshot.NamespaceId)}
               ,url.{Public.UsersRolesLink.IsManual}      {nameof(AssignedUserRoleSnapshot.IsManual)}
           
               ,upl.{Public.UsersPermissionsLink.PermissionId}    {nameof(AssignedUserPermissionSnapshot.PermissionId)}
               ,upl.{Public.UsersPermissionsLink.NamespaceId}     {nameof(AssignedUserPermissionSnapshot.NamespaceId)}
               ,upl.{Public.UsersPermissionsLink.IsManual}        {nameof(AssignedUserPermissionSnapshot.IsManual)}
           
           FROM 
               {Public.UserTable} u
           LEFT JOIN
               {Public.UserRoleLinkTable} url ON url.{Public.UsersRolesLink.UserId} = u.{Public.Users.Id}
           LEFT JOIN
               {Public.UserPermissionLinkTable} upl ON upl.{Public.UsersPermissionsLink.UserId} = u.{Public.Users.Id}
           WHERE {Public.Users.Email} = @p_email
        """;
    }

    public static class Insert
    {
        public const string AddUser = $"""
           INSERT INTO {Public.UserTable} (
               {Public.Users.Id},
               {Public.Users.IdentityId},
               {Public.Users.Name},
               {Public.Users.Email},
               {Public.Users.CreatedAt},
               {Public.Users.UpdatedAt}
           )
           VALUES (
               @{nameof(UserSnapshot.UserId)},
               @{nameof(UserSnapshot.IdentityId)},
               @{nameof(UserSnapshot.Name)},
               @{nameof(UserSnapshot.Email)},
               @{nameof(UserSnapshot.CreatedAt)},
               @{nameof(UserSnapshot.UpdatedAt)}
           )
           ON CONFLICT ({Public.Users.IdentityId}) DO NOTHING;
        """;
    }
}