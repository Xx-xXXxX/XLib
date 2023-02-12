using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.ID;

namespace XLib.Behaviors
{
	public interface IBehaviorComponentBase: IBehavior
	{
		bool PauseBehavior(int id);
		bool ActivateBehavior(int id);
		void OnNetUpdateSend(BinaryWriter writer);
		void OnNetUpdateReceive(BinaryReader reader);
		bool NetUpdateThis { get; }
	}
	/// <summary>
	/// 通过组合模式操作behavior
	/// Add应在Initialize前完成，否则应该报错
	/// 否则可能出现联机同步错误
	/// </summary>
	public interface IBehaviorComponentIn<in TBehavior> : IBehaviorComponentBase
	{
		/// <summary>
		/// 加入成员，应在Initialize前完成
		/// </summary>
		int Add(TBehavior behavior);
	}
	public interface IBehaviorComponentOut<out TBehavior>: IBehaviorComponentBase
	{
		IEnumerable<TBehavior> GetUsings();
		IReadOnlyList<TBehavior> BehaviorsList { get; }
	}
	public interface IBehaviorComponent<TBehavior> : IBehaviorComponentIn<TBehavior>, IBehaviorComponentOut<TBehavior> { }

	public abstract class BehaviorComponentOut<TBehavior>: Behavior, IEnumerable<TBehavior>, IBehaviorComponentOut<TBehavior>//, IBehaviorComponent<RealBehaviorType>
		where TBehavior : IBehavior
	{
		/// <summary>
		/// 获取Behavior
		/// </summary>
		public TBehavior this[int id] => BehaviorsList[id];
		/// <summary>
		/// 装有Behavior的容器
		/// </summary>
		public abstract IReadOnlyList<TBehavior> BehaviorsList { get; }
		/// <summary>
		/// 如果存在需要同步的组件则同步
		/// </summary>
		public override bool NetUpdate
		{
			get
			{
				if (NetUpdateThis) return true;
				foreach (var i in GetUsings()) if (i.NetUpdate) return true;
				return false;
			}
		}
		/// <summary>
		/// 是否同步自己
		/// </summary>
		public abstract bool NetUpdateThis { get; }
		public override void AI()
		{
			foreach (var i in GetUsings())
			{
				i.AI();
			}
		}
		public override object Call(params object[] vs)
		{
			return null;
		}
		public override bool CanActivate()
		{
			bool can = base.CanActivate();
			if (!can) return false;
			foreach (var i in GetUsings())
			{
				can &= i.CanActivate();
				if (!can) return false;
			}
			return true;
		}
		public override bool CanPause()
		{
			bool can = base.CanPause();
			if (!can) return false;
			foreach (var i in GetUsings())
			{
				can &= i.CanPause();
				if (!can) return false;
			}
			return true;
		}
		public override void Dispose()
		{
			foreach (var i in BehaviorsList)
			{
				i.Dispose();
			}
		}
		public IEnumerator<TBehavior> GetEnumerator() => GetUsings().GetEnumerator();
		/// <summary>
		/// 获取启动的bahavior
		/// </summary>
		public abstract IEnumerable<TBehavior> GetUsings();
		public override void Initialize()
		{
			foreach (var i in BehaviorsList)
			{
				i.Initialize();
			}
		}
		public override void NetUpdateReceive(BinaryReader reader)
		{
			base.NetUpdateReceive(reader);
			bool netUpdateThis = reader.ReadBoolean();
			if (netUpdateThis) OnNetUpdateReceive(reader);
			int Count = reader.ReadInt32();
			for (int i = 0; i < Count; ++i)
			{
				int id = i;
				Terraria.BitsByte bits = reader.ReadByte();
				bool active = bits[0];
				bool NetUpdate = bits[1];
				IBehavior behavior = BehaviorsList[id];
				if (active)
					ActivateBehavior(id);
				else
					PauseBehavior(id);
				if (NetUpdate)
					behavior.NetUpdateReceive(reader);
			}
		}
		public override void NetUpdateSend(BinaryWriter writer)
		{
			bool All = Utils.ExistNewPlayer();
			if (NetUpdateThis)
			{
				writer.Write(true);
				OnNetUpdateSend(writer);
			}
			else writer.Write(false);
			writer.Write(BehaviorsList.Count);
			for (int i = 0; i < BehaviorsList.Count; ++i)
			{
				TBehavior behavior = BehaviorsList[i];
				Terraria.BitsByte bits = 0;
				bits[0] = behavior.Active;
				bool NetUpdate = bits[1] = All || behavior.NetUpdate;
				writer.Write(bits);
				if (NetUpdate)
					behavior.NetUpdateSend(writer);
			}
		}
		public override void OnActivate()
		{
			foreach (var i in GetUsings())
			{
				i.OnActivate();
			}
			base.OnActivate();
		}
		public virtual void OnNetUpdateReceive(BinaryReader reader) { }
		public virtual void OnNetUpdateSend(BinaryWriter writer) { }
		public override void OnPause()
		{
			foreach (var i in GetUsings())
			{
				i.OnPause();
			}
			base.OnPause();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TBehavior>)this).GetEnumerator();
		}
		public abstract bool ActivateBehavior(int id);
		public abstract bool PauseBehavior(int id);
	}

	/// <summary>
	/// 通过组合模式操作behavior
	/// </summary>
	public abstract class BehaviorComponent<TBehavior> : BehaviorComponentOut<TBehavior>,IBehaviorComponent<TBehavior>
		where TBehavior : IBehavior
	{

		/// <summary>
		/// 添加新的behavior
		/// </summary>
		/// <param name="behavior"></param>
		/// <returns>其id</returns>
		public abstract int Add(TBehavior behavior);
		public override bool ActivateBehavior(int id)
		{
			TBehavior behavior = BehaviorsList[id];
			if (behavior == null) return false;
			if (!behavior.CanActivate()) return false;
			if (Active) behavior.OnActivate();
			ActivateBehaviorToUsing(id);
			return true;
		}
		public override bool PauseBehavior(int id)
		{
			TBehavior behavior = BehaviorsList[id];
			if (behavior == null) return false;
			if (!behavior.CanPause()) return false;
			if (Active) behavior.OnPause();
			PauseBehaviorToUsing(id);
			return true;
		}
		/// <summary>
		/// 启动behavior时
		/// </summary>
		/// <param name="id"></param>
		/// 
		protected abstract void ActivateBehaviorToUsing(int id);
		/// <summary>
		/// 暂停behavior时
		/// </summary>
		/// <param name="id"></param>
		/// 
		protected abstract void PauseBehaviorToUsing(int id);
	}

	public class BehaviorComponentStored<TBehavior> : BehaviorComponent<TBehavior>
		where TBehavior : IBehavior
	{
		/// <summary>
		/// 是否同步自己
		/// </summary>
		public override bool NetUpdateThis { get=>false; }
		/// <summary>
		/// 装有Behavior的容器
		/// </summary>
		//protected List<RealBehaviorType> BehaviorsList = new List<RealBehaviorType>();
		protected IList<TBehavior> behaviorsList = new List<TBehavior>();
		public override IReadOnlyList<TBehavior> BehaviorsList => (IReadOnlyList<TBehavior>)behaviorsList;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override int Add(TBehavior behavior)
		{
			int i = behaviorsList.Count;
			behaviorsList.Add(behavior);
			return i;
		}
		protected SetEx<int> Usings = new();
		protected override void ActivateBehaviorToUsing(int id)
		{
			Usings.Add(id);
		}
		protected override void PauseBehaviorToUsing(int id)
		{
			Usings.Remove(id);
		}
		public override IEnumerable<TBehavior> GetUsings() {
			foreach (var i in Usings) yield return behaviorsList[i];
		}
	}

}
