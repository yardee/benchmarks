using BTDB.ODBLayer;

namespace SplitBrainPrimaryKey.Db
{
    public class CompanyWorkIndicatorDb
    {
        /// <summary>
        /// Make sure that u will put there unique value
        /// </summary>
        [PrimaryKey(1)]
        public string Feature { get; set; } = null!;
        [PrimaryKey(2)]
        public ulong CompanyId { get; set; }

        public CompanyWorkIndicatorFlag Flag { get; set; }
    }
}
