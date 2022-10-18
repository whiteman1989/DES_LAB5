using System.Collections;
using System.Text;

namespace DES_LAB5;

public class DesAlgorithm
{
	public static readonly Encoding ansii = Encoding.ASCII;

	private static readonly int[] IpTable =
	{
		58, 50, 42, 34, 26, 18, 10, 2,
		60, 52, 44, 36, 28, 20, 12, 4,
		62, 54, 46, 38, 30, 22, 14, 6,
		64, 56, 48, 40, 32, 24, 16, 8,
		57, 49, 41, 33, 25, 17, 9, 1,
		59, 51, 43, 35, 27, 19, 11, 3,
		61, 53, 45, 37, 29, 21, 13, 5,
		63, 55, 47, 39, 31, 23, 15, 7
	};

	private static readonly int[] PC_1 =
	{
		57, 49, 41, 33, 25, 17, 9,
		1, 58, 50, 42, 34, 26, 18,
		10, 2, 59, 51, 43, 35, 27,
		19, 11, 3, 60, 52, 44, 36,
		63, 55, 47, 39, 31, 23, 15,
		7, 62, 54, 46, 38, 30, 22,
		14, 6, 61, 53, 45, 37, 29,
		21, 13, 5, 28, 20, 12, 4
	};

	private static readonly int[] PC_2 =
	{
		14, 17, 11, 24, 1, 5,
		3, 28, 15, 6, 21, 10,
		23, 19, 12, 4, 26, 8,
		16, 7, 27, 20, 13, 2,
		41, 52, 31, 37, 47, 55,
		30, 40, 51, 45, 33, 48,
		44, 49, 39, 56, 34, 53,
		46, 42, 50, 36, 29, 32
	};

	private static readonly int[] EFn =
	{
		32, 1, 2, 3, 4, 5,
		4, 5, 6, 7, 8, 9,
		8, 9, 10, 11, 12, 13,
		12, 13, 14, 15, 16, 17,
		16, 17, 18, 19, 20, 21,
		20, 21, 22, 23, 24, 25,
		24, 25, 26, 27, 28, 29,
		28, 29, 30, 31, 32, 1
	};

