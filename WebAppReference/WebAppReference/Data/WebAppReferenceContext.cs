using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAppReference.Models;

namespace WebAppReference.Data
{
    public class WebAppReferenceContext : DbContext
    {
        public WebAppReferenceContext (DbContextOptions<WebAppReferenceContext> options)
            : base(options)
        {
        }

        public DbSet<WebAppReference.Models.Movie> Movie { get; set; }
    }
}
