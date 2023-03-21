using UnityEditor.Experimental.GraphView;

namespace CheeseCraft.Editor
{
    public sealed class NodePort : Port
    {
        public NodePort( Direction portDirection, Capacity portCapacity) : base(Orientation.Vertical, portDirection, portCapacity, typeof(bool))
        {
        }
    }
}