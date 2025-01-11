using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerLibrary.Constant;

namespace ServerLibrary.Data
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            foreach (var Item in Constant.Constant.RoleName)
            {
                builder.HasData(new IdentityRole
                {
                    Name = Item.Value.Name,
                    NormalizedName = Item.Value.NormalizedName
                });
            }

        }
    }
}
