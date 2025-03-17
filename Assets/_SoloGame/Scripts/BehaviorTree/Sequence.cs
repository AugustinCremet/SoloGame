using System.Collections.Generic;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        private int _currentChildIndex = 0;
        public Sequence() { }
        public Sequence(List<Node> children) : base(children) { }
        public override NodeState Evaluate()
        {
            //bool anyChildIsRunning = false;

            //foreach(Node node in _children)
            //{
            //    switch(node.Evaluate())
            //    {
            //        case NodeState.FAILURE:
            //            _state = NodeState.FAILURE;
            //            return _state;
            //        case NodeState.SUCCESS:
            //            continue;
            //        case NodeState.RUNNING:
            //            _state = NodeState.RUNNING;
            //            return _state;
            //            //anyChildIsRunning = true;
            //            //continue;
            //        default:
            //            _state = NodeState.SUCCESS;
            //            return _state;
            //    }
            //}

            //_state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            //return _state;

            // Start from where it left off
            for (int i = _currentChildIndex; i < _children.Count; i++)
            {
                NodeState childState = _children[i].Evaluate();

                switch (childState)
                {
                    case NodeState.FAILURE:
                        _currentChildIndex = 0; // Reset the sequence on failure
                        _state = NodeState.FAILURE;
                        return _state;

                    case NodeState.RUNNING:
                        _currentChildIndex = i; // Save the running child's index
                        _state = NodeState.RUNNING;
                        return _state;

                    case NodeState.SUCCESS:
                        continue; // Move to the next child
                }
            }

            // All children succeeded, reset for the next evaluation
            _currentChildIndex = 0;
            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}

