using LinePutScript.Localization.WPF;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.Sane {
	internal class Langs {
		/// <summary>
		/// 委托SpeakC的构造函数
		/// </summary>
		private static Func<IMainWindow,SpeakC> speakC_func;
		internal class SpeakC {
			internal readonly ClickText[] foodOverflowLow;
			internal readonly ClickText[] foodOverflowMid;
			internal readonly ClickText[] foodOverflowHigh;
			internal readonly ClickText[] foodOverflowDanger;

			internal readonly ClickText[] drinkOverflowLow;
			internal readonly ClickText[] drinkOverflowMid;
			internal readonly ClickText[] drinkOverflowHigh;
			internal readonly ClickText[] drinkOverflowDanger;
			static SpeakC() => speakC_func = (MW) => new SpeakC(MW);
			private SpeakC(IMainWindow MW) {
				foodOverflowLow = [.. MW.ClickTexts.FindAll(x => x.Working == "sepak_foodOverflowLow")];//List<ClickText>.ToArray();
				foodOverflowMid = [.. MW.ClickTexts.FindAll(x => x.Working == "sepak_foodOverflowMid")];
				foodOverflowHigh = [.. MW.ClickTexts.FindAll(x => x.Working == "sepak_foodOverflowHigh")];
				foodOverflowDanger = [.. MW.ClickTexts.FindAll(x => x.Working == "sepak_foodOverflowDanger")];

				drinkOverflowLow = [.. MW.ClickTexts.FindAll(x => x.Working == "sepak_drinkOverflowLow")];
				drinkOverflowMid = [.. MW.ClickTexts.FindAll(x => x.Working == "sepak_drinkOverflowMid")];
				drinkOverflowHigh = [.. MW.ClickTexts.FindAll(x => x.Working == "sepak_drinkOverflowHigh")];
				drinkOverflowDanger = [.. MW.ClickTexts.FindAll(x => x.Working == "sepak_drinkOverflowDanger")];
			}
			internal static string GetSpeakRan(ClickText[] say) => say[new Random().Next(say.Length)].TranslateText;
		}
		private readonly SpeakC speak;
		internal SpeakC Speak => speak;

		/// <summary>
		/// 初始化语言类
		/// </summary>
	 	internal Langs(IMainWindow MW) {
			RuntimeHelpers.RunClassConstructor(typeof(SpeakC).TypeHandle);
			speak = speakC_func(MW);
		}
	}
}
