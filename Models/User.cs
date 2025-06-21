using System.ComponentModel.DataAnnotations;

namespace CameraView.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // thêm thuộc tính Role
    }
}
