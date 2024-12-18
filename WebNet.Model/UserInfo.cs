using System;
using System.ComponentModel.DataAnnotations;
namespace WebNet.Model
{
	/// <summary>UserInfo表实体类
	/// 作者:牛腩(QQ:164423073)
	/// 创建时间:2024-11-27 10:02:51
	/// </summary>
	public class UserInfo
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
		private string _Username;
	 /// <summary> 
	 /// 用户名 
	 /// </summary> 
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Username
		{
			set{ _Username=value;}
			get{return _Username;}
		}
		private string _Password;
	 /// <summary> 
	 /// 用户密码 
	 /// </summary> 
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Password
		{
			set{ _Password=value;}
			get{return _Password;}
		}
	}
}
