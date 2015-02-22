!function () {
    this.lpp.event = {
        /*
	    ** @获取事件信息对象
	    */
        getEventObj: function _getEventObj() {
            return window.event || _getEventObj.caller.arguments[0];
        },
        /*
	    ** @禁止事件冒泡
	    */
        stopPropagation: function _stopPropagation() {
		    if (window.event){
			    window.event.cancelBubble = true;
            }
            else{
		        _stopPropagation.caller.arguments[0].stopPropagation();
            }
        },
        /*
	    ** @禁止事件原有操作
	    ** @param arguments: 调用方法的方法内置参数对象
	    */
        preventDefault: function _preventDefault() {
            if (window.event) {
                e.returnValue = false;
            }
            else {
                _preventDefault.caller.arguments[0].preventDefault();
            }
        }
    };
}();
