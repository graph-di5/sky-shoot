using System.Security.Cryptography;
using System.Text;

namespace SkyShoot.WinFormsClient
{
	public class HashHelper
	{
		/// <summary>
		/// ����� ������������������ �� 32 ����������������� ���� (md5 ��� �� ���������)
		/// </summary>
		public static string GetMd5Hash(string input)
		{
			MD5 md5Hasher = MD5.Create();

			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

			var sBuilder = new StringBuilder();

			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			return sBuilder.ToString();
		}
	}
}