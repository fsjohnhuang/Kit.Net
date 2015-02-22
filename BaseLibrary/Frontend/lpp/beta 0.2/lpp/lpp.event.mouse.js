!function () {
    this.lpp.event.mouse = {
        /*
	    ** @禁止全局鼠标默认右键菜单(需在DOM树加载完成后调用)
	    */
        preventGobalContextMenu: function () {
            lpp(document).on("contextmenu", function (e) {
                return false;
            });
        },
        /*
        ** @禁止局部鼠标默认右键菜单(需在DOM树加载完成后调用)
        ** @param target：局部容器对象的jquery选择器字符串或dom、jquery对象
        */
        preventContextMenu: function (target) {
            lpp(target).on("contextmenu", function (e) {
                return false;
            });
        },
        /*
	    ** @禁止局部鼠标默认右键菜单,而执行自定义操作(需在DOM树加载完成后调用)
	    ** @param target：局部容器的jquery选择器字符串或dom、jquery对象
	    ** @param actions：按键事件处理函数集对象
	    **        actions = {lAct:function(){},mAct:function(){},rAct:function(){}}
	    */
        setMouseDown: function (target, actions) {
            lpp(target).on("contextmenu", function (e) {
                return false;
            });

            lpp(target).on("mousedown", function (e) {
                if (1 === e.which) {
                    // 左键
                    if ("function" === typeof actions.lAct) actions.lAct();
                    else if (e.ctrlKey && "function" === typeof actions.lCtrlAct) actions.lCtrlAct();
                    else if (e.altKey && "function" === typeof actions.lAltAct) actions.lAltAct();
                }
                else if (2 === e.which && "function" === typeof actions.mAct) {
                    // 滚轮键
                    actions.mAct();
                }
                else if (3 === e.which) {
                    // 右键
                    if ("function" === typeof actions.rAct) actions.rAct();
                    else if (e.ctrlKey && "function" === typeof actions.rCtrlAct) actions.rCtrlAct();
                    else if (e.altKey && "function" === typeof actions.rAltAct) actions.rAltAct();
                }
            });
        },
        /*
	    ** 获取鼠标滑轮对象
	    ** @param eventObj{object} 事件对象
	    */
        getMouseWheelAct: function (eventObj){
		    var d = eventObj.detail || eventObj.wheelDelta;// detail的取值范围为+-3;wheelDelta为+-120；整数为向上，负数为向下。
            return d;
        },
        /*
	    ** 获取鼠标进入或离开容器的方向
	    ** @param containerId{string} 容器的ID
	    ** @param eventObj{object} 事件对象
	    ** @return direction{int} 方向值（0:上，1:右，2:下，3:左）
	    */
        hoverDir: function (containerId, eventObj){
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
    }
}();