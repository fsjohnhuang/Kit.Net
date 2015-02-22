var commonjs = (function() {
	
	// @region BOM属性操作模块

	/*
	** @浏览器可视域的高度
	*/
	function windowHeight(){
		var de = document.documentElement;
	    return self.innerHeight || (de && de.clientHeight) || document.body.clientHeight;
	}

	/*
	** @浏览器可视域的宽度
	*/
	function windowWidth(){
		var de = document.documentElement;
		return self.innerWidth || (de && de.clientWidth) || document.body.clientWidth;
	}

	/*
	** @浏览器垂直滚动位置
	*/
	function scrollY(){
		var de = document.documentElement;
		return self.pageYOffset || (de && de.scrollTop) || document.body.scrollTop;
	}

	/*
	** @浏览器水平滚动位置
	*/
	function scrollX(){
		var de = document.documentElement;
		return self.pageXOffset || (de && de.scrollLeft) || document.body.scrollLeft;
	}

	/*
	** @内容高度
	*/
	function pageHeight() {
		return document.body.scrollHeight;
	}

	/*
	** @内容宽度
	*/
	function pageWidth() {
		return document.body.scrollWidth;
	}


    /*
	** @根据URL参数名获取参数值
	** @param paramKey:参数键名
	** @return 参数值
	*/
    function getReqParamVal(paramKey) {
        var url = location.href;
        var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
        var paraObj = {}
        for (i = 0; j = paraString[i]; i++) {
            paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
        }
        var returnValue = paraObj[paramKey.toLowerCase()];
        if (typeof (returnValue) == "undefined") {
            return "";
        } else {
            return returnValue;
        }
    }

	// @endregion BOM属性操作模块


	// @region 事件辅佐模块

	/*
	** @获取事件信息对象
	** @param arguments: 调用方法的方法内置参数对象
	*/
	function getEventObj(arguments){
		return window.event || arguments.callee.caller.arguments[0];
	}

	/*
	** @禁止事件冒泡
	** @param arguments: 调用方法的方法内置参数对象
	*/
	function stopPropagation(arguments){
		if (window.event){
			window.event.cancelBubble=true;
		}
		else{
			arguments.callee.caller.arguments[0].stopPropagation();
		}
	}

	/*
	** @禁止事件原有操作
	** @param arguments: 调用方法的方法内置参数对象
	*/
	function preventDefault(arguments){
		var e = getEventObj(arguments);
		if (e.preventDefault){
			e.preventDefault();
		}
		else{
			e.returnValue = false;
		}
	}

	/*
	** @禁止全局鼠标默认右键菜单(需在DOM树加载完成后调用)
	*/
	function preventGobalContextMenu(){
		$(document).bind("contextmenu",function(e){
			return false;
		});
	}

	/*
	** @禁止局部鼠标默认右键菜单(需在DOM树加载完成后调用)
	** @param target：局部容器对象的jquery选择器字符串或dom、jquery对象
	*/
	function preventContextMenu(target){
		$(target).bind("contextmenu",function(e){
			return false;
		});
	}

	/*
	** @禁止局部鼠标默认右键菜单,而执行自定义操作(需在DOM树加载完成后调用)
	** @param target：局部容器的jquery选择器字符串或dom、jquery对象
	** @param actions：按键事件处理函数集对象
	**        actions = {lAct:function(){},mAct:function(){},rAct:function(){}}
	*/
	function setMouseDown(target, actions){
		$(target).bind("contextmenu",function(e){
			return false;
		});

		$(target).bind("mousedown",function(e){
			if (1 === e.which){
				// 左键
				if ("function" === typeof actions.lAct) actions.lAct();
				else if (e.ctrlKey && "function" ===  typeof actions.lCtrlAct) actions.lCtrlAct();
				else if (e.altKey && "function" ===  typeof actions.lAltAct) actions.lAltAct();
			}
			else if (2 === e.which && "function" === typeof actions.mAct){
				// 滚轮键
				actions.mAct();
			}
			else if (3 === e.which){
				// 右键
				if ("function" === typeof actions.rAct) actions.rAct();
				else if (e.ctrlKey && "function" ===  typeof actions.rCtrlAct) actions.rCtrlAct();
				else if (e.altKey && "function" ===  typeof actions.rAltAct) actions.rAltAct();
			}
		});
	}

	/*
	** @禁止全局Esc键默认操作(需在DOM树加载完成后调用)
	*/
	function preventGobalEsc(){
		$(document).bind("keydown",function(e){
			if (e.keyCode == 27){
				return false;
			}
		});
	}

	/*
	** @禁止Esc键默认操作,而执行自定义操作(需在DOM树加载完成后调用)
	** @param target：局部容器的jquery选择器字符串或dom、jquery对象
	** @param fns
	**        fns = [function(e){}, function(e){}]
	*/
	function setEscKeyDown(target, fns){
		$(target).bind("keydown",function(e){
			if (e.keyCode == 27){
				$.each(fns, function(i, fn){
					fn(e);
				});

				return false;
			}
		});
	}
	
	/*
	** 获取鼠标滑轮对象
	** @param eventObj{object} 事件对象
	*/
	function getMouseWheelAct(eventObj){
		var d = eventObj.detail || eventObj.wheelDelta;// detail的取值范围为+-3;wheelDelta为+-120；整数为向上，负数为向下。
		return d;
	}
	
	/*
	** 获取鼠标进入或离开容器的方向
	** @param containerId{string} 容器的ID
	** @param eventObj{object} 事件对象
	** @return direction{int} 方向值（0:上，1:右，2:下，3:左）
	*/
	function hoverDir(containerId, eventObj){
		var wrap = document.getElementById(containerId);
		var w=wrap.offsetWidth;
		var h=wrap.offsetHeight;
		var x=(eventObj.clientX - wrap.offsetLeft - (w / 2)) * (w > h ? (h / w) : 1);
		var y=(eventObj.clientY - wrap.offsetTop - (h / 2)) * (h > w ? (w / h) : 1);
		var direction = Math.round((((Math.atan2(y, x) * (180 / Math.PI)) + 180) / 90) + 3) % 4;
		var eventType = e.type;
		var dirName = new Array('上方','右侧','下方','左侧');
		return direction;
	}
	
	/*
	** @函数节流
	** @param fn 实际函数
	** @param delay 函数动作延期时间
	** @param mustRunDelay 函数必须执行的延期时间
	*/
	function throttle(fn, delay, mustRunDelay){
		var _timer = null, _startTime = null;
	
		return function(){
			var _cxt = this, _args = arguments, _curTime = +new Date();
			clearTimeout(_timer);
			
			if (!_startTime){
				_startTime = _curTime;
			}
			if (_curTime - _startTime >= mustRunDelay){
				fn.apply(_cxt, _args);
				_startTime = _curTime;
			}
			else{
				_timer = setTimeout(function(){
					fn.apply(_cxt, _args);
				}, delay);
			}
		};
	}
	
	/*
	** @禁止通过IFrame成为内页
	*/
	function ForbidAsIFrame(){
		if (top !== window){
			top.location.href = window.location.href;
		}
	}

	// @endregion 事件辅佐模块


	// @region 文本辅佐模块

	/*
	** @组装url
	** @param url: url地址
	** @param queryParams: queryString参数对象
	*/
	function combineUrl(url, queryParams){
		if ("#" !== url && "object" == typeof queryParams && !$.isEmptyObject(queryParams)){
			var queryStr = "";
			$.each(queryParams, function(k, v){
				queryStr += k + "=" + encodeURIComponent(v) + "&";
			});
			if (queryStr.length >= 1){
				queryStr = queryStr.substring(0, queryStr.length-1);
			}

			if (url.indexOf("?")>=0){
				url += "&" + queryStr;
			}
			else{
				url += "?" + queryStr;
			}
		}

		return url;
	}

	/*
	** @删除左右两端的空格
	*/
	function trim(str){ 
　　	return str.replace(/(^\s*)|(\s*$)/g, "");
　　}

	/*
	** @删除左边的空格
	*/
	function ltrim(str){ 
　　	return str.replace(/(^\s*)/g,"");
　　}

	/*
	** @删除右边的空格
	*/
	function rtrim(str){
　　	return str.replace(/(\s*$)/g,"");
　　}

	// @endregion 文本辅佐模块


	// @region 数据验证模块

	/*
	** @检查是否为有效的固话号码
	*/
	function isValidPhone(phoneNum){
		var isValid = false;
		isValid = /^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$/.test(phoneNum);

		return isValid;
	}

	/*
	** @检查是否为有效的“移动”手机号码
	*/
	function isValidMobileBrandPhone(phoneNum){
		var isValid = false;
		isValid = /^1(34|35|36|37|38|39|50|51|52|58|59|47|82|87|88)[0-9]{8,8}$/.test(phoneNum);

		return isValid;
	}

	/*
	** @检查是否为有效的“联通”手机号码
	*/
	function isValidUnicomBrandPhone(phoneNum){
		var isValid = false;
		isValid = /^1(32|30|31|32|55|56|85|86)[0-9]{8,8}$/.test(phoneNum);

		return isValid;
	}

	/*
	** @检查是否为有效的“电信”手机号码
	*/
	function isValidTelecomBrandPhone(phoneNum){
		var isValid = false;
		isValid = /^133[0-9]{8,8}$/.test(phoneNum);

		return isValid;
	}

	/*
	** @检查是否为有效的身份证号码
	*/
	function isValidID(id){
		var isValid = false;
		isValid = /^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$/.test(id);

		return isValid;
	}

	/*
	** @检查是否为有效的电子邮箱地址
	*/
	function isValidEMail(email){
		var isValid = false;
		isValid = /^w+([-+.]w+)*@w+([-.]w+)*.w+([-.]w+)*$/.test(email);

		return isValid;
	}

	/*
	** @检查是否为有效的中国邮政编码
	*/
	function isValidPostcode(postcode){
		var isValid = false;
		isValid = /^[1-9]{1}(\d+){5}$/.test(postcode);

		return isValid;
	}

	/*
	** @检查是否为全中文字符串
	*/
	function isAllChinese(str){
		var isValid = false;
		isValid = /^[\u4e00-\u9fa5]*$/.test(str);

		return isValid;
	}

	/*
	** @检查是否为有效的url
	*/
	function isValidUrl(url){
		var isValid = false;
		isValid = /^[a-zA-z]+:\/\/[^\s]*$/.test(url);

		return isValid;
	}

	/*
	** @检查是否为有效的用户账号
	*/
	function isValidAccount(account){
		var isValid = false;
		isValid = /^[a-zA-Z][a-zA-Z0-9_]{4,15}$/.test(account);

		return isValid;
	}

	/*
	** @检查是否为整数
	*/
	function isInt(str){
		var isValid = false;
		isValid = /^(-|\+)?\d+$/.test(str);

		return isValid;
	}

	/*
	** @检查是否为数字（可含小数）
	*/
	function isNumeric(str){
		var isValid = false;
		isValid = !isNaN(str);

		return isValid;
	}

	/*
	** @检查是否为有效的QQ账号
	*/
	function isValidQQ(qq){
		var isValid = false;
		isValid = /^[1-9][0-9]{4,}$/.test(qq);

		return isValid;
	}


	// @endregion 数据验证模块


	// @region 编码模块

	function toASCII(str) {
       return this.ToNormal(str).replace(/[^\u0000-\u00FF]/g, function () { return escape(arguments[0]).replace(/(?:%u)([0-9a-f]{4})/gi, "\$1;") });
   	}
   	function toUnicode(str) {
       return this.ToNormal(str).replace(/[^\u0000-\u00FF]/g, function () { return escape(arguments[0]).replace(/(?:%u)([0-9a-f]{4})/gi, "\\u$1") });
   	}
   	function toNormal(str) {
       return str.replace(/(?:)([0-9a-f]{4});|(?:\\u)([0-9a-f]{4})/gi, function () { return unescape("%u" + (arguments[1] || arguments[2])); });
   	}
   	function toCss(str) {
       return this.ToNormal(str).replace(/[^\u0000-\u00FF]/g, function () { return escape(arguments[0]).replace(/(?:%u)([0-9a-f]{4})/gi, "\\$1") });
   	}

   	// @endregion 编码模块
	
	
	// @region 日期、时间模块
	function getNow(){
		var date = new Date();
		var now = date.getFullYear() + "/";
		now += (date.getMonth() + 1) + "/";
		now += date.getDate() + " ";
		now += date.getHours() + ":";
		now += date.getMinutes() + ":";
		now += date.getSeconds();
		
		return now;
	}
	// @endregion 日期、时间模块

	// @region 其他
	
	/* 获取对象的数据类型
	** @param {variable} o 对象变量
	** @return {string} 入参对象变量的类型名称（小写）
	*/
	function getType(o)
	{
		var _t;
		return ((_t = typeof(o)) == "object" ? o==null && "null" || Object.prototype.toString.call(o).slice(8,-1):_t).toLowerCase();
	}
	
	/* 深度复制
	** @param {array/object} destination 目标对象
	** @param {array/object} source 源对象
	*/
	function extend(destination,source)
	{
		for(var p in source)
		{
			if(getType(source[p])=="array"||getType(source[p])=="object")
			{
				destination[p]=getType(source[p])=="array"?[]:{};
				arguments.callee(destination[p],source[p]);
			}
			else
			{
				destination[p]=source[p];
			}
		}
	}
	
	/*
	** @根据亚拉伯数字获取中文数字
	** @param num:亚拉伯数字
	**        isTradition: 是否使用繁体字
	** @return 中文数字
	*/
	function convertToChineseNum(num,isTrandition){
		var units = Array("","十","百","千","万","十","百","千","亿");
		var digit = "" + num;
		
		var length=digit.length;
		var arr1=new Array(length);
		var arr2=new Array(length);
		var result="";
		for(var i=0;i<length;i++)
		{
			arr1[i] = digit.substr(i,1);
			arr2[i] = _getChineseNum(arr1[i], isTrandition);
			result += arr2[i]+units[length-i-1];
		}
		
		return result;
	}


	// @ 数字金额大写转换(可以处理整数,小数,负数)
	function upDigit(n)  
	{ 
	    var fraction = ['角', '分']; 
	    var digit = ['零', '壹', '贰', '叁', '肆', '伍', '陆', '柒', '捌', '玖']; 
	    var unit = [ ['元', '万', '亿'], ['', '拾', '佰', '仟']  ]; 
	    var head = n < 0? '欠': ''; 
	    n = Math.abs(n); 
	    var s = ''; 
	    for (var i = 0; i < fraction.length; i++)  
	    { 
	        s += (digit[Math.floor(n * 10 * Math.pow(10, i)) % 10] + fraction[i]).replace(/零./, ''); 
	    } 
	    s = s || '整'; 
	    n = Math.floor(n); 
	    for (var i = 0; i < unit[0].length && n > 0; i++)  
	    { 
	        var p = ''; 
	        for (var j = 0; j < unit[1].length && n > 0; j++)  
	        { 
	            p = digit[n % 10] + unit[1][j] + p; 
	            n = Math.floor(n / 10); 
	        } 
	        s = p.replace(/(零.)*零$/, '').replace(/^$/, '零')  + unit[0][i] + s; 
	    } 
	    return head + s.replace(/(零.)*零元/, '元').replace(/(零.)+/g, '零').replace(/^整$/, '零元整'); 
	}

	// @endregion 其他


	// @region 私有方法
	
	/* 
	** @根据单个亚拉伯数字获取当个中文数字
	** @param digit:亚拉伯数字
	**        isTradition: 是否使用繁体字
	** @return 单个中文数字
	*/
	function _getChineseNum(digit, isTradition){
		var input = digit;
		if(input=="0")
			return (isTradition ? "零" : "零");
		else if(input=="1")
			return (isTradition ? "壹" : "一");
		else if(input=="2")
			return (isTradition ? "贰" : "二");
		else if(input=="3")
			return (isTradition ? "叁" : "三");
		else if(input=="4")
			return (isTradition ? "肆" : "四");
		else if(input=="5")
			return (isTradition ? "伍": "五");
		else if(input=="6")
			return (isTradition ? "陆": "六");
		else if(input=="7")
			return (isTradition ? "柒": "七");
		else if(input=="8")
			return (isTradition ? "捌" : "八");
		else if(input=="9")
			return (isTradition ? "玖" : "九");
	}

	// @endregion 私有方法
    
    return {
    	windowHeight: windowHeight,
		windowWidth: windowWidth,
		scrollY: scrollY,
		scrollX: scrollX,
		pageHeight: pageHeight,
		pageWidth: pageWidth,
        getReqParamVal: getReqParamVal,

        combineUrl: combineUrl,
		trim: trim,
		ltrim: ltrim,
		rtrim: rtrim,

		isValidPhone: isValidPhone,
		isValidMobileBrandPhone: isValidMobileBrandPhone,
		isValidUnicomBrandPhone: isValidUnicomBrandPhone,
		isValidTelecomBrandPhone: isValidTelecomBrandPhone,
		isValidID: isValidID,
		isValidEMail: isValidEMail,
		isValidPostcode: isValidPostcode,
		isAllChinese: isAllChinese,
		isValidUrl: isValidUrl,
		isValidAccount: isValidAccount,
		isInt: isInt,
		isNumeric: isNumeric,
		isValidQQ: isValidQQ,

		getEventObj: getEventObj,
		stopPropagation: stopPropagation,
		preventDefault: preventDefault,
		preventGobalContextMenu: preventGobalContextMenu,
		preventContextMenu: preventContextMenu,
		setMouseDown: setMouseDown,
		preventGobalEsc: preventGobalEsc,
		setEscKeyDown: setEscKeyDown,
		hoverDir: hoverDir,
		getMouseWheelAct: getMouseWheelAct,
		throttle: throttle,

		toASCII: toASCII,
		toUnicode: toUnicode,
		toNormal: toNormal,
		toCss: toCss,

		getNow: getNow,
		
		getType: getType,
		extend: extend,
		convertToChineseNum: convertToChineseNum
    };
})()