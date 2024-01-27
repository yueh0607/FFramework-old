using System.Collections.Generic;

namespace FFramework.AI
{
    public class Sequence : TreeNode
    {
        public Sequence(List<TreeNode> children) : base(children)
        {

        }

        public Sequence() : base()
        {
        }

        /// <summary>
        /// 顺序节点：评估子节点，直到找到一个失败的节点，或者所有节点都成功
        /// </summary>
        /// <returns></returns>
        public override NodeStatus Evaluate()
        {
            bool anyChildIsRunning = false;
            foreach (TreeNode node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeStatus.Failure:
                        Status = NodeStatus.Failure;
                        return Status;

                    case NodeStatus.Success:
                        continue;

                    case NodeStatus.Running:
                        anyChildIsRunning = true;
                        continue;

                    default:
                        Status = NodeStatus.Success;
                        return Status;
                }

            }
            Status = anyChildIsRunning ? NodeStatus.Running : NodeStatus.Success;
            return Status;
        }
    }

}
