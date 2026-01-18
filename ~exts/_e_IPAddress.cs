using System.Collections;
using System.Net;

namespace Ans.Net10.Common
{

	public static partial class _e_IPAddress
	{

		/* functions */


		public static bool IsInSubnet(
			this IPAddress address,
			IPSubnet subnet)
		{
			if (subnet.MaskLength == 0)
				return true;
			var a1 = address.GetAddressBytes().Reverse().ToArray();
			if (subnet.IsV6)
			{
				var bits1 = new BitArray(a1);
				var l1 = bits1.Length;
				if (subnet.MaskV6.Length != bits1.Length)
					throw new ArgumentException(
						Resources.Exceptions.IPSubnet_LengthAddressAndMaskNotMatch);
				for (var i2 = l1 - 1; i2 >= l1 - subnet.MaskLength; i2--)
					if (bits1[i2] != subnet.MaskV6[i2])
						return false;
				return true;
			}
			var bits2 = BitConverter.ToUInt32(a1, 0);
			var result2 = bits2 & subnet.MaskV4Template;
			return (subnet.MaskV4Result == result2);
		}

	}

}
