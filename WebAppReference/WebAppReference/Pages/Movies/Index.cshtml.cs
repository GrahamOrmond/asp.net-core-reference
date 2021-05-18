using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebAppReference.Models;

namespace WebAppReference.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly WebAppReference.Data.WebAppReferenceContext _context;

        public IndexModel(WebAppReference.Data.WebAppReferenceContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }

        public async Task OnGetAsync()
        {
            Movie = await _context.Movie.ToListAsync();
        }
    }
}
