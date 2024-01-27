using System.Collections.Generic;

namespace FFramework.AI
{
    public class Selector : TreeNode
    {
        public Selector(List<TreeNode> children) : base(children)
        {

        }

        public Selector() : base()
        {
        }

        /// <summary>
        /// 选择节点：按顺序评估子节点，直到找到一个成功的节点，或者所有节点都失败
        /// </summary>
        /// <returns></returns>
        public override NodeStatus Evaluate()
        {
            foreach (TreeNode node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeStatus.Failure:
                        continue;

                    case NodeStatus.Success:
                        Status = NodeStatus.Success;
                        return Status;

                    case NodeStatus.Running:
                        Status = NodeStatus.Running;
                        return Status;

                    default:
                        continue;
                }

            }
            Status = NodeStatus.Failure;
            return Status;
        }
    }

}
