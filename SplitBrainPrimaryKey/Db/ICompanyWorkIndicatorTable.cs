using System.Collections.Generic;
using BTDB.ODBLayer;

namespace SplitBrainPrimaryKey.Db
{
    public interface ICompanyWorkIndicatorTable : IRelation<CompanyWorkIndicatorDb>
    {
        CompanyWorkIndicatorDb FindByIdOrDefault(string feature, ulong companyId);
        bool RemoveById(string feature, ulong companyId);
        IEnumerable<CompanyWorkIndicatorDb> ListById(string feature);
    }
}
