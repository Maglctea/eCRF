using System.Collections.Generic;
using System.Threading.Tasks;

using SQLite;
using eCRF.Interface;

namespace eCRF.Data
{
    public class DB <T> where T : ITable, new()
    {
        readonly SQLiteAsyncConnection db;

        public DB(string connectionString)
        {
            db = new SQLiteAsyncConnection(connectionString);
        }

        public SQLiteAsyncConnection getDB()
        {
            return db;
        }

        public Task<CreateTableResult> CreateTableAsync()
        {
            return db.CreateTableAsync<T>();
        }


        public Task<List<T>> GetItemsAsync()
        {
            return db.Table<T>().ToListAsync();
        }

        public Task<T> GetItemAsync(int id)
        {
            return db.Table<T>().Where(i => i.id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(T item)
        {
            if (item.id != 0)
            {
                return db.UpdateAsync(item);
            }
            else
            {
                return db.InsertAsync(item);
            }
        }
        public Task<int> InsertItemAsync(T item)
        {
            return db.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(T item)
        {
            return db.DeleteAsync(item);
        }
    }
}
