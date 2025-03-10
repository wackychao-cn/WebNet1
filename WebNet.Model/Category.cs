using System;
using System.ComponentModel.DataAnnotations;
namespace WebNet.Model
{
	/// <summary>Category表实体类
	/// 作者:牛腩(QQ:164423073)
	/// 创建时间:2024-11-27 10:02:51
	/// </summary>
	public class Category
	{
		private int _Id;
	 /// <summary> 
	 /// 主键 
	 /// </summary> 
		public int Id
		{
			set{ _Id=value;}
			get{return _Id;}
		}
		private DateTime _CreateTime = DateTime.Now;
	 /// <summary> 
	 /// 创建时间 
	 /// </summary> 
		public DateTime CreateTime
		{
			set{ _CreateTime=value;}
			get{return _CreateTime;}
		}
		private string _Caname;
	 /// <summary> 
	 /// 分类名称 
	 /// </summary> 
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Caname
		{
			set{ _Caname=value;}
			get{return _Caname;}
		}
		private string _Bh;
	 /// <summary> 
	 /// 编号 
	 /// </summary> 
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Bh
		{
			set{ _Bh=value;}
			get{return _Bh;}
		}
		private string _Pbh;
	 /// <summary> 
	 /// 父编号 
	 /// </summary> 
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Pbh
		{
			set{ _Pbh=value;}
			get{return _Pbh;}
		}
        private int _Sequence;
        /// <summary> 
        /// 序号 
        /// </summary> 
        public int Sequence
        {
            set { _Sequence = value; }
            get { return _Sequence; }
        }
    }
}
