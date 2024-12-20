using System;
using System.ComponentModel.DataAnnotations;
namespace WebNet.Model
{
	/// <summary>Travel表实体类
	/// 作者:牛腩(QQ:164423073)
	/// 创建时间:2024-12-19 09:00:16
	/// </summary>
	public partial class Travel
	{
		private int _Id;
	 /// <summary> 
	 /// Id 
	 /// </summary> 
		public int Id
		{
			set{ _Id=value;}
			get{return _Id;}
		}
		private DateTime _CreateTime = DateTime.Now;
	 /// <summary> 
	 /// CreateTime 
	 /// </summary> 
		public DateTime CreateTime
		{
			set{ _CreateTime=value;}
			get{return _CreateTime;}
		}
		private string _Name;
	 /// <summary> 
	 /// Name 
	 /// </summary> 
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Name
		{
			set{ _Name=value;}
			get{return _Name;}
		}
		private DateTime _StartTime = DateTime.Now;
	 /// <summary> 
	 /// StartTime 
	 /// </summary> 
		public DateTime StartTime
		{
			set{ _StartTime=value;}
			get{return _StartTime;}
		}
		private DateTime _EndTime = DateTime.Now;
	 /// <summary> 
	 /// EndTime 
	 /// </summary> 
		public DateTime EndTime
		{
			set{ _EndTime=value;}
			get{return _EndTime;}
		}
	}
}
