namespace Kruggers_Backend.Data
{
    public class CreatorDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string Name
        {
            private get;
            set;
        }

        public string LastName
        {
            private get;
            set;
        }

        public string FullName => $"{Name} {LastName}";
    }
}
