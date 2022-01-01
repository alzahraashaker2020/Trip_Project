using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models
{
    public partial class TripContext : DbContext
    {
        public TripContext()
        {
        }

        public TripContext(DbContextOptions<TripContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<AreaDiscount> AreaDiscount { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<FavouriteArea> FavouriteArea { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Offer> Offer { get; set; }
        public virtual DbSet<PublicHoliday> PublicHoliday { get; set; }
        public virtual DbSet<Rate> Rate { get; set; }
        public virtual DbSet<Ride> Ride { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\MSSQLSERVER17;Database=Trip;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Longitude).HasMaxLength(50);
            });

            modelBuilder.Entity<AreaDiscount>(entity =>
            {
                entity.ToTable("Area_discount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AreaId).HasColumnName("Area_ID");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.AreaDiscount)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_Area_discount_Area");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EventDate)
                    .HasColumnName("Event_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .HasColumnName("Event_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(100);

                entity.Property(e => e.RideId).HasColumnName("Ride_ID");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.RideId)
                    .HasConstraintName("FK_Event_Ride");
            });

            modelBuilder.Entity<FavouriteArea>(entity =>
            {
                entity.ToTable("Favourite_Area");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AreaId).HasColumnName("Area_Id");

                entity.Property(e => e.DriverId).HasColumnName("Driver_Id");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.FavouriteArea)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_Favourite_Area_Favourite_Area");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.FavouriteArea)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_Favourite_Area_User");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Note).HasMaxLength(200);

                entity.Property(e => e.NotifDate)
                    .HasColumnName("Notif_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NotificationName)
                    .HasColumnName("Notification_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.OperationId).HasColumnName("Operation_Id");

                entity.Property(e => e.SeenStatus).HasColumnName("Seen_Status");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Notification_User");
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DriverId).HasColumnName("Driver_ID");

                entity.Property(e => e.RideId).HasColumnName("Ride_ID");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.Offer)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_Offer_User");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.Offer)
                    .HasForeignKey(d => d.RideId)
                    .HasConstraintName("FK_Offer_Ride");
            });

            modelBuilder.Entity<PublicHoliday>(entity =>
            {
                entity.ToTable("Public_Holiday");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HolidayDate)
                    .HasColumnName("Holiday_Date")
                    .HasColumnType("date");

                entity.Property(e => e.Note).HasMaxLength(100);
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientId).HasColumnName("Client_ID");

                entity.Property(e => e.DriverId).HasColumnName("Driver_ID");

                entity.Property(e => e.Review).HasMaxLength(50);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.RateClient)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Rate_User1");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.RateDriver)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_Rate_User");
            });

            modelBuilder.Entity<Ride>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientId).HasColumnName("Client_ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DiscountVal).HasColumnName("Discount_val");

                entity.Property(e => e.DistinationArea).HasColumnName("Distination_Area");

                entity.Property(e => e.DriverId).HasColumnName("Driver_ID");

                entity.Property(e => e.PassengerNo).HasColumnName("Passenger_No");

                entity.Property(e => e.RideState)
                    .HasColumnName("Ride_State")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SourceArea).HasColumnName("Source_Area");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.RideClient)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Ride_User");

                entity.HasOne(d => d.DistinationAreaNavigation)
                    .WithMany(p => p.RideDistinationAreaNavigation)
                    .HasForeignKey(d => d.DistinationArea)
                    .HasConstraintName("FK_Ride_Area1");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.RideDriver)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_Ride_User1");

                entity.HasOne(d => d.SourceAreaNavigation)
                    .WithMany(p => p.RideSourceAreaNavigation)
                    .HasForeignKey(d => d.SourceArea)
                    .HasConstraintName("FK_Ride_Area");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BirthDate)
                    .HasColumnName("Birth_date")
                    .HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.LicenceId)
                    .HasColumnName("LicenceID")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NationalId)
                    .HasColumnName("NationalID")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
