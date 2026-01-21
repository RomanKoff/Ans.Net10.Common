namespace Ans.Net10.Common
{

	public class TagClassBuilder
	{

		private readonly List<string> _classes = [];


		/* ctors */


		public TagClassBuilder()
		{
		}


		public TagClassBuilder(
			params string[] cssClasses)
			: this()
		{
			foreach (var s1 in cssClasses)
				Append(s1);
		}


		/* readonly properties */


		public int Count
			=> _classes.Count;


		/* functions */


		private string _join;
		public override string ToString()
		{
			if (_join != null)
				return _join;
			var s1 = string.Join(" ", _classes);
			return _join ??= (string.IsNullOrEmpty(s1)) ? null : s1;
		}


		public string GetJoin()
		{
			_join = null;
			return ToString();
		}


		public int IndexOf(
			string cssClass)
		{
			return _classes.IndexOf(cssClass);
		}


		public bool IsExists(
			string cssClass)
		{
			return IndexOf(cssClass) > 0;
		}


		/* methods */


		public void Append(
			string cssClass)
		{
			if (!string.IsNullOrEmpty(cssClass)
				&& !IsExists(cssClass))
				foreach (var s1 in cssClass.Split(" ",
					StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
					_classes.Add(s1);
		}


		public void AppendIf(
			bool check,
			string cssClass)
		{
			if (check)
				Append(cssClass);
		}


		public void AppendTemplate(
			string template,
			params object[] args)
		{
			Append(string.Format(template, args));
		}


		public void Switch(
			string cssClass)
		{
			if (string.IsNullOrEmpty(cssClass))
				return;
			var i1 = IndexOf(cssClass);
			if (i1 > 0)
				_classes.RemoveAt(i1);
			else
				_classes.Add(cssClass);
		}


		public void Replace(
			string oldCssClass,
			string newCssClass)
		{
			Remove(oldCssClass);
			Append(newCssClass);
		}


		public void Remove(
			string cssClass)
		{
			var i1 = IndexOf(cssClass);
			if (i1 > 0)
				_classes.RemoveAt(i1);
		}


		public void Clear()
		{
			_classes.Clear();
		}

	}

}
