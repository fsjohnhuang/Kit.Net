# ��ʽ������ʱ�䣬��dateΪnull�򲻺Ϸ�����ֵ����ʹ�õ�������
@lpp.Date = 
	format: (formatStr, date) ->
		date = new Date() unless date? && lpp.isDate date
		[year, month, date, day, hours, minutes, seconds] = [
			date.getFullYear(),
			date.getMonth() + 1,
			date.getDate(),
			["������", "����һ", "���ڶ�", "������", "������", "������", "������"][date.getDay()],
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
