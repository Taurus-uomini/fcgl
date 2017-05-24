using System.Data.Entity;

namespace fcgl.Models
{
    public class DBModels:DbContext
    {
        public DBModels() : base("name=mydbEntities")
        {
            
        }
        public DbSet<AdminModels> Admin { get; set; }
        public DbSet<UserModels> User { get; set; }

        public System.Data.Entity.DbSet<fcgl.Models.HousePropertyModels> HousePropertyModels { get; set; }
        public DbSet<ProvincesModels> Provinces { get; set; }
        public DbSet<CitiesModels> Cities { get; set; }
        public DbSet<AreasModels> Areas { get; set; }
        public DbSet<OrderModels> Order { get; set; }
        public DbSet<InfoModels> Info { get; set; }
        public DbSet<PropertyImageModels> propertyimg { get; set; }
    }
}