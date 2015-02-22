!function () {
    this.lpp.validity = {
        /*
	    ** @检查是否为有效的固话号码
	    */
        isValidPhone: function(phoneNum){
		    var isValid = false;
            isValid = /^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$/.test(phoneNum);

            return isValid;
        },

        /*
        ** @检查是否为有效的“移动”手机号码
        */
        isValidMobileBrandPhone: function (phoneNum){
            var isValid = false;
            isValid = /^1(34|35|36|37|38|39|50|51|52|58|59|47|82|87|88)[0-9]{8,8}$/.test(phoneNum);

            return isValid;
        },

        /*
        ** @检查是否为有效的“联通”手机号码
        */
        isValidUnicomBrandPhone: function(phoneNum){
            var isValid = false;
            isValid = /^1(32|30|31|32|55|56|85|86)[0-9]{8,8}$/.test(phoneNum);

            return isValid;
        },

        /*
        ** @检查是否为有效的“电信”手机号码
        */
        isValidTelecomBrandPhone: function (phoneNum){
            var isValid = false;
            isValid = /^133[0-9]{8,8}$/.test(phoneNum);

            return isValid;
        },

        /*
        ** @检查是否为有效的身份证号码
        */
        isValidID: function(id){
            var isValid = false;
            isValid = /^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$/.test(id);

            return isValid;
        },
        /*
        ** @检查是否为有效的电子邮箱地址
        */
        isValidEMail: function (email){
            var isValid = false;
            isValid = /^w+([-+.]w+)*@w+([-.]w+)*.w+([-.]w+)*$/.test(email);

            return isValid;
        },
        /*
        ** @检查是否为有效的中国邮政编码
        */
        isValidPostcode: function (postcode){
            var isValid = false;
            isValid = /^[1-9]{1}(\d+){5}$/.test(postcode);

            return isValid;
        },
        /*
        ** @检查是否为全中文字符串
        */
        isAllChinese: function (str){
            var isValid = false;
            isValid = /^[\u4e00-\u9fa5]*$/.test(str);

            return isValid;
        },
        /*
        ** @检查是否为有效的url
        */
        isValidUrl: function (url){
            var isValid = false;
            isValid = /^[a-zA-z]+:\/\/[^\s]*$/.test(url);

            return isValid;
        },
        /*
        ** @检查是否为有效的用户账号
        */
        isValidAccount: function (account){
            var isValid = false;
            isValid = /^[a-zA-Z][a-zA-Z0-9_]{4,15}$/.test(account);

            return isValid;
        },
        /*
        ** @检查是否为整数
        */
        isInt: function (str){
            var isValid = false;
            isValid = /^(-|\+)?\d+$/.test(str);

            return isValid;
        },
        /*
        ** @检查是否为数字（可含小数）
        */
        isNumeric: function (str){
            var isValid = false;
            isValid = !isNaN(str);

            return isValid;
        },
        /*
        ** @检查是否为有效的QQ账号
        */
        isValidQQ: function (qq){
            var isValid = false;
            isValid = /^[1-9][0-9]{4,}$/.test(qq);

            return isValid;
        }
    };
}();
