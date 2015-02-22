!function () {
    this.lpp.string = {
        /*
	    ** @删除左右两端的空格
	    */
        trim: function(str){ 
　　	    return str.replace(/(^\s*)|(\s*$)/g, "");
        },
        /*
        ** @删除左边的空格
        */
        ltrim: function(str){ 
            return str.replace(/(^\s*)/g,"");
        },
        /*
        ** @删除右边的空格
        */
        rtrim: function(str){
            return str.replace(/(\s*$)/g,"");
        },
        /*
	    ** @根据亚拉伯数字获取中文数字
	    ** @param num:亚拉伯数字
	    **        isTradition: 是否使用繁体字
	    ** @return 中文数字
	    */
        convertToChineseNum: function (num,isTrandition){
		    var units = Array("","十","百","千","万","十","百","千","亿");
		    var digit = "" + num;

            /* 
	        ** @根据单个亚拉伯数字获取当个中文数字
	        ** @param digit:亚拉伯数字
	        **        isTradition: 是否使用繁体字
	        ** @return 单个中文数字
	        */
		    var _getChineseNum = function (digit, isTradition) {
		        var input = digit;
		        if (input == "0")
		            return (isTradition ? "零" : "零");
		        else if (input == "1")
		            return (isTradition ? "壹" : "一");
		        else if (input == "2")
		            return (isTradition ? "贰" : "二");
		        else if (input == "3")
		            return (isTradition ? "叁" : "三");
		        else if (input == "4")
		            return (isTradition ? "肆" : "四");
		        else if (input == "5")
		            return (isTradition ? "伍" : "五");
		        else if (input == "6")
		            return (isTradition ? "陆" : "六");
		        else if (input == "7")
		            return (isTradition ? "柒" : "七");
		        else if (input == "8")
		            return (isTradition ? "捌" : "八");
		        else if (input == "9")
		            return (isTradition ? "玖" : "九");
		    };
		
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
        },
        // @ 数字金额大写转换(可以处理整数,小数,负数)
        upDigit: function (n)  
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
    };
}();
