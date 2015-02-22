class Ajax
	[xhrCtor, arg] = if window.XMLHttpRequest? then [ window.XMLHttpRequest, null ] else [ ActiveXObject, 'Microsoft.XMLHTTP']

	xhrQueue = do ->
		_xhrQueue = (new xhrCtor(arg) for i in [0..3])
	
	_getXHR = ->
		if (xhrQueue.length >= 1)
			xhrQueue.pop()
		else
			new xhrCtor(arg)
	
	# 发起异步请求
	req: (url, callback, state, method, postData) ->
		return if lpp.isEmpty url
		method ?= 'GET'

		xhr = _getXHR()
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
						callback xhr, state
					xhr.abort()
					xhrQueue.push xhr
				else
					xhr.abort()
					xhrQueue.push xhr
		xhr.open method, url, true
		unless method.toLocaleLowerCase() is 'get'
			xhr.setRequestHeader 'Content-type', 'application/x-www-form-urlencoded'
			xhr.setRequestHeader 'Content-length', postData?.length
			xhr.setRequestHeader 'Connection', 'close'
		xhr.send postData ?= null

	# 发起同步请求
	syncReq: (url, callback, state, method, postData) ->
		return if lpp.isEmpty url
		method ?= 'GET'

		xhr = _getXHR()
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
						callback xhr, state
					xhrQueue.push xhr
				else
					xhrQueue.push xhr
		xhr.open method, url, false
		unless method.toLocaleLowerCase() is 'get'
			xhr.setRequestHeader 'Content-type', 'application/x-www-form-urlencoded'
			xhr.setRequestHeader 'Content-length', postData?.length
			xhr.setRequestHeader 'Connection', 'close'
		xhr.send postData ?= null


@lpp.Ajax = new Ajax()