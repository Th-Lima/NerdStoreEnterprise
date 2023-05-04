using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Payment.API.Models;

namespace NSE.Payment.API.Data.Mappings
{
    public class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(c => c.Id);

            // 1 : N => Pagamento : Transacao
            builder.HasOne(c => c.Payment)
                .WithMany(c => c.Transactions);

            builder.ToTable("Transactions");
        }
    }
}
