using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Pdf.Xobject;
using iText.Signatures;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using static iText.Signatures.PdfSignatureAppearance;

namespace pdfsvc.Core
{
    public class ExPdfSigner : PdfSigner
    {
        public const float MARGIN = 2;

        private float opacity = 1.0f;

        private readonly StampInfo _stampInfo;
        private readonly ImageData _stamper;

        public ExPdfSigner(PdfReader reader,
                       Stream outputStream,
                       StampInfo stampInfo,
                       ImageData stamper,
                       StampingProperties properties) : base(reader, outputStream, properties)
        {
            _stampInfo = stampInfo;
            _stamper = stamper;
        }

        public ExPdfSigner SetOpacity(float opacity)
        {
            this.opacity = opacity;
            return this;
        }

        public int Sign(IExternalSignature signature,
                        X509Certificate[] chain,
                        Collection<ICrlClient> crlList,
                        IOcspClient ocspClient,
                        ITSAClient tsaClient,
                        int estimatedSize,
                        CryptoStandard sigType)
        {
            List<IPdfTextLocation> locations = RegexLocationUtils.ExtractLocation(document, _stampInfo);

            if (locations.Count > 0)
            {
                // log.warn("没有找到要求的盖章位置：{}", _stampInfo);
                return 0;
            }

            DateTime dateTime = DateTime.Now;
            if (_stampInfo.SignDate != 0)
            {
                dateTime = new DateTime(_stampInfo.SignDate);
            }
            SetSignDate(dateTime);

            appearance
                .SetReason(_stampInfo.Reason)
                .SetLocation(_stampInfo.Location)
                .SetSignatureCreator(_stampInfo.Creator)
                .SetContact(_stampInfo.Contact)
                .SetSignatureGraphic(_stamper)
                .SetRenderingMode(RenderingMode.GRAPHIC);

            int index = locations.Count - 1;
            if (!_stampInfo.Latest)
            {
                index = 0;
            }
            IPdfTextLocation location = locations[index];

            Rectangle locationRectangle = location.GetRectangle();
            float locationX = locationRectangle.GetX();
            float locationY = locationRectangle.GetY();
            float locationWidth = locationRectangle.GetWidth();
            float locationHeight = locationRectangle.GetHeight();

            float imgWidth = _stamper.GetWidth();
            float imgHeight = _stamper.GetHeight();


            float x = locationX + (locationWidth - imgWidth) / 2;
            float y = locationY + (locationHeight - imgHeight) / 2;

            Rectangle rectangle = new Rectangle(x, y, imgWidth, imgHeight);
            appearance.SetPageRect(rectangle);
            InitAppearanceLayer2();

            base.SignDetached(signature,
                    chain,
                    crlList,
                    ocspClient,
                    tsaClient,
                    estimatedSize,
                    sigType);

            return 1;
        }

        protected void InitAppearanceLayer2()
        {
            PdfFormXObject n2 = appearance.GetLayer2();
            PdfCanvas canvas = new PdfCanvas(n2, document);

            int page = appearance.GetPageNumber();
            Rectangle rect = appearance.GetPageRect();
            int rotation = document.GetPage(page).GetRotation();

            if (rotation == 90)
            {
                canvas.ConcatMatrix(0, 1, -1, 0, rect.GetWidth(), 0);
            }
            else if (rotation == 180)
            {
                canvas.ConcatMatrix(-1, 0, 0, -1, rect.GetWidth(), rect.GetHeight());
            }
            else if (rotation == 270)
            {
                canvas.ConcatMatrix(0, -1, 1, 0, 0, rect.GetHeight());
            }

            Rectangle rotatedRect = RotateRectangle(rect, document.GetPage(page).GetRotation());

            Rectangle signatureRect = new Rectangle(
                    MARGIN,
                    MARGIN,
                    rotatedRect.GetWidth() - 2 * MARGIN,
                    rotatedRect.GetHeight() - 2 * MARGIN);

            ImageData signatureGraphic = appearance.GetSignatureGraphic();
            float imgWidth = signatureGraphic.GetWidth();

            if (imgWidth == 0)
            {
                imgWidth = signatureRect.GetWidth();
            }

            float imgHeight = signatureGraphic.GetHeight();

            if (imgHeight == 0)
            {
                imgHeight = signatureRect.GetHeight();
            }

            float multiplierH = signatureRect.GetWidth() / signatureGraphic.GetWidth();
            float multiplierW = signatureRect.GetHeight() / signatureGraphic.GetHeight();
            float multiplier = Math.Min(multiplierH, multiplierW);
            imgWidth *= multiplier;
            imgHeight *= multiplier;

            float x = signatureRect.GetLeft() + (signatureRect.GetWidth() - imgWidth) / 2;
            float y = signatureRect.GetBottom() + (signatureRect.GetHeight() - imgHeight) / 2;

            PdfExtGState gState = new PdfExtGState();
            gState.SetFillOpacity(opacity);
            gState.SetStrokeOpacity(opacity);
            canvas.SetExtGState(gState);

            canvas.AddImageWithTransformationMatrix(_stamper, imgWidth, 0, 0, imgHeight, x, y);
        }

        public static Rectangle RotateRectangle(Rectangle rect, int angle)
        {
            if (0 == (angle / 90) % 2)
            {
                return new Rectangle(rect.GetWidth(), rect.GetHeight());
            }
            else
            {
                return new Rectangle(rect.GetHeight(), rect.GetWidth());
            }
        }
    }
}
