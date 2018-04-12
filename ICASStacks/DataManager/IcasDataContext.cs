using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ICASStacks.DataContract;
using ICASStacks.DataContract.BioEnroll;
using ICASStacks.DataContract.ChurchAdministrative;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.DataManager
{
    internal partial class IcasDataContext : DbContext
    {

        public IcasDataContext()
            : base("name=ICASDBEntities")
        {
            Configuration.ProxyCreationEnabled = false;
        }
        
        //#region Terminal API Contracts
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<StaffUser> StaffUsers { get; set; }
        //public DbSet<UserProfile> UserProfiles { get; set; }
        //public DbSet<StationReg> StationRegs { get; set; }
        //public DbSet<BeneficiaryBiometric> BeneficiaryBiometrics { get; set; } 

        //#endregion



        #region ChurchAdministratives

        public DbSet<Church> Churches { get; set; }

        public DbSet<ChurchStructureParishHeadQuarter> ChurchStructureParishHeadQuarters { get; set; } 
        public DbSet<ChurchService> ChurchServices { get; set; }
        public DbSet<ChurchMember> ChurchMembers { get; set; }
        public DbSet<RoleInChurch> RoleInChurches { get; set; }
        public DbSet<Profession> Professions { get; set; }


        #region Latest

        public DbSet<ChurchServiceType> ChurchServiceTypes { get; set; }
        public DbSet<CollectionType> CollectionTypes { get; set; }
        public DbSet<ClientChurchCollectionType> ClientChurchCollectionTypes { get; set; }
        public DbSet<ChurchCollectionType> ChurchCollectionTypes { get; set; }
        public DbSet<ChurchRoleType> ChurchRoleTypes { get; set; }
        public DbSet<ChurchThemeSetting> ChurchThemeSettings { get; set; }

        #endregion

        #endregion



        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientChurch> ClientChurches { get; set; }
        public DbSet<ChurchStructureType> ChurchStructureTypes { get; set; }
        public DbSet<ChurchStructure> ChurchStructures { get; set; }
        public DbSet<ChurchServiceAttendance> ChurchServiceAttendances { get; set; }
        public DbSet<StateOfLocation> StateOfLocations { get; set; }
        public DbSet<Bank> Banks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<IcasDataContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            #region ChurchServiceAttendance 1 : Many Relation

            modelBuilder.Entity<ChurchStructureParishHeadQuarter>()
                .Property(x => x._Parish)
                .HasColumnName("Parish").HasColumnType("text")
                .IsRequired();


            modelBuilder.Entity<Client>()
                .Property(x => x._Parish)
                .HasColumnName("ClientStructureChurchHeadQuarter").HasColumnType("text")
                .IsOptional();

            modelBuilder.Entity<ClientChurch>()
                .Property(x => x._Parish)
                .HasColumnName("ClientStructureChurchHeadQuarter").HasColumnType("text")
                .IsOptional();

            modelBuilder.Entity<ClientChurch>()
               .Property(x => x._Account)
               .HasColumnName("AccountInfo").HasColumnType("text")
               .IsOptional();


            modelBuilder.Entity<ChurchService>()
                .Property(x => x._ServiceTypeDetail)
                .HasColumnName("ServiceTypeDetail").HasColumnType("text")
                .IsOptional();

            modelBuilder.Entity<ClientChurchService>()
                .Property(x => x._ServiceTypeDetail)
                .HasColumnName("ServiceTypeDetail").HasColumnType("text")
                .IsOptional();


            modelBuilder.Entity<ChurchCollectionType>()
                .Property(x => x._Collection)
                .HasColumnName("CollectionTypes").HasColumnType("text")
                .IsOptional();

            modelBuilder.Entity<ClientChurchCollectionType>()
                .Property(x => x._Collection)
                .HasColumnName("CollectionTypes").HasColumnType("text")
                .IsOptional();

            modelBuilder.Entity<ChurchServiceAttendance>()
               .Property(x => x._ServiceAttendanceDetail)
               .HasColumnName("ServiceAttendanceDetail").HasColumnType("text")
               .IsOptional();

            modelBuilder.Entity<ChurchStructure>()
               .Property(x => x._ChurchStructureTypeDetail)
               .HasColumnName("ChurchStructureTypeDetail").HasColumnType("text")
               .IsOptional();
            
            
            modelBuilder.Entity<Church>()
                .HasMany(x => x.ChurchStructureParishHeadQuarters)
                .WithRequired(x => x.Church)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Church>()
                .HasMany(x => x.ChurchCollectionTypes)
                .WithRequired(x => x.Church)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ChurchStructureType>()
                .HasMany(x => x.ChurchStructureParishHeadQuarters)
                .WithRequired(x => x.ChurchStructureType)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ClientChurch>()
                .HasMany(x => x.ChurchServiceAttendances)
                .WithRequired(x => x.ClientChurch)
                .WillCascadeOnDelete(true);
            
            #endregion

            
        }
    }
}
