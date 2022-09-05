using System.Threading.Tasks;
using Contracts.Repository;
using Domain.Entities;
using Infrastructure.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class OtpCodeRepository : RepositoryBase<OtpCode>, IOtpCodeRepository
    {
        public OtpCodeRepository(PgSqlDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateOtpCode(OtpCode otpCode)
        {
            Create(otpCode);
        }

        public async Task CreateOtpCodeAsync(OtpCode otpCode)
        {
            await CreateAsync(otpCode);
        }

        public async Task<OtpCode> GetOtpCodeByValue(string value, bool trackChanges)
        {
            return await FindByCondition(code => code.Value.Equals(value), trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}