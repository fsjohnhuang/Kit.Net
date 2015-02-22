!function () {
    this.lpp.browser = {
        /*
	    ** @浏览器可视域的高度
	    */
        clientHeight: function (){
		    var de = document.documentElement;
            return self.innerHeight || (de && de.clientHeight) || document.body.clientHeight;
        },
        /*
        ** @浏览器可视域的宽度
        */
        clientWidth: function (){
            var de = document.documentElement;
            return self.innerWidth || (de && de.clientWidth) || document.body.clientWidth;
        },
        /*
        ** @浏览器垂直滚动位置
        */
        scrollY: function(){
            var de = document.documentElement;
            return self.pageYOffset || (de && de.scrollTop) || document.body.scrollTop;
        },
        /*
        ** @浏览器水平滚动位置
        */
        scrollX: function (){
            var de = document.documentElement;
            return self.pageXOffset || (de && de.scrollLeft) || document.body.scrollLeft;
        },
        /*
        ** @内容高度
        */
        contentHeight: function () {
            return document.body.scrollHeight;
        },
        /*
        ** @内容宽度
        */
        contentWidth: function () {
            return document.body.scrollWidth;
        }
    };
}();
