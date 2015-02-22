!function () {
    this.lpp.location = {
        getQueryParam: function (key) {
            var reg = new RegExp("(^|&)" + key + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null)
                return unescape(r[2]);
            return null;
        },
        /*
	    ** @组装url
	    ** @param url: url地址
	    ** @param queryParams: queryString参数对象
	    */
        combineUrl: function (url, queryParams){
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
        },
        /*
	    ** @禁止通过IFrame成为内页
	    */
        forbidAsIFrame: function () {
		    if (top !== window){
		        top.location.href = window.location.href;
		    }
        }
    };
}();