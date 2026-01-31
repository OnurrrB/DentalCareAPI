
namespace DentalCareAPI.DTOs
{
    public class UserCreateDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public bool IsSmoker { get; set; }
        public bool IsAlcoholUser { get; set; }
        public bool HasOrthodontic { get; set; }
        public bool IsBruxizim { get; set; }
    }
}

