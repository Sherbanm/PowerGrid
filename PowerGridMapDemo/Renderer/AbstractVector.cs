using System;

namespace PowerGridMapDemo
{
    public abstract class AbstractVector:IVector
    {

        public float x
        {
            get;
            set;
        }

        public float y
        {
            get;
            set;
        }

        public float z
        {
            get;
            set;
        }

        public AbstractVector()
        {
        }

        public abstract AbstractVector Add(AbstractVector v2);
        public abstract AbstractVector Subtract(AbstractVector v2);
        public abstract AbstractVector Multiply(float n);
        public abstract AbstractVector Divide(float n);
        public abstract float Magnitude();
        //public abstract public abstract AbstractVector Normal();
        public abstract AbstractVector Normalize();
        public abstract AbstractVector SetZero();
        public abstract AbstractVector SetIdentity();

        public static AbstractVector operator +(AbstractVector a, AbstractVector b)
        {
            if (a is FDGVector2 && b is FDGVector2)
                return (a as FDGVector2) + (b as FDGVector2);
            return null;
        }
        public static AbstractVector operator -(AbstractVector a, AbstractVector b)
        {
            if (a is FDGVector2 && b is FDGVector2)
                return (a as FDGVector2) - (b as FDGVector2);
            return null;
        }
        public static AbstractVector operator *(AbstractVector a, float b)
        {
            if (a is FDGVector2)
                return (a as FDGVector2) * b;
            return null;
        }
        public static AbstractVector operator *(float a, AbstractVector b)
        {
            if (b is FDGVector2)
                return a * (b as FDGVector2);
            return null;
        }

        public static AbstractVector operator /(AbstractVector a, float b)
        {
            if (a is FDGVector2)
                return (a as FDGVector2) / b;
            return null;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(System.Object obj)
        {
            return this==(obj as AbstractVector);
        }
        public static bool operator ==(AbstractVector a, AbstractVector b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            if (a is FDGVector2 && b is FDGVector2)
                return (a as FDGVector2) == (b as FDGVector2);
            return false;

        }

        public static bool operator !=(AbstractVector a, AbstractVector b)
        {
            return !(a == b);
        }

        private static Random random = new Random();
        public static float RandomFloat()
        {
            var result = random.NextDouble();
            return (float)result;
        }


    }


}
