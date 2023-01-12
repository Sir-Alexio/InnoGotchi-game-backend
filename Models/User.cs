namespace InnoGotchi_backend.Models
{
    public class User
    {
        public int UserId{ get; set; }
        public string FirstName{ get; set; }
        public string LastName{ get; set; }
        public string Email{ get; set; }
        public int FarmId{ get; set; }
        public List<User> Colaborators { get; set; }
        public List<Farm> FarmsColaborators { get; set; }
    }
}
