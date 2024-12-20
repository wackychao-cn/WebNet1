using System;
using System.ComponentModel.DataAnnotations;
namespace WebNet.Model
{
	/// <summary>Duty表实体类
	/// 作者:牛腩(QQ:164423073)
	/// 创建时间:2024-12-19 09:00:16
	/// </summary>
	public partial class Duty
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
		private DateTime _DutyTime = DateTime.Now;
	 /// <summary> 
	 /// DutyTime 
	 /// </summary> 
		public DateTime DutyTime
		{
			set{ _DutyTime=value;}
			get{return _DutyTime;}
		}
	}
}
