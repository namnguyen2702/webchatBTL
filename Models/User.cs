using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BTL.Models
{
    [Index(nameof(Email), Name = "UQ__Users__A9D10534058B5E5C", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            Events = new HashSet<Event>();
            Files = new HashSet<File>();
            GroupInCompanies = new HashSet<GroupInCompany>();
            GroupMembers = new HashSet<GroupMember>();
            Groups = new HashSet<Group>();
            MessageReceivers = new HashSet<Message>();
            MessageSenders = new HashSet<Message>();
            Notifications = new HashSet<Notification>();
            Participants = new HashSet<Participant>();
            Projects = new HashSet<Project>();
            Tasks = new HashSet<Task>();
            UserSessions = new HashSet<UserSession>();
        }

        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }
        [StringLength(15)]
        public string Phone { get; set; }
        public int? CompanyId { get; set; }
        public int? RoleId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        public int? GroupId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Users")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(GroupId))]
        [InverseProperty(nameof(GroupInCompany.Users))]
        public virtual GroupInCompany Group { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("Users")]
        public virtual Role Role { get; set; }
        [InverseProperty(nameof(Event.User))]
        public virtual ICollection<Event> Events { get; set; }
        [InverseProperty(nameof(File.UploadedByNavigation))]
        public virtual ICollection<File> Files { get; set; }
        [InverseProperty(nameof(GroupInCompany.Manager))]
        public virtual ICollection<GroupInCompany> GroupInCompanies { get; set; }
        [InverseProperty(nameof(GroupMember.User))]
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<Group> Groups { get; set; }
        [InverseProperty(nameof(Message.Receiver))]
        public virtual ICollection<Message> MessageReceivers { get; set; }
        [InverseProperty(nameof(Message.Sender))]
        public virtual ICollection<Message> MessageSenders { get; set; }
        [InverseProperty(nameof(Notification.User))]
        public virtual ICollection<Notification> Notifications { get; set; }
        [InverseProperty(nameof(Participant.User))]
        public virtual ICollection<Participant> Participants { get; set; }
        [InverseProperty(nameof(Project.CreatedByNavigation))]
        public virtual ICollection<Project> Projects { get; set; }
        [InverseProperty(nameof(Task.AssignedToNavigation))]
        public virtual ICollection<Task> Tasks { get; set; }
        [InverseProperty(nameof(UserSession.User))]
        public virtual ICollection<UserSession> UserSessions { get; set; }
    }
}
