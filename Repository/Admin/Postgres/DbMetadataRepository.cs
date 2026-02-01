
namespace Repository.Admin.Postgres
{
    public class DbMetadataRepository : IDbMetadataRepository
    {
        public Task<bool> CreateDb(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDb(string name)
        {
            throw new NotImplementedException();
        }
    }
}
