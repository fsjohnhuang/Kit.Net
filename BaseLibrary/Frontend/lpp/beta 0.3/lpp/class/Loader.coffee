class Loader
	_config =
		auto: true # ����create��defineʱ�Զ�ִ��ͬ������js�ļ�
		history: [] # �Ѽ��ص���ʷ��¼
		paths: # �������ļ�Ŀ¼ӳ���ϵ
			'lpp': './lpp'

	constructor: () ->

	# ��ȡ������ʷ��¼��ֻ����
	getHistory: ->
		v for v in _config.history

	# ��������Ŀ¼·��ӳ���ϵ
	setPaths: (paths) ->
		lpp.merge(_config.paths, paths)

	# ���õ���create��defineʱ�Զ�ִ��ͬ������js�ļ�
	setAuto: (isAuto) ->
		_config.auto = if lpp.isBool(isAuto) then isAuto else false
	getAuto: ->
		return _config.auto

	# �첽����js�ļ�
	require: (clsName, callback) ->
		if clsName in _config.history
			callback()
			return undefined
		
		path = parseClsName2Path(clsName)
		scripts = document.getElementsByTagName 'SCRIPT'
		had = false
		for item in scripts
			had = item.src is path
			break if had
		if not had
			loadedFn = () ->
				unless clsName in _config.history
					_config.history.push(clsName)
				callback?()

			script = document.createElement 'SCRIPT'
			if ('addEventLitener' in script)
				script.onload = loadedFn
			else if ('readyState' in script)
				script.onreadystatechange = ->
					if @readyState == 'loaded' || @readyState == 'complete'
						loadedFn()
			else
				script.onload = loadedFn
			[script.type, script.src] = ['text/javascript',path]
			pNode = if document.getElementsByTagName("HEAD").length >= 1 then document.getElementsByTagName("HEAD")[0] else document.body
			pNode.appendChild script
		undefined

	# ͬ������js�ļ�
	syncRequire: (clsName) ->
		if clsName in _config.history
			return undefined

		path = parseClsName2Path(clsName)
		[xhrCtor, arg] = if window.XMLHttpRequest? then [ window.XMLHttpRequest, null ] else [ ActiveXObject, 'Microsoft.XMLHTTP']
		xhr = new xhrCtor(arg)
		xhr.onreadystatechange = ()->
			switch xhr.readyState
				when 1
					lpp.log 'XHR is opened!'
				when 2
					lpp.log 'XHR is on preload!'
				when 3
					lpp.log 'XHR is on loading!'
				when 4
					if xhr.status is 200 && xhr.statusText.toLocaleLowerCase() is 'ok'
						eval(xhr.responseText)
		xhr.open('GET', path, false)
		xhr.send(null)
		unless clsName in _config.history
			_config.history.push(clsName)
		undefined

	### ˽�з��� ###
	# ����ȫ��ת��Ϊ�ļ�·��
	parseClsName2Path = (clsName) ->
		path = leastPath = replacedPath = ''
		for p, v of _config.paths
			if (index = clsName.indexOf(p)) >= 0
				leastPath = clsName.substring(index + p.length)
				replacedPath = v
				break
		if index == -1
			path = path.replace(///\.///g, '/') + '.js'
		else
			path = replacedPath + leastPath.replace(///\.///g, '/') + '.js'


@lpp.class ?= {}
@lpp.class.Loader = new Loader()
@lpp.Loader = @lpp.class.Loader
@lpp.require = @lpp.Loader.require
@lpp.syncRequire = @lpp.Loader.syncRequire