@lpp.Location = 
	# 获取绝对路径
	getAP: (relativePath)->
		lpp("<div><a href=\"#{relativePath}\"></a></div>").child('a').dom[0].href
	# 获取当前目录的绝对路径
	getCurDirAP: ->
		getAP './'
	# 获取指定键名的queryParam
	getQueryParam: (key) ->
		reg = ///(^|&)#{key}=([^&]*)(&|$)///i
		r = location.search.substr(1).match(reg);
		if r? then unescape(r[2]) else null
