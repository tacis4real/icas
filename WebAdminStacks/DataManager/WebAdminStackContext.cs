using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebAdminStacks.DataContract;

namespace WebAdminStacks.DataManager
{
    internal partial class WebAdminStackEntities : DbContext
    {

        public WebAdminStackEntities() :
            base("name=ICASWebAdminStackEntities")
        {
            Configuration.ProxyCreationEnabled = false;
        }
        

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<UserLoginActivity> UserLoginActivities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<DeviceAccessAuthorization> DeviceAccessAuthorizations { get; set; }

        #region Client

        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<RoleClient> RoleClients { get; set; }
        public DbSet<ClientRole> ClientRoles { get; set; } 
        public DbSet<ClientDevice> ClientDevices { get; set; }
        public DbSet<ClientLoginActivity> ClientLoginActivities { get; set; }
        public DbSet<ClientDeviceAccessAuthorization> ClientDeviceAccessAuthorizations { get; set; }
        
        #endregion


        #region Client Church

        public DbSet<ClientChurchProfile> ClientChurchProfiles { get; set; }
        public DbSet<ClientChurchRole> ClientChurchRoles { get; set; }
        public DbSet<ClientChurchDevice> ClientChurchDevices { get; set; }
        public DbSet<ClientChurchLoginActivity> ClientChurchLoginActivities { get; set; }
        public DbSet<ClientChurchDeviceAccessAuthorization> ClientChurchDeviceAccessAuthorizations { get; set; }

        #endregion




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<WebAdminStackEntities>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region Portal Admins
            
            modelBuilder.Entity<Role>()
                .HasMany(x => x.UserRoles)
                .WithRequired(x => x.Role)
                .WillCascadeOnDelete(true);
           
            modelBuilder.Entity<User>()
                .HasMany(x => x.UserRoles)
                .WithRequired(x => x.User)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(x => x.UserDevices)
                .WithRequired(x => x.User)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(x => x.UserLoginActivities)
                .WithRequired(x => x.User)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<UserDevice>()
                .HasMany(x => x.DeviceAccessAuthorizations)
                .WithRequired(x => x.UserDevice)
                .WillCascadeOnDelete(true);

            #endregion

            #region Client

            modelBuilder.Entity<ClientProfile>()
                .HasMany(x => x.ClientRoles)
                .WithRequired(x => x.ClientProfile)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ClientProfile>()
                .HasMany(x => x.ClientDevices)
                .WithRequired(x => x.ClientProfile)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ClientProfile>()
                .HasMany(x => x.ClientLoginActivities)
                .WithRequired(x => x.ClientProfile)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RoleClient>()
                .HasMany(x => x.ClientRoles)
                .WithRequired(x => x.RoleClient)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ClientDevice>()
                .HasMany(x => x.ClientDeviceAccessAuthorizations)
                .WithRequired(x => x.ClientDevice)
                .WillCascadeOnDelete(true);

            #endregion

            #region Client Church

            modelBuilder.Entity<ClientChurchProfile>()
                .HasMany(x => x.ClientChurchRoles)
                .WithRequired(x => x.ClientChurchProfile)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ClientChurchProfile>()
                .HasMany(x => x.ClientChurchDevices)
                .WithRequired(x => x.ClientChurchProfile)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ClientChurchProfile>()
                .HasMany(x => x.ClientChurchLoginActivities)
                .WithRequired(x => x.ClientChurchProfile)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RoleClient>()
                .HasMany(x => x.ClientChurchRoles)
                .WithRequired(x => x.RoleClient)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ClientChurchDevice>()
                .HasMany(x => x.ClientChurchDeviceAccessAuthorizations)
                .WithRequired(x => x.ClientChurchDevice)
                .WillCascadeOnDelete(true);

            #endregion
        }
        
    }
}
