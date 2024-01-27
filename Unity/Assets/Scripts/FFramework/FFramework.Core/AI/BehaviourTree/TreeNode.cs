using System.Collections.Generic;


namespace FFramework.AI
{
    public class TreeNode
    {
        public TreeNode Parent { get; set; } = null;

        /* 项目“FFrameworkGenerator”的未合并的更改
        在此之前:
                public List<TreeNode> Children { get; } = new List<TreeNode>();

                public NodeStatus Status { get;protected set; } = NodeStatus.Running;
        在此之后:
                public List<TreeNode> Children { get; } = new List<TreeNode>();

                public NodeStatus Status { get;protected set; } = NodeStatus.Running;
        */
        public List<TreeNode> Children { get; } = new List<TreeNode>();

        public NodeStatus Status { get; protected set; } = NodeStatus.Running;

        public void Attach(TreeNode child)
        {
            Children.Add(child);
            child.Parent = this;
        }

        public void Detach(TreeNode child)
        {
            Children.Remove(child);
            child.Parent = null;
        }

        public TreeNode(List<TreeNode> children)
        {
            foreach (var child in children)
            {
                Attach(child);
            }
        }
        public TreeNode()
        {

        }

        public virtual NodeStatus Evaluate() => NodeStatus.Failure;
    }
}

