using System.Collections.Generic;

namespace WebNet.DAL.Interface
{
    public interface INews
    {
        int Add(Model.News model);
        bool Update(Model.News model);
        bool UpdateByCond(string strset, string cond);
        bool Delete(int id);
        bool DeleteByCond(string cond);
        Model.News GetModel(int id);
        Model.News GetModelByCond(string cond);
        List<Model.News> GetList(string cond);
        List<Model.News> GetList(string fileds, string order,
            string ordertype, int pagesize, int pageindex, string cond);
        int CalcCount(string cond);
        string GetOneField(string filed, string cond);
    }
}
