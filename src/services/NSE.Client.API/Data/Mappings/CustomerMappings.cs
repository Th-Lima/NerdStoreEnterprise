using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Client.API.Models;
using NSE.Core.DomainObjects;

namespace NSE.Client.API.Data.Mappings
{
    public class CustomerMappings : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.OwnsOne(c => c.Cpf, tf =>
            {
                tf.Property(c => c.Number)
                    .IsRequired()
                    .HasMaxLength(CPF.CPFMaxLength)
                    .HasColumnName("Cpf")
                    .HasColumnType($"varchar({CPF.CPFMaxLength})");
            });

            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.AddressEmail)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType($"varchar({Email.EmailAddressMaxLength})");
            });

            // 1 : 1 => Aluno : Endereco
            builder.HasOne(c => c.Address)
                .WithOne(c => c.Customer);

            builder.ToTable("Customers");
        }
    }
}
