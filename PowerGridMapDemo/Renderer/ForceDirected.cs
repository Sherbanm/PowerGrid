using PowerGrid.Domain;
using System.Collections.Generic;
using System.Linq;

namespace PowerGridMapDemo
{

    public class NearestPoint{
            public NearestPoint()
            {
                node=null;
                point=null;
                distance=null;
            }
            public City node;
            public Point point;
            public float? distance;
        }

    public class BoundingBox
    {
        public static float defaultBB= 2.0f;
        public static float defaultPadding = 0.07f; // ~5% padding
        
        public BoundingBox()
        {
            topRightBack = null;
            bottomLeftFront = null;
        }
        public AbstractVector topRightBack;
        public AbstractVector bottomLeftFront;
    }

    public abstract class ForceDirected<Vector> : IForceDirected where Vector : IVector
    {
    	public float Stiffness
        {
            get;
            set;
        }

        public float Repulsion
        {
            get;
            set;
        }

        public float Damping
        {
            get;
            set;
        }

        public float Threadshold
        {
            get;
            set;
        }

        public bool WithinThreashold
        {
            get;
            private set;
        }
        protected Dictionary<string, Point> m_nodePoints;
        protected Dictionary<string, Spring> m_edgeSprings;
        public PositionalGraph graph
        {
            get;
            protected set;
        }
        public void Clear()
        {
            m_nodePoints.Clear();
            m_edgeSprings.Clear();
            graph.Clear();
        }

        public ForceDirected(PositionalGraph iGraph, float iStiffness, float iRepulsion, float iDamping)
        {
            graph=iGraph;
            Stiffness=iStiffness;
            Repulsion=iRepulsion;
            Damping=iDamping;
            m_nodePoints = new Dictionary<string, Point>();
            m_edgeSprings = new Dictionary<string, Spring>();

            Threadshold = 0.01f;
        }

        public abstract Point GetPoint(KeyValuePair<City, AbstractVector> kvPair);



        public Spring GetSpring(Connection iEdge)
        {
            if(!(m_edgeSprings.ContainsKey(iEdge.ID)))
            {
                float length = iEdge.Length;
                Spring existingSpring = null;

                List<Connection> fromEdges= graph.GetEdges(iEdge.CityA,iEdge.CityB);
                if (fromEdges != null)
                {
                    foreach (Connection e in fromEdges)
                    {
                        if (existingSpring == null && m_edgeSprings.ContainsKey(e.ID))
                        {
                            existingSpring = m_edgeSprings[e.ID];
                            break;
                        }
                    }
                
                }
                if(existingSpring!=null)
                {
                    return new Spring(existingSpring.point1, existingSpring.point2, 0.0f, 0.0f);
                }

                List<Connection> toEdges = graph.GetEdges(iEdge.CityA,iEdge.CityB);
                if (toEdges != null)
                {
                    foreach (Connection e in toEdges)
                    {
                        if (existingSpring == null && m_edgeSprings.ContainsKey(e.ID))
                        {
                            existingSpring = m_edgeSprings[e.ID];
                            break;
                        }
                    }
                }
                
                if(existingSpring!=null)
                {
                    return new Spring(existingSpring.point2, existingSpring.point1, 0.0f, 0.0f);
                }
                var source = graph.NodesWithPosition.First(x => x.Key.Equals(iEdge.CityA));
                var target = graph.NodesWithPosition.First(x => x.Key.Equals(iEdge.CityB));
                m_edgeSprings[iEdge.ID] = new Spring(GetPoint(source), GetPoint(target), length, Stiffness);

            }
            return m_edgeSprings[iEdge.ID];
        }

        // TODO: change this for group only after node grouping
        protected void applyCoulombsLaw()
        {
            foreach(KeyValuePair<City, AbstractVector> kv in graph.NodesWithPosition)
            {
                Point point1 = GetPoint(kv);
                foreach(var kv2 in graph.NodesWithPosition)
                {
                    Point point2 = GetPoint(kv2);
                    if(point1!=point2)
                    {
                        AbstractVector d=point1.position-point2.position;
                        float distance = d.Magnitude() +0.1f;
                        AbstractVector direction = d.Normalize();
                        if (kv.Key.Pinned && kv2.Key.Pinned)
                        {
                            point1.ApplyForce(direction * 0.0f);
                            point2.ApplyForce(direction * 0.0f);
                        }
                        else if (kv.Key.Pinned)
                        {
                            point1.ApplyForce(direction*0.0f);
                            //point2.ApplyForce((direction * Repulsion) / (distance * distance * -1.0f));
                            point2.ApplyForce((direction * Repulsion) / (distance * -1.0f));
                        }
                        else if (kv.Key.Pinned)
                        {
                            //point1.ApplyForce((direction * Repulsion) / (distance * distance));
                            point1.ApplyForce((direction * Repulsion) / (distance));
                            point2.ApplyForce(direction * 0.0f);
                        }
                        else
                        {
//                             point1.ApplyForce((direction * Repulsion) / (distance * distance * 0.5f));
//                             point2.ApplyForce((direction * Repulsion) / (distance * distance * -0.5f));
                            point1.ApplyForce((direction * Repulsion) / (distance * 0.5f));
                            point2.ApplyForce((direction * Repulsion) / (distance * -0.5f));
                        }

                    }
                }
            }
        }

