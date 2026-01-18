using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace Ans.Net10.Common
{

	public class IPSubnetsList
		: List<IPSubnet>
	{
		public IPSubnetsList(
			params string[] cidrs)
		{
			foreach (string s1 in cidrs)
				Add(new IPSubnet(s1));
		}

		public IPSubnetsList(
			string cidrs)
			: this(cidrs.Split([';', ',']))
		{
		}

		public override string ToString()
		{
			return string.Join(";", this.Select(x => x.ToString()));
		}
	}



	public class IPSubnet
	{

		/* ctors */


		public IPSubnet(
			IPAddress address,
			int maskLength)
		{
			_init(address, maskLength);
		}


		public IPSubnet(
			string cidr)
		{
			var i1 = cidr.IndexOf('/');
			if (i1 == -1)
				throw new NotSupportedException(
					Resources.Exceptions.IPSubnet_Length);
			_init(
				IPAddress.Parse(cidr[..i1]),
				int.Parse(cidr[(i1 + 1)..]));
		}


		/* readonly properties */


		public IPAddress Address { get; private set; }
		public bool IsV6 { get; private set; }
		public int MaskLength { get; private set; }
		public uint MaskV4Template { get; private set; }
		public uint MaskV4Result { get; private set; }
		public BitArray MaskV6 { get; private set; }


		/* functions */


		public override string ToString()
		{
			return $"{Address}/{MaskLength}";
		}


		/* privates */


		private void _init(
			IPAddress address,
			int maskLength)
		{
			if (maskLength < 0)
				throw new NotSupportedException(
					Resources.Exceptions.IPSubnet_Less0);
			Address = address;
			MaskLength = maskLength;
			var a1 = Address.GetAddressBytes().Reverse().ToArray();
			switch (Address.AddressFamily)
			{
				case AddressFamily.InterNetwork:
					IsV6 = false;
					var bits1 = BitConverter.ToUInt32(a1, 0);
					MaskV4Template = uint.MaxValue << (32 - MaskLength);
					MaskV4Result = bits1 & MaskV4Template;
					break;
				case AddressFamily.InterNetworkV6:
					IsV6 = true;
					MaskV6 = new BitArray(a1);
					break;
				default:
					throw new NotSupportedException(
						Resources.Exceptions.IPSubnet_OnlyIPv4orIPv6);
			}
		}

	}

}
