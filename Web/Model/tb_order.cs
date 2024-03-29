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
    /// 实体类tb_order。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Table("tb_order")]
    [Serializable]
    public partial class tb_order : Entity
    {
        #region Model
		private int _id;
		private string _projectid;
		private string _uid;
		private string _price;
		private string _istype;
		private string _notify_url;
		private string _return_url;
		private string _orderid;
		private string _orderuid;
		private string _goodsname;
		private string _key;
		private string _paysapi_id;
		private string _realprice;
		private DateTime? _ordertime;
		private DateTime? _paytime;
		private string _ispay;

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
		[Field("projectid")]
		public string projectid
		{
			get{ return _projectid; }
			set
			{
				this.OnPropertyValueChange("projectid");
				this._projectid = value;
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
		[Field("price")]
		public string price
		{
			get{ return _price; }
			set
			{
				this.OnPropertyValueChange("price");
				this._price = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("istype")]
		public string istype
		{
			get{ return _istype; }
			set
			{
				this.OnPropertyValueChange("istype");
				this._istype = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("notify_url")]
		public string notify_url
		{
			get{ return _notify_url; }
			set
			{
				this.OnPropertyValueChange("notify_url");
				this._notify_url = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("return_url")]
		public string return_url
		{
			get{ return _return_url; }
			set
			{
				this.OnPropertyValueChange("return_url");
				this._return_url = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("orderid")]
		public string orderid
		{
			get{ return _orderid; }
			set
			{
				this.OnPropertyValueChange("orderid");
				this._orderid = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("orderuid")]
		public string orderuid
		{
			get{ return _orderuid; }
			set
			{
				this.OnPropertyValueChange("orderuid");
				this._orderuid = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("goodsname")]
		public string goodsname
		{
			get{ return _goodsname; }
			set
			{
				this.OnPropertyValueChange("goodsname");
				this._goodsname = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("key")]
		public string key
		{
			get{ return _key; }
			set
			{
				this.OnPropertyValueChange("key");
				this._key = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("paysapi_id")]
		public string paysapi_id
		{
			get{ return _paysapi_id; }
			set
			{
				this.OnPropertyValueChange("paysapi_id");
				this._paysapi_id = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("realprice")]
		public string realprice
		{
			get{ return _realprice; }
			set
			{
				this.OnPropertyValueChange("realprice");
				this._realprice = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ordertime")]
		public DateTime? ordertime
		{
			get{ return _ordertime; }
			set
			{
				this.OnPropertyValueChange("ordertime");
				this._ordertime = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("paytime")]
		public DateTime? paytime
		{
			get{ return _paytime; }
			set
			{
				this.OnPropertyValueChange("paytime");
				this._paytime = value;
			}
		}
		/// <summary>
		/// 是否支付
		/// </summary>
		[Field("ispay")]
		public string ispay
		{
			get{ return _ispay; }
			set
			{
				this.OnPropertyValueChange("ispay");
				this._ispay = value;
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
				_.projectid,
				_.uid,
				_.price,
				_.istype,
				_.notify_url,
				_.return_url,
				_.orderid,
				_.orderuid,
				_.goodsname,
				_.key,
				_.paysapi_id,
				_.realprice,
				_.ordertime,
				_.paytime,
				_.ispay,
			};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._id,
				this._projectid,
				this._uid,
				this._price,
				this._istype,
				this._notify_url,
				this._return_url,
				this._orderid,
				this._orderuid,
				this._goodsname,
				this._key,
				this._paysapi_id,
				this._realprice,
				this._ordertime,
				this._paytime,
				this._ispay,
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
			public readonly static Field All = new Field("*", "tb_order");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field id = new Field("id", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field projectid = new Field("projectid", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field uid = new Field("uid", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field price = new Field("price", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field istype = new Field("istype", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field notify_url = new Field("notify_url", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field return_url = new Field("return_url", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field orderid = new Field("orderid", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field orderuid = new Field("orderuid", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field goodsname = new Field("goodsname", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field key = new Field("key", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field paysapi_id = new Field("paysapi_id", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field realprice = new Field("realprice", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field ordertime = new Field("ordertime", "tb_order", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field paytime = new Field("paytime", "tb_order", "");
            /// <summary>
			/// 是否支付
			/// </summary>
			public readonly static Field ispay = new Field("ispay", "tb_order", "是否支付");
        }
        #endregion
	}
}