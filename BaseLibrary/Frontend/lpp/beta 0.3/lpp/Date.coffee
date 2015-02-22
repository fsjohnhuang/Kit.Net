# 格式化日期时间，若date为null或不合法日期值，则使用当期日期
@lpp.Date = 
	format: (formatStr, date) ->
		date = new Date() unless date? && lpp.isDate date
		[year, month, date, day, hours, minutes, seconds] = [
			date.getFullYear(),
			date.getMonth() + 1,
			date.getDate(),
			["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"][date.getDay()],
			date.getHours(),
			date.getMinutes(),
			date.getSeconds()
		]
		formatStr.replace("{yyyy}", year)
			.replace("{MM}", month)
			.replace("{day}", day)
			.replace("{dd}", date)
			.replace("{hh}", hours)
			.replace("{mm}", minutes)
			.replace("{ss}", seconds)
