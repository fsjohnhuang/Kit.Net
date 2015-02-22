!function () {
    this.lpp.date = {
        getNow: function (dateSeparator, datetimeSeparator ,timeSeparator) {
            var _dateSeparator = dateSeparator || '/',
                _datetimeSeparator = datetimeSeparator || ' ',
                _timeSeparator = timeSeparator || ':';
		    var date = new Date();
		    var now = date.getFullYear() + _dateSeparator;
		    now += (date.getMonth() + 1) + _dateSeparator;
		    now += date.getDate() + _dateSeparator;
            now += date.getHours() + _timeSeparator;
            now += date.getMinutes() + _timeSeparator;
            now += date.getSeconds();
		
            return now;
        },
        getDate: function (pattern) {
            var now = new Date();
            var year = now.getFullYear(),
                month = now.getMonth() + 1,
                date = now.getDate(),
                day = ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"][now.getDay()],
                hours = now.getHours(),
                minutes = now.getMinutes(),
                seconds = now.getSeconds();
            var result = pattern.replace("{yyyy}", year)
                .replace("{mm}", month)
                .replace("{day}", day)
                .replace("{dd}", date)
                .replace("{hh}", hours)
                .replace("{MM}", minutes)
                .replace("{ss}", seconds);
            
            return result;
        }
    };
}();
