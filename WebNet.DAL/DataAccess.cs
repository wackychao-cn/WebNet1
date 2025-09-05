using System; 

namespace WebNet.DAL
{
	/// <summary>
	/// 根据数据库字符串返回相应的数据库实现类
	/// </summary>
	public class DataAccess
	{
        			 public static Interface.IUserInfo CreateUserInfoDAL(string db) {
				 Interface.IUserInfo dal = null;
				 switch (db.ToLower()){ 
					 case "sqlserver":
						 dal = new UserInfoDAL();
						 break;
				 }
				 return dal;
			 }

			 public static Interface.ICategory CreateCategoryDAL(string db) {
				 Interface.ICategory dal = null;
				 switch (db.ToLower()){ 
					 case "sqlserver":
						 dal = new CategoryDAL();
						 break;
				 }
				 return dal;
			 }

			 public static Interface.INews CreateNewsDAL(string db) {
				 Interface.INews dal = null;
				 switch (db.ToLower()){ 
					 case "sqlserver":
						 dal = new NewsDAL();
						 break;
				 }
				 return dal;
			 }
        public static Interface.ITravel CreateTravelDAL(string db)
        {
            Interface.ITravel dal = null;
            switch (db.ToLower())
            {
                case "sqlserver":
                    dal = new TravelDAL();
                    break;
            }
            return dal;
        }

        public static Interface.IDuty CreateDutyDAL(string db)
        {
            Interface.IDuty dal = null;
            switch (db.ToLower())
            {
                case "sqlserver":
                    dal = new DutyDAL();
                    break;
            }
            return dal;
        }
        public static Interface.IZaiduan CreateZaiduanDAL(string db)
        {
            Interface.IZaiduan dal = null;
            switch (db.ToLower())
            {
                case "sqlserver":
                    dal = new ZaiduanDAL();
                    break;
            }
            return dal;
        }
    }
}
