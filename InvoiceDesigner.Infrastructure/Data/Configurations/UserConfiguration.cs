using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.Models.Directories; // Для Company
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic; // Для Dictionary

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user.HasKey(e => e.Id);

            user.HasIndex(e => e.Login).IsUnique();

            user.Property(e => e.Login)
                .IsRequired()
                .HasMaxLength(100);

            user.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            user.Property(e => e.PasswordHash)
                .IsRequired();

            user.Property(e => e.PasswordSalt)
                .IsRequired();

            user.HasMany(e => e.Companies)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UserCompany",
                    uc => uc.HasOne<Company>().WithMany().HasForeignKey("CompanyId").OnDelete(DeleteBehavior.Cascade),
                    uc => uc.HasOne<User>().WithMany().HasForeignKey("ActivityUserId").OnDelete(DeleteBehavior.Cascade)
                );

            // default login: admin, password: admin
            user.HasData(
                    new User
                    {
                        Id = 1,
                        Login = "admin",
                        Name = "Super Admin",
                        PasswordHash = "1708D30988E562DD2958B50B77F0D61C47C59FD7555F3B91AB02D486F361504F7E0C569157D104D99E5076BFF20AF9EE38482A63BA10993B28C38F9936668010",
                        PasswordSalt = "7A6604F49A4E8EFCBB8B6CA86305FB0E4E14F817AE6D8726DE7A56463581A1D21D68699970298BBFE2182AE02366BCBB56C14DF47B9D000AF0C5D74DCED88953",
                        IsAdmin = true
                    }
                );
        }
    }
}
