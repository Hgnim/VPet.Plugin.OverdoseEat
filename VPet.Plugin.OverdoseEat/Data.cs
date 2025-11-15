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

		internal enum OverflowType {
			none, low, mid, high, danger
		}
		private OverflowType foodOfStatus = OverflowType.none;
		/// <summary>
		/// 当前食物溢出状态
		/// </summary>
		internal OverflowType FoodOfStatus {
			get => foodOfStatus;
			set => foodOfStatus = value;
		}
		private OverflowType drinkOfStatus = OverflowType.none;
		/// <summary>
		/// 当前饮品溢出状态
		/// </summary>
		internal OverflowType DrinkOfStatus {
			get => drinkOfStatus;
			set => drinkOfStatus = value;
		}
	}
}
