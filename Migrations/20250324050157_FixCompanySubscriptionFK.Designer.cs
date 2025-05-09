﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using webchatBTL.Models;

#nullable disable

namespace webchatBTL.Migrations
{
    [DbContext(typeof(WebchatBTLDbContext))]
    [Migration("20250324050157_FixCompanySubscriptionFK")]
    partial class FixCompanySubscriptionFK
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("webchatBTL.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("webchatBTL.Models.CompanySetting", b =>
                {
                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Logo")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Theme")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CompanyId")
                        .HasName("PK__CompanyS__2D971CAC674DEA60");

                    b.ToTable("CompanySettings");
                });

            modelBuilder.Entity("webchatBTL.Models.CompanySubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<int>("PlanId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id")
                        .HasName("PK_CompanySubscription");

                    b.HasIndex("CompanyId");

                    b.HasIndex("PlanId");

                    b.ToTable("CompanySubscriptions");
                });

            modelBuilder.Entity("webchatBTL.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"));

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<DateTime?>("End")
                        .HasColumnType("datetime");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFullDay")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ThemeColor")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("EventId");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("webchatBTL.Models.File", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FileId"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FileType")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("MessageId")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UploadedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("UploadedBy")
                        .HasColumnType("int");

                    b.HasKey("FileId");

                    b.HasIndex("MessageId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UploadedBy");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("webchatBTL.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("GroupId");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("webchatBTL.Models.GroupInCompany", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("ManagerId")
                        .HasColumnType("int");

                    b.HasKey("GroupId")
                        .HasName("PK__GroupInC__149AF36A886E463B");

                    b.HasIndex("ManagerId");

                    b.ToTable("GroupInCompany");
                });

            modelBuilder.Entity("webchatBTL.Models.GroupMember", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("JoinedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("GroupId", "UserId")
                        .HasName("PK__GroupMem__C5E27FAE4B4736D3");

                    b.HasIndex("UserId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("webchatBTL.Models.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoiceId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("InvoiceId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("webchatBTL.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("MessageType")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("MessageId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("webchatBTL.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("webchatBTL.Models.Participant", b =>
                {
                    b.Property<int>("ParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParticipantId"));

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((0))");

                    b.Property<DateTime?>("JoinedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ParticipantId");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("webchatBTL.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ProjectId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("webchatBTL.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("webchatBTL.Models.SubscriptionPlan", b =>
                {
                    b.Property<int>("PlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlanId"));

                    b.Property<int?>("MaxStorage")
                        .HasColumnType("int");

                    b.Property<int?>("MaxUsers")
                        .HasColumnType("int");

                    b.Property<string>("PlanName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("PlanId")
                        .HasName("PK__Subscrip__755C22B72F8DDC56");

                    b.ToTable("SubscriptionPlans");
                });

            modelBuilder.Entity("webchatBTL.Models.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskId"));

                    b.Property<int?>("AssignedTo")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("date");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TaskId");

                    b.HasIndex("AssignedTo");

                    b.HasIndex("ProjectId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("webchatBTL.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Phone")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("GroupId");

                    b.HasIndex("RoleId");

                    b.HasIndex(new[] { "Email" }, "UQ__Users__A9D10534058B5E5C")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("webchatBTL.Models.UserSession", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SessionId"));

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<DateTime>("LoginTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("LogoutTime")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("SessionId")
                        .HasName("PK__UserSess__C9F49290BE3DC733");

                    b.HasIndex("UserId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("webchatBTL.Models.CompanySetting", b =>
                {
                    b.HasOne("webchatBTL.Models.Company", "Company")
                        .WithOne("CompanySetting")
                        .HasForeignKey("webchatBTL.Models.CompanySetting", "CompanyId")
                        .IsRequired()
                        .HasConstraintName("FK__CompanySe__Compa__45F365D3");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("webchatBTL.Models.CompanySubscription", b =>
                {
                    b.HasOne("webchatBTL.Models.Company", "Company")
                        .WithMany("CompanySubscriptions")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("FK_CompanySubscription_Company");

                    b.HasOne("webchatBTL.Models.SubscriptionPlan", "Plan")
                        .WithMany("CompanySubscriptions")
                        .HasForeignKey("PlanId")
                        .IsRequired()
                        .HasConstraintName("FK_CompanySubscription_SubscriptionPlan");

                    b.Navigation("Company");

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("webchatBTL.Models.Event", b =>
                {
                    b.HasOne("webchatBTL.Models.GroupInCompany", "Group")
                        .WithMany("Events")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__Events__GroupId__44CA3770");

                    b.HasOne("webchatBTL.Models.User", "User")
                        .WithMany("Events")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Events__UserId__43D61337");

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("webchatBTL.Models.File", b =>
                {
                    b.HasOne("webchatBTL.Models.Message", "Message")
                        .WithMany("Files")
                        .HasForeignKey("MessageId")
                        .HasConstraintName("FK__Files__MessageId__17F790F9");

                    b.HasOne("webchatBTL.Models.Project", "Project")
                        .WithMany("Files")
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("FK__Files__ProjectId__17036CC0");

                    b.HasOne("webchatBTL.Models.User", "UploadedByNavigation")
                        .WithMany("Files")
                        .HasForeignKey("UploadedBy")
                        .IsRequired()
                        .HasConstraintName("FK__Files__UploadedB__151B244E");

                    b.Navigation("Message");

                    b.Navigation("Project");

                    b.Navigation("UploadedByNavigation");
                });

            modelBuilder.Entity("webchatBTL.Models.Group", b =>
                {
                    b.HasOne("webchatBTL.Models.User", "CreatedByNavigation")
                        .WithMany("Groups")
                        .HasForeignKey("CreatedBy")
                        .IsRequired()
                        .HasConstraintName("FK__Groups__CreatedB__0C85DE4D");

                    b.Navigation("CreatedByNavigation");
                });

            modelBuilder.Entity("webchatBTL.Models.GroupInCompany", b =>
                {
                    b.HasOne("webchatBTL.Models.User", "Manager")
                        .WithMany("GroupInCompanies")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__GroupInCo__Manag__40F9A68C");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("webchatBTL.Models.GroupMember", b =>
                {
                    b.HasOne("webchatBTL.Models.Group", "Group")
                        .WithMany("GroupMembers")
                        .HasForeignKey("GroupId")
                        .IsRequired()
                        .HasConstraintName("FK__GroupMemb__Group__10566F31");

                    b.HasOne("webchatBTL.Models.User", "User")
                        .WithMany("GroupMembers")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__GroupMemb__UserI__114A936A");

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("webchatBTL.Models.Invoice", b =>
                {
                    b.HasOne("webchatBTL.Models.Company", "Company")
                        .WithMany("Invoices")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("FK__Invoices__Compan__4E88ABD4");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("webchatBTL.Models.Message", b =>
                {
                    b.HasOne("webchatBTL.Models.User", "Receiver")
                        .WithMany("MessageReceivers")
                        .HasForeignKey("ReceiverId")
                        .HasConstraintName("FK__Messages__Receiv__07C12930");

                    b.HasOne("webchatBTL.Models.User", "Sender")
                        .WithMany("MessageSenders")
                        .HasForeignKey("SenderId")
                        .IsRequired()
                        .HasConstraintName("FK__Messages__Sender__06CD04F7");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("webchatBTL.Models.Notification", b =>
                {
                    b.HasOne("webchatBTL.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Notificat__UserI__1AD3FDA4");

                    b.Navigation("User");
                });

            modelBuilder.Entity("webchatBTL.Models.Participant", b =>
                {
                    b.HasOne("webchatBTL.Models.Event", "Event")
                        .WithMany("Participants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Participa__Event__55F4C372");

                    b.HasOne("webchatBTL.Models.User", "User")
                        .WithMany("Participants")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Participa__UserI__56E8E7AB");

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("webchatBTL.Models.Project", b =>
                {
                    b.HasOne("webchatBTL.Models.Company", "Company")
                        .WithMany("Projects")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("FK__Projects__Compan__7A672E12");

                    b.HasOne("webchatBTL.Models.User", "CreatedByNavigation")
                        .WithMany("Projects")
                        .HasForeignKey("CreatedBy")
                        .IsRequired()
                        .HasConstraintName("FK__Projects__Create__7B5B524B");

                    b.Navigation("Company");

                    b.Navigation("CreatedByNavigation");
                });

            modelBuilder.Entity("webchatBTL.Models.Task", b =>
                {
                    b.HasOne("webchatBTL.Models.User", "AssignedToNavigation")
                        .WithMany("Tasks")
                        .HasForeignKey("AssignedTo")
                        .HasConstraintName("FK__Tasks__AssignedT__01142BA1");

                    b.HasOne("webchatBTL.Models.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .IsRequired()
                        .HasConstraintName("FK__Tasks__ProjectId__00200768");

                    b.Navigation("AssignedToNavigation");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("webchatBTL.Models.User", b =>
                {
                    b.HasOne("webchatBTL.Models.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK__Users__CompanyId__71D1E811");

                    b.HasOne("webchatBTL.Models.GroupInCompany", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .HasConstraintName("FK_Users_GroupInCompany");

                    b.HasOne("webchatBTL.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__Users__RoleId__72C60C4A");

                    b.Navigation("Company");

                    b.Navigation("Group");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("webchatBTL.Models.UserSession", b =>
                {
                    b.HasOne("webchatBTL.Models.User", "User")
                        .WithMany("UserSessions")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__UserSessi__UserI__1F98B2C1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("webchatBTL.Models.Company", b =>
                {
                    b.Navigation("CompanySetting");

                    b.Navigation("CompanySubscriptions");

                    b.Navigation("Invoices");

                    b.Navigation("Projects");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("webchatBTL.Models.Event", b =>
                {
                    b.Navigation("Participants");
                });

            modelBuilder.Entity("webchatBTL.Models.Group", b =>
                {
                    b.Navigation("GroupMembers");
                });

            modelBuilder.Entity("webchatBTL.Models.GroupInCompany", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("webchatBTL.Models.Message", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("webchatBTL.Models.Project", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("webchatBTL.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("webchatBTL.Models.SubscriptionPlan", b =>
                {
                    b.Navigation("CompanySubscriptions");
                });

            modelBuilder.Entity("webchatBTL.Models.User", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Files");

                    b.Navigation("GroupInCompanies");

                    b.Navigation("GroupMembers");

                    b.Navigation("Groups");

                    b.Navigation("MessageReceivers");

                    b.Navigation("MessageSenders");

                    b.Navigation("Notifications");

                    b.Navigation("Participants");

                    b.Navigation("Projects");

                    b.Navigation("Tasks");

                    b.Navigation("UserSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
