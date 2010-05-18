namespace RSANameSpace.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RSA
    {
        private const int CONFIDENCE_VALUE = NUMBER_OF_BITS/2;
        private const int NUMBER_OF_BITS = 512;
        private const int NUMBER_OF_BITS_IN_BYTE = 8;
        private BigInteger p, q, n, eulerOfN, e, d;

        public RSA()
        {
            Random randomGenerator = new Random(DateTime.Now.Millisecond);

            this.p = BigInteger.genPseudoPrime(NUMBER_OF_BITS, CONFIDENCE_VALUE, randomGenerator);
            this.q = BigInteger.genPseudoPrime(NUMBER_OF_BITS, CONFIDENCE_VALUE, randomGenerator);
            
            this.n = this.p * this.q;
            this.eulerOfN = (this.p - 1) * (this.q - 1);

            do
            {
                this.e = this.eulerOfN.genCoPrime(randomGenerator.Next(1, this.eulerOfN.bitCount()), randomGenerator);
            }
            while ((this.e > this.eulerOfN) && this.e.gcd(this.eulerOfN) > 1);

            this.d = e.modInverse(eulerOfN);
        }

        public RSA(BigInteger p, BigInteger q)
        {
            if (!p.isProbablePrime())
            {
                throw new Exception("Podana przez Ciebie liczba p prawdopodobnie nie jest liczbą pierwszą!");
            }

            if (!q.isProbablePrime())
            {
                throw new Exception("Podana przez Ciebie liczba q prawdopodobnie nie jest liczbą pierwszą!");
            }

            this.p = p;
            this.q = q;
            this.n = this.p * this.q;
            this.eulerOfN = (this.p - 1) * (this.q - 1);
            Random randomGenerator = new Random(DateTime.Now.Millisecond);
            
            do
            {
                this.e = this.eulerOfN.genCoPrime(randomGenerator.Next(1, this.eulerOfN.bitCount()), randomGenerator);
            }
            while ((this.e > this.eulerOfN) && this.e.gcd(this.eulerOfN) > 1);
            
            this.d = e.modInverse(eulerOfN);
        }

        public BigInteger N
        {
            get
            {
                return this.n;
            }
        }

        public BigInteger E
        {
            get
            {
                return this.e;
            }

            set { this.e = value; }
        }

        public BigInteger P
        {
            get
            {
                return this.p;
            }
        }

        public BigInteger Q
        {
            get
            {
                return this.q;
            }
        }

        public BigInteger D
        {
            get
            {
                return this.d;
            }

            set { this.d = value; }
        }

        
        public int MaxCountOfBytes
        {
            get
            {
                return 40;
            }
        }

        public string PublicKey
        {
            get
            {
                return "[" + this.e + "," + this.n + "]";
            }
        }

        public BigInteger Cipher(BigInteger m)
        {
            return m.modPow(this.e, this.n);
        }

        public BigInteger Encipher(BigInteger c)
        {
            return c.modPow(this.d, this.n);
        }
    }
}
