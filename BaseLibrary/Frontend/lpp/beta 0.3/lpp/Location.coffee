@lpp.Location = 
	# ��ȡ����·��
	getAP: (relativePath)->
		lpp("<div><a href=\"#{relativePath}\"></a></div>").child('a').dom[0].href
	# ��ȡ��ǰĿ¼�ľ���·��
	getCurDirAP: ->
		getAP './'
	# ��ȡָ��������queryParam
	getQueryParam: (key) ->
		reg = ///(^|&)#{key}=([^&]*)(&|$)///i
		r = location.search.substr(1).match(reg);
		if r? then unescape(r[2]) else null
