using demo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo.BLL.Servicer
{
    public class TrangThaiService
    {
        private QLDICHBENHEntities dbContext;
        public TrangThaiService()
        {
            dbContext = new QLDICHBENHEntities();
        }
        public List<TrangThai> GetAllTrangThai()
        {
            return dbContext.TrangThai.ToList();
        }
        public TrangThai GetTrangThaiById(int id)
        {
            return dbContext.TrangThai.Find(id);
        }
    }
}
