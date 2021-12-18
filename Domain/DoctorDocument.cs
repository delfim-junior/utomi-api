namespace Domain
{
    public class DoctorDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Doctor Doctor { get; set; }
    }
}