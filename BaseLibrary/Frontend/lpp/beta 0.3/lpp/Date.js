(function() {
  this.lpp.Date = {
    format: function(formatStr, date) {
      var day, hours, minutes, month, seconds, year, _ref;

      if (!((date != null) && lpp.isDate(date))) {
        date = new Date();
      }
      _ref = [date.getFullYear(), date.getMonth() + 1, date.getDate(), ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"][date.getDay()], date.getHours(), date.getMinutes(), date.getSeconds()], year = _ref[0], month = _ref[1], date = _ref[2], day = _ref[3], hours = _ref[4], minutes = _ref[5], seconds = _ref[6];
      return formatStr.replace("{yyyy}", year).replace("{MM}", month).replace("{day}", day).replace("{dd}", date).replace("{hh}", hours).replace("{mm}", minutes).replace("{ss}", seconds);
    }
  };

}).call(this);
