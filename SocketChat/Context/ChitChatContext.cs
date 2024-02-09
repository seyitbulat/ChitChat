using Microsoft.EntityFrameworkCore;

public class ChitChatContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Hub> Hubs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer("Data Source=DESKTOP-R04PVQ3\\SQLEXPRESS01; Initial Catalog=ChitChat; Integrated Security=true; TrustServerCertificate=True");

        int[] ses = new int[] { 1, 2, 3 };  

        

    }
}
