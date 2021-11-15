namespace PowerGridMapDemo
{
    public class Spring
    {
        public Spring(Point iPoint1, Point iPoint2, float iLength, float iK)
        {
            point1 = iPoint1;
            point2 = iPoint2;
            Length = iLength;
            K = iK;
        }

        public Point point1
        {
            get;
            private set;
        }
        public Point point2
        {
            get;
            private set;
        }

        public float Length
        {
            get;
            private set;
        }

        public float K
        {
            get;
            private set;
        }
    }
}
