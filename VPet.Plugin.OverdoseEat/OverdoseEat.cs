using LinePutScript.Localization.WPF;
using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.OverdoseEat {
	[SupportedOSPlatform("windows")]
	public class OverdoseEat : MainPlugin {
		public OverdoseEat(IMainWindow mainwin) : base(mainwin) {
		}
		double strengthFoodOverflow = 0;
		double StrengthFoodOverflow {
			get => strengthFoodOverflow;
			set => strengthFoodOverflow = value > 0 ? value : 0;
		}
		double strengthDrinkOverflow = 0;
		double StrengthDrinkOverflow {
			get => strengthDrinkOverflow;
			set => strengthDrinkOverflow = value > 0 ? value : 0;
		}


		public override void LoadPlugin() {
			MW.Event_TakeItem += MW_Event_TakeItem;
			MW.Main.TimeHandle += Main_TimeHandle;
		}

		private void Main_TimeHandle(Main main) {
			StrengthFoodOverflow=OverflowHealing(StrengthFoodOverflow);
			StrengthDrinkOverflow=OverflowHealing(StrengthDrinkOverflow);
		}
		double OverflowHealing(double targetValueInput) {
			if (targetValueInput > 0) {
				if (targetValueInput < 50) {
					targetValueInput -= 5;
				}
				else if (targetValueInput < 150) {
					targetValueInput -= 15;
					MW.Core.Save.Health--;
				}
				else if (targetValueInput < 250) {
					targetValueInput -= 10;
					MW.Core.Save.Health--;
				}
				else {
					targetValueInput -= 5;
					MW.Core.Save.Health--;
				}
			}
			return targetValueInput;
		}

		private void MW_Event_TakeItem(Food food) {
			double getFoodValue = MW.Core.Save.StrengthFood + MW.Core.Save.ChangeStrengthFood + (food.StrengthFood/2) - MW.Core.Save.StrengthMax;
			double getDrinkValue = MW.Core.Save.StrengthDrink + MW.Core.Save.ChangeStrengthDrink + (food.StrengthDrink/2) - MW.Core.Save.StrengthMax;
			if (getFoodValue > 0 || getDrinkValue > 0) {
				if (getFoodValue > 0) {
					StrengthFoodOverflow += getFoodValue;
				}
				if (getDrinkValue > 0) {
					StrengthDrinkOverflow += getDrinkValue;
				}
				if (food.StrengthFood > food.StrengthDrink) {
					if (StrengthFoodOverflow >= 25) {
						if (StrengthFoodOverflow < 50) {
							MW.Main.Say("吃饱啦");
						}
						else if (StrengthFoodOverflow < 150) {
							MW.Main.Say("有点撑了");
						}
						else if (StrengthFoodOverflow < 250) {
							MW.Main.Say("实在吃不下了");
						}
						else {
							MW.Main.Say("不能再喂了啦，要被撑死了");
						}
					}
				}
				else {
					if (StrengthDrinkOverflow >= 25) {
						if (StrengthDrinkOverflow < 50) {
							MW.Main.Say("喝水喝饱了");
						}
						else if (StrengthDrinkOverflow < 150) {
							MW.Main.Say("喝的有点撑了");
						}
						else if (StrengthDrinkOverflow < 250) {
							MW.Main.Say("实在喝不下了");
						}
						else {
							MW.Main.Say("不能再喂啦，要被撑死了");
						}
					}
				}
			}
		}
		public override string PluginName => "Overdose Eat";
	}	
}
