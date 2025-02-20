using System.Collections.Generic;

namespace WebNet.DAL.Interface
{
    public interface IUserInfo
    {
        int Add(Model.UserInfo model);
        bool Update(Model.UserInfo model);
        bool UpdateByCond(string strset, string cond);
        bool Delete(int id);
        bool DeleteByCond(string cond);
        Model.UserInfo GetModel(int id);
        Model.UserInfo GetModelByCond(string cond);
        Model.UserInfo GetModelByUsernameAndPassword(string username, string password);
        List<Model.UserInfo> GetList(string cond);
        List<Model.UserInfo> GetList(string fileds, string order,
            string ordertype, int pagesize, int pageindex, string cond);
        int CalcCount(string cond);
        string GetOneField(string filed, string cond);
    }
}
