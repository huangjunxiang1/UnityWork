using System.Text;

public static class ByteHelper
{
	public static void WriteTo(this byte[] bytes, int offset, uint num)
	{
		bytes[offset] = (byte)(num & 0xff);
		bytes[offset + 1] = (byte)((num & 0xff00) >> 8);
		bytes[offset + 2] = (byte)((num & 0xff0000) >> 16);
		bytes[offset + 3] = (byte)((num & 0xff000000) >> 24);
	}

	public static void WriteTo(this byte[] bytes, int offset, int num)
	{
		bytes[offset] = (byte)(num & 0xff);
		bytes[offset + 1] = (byte)((num & 0xff00) >> 8);
		bytes[offset + 2] = (byte)((num & 0xff0000) >> 16);
		bytes[offset + 3] = (byte)((num & 0xff000000) >> 24);
	}

	public static void WriteTo(this byte[] bytes, int offset, byte num)
	{
		bytes[offset] = num;
	}

	public static void WriteTo(this byte[] bytes, int offset, short num)
	{
		bytes[offset] = (byte)(num & 0xff);
		bytes[offset + 1] = (byte)((num & 0xff00) >> 8);
	}

	public static void WriteTo(this byte[] bytes, int offset, ushort num)
	{
		bytes[offset] = (byte)(num & 0xff);
		bytes[offset + 1] = (byte)((num & 0xff00) >> 8);
	}

	public static unsafe void WriteTo(this byte[] bytes, int offset, long num)
	{
		byte* bPoint = (byte*)&num;
		for (int i = 0; i < sizeof(long); ++i)
		{
			bytes[offset + i] = bPoint[i];
		}
	}
}