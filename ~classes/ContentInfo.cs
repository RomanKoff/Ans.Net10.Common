// rev 2026-09-16

using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Определяет категории (группы) типов контента.
	/// </summary>
	public enum ContentGroupEnum
	{
		/// <summary>
		/// Архивные файлы (например: zip, rar, tar).
		/// </summary>
		Archive,

		/// <summary>
		/// Аудиофайлы (например: mp3, wav, ogg).
		/// </summary>
		Audio,

		/// <summary>
		/// Бинарные и исполняемые файлы (например: exe, dll, bin).
		/// </summary>
		Bin,

		/// <summary>
		/// Файлы исходного кода (например: cs, js, cpp, html).
		/// </summary>
		Code,

		/// <summary>
		/// Документы и офисные форматы (например: docx, pdf, xlsx).
		/// </summary>
		Document,

		/// <summary>
		/// Файлы шрифтов (например: ttf, otf, woff).
		/// </summary>
		Font,

		/// <summary>
		/// Изображения и графические файлы (например: png, jpeg, svg).
		/// </summary>
		Image,

		/// <summary>
		/// Простые текстовые файлы (например: txt, log, csv).
		/// </summary>
		Text,

		/// <summary>
		/// Видеофайлы (например: mp4, mkv, avi).
		/// </summary>
		Video,
	}



	/// <summary>
	/// Предоставляет структурированную информацию о типе контента, 
	/// включая MIME-тип, расширение файла и категорию контента.
	/// </summary>
	public class ContentInfo
	{

		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ContentInfo"/>.
		/// </summary>
		/// <param name="extension">Расширение файла (например, ".jpg").</param>
		/// <param name="contentType">Строковое представление MIME-типа (например, "image/jpeg").</param>
		/// <param name="group">Группа, к которой относится данный контент.</param>
		/// <param name="isWebImage">Флаг, указывающий, является ли изображение оптимизированным для Web.</param>
		/// <param name="isJpeg">Флаг, указывающий, является ли изображение форматом JPEG.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ContentInfo(
			string extension,
			string contentType,
			ContentGroupEnum group,
			bool isWebImage = false,
			bool isJpeg = false)
		{
			Extension = extension;
			ContentType = contentType;
			Group = group;
			IsImage = group == ContentGroupEnum.Image;
			IsWebImage = isWebImage;
			IsJpeg = isJpeg;
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает расширение файла, связанное с этим контентом.
		/// </summary>
		public string Extension { get; }


		/// <summary>
		/// Возвращает MIME-тип контента.
		/// </summary>
		public string ContentType { get; }


		/// <summary>
		/// Возвращает группу контента (категорию).
		/// </summary>
		public ContentGroupEnum Group { get; }


		/// <summary>
		/// Возвращает значение, указывающее, относится ли контент к группе изображений.
		/// </summary>
		public bool IsImage { get; }


		/// <summary>
		/// Возвращает значение, указывающее, является ли изображение стандартным
		/// для использования в Web (например: png, jpeg, gif, webp).
		/// </summary>
		public bool IsWebImage { get; }


		/// <summary>
		/// Возвращает значение, указывающее, является ли изображение форматом JPEG.
		/// </summary>
		public bool IsJpeg { get; }


		/// <summary>
		/// Возвращает объект заголовка типа медиа, созданный на основе <see cref="ContentType"/>.
		/// В случае некорректного формата возвращает дефолтный "application/octet-stream".
		/// </summary>
		public MediaTypeHeaderValue MediaType
		{
			get
			{
				_mediaType ??= MediaTypeHeaderValue.TryParse(ContentType, out var parsed1)
					? parsed1
					: new MediaTypeHeaderValue("application/octet-stream");
				return _mediaType;
			}
		}
		private MediaTypeHeaderValue? _mediaType;


		/* functions */


		/// <summary>
		/// Возвращает строковое представление текущего объекта информации о контенте.
		/// </summary>
		/// <returns>Строка с ключевыми параметрами контента.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return $"[{Group}] '{ContentType}' IsImage:{IsImage} IsWebImage:{IsWebImage} IsJpeg:{IsJpeg}";
		}

	}

}
