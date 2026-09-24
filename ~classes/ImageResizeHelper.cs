// rev 2026-09-18

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Определяет пространственную ориентацию изображения на основе соотношения его сторон.
	/// </summary>
	public enum ImageOrientationEnum
		: int
	{
		/// <summary>
		/// Ориентация не определена.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Альбомная (горизонтальная) ориентация.
		/// </summary>
		Landscape = 1,

		/// <summary>
		/// Книжная (вертикальная) ориентация.
		/// </summary>
		Portrait = 2,

		/// <summary>
		/// Квадратное изображение.
		/// </summary>
		Square = 3
	}



	/// <summary>
	/// Определяет варианты смещения рамки кадрирования (обрезки) относительно сторон изображения.
	/// </summary>
	public enum ImageShiftEnum : int
	{
		/// <summary>
		/// По центру.
		/// </summary>
		Center = 0,

		/// <summary>
		/// От начала (левый/верхний край).
		/// </summary>
		Start = 1,

		/// <summary>
		/// Смещение к началу (на 25%).
		/// </summary>
		StartMiddle = 2,

		/// <summary>
		/// Смещение к концу (на 75%).
		/// </summary>
		EndMiddle = 3,

		/// <summary>
		/// От конца (правый/нижний край).
		/// </summary>
		End = 4
	}



	/// <summary>
	/// Вспомогательный класс для расчета новых пропорций, коэффициентов масштабирования
	/// и координат кадрирования (обрезки) изображений.
	/// </summary>
	public class ImageResizeHelper
	{

		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр хелпера на основе исходных размеров изображения.
		/// </summary>
		/// <param name="width">Исходная ширина изображения в пикселях.</param>
		/// <param name="height">Исходная высота изображения в пикселях.</param>
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
				IsNearSquare = Ratio < 1.1f && Ratio > 0.9f;
				Orientation = Ratio > 1.0f
					? ImageOrientationEnum.Landscape
					: ImageOrientationEnum.Portrait;
			}
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает исходную ширину изображения.
		/// </summary>
		public uint Width { get; }


		/// <summary>
		/// Возвращает исходную высоту изображения.
		/// </summary>
		public uint Height { get; }


		/// <summary>
		/// Возвращает соотношение сторон изображения (Ширина / Высота).
		/// </summary>
		public float Ratio { get; }


		/// <summary>
		/// Возвращает пространственную ориентацию изображения.
		/// </summary>
		public ImageOrientationEnum Orientation { get; }


		/// <summary>
		/// Возвращает признак того, что изображение близко к квадратному формату.
		/// </summary>
		public bool IsNearSquare { get; }


		/// <summary>
		/// Возвращает рассчитанную новую ширину изображения после вызова методов масштабирования.
		/// </summary>
		public uint NewWidth { get; private set; }


		/// <summary>
		/// Возвращает рассчитанную новую высоту изображения после вызова методов масштабирования.
		/// </summary>
		public uint NewHeight { get; private set; }


		/* methods */


		/// <summary>
		/// Масштабирует изображение по заданному коэффициенту.
		/// </summary>
		/// <param name="ratio">Коэффициент масштабирования (например, 0.5f для уменьшения вдвое).</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Scale(
			float ratio)
		{
			NewWidth = SuppMath.RoundToUInt(Width * ratio);
			NewHeight = SuppMath.RoundToUInt(Height * ratio);
		}


		/// <summary>
		/// Пропорционально масштабирует изображение, чтобы оно полностью вписалось внутрь указанных границ.
		/// </summary>
		/// <param name="width">Максимально допустимая ширина.</param>
		/// <param name="height">Максимально допустимая высота.</param>
		/// <param name="noIncrease">
		/// Если <see langword="true"/>, то маленькое изображение не будет увеличиваться,
		/// если границы больше него.
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScaleInside(
			float width,
			float height,
			bool noIncrease)
		{
			float r1 = (noIncrease && width >= Width && height >= Height)
				? 1.0f : Math.Min(width / Width, height / Height);
			Scale(r1);
		}


		/// <summary>
		/// Пропорционально масштабирует изображение так, чтобы оно полностью
		/// заполнило указанные границы снаружи (с возможным выходом за края).
		/// </summary>
		/// <param name="width">Целевая ширина рамки.</param>
		/// <param name="height">Целевая высота рамки.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScaleAround(
			float width,
			float height)
		{
			float r1 = Math.Max(width / Width, height / Height);
			Scale(r1);
		}


		/// <summary>
		/// Масштабирует изображение методом усреднения пропорций вокруг заданной стороны.
		/// </summary>
		/// <param name="side">Размер целевой стороны.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScaleAverage(
			uint side)
		{
			float w1 = side * (float)Width / Height;
			float h1 = side * (float)Height / Width;
			NewWidth = SuppMath.RoundToUInt((side + w1) / 2.0f);
			NewHeight = SuppMath.RoundToUInt((side + h1) / 2.0f);
		}


		/// <summary>
		/// Масштабирует изображение до достижения заданной ширины с сохранением исходных пропорций высоты.
		/// </summary>
		/// <param name="width">Целевая ширина.</param>
		/// <param name="noIncrease">
		/// Если <see langword="true"/>, изображение не будет увеличиваться,
		/// если целевая ширина больше исходной.
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScaleToWidth(
			uint width,
			bool noIncrease)
		{
			float r1 = (noIncrease && width >= Width)
				? 1.0f : (float)width / Width;
			NewWidth = SuppMath.RoundToUInt(Width * r1);
			NewHeight = SuppMath.RoundToUInt(Height * r1);
		}


		/// <summary>
		/// Масштабирует изображение до достижения заданной высоты с сохранением исходных пропорций ширины.
		/// </summary>
		/// <param name="height">Целевая высота.</param>
		/// <param name="noIncrease">
		/// Если <see langword="true"/>, изображение не будет увеличиваться,
		/// если целевая высота больше исходной.
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScaleToHeight(
			uint height,
			bool noIncrease)
		{
			float r1 = (noIncrease && height >= Height)
				? 1.0f : (float)height / Height;
			NewWidth = SuppMath.RoundToUInt(Width * r1);
			NewHeight = SuppMath.RoundToUInt(Height * r1);
		}


		/* functions */


		/// <summary>
		/// Возвращает стартовую координату (пиксель) для начала кадрирования на основе процентного смещения.
		/// </summary>
		/// <param name="length">Исходный размер стороны изображения (ширина или высота).</param>
		/// <param name="crop">Целевой размер рамки обрезки.</param>
		/// <param name="percentageOfDisplacement">
		/// Процент смещения от 0 (начало) до 100 (конец). Значение 50 означает центр.
		/// </param>
		/// <returns>Стартовая координата начала обрезки.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint GetCropStart(
			uint length,
			uint crop,
			byte percentageOfDisplacement)
		{
			if (length <= crop)
				return 0;
			uint delta1 = length - crop;
			byte safePercent1 = SuppMath.GetRestrict(percentageOfDisplacement, (byte)0, (byte)100);
			return SuppMath.RoundToUInt(delta1 * (safePercent1 / 100.0f));
		}


		/// <summary>
		/// Возвращает стартовую координату (пиксель) для начала кадрирования
		/// на основе перечисления вариантов смещения.
		/// </summary>
		/// <param name="length">Исходный размер стороны изображения (ширина или высота).</param>
		/// <param name="crop">Целевой размер рамки обрезки.</param>
		/// <param name="shift">Вариант смещения рамки из перечисления <see cref="ImageShiftEnum"/>.</param>
		/// <returns>Стартовая координата начала обрезки.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint GetCropStart(
			uint length,
			uint crop,
			ImageShiftEnum shift)
		{
			if (length <= crop)
				return 0;
			uint delta1 = length - crop;
			float percentage1 = shift switch
			{
				ImageShiftEnum.Start => 0.0f,
				ImageShiftEnum.StartMiddle => 0.25f,
				ImageShiftEnum.EndMiddle => 0.75f,
				ImageShiftEnum.End => 1.0f,
				_ => 0.5f // ImageShiftEnum.Center
			};
			return SuppMath.RoundToUInt(delta1 * percentage1);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return $"[{Width}:{Height}]->[{NewWidth}:{NewHeight}]";
		}

	}

}
