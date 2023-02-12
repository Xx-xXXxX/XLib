using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace XLib
{
	public class BulkDictionary
	{

		public static List<Item> items;
		public static Dictionary<int, int> itemToBanner;
		public static IDictionary<int, int> NPCLoaderBannerToItem;
		public static SortedDictionary<TileStyle, int> tileToBanner;
		//public static Dictionary<int, TileStyle> itemToTile;
		//public static Dictionary<TileStyle, int> TileToItem;
		public struct TileStyle:IComparable<TileStyle>, IEquatable<TileStyle>
		{
			public ushort type;
			public short frameX;
			public short frameY;
			public TileStyle(int type, int frameX, int frameY) : this((ushort)type, (short)frameX, (short)frameY) { }
			public TileStyle(ushort type, short frameX, short frameY)
			{
				this.type = type;
				this.frameX = frameX;
				this.frameY = frameY;
			}
			public TileStyle(Tile tile)
			{
				type = tile.TileType;
				frameX = tile.TileFrameX;
				frameY = tile.TileFrameY;

			}
			public override bool Equals( object obj)
			{
				if (obj is not TileStyle v) return false;
				return v.type == type && v.frameX == frameX && v.frameY == frameY;
			}
			public override int GetHashCode()
			{
				return (type<<8)^frameX^(frameY<<16);
			}

			public static bool operator ==(TileStyle left, TileStyle right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(TileStyle left, TileStyle right)
			{
				return !(left == right);
			}

			public override string ToString()
			{
				return $"{type}({frameX},{frameY})";
			}

			public int CompareTo(TileStyle other)
			{
				if (other.type != type) return 3 * (other.type > type).ToDirectionInt();
				if(other.frameY!=frameY) return 2 * (other.frameY > frameY).ToDirectionInt();
				if(other.frameX!=frameX) return (other.frameX > frameX).ToDirectionInt();
				return 0;
			}

			public bool Equals(TileStyle other)
			{
				return other.type == type && other.frameX == frameX && other.frameY == frameY;
			}
		}
		public static void Load()
		{
			items = new List<Item>();
			itemToBanner = new();
			tileToBanner = new();
			Tile tiler = Main.tile[16, 16];
			NPCLoaderBannerToItem = (IDictionary<int, int>)typeof(NPCLoader).GetField("bannerToItem",
				BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			foreach (var dict in NPCLoaderBannerToItem)
			{
				itemToBanner.Add(dict.Value, dict.Key);
			}
			for (int i = 1; i < ItemLoader.ItemCount; ++i)
			{
				Item item = new();
				item.SetDefaults(i);
				items.Add(item);
				int bannerType;
				if (i <= ItemID.Count && item.createTile == TileID.Banners)
				{
					int style = item.placeStyle;
					int frameX = style * 18;
					int frameY = 0;
					int widthcount = 90 + 21;
					if (style >= widthcount)
					{
						frameX -= widthcount*18;
						frameY += 54;
					}
					if (frameX >= 396 || frameY >= 54)
					{
						bannerType = frameX / 18 - 21;
						for (int num4 = frameY; num4 >= 54; num4 -= 54)
						{
							bannerType += widthcount;
						}
						itemToBanner.TryAdd(i, bannerType);
						TileStyle tilestyle = new(item.createTile, frameX, frameY);
						bool v1 = tilestyle.Equals(new(item.createTile, frameX, frameY));
						bool v2 = tilestyle == new TileStyle(item.createTile, frameX, frameY);


						tileToBanner.Add(tilestyle, bannerType);
						bool v3 = tileToBanner.ContainsKey(tilestyle);
						bool v4 = tileToBanner.ContainsKey(new(item.createTile, frameX, frameY));

					}
				}
				else if (item.createTile > TileID.Dirt && itemToBanner.TryGetValue(i, out bannerType))
				{
					//TileLoader.GetTile(item.createTile).Pl
					int style = item.placeStyle;
					int frameX = style * 18;
					int frameY = 0;
					if (style >= 90)
					{
						frameX -= 1620;
						frameY += 54;
					}
					tileToBanner.Add(new((ushort)item.createTile, (short)frameX, (short)frameY), bannerType);
				}
			}
		}

		public static void Unload()
		{
			items = null;
			itemToBanner = null;
			NPCLoaderBannerToItem = null;
		}
	}
}
