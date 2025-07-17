using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    public class Node
    {
        protected NodeState _state;
        protected BehaviorTreeContext _context;
        protected Blackboard _blackboard;

        public Node Parent;
        protected List<Node> _children = new List<Node>();

        private Dictionary<string, object>  _dataContext = new Dictionary<string, object>();

        public Node()
        {
            Parent = null;
        }

        public Node(List<Node> children)
        {
            Parent = null;
            foreach (Node child in children)
            { 
                Attach(child);
            }
        }

        public void SetContext(BehaviorTreeContext context)
        {
            _context = context;
            foreach (var child in _children)
            {
                child.SetContext(context);
            }
        }

        public void SetBlackboard(Blackboard bb)
        {
            _blackboard = bb;
            foreach (var child in _children)
            {
                child.SetBlackboard(bb);
            }
        }

        protected virtual void OnContextSet() { }

        private void Attach(Node node)
        {
            node.Parent = this;
            _children.Add(node);
        }

        public NodeState PreEvaluate()
        {
            if (!_context.Enemy.IsAIActive)
                return NodeState.FAILURE;

            return Evaluate();
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;

            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = Parent;

            while (node != null)
            {
                value = node.GetData(key);

                if (value != null)
                    return value;

                node = node.Parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = Parent;

            while (node != null)
            {
                bool cleared = node.ClearData(key);

                if (cleared)
                    return true;

                node = node.Parent;
            }

            return false;
        }
    }

}
