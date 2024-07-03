namespace EmployeesManagment.Models
{
    public class City : UserActivity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
