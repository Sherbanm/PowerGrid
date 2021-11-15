using PowerGrid.Domain;

namespace PowerGridMapDemo
{
    public class Point
    {
        public Point(AbstractVector iPosition, AbstractVector iVelocity, AbstractVector iAcceleration, City iNode)
        {
            position=iPosition;
            node = iNode;
            velocity = iVelocity;
            acceleration = iAcceleration;
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Point p = obj as Point;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return position==p.position;
        }

        public bool Equals(Point p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return position==p.position;
        }

        public static bool operator ==(Point a, Point b)
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
            return (a.position == b.position);
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public void ApplyForce(AbstractVector force)
        {
            acceleration.Add(force/mass);
        }

        public AbstractVector position { get; set; }
        public City node { get; private set; }
        public float mass
        {
            get
            {
                return node.Mass;
            }
            private set
            {
                node.Mass = value;
            }
        }
        public AbstractVector velocity { get; private set; }
        public AbstractVector acceleration { get; private set; }
     }
}
