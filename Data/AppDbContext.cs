using Microsoft.EntityFrameworkCore;
using CameraView.Models;
using Microsoft.SqlServer.Server;
namespace CameraView.Data

{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<DeviceRecord> DeviceRecords { get; set; }
    }
}
