using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.ModLoader;

using XLib.Behaviors;

namespace XLib.Projectiles.Behaviors
{
	public class FromBehavior:ModProjBehavior<ModProjectile>
	{
		

		public readonly IBehavior behavior;

		public FromBehavior(ModProjectile modProj, IBehavior behavior) : base(modProj)
		{
			this.behavior = behavior;
		}

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


	public class FromBehaviorComponent< TModProj> : ModProjBehaviorComponent<TModProj>
		where TModProj : ModProjectile
	{
		public readonly IBehaviorComponent<ModProjBehavior<TModProj>> behaviorComponent;

		public FromBehaviorComponent(IBehaviorComponent<ModProjBehavior<TModProj>> behaviorComponent, TModProj modProj) : base(modProj)
		{
			this.behaviorComponent = behaviorComponent;
		}

		public override bool Active { get => behaviorComponent.Active; set => behaviorComponent.Active = value; }

		public override string BehaviorName => "From" + behaviorComponent.BehaviorName;

		public override bool NetUpdateThis => behaviorComponent.NetUpdateThis;

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

		public override IEnumerable<IModProjBehavior> GetUsings()
		{
			return behaviorComponent.GetUsings();
		}
	}
}
