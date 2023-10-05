using CascadingDropdown.Data;

namespace CascadingDropdown.Models
{
    public class CascadingDropdownViewModel
    {
        public List<Country> Countries { get; set; }

        public List<City> Cities { get; set; }

        public int SelectedCountryId { get; set; }

    }
}
