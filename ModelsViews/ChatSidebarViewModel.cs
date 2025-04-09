using webchatBTL.Models;
using System.Collections.Generic;

namespace webchatBTL.ModelsViews
{
    public class ChatSidebarViewModel
    {
        public IEnumerable<RecentChatModel> RecentChats { get; set; }
        public IEnumerable<GroupModel> Groups { get; set; } // Thêm danh sách groups
    }

    public class GroupModel
    {
        public string GroupName { get; set; }
        public int UnreadCount { get; set; }
    }

    public class RecentChatModel
    {
        public User User { get; set; }
        public Message LastMessage { get; set; }
        public int UnreadCount { get; set; }
    }
}
