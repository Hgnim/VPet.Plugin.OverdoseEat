using LinePutScript.Localization.WPF;
using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using VPet.Plugin.Sane;
using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.OverdoseEat {
	[SupportedOSPlatform("windows")]
	public class OverdoseEat : MainPlugin {
		public OverdoseEat(IMainWindow mainwin) : base(mainwin) {
		}
		public override string PluginName => "Overdose Eat";

		readonly Data dat = new();
		Langs lang;

		public override void LoadPlugin() {
			lang = new(MW);

			MW.Event_TakeItem += TakeItem;
			MW.Main.TimeHandle += TimeHandle;
		}

		/// <summary>
		/// 游戏每个tick的调用
		/// </summary>
		private void TimeHandle(Main main) {
			dat.FoodOverflow=OverflowHealing(dat.FoodOverflow);
			if (//存在溢出时饱腹度不会减小，口渴度同理
				dat.FoodOverflow > 0 &&
				MW.Core.Save.StrengthFood + MW.Core.Save.ChangeStrengthFood < MW.Core.Save.StrengthMax
				)
				MW.Core.Save.ChangeStrengthFood = MW.Core.Save.StrengthMax - MW.Core.Save.StrengthFood;

			dat.DrinkOverflow =OverflowHealing(dat.DrinkOverflow);
			if (
				dat.DrinkOverflow > 0 &&
				MW.Core.Save.StrengthDrink + MW.Core.Save.ChangeStrengthDrink < MW.Core.Save.StrengthMax
				)
				MW.Core.Save.ChangeStrengthDrink = MW.Core.Save.StrengthMax - MW.Core.Save.StrengthDrink;
		}
		/// <summary>
		/// 摄入过量恢复计算，恢复时减少健康值
		/// </summary>
		/// <param name="targetValueInput">溢出值</param>
		/// <returns>返回恢复后剩余的溢出值</returns>
		double OverflowHealing(double targetValueInput) {
			if (targetValueInput > 0) {
				if (targetValueInput < 100) {
					targetValueInput -= MW.Core.Save.Health * 0.3;
					MW.Core.Save.Health-= targetValueInput*0.01;
				}
				else if (targetValueInput < 200) {
					targetValueInput -= MW.Core.Save.Health * 0.2;
					MW.Core.Save.Health-= targetValueInput*0.013;
				}
				else if (targetValueInput < 300) {
					targetValueInput -= MW.Core.Save.Health * 0.1 + 10;
					MW.Core.Save.Health-= targetValueInput * 0.02;
				}
				else {
					targetValueInput -= MW.Core.Save.Health * 0.1 + 15;
					MW.Core.Save.Health-= targetValueInput * 0.025;
				}
			}
			return targetValueInput;
		}

		/// <summary>
		/// 食用/得到对象事件
		/// </summary>
		private void TakeItem(Food food) {
			void of(byte type) {
				double ofValue = 
					type == 0 
					? dat.FoodOverflow 
					: dat.DrinkOverflow;

				if (ofValue < 100) {
					if(type==0)
						MW.Main.Say(Langs.SpeakC.GetSpeakRan(lang.Speak.foodOverflowLow));
					else
						MW.Main.Say(Langs.SpeakC.GetSpeakRan(lang.Speak.drinkOverflowLow));
				}
				else if (ofValue < 200) {
					if (type == 0)
						MW.Main.Say(Langs.SpeakC.GetSpeakRan(lang.Speak.foodOverflowMid));
					else
						MW.Main.Say(Langs.SpeakC.GetSpeakRan(lang.Speak.drinkOverflowMid));
				}
				else if (ofValue < 300) {
					if (type == 0)
						MW.Main.Say(Langs.SpeakC.GetSpeakRan(lang.Speak.foodOverflowHigh));
					else
						MW.Main.Say(Langs.SpeakC.GetSpeakRan(lang.Speak.drinkOverflowHigh));
				}
				else {
					if (type == 0)
						MW.Main.Say(Langs.SpeakC.GetSpeakRan(lang.Speak.foodOverflowDanger));
					else
						MW.Main.Say(Langs.SpeakC.GetSpeakRan(lang.Speak.drinkOverflowDanger));
				}
			}
			void foodOf() => of(0);
			void drinkOf() => of(1);
			//计算溢出值
			double ofFoodValue = MW.Core.Save.StrengthFood + MW.Core.Save.ChangeStrengthFood + (food.StrengthFood/2) - MW.Core.Save.StrengthMax;
			double ofDrinkValue = MW.Core.Save.StrengthDrink + MW.Core.Save.ChangeStrengthDrink + (food.StrengthDrink/2) - MW.Core.Save.StrengthMax;
			if (ofFoodValue > 0 || ofDrinkValue > 0) {
				if (ofFoodValue > 0)
					dat.FoodOverflow += ofFoodValue;
				if (ofDrinkValue > 0)
					dat.DrinkOverflow += ofDrinkValue;
				if (ofFoodValue>0 && ofDrinkValue>0) {
					if(food.StrengthFood > food.StrengthDrink)
						foodOf();
					else
						drinkOf();
				}
				else {
					if (ofFoodValue > 0)
						foodOf();
					else
						drinkOf();
				}
			}
		}
		
	}	
}
