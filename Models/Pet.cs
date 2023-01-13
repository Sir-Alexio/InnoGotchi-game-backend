namespace InnoGotchi_backend.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public string PetName { get; set; }
        public int Age { get; set; }
        public string HungerLevel { get; set; }
        public string ThirstyLevel { get; set; }
        public int HappyDaysCount { get; set; }
       
        public Pet(string petName)
        {
            this.PetName = petName;
            Age = 5;
            HungerLevel = "FULL";
            ThirstyLevel = "NORMAL";
            HappyDaysCount = 1;
        }
       
    }
}