        protected void applyHookesLaw()
        {
            foreach(Connection e in graph.Connections)
            {
                Spring spring = GetSpring(e);
                AbstractVector d = spring.point2.position-spring.point1.position;
                float displacement = spring.Length-d.Magnitude();
                AbstractVector direction = d.Normalize();

                if (spring.point1.node.Pinned && spring.point2.node.Pinned)
                {
                    spring.point1.ApplyForce(direction * 0.0f);
                    spring.point2.ApplyForce(direction * 0.0f);
                }
                else if (spring.point1.node.Pinned)
                {
                    spring.point1.ApplyForce(direction * 0.0f);
                    spring.point2.ApplyForce(direction * (spring.K * displacement));
                }
                else if (spring.point2.node.Pinned)
                {
                    spring.point1.ApplyForce(direction * (spring.K * displacement * -1.0f));
                    spring.point2.ApplyForce(direction * 0.0f);
                }
                else
                {
                    spring.point1.ApplyForce(direction * (spring.K * displacement * -0.5f));
                    spring.point2.ApplyForce(direction * (spring.K * displacement * 0.5f));
                }

                
            }
        }

        protected void attractToCentre()
        {
            foreach(var kv in graph.NodesWithPosition)
            {
                Point point = GetPoint(kv);
                if (!point.node.Pinned)
                {
                    AbstractVector direction = point.position*-1.0f;
                    //point.ApplyForce(direction * ((float)Math.Sqrt((double)(Repulsion / 100.0f))));

                    
                    float displacement = direction.Magnitude();
                    direction = direction.Normalize();
                    point.ApplyForce(direction * (Stiffness * displacement * 0.4f));
                }
             }
        }

        protected void updateVelocity(float iTimeStep)
        {
            foreach(var kv in graph.NodesWithPosition)
            {
                Point point = GetPoint(kv);
                point.velocity.Add(point.acceleration*iTimeStep);
                point.velocity.Multiply(Damping);
                point.acceleration.SetZero();
            }
        }

        protected void updatePosition(float iTimeStep)
        {
            foreach(var kv in graph.NodesWithPosition)
            {
                Point point = GetPoint(kv);
                point.position.Add(point.velocity*iTimeStep);
            }
        }

        protected float getTotalEnergy()
        {
            float energy=0.0f;
            foreach(var kv in graph.NodesWithPosition)
            {
                Point point = GetPoint(kv);
                float speed = point.velocity.Magnitude();
                energy+=0.5f *point.mass *speed*speed;
            }
            return energy;
        }

        public void Calculate(float iTimeStep) // time in second
        {
            applyCoulombsLaw();
            applyHookesLaw();
            attractToCentre();
            updateVelocity(iTimeStep);
            updatePosition(iTimeStep);
            if (getTotalEnergy() < Threadshold)
            {
                WithinThreashold = true;
            }
            else
                WithinThreashold = false;
        }


        public void EachEdge(EdgeAction del)
        {
            foreach(Connection e in graph.Connections)
            {
                del(e, GetSpring(e));
            }
        }

        public void EachNode(NodeAction del)
        {
            foreach (var kv in graph.NodesWithPosition)
            {
                del(kv.Key, GetPoint(kv));
            }
        }

        public NearestPoint Nearest(AbstractVector position)
        {
            NearestPoint min = new NearestPoint();
            foreach(var kv in graph.NodesWithPosition)
            {
                Point point = GetPoint(kv);
                float distance = (point.position-position).Magnitude();
                if(min.distance==null || distance<min.distance)
                {
                    min.node=kv.Key;
                    min.point=point;
                    min.distance=distance;
                }
            }
            return min;
        }

        public abstract BoundingBox GetBoundingBox();
	
    }

    public class ForceDirected2D : ForceDirected<FDGVector2>
    {
        public ForceDirected2D(PositionalGraph iGraph, float iStiffness, float iRepulsion, float iDamping)
            : base(iGraph, iStiffness, iRepulsion, iDamping)
        {

        }

        public override Point GetPoint(KeyValuePair<City, AbstractVector> kvPair)
        {
            if (!(m_nodePoints.ContainsKey(kvPair.Key.ID)))
            {
                FDGVector2 iniPosition = kvPair.Value as FDGVector2;
                if (iniPosition == null)
                    iniPosition = FDGVector2.Random() as FDGVector2;
                m_nodePoints[kvPair.Key.ID] = new Point(iniPosition, FDGVector2.Zero(), FDGVector2.Zero(), kvPair.Key);
            }
            return m_nodePoints[kvPair.Key.ID];
        }

        public override BoundingBox GetBoundingBox()
        {
            BoundingBox boundingBox = new BoundingBox();
            FDGVector2 bottomLeft = FDGVector2.Identity().Multiply(BoundingBox.defaultBB * -1.0f) as FDGVector2;
            FDGVector2 topRight = FDGVector2.Identity().Multiply(BoundingBox.defaultBB) as FDGVector2;
            foreach (var kv in graph.NodesWithPosition)
            {
                FDGVector2 position = GetPoint(kv).position as FDGVector2;

                if(position.x < bottomLeft.x)
                    bottomLeft.x=position.x;
                if(position.y<bottomLeft.y)
                    bottomLeft.y=position.y;
                if(position.x>topRight.x)
                    topRight.x=position.x;
                if(position.y>topRight.y)
                    topRight.y=position.y;
            }
            AbstractVector padding = (topRight-bottomLeft).Multiply(BoundingBox.defaultPadding);
            boundingBox.bottomLeftFront=bottomLeft.Subtract(padding);
            boundingBox.topRightBack=topRight.Add(padding);
            return boundingBox;

        }
    }

}
