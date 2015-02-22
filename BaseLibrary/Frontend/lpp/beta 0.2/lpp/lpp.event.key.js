!function () {
    this.lpp.event.key = {
        /*
	    ** @禁止全局Esc键默认操作(需在DOM树加载完成后调用)
	    */
        preventGobalEsc: function (){
	        lpp(document).on('keydown',function(e){
		        if (e.keyCode == 27){
		            return false;
		        }
	        });
        },
        /*
	    ** @禁止Esc键默认操作,而执行自定义操作(需在DOM树加载完成后调用)
	    ** @param target：局部容器的jquery选择器字符串或dom、jquery对象
	    ** @param fns
	    **        fns = [function(e){}, function(e){}]
	    */
        setEscKeyDown: function (target, fns){
		    lpp(target).on("keydown",function(e){
		        if (e.keyCode == 27){
		            lpp.util.each(fns, function(i, fn){
		                fn(e);
		            });

		            return false;
		        }
		    });
        }
    };
}();
