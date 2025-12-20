namespace FlowerBot.src.Data.Models.Database
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // TODO: add more settings
    }
}
