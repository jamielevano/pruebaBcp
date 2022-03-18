using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTipoCambio.Model
{
    public class TipoCambioDbContext : DbContext
    {
     
        public TipoCambioDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TipoCambio> TiposCambios { get; set; }
        public DbSet<OperacionTC> OperacionesTC { get; set; }


    }
}
