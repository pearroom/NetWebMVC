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
    /// 实体类dict_role。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Table("dict_role")]
    [Serializable]
    public partial class dict_role : Entity
    {
        #region Model
		private int _id;
		private string _rolename;
		private string _menus;
		private string _powers;

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
		/// 角色名称
		/// </summary>
		[Field("rolename")]
		public string rolename
		{
			get{ return _rolename; }
			set
			{
				this.OnPropertyValueChange("rolename");
				this._rolename = value;
			}
		}
		/// <summary>
		/// 菜单ID集合
		/// </summary>
		[Field("menus")]
		public string menus
		{
			get{ return _menus; }
			set
			{
				this.OnPropertyValueChange("menus");
				this._menus = value;
			}
		}
		/// <summary>
		/// 权限ID集合
		/// </summary>
		[Field("powers")]
		public string powers
		{
			get{ return _powers; }
			set
			{
				this.OnPropertyValueChange("powers");
				this._powers = value;
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
				_.rolename,
				_.menus,
				_.powers,
			};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._id,
				this._rolename,
				this._menus,
				this._powers,
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
			public readonly static Field All = new Field("*", "dict_role");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field id = new Field("id", "dict_role", "");
            /// <summary>
			/// 角色名称
			/// </summary>
			public readonly static Field rolename = new Field("rolename", "dict_role", "角色名称");
            /// <summary>
			/// 菜单ID集合
			/// </summary>
			public readonly static Field menus = new Field("menus", "dict_role", "菜单ID集合");
            /// <summary>
			/// 权限ID集合
			/// </summary>
			public readonly static Field powers = new Field("powers", "dict_role", "权限ID集合");
        }
        #endregion
	}
}