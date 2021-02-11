using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Security.Cryptography;

namespace AESEncryptor
{
	public partial class Window1 : Window
	{
		// start vector for first block of data
		const string vector = "1234567890123456"; // 128 bytes / 8 bytes = 16 chars
		AesCryptoServiceProvider provider;
		
		public Window1()
		{
			InitializeComponent();
			
			provider = new AesCryptoServiceProvider();
			provider.IV = Encoding.ASCII.GetBytes(vector);
			provider.KeySize = 256; // 32 chars
			provider.Mode = CipherMode.CBC;
			provider.Padding = PaddingMode.PKCS7;
		}
		
		void Encrypt(object sender, RoutedEventArgs e)
		{
			try
            {
				provider.Key = Encoding.ASCII.GetBytes(CipherKey.Text);
				ICryptoTransform trans = provider.CreateEncryptor();

				byte[] clearBytes = Encoding.Unicode.GetBytes(MyText.Text); // unicode for russian chars, for me
				byte[] bytes = trans.TransformFinalBlock(clearBytes, 0, clearBytes.Length);

				MyText.Text = Convert.ToBase64String(bytes);
			}
			catch
            {
				MyText.Text = "Error in text encrypting. Check yor key-string, only 32 chars";
            }
		}
		
		void Decrypt(object sender, RoutedEventArgs e)
		{
			try
            {
				provider.Key = Encoding.ASCII.GetBytes(CipherKey.Text);
				ICryptoTransform trans = provider.CreateDecryptor();

				byte[] cipherBytes = Convert.FromBase64String(MyText.Text);
				byte[] bytes = trans.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

				MyText.Text = Encoding.Unicode.GetString(bytes);
			}
			catch
            {
				MyText.Text = "Error in text decrypting." +
					" You can encrypt your text several times, but not decrypt more times then encrypt." +
					" And check your key-string, only 32 chars";
			}
		}
	}
}