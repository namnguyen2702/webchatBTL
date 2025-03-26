using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using webchatBTL.Models;

#nullable disable

namespace webchatBTL.Models
{
    public partial class Message
    {
        public Message()
        {
            Files = new HashSet<File>();
        }

        [Key]
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public int? GroupId { get; set; }
        [StringLength(50)]
        public string MessageType { get; set; }
        [Required]
        public string Content { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        [InverseProperty(nameof(User.MessageReceivers))]
        public virtual User Receiver { get; set; }
        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(User.MessageSenders))]
        public virtual User Sender { get; set; }
        [InverseProperty(nameof(File.Message))]
        public virtual ICollection<File> Files { get; set; }
    }
}
