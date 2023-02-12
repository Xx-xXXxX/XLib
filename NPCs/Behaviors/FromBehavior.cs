using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.ModLoader;

using XLib.Behaviors;

namespace XLib.NPCs.Behaviors
{
	public class FromBehavior :ModNPCBehavior<ModNPC>
	{

		public readonly IBehavior behavior;

		public FromBehavior(ModNPC nPC, IBehavior behavior) : base(nPC) { this.behavior = behavior; }

		public override bool Active { get => behavior.Active; set => behavior.Active = value; }

		public override string BehaviorName => "From" + behavior.BehaviorName;

		public override bool NetUpdate => behavior.NetUpdate;

		public override void AI()
		{
			behavior.AI();
		}

		public override object Call(params object[] vs)
		{
			return behavior.Call(vs);
		}

		public override bool CanActivate()
		{
			return behavior.CanActivate();
		}

		public override bool CanPause()
		{
			return behavior.CanPause();
		}

		public override void Dispose()
		{
			behavior.Dispose();
		}

		public override void Initialize()
		{
			behavior.Initialize();
		}

		public override void NetUpdateReceive(BinaryReader reader)
		{
			behavior.NetUpdateReceive(reader);
		}

		public override void NetUpdateSend(BinaryWriter writer)
		{
			behavior.NetUpdateSend(writer);
		}

		public override void OnActivate()
		{
			behavior.OnActivate();
		}

		public override void OnPause()
		{
			behavior.OnPause();
		}
	}

	public class FromBehaviorComponent<TModNPC> : ModNPCBehaviorComponent<TModNPC> 
		where TModNPC:ModNPC
	{
		public readonly IBehaviorComponentOut<ModNPCBehavior<TModNPC>> behaviorComponent;

		public FromBehaviorComponent(IBehaviorComponent<ModNPCBehavior<TModNPC>> behaviorComponent, TModNPC modNPC) : base(modNPC)
		{
			this.behaviorComponent=behaviorComponent;
		}

		public override bool Active { get => behaviorComponent.Active; set => behaviorComponent.Active = value; }

		public override string BehaviorName =>"From"+behaviorComponent.BehaviorName;

		public override bool NetUpdateThis => behaviorComponent.NetUpdateThis;

		public override IReadOnlyList<IModNPCBehavior> BehaviorsList => behaviorComponent.BehaviorsList;

		public override void AI()
		{
			behaviorComponent.AI();
		}

		public override object Call(params object[] vs)
		{
			return behaviorComponent.Call(vs);
		}

		public override bool CanActivate()
		{
			return behaviorComponent.CanActivate();
		}

		public override bool CanPause()
		{
			return behaviorComponent.CanPause();
		}

		public override void Dispose()
		{
			behaviorComponent.Dispose();
		}

		public override void Initialize()
		{
			behaviorComponent.Initialize();
		}

		public override void OnNetUpdateReceive(BinaryReader reader)
		{
			behaviorComponent.OnNetUpdateReceive(reader);
		}

		public override void OnNetUpdateSend(BinaryWriter writer)
		{
			behaviorComponent.OnNetUpdateSend(writer);
		}

		public override void OnActivate()
		{
			behaviorComponent.OnActivate();
		}

		public override void OnPause()
		{
			behaviorComponent.OnPause();
		}

		public override IEnumerable<IModNPCBehavior> GetUsings()
		{
			return behaviorComponent.GetUsings();
		}

		public override bool ActivateBehavior(int id)
		{
			return behaviorComponent.ActivateBehavior(id);
		}

		public override bool PauseBehavior(int id)
		{
			return behaviorComponent.PauseBehavior(id);
		}
	}
}
