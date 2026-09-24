// rev 2026-09-19

namespace Ans.Net10.Common
{

	/// <summary>
	/// Объединенный контейнер стилей и классов для централизованного
	/// управления визуальным оформлением HTML-тегов.
	/// </summary>
	public class TagStyler
	{

		/* ctors */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="TagStyler"/> на основе готовых строителей классов и стилей.
		/// </summary>
		public TagStyler(
			TagClassesBuilder? classes,
			TagStylesBuilder? styles = null)
		{
			Classes = classes ?? new TagClassesBuilder();
			Styles = styles ?? new TagStylesBuilder();
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="TagStyler"/> на основе строковых определений классов и стилей.
		/// </summary>
		public TagStyler(
			string? cssClasses,
			string? cssStyles = null)
			: this(new TagClassesBuilder(cssClasses), new TagStylesBuilder(cssStyles))
		{
		}


		/// <summary>
		/// Инициализирует новый пустой экземпляр класса <see cref="TagStyler"/>.
		/// </summary>
		public TagStyler()
		{
			Classes = new TagClassesBuilder();
			Styles = new TagStylesBuilder();
		}


		/* properties */


		/// <summary>
		/// Возвращает или задает строитель CSS-классов.
		/// </summary>
		public TagClassesBuilder Classes { get; set; }


		/// <summary>
		/// Возвращает или задает строитель инлайновых CSS-стилей.
		/// </summary>
		public TagStylesBuilder Styles { get; set; }


		/* methods */


		/// <summary>
		/// Безопасно применяет базовые (оригинальные) классы и стили из другого стайлера,
		/// не затирая текущие уникальные параметры.
		/// </summary>
		/// <param name="styler">Стайлер, чьи базовые настройки необходимо перенести.</param>
		public void ApplyBase(
			TagStyler styler)
		{
			Classes.ApplyOriginal(styler.Classes.Items);
			Styles.ApplyOriginal(styler.Styles.Items);
		}

	}

}
