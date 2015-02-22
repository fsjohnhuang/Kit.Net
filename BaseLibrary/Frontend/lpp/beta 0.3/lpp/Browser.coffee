class Browser
	isIE: ->
		navigator.appName == 'Microsoft Internet Explorer'
	isIE7: ->
		navigator.userAgent.indexOf('MSIE 7') >= 0
	isNetscape: ->
		navigator.appName == 'Netscape'
	# 浏览器可视域的高度
	clientHeight: ->
		de = document.documentElement;
		if de? then de.clientHeight else document.body.clientHeight
	# 浏览器可视域的宽度
	clientWidth: ->
		de = document.documentElement;
		if de? then de.clientWidth else document.body.clientWidth

@lpp ?= {}
lpp.Browser = new Browser()
lpp.merge?(lpp, lpp.Browser)