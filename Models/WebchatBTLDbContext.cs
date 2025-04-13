using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace webchatBTL.Models
{
    public partial class WebchatBTLDbContext : DbContext
    {
        public WebchatBTLDbContext()
        {
        }

        public WebchatBTLDbContext(DbContextOptions<WebchatBTLDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanySetting> CompanySettings { get; set; }
        public virtual DbSet<CompanySubscription> CompanySubscriptions { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupInCompany> GroupInCompanies { get; set; }
        public virtual DbSet<GroupMember> GroupMembers { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSession> UserSessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-NIETNG6; Initial Catalog=BTL-WEBCHAT; User ID=sa;Password=123456;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<CompanySetting>(entity =>
            {
                entity.HasKey(e => e.CompanyId)
                    .HasName("PK__CompanyS__2D971CAC674DEA60");

                entity.Property(e => e.CompanyId).ValueGeneratedNever();

                entity.HasOne(d => d.Company)
                    .WithOne(p => p.CompanySetting)
                    .HasForeignKey<CompanySetting>(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CompanySe__Compa__45F365D3");
            });

            modelBuilder.Entity<CompanySubscription>(entity =>
            {
                entity.HasKey(e => e.Id) // Sửa tại đây: dùng Id là khóa chính
                    .HasName("PK_CompanySubscription");

                entity.Property(e => e.StartDate).HasColumnType("date");
                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanySubscriptions)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanySubscription_Company");

                entity.HasOne(d => d.Plan)
                    .WithMany(p => p.CompanySubscriptions)
                    .HasForeignKey(d => d.PlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanySubscription_SubscriptionPlan");
            });


            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Events__GroupId__44CA3770");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Events__UserId__43D61337");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.FileType).IsUnicode(false);

                entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK__Files__MessageId__17F790F9");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Files__ProjectId__17036CC0");

                entity.HasOne(d => d.UploadedByNavigation)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.UploadedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Files__UploadedB__151B244E");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Groups__CreatedB__0C85DE4D");
            });

            modelBuilder.Entity<GroupInCompany>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK__GroupInC__149AF36A886E463B");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.GroupInCompanies)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK__GroupInCo__Manag__40F9A68C");
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.UserId })
                    .HasName("PK__GroupMem__C5E27FAE4B4736D3");

                entity.Property(e => e.JoinedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupMembers)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupMemb__Group__10566F31");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupMemb__UserI__114A936A");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Invoices__Compan__4E88ABD4");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.MessageType).IsUnicode(false);

                entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessageReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .HasConstraintName("FK__Messages__Receiv__07C12930");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSenders)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Messages__Sender__06CD04F7");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__UserI__1AD3FDA4");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.Property(e => e.IsConfirmed).HasDefaultValueSql("((0))");

                entity.Property(e => e.JoinedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__Participa__Event__55F4C372");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__UserI__56E8E7AB");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Projects__Compan__7A672E12");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Projects__Create__7B5B524B");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.RoleName).IsUnicode(false);
            });

            modelBuilder.Entity<SubscriptionPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId)
                    .HasName("PK__Subscrip__755C22B72F8DDC56");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.AssignedTo)
                    .HasConstraintName("FK__Tasks__AssignedT__01142BA1");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tasks__ProjectId__00200768");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__Users__CompanyId__71D1E811");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_Users_GroupInCompany");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__RoleId__72C60C4A");
            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("PK__UserSess__C9F49290BE3DC733");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LoginTime).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSessions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserSessi__UserI__1F98B2C1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
