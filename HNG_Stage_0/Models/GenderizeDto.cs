namespace HNG_Stage_0.Models
{
    public class GenderizeDto
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public double Probability { get; set; }
  
        public int Count { get; set; }

    }
    public class GenderizeResponseDto
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public double Probability { get; set; }
        public int Sample_size { get; set; }
        public bool Is_confident { get; set; }
        public string Processed_at { get; set; }

    }
}