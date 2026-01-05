using System;
using System.Collections.Generic;
using System.Linq;
using demo.DAL;

namespace demo.BLL.Service
{
    public class DiaPhuongService
    {
        private QLDICHBENHEntities dbContext;

        public DiaPhuongService()
        {
            dbContext = new QLDICHBENHEntities();
        }

        public List<DiaPhuong> GetAllDiaPhuong()
        {
            return dbContext.DiaPhuong.ToList();
        }

        public DiaPhuong GetDiaPhuongById(string maDP)
        {
            return dbContext.DiaPhuong.Find(maDP);
        }

        // Thêm địa phương
        public void AddDiaPhuong(DiaPhuong diaPhuong)
        {
            var dp = dbContext.DiaPhuong.Find(diaPhuong.MaDP);
            if (dp == null)
            {
                dbContext.DiaPhuong.Add(diaPhuong);
                dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Địa phương đã tồn tại");
            }
        }

        // Cập nhật địa phương
        public void UpdateDiaPhuong(DiaPhuong diaPhuong)
        {
            var dp = dbContext.DiaPhuong.Find(diaPhuong.MaDP);
            if (dp != null)
            {
                dp.TenDP = diaPhuong.TenDP;
                dp.SoCaNhiemMoi = diaPhuong.SoCaNhiemMoi;
                dp.MaTT = diaPhuong.MaTT;

                dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Địa phương không tồn tại");
            }
        }
    }
}
