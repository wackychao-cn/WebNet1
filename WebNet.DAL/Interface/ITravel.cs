using System.Collections.Generic;

namespace WebNet.DAL.Interface
{
    public interface ITravel
    {
        int Add(Model.Travel model);
        bool Update(Model.Travel model);
        bool UpdateByCond(string strset, string cond);
        bool Delete(int id);
        bool DeleteByCond(string cond);
        Model.Travel GetModel(int id);
        Model.Travel GetModelByCond(string cond);
        List<Model.Travel> GetList(string cond);
        List<Model.Travel> GetList(string fileds, string order,
            string ordertype, int pagesize, int pageindex, string cond);
        int CalcCount(string cond);
        string GetOneField(string filed, string cond);
    }
}
