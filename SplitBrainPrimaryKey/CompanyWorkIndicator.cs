using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BTDB.ODBLayer;
using SplitBrainPrimaryKey.Db;

namespace SplitBrainPrimaryKey
{
    public static class CompanyWorkIndicator
    {
        public static void SetCompanyDirty(string feature, ulong companyId, Func<ICompanyWorkIndicatorTable> companyWorkIndicatorTableFactory)
        {
            var companyWorkIndicatorTable = companyWorkIndicatorTableFactory();
            var companyWorkIndicator = companyWorkIndicatorTable.FindByIdOrDefault(feature, companyId);
            if (companyWorkIndicator == null)
            {
                companyWorkIndicator = new Db.CompanyWorkIndicatorDb { Feature = feature, CompanyId = companyId, Flag = CompanyWorkIndicatorFlag.Dirty };
                companyWorkIndicatorTable.Upsert(companyWorkIndicator);
                return;
            }

            if (companyWorkIndicator.Flag.HasFlag(CompanyWorkIndicatorFlag.Dirty))
            {
                return;
            }

            companyWorkIndicator.Flag |= CompanyWorkIndicatorFlag.Dirty;
            companyWorkIndicatorTable.Upsert(companyWorkIndicator);
        }

        /*
        public static bool HasAnyDirtyCompany(string feature, ISchedulerContext context)
        {
            using var queryContext = context.Continent.CreateQueryContext();
            var companyWorkIndicatorTable = queryContext.Transaction.GetRelation<ICompanyWorkIndicatorTable>();
            foreach (var companyWorkIndicator in companyWorkIndicatorTable.ListById(feature))
            {
                if (companyWorkIndicator.Flag.HasFlag(CompanyWorkIndicatorFlag.Dirty))
                {
                    return true;
                }
            }

            return false;
        }
        */

        // In case of split-brain investigation - could be related with OPL-2021
        public static IEnumerable<ulong> GetDirtyCompanyIdsAndSetActionQueued(string feature, Func<ICompanyWorkIndicatorTable> companyWorkIndicatorTableFactory)
        {
            var companyWorkIndicatorTable = companyWorkIndicatorTableFactory();

            // Fixing potential problem with writing and enumerating relation in the same time (especially bad when it is called multiple times before enumeration)
            var dirtyCompanyWorkIndicators = GetCompanyWorkIndicatorWithDataCheck();
            
            foreach (var companyWorkIndicator in dirtyCompanyWorkIndicators)
            {
                companyWorkIndicator.Flag = CompanyWorkIndicatorFlag.ActionQueued;
                companyWorkIndicatorTable.Upsert(companyWorkIndicator);
            }

            return dirtyCompanyWorkIndicators.Select(indicator => indicator.CompanyId);

            IList<CompanyWorkIndicatorDb> GetCompanyWorkIndicatorWithDataCheck()
            {
                // Obtain data from enumeration with split-brain condition check and 1000 retries (OPL-2021)

                var dirtyWorkIndicators = new List<CompanyWorkIndicatorDb>();    
                    
                foreach (var workIndicator in companyWorkIndicatorTable.ListById(feature))
                {
                    // Checking split-brain condition (OPL-2021), incorrect feature enumeration
                    if (workIndicator.Feature != feature)
                    {
                        throw new Exception($"workIndicator.Feature, workIndicator.Feature:{workIndicator.Feature}, feature:{feature}");
                    }
                    //
                    // if((workIndicator.Flag & CompanyWorkIndicatorFlag.Dirty) == 0)
                    //     continue;
                        
                    dirtyWorkIndicators.Add(workIndicator);
                }
                    
                return dirtyWorkIndicators;
            }
        }


        public static bool IsCompanyDirty(string feature, ulong companyId, Func<ICompanyWorkIndicatorTable> companyWorkIndicatorTableFactory)
        {
            var companyWorkIndicatorTable = companyWorkIndicatorTableFactory();
            var companyWorkIndicator = companyWorkIndicatorTable.FindByIdOrDefault(feature, companyId);
            if (companyWorkIndicator == null)
            {
                return false;
            }

            if (companyWorkIndicator.Flag.HasFlag(CompanyWorkIndicatorFlag.Dirty))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="companyId"></param>
        /// <param name="context"></param>
        /// <returns>is there some undone work</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static bool TryFinishCommunicationProcessing(string feature, ulong companyId, Func<ICompanyWorkIndicatorTable> companyWorkIndicatorTableFactory)
        {
            var companyWorkIndicatorTable = companyWorkIndicatorTableFactory();
            var companyWorkIndicator = companyWorkIndicatorTable.FindByIdOrDefault(feature, companyId);
            if (companyWorkIndicator == null)
            {
                Console.WriteLine($"Feature '{feature}' on company '{companyId}' finished without previous start. " +
                                  $"Action run is not optimal.");
                return false;
            }

            if (!companyWorkIndicator.Flag.HasFlag(CompanyWorkIndicatorFlag.ActionQueued))
            {
                Console.WriteLine($"Feature '{feature}' on company '{companyId}' finished without flag set to " +
                                  $"'{nameof(CompanyWorkIndicatorFlag.ActionQueued)}'. Set flag is '{companyWorkIndicator.Flag}'." +
                                  $"Action run is not optimal.");
                return false;
            }

            if (companyWorkIndicator.Flag.HasFlag(CompanyWorkIndicatorFlag.Dirty))
            {
                companyWorkIndicator.Flag &= ~CompanyWorkIndicatorFlag.ActionQueued;
                companyWorkIndicatorTable.Upsert(companyWorkIndicator);
                return true;
            }

            companyWorkIndicatorTable.RemoveById(feature, companyId);
            return false;
        }
    }
}
