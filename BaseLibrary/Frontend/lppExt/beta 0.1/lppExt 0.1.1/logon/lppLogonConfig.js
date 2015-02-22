var m_ImageDir = "./lppExt/resources/images/"; // 图片路径

var mainPageUrl = 'index.aspx'; // 登录后进入的页面URL
var loginUrl = './Handlers/Login/Login.ashx'; // 登录验证URL 
var getAuthCodeUrl = '{0}'; // 获取验证码的URL
var resetPwUrl = './Handlers/Login/ResetPw.ashx'; // 重设密码的URL
var sysCode = 'LZ'; // 默认系统编码

var isShowWithAnimate = ''; // 动画显示窗口

var systemBigImg = m_ImageDir + 'logo.gif'; // 登录窗口的头部
var winTitle = '佛山市第六中学网站管理后台'; // 登录窗口标题
var winTitleImg = ''; // 登录窗标题图片
var winLoginTxt = '用户验证'; // 登录窗登录页签标题
var winLoginImg = m_ImageDir + "/home.png"; // 登录窗登录页签图片
var winNoticeTxt = '公告信息'; // 登录窗公告栏页签标题
var winNoticeImg = m_ImageDir + "/message.png"; // 登录窗公告栏页签图片
var winNoticeContent = ''; // 登录窗公告栏页签主内容
var winConfirmImg = m_ImageDir + "/accept.png"; // 登录按钮图片
var winConfirmTxt = '登录'; // 登录按钮文字
var winConfirmTips = ''; // 登录按钮提示
var winOptImg = m_ImageDir + "/tbar_synchronize.png"; // 选项按钮图片
var winOptTxt = '选项'; // 选项按钮文字
var winModifyKeyImg = m_ImageDir + "/config.png"; // 选项按钮图片
var winModifyKeyTxt = '修改密码'; // 选项按钮文字

var connFailTitle = '出错啦！'; // 无法连接提示标题
var connFailTxt = '网络故障，请稍后再试！'; // 无法连接提示内容
var serverFailTitle = '出错啦！'; // 服务端验证失败提示标题
var serverFailTxt = '账号或密码错误！'; // 服务端验证失败提示内容
var loginWaitTitle = '提示'; // 登录时进度条标题
var loginWaitMsg = '正在登录，请稍等......'; // 登录时进度条信息

var winChangeKeyTitle = '密码修改'; // 密码修改窗口标题
var winChangeKeyTitleImg = m_ImageDir + "/config.png"; // 密码修改窗口标题
var changeKeyWaitMsg = '正在提交，请稍等......'; // 密码修改窗口标题





function addStylesheet(id, cssRuleText) {
    var newStyle = document.createElement("STYLE");
    newStyle.id = id;
    newStyle.type = "text/css";
    newStyle.media = "screen";

    if (newStyle.styleSheet) {
        newStyle.styleSheet.cssText = cssRuleText;
    }
    else {
        newStyle.appendChild(document.createTextNode(cssRuleText));
    }
    var parentNode = document.getElementsByTagName("HEAD")[0] || document.body;
    parentNode.appendChild(newStyle);
}
styleStr = ".txtAccount input { background-image:url(" + m_ImageDir + "/user.png); background-repeat: no-repeat; padding-left:20px; background-position: 2px 2px;position:relative;top:0px}"
    + ".txtPw input { background-image:url(" + m_ImageDir + "/key.png); background-repeat: no-repeat; padding-left:20px; background-position: 2px 2px;position:relative;top:0px}"
    + ".txtOldKey input { background-image:url(" + m_ImageDir + "/edit2.png); background-repeat: no-repeat; padding-left:20px; background-position: 2px 2px;position:relative;top:0px}"
    + ".txtNewKey input { background-image:url(" + m_ImageDir + "/key.png); background-repeat: no-repeat; padding-left:20px; background-position: 2px 2px;position:relative;top:0px}"
    + ".txtRepeatKey input { background-image:url(" + m_ImageDir + "/key.png); background-repeat: no-repeat; padding-left:20px; background-position: 2px 2px;position:relative;top:0px}"
    + "#CheckCode{ float:left }"
    + "#CheckCode input{ position:relative;top:0px }"
    + ".x-form-code{width:73px;height:20px;vertical-align:middle;cursor:pointer; float:left; margin-left:7px;margin-top:12px}";
addStylesheet("imgPath", styleStr);