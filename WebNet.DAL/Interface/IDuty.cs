using System.Collections.Generic;

namespace WebNet.DAL.Interface
{
    public interface IDuty
    {
        int Add(Model.Duty model);
        bool Update(Model.Duty model);
        bool UpdateByCond(string strset, string cond);
        bool Delete(int id);
        bool DeleteByCond(string cond);
        Model.Duty GetModel(int id);
        Model.Duty GetModelByCond(string cond);
        List<Model.Duty> GetList(string cond);
        List<Model.Duty> GetList(string fileds, string order,
            string ordertype, int pagesize, int pageindex, string cond);
        int CalcCount(string cond);
        string GetOneField(string filed, string cond);
        public int ImportDutyToDatabase(string filePath);
        public bool ClearAll();
    }
}
