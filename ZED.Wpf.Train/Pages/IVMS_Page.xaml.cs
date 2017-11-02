using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZED.CustomControl;
using ZED.IVMS7200;
using ZED.Train.Entiry;

namespace ZED.Wpf.Train
{
    /// <summary>
    /// IVMS_Page.xaml 的交互逻辑
    /// </summary>
    public partial class IVMS_Page : Page
    {
        #region 变量

        private readonly IDevice devService;

        #endregion

        #region 属性

        private List<IVMSTreeListViewModel> deviceTreeList = new List<IVMSTreeListViewModel>();
        /// <summary>
        /// 设备树形结构
        /// </summary>
        public List<IVMSTreeListViewModel> DeviceTreeList
        {
            get { return deviceTreeList; }
            set
            {
                deviceTreeList = value;
                Dispatcher.BeginInvoke(new Action(() => { tv_orgList.ItemsSource = deviceTreeList; }));
            }
        }

        #endregion

        #region 依赖属性
        #endregion

        public IVMS_Page()
        {
            InitializeComponent();
            devService = new DeviceService();
        }

        #region 查找
        private void ImgButton_Click(object sender, RoutedEventArgs e)
        {
            DevTreeInfo orgList = new DevTreeInfo();
            var userName = txt_userName.Text;
            var userPwd = txt_pwd.Password;
            WaitingWinBox.Show(new Action(() =>
            {
                orgList = devService.GetDevTreeInfo(userName, userPwd, 1);
            }),"获取组织机构...");
            if ("101".Equals(orgList.MessageCode))
            {
                MessageBox.Show("用户名或密码不正确！");
            }
            else if ("102".Equals(orgList.MessageCode))
            {
                MessageBox.Show("用户账户已经过期！");
            }
            else if ("200".Equals(orgList.MessageCode)) //成功
            {

                MessageBox.Show("登陆成功！");
                InitOrg(orgList);
            }
        }
        #endregion

        #region 初始化组织机构列表
        private void InitOrg(DevTreeInfo devList)
        {
            try
            {
                var orgList = new List<IVMSTreeListViewModel>();
                if (devList.AreaList.Count <= 0)
                    return;

                //获取组织机构parentNode号码
                List<int> rootNumberList = devList.AreaList.Select(x => Convert.ToInt32(x.AreaId)).ToList().Distinct().ToList().OrderBy(x => x).ToList();
                //如果组织机构父节点没有在parentNode里面。返回的数据有错，返回了两条ID相同的数据，所以要做处理
                var rootOrgList = devList.AreaList.Where(x => !rootNumberList.Any(y => y.ToString() == x.ParentAreaId)).ToList();
                //创建组织机构根节点
                foreach (var item in rootOrgList)
                {
                    var orgModel = new IVMSTreeListViewModel();
                    orgModel.ParentNode = item.ParentAreaId;
                    orgModel.Node = item.AreaId;
                    orgModel.NodeName = item.AreaName;
                    orgModel.NodeImage = "/Resources/communication_organization_yrjhzk.png";
                    orgModel.IsNodeExpanded = true;
                    orgList.Add(orgModel);
                }
                //因为获取的数据中有两条ID相同的"青岛分公司"，删除掉父节点不是-1的青岛分公司
                orgList.RemoveAll(x => !"-1".Equals(x.ParentNode));
                //添加组织机构子节点
                AddOrgNode(orgList, devList.AreaList.ToList());
                //添加设备
                AddDeviceNode(orgList, devList.DeviceNodeList.ToList());
                DeviceTreeList = orgList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddDeviceNode(List<IVMSTreeListViewModel> list, List<DeviceNode> deviceNodeList)
        {
            foreach (var item in list)
            {
                if (deviceNodeList.Any(x => x.ParentNodeId == item.Node))
                {
                    var newList = deviceNodeList.Where(x => x.ParentNodeId == item.Node).ToList();

                    foreach (var itemNew in newList)
                    {
                        var model = new IVMSTreeListViewModel();
                        model.ParentNode = itemNew.ParentNodeId;
                        model.NodeName = itemNew.Devicename;
                        model.Node = itemNew.Deviceid;
                        model.NodeData = itemNew;
                        model.NodeImage = "/Resources/tree_tvwall_monitor_list3.png";

                        item.ChildrenList.Add(model);
                        deviceNodeList.RemoveAll(x => x.Deviceid == model.Node);
                    }
                }
                AddDeviceNode(item.ChildrenList, deviceNodeList);
            }
        }

        private void AddOrgNode(List<IVMSTreeListViewModel> orgList, List<Area> areaList)
        {
            foreach (var item in orgList)
            {
                if (areaList.Any(x => x.ParentAreaId == item.Node))
                {
                    var newList = areaList.Where(x => x.ParentAreaId == item.Node).ToList();
                    foreach (var itemNew in newList)
                    {
                        var orgModel = new IVMSTreeListViewModel();
                        orgModel.Node = itemNew.AreaId;
                        orgModel.NodeName = itemNew.AreaName;
                        orgModel.ParentNode = item.Node;
                        orgModel.NodeData = itemNew;
                        orgModel.NodeImage = "/Resources/communication_organization_yrjhwzk.png";

                        item.ChildrenList.Add(orgModel);
                        areaList.RemoveAll(x => x.AreaId == orgModel.Node);
                    }
                }
                AddOrgNode(item.ChildrenList, areaList);
            }
        }
        #endregion
    }
}
