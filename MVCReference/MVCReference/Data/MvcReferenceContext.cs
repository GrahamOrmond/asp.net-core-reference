using Microsoft.EntityFrameworkCore;
using MVCReference.Models;

namespace MVCReference.Data
{
    public class MvcReferenceContext : DbContext
    {
        public MvcReferenceContext(DbContextOptions<MvcReferenceContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
    }
}
