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
            // Start from where it left off
            for (int i = _currentChildIndex; i < _children.Count; i++)
            {
                NodeState childState = _children[i].PreEvaluate();

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

        public override void Reset()
        {
            _currentChildIndex = 0;

            foreach (Node child in _children)
            {
                child.Reset();
            }
        }
    }
}

