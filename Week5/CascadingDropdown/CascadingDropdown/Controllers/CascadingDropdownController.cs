using CascadingDropdown.Data;
using CascadingDropdown.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CascadingDropdown.Controllers
{
    public class CascadingDropdownController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CascadingDropdownController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var countries = await _context.Countries.ToListAsync();

            var city = new List<City>();

            var viewModel = new CascadingDropdownViewModel
            {
                Countries = countries,
                Cities = city
            };

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult GetCitiesByCountry(int countryId)
        {
            var cities = _context.Cities.Where(c=>c.CountryId == countryId).ToList();
            return Json(cities);

        }
    }
}
