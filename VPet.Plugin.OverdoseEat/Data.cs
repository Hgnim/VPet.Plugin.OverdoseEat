using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPet.Plugin.OverdoseEat {
	internal class Data {
		private double foodOverflow = 0;
		/// <summary>
		/// 食物食用溢出值
		/// </summary>
		internal double FoodOverflow {
			get => foodOverflow;
			set => foodOverflow = value > 0 ? value : 0;
		}
		private double drinkOverflow = 0;
		/// <summary>
		/// 饮品饮用溢出值
		/// </summary>
		internal double DrinkOverflow {
			get => drinkOverflow;
			set => drinkOverflow = value > 0 ? value : 0;
		}
	}
}
