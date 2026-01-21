namespace Ans.Net10.Common
{

	public enum ImageOrientationEnum
	   : int
	{
		Unknown = 0,
		Landscape = 1,
		Portrait = 2,
		Square = 3
	}



	public enum ImageShiftEnum
		: int
	{
		Center = 0,
		Start = 1,
		StartMiddle = 2,
		EndMiddle = 3,
		End = 4
	}



	public class ImageResizeHelper
	{

		/* ctor */


		public ImageResizeHelper(
			uint width,
			uint height)
		{
			Width = width;
			Height = height;
			Ratio = (float)width / height;
			if (Width == Height)
			{
				IsNearSquare = true;
				Orientation = ImageOrientationEnum.Square;
			}
			else
			{
				IsNearSquare = (Ratio < 1.1 && Ratio > 0.9);
				Orientation = Ratio > 1
					? ImageOrientationEnum.Landscape
					: ImageOrientationEnum.Portrait;
			}
		}


		/* readonly properties */


		public uint Width { get; }
		public uint Height { get; }
		public float Ratio { get; }
		public ImageOrientationEnum Orientation { get; }
		public bool IsNearSquare { get; }

		public uint NewWidth { get; private set; }
		public uint NewHeight { get; private set; }


		/* methods */


		/// <summary>
		/// Масштабировать
		/// </summary>
		public void Scale(
			float ratio)
		{
			NewWidth = SuppMath.RoundToUInt(Width * ratio);
			NewHeight = SuppMath.RoundToUInt(Height * ratio);
		}


		/// <summary>
		/// Масштабировать внутри
		/// </summary>
		public void ScaleInside(
			float width,
			float height,
			bool noIncrease)
		{
			float r1 = (noIncrease && width >= Width && height >= Height)
				? 1 : Math.Min((width / Width), (height / Height));
			Scale(r1);
		}


		/// <summary>
		/// Масштабировать снаружи
		/// </summary>
		public void ScaleAround(
			float width,
			float height)
		{
			float r1 = Math.Max((width / Width), (height / Height));
			Scale(r1);
		}


		/// <summary>
		/// Масштабировать усреднением
		/// </summary>
		public void ScaleAverage(
			uint side)
		{
			float w1 = side * Width / Height;
			float h1 = side * Height / Width;
			NewWidth = SuppMath.RoundToUInt((side + w1) / 2);
			NewHeight = SuppMath.RoundToUInt((side + h1) / 2);
		}


		/// <summary>
		/// Масштабировать до ширины
		/// </summary>
		public void ScaleToWidth(
			uint width,
			bool noIncrease)
		{
			float r1 = (noIncrease && width >= Width)
				? 1 : (float)width / Width;
			NewWidth = width;
			NewHeight = SuppMath.RoundToUInt(Height * r1);
		}


		/// <summary>
		/// Масштабировать до высоты
		/// </summary>
		public void ScaleToHeight(
			uint height,
			bool noIncrease)
		{
			float r1 = (noIncrease && height >= Height)
				? 1 : (float)height / Height;
			NewWidth = SuppMath.RoundToUInt(Width * r1);
			NewHeight = height;
		}


		/* functions */


		/// <summary>
		/// Возвращает стартовое значение обрезки
		/// </summary>
		/// <param name="length">Исходная длина</param>
		/// <param name="crop">Длина обрезки</param>
		/// <param name="percentageOfDisplacement">
		/// Процент смещения [0..100]
		/// (0 - начало, 50 - середина, 100 - конец)
		/// </param>
		/// <returns></returns>
		public static uint GetCropStart(
			uint length,
			uint crop,
			byte percentageOfDisplacement)
		{
			if (length <= crop)
				return crop;
			uint delta1 = length - crop;
			return SuppMath.RoundToUInt(
				delta1 * SuppMath.GetRestrict(percentageOfDisplacement, 0, 100) / 100d);
		}


		/// <summary>
		/// Возвращает стартовое значение обрезки
		/// </summary>
		/// <param name="length">Исходная длина</param>
		/// <param name="crop">Длина обрезки</param>
		/// <param name="shift">Смещение</param>
		/// <returns></returns>
		public static uint GetCropStart(
			uint length,
			uint crop,
			ImageShiftEnum shift)
		{
			if (length <= crop)
				return 0;
			uint delta1 = length - crop;
			byte percentage1 = shift switch
			{
				ImageShiftEnum.Start => 0,
				ImageShiftEnum.StartMiddle => 25,
				ImageShiftEnum.EndMiddle => 75,
				ImageShiftEnum.End => 100,
				_ => 50
			};
			return SuppMath.RoundToUInt(delta1 * percentage1 / 100d);
		}

	}

}
