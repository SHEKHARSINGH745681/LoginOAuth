namespace CountryJson.Models
{
    public class State
    { 
            public int Id { get; set; }
            public string StateName { get; set; }
            public List<District> Districts { get; set; }
        }

    }
