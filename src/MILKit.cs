using Matrox.MatroxImagingLibrary;
using OpenCvSharp;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MILKit {

    public class MILKit {
        private bool _verbose = false;

        public MILKit(bool verbose = false) {
            _verbose = verbose;
        }

        public bool BufferToMat(MIL_ID BufferID, ref Mat Image) {
            try {
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

                return true;
            } catch (Exception ex) {
                if (_verbose)
                    MessageBox.Show($"{ex.StackTrace}\n\n{ex.Message}", "MILKit",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool MatToBuffer(Mat Image, MIL_ID BufferID) {
            try {
                int width = Image.Width;
                int height = Image.Height;
                byte[] buffer = new byte[width * height];

                Marshal.Copy(Image.Data, buffer, 0, buffer.Length);
                MIL.MbufPut(BufferID, buffer);

                return true;
            } catch (Exception ex) {
                if (_verbose)
                    MessageBox.Show($"{ex.StackTrace}\n\n{ex.Message}", "MILKit",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool Normalization(MIL_ID SrcBufferID, MIL_ID DestBufferID, double Alpha = 0, double Beta = 255) {
            try {
                Mat Image = new Mat();
                BufferToMat(SrcBufferID, ref Image);
                Cv2.Normalize(Image, Image, Alpha, Beta, NormTypes.MinMax);
                MatToBuffer(Image, DestBufferID);

                return true;
            } catch (Exception ex) {
                if (_verbose)
                    MessageBox.Show($"{ex.StackTrace}\n\n{ex.Message}", "MILKit",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}