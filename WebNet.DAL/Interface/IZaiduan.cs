using System.Collections.Generic;

namespace WebNet.DAL.Interface
{
    public interface IZaiduan
    {
        int Add(Model.Zaiduan model);
        bool Update(Model.Zaiduan model);
        bool UpdateByCond(string strset, string cond);
        bool Delete(int id);
        bool DeleteByCond(string cond);
        Model.Zaiduan GetModel(int id);
        Model.Zaiduan GetModelByCond(string cond);
        List<Model.Zaiduan> GetList(string cond);
        List<Model.Zaiduan> GetList(string fileds, string order,
            string ordertype, int pagesize, int pageindex, string cond);
        int CalcCount(string cond);
        string GetOneField(string filed, string cond);
        public int ImportZaiduanToDatabase(string filePath);
        public bool ClearAll();
    }
}
