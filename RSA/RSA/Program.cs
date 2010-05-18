namespace RSANameSpace
{
    using RSANameSpace.Domain;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            string
                  fileNameIn = "zo_attack1.wav"
                , fileNameInOut = "temp.wav"
                , fileNameOut = "!zo_attack1.wav";

            RSA rsa = new RSA();

            FileStream inputFileStream, outputFileStream;

            // szyfrowanie
            using (inputFileStream = new FileStream(@fileNameIn, FileMode.Open))
            {
                using (outputFileStream = new FileStream(@fileNameInOut, FileMode.Create))
                {
                    byte[] inputBytes = new byte[rsa.MaxCountOfBytes];
                    inputBytes[0] = 0x80;

                    int nuberOfBytes;
                    while ((nuberOfBytes = inputFileStream.Read(inputBytes, 1, rsa.MaxCountOfBytes - 1)) != 0)
                    {
                        BigInteger inputBlock = new BigInteger(inputBytes, nuberOfBytes + 1);
                        BigInteger cipheredBlock = rsa.Cipher(inputBlock);

                        BigInteger encipheredBlock = rsa.Encipher(new BigInteger(cipheredBlock.getBytes(), cipheredBlock.getBytes().Length));

                        if (!encipheredBlock.Equals(inputBlock))
                        {
                            throw new Exception();
                        }

                        outputFileStream.WriteByte((byte)(nuberOfBytes));
                        outputFileStream.WriteByte((byte)(cipheredBlock.getBytes().Length));
                        outputFileStream.Write(cipheredBlock.getBytes(), 0, cipheredBlock.getBytes().Length);
                    }
                }
            }

            // deszyfrowanie
            using (inputFileStream = new FileStream(@fileNameInOut, FileMode.Open))
            {
                using (outputFileStream = new FileStream(@fileNameOut, FileMode.Create))
                {
                    byte[] inputBytes = new byte[rsa.N.getBytes().Length];

                    do
                    {
                        int plainTextBytes = inputFileStream.ReadByte();
                        if (plainTextBytes == -1)
                        {
                            break;
                        }

                        int cipheredBytesLenght = inputFileStream.ReadByte();

                        BigInteger inputBlock = inputFileStream.Read(inputBytes, 0, cipheredBytesLenght);
                        BigInteger encipheredBlock = rsa.Encipher(inputBlock);
                        outputFileStream.Write(encipheredBlock.getBytes(), 1, plainTextBytes);
                    }
                    while (true);
                }
            }
        }
    }
}
