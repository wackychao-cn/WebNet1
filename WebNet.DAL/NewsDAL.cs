using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace WebNet.DAL
{
    /// <summary>[News]表数据访问类
    /// 作者:牛腩(QQ:164423073)
    /// 创建时间:2024-11-27 10:02:51
    /// </summary>
    public partial class NewsDAL:Interface.INews
    {
        private static readonly object _lock = new object();
        public NewsDAL()
        { }
        /// <summary>增加一条数据
        /// 
        /// </summary>
        public int Add(WebNet.Model.News model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [News](");
            strSql.Append("[CreateTime], [Caname], [Bh], [Title], [Body]  )");
            strSql.Append(" values (");
            strSql.Append("@CreateTime, @Caname, @Bh, @Title, @Body  )");
            strSql.Append(";select @@IDENTITY");
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());
            h.AddParameter("@Id", model.Id);
            h.AddParameter("@CreateTime", model.CreateTime);
            h.AddParameter("@Caname", model.Caname);
            h.AddParameter("@Bh", model.Bh);
            h.AddParameter("@Title", model.Title);
            h.AddParameter("@Body", model.Body);

            int result;
            string obj = h.ExecuteScalar();
            if (!int.TryParse(obj, out result))
            {
                return 0;
            }
            return result;
        }

        /// <summary>更新一条数据
        /// 
        /// </summary>
        public bool Update(WebNet.Model.News model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [News] set ");
            strSql.Append("[CreateTime]=@CreateTime, [Caname]=@Caname, [Bh]=@Bh, [Title]=@Title, [Body]=@Body  ");
            strSql.Append(" where Id=@Id ");
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());
            h.AddParameter("@Id", model.Id);
            h.AddParameter("@CreateTime", model.CreateTime);
            h.AddParameter("@Caname", model.Caname);
            h.AddParameter("@Bh", model.Bh);
            h.AddParameter("@Title", model.Title);
            h.AddParameter("@Body", model.Body);

            return h.ExecuteNonQuery();
        }

        /// <summary>按条件更新数据
        /// 
        /// </summary>
        public bool UpdateByCond(string str_set, string cond)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [News] set "+str_set +" "); 
            strSql.Append(" where "+cond);
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());
            return h.ExecuteNonQuery();
        }

        /// <summary>删除一条数据
        /// 
        /// </summary>
        public bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [News] ");
            strSql.Append(" where Id=@Id ");
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());
            h.AddParameter("@Id", Id);
            return h.ExecuteNonQuery();
        }

        /// <summary>根据条件删除数据
        /// 
        /// </summary>
        public bool DeleteByCond(string cond)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [News] ");
            if (!string.IsNullOrEmpty(cond))
            {
                strSql.Append(" where " + cond);
            }
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());
            return h.ExecuteNonQuery();
        }
		
        /// <summary>取一个字段的值
        /// 
        /// </summary>
        /// <param name="filed">字段，如sum(je)</param>
        /// <param name="cond">条件，如userid=2</param>
        /// <returns></returns>
        public string GetOneField(string filed, string cond )
        {
            string sql = "select " + filed + " from [News]";
            if (!string.IsNullOrEmpty(cond))
            {
                sql += " where " + cond;
            }
			
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(sql);
          
            return h.ExecuteScalar();
        }

        /// <summary>得到一个对象实体
        /// 
        /// </summary>
        public WebNet.Model.News GetModel(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [News] ");
            strSql.Append(" where Id=@Id ");
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());
            h.AddParameter("@Id", Id);
            WebNet.Model.News model = null;
            using (IDataReader dataReader = h.ExecuteReader())
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
                h.CloseConn();
            }
            return model;
        }

        /// <summary>根据条件得到一个对象实体
        /// 
        /// </summary>
        public WebNet.Model.News GetModelByCond(string cond )
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from [News] ");
            if (!string.IsNullOrEmpty(cond))
            {
                strSql.Append(" where " + cond);
            }
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());
         
            WebNet.Model.News model = null;
            using (IDataReader dataReader = h.ExecuteReader())
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
                h.CloseConn();
            }
            return model;
        }

        /// <summary>获得数据列表
        /// 
        /// </summary>
        public DataSet GetListDS(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM [News] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());
            DataTable dt = h.ExecuteQuery();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>分页获取数据列表
        /// 
        /// </summary>
        public DataSet GetListDS(string fileds, string order, string ordertype, int PageSize, int PageIndex, string strWhere)
        {
           string cond = string.IsNullOrEmpty(strWhere) ? "" : string.Format(" where {0}", strWhere);
            string sql = string.Format("SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY {0} {1}) AS pos, {2} FROM  [News] {3}  ) AS sp WHERE pos BETWEEN {4} AND {5}", order, ordertype, fileds, cond, (((PageIndex - 1) * PageSize) + 1), PageSize * PageIndex);

            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(sql); 
            DataTable dt = h.ExecuteQuery();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

         /// <summary>
        ///分页，使用offset,mssql2012以后有用
        /// </summary>
        /// <param name="fileds"></param>
        /// <param name="orderstr">如：yydate desc,yytime asc,id desc,必须形成唯一性</param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetListOFFSET(string fileds, string orderstr, int PageSize, int PageIndex, string strWhere) {

            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = " where " + strWhere;
            }
            string sql = string.Format("select {0} from [News] {1} order by {2} offset {3} rows fetch next {4} rows only", fileds, strWhere, orderstr, (PageIndex - 1) * PageSize, PageSize);
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(sql);
            DataTable dt = h.ExecuteQuery();
            return dt;

        }

        /// <summary>获得数据列表（比DataSet效率高，推荐使用）
        /// 
        /// </summary>
        public List<WebNet.Model.News> GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM [News] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<WebNet.Model.News> list = new List<WebNet.Model.News>();
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());
		
            using (IDataReader dataReader = h.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
                h.CloseConn();
            }
            return list;
        }

        /// <summary>分页获取数据列表
        /// 
        /// </summary>
        public List<WebNet.Model.News> GetList(string fileds, string order, string ordertype, int PageSize, int PageIndex, string strWhere)
        {
		//  string cond = string.IsNullOrEmpty(strWhere) ? "" : $" where {strWhere}";
           // string sql = $"SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY {order} {ordertype}) AS pos, {fileds} FROM  [News] {cond}  ) AS sp WHERE pos BETWEEN {(((PageIndex - 1) * PageSize) + 1)} AND {PageSize * PageIndex}";

            string cond = string.IsNullOrEmpty(strWhere) ? "" : string.Format(" where {0}",strWhere);
            string sql = string.Format("SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY {0} {1}) AS pos, {2} FROM  [News] {3}  ) AS sp WHERE pos BETWEEN {4} AND {5}",order,ordertype,fileds,cond, (((PageIndex - 1) * PageSize) + 1), PageSize * PageIndex);
          
            MSSQLHelper h = new MSSQLHelper();
			h.CreateCommand(sql);
    

           /* h.CreateStoredCommand("[proc_SplitPage]");
            h.AddParameter("@tblName", "[News]");
            h.AddParameter("@strFields", fileds);
            h.AddParameter("@strOrder", order);
            h.AddParameter("@strOrderType", ordertype);
            h.AddParameter("@PageSize", PageSize);
            h.AddParameter("@PageIndex", PageIndex);
            h.AddParameter("@strWhere", strWhere);*/

            List<WebNet.Model.News> list = new List<WebNet.Model.News>();
            using (IDataReader dataReader = h.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
                h.CloseConn();
            }
            return list;
        }

         /// <summary>
        ///分页，使用offset,mssql2012以后有用
        /// </summary>
        /// <param name="fileds"></param>
        /// <param name="orderstr">如：yydate desc,yytime asc,id desc,必须形成唯一性</param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<WebNet.Model.News> GetListArrayOFFSET(string fileds, string orderstr, int PageSize, int PageIndex, string strWhere) {

            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = " where " + strWhere;
            }
            string sql = string.Format("select {0} from [News] {1} order by {2} offset {3} rows fetch next {4} rows only", fileds, strWhere, orderstr, (PageIndex - 1) * PageSize, PageSize);
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(sql);
            List<WebNet.Model.News> list = new List<WebNet.Model.News>();
            using (IDataReader dataReader = h.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
                h.CloseConn();
            }
            return list;
        }

        /// <summary>对象实体绑定数据
        /// 
        /// </summary>
        public WebNet.Model.News ReaderBind(IDataReader dataReader)
        {
            WebNet.Model.News model = new WebNet.Model.News();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            ojb = dataReader["CreateTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreateTime = (DateTime)ojb;
            }
            ojb = dataReader["Caname"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Caname = ojb.ToString();
            }
            ojb = dataReader["Bh"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Bh = ojb.ToString();
            }
            ojb = dataReader["Title"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Title = ojb.ToString();
            }
            ojb = dataReader["Body"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Body = ojb.ToString();
            }

            return model;
        }

        /// <summary>计算记录数
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int CalcCount(string cond)
        {
            string sql = "select count(1) from [News]";
            if (!string.IsNullOrEmpty(cond))
            {
                sql += " where " + cond;
            }
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(sql);
		
            return int.Parse(h.ExecuteScalar());
        }

         /// <summary>取distinct的datatable
        /// 
        /// </summary>
        /// <param name="fileds">字段</param>
        /// <param name="cond">条件</param>
        /// <returns></returns>
        public DataTable GetDistinctTable(string fileds, string cond) {
            string sql = "select distinct "+fileds+" from [News] ";
            if (!string.IsNullOrEmpty(cond))
            {
                sql += " where "+cond;
            }
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(sql);
            return h.ExecuteQuery();
        }
        /// <summary>
        /// 根据获取后n条数据,根据创建时间
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<WebNet.Model.News> GetBottomList(int num, string strWhere)
        {
            if (num < 1)
            {
                num = 1;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP " + num.ToString() + " * ");
            strSql.Append(" FROM [News]");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere + " ORDER BY Id DESC");
            }
            else
            {
                strSql.Append(" ORDER BY Id DESC");
            }
            List<WebNet.Model.News> list = new List<WebNet.Model.News>();
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());

            using (IDataReader dataReader = h.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
                h.CloseConn();
            }
            return list;
        }
        public List<WebNet.Model.News> GetBottomListbytime(int num, string strWhere)
        {
            if (num < 1)
            {
                num = 1;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP " + num.ToString() + " * ");
            strSql.Append(" FROM [News]");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere + " ORDER BY CreateTime DESC");
            }
            else
            {
                strSql.Append(" ORDER BY CreateTime DESC");
            }
            List<WebNet.Model.News> list = new List<WebNet.Model.News>();
            MSSQLHelper h = new MSSQLHelper();
            h.CreateCommand(strSql.ToString());

            using (IDataReader dataReader = h.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
                h.CloseConn();
            }
            return list;
        }
    }
}

