namespace Restaurant2API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Emali { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }    
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public string? PassworaHash  { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

    }
}
