using IL.Terraria.GameContent;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XLib.Behaviors
{
	/*
	public enum ClockEventState : byte
	{
		Waiting, Doing, Ended
	}
	public class Clock<TBehavior> : BehaviorComponent<TBehavior>
		where TBehavior : Behavior
	{
		public delegate bool ClockAction(ClockEventState state, int time);
		public override bool NetUpdateThis => false;
		public override string BehaviorName => "Clock";
		protected int Time = 0;
		//public readonly LinkedList<ClockEvent> events = new();
		public readonly MapEx<int, LinkedList<ClockEvent>> eventsMap = new();
		public readonly List<ClockAction> clockActions = new();
		public readonly LinkedList<ClockEvent> clockActionsActive = new();
		public class ClockEvent {
			public int createMoment;
			public int startTimeRaw;
			public int stayTimeRaw;
			//public Func<int> DelayStart;
			//public Func<int> DelayEnd;
			public int StartTimeReal => createMoment + startTimeRaw;
			public int EndTimeReal => createMoment + startTimeRaw + stayTimeRaw;
			public ClockEventState state;
			public int clockActionId;
			public int ActiveTime {
				get {
					return state switch
					{
						ClockEventState.Waiting => StartTimeReal,
						ClockEventState.Doing => EndTimeReal,
						_ => int.MaxValue,
					};
				}
			}
			public int behaviorId;
			public ClockEvent(int createMoment, int startTime, int stayTime, ClockEventState state, int behaviorId = -1, int clockActionId = -1)
			{
				this.createMoment = createMoment;
				this.startTimeRaw = startTime;
				this.stayTimeRaw = stayTime;
				this.state = state;
				this.behaviorId = behaviorId;
				this.clockActionId = clockActionId;
			}
		}
		public bool TryDo(ClockEvent i) => (i.clockActionId != -1 && !clockActions[i.clockActionId](i.state, i.state switch {ClockEventState.Waiting=>i.startTimeRaw, ClockEventState.Doing => i.stayTimeRaw,_=>-1})) || (i.behaviorId != -1 && !(i.state switch { ClockEventState.Waiting => ActivateBehavior(i.behaviorId), ClockEventState.Doing =>PauseBehavior(i.behaviorId), _ => true }));
		public void AddClockEvent(int startTime,int stayTime, int behaviorId = -1, int clockActionId = -1) {
			AddClockEvent(new ClockEvent(Time, startTime, stayTime, ClockEventState.Waiting, behaviorId,clockActionId));
		}
		public void AddClockEvent(ClockEvent clockEvent) {
			LinkedList<ClockEvent> list;
			if (eventsMap.First?.Key.Equals(clockEvent.ActiveTime)??false) list = eventsMap.First.Value;
			else if(eventsMap.First?.Next?.Key.Equals(clockEvent.ActiveTime)??false ) list = eventsMap.First.Next.Value;
			else if (!eventsMap.TryGetValue(clockEvent.ActiveTime, out list)) {
				eventsMap.Add(clockEvent.ActiveTime, list = new LinkedList<ClockEvent>());
			}
			list.AddLast(clockEvent);
		}

		public override void AI()
		{
			{
				LinkedListNode<ClockEvent> node = clockActionsActive.First;
				LinkedListNode < ClockEvent > Remove(ref LinkedListNode<ClockEvent> node) {
					var n2 = node.Next;
					clockActionsActive.Remove(node);
					return node=n2;
				}
				while (node != null) {
					var i = node.Value;
					if (i.state != ClockEventState.Doing) {
						Remove(ref node);
						continue;
					}
					if (TryDo(i)) {
						i.state += 1;
						Remove(ref node);
						continue;
					}
					node = node.Next;
				}
			}
			if (eventsMap.Count>0&&eventsMap.First.Key == Time) {
				var l=eventsMap.First.Value;
				eventsMap.Remove(Time);
				foreach (var i in l) {
					if (i.state == ClockEventState.Waiting) 
					{
						if (TryDo(i))
						{//delay
							i.startTimeRaw += 1;
							AddClockEvent(i);
						}
						else
						{//next
							i.state += 1;
							clockActionsActive.AddLast(i);
							AddClockEvent(i);
						}
					}else
					if (i.state == ClockEventState.Doing)
					{
						if (TryDo(i))
						{//delay
							i.stayTimeRaw += 1;
							AddClockEvent(i);
							continue;
						}
						else
						{//next
							i.state += 1;
						}
					}
				}
			}
			base.AI();
			Time += 1;
		}
		public override void OnNetUpdateSend(BinaryWriter writer)
		{
			List<ClockEvent> es = new();
			foreach (var i in eventsMap) {
				foreach (var j in i.Value) {
					es.Add(j);
				}
			}
			writer.Write(es.Count);
			foreach (var i in es) {
				writer.Write(i.createMoment-Time);
				writer.Write(i.startTimeRaw);
				writer.Write(i.stayTimeRaw);
				writer.Write((byte)i.state);
				writer.Write(i.behaviorId);
				writer.Write(i.clockActionId);
			}
		}
		public override void OnNetUpdateReceive(BinaryReader reader)
		{
			//events.ClearStoreLast();
			eventsMap.Clear();
			int count = reader.ReadInt32();
			//int t_=-1;
			//LinkedList<ClockEvent> list;
			//LinkedListNode<ClockEvent> node;
			//void NextT(int t)
			//{
			//	if (t == t_) return;
			//	if (!events.TryGetValue(t, out list))
			//	{
			//		events.Add(t, list = new LinkedList<ClockEvent>());
			//	}
			//}
			
			for (int i = 0; i < count; ++i) {
				//int cm=reader.ReadInt32()+Time;
				//int start=reader.ReadInt32();
				//int stay=reader.ReadInt32();
				//ClockEventState state = (ClockEventState)reader.ReadByte();
				//int id=reader.ReadInt32();
				//int t;
				//if (state == ClockEventState.Waiting) t = cm + start;
				//else t = cm + stay;
				//NextT(t);
				AddClockEvent(new ClockEvent(
					  reader.ReadInt32() + Time,
						reader.ReadInt32(),
						reader.ReadInt32(),
						(ClockEventState)reader.ReadByte(),
						reader.ReadInt32(),
						reader.ReadInt32()));
			}
		}
	}*/
}
