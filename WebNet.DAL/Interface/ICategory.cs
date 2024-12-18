using System.Collections.Generic;

namespace WebNet.DAL.Interface
{
    public interface ICategory
    {
        int Add(Model.Category model);
        bool Update(Model.Category model);
        bool UpdateByCond(string strset, string cond);
        bool Delete(int id);
        bool DeleteByCond(string cond);
        Model.Category GetModel(int id);
        Model.Category GetModelByCond(string cond);
        List<Model.Category> GetList(string cond);
        List<Model.Category> GetList(string fileds, string order,
            string ordertype, int pagesize, int pageindex, string cond);
        int CalcCount(string cond);
        string GetOneField(string filed, string cond);
    }
}
