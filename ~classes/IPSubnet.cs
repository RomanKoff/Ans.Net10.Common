// rev 2026-09-18

using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Представляет список подсетей IP и методы для их инициализации из строк.
	/// </summary>
	public class IPSubnetsList
		: List<IPSubnet>
	{

		private static readonly char[] _Sep_Nets = [';', ','];


		/* ctors */


		/// <summary>
		/// Инициализирует новый экземпляр списка подсетей на основе массива CIDR-строк.
		/// </summary>
		/// <param name="cidrs">Массив строк в формате CIDR (например, "192.168.1.0/24").</param>
		public IPSubnetsList(
			params string[] cidrs)
		{
			if (cidrs == null)
				return;
			foreach (var s1 in cidrs)
			{
				if (string.IsNullOrWhiteSpace(s1))
					continue;
				Add(new IPSubnet(s1.Trim()));
			}
		}


		/// <summary>
		/// Инициализирует новый экземпляр списка подсетей на основе строки с разделителями.
		/// </summary>
		/// <param name="cidrs">Строка с перечислением CIDR, разделенная запятыми или точкой с запятой.</param>
		public IPSubnetsList(
			string cidrs)
			: this(string.IsNullOrWhiteSpace(cidrs)
				  ? [] : cidrs.Split(_Sep_Nets, StringSplitOptions.RemoveEmptyEntries))
		{
		}


		/* functions */


		/// <summary>
		/// Возвращает строковое представление списка подсетей, разделенное точкой с запятой.
		/// </summary>
		public override string ToString()
		{
			return string.Join(";", this.Select(x => x.ToString()));
		}

	}



	/// <summary>
	/// Представляет подсеть IP-адресов (IPv4 или IPv6) и инкапсулирует логику битовых масок.
	/// </summary>
	public class IPSubnet
	{

		/* ctors */


		/// <summary>
		/// Инициализирует новый экземпляр подсети по базовому IP-адресу и длине маски.
		/// </summary>
		/// <param name="address">Базовый IP-адрес подсети.</param>
		/// <param name="maskLength">Длина префикса маски в битах.</param>
		public IPSubnet(
			IPAddress address,
			int maskLength)
		{
			_init(address, maskLength);
		}


		/// <summary>
		/// Инициализирует новый экземпляр подсети на основе CIDR-строки.
		/// </summary>
		/// <param name="cidr">Строка в формате "IP/Маска" (например, "10.0.0.0/8").</param>
		/// <exception cref="NotSupportedException">Вызывается, если строка не содержит символ разделителя маски.</exception>
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


		/// <summary>
		/// Возвращает базовый IP-адрес подсети.
		/// </summary>
		public IPAddress Address { get; private set; } = null!;


		/// <summary>
		/// Возвращает признак того, является ли подсеть сетью протокола IPv6.
		/// </summary>
		public bool IsV6 { get; private set; }


		/// <summary>
		/// Возвращает длину префикса маски в битах.
		/// </summary>
		public int MaskLength { get; private set; }


		/// <summary>
		/// Возвращает битовую маску IPv4 шаблона.
		/// </summary>
		public uint MaskV4Template { get; private set; }


		/// <summary>
		/// Возвращает отфильтрованный результат базового адреса IPv4 подсети.
		/// </summary>
		public uint MaskV4Result { get; private set; }


		/// <summary>
		/// Возвращает массив байт маски для подсети IPv6.
		/// </summary>
		public byte[] MaskV6Bytes { get; private set; } = null!;


		/* functions */


		/// <summary>
		/// Возвращает строковое представление подсети в формате CIDR.
		/// </summary>
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
			Span<byte> a1 = stackalloc byte[16];
			if (!Address.TryWriteBytes(a1, out int _))
				throw new NotSupportedException(Resources.Exceptions.IPSubnet_OnlyIPv4orIPv6);
			switch (Address.AddressFamily)
			{
				case AddressFamily.InterNetwork:
					IsV6 = false;
					if (MaskLength > 32)
						throw new NotSupportedException(
							Resources.Exceptions.IPSubnet_LengthAddressAndMaskNotMatch);
					uint ipV4Int1 = BinaryPrimitives.ReadUInt32BigEndian(a1[..4]);
					MaskV4Template = MaskLength == 0
						? 0 : uint.MaxValue << (32 - MaskLength);
					MaskV4Result = ipV4Int1 & MaskV4Template;
					MaskV6Bytes = [];
					break;
				case AddressFamily.InterNetworkV6:
					IsV6 = true;
					if (MaskLength > 128)
						throw new NotSupportedException(
							Resources.Exceptions.IPSubnet_LengthAddressAndMaskNotMatch);
					MaskV6Bytes = new byte[16];
					int fullBytes1 = MaskLength / 8;
					int remainingBits1 = MaskLength % 8;
					for (int i1 = 0; i1 < fullBytes1; i1++)
						MaskV6Bytes[i1] = a1[i1];
					if (remainingBits1 > 0)
					{
						byte mask1 = (byte)(0xFF << (8 - remainingBits1));
						MaskV6Bytes[fullBytes1] = (byte)(a1[fullBytes1] & mask1);
					}
					break;
				default:
					throw new NotSupportedException(
						Resources.Exceptions.IPSubnet_OnlyIPv4orIPv6);
			}
		}

	}

}
