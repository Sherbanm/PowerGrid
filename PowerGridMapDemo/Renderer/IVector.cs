namespace PowerGridMapDemo
{
    public interface IVector
    {
        float x
        {
            get;
            set;
        }

        float y
        {
            get;
            set;
        }

        float z
        {
            get;
            set;
        }

        AbstractVector Add(AbstractVector v2);
        AbstractVector Subtract(AbstractVector v2);
        AbstractVector Multiply(float n);
        AbstractVector Divide(float n);
        float Magnitude();
        //public abstract AbstractVector Normal();
        AbstractVector Normalize();
        AbstractVector SetZero();
        AbstractVector SetIdentity();
    }
}
