using System.Collections.Generic;

namespace BehaviorTree
{
    public class Selector : Node
    {
        public Selector() { }
        public Selector(List<Node> children) : base(children) { }
        public override NodeState Evaluate()
        {
            foreach (Node node in _children)
            {
                switch (node.PreEvaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        ResetOtherChildren(node);
                        _state = NodeState.SUCCESS;
                        return _state;
                    case NodeState.RUNNING:
                        ResetOtherChildren(node);
                        _state = NodeState.RUNNING;
                        return _state;
                    default:
                        continue;
                }
            }

            _state = NodeState.FAILURE;
            return _state;
        }

        private void ResetOtherChildren(Node activeChild)
        {
            foreach(Node node in _children)
            {
                if(node != activeChild)
                {
                    node.Reset();
                }
            }
        }
    }
}
