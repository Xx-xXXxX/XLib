using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace XLib.Projectiles.Behaviors
{
	public class CheckTarget:ModProjBehavior<ModProjectile>,IRefValue<UnifiedTarget>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override string BehaviorName => "Target";
		public override bool NetUpdate => false;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public readonly IRefValue<UnifiedTarget> Target;
		public UnifiedTarget Value { get => Target.Value; set => Target.Value = value; }
		/// <summary>
		/// 是否在目标可用的时候进行更新
		/// </summary>
		public Func<bool> DoUpdate = null;
		/// <summary>
		/// 玩家是否可以成为目标
		/// </summary>
		public Func<Player, bool> PlayerCanBeTargeted = null;
		/// <summary>
		/// NPC是否可以成为目标
		/// </summary>
		public Func<NPC, bool> NPCCanBeTargeted = null;
		/// <summary>
		/// 玩家的值
		/// </summary>
		public Func<Player, float> PlayerValue = null;
		/// <summary>
		/// NPC的值
		/// </summary>
		public Func<NPC, float> NPCValue = null;
		/// <summary>
		/// 是否寻找友善生物，包括玩家和友好NPC
		/// </summary>
		public bool FindFriendly = false;
		/// <summary>
		/// 是否寻找邪恶生物
		/// </summary>
		public bool FindHostile = true;
		/// <summary>
		/// 初始价值，相当于距离
		/// </summary>
		public float DefaultValue = 2048;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public CheckTarget(ModProjectile modProjectile, IRefValue<UnifiedTarget> target) : base(modProjectile)
		{
			DoUpdate = () => Projectile.timeLeft % 15 == 0;
			Target = target;
		}
		public CheckTarget(ModProjectile modProjectile, int UpdateTime, IRefValue<UnifiedTarget> target) : base(modProjectile)
		{
			DoUpdate = () => Projectile.timeLeft % UpdateTime == 0;
			Target = target;
		}
		/// <summary>
		/// 在进行查找之前检查，返回false阻止之后的检查
		/// </summary>
		public Func<IRefValue<UnifiedTarget>, bool> CheckBefore = null;
		public override void AI()
		{
			if ((!CheckBefore?.Invoke(Target))??false) return;

			DoUpdate ??= () => Projectile.timeLeft % 15 == 0;
			NPCCanBeTargeted ??= Utils.NPCCanFind;
			PlayerCanBeTargeted ??= Utils.PlayerCanFind;
			if (DoUpdate.Invoke() || 
					Value.IsNPC && !NPCCanBeTargeted(Value.npc) ||
					Value.IsPlayer && !PlayerCanBeTargeted(Value.player) ||
					Value.IsNull
				)
			{
				Value = Utils.CalculateUtils.FindTargetClosest(Projectile.Center, DefaultValue, FindFriendly, FindHostile, NPCCanBeTargeted, PlayerCanBeTargeted, NPCValue, PlayerValue);
			}
		}
		public override void OnActivate()
		{
			AI();
		}
		/// <summary>
		/// 设置为搜索邪恶生物
		/// </summary>
		public CheckTarget SetForHostileNPC(Func<NPC, bool> NPCCanBeTargeted = null, Func<NPC, float> NPCValue = null)
		{
			this.NPCCanBeTargeted = NPCCanBeTargeted;
			this.NPCValue = NPCValue;
			FindFriendly = false;
			FindHostile = true;
			this.PlayerCanBeTargeted = (p) => false;
			return this;
		}
		/// <summary>
		/// 设置为搜索玩家
		/// </summary>
		public CheckTarget SetForPlayer(Func<Player, bool> PlayerCanBeTargeted = null, Func<Player, float> PlayerValue = null)
		{
			this.PlayerCanBeTargeted = PlayerCanBeTargeted;
			this.PlayerValue = PlayerValue;
			FindFriendly = true;
			FindHostile = false;
			this.NPCCanBeTargeted = (p) => false;
			return this;
		}
		/// <summary>
		/// 设置优先搜索玩家的MinionAttackTarget
		/// </summary>
		/// <returns></returns>
		public CheckTarget SetForMinionAttackTarget()
		{
			CheckBefore = (IRefValue<UnifiedTarget> target)=>
			{
				Player player = Main.player[Projectile.owner];
				if (player.HasMinionAttackTargetNPC&&(target.Value.NPCID!=player.MinionAttackTargetNPC))
				{
					target.Value = UnifiedTarget.MakeNPC(player.MinionAttackTargetNPC);
					return false;
				}
				return true;
			};
			return this;
		}
	}
}
