using Matrox.MatroxImagingLibrary;
using OpenCvSharp;
using System.Runtime.InteropServices;

namespace MILKit {

    public class MILKit {
        public void BufferToMat(MIL_ID BufferID, ref Mat Image) {
            int width = 0;
            int height = 0;
            int depth = 0;

            MIL.MbufInquire(BufferID, MIL.M_SIZE_X, ref width);
            MIL.MbufInquire(BufferID, MIL.M_SIZE_Y, ref height);
            MIL.MbufInquire(BufferID, MIL.M_SIZE_BIT, ref depth);

            byte[] buffer = new byte[width * height];

            Image = new Mat(height, width, MatType.CV_8UC1);
            MIL.MbufGet2d(BufferID, 0, 0, width, height, buffer);
            Marshal.Copy(buffer, 0, Image.Data, buffer.Length);
        }

        public void MatToBuffer(Mat Image, MIL_ID BufferID) {
            int width = Image.Width;
            int height = Image.Height;
            byte[] buffer = new byte[width * height];

            Marshal.Copy(Image.Data, buffer, 0, buffer.Length);
            MIL.MbufPut(BufferID, buffer);
        }

        public void Normalization(MIL_ID SrcBufferID, MIL_ID DestBufferID, double Alpha = 0, double Beta = 255) {
            Mat Image = new Mat();
            BufferToMat(SrcBufferID, ref Image);
            Cv2.Normalize(Image, Image, Alpha, Beta, NormTypes.MinMax);
            MatToBuffer(Image, DestBufferID);
        }

    }
}