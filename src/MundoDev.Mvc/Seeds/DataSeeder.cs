using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Business.Models.Domains.Relationships;
using MundoDev.Data.Contexts;

namespace MundoDev.Mvc.Seeds
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();

            await SeedRolesAsync(context);
            await SeedModulesAndPermissionsAsync(context);
            await SeedUsersAsync(context);
            await SeedRolePermissionsAsync(context);
        }

        // ── FIXED IDs ──────────────────────────────────────────────────────────────

        // Roles
        private static readonly Guid RoleAlunoId        = new("00000001-0000-0000-0000-000000000001");
        private static readonly Guid RoleSupervisorId   = new("00000001-0000-0000-0000-000000000002");
        private static readonly Guid RoleAdminId        = new("00000001-0000-0000-0000-000000000003");

        // Users
        private static readonly Guid UserCarlosId       = new("00000002-0000-0000-0000-000000000001");
        private static readonly Guid UserSupervisorId   = new("00000002-0000-0000-0000-000000000002");
        private static readonly Guid UserAdminId        = new("00000002-0000-0000-0000-000000000003");

        // Modules
        private static readonly Guid ModuleUsersId      = new("00000003-0000-0000-0000-000000000001");
        private static readonly Guid ModuleRolesId      = new("00000003-0000-0000-0000-000000000002");
        private static readonly Guid ModuleModulesId    = new("00000003-0000-0000-0000-000000000003");
        private static readonly Guid ModulePermsId      = new("00000003-0000-0000-0000-000000000004");
        private static readonly Guid ModuleCoursesId    = new("00000003-0000-0000-0000-000000000005");
        private static readonly Guid ModuleTopicsId     = new("00000003-0000-0000-0000-000000000006");
        private static readonly Guid ModuleLessonsId    = new("00000003-0000-0000-0000-000000000007");
        private static readonly Guid ModuleArticlesId   = new("00000003-0000-0000-0000-000000000008");
        private static readonly Guid ModulePlansId      = new("00000003-0000-0000-0000-000000000009");
        private static readonly Guid ModuleOrdersId     = new("00000003-0000-0000-0000-000000000010");

        // ── SEED METHODS ───────────────────────────────────────────────────────────

        private static async Task SeedRolesAsync(MainDbContext context)
        {
            if (await context.Set<Role>().AnyAsync()) return;

            context.Set<Role>().AddRange(
                new Role { Id = RoleAlunoId,      Name = "Aluno",          IsActived = true },
                new Role { Id = RoleSupervisorId, Name = "Supervisor",     IsActived = true },
                new Role { Id = RoleAdminId,      Name = "Administrador",  IsActived = true }
            );
            await context.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(MainDbContext context)
        {
            if (await context.Set<User>().AnyAsync()) return;

            var hash = BCrypt.Net.BCrypt.HashPassword("Password.123");

            context.Set<User>().AddRange(
                new User
                {
                    Id          = UserCarlosId,
                    RoleId      = RoleAlunoId,
                    FirstName   = "Carlos",
                    LastName    = "Freire",
                    Email       = "carlosrenatofreire@hotmail.com",
                    Password    = hash,
                    PhoneNumber = "+55 11 99999-0001",
                    City        = "São Paulo",
                    State       = "SP",
                    Country     = "Brasil",
                    CreatedDate = DateTime.UtcNow,
                    IsActived   = true
                },
                new User
                {
                    Id          = UserSupervisorId,
                    RoleId      = RoleSupervisorId,
                    FirstName   = "Supervisor",
                    LastName    = "MundoDev",
                    Email       = "supervisor@mundodev.app.br",
                    Password    = hash,
                    PhoneNumber = "+55 11 99999-0002",
                    City        = "São Paulo",
                    State       = "SP",
                    Country     = "Brasil",
                    CreatedDate = DateTime.UtcNow,
                    IsActived   = true
                },
                new User
                {
                    Id          = UserAdminId,
                    RoleId      = RoleAdminId,
                    FirstName   = "Administrador",
                    LastName    = "MundoDev",
                    Email       = "admin@mundodev.app.br",
                    Password    = hash,
                    PhoneNumber = "+55 11 99999-0003",
                    City        = "São Paulo",
                    State       = "SP",
                    Country     = "Brasil",
                    CreatedDate = DateTime.UtcNow,
                    IsActived   = true
                }
            );
            await context.SaveChangesAsync();
        }

        private static async Task SeedModulesAndPermissionsAsync(MainDbContext context)
        {
            if (await context.Set<Module>().AnyAsync()) return;

            var modules = new[]
            {
                new Module { Id = ModuleUsersId,   Name = "Users",       IsActived = true },
                new Module { Id = ModuleRolesId,   Name = "Roles",       IsActived = true },
                new Module { Id = ModuleModulesId, Name = "Modules",     IsActived = true },
                new Module { Id = ModulePermsId,   Name = "Permissions", IsActived = true },
                new Module { Id = ModuleCoursesId, Name = "Courses",     IsActived = true },
                new Module { Id = ModuleTopicsId,  Name = "Topics",      IsActived = true },
                new Module { Id = ModuleLessonsId, Name = "Lessons",     IsActived = true },
                new Module { Id = ModuleArticlesId,Name = "Articles",    IsActived = true },
                new Module { Id = ModulePlansId,   Name = "Plans",       IsActived = true },
                new Module { Id = ModuleOrdersId,  Name = "Orders",      IsActived = true },
            };
            context.Set<Module>().AddRange(modules);
            await context.SaveChangesAsync();

            // Permissions: Ver / Adicionar / Editar / Excluir per module
            var permNames = new[] { "Ver", "Adicionar", "Editar", "Excluir" };
            var moduleIds = new[]
            {
                ModuleUsersId, ModuleRolesId, ModuleModulesId, ModulePermsId,
                ModuleCoursesId, ModuleTopicsId, ModuleLessonsId, ModuleArticlesId,
                ModulePlansId, ModuleOrdersId
            };

            int seq = 1;
            var permissions = new List<Permission>();
            foreach (var modId in moduleIds)
            {
                foreach (var pname in permNames)
                {
                    permissions.Add(new Permission
                    {
                        Id        = new Guid($"00000004-0000-0000-0000-{seq:D12}"),
                        ModuleId  = modId,
                        Name      = pname,
                        IsActived = true
                    });
                    seq++;
                }
            }
            context.Set<Permission>().AddRange(permissions);
            await context.SaveChangesAsync();
        }

        private static async Task SeedRolePermissionsAsync(MainDbContext context)
        {
            if (await context.Set<RolePermission>().AnyAsync()) return;

            var allPermissionIds = await context.Set<Permission>()
                .Select(p => p.Id).ToListAsync();

            // Administrador → all permissions
            var adminRolePerms = allPermissionIds.Select((pid, i) => new RolePermission
            {
                Id           = new Guid($"00000005-0000-0000-0000-{(i + 1):D12}"),
                RoleId       = RoleAdminId,
                PermissionId = pid
            });

            // Supervisor → Ver + Adicionar + Editar (not Excluir)
            var supervisorPermIds = await context.Set<Permission>()
                .Where(p => p.Name != "Excluir")
                .Select(p => p.Id).ToListAsync();

            var supervisorRolePerms = supervisorPermIds.Select((pid, i) => new RolePermission
            {
                Id           = new Guid($"00000006-0000-0000-0000-{(i + 1):D12}"),
                RoleId       = RoleSupervisorId,
                PermissionId = pid
            });

            // Aluno → only Ver
            var alunoPermIds = await context.Set<Permission>()
                .Where(p => p.Name == "Ver")
                .Select(p => p.Id).ToListAsync();

            var alunoRolePerms = alunoPermIds.Select((pid, i) => new RolePermission
            {
                Id           = new Guid($"00000007-0000-0000-0000-{(i + 1):D12}"),
                RoleId       = RoleAlunoId,
                PermissionId = pid
            });

            context.Set<RolePermission>().AddRange(adminRolePerms);
            context.Set<RolePermission>().AddRange(supervisorRolePerms);
            context.Set<RolePermission>().AddRange(alunoRolePerms);
            await context.SaveChangesAsync();
        }
    }
}