	private static readonly int[,] S1 =
	{
		{ 4, 4, 3, 1, 2, 5, 1, 8, 3, 0, 6, 2, 5, 9, 0, 7 },
		{ 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
		{ 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
		{ 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13, }
	};

	private static readonly int[,] S2 =
	{
		{ 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
		{ 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
		{ 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
		{ 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
	};

	private static readonly int[,] S3 =
	{
		{ 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8, },
		{ 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
		{ 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7, },
		{ 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12, }
	};

	private static readonly int[,] S4 =
	{
		{ 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15, },
		{ 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9, },
		{ 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
		{ 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
	};

	private static readonly int[,] S5 =
	{
		{ 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
		{ 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6, },
		{ 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14, },
		{ 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
	};

	private static readonly int[,] S6 =
	{
		{ 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11, },
		{ 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8, },
		{ 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6, },
		{ 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }

	};

	private static readonly int[,] S7 =
	{
		{ 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
		{ 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
		{ 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
		{ 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
	};

	private static readonly int[,] S8 =
	{
		{ 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
		{ 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
		{ 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
		{ 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
	};

	private static readonly int[] P =
	{
		16, 7, 20, 21,
		29, 12, 28, 17,
		1, 15, 23, 26,
		5, 18, 31, 10,
		2, 8, 24, 14,
		32, 27, 3, 9,
		19, 13, 30, 6,
		22, 11, 4, 25
	};

	private static readonly int[] IPRevers =
	{
		40, 8, 48, 16, 56, 24, 64, 32,
		39, 7, 47, 15, 55, 23, 63, 31,
		38, 6, 46, 14, 54, 22, 62, 30,
		37, 5, 45, 13, 53, 21, 61, 29,
		36, 4, 44, 12, 52, 20, 60, 28,
		35, 3, 43, 11, 51, 19, 59, 27,
		34, 2, 42, 10, 50, 18, 58, 26,
		33, 1, 41, 9, 49, 17, 57, 25
	};

private static readonly string Key = "password";

	private BitArray TransformIpBlock(BitArray arr)
	{
		var res = new BitArray(IpTable.Length);
		for (int i = 0; i < IpTable.Length; i++)
		{
			res[i] = arr[IpTable[i]-1];
		}

		return res;
	}

	private BitArray TransformIpRevBlock(BitArray arr)
	{
		var res = new BitArray(IPRevers.Length);
		for (int i = 0; i < IPRevers.Length; i++)
		{
			res[i] = arr[IPRevers[i]-1];
		}

		return res;
	}

	private BitArray Sfn(BitArray arr, int[,] s)
	{
		if (arr.Length != 6)
		{
			throw new ArgumentException("Argument length shall be at most 6 bits.");	
		}

		var hBits = new BitArray(2)
		{
			[0] = arr[0],
			[1] = arr[5]
		};
		var h = GetIntFromBitArray(hBits);

		var wBits = new BitArray(4)
		{
			[0] = arr[1],
			[1] = arr[2],
			[2] = arr[3],
			[3] = arr[4]
		};
		var w = GetIntFromBitArray(wBits);

		var rNum = s[h, w];
		return GetBitsFromInt(rNum);
	}

	private (BitArray r, BitArray l) Round(BitArray arrR, BitArray arrL, BitArray k)
	{
		var eRes = ApplEfn(arrR);
		var eFnResAndKey = eRes.Xor(k);
		var sRes = new BitArray(32);
		int[][,] sFns =
		{
			S1, S2, S3, S4, S5, S6, S7, S8
		};
		for (int i = 0; i < 8; i++)
		{
			var s = SliceBitArray(eFnResAndKey, i * 6, 6);
			var sr = Sfn(s, sFns[i]);
			for (int j = 0; j < 4; j++)
			{
				sRes[i * 4 + j] = s[j];
			}
		}

		var pRes = Pfn(sRes);
		var xRes = pRes.Xor(arrL);
		return new (arrL, xRes);
	}

	private BitArray Pfn(BitArray arr)
	{
		var res = new BitArray(P.Length);
		for (int i = 0; i < P.Length; i++)
		{
			res[i] = arr[P[i]-1];
		}

		return res;
	}

	private BitArray SliceBitArray(BitArray arr, int start, int count)
	{
		var res = new BitArray(count);
		for (int i = start; i < count + start; i++)
		{
			res[i - start] = arr[i];
		}

		return res;
	}

	private BitArray TransformKey(BitArray arr)
	{
		var res = new BitArray(PC_1.Length);
		for (int i = 0; i < PC_1.Length; i++)
		{
			res[i] = arr[PC_1[i]-1];
		}

		return res;
	}
	
	private BitArray Transform2Key(BitArray arr)
	{
		var res = new BitArray(PC_2.Length);
		for (int i = 0; i < PC_2.Length; i++)
		{
			res[i] = arr[PC_2[i]-1];
		}

		return res;
	}

	private BitArray ApplEfn(BitArray arr)
	{
		var res = new BitArray(EFn.Length);
		for (int i = 0; i < EFn.Length; i++)
		{
			res[i] = arr[EFn[i]-1];
		}

		return res;
	}

	private BitArray StringToBites(string text)
	{
		return new BitArray(ansii.GetBytes(text));
	}

	private BitArray GetRight(BitArray arr)
	{
		var a = new byte[8];
		arr.CopyTo(a, 0);
		var r = a.Skip(4).ToArray();
		return new BitArray(r);
	}
	
	private BitArray GetLeft(BitArray arr)
	{
		var a = new byte[8];
		arr.CopyTo(a, 0);
		var l = a.Take(4).ToArray();
		return new BitArray(l);
	}

	private BitArray GetC1(BitArray array)
	{
		var res = new BitArray(28);
		for (int i = 0; i < 28; i++)
		{
			res.Set(i, array.Get(i));
		}

		return res.LeftShift(1);
	}
	
	private int GetIntFromBitArray(BitArray bitArray)
	{

		if (bitArray.Length > 32)
			throw new ArgumentException("Argument length shall be at most 32 bits.");

		int[] array = new int[1];
		bitArray.CopyTo(array, 0);
		return array[0];

	}

	private BitArray GetBitsFromInt(int val)
	{
		return new BitArray(BitConverter.GetBytes(val));
	}

	private BitArray GetD1(BitArray array)
	{
		var res = new BitArray(28);
		for (int i = 28; i < 56; i++)
		{
			res.Set(i-28, array.Get(i));
		}
		return res.LeftShift(1);
	}

	private BitArray GetShiftedKey(BitArray array)
	{
		var c1 = GetC1(array);
		var d1 = GetD1(array);
		var res = new BitArray(56);
		for (int i = 0; i < 28; i++)
		{
			res.Set(i, array.Get(i));
		}
		for (int i = 28; i < 56; i++)
		{
			res.Set(i, array.Get(i-28));
		}

		return res;
	}

	public static byte[] BitArrayToByteArray(BitArray bits)
	{
		byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
		bits.CopyTo(ret, 0);
		return ret;
	}
	
	public string Encode(string text)
	{
		var textBites = StringToBites(text);
		var transformedBites = TransformIpBlock(textBites);

		BitArray left = GetLeft(transformedBites);
		BitArray right = GetRight(transformedBites);
		var keyBites = StringToBites(Key);
		BitArray tKey = TransformKey(keyBites);
		BitArray key = GetShiftedKey(tKey);
		BitArray p2Key = Transform2Key(key);
		var roundRes = Round(right, left, p2Key);
		var rl = new BitArray(64);
		for (int i = 0; i < 32; i++)
		{
			rl[i] = roundRes.l[i];
		}
		for (int i = 32; i < 64; i++)
		{
			rl[i] = roundRes.r[i-32];
		}

		var res = TransformIpRevBlock(rl);
		return ToBitString(res);
	}
	
	public static string ToBitString(BitArray bits)
	{
		var sb = new StringBuilder();

		for (int i = 0; i < bits.Count; i++)
		{
			char c = bits[i] ? '1' : '0';
			sb.Append(c);
		}

		return sb.ToString();
	}
}