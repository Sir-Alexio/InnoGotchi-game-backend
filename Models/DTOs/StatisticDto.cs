namespace InnoGotchi_backend.Models.DTOs
{
    public class StatisticDto
    {
        public int AlivePetCount { get; set; }
        public int DeadPetCount { get; set; }
        public int AverageFeedingPeriod { get; set; }
        public int AverageDrinkingPeriod { get; set; }
        public int AverageHappyDaysCount { get; set; }
        public int AveragePetsAge { get; set; }
    }
}
