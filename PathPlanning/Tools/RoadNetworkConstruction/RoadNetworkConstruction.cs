using OpenCvSharp;

namespace PathPlanning.Tools.RoadNetworkConstruction
{
    public struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    public class RoadNetwork
    {
        static private Mat ThinImage(Mat src)
        {
            Mat dst = new Mat();
            int height = src.Rows;
            int width = src.Cols;
            src.CopyTo(dst);
            int count = 0;
            while (true)
            {
                count++;
                List<Coordinate> mFlag = new List<Coordinate>();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        int p1 = dst.At<byte>(i, j);
                        if (p1 != 1)
                        {
                            continue;
                        }
                        int p4 = (j == width - 1) ? 0 : dst.At<byte>(i, j + 1);
                        int p8 = (j == 0) ? 0 : dst.At<byte>(i, j - 1);
                        int p2 = (i == 0) ? 0 : dst.At<byte>(i - 1, j);
                        int p3 = (i == 0 || j == width - 1) ? 0 : dst.At<byte>(i - 1, j + 1);
                        int p9 = (i == 0 || j == 0) ? 0 : dst.At<byte>(i - 1, j - 1);
                        int p6 = (i == height - 1) ? 0 : dst.At<byte>(i + 1, j);
                        int p5 = (i == height - 1 || j == width - 1) ? 0 : dst.At<byte>(i + 1, j + 1);
                        int p7 = (i == height - 1 || j == 0) ? 0 : dst.At<byte>(i + 1, j - 1);

                        if ((p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9) >= 2 && (p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9) <= 6)
                        {
                            int ap = 0;
                            if (p2 == 0 && p3 == 1) ++ap;
                            if (p3 == 0 && p4 == 1) ++ap;
                            if (p4 == 0 && p5 == 1) ++ap;
                            if (p5 == 0 && p6 == 1) ++ap;
                            if (p6 == 0 && p7 == 1) ++ap;
                            if (p7 == 0 && p8 == 1) ++ap;
                            if (p8 == 0 && p9 == 1) ++ap;
                            if (p9 == 0 && p2 == 1) ++ap;

                            if (ap == 1 && p2 * p4 * p6 == 0 && p4 * p6 * p8 == 0)
                            {
                                mFlag.Add(new Coordinate(i, j));
                            }
                        }
                    }
                }

                mFlag.ForEach((Coordinate coordinate) =>
                {
                    dst.Set<byte>(coordinate.X, coordinate.Y, 0);
                });

                if (mFlag.Count == 0)
                {
                    break;
                }
                else
                {
                    mFlag.Clear();
                }

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        int p1 = dst.At<byte>(i, j);
                        if (p1 != 1)
                        {
                            continue;
                        }
                        int p4 = (j == width - 1) ? 0 : dst.At<byte>(i, j + 1);
                        int p8 = (j == 0) ? 0 : dst.At<byte>(i, j - 1);
                        int p2 = (i == 0) ? 0 : dst.At<byte>(i - 1, j);
                        int p3 = (i == 0 || j == width - 1) ? 0 : dst.At<byte>(i - 1, j + 1);
                        int p9 = (i == 0 || j == 0) ? 0 : dst.At<byte>(i - 1, j - 1);
                        int p6 = (i == height - 1) ? 0 : dst.At<byte>(i + 1, j);
                        int p5 = (i == height - 1 || j == width - 1) ? 0 : dst.At<byte>(i + 1, j + 1);
                        int p7 = (i == height - 1 || j == 0) ? 0 : dst.At<byte>(i + 1, j - 1);
                        if ((p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9) >= 2 && (p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9) <= 6)
                        {
                            int ap = 0;
                            if (p2 == 0 && p3 == 1) ++ap;
                            if (p3 == 0 && p4 == 1) ++ap;
                            if (p4 == 0 && p5 == 1) ++ap;
                            if (p5 == 0 && p6 == 1) ++ap;
                            if (p6 == 0 && p7 == 1) ++ap;
                            if (p7 == 0 && p8 == 1) ++ap;
                            if (p8 == 0 && p9 == 1) ++ap;
                            if (p9 == 0 && p2 == 1) ++ap;

                            if (ap == 1 && p2 * p4 * p8 == 0 && p2 * p6 * p8 == 0)
                            {
                                mFlag.Add(new Coordinate(i, j));
                            }
                        }
                    }
                }

                mFlag.ForEach((Coordinate coordinate) =>
                {
                    dst.Set<byte>(coordinate.X, coordinate.Y, 0);
                });

                if (mFlag.Count == 0)
                {
                    break;
                }
                else
                {
                    mFlag.Clear();
                }

            }
            return dst;
        }
        static private void FilterOver(Mat thinSrc)
        {
            int height = thinSrc.Rows;
            int width = thinSrc.Cols;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int p1 = thinSrc.At<byte>(i, j);
                    if (p1 != 1)
                    {
                        continue;
                    }
                    int p4 = (j == width - 1) ? 0 : thinSrc.At<byte>(i, j + 1);
                    int p8 = (j == 0) ? 0 : thinSrc.At<byte>(i, j - 1);
                    int p2 = (i == 0) ? 0 : thinSrc.At<byte>(i - 1, j);
                    int p3 = (i == 0 || j == width - 1) ? 0 : thinSrc.At<byte>(i - 1, j + 1);
                    int p9 = (i == 0 || j == 0) ? 0 : thinSrc.At<byte>(i - 1, j - 1);
                    int p6 = (i == height - 1) ? 0 : thinSrc.At<byte>(i + 1, j);
                    int p5 = (i == height - 1 || j == width - 1) ? 0 : thinSrc.At<byte>(i + 1, j + 1);
                    int p7 = (i == height - 1 || j == 0) ? 0 : thinSrc.At<byte>(i + 1, j - 1);
                    if (p2 + p3 + p8 + p9 >= 1)
                    {
                        thinSrc.Set<byte>(i, j, 0);
                    }
                }
            }
        }
        static private List<Coordinate> GetPoints(Mat thinSrc)
        {
            List<Coordinate> points = new List<Coordinate>();
            int height = thinSrc.Rows;
            int width = thinSrc.Cols;
            Mat tmp = new Mat();
            thinSrc.CopyTo(tmp);
            int radius = 6;
            int thresholdMax = 9;
            int thresholdMin = 6;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (tmp.Get<byte>(i, j) != 1)
                    {
                        continue;
                    }
                    int count = 0;
                    for (int k = i - radius; k < i + radius + 1; k++)
                    {
                        for (int l = j - radius; l < j + radius + 1; l++)
                        {
                            if (k < 0 || l < 0 || k > height - 1 || l > width - 1)
                            {
                                continue;
                            }
                            else if (tmp.Get<byte>(k, l) == 1)
                            {
                                count++;
                            }
                        }
                    }
                    if (count > thresholdMax || count < thresholdMin)
                    {
                        points.Add(new Coordinate(i, j));
                    }
                }
            }
            return points;
        }
        static private void Normalization(Mat src, List<Coordinate> points)
        {
            List<Coordinate> result = new List<Coordinate>();
            bool cantFindMore = false;
            while (true)
            {
                if (cantFindMore)
                {
                    break;
                }
                cantFindMore = true;
                bool shouldBreak = false;
                for (int i = 0; i < points.Count; i++)
                {
                    if (shouldBreak)
                    {
                        break;
                    }
                    for (int j = 0; j < points.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (Math.Sqrt(Math.Pow(points[i].X - points[j].X, 2) + Math.Pow(points[i].Y - points[j].Y, 2)) < 15)
                        {
                            cantFindMore = false;
                            points.Add(new Coordinate((points[i].X + points[j].X) / 2, (points[i].Y + points[j].Y) / 2));
                            points.RemoveAt(i);
                            if (i < j)
                            {
                                points.RemoveAt(j - 1);
                            }
                            else
                            {
                                points.RemoveAt(j);
                            }
                            shouldBreak = true;
                            break;
                        }
                    }
                }
            }
            int height = src.Cols;
            int width = src.Rows;
            for (int i = 0; i < points.Count; i++)
            {
                int p = src.At<byte>(points[i].X, points[i].Y);
                if (p != 1)
                {
                    int depth = 1;
                    int x = points[i].X;
                    int y = points[i].Y;
                    while (true)
                    {
                        if (x - depth >= 0 && y - depth >= 0)
                        {
                            int point = src.At<byte>(x - depth, y - depth);
                            if (point == 1)
                            {
                                points[i] = new Coordinate(x - depth, y - depth);
                                break;
                            }
                        }
                        if (x - depth >= 0)
                        {
                            int point = src.At<byte>(x - depth, y);
                            if (point == 1)
                            {
                                points[i] = new Coordinate(x - depth, y);
                                break;
                            }
                        }
                        if (x - depth >= 0 && y + depth < height)
                        {
                            int point = src.At<byte>(x - depth, y + depth);
                            if (point == 1)
                            {
                                points[i] = new Coordinate(x - depth, y + depth);
                                break;
                            }
                        }
                        if (y - depth >= 0)
                        {
                            int point = src.At<byte>(x, y - depth);
                            if (point == 1)
                            {
                                points[i] = new Coordinate(x, y - depth);
                                break;
                            }
                        }
                        if (y + depth < height)
                        {
                            int point = src.At<byte>(x, y + depth);
                            if (point == 1)
                            {
                                points[i] = new Coordinate(x, y + depth);
                                break;
                            }
                        }
                        if (x + depth < width && y - depth >= 0)
                        {
                            int point = src.At<byte>(x + depth, y - depth);
                            if (point == 1)
                            {
                                points[i] = new Coordinate(x + depth, y - depth);
                                break;
                            }
                        }
                        if (x + depth < width)
                        {
                            int point = src.At<byte>(x + depth, y);
                            if (point == 1)
                            {
                                points[i] = new Coordinate(x + depth, y);
                                break;
                            }
                        }
                        if (x + depth < width && y + depth < height)
                        {
                            int point = src.At<byte>(x + depth, y + depth);
                            if (point == 1)
                            {
                                points[i] = new Coordinate(x + depth, y + depth);
                                break;
                            }
                        }
                        depth++;
                    }
                }
            }
        }

        static public List<Coordinate> Construct(string imgName)
        {
            Mat src = Cv2.ImRead(@"./Images/RoadNetworkConstruction/" + imgName, ImreadModes.Grayscale);
            Cv2.Threshold(src, src, 128, 1, ThresholdTypes.Binary);
            Mat dst = ThinImage(src);
            FilterOver(dst);
            List<Coordinate> points = GetPoints(dst);
            Normalization(src, points);
            dst = dst * 255;
            src = src * 255;
            points.ForEach((Coordinate coordinate) =>
            {
                Cv2.Circle(src, new Point(coordinate.Y, coordinate.X), 6, 128, 2);
            });
            return points;
        }
    }
}
