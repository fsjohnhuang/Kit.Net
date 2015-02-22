using lpp.EnumHelper;
using lpp.LogHelper;
using lpp.StringHelper;
using lpp.WinFormController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace lpp.WinFormController
{
    public partial class FileBrowser : Form
    {
        public const string NEW_TXT = "新建文本文件";
        public const string NEW_DIR = "新建文件夹";

        public string Ext { get; set; } // 显示的文件后缀, 空表示所有;用|分割多个后缀
        public FilterType FilterType { get; set; } // 显示的文件、文件夹还是全部
        public FilterType SelectableType { get; set; } // 可选的文件类型
        public string Catable = ""; // 可查看的文件后缀名
        public string Editable = ""; // 可编辑的文件后缀名
        public string Downloadable = ""; // 可下载的文件后缀名

        private ImageList imgLst = new ImageList();

        #region Delegate
        // 根据父节点绝对路径获取子节点
        public delegate Dictionary<string, List<FileTreeNodeInfo>> GetSubFileTreeNodeInfosDelegate(List<string> fullPaths);
        // 点击提交按钮
        public delegate void SubmitDelegate(string path);
        // 操作文件、文件夹
        public delegate bool OperItemDelegate(string path);
        // 重命名
        public delegate bool RenameDelegate(bool isDir, string origPath, string newPath);
        // 获取文件内容
        public delegate string CatDelegate(string path);
        // 保存文件内容修改结果
        public delegate bool SaveDelegate(string path, string content);
        // 下载文件
        public delegate byte[] DownloadDelegate(string path);
        #endregion

        #region Action
        // 根据父节点绝对路径获取子节点
        public GetSubFileTreeNodeInfosDelegate GetSubFileTreeNodeInfos;
        // 点击提交按钮
        public SubmitDelegate Submit;
        // 新增文件
        public OperItemDelegate AddTxt;
        // 新增文件夹
        public OperItemDelegate AddDir;
        // 删除文件
        public OperItemDelegate DelFile;
        // 删除文件夹
        public OperItemDelegate DelDir;
        // 重命名
        public RenameDelegate Rename;
        // 读取文件内容
        public CatDelegate Cat;
        // 保存文件内容修改结果
        public SaveDelegate SaveEdit;
        // 下载文件
        public DownloadDelegate Download;
        #endregion

        #region Initialize
        public FileBrowser(string title, string ext = "", FilterType filterType = null, Icon icon = null, FilterType selectableType = null)
        {
            if (null == filterType)
            {
                filterType = FilterType.ANY;
            }

            InitializeComponent();

            try
            {
                this.Text = title;
                Ext = ext;
                FilterType = filterType;
                if (selectableType == null)
                {
                    SelectableType = FilterType.DIR;
                }
                else
                {
                    SelectableType = selectableType;
                }
                if (icon != null)
                {
                    this.Icon = icon;
                }

                // 初始化ImageList
                imgLst.Images.AddRange(FileBrowserImg.GetAllImgs().ToArray());
                tvFile.ImageList = imgLst;

                InitEvent();
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        public void InitEvent()
        {
            tvFile.BeforeExpand += tvFile_BeforeExpand;
            tvFile.AfterExpand += tvFile_AfterExpand;
            tvFile.AfterCollapse += tvFile_AfterCollapse;

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            tvFile.MouseDown += tvFile_MouseDown;
            tvFile.KeyDown += tvFile_KeyDown;
            cms.ItemClicked += cms_ItemClicked;
            tsmiCreate.DropDownItemClicked += tsmiCreate_DropDownItemClicked;
        }

        public void Init(List<FileTreeNodeInfo> fileTreeNodeInfos)
        {
            // 配置菜单
            if (AddTxt == null && AddDir == null && Rename == null && DelDir == null && DelFile == null
                && Str.IsNullOrWhiteSpace(Catable) && Str.IsNullOrWhiteSpace(Editable))
            {
                tvFile.MouseDown -= tvFile_MouseDown;
            }

            tvFile.Nodes.AddRange(BindData(fileTreeNodeInfos, true).ToArray());
        }
        #endregion

        #region Event handlers

        void tvFile_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                // 获取文件夹子节点的全路径数据
                List<string> fullPaths = new List<string>();
                foreach (TreeNode childNode in e.Node.Nodes)
                {
                    if (childNode.ImageIndex == FileBrowserImg.DIR.Value)
                    {
                        fullPaths.Add(childNode.Name);
                    }
                }

                if (fullPaths.Count != 0)
                {
                    Dictionary<string, List<FileTreeNodeInfo>> subFileTreeNodeInfos = GetSubFileTreeNodeInfos(fullPaths);
                    foreach (string fullPath in subFileTreeNodeInfos.Keys)
                    {
                        foreach (TreeNode childNode in e.Node.Nodes)
                        {
                            if (string.Equals(childNode.Name, fullPath))
                            {
                                childNode.Nodes.AddRange(BindData(subFileTreeNodeInfos[fullPath]).ToArray());
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void tvFile_AfterExpand(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Parent != null)
                {
                    e.Node.ImageIndex = FileBrowserImg.DIR_OPEN.Value;
                    e.Node.SelectedImageIndex = FileBrowserImg.DIR_OPEN.Value;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void tvFile_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Parent != null)
                {
                    e.Node.ImageIndex = FileBrowserImg.DIR.Value;
                    e.Node.SelectedImageIndex = FileBrowserImg.DIR.Value;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode selectedNode = tvFile.SelectedNode;
                if (null == selectedNode || selectedNode.ImageIndex == FileBrowserImg.DRIVER.Value)
                {
                    MsgBox.ShowInfo("提示", string.Format("请选择{0}!", SelectableType.Str));
                }
                else if (selectedNode.ImageIndex == FileBrowserImg.DIR.Value && SelectableType.Value == FilterType.FILE.Value)
                {
                    MsgBox.ShowInfo("提示", string.Format("请选择{0}!", SelectableType.Str));
                }
                else if (selectedNode.ImageIndex != FileBrowserImg.DIR.Value && selectedNode.ImageIndex != FileBrowserImg.DIR_OPEN.Value && SelectableType.Value == FilterType.DIR.Value)
                {
                    MsgBox.ShowInfo("提示", string.Format("请选择{0}!", SelectableType.Str));
                }
                else
                {
                    this.Close();
                    Submit(selectedNode.Name);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void tvFile_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (tvFile.SelectedNode != null && e.KeyCode == Keys.F2)
                {
                    RenameTreeNode();
                }
                else if(e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void tvFile_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    tvFile.SelectedNode = tvFile.HitTest(e.X, e.Y).Node;
                    
                    if(tvFile.SelectedNode.ImageIndex == FileBrowserImg.DRIVER.Value){
                        // 节点为盘符
                        tsmiCat.Visible = false;
                        tsmiEdit.Visible = false;

                        tsmiCreate.Visible = AddTxt != null || AddDir != null;
                        tsmiTxt.Visible = AddTxt != null;
                        tsmiDir.Visible = AddDir != null;
                        tss.Visible = tsmiTxt.Visible && tsmiDir.Visible;

                        tsmiRename.Visible = false;
                        tsmiDelete.Visible = false;
                    }
                    else if (tvFile.SelectedNode.ImageIndex == FileBrowserImg.DIR.Value
                        || tvFile.SelectedNode.ImageIndex == FileBrowserImg.DIR_OPEN.Value)
                    {
                        // 节点为目录
                        tsmiCat.Visible = false;
                        tsmiEdit.Visible = false;

                        tsmiCreate.Visible = AddTxt != null || AddDir != null;
                        tsmiTxt.Visible = AddTxt != null;
                        tsmiDir.Visible = AddDir != null;
                        tss.Visible = tsmiTxt.Visible && tsmiDir.Visible;

                        tsmiRename.Visible = Rename != null;
                        tsmiDelete.Visible = DelDir != null;
                    }
                    else
                    {
                        // 节点为非盘符和非目录
                        string[] catExts = Catable.Split('|');
                        string[] editExts = Editable.Split('|');
                        string[] downloadExts = Downloadable.Split('|');
                        tsmiCat.Visible = Str.IsMatch(tvFile.SelectedNode.Name, catExts, MatchType.SUFFIX);
                        tsmiEdit.Visible = Str.IsMatch(tvFile.SelectedNode.Name, editExts, MatchType.SUFFIX);
                        tsmiDownload.Visible = Str.IsMatch(tvFile.SelectedNode.Name, downloadExts, MatchType.SUFFIX);

                        tsmiCreate.Visible = false;
                        tsmiTxt.Visible = false;
                        tsmiDir.Visible = false;
                        tss.Visible = false;

                        tsmiRename.Visible = Rename != null;
                        tsmiDelete.Visible = DelFile != null;
                    }

                    cms.Show(tvFile, new Point(e.X, e.Y));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void cms_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (string.Equals(e.ClickedItem.Name, tsmiCat.Name))
                {
                    string content = Cat(tvFile.SelectedNode.Name);
                    FileEditor feCat = new FileEditor("浏览：" + tvFile.SelectedNode.Text, content);
                    feCat.ShowDialog();
                }
                else if (string.Equals(e.ClickedItem.Name, tsmiEdit.Name))
                {
                    string content = Cat(tvFile.SelectedNode.Name);
                    FileEditor feEdit = new FileEditor("编辑：" + tvFile.SelectedNode.Text, content, (newContent) => {
                        bool isSuccess = false;
                        isSuccess = SaveEdit(tvFile.SelectedNode.Name, newContent);

                        return isSuccess;
                    });
                    feEdit.ShowDialog();
                }
                else if (string.Equals(e.ClickedItem.Name, tsmiRename.Name))
                {
                    RenameTreeNode();
                }
                else if (string.Equals(e.ClickedItem.Name, tsmiDelete.Name))
                {
                    cms.Close();
                    MsgBox.ShowYNQuestion("询问", "确定删除？", () => {
                        if (tvFile.SelectedNode.ImageIndex == FileBrowserImg.DIR.Value)
                        {
                            DelDir(tvFile.SelectedNode.Name);
                        }
                        else
                        {
                            DelFile(tvFile.SelectedNode.Name);
                        }

                        tvFile.SelectedNode.Remove();
                    });
                }
                else if(string.Equals(e.ClickedItem.Name, tsmiDownload.Name)){
                    cms.Close();
                    sfd.DefaultExt = Path.GetExtension(tvFile.SelectedNode.Name);
                    DialogResult dr = sfd.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        string savePath = sfd.FileName;
                        byte[] contentBytes = Download(tvFile.SelectedNode.Name);
                        FileStream fs = null;
                        try
                        {
                            fs = new FileStream(savePath, FileMode.OpenOrCreate);
                            fs.Write(contentBytes, 0, contentBytes.Length);
                        }
                        catch (Exception exc)
                        {
                            Logger.WriteEx2LogFile(exc);
                        }
                        finally
                        {
                            if (null != fs)
                            {
                                fs.Close();
                            }
                        }
                    }
                }
                if (cms != null)
                {
                    cms.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        void tsmiCreate_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (string.Equals(e.ClickedItem.Name, tsmiTxt.Name))
                {
                    List<string> existNames = new List<string>();
                    string[] existNameArray;
                    foreach (TreeNode subTreeNode in tvFile.SelectedNode.Nodes)
                    {
                        if (subTreeNode.ImageIndex != FileBrowserImg.DIR.Value)
                        {
                            existNames.Add(Path.GetFileNameWithoutExtension(subTreeNode.Text));
                        }
                    }
                    existNameArray = existNames.ToArray();

                    string newName = NEW_TXT;
                    int num = 0;
                    bool isExist = true;
                    do
                    {
                        if (num != 0)
                        {
                            newName = string.Format("{0}{1}", NEW_TXT, num);
                        }
                        isExist = Str.IsMatch(newName, existNameArray, MatchType.ALL);
                        ++num;
                    }
                    while (isExist);

                    newName = newName + ".txt";
                    TreeNode newTxt = new TreeNode();
                    newTxt.ImageIndex = FileBrowserImg.TXT.Value;
                    newTxt.SelectedImageIndex = FileBrowserImg.TXT.Value;
                    newTxt.Text = newName;
                    newTxt.Name = tvFile.SelectedNode.Name + Path.DirectorySeparatorChar + newName;
                    if (AddTxt(newTxt.Name))
                    {
                        tvFile.SelectedNode.Nodes.Add(newTxt);
                        tvFile.SelectedNode = newTxt;
                    }
                    else
                    {
                        MsgBox.ShowError("出错", "新增文本文件失败！");
                    }
                }
                else if (string.Equals(e.ClickedItem.Name, tsmiDir.Name))
                {
                    List<string> existNames = new List<string>();
                    string[] existNameArray;
                    foreach (TreeNode subTreeNode in tvFile.SelectedNode.Nodes)
                    {
                        if (subTreeNode.ImageIndex == FileBrowserImg.DIR.Value)
                        {
                            existNames.Add(Path.GetFileNameWithoutExtension(subTreeNode.Text));
                        }
                    }
                    existNameArray = existNames.ToArray();

                    string newName = NEW_DIR;
                    int num = 0;
                    bool isExist = true;
                    do
                    {
                        if (num != 0)
                        {
                            newName = string.Format("{0}{1}", NEW_DIR, num);
                        }
                        isExist = Str.IsMatch(newName, existNameArray, MatchType.ALL);
                        ++num;
                    }
                    while (isExist);

                    TreeNode newDir = new TreeNode();
                    newDir.ImageIndex = FileBrowserImg.DIR.Value;
                    newDir.SelectedImageIndex = FileBrowserImg.DIR_OPEN.Value;
                    newDir.Text = newName;
                    newDir.Name = tvFile.SelectedNode.Name + Path.DirectorySeparatorChar + newName;

                    if (AddDir(newDir.Name))
                    {
                        tvFile.SelectedNode.Nodes.Add(newDir);
                        tvFile.SelectedNode = newDir;
                    }
                    else
                    {
                        MsgBox.ShowError("出错", "新增文本夹失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        #endregion

        #region 私有方法
        private List<TreeNode> BindData(List<FileTreeNodeInfo> fileTreeNodeInfos, bool isRoot = false)
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (FileTreeNodeInfo fileTreeNodeInfo in fileTreeNodeInfos)
            {
                TreeNode treeNode = null;
                if (isRoot)
                {
                    treeNode = new TreeNode()
                    {
                        Text = fileTreeNodeInfo.Name,
                        Name = fileTreeNodeInfo.Path,
                        ImageIndex = FileBrowserImg.DRIVER.Value,
                        SelectedImageIndex = FileBrowserImg.DRIVER.Value
                    };
                }
                else
                {
                    if (fileTreeNodeInfo.FileTreeNodeType == 0)
                    {
                        if (FilterType.Value != WinFormController.FilterType.DIR.Value)
                        {
                            if (!Str.IsNullOrWhiteSpace(Ext))
                            {
                                string[] exts = Ext.Split('|');
                                if (Str.IsMatch(fileTreeNodeInfo.Path, exts, MatchType.SUFFIX)) continue;
                            }

                            int imgIndex = FileBrowserImg.GetValueByStr(fileTreeNodeInfo.Path);
                            treeNode = new TreeNode()
                            {
                                Text = fileTreeNodeInfo.Name,
                                Name = fileTreeNodeInfo.Path,
                                ImageIndex = imgIndex,
                                SelectedImageIndex = imgIndex
                            };
                        }
                    }
                    else
                    {
                        treeNode = new TreeNode()
                        {
                            Text = fileTreeNodeInfo.Name,
                            Name = fileTreeNodeInfo.Path,
                            ImageIndex = FileBrowserImg.DIR.Value,
                            SelectedImageIndex = FileBrowserImg.DIR_OPEN.Value
                        };
                    }
                }

                foreach (FileTreeNodeInfo item in fileTreeNodeInfo.ChildDirs)
                {
                    TreeNode childTreeNode = null;
                    if (item.FileTreeNodeType == 0)
                    {
                        // 文件
                        if (FilterType.Value != FilterType.DIR.Value)
                        {
                            if (!Str.IsNullOrWhiteSpace(Ext))
                            {
                                string[] exts = Ext.Split('|');
                                if (Str.IsMatch(item.Path, exts, MatchType.SUFFIX)) continue;
                            }

                            int imgIndex = FileBrowserImg.GetValueByStr(item.Path);
                            childTreeNode = new TreeNode()
                            {
                                Text = item.Name,
                                Name = item.Path,
                                ImageIndex = imgIndex,
                                SelectedImageIndex = imgIndex
                            };
                        }
                    }
                    else
                    {
                        // 文件夹
                        childTreeNode = new TreeNode()
                        {
                            Text = item.Name,
                            Name = item.Path,
                            ImageIndex = FileBrowserImg.DIR.Value,
                            SelectedImageIndex = FileBrowserImg.DIR_OPEN.Value
                        };
                    }
                    if (null != childTreeNode)
                    {
                        treeNode.Nodes.Add(childTreeNode);
                    }
                }
                if (treeNode != null)
                {
                    treeNodes.Add(treeNode);
                }
            }

            return treeNodes;
        }

        /// <summary>
        /// 修改子节点的Name属性
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        private void UpdateSubTreeNodeName(TreeNode tn, string oldName, string newName)
        {
            Regex regex = new Regex("/" + oldName + "/");
            foreach (TreeNode subTreeNode in tn.Nodes)
            {
                subTreeNode.Name = regex.Replace(subTreeNode.Name, newName);
                if (subTreeNode.ImageIndex == FileBrowserImg.DIR.Value)
                {
                    UpdateSubTreeNodeName(subTreeNode, oldName, newName);
                }
            }
        }

        private void RenameTreeNode()
        {
            RenameDialog rd = new RenameDialog(Path.GetFileNameWithoutExtension(tvFile.SelectedNode.Text), (newName) =>
            {
                bool isSuccess = false;
                string oldName = Path.GetFileNameWithoutExtension(tvFile.SelectedNode.Text);
                string newFullName = tvFile.SelectedNode.Text.Replace(Path.GetFileNameWithoutExtension(tvFile.SelectedNode.Text), newName);
                string newPath = Path.GetDirectoryName(tvFile.SelectedNode.Name) + Path.DirectorySeparatorChar + newFullName;
                isSuccess = Rename(tvFile.SelectedNode.ImageIndex == FileBrowserImg.DIR.Value, tvFile.SelectedNode.Name, newName);
                if (isSuccess)
                {
                    tvFile.SelectedNode.Text = newFullName;
                    tvFile.SelectedNode.Name = newPath;
                    if (tvFile.SelectedNode.ImageIndex == FileBrowserImg.DIR.Value)
                    {
                        // 重命名文件夹时，要修改其子文件和文件夹的路径信息
                        UpdateSubTreeNodeName(tvFile.SelectedNode, oldName, newName);
                    }
                }

                return isSuccess;
            });
            rd.ShowDialog();
        }
        #endregion
    }

    //public enum FilterType
    //{
    //    ANY = 0,
    //    DIR = 1,
    //    FILE = 2
    //}

    public sealed class FilterType : BaseEnum
    {
        public static FilterType ANY = new FilterType(0, "项");
        public static FilterType DIR = new FilterType(1, "目录");
        public static FilterType FILE = new FilterType(2, "文件");

        private FilterType(int value, string str)
            : base(value, str)
        {
        }
    }


    public class FileTreeNodeInfo
    {
        public string Name { get; set; } // 文件、文件夹、盘符名称
        public string Path { get; set; } // 完整路径
        public int FileTreeNodeType { get; set; } // 0：文件, 1：文件夹, 2: 盘符,
        public List<FileTreeNodeInfo> ChildDirs = new List<FileTreeNodeInfo>();
    }

    internal sealed class FileBrowserImg : BaseEnum
    {
        public static FileBrowserImg DEFAULT = new FileBrowserImg(0, "default", "lpp.WinFormController.fileBrowserImgs.docs.gif");
        public static FileBrowserImg DIR = new FileBrowserImg(1, "dir","lpp.WinFormController.fileBrowserImgs.folder.gif");
        public static FileBrowserImg DIR_OPEN = new FileBrowserImg(2, "dir_open", "lpp.WinFormController.fileBrowserImgs.folder-open.gif");
        public static FileBrowserImg XHTML = new FileBrowserImg(3, ".xhtml","lpp.WinFormController.fileBrowserImgs.xhtml.png");
        public static FileBrowserImg JS = new FileBrowserImg(4, ".js","lpp.WinFormController.fileBrowserImgs.script_code.png");
        public static FileBrowserImg PPT = new FileBrowserImg(5, ".ppt", "lpp.WinFormController.fileBrowserImgs.powerpoint.png");
        public static FileBrowserImg PPTX = new FileBrowserImg(6, ".pptx", "lpp.WinFormController.fileBrowserImgs.powerpoint.png");
        public static FileBrowserImg TXT = new FileBrowserImg(7, ".txt", "lpp.WinFormController.fileBrowserImgs.report.png");
        public static FileBrowserImg COFFEE = new FileBrowserImg(8, ".coffee", "lpp.WinFormController.fileBrowserImgs.play.png");
        public static FileBrowserImg PIC = new FileBrowserImg(9, ".pic", "lpp.WinFormController.fileBrowserImgs.photos.png");
        public static FileBrowserImg JPG = new FileBrowserImg(10, ".jpg", "lpp.WinFormController.fileBrowserImgs.photos.png");
        public static FileBrowserImg PNG = new FileBrowserImg(11, ".png", "lpp.WinFormController.fileBrowserImgs.picture.png");
        public static FileBrowserImg DOC = new FileBrowserImg(12, ".doc", "lpp.WinFormController.fileBrowserImgs.page_white_word.png");
        public static FileBrowserImg DOCX = new FileBrowserImg(13, ".docx", "lpp.WinFormController.fileBrowserImgs.page_white_word.png");
        public static FileBrowserImg RB = new FileBrowserImg(14, ".rb", "lpp.WinFormController.fileBrowserImgs.page_white_ruby.png");
        public static FileBrowserImg FLX = new FileBrowserImg(15, ".flx", "lpp.WinFormController.fileBrowserImgs.page_white_flash.png");
        public static FileBrowserImg RAR = new FileBrowserImg(16, ".rar", "lpp.WinFormController.fileBrowserImgs.1-11112R01531-50.png");
        public static FileBrowserImg ZIP = new FileBrowserImg(17, ".zip", "lpp.WinFormController.fileBrowserImgs.1-11112R01531-50.png");
        public static FileBrowserImg C = new FileBrowserImg(18, ".c", "lpp.WinFormController.fileBrowserImgs.page_white_c.png");
        public static FileBrowserImg XLS = new FileBrowserImg(19, ".xls", "lpp.WinFormController.fileBrowserImgs.page_excel.png");
        public static FileBrowserImg XLSX = new FileBrowserImg(20, ".xlsx", "lpp.WinFormController.fileBrowserImgs.page_excel.png");
        public static FileBrowserImg HTML = new FileBrowserImg(21, ".html", "lpp.WinFormController.fileBrowserImgs.html.png");
        public static FileBrowserImg MOV = new FileBrowserImg(22, ".mov", "lpp.WinFormController.fileBrowserImgs.film.png");
        public static FileBrowserImg WMV = new FileBrowserImg(23, ".wmv", "lpp.WinFormController.fileBrowserImgs.film.png");
        public static FileBrowserImg CSS = new FileBrowserImg(24, ".css", "lpp.WinFormController.fileBrowserImgs.css.png");
        public static FileBrowserImg JAVA = new FileBrowserImg(25, ".java", "lpp.WinFormController.fileBrowserImgs.cup.png");
        public static FileBrowserImg JAR = new FileBrowserImg(26, ".jar", "lpp.WinFormController.fileBrowserImgs.1-11112R01531-50.png");
        public static FileBrowserImg BAK = new FileBrowserImg(27, ".bat", "lpp.WinFormController.fileBrowserImgs.application_xp_terminal.png");
        public static FileBrowserImg SH = new FileBrowserImg(28, ".sh", "lpp.WinFormController.fileBrowserImgs.tux.png");
        public static FileBrowserImg DLL = new FileBrowserImg(29, ".dll", "lpp.WinFormController.fileBrowserImgs.application_link.png");
        public static FileBrowserImg EXE = new FileBrowserImg(30, ".exe", "lpp.WinFormController.fileBrowserImgs.application_view_tile.png");
        public static FileBrowserImg DRIVER = new FileBrowserImg(31, "driver", "lpp.WinFormController.fileBrowserImgs.drive.png");
        public static FileBrowserImg SLN = new FileBrowserImg(32, ".sln", "lpp.WinFormController.fileBrowserImgs.page_white_visualstudio.png");
        public static FileBrowserImg SUO = new FileBrowserImg(33, ".suo", "lpp.WinFormController.fileBrowserImgs.page_white_visualstudio.png");
        public static FileBrowserImg PHP = new FileBrowserImg(34, ".php", "lpp.WinFormController.fileBrowserImgs.page_white_php.png");
        public static FileBrowserImg CPP = new FileBrowserImg(35, ".c++", "lpp.WinFormController.fileBrowserImgs.page_white_cplusplus.png");
        public static FileBrowserImg CS = new FileBrowserImg(36, ".cs", "lpp.WinFormController.fileBrowserImgs.page_white_csharp.png");
        public static FileBrowserImg LDF = new FileBrowserImg(37, ".ldf", "lpp.WinFormController.fileBrowserImgs.page_white_database.png");
        public static FileBrowserImg MDF = new FileBrowserImg(38, ".mdf", "lpp.WinFormController.fileBrowserImgs.server_database.png");
        public static FileBrowserImg PDF = new FileBrowserImg(39, ".pdf", "lpp.WinFormController.fileBrowserImgs.page_white_acrobat.png");
        public static FileBrowserImg PPS = new FileBrowserImg(40, ".pps", "lpp.WinFormController.fileBrowserImgs.powerpoint.png");
        public static FileBrowserImg GIF = new FileBrowserImg(41, ".gif", "lpp.WinFormController.fileBrowserImgs.images.png");

        private static List<BaseEnum> items = new List<BaseEnum>() { DEFAULT, DIR, DIR_OPEN, XHTML, JS, PPT, PPTX, TXT, COFFEE, PIC
                , JPG, PNG, DOC, DOCX, RB, FLX, RAR, ZIP, C, XLS
                , XLSX, HTML, MOV, WMV, CSS, JAVA, JAR, BAK, SH, DLL
                , EXE, DRIVER, SLN, SUO, PHP, CPP, CS, LDF, MDF, PDF
                , PPS, GIF };


        public Image Img = null;
        private static IDictionary<int, string> dic;
        private static IDictionary<string, int> strToIntDic;

        private FileBrowserImg(int value, string str, string path)
            : base(value, str)
        {
            Img = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(path));
        }

        public static Dictionary<int, string> ToDic()
        {
            return BaseEnum.ToDic(items);
        }

        public static Dictionary<string, int> ToStr2IntDic()
        {
            return BaseEnum.ToStr2IntDic(items);
        }

        public static List<Image> GetAllImgs()
        {
            List<Image> imgs = new List<Image>() { DEFAULT.Img, DIR.Img, DIR_OPEN.Img, XHTML.Img, JS.Img, PPT.Img, PPTX.Img, TXT.Img, COFFEE.Img, PIC.Img
                , JPG.Img, PNG.Img, DOC.Img, DOCX.Img, RB.Img, FLX.Img, RAR.Img, ZIP.Img, C.Img, XLS.Img
                , XLSX.Img, HTML.Img, MOV.Img, WMV.Img, CSS.Img, JAVA.Img, JAR.Img, BAK.Img, SH.Img, DLL.Img
                , EXE.Img, DRIVER.Img, SLN.Img, SUO.Img, PHP.Img, CPP.Img, CS.Img, LDF.Img, MDF.Img, PDF.Img
                , PPS.Img, GIF.Img  };
            return imgs;
        }

        public static string GetStrByValue(int value)
        {
            if (dic == null)
            {
                lock (typeof(FileBrowserImg))
                {
                    if (dic == null)
                    {
                        dic = ToDic();
                    }
                }
            }

            if (!dic.ContainsKey(value)) return null;

            return dic[value];
        }

        public static int GetValueByStr(string str)
        {
            if (strToIntDic == null)
            {
                lock (typeof(FileBrowserImg))
                {
                    if (strToIntDic == null)
                    {
                        strToIntDic = ToStr2IntDic();
                    }
                }
            }

            // 获取后缀
            int index = str.LastIndexOf(".");
            string ext = string.Empty;
            if (index != -1)
            {
                ext = str.Substring(index);
                ext = ext.ToLower();
            }

            if (!strToIntDic.ContainsKey(ext)) return 0;

            return strToIntDic[ext];
        }
    }
}
