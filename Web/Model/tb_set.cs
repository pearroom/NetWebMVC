﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//     Website: http://ITdos.com/Dos/ORM/Index.html
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dos.ORM;

namespace Model
{
    /// <summary>
    /// 实体类tb_set。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Table("tb_set")]
    [Serializable]
    public partial class tb_set : Entity
    {
        #region Model
		private int _id;
		private string _token;
		private string _uid;
		private string _smsappkey;
		private string _host;

		/// <summary>
		/// 
		/// </summary>
		[Field("id")]
		public int id
		{
			get{ return _id; }
			set
			{
				this.OnPropertyValueChange("id");
				this._id = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("token")]
		public string token
		{
			get{ return _token; }
			set
			{
				this.OnPropertyValueChange("token");
				this._token = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("uid")]
		public string uid
		{
			get{ return _uid; }
			set
			{
				this.OnPropertyValueChange("uid");
				this._uid = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("smsappkey")]
		public string smsappkey
		{
			get{ return _smsappkey; }
			set
			{
				this.OnPropertyValueChange("smsappkey");
				this._smsappkey = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("host")]
		public string host
		{
			get{ return _host; }
			set
			{
				this.OnPropertyValueChange("host");
				this._host = value;
			}
		}
		#endregion

		#region Method
        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {
				_.id,
			};
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
				_.id,
				_.token,
				_.uid,
				_.smsappkey,
				_.host,
			};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._id,
				this._token,
				this._uid,
				this._smsappkey,
				this._host,
			};
        }
        /// <summary>
        /// 是否是v1.10.5.6及以上版本实体。
        /// </summary>
        /// <returns></returns>
        public override bool V1_10_5_6_Plus()
        {
            return true;
        }
        #endregion

		#region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
			/// <summary>
			/// * 
			/// </summary>
			public readonly static Field All = new Field("*", "tb_set");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field id = new Field("id", "tb_set", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field token = new Field("token", "tb_set", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field uid = new Field("uid", "tb_set", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field smsappkey = new Field("smsappkey", "tb_set", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field host = new Field("host", "tb_set", "");
        }
        #endregion
	}
}