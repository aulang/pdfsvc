using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Signatures;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using pdfsvc.Core;
using System.IO;

namespace pdfsvc.Business
{
    public class FdfManager
    {
        private readonly SignInfo _signInfo;

        private readonly ImageData _stamper;
        private readonly X509Certificate[] _chain;
        private readonly ICipherParameters _privateKey;

        public FdfManager(IOptions<SignInfo> options)
        {
            _signInfo = options.Value;

            // 盖章图片
            _stamper = ImageDataFactory.Create(_signInfo.Stamper);

            using FileStream fs = new FileStream(_signInfo.Cert, FileMode.Open);
            Pkcs12Store store = new Pkcs12Store(fs, _signInfo.Password.ToCharArray());

            X509CertificateEntry[] entries = store.GetCertificateChain(_signInfo.Alias);

            int length = entries.Length;
            _chain = new X509Certificate[entries.Length];
            for (int i = 0; i != length; ++i)
            {
                _chain[i] = entries[i].Certificate;
            }

            _privateKey = store.GetKey(_signInfo.Alias).Key;
        }

        public int Sign(Stream pdf, Stream outPdf, StampInfo stampInfo)
        {
            using PdfReader reader = new PdfReader(pdf);
            StampingProperties properties = new StampingProperties().UseAppendMode();

            ExPdfSigner signer = new ExPdfSigner(reader, outPdf, stampInfo, _stamper, properties)
                .SetOpacity(_signInfo.Opacity);

            PrivateKeySignature signature = new PrivateKeySignature(_privateKey, DigestAlgorithms.SHA256);

            return signer.Sign(signature,
                    _chain,
                    null,
                    null,
                    null,
                    0,
                    PdfSigner.CryptoStandard.CMS);
        }
    }
}