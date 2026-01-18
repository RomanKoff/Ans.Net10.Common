using System.Text;

namespace Ans.Net10.Common
{

	public class TypografHelper
	{

		private readonly StringBuilder _result = new();


		/* ctor */


		public TypografHelper(
			string source)
		{
			Source = source;
			_parseTagStart(0);
			Result = _result.ToString();
		}


		/* readonly properties */


		public string Source { get; }
		public string Result { get; }


		/* privates */


		private void _parseTagStart(
			int current)
		{
			var i1 = Source.IndexOf('<', current);
			if (i1 == -1)
			{
				_result.Append(
					SuppTypograph.GetTypografElem(
						Source[current..]));
				return;
			}
			_result.Append(
				SuppTypograph.GetTypografElem(
					Source[current..i1]));
			_parseTagEnd(i1);
		}


		private void _parseTagEnd(
			int current)
		{
			var i1 = Source.IndexOf('>', current);
			if (i1 == -1)
			{
				_result.Append(Source[current..]);
				return;
			}
			i1++;
			_result.Append(Source[current..i1]);
			_parseTagStart(i1);
		}

	}

}
