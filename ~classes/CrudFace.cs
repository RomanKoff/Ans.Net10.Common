// rev 2026-09-20

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Определяет интерфейс для описания визуального отображения
	/// и метаданных полей CRUD-интерфейса.
	/// </summary>
	public interface ICrudFace
	{
		/// <summary>
		/// Системное имя поля.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Основной заголовок поля.
		/// </summary>
		string Title { get; }

		/// <summary>
		/// Сокращенный заголовок поля.
		/// </summary>
		string ShortTitle { get; }

		/// <summary>
		/// Подробное описание или всплывающая подсказка поля.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Пример заполнения поля (плейсхолдер).
		/// </summary>
		string Sample { get; }

		/// <summary>
		/// Ссылка на справочную документацию по данному поле.
		/// </summary>
		string HelpLink { get; }
	}



	/// <summary>
	/// Реализация интерфейса отображения полей CRUD с поддержкой парсинга
	/// из сериализованных строк и расчетных заголовков.
	/// </summary>
	public class CrudFace
		: ICrudFace
	{

		/* ctors */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="CrudFace"/>,
		/// разбирая параметры отображения из единой строки.
		/// </summary>
		/// <param name="name">Системное имя поля.</param>
		/// <param name="face">
		/// Сериализованная строка формата "title|shortTitle|description|sample|helpLink".
		/// </param>
		public CrudFace(
			string name,
			string face)
		{
			var parser1 = new StringParser(face);
			Name = name;
			TitleRaw = parser1.Get(0) ?? string.Empty;
			ShortTitleRaw = parser1.Get(1) ?? string.Empty;
			Description = parser1.Get(2) ?? string.Empty;
			Sample = parser1.Get(3) ?? string.Empty;
			HelpLink = parser1.Get(4) ?? string.Empty;
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="CrudFace"/> с явным указанием всех параметров отображения.
		/// </summary>
		/// <param name="name">Системное имя поля.</param>
		/// <param name="title">Основной заголовок поля.</param>
		/// <param name="shortTitle">Сокращенный заголовок поля.</param>
		/// <param name="description">Подробное описание поля.</param>
		/// <param name="sample">Пример заполнения поля.</param>
		/// <param name="helpLink">Ссылка на справку.</param>
		public CrudFace(
			string name,
			string title,
			string shortTitle,
			string description,
			string sample,
			string helpLink)
		{
			Name = name;
			TitleRaw = title;
			ShortTitleRaw = shortTitle;
			Description = description;
			Sample = sample;
			HelpLink = helpLink;
		}


		/* readonly properties */


		/// <inheritdoc />
		public string Name { get; } = string.Empty;


		/// <inheritdoc />
		public string TitleRaw { get; } = string.Empty;


		/// <inheritdoc />
		public string ShortTitleRaw { get; } = string.Empty;


		/// <inheritdoc />
		public string Description { get; } = string.Empty;


		/// <inheritdoc />
		public string Sample { get; } = string.Empty;


		/// <inheritdoc />
		public string HelpLink { get; } = string.Empty;


		/// <summary>
		/// Возвращает рассчитанный заголовок. Если <see cref="Title"/>
		/// не задан, возвращает системное <see cref="Name"/>.
		/// </summary>
		public string Title
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => HasTitle
				? TitleRaw : Name;
		}


		/// <summary>
		/// Возвращает рассчитанный короткий заголовок.
		/// Если он не задан, возвращает <see cref="Title"/>.
		/// </summary>
		public string ShortTitle
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => HasShortTitle
				? ShortTitleRaw : Title;
		}


		/// <summary>
		/// Признак наличия основного заголовка.
		/// </summary>
		public bool HasTitle
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => !string.IsNullOrEmpty(TitleRaw);
		}


		/// <summary>
		/// Признак наличия короткого заголовка.
		/// </summary>
		public bool HasShortTitle
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => !string.IsNullOrEmpty(ShortTitleRaw);
		}


		/// <summary>
		/// Признак наличия описания поля.
		/// </summary>
		public bool HasDescription
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => !string.IsNullOrEmpty(Description);
		}


		/// <summary>
		/// Признак наличия примера заполнения.
		/// </summary>
		public bool HasSample
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => !string.IsNullOrEmpty(Sample);
		}


		/// <summary>
		/// Признак наличия ссылки на документацию.
		/// </summary>
		public bool HasHelpLink
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => !string.IsNullOrEmpty(HelpLink);
		}


		/// <summary>
		/// Признак наличия хотя бы одного заполненного визуального параметра отображения.
		/// </summary>
		public bool HasFace
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => HasTitle
				|| HasShortTitle
				|| HasDescription
				|| HasSample
				|| HasHelpLink;
		}


		/* functions */


		/// <summary>
		/// Формирует сериализованную строку параметров отображения поля
		/// в формате "title|shortTitle|description|sample|helpLink".
		/// </summary>
		/// <returns>Строка параметров с разделителями.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return $"{TitleRaw}|{ShortTitleRaw}|{Description}|{Sample}|{HelpLink}";
		}

	}

}
