using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZED.Wpf.Train
{
    public class IVMSTreeListViewModel : ViewModelBase
    {
        public IVMSTreeListViewModel()
        {
            ChildrenList = new List<IVMSTreeListViewModel>();
            NodeData = null;
            Node = string.Empty;
            nodeImage = "/Resources/deltext.png";
            NodeName = string.Empty;
            ParentNode = string.Empty;
            isNodeExpanded = false;
        }

        /// <summary>
        /// node ID
        /// </summary>
        public string Node { get; set; }


        private string nodeImage;
        /// <summary>
        /// 结点图片
        /// </summary>
        public string NodeImage
        {
            get { return nodeImage; }
            set
            {
                if (value != this.nodeImage)
                {
                    nodeImage = value;
                    OnPropertyChanged(() => this.nodeImage);
                }
            }
        }

        /// <summary>
        /// 结点名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 父结点
        /// </summary>
        public string ParentNode { get; set; }

        private bool isNodeExpanded;
        /// <summary>
        /// 结点是否展开
        /// </summary>
        public bool IsNodeExpanded
        {
            get { return isNodeExpanded; }
            set
            {
                if (value != this.isNodeExpanded)
                {
                    isNodeExpanded = value;
                    OnPropertyChanged(() => this.isNodeExpanded);
                    if (this.ChildrenList.Count <= 0)
                    {
                        return;
                    }
                    if (isNodeExpanded)
                    {
                        this.NodeImage = "/Resources/communication_organization_yrjhzk.png";
                    }
                    else
                    {
                        this.NodeImage = "/Resources/communication_organization_yrjhwzk.png";
                    }
                }
            }
        }

        /// <summary>
        /// Object
        /// </summary>
        public Object NodeData { get; set; }

        /// <summary>
        /// 子结点
        /// </summary>
        public List<IVMSTreeListViewModel> ChildrenList { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Node, NodeImage);
        }
    }
}
