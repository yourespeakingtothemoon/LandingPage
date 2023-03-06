using System.ComponentModel.DataAnnotations;

namespace LandingPage.Models
{
    public class Message
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string fromUserID { get; set; }
        [Required]
        public string toUserID { get; set; }
        [Required]
        public string m_Message { get; set; }
        [Required]
        public bool isPublic { get; set; } = true;

        
        public Message(string message, string userId, string toUserId, bool pub)
        {
            m_Message = message;
            fromUserID = userId;
            toUserID = toUserId;
			isPublic = pub;
		}
      
        public Message()
        { }
    }
}
