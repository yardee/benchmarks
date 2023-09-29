using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;
using SplitBrainPrimaryKey.Db;

namespace SplitBrainPrimaryKey
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Directory.CreateDirectory("db");
            var keyValueDb = new KeyValueDB(new OnDiskFileCollection("db"));
            var db = new ObjectDB();

            var options = new DBOptions()
                .WithSelfHealing();

            db.Open(keyValueDb, true, options);


            var creator = InitRelation(db);

            var tasks1 = RunRead(db, creator);
            
            foreach (var i in Enumerable.Range(1, 100))
            {
                SetDirty(db, creator, (ulong)i * 100);
            }

            var t= Task.Run(() =>
            {
                Task.Yield();
                foreach (var i in Enumerable.Range(1, 10))
                {
                    GetAndSetDirtyCompanyIds(db, creator);
                    Task.Yield();
                }
            });
            
            var tasks = RunRead(db, creator);
            

            var allTasks = tasks.ToList();
            allTasks.Add(t);
            allTasks.AddRange(tasks1);
            
            await Task.WhenAll(allTasks);
            //
            // foreach (var i in Enumerable.Range(1, 100))
            // {
            //     SetDirty(db, creator, (ulong)i * 100);
            // }

            // CheckIntegrity(db, creator);
            

            // GetDirtyCompanyIds(db, creator);
            Cleaup(db, creator);
        }

        static Func<IObjectDBTransaction, ICompanyWorkIndicatorTable> InitRelation(ObjectDB db)
        {
            using var tr = db.StartTransaction();
            var creator = tr.InitRelation<ICompanyWorkIndicatorTable>("CompanyWorkIndicatorTable");
            tr.Commit();
            return creator;
        }
        
        static void SetDirty(ObjectDB db, Func<IObjectDBTransaction, ICompanyWorkIndicatorTable> creator, ulong i)
        {
            using var tr = db.StartTransaction();
            foreach (var j in Enumerable.Range(0, 5))
            {
                CompanyWorkIndicator.SetCompanyDirty(CompanyWorkIndicatorFeature.FastProcessChangeInputs, 1 + i,
                    () => creator(tr));

                CompanyWorkIndicator.SetCompanyDirty(CompanyWorkIndicatorFeature.ProcessChangeInputs, 3 + i, () => creator(tr));
                CompanyWorkIndicator.SetCompanyDirty(CompanyWorkIndicatorFeature.ProcessChangeInputs, 2 + i, () => creator(tr));

                CompanyWorkIndicator.SetCompanyDirty(CompanyWorkIndicatorFeature.FastProcessChangeInputs, 3 + i, () => creator(tr));

            }
            
            tr.Commit();
        }

        static void CheckIntegrity(ObjectDB db, Func<IObjectDBTransaction, ICompanyWorkIndicatorTable> creator)
        {
            using var tr = db.StartTransaction();

            var companyWorkIndicatorTable = creator(tr);

            foreach (var companyWorkIndicatorDb in companyWorkIndicatorTable.ListById(CompanyWorkIndicatorFeature.FastProcessChangeInputs))
            {
                var b = companyWorkIndicatorTable.ToList();
                var c = companyWorkIndicatorTable.ToList();
            }
        }
        
        static IEnumerable<Task> RunRead(ObjectDB db, Func<IObjectDBTransaction, ICompanyWorkIndicatorTable> creator)
        {
            foreach (var i in Enumerable.Range(0, 2))
            {
                Task.Yield();
                yield return Task.Run(() =>
                {
                    foreach (var j in Enumerable.Range(0, 10))
                    {
                        using var tr = db.StartReadOnlyTransaction();

                        var companyWorkIndicatorTable = creator(tr);
                        foreach (var companyWorkIndicatorDb in companyWorkIndicatorTable.ListById(CompanyWorkIndicatorFeature.FastProcessChangeInputs))
                        {
                            Task.Yield();
                            Console.WriteLine($"RunRead: {companyWorkIndicatorDb}");
                        }
                    }
                });
            }
        }
        
        static void Finish(ObjectDB db, Func<IObjectDBTransaction, ICompanyWorkIndicatorTable> creator, ulong i)
        {
            using var tr = db.StartTransaction();

            CompanyWorkIndicator.TryFinishCommunicationProcessing(CompanyWorkIndicatorFeature.FastProcessChangeInputs, 1 + i,
                () => creator(tr));

            CompanyWorkIndicator.TryFinishCommunicationProcessing(CompanyWorkIndicatorFeature.ProcessChangeInputs, 3 + i, () => creator(tr));

            tr.Commit();
        }

        static void GetAndSetDirtyCompanyIds(ObjectDB db, Func<IObjectDBTransaction, ICompanyWorkIndicatorTable> creator)
        {
            Console.WriteLine("------------- GetAndSetDirtyCompanyIds ---------------");
            using var tr = db.StartTransaction();

            var dirty = CompanyWorkIndicator.GetDirtyCompanyIdsAndSetActionQueued(
                CompanyWorkIndicatorFeature.ProcessChangeInputs, () => creator(tr));
            
            var dirty2 = CompanyWorkIndicator.GetDirtyCompanyIdsAndSetActionQueued(
                CompanyWorkIndicatorFeature.FastProcessChangeInputs, () => creator(tr));
            
            Console.WriteLine($"Dirty ProcessChangeInputs: {dirty.Count()}, {dirty2.Count()}");
            
            tr.Commit();
        }

        static void Cleaup(ObjectDB db, Func<IObjectDBTransaction, ICompanyWorkIndicatorTable> creator)
        {
            Console.WriteLine("------------- Clean ---------------");
            var tr = db.StartTransaction();

            var companyWorkIndicatorTable = creator(tr);
            foreach (var item in companyWorkIndicatorTable.ToList())
            {
                companyWorkIndicatorTable.RemoveById(item.Feature, item.CompanyId);
            }

            tr.Commit();
            tr.Dispose();
            db.Dispose();
        }
    }
}