using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using NumSharp;
using OpenCvSharp;

namespace PathPlanning.Tools.DLinkNetInference
{
    public class ONNXInference
    {
        //Microsoft.ML.OnnxRuntime.SessionOptions.MakeSessionOptionWithCudaProvider(0)
        //static public InferenceSession session = new InferenceSession("/app/Tools/DLinkNetInference/dlinknet.onnx");
        //static public InferenceSession session = new InferenceSession("/app/onnx/dlinknet.onnx", Microsoft.ML.OnnxRuntime.SessionOptions.MakeSessionOptionWithCudaProvider(0));
        //static public InferenceSession session = new InferenceSession("/app/Tools/DLinkNetInference/dlinknet.onnx", Microsoft.ML.OnnxRuntime.SessionOptions.MakeSessionOptionWithCudaProvider(0));
        //static public InferenceSession session = new InferenceSession("onnx\\dlinknet.onnx");
        //static public InferenceSession session = new InferenceSession("onnx\\dlinknet.onnx", Microsoft.ML.OnnxRuntime.SessionOptions.MakeSessionOptionWithCudaProvider(0));
        static public InferenceSession session = new InferenceSession("D:\\Fate\\1_MyProject\\PathPlanning\\Backend\\PathPlanningServer\\PathPlanning\\Tools\\DLinkNetInference\\dlinknet.onnx", Microsoft.ML.OnnxRuntime.SessionOptions.MakeSessionOptionWithCudaProvider(0));
        static private void RGBMatToFourDimensionArray(Mat image,ref float[,,,] array)
        {
            for (int i = 0; i < image.Rows; i++)
            {
                for (int j = 0; j < image.Cols; j++)
                {
                    array[0, i, j, 0] = image.At<Vec3b>(i, j)[0];
                    array[0, i, j, 1] = image.At<Vec3b>(i, j)[1];
                    array[0, i, j, 2] = image.At<Vec3b>(i, j)[2];
                }
            }
        }
        static private void OneDimensionArrayToThreeDimensionArray(float[] oneDimensionArray, ref float[,,] threeDimensionArray)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 1024; j++)
                {
                    for (int k = 0; k < 1024; k++)
                    {
                        threeDimensionArray[i, j, k] = oneDimensionArray[i * 1024 * 1024 + j * 1024 + k];
                    }
                }
            }
        }
        static public string PathRecognition(string imgName)
        {
            var img = Cv2.ImRead("././Images/Input/"+imgName);
            Mat imgRot90 = new Mat();
            Cv2.Rotate(img, imgRot90, RotateFlags.Rotate90Counterclockwise);
            float[,,,] imgArray = new float[1, img.Rows, img.Cols, img.Channels()];
            float[,,,] imgRot90Array = new float[1, imgRot90.Rows, imgRot90.Cols, imgRot90.Channels()];
            RGBMatToFourDimensionArray(img, ref imgArray);
            RGBMatToFourDimensionArray(imgRot90, ref imgRot90Array);
            NDArray imgNDArray = np.array(imgArray);
            NDArray imgRot90NDArray = np.array(imgRot90Array);
            NDArray img1 = np.concatenate(new NDArray[] { imgNDArray, imgRot90NDArray });
            NDArray img2 = img1[":,::-1"];
            NDArray img3 = np.concatenate(new NDArray[] { img1, img2 });
            NDArray img4 = img3[":,:,::-1"];
            NDArray img5 = img3.transpose(new int[] { 0, 3, 1, 2 });
            img5 = img5 / 255.0 * 3.2 - 1.6;
            NDArray img6 = img4.transpose(new int[] { 0, 3, 1, 2 });
            img6 = img6 / 255.0 * 3.2 - 1.6;
            var img5InputArray = img5.ToArray<float>();
            var inputTensor = new DenseTensor<float>(img5InputArray, new int[] { 4, 3, 1024, 1024 });
            var input = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<float>("input", inputTensor) };
            var output = session.Run(input);
            var outputArray = output.ToArray()[0].AsTensor<float>().ToArray<float>();
            float[,,] maskAArray = new float[4, 1024, 1024];
            OneDimensionArrayToThreeDimensionArray(outputArray, ref maskAArray);
            NDArray maskA = np.array(maskAArray);
            var img6InputArray = img6.ToArray<float>();
            inputTensor = new DenseTensor<float>(img6InputArray, new int[] { 4, 3, 1024, 1024 });
            input = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<float>("input", inputTensor) };
            output = session.Run(input);
            outputArray = output.ToArray()[0].AsTensor<float>().ToArray<float>();
            float[,,] maskBArray = new float[4, 1024, 1024];
            OneDimensionArrayToThreeDimensionArray(outputArray, ref maskBArray);
            NDArray maskB = np.array(maskBArray);
            var mask1 = maskA + maskB[":,:,::-1"];
            var mask2 = mask1[":2"] + mask1["2:,::-1"];
            float[,] mask2_1_rot90 = new float[1024, 1024];
            for (int i = 0; i < 1024; i++)
            {
                for (int j = 0; j < 1024; j++)
                {
                    mask2_1_rot90[i, j] = mask2[1][j][1023 - i];
                }
            }
            NDArray mask2_1_rot90NDArray = np.array(mask2_1_rot90);
            var mask3 = mask2[0] + mask2_1_rot90NDArray["::-1,::-1"];
            int[,,] mask = new int[1024, 1024, 1];
            for (int i = 0; i < 1024; i++)
            {
                for (int j = 0; j < 1024; j++)
                {
                    if (float.Parse(mask3[i][j].ToString()) > 4.0)
                    {
                        mask[i, j, 0] = 255;
                    }
                    else
                    {
                        mask[i, j, 0] = 0;
                    }
                }
            }
            NDArray maskNDArray = np.array(mask);
            maskNDArray = np.concatenate(new NDArray[] { maskNDArray, maskNDArray, maskNDArray }, axis: 2);
            Mat outputImg = new Mat(1024, 1024, MatType.CV_8UC3);
            for (int i = 0; i < outputImg.Rows; i++)
            {
                for (int j = 0; j < outputImg.Cols; j++)
                {
                    outputImg.At<Vec3b>(i, j)[0] = maskNDArray[i][j][0];
                    outputImg.At<Vec3b>(i, j)[1] = maskNDArray[i][j][1];
                    outputImg.At<Vec3b>(i, j)[2] = maskNDArray[i][j][2];
                }
            }
            string outputName = Path.GetFileNameWithoutExtension(imgName) + "_mask.png";
            Cv2.ImWrite(@"./Images/Output/"+outputName, outputImg);
            return outputName;
        }
    }
}
