using Terraria.ModLoader;
using Terraria.Localization;
using System.Linq;
using System.Linq.Expressions;
using ReLogic.Content.Sources;

namespace XLib
{
	public class XLib : Mod
	{
		private static XLib instance=null;

		public static XLib Instance => instance;
		public XLib()
		{
			instance = this;
			
		}


		public override void Load()
		{
			Logger.Info("Load");
			ModTranslation Translations = LocalizationLoader.CreateTranslation(this, "Disabled");
			Translations.SetDefault("Disabled");
			Translations.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese), "已禁用");
			LocalizationLoader.AddTranslation(Translations);

			Translations = LocalizationLoader.CreateTranslation(this, "Enabled");
			Translations.SetDefault("Enabled");
			Translations.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese), "已启用");
			LocalizationLoader.AddTranslation(Translations);

			Translations = LocalizationLoader.CreateTranslation(this, "On");
			Translations.SetDefault("On");
			Translations.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese), "开");
			LocalizationLoader.AddTranslation(Translations);


			Translations = LocalizationLoader.CreateTranslation(this, "Off");
			Translations.SetDefault("Off");
			Translations.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese), "关");
			LocalizationLoader.AddTranslation(Translations);
		}
		public override void PostSetupContent()
		{
			base.PostSetupContent();
			Logger.Info("PostSetupContent");
			BulkDictionary.Load();
		}
		public override IContentSource CreateDefaultContentSource()
		{
			Logger.Info("CreateDefaultContentSource");
			return base.CreateDefaultContentSource();
		}
		public override void Unload()
		{
			Logger.Info("Unload");
			instance = null;
			UnloadDoHolder.Unload();
			BulkDictionary.Unload();
		}
	}
}