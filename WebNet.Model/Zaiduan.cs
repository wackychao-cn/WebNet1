using System;
using System.ComponentModel.DataAnnotations;
namespace WebNet.Model
{
    /// <summary>Zaiduan表实体类
    /// 作者:牛腩(QQ:164423073)
    /// 创建时间:2024-12-24 11:44:54
    /// </summary>
    public partial class Zaiduan
    {
        private int _Id;
        /// <summary> 
        /// Id 
        /// </summary> 
        public int Id
        {
            set { _Id = value; }
            get { return _Id; }
        }
        private DateTime _CreateTime = DateTime.Now;
        /// <summary> 
        /// CreateTime 
        /// </summary> 
        public DateTime CreateTime
        {
            set { _CreateTime = value; }
            get { return _CreateTime; }
        }
        private string _Name;
        /// <summary> 
        /// Name 
        /// </summary> 
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name
        {
            set { _Name = value; }
            get { return _Name; }
        }
        private string _Number;
        /// <summary> 
        /// Number 
        /// </summary> 
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Number
        {
            set { _Number = value; }
            get { return _Number; }
        }
        private string _Mission;
        /// <summary> 
        /// 任务 
        /// </summary> 
        public string Mission
        {
            set { _Mission = value; }
            get { return _Mission; }
        }
        private string _Area;
        /// <summary> 
        /// 地点 
        /// </summary> 
        public string Area
        {
            set { _Area = value; }
            get { return _Area; }
        }
        private string _Sequnce;
        /// <summary> 
        /// 序号排序
        /// </summary> 
        public string Sequnce
        {
            set { _Sequnce = value; }
            get { return _Sequnce; }
        }
    }
}
