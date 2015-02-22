###
˽���ࣺlppEl
###
class lppEl
	constructor: (dom, _pDom = document, _isDown = false) ->

		if lpp.isStr dom
			if lpp.Str.trim(dom).indexOf('<') == 0
				return unless /^<(\w+)(\s+?.*?)*(\/>|>.*<\/\s*\1>)$/i.test dom

				tblEl = /^<(tr|td)\b/i.exec dom
				tmpDom = if tblEl? then tmpDom = document.createElement(tblEl[1]) else tmpDom = document.createElement('DIV')
				tmpDom.innerHTML = dom
				@dom = if tblEl? then tmpDom else lpp.toArray(tmpDom.childNodes)
			else
				if _pDom.querySelectorAll? && !_isDown
					@dom = lpp.toArray _pDom.querySelectorAll dom
				else	
					for symbol in dom.split '>'
						unless lpp.isEmpty(symbol)
							symbol = lpp.Str.trim symbol
							if lpp.isArray _pDom
								len = _pDom.length
								i = 0
								while i < len
									i += 1
									if symbol.indexOf('.') == 0
										_pDom.push(_pDom.shift().getElementsByClassName symbol.replace '.', '')
									else if symbol.indexOf('#') == 0
										_nodeList = _pDom.shift().getElementById symbol.replace '#', ''
										_pDom.push _node for _node in _nodeList
									else if symbol.indexOf('&') == 0
										_pDom.push(_pDom.shift().getElementsByClassName symbol.replace '&', '')
									else
										_nodeList = _pDom.shift().getElementsByTagName symbol 
										_pDom.push _node for _node in _nodeList
							else
								if symbol.indexOf('.') == 0
									_pDom = lpp.toArray _pDom.getElementsByClassName symbol.replace '.', ''
								else if symbol.indexOf('#') == 0
									_pDom = [_pDom.getElementById symbol.replace '#', '']
								else if symbol.indexOf('&') == 0
									_pDom = lpp.toArray _pDom.getElementsByClassName symbol.replace '&', ''
								else
									_pDom = lpp.toArray _pDom.getElementsByTagName symbol

								# ����nullԪ��
								[orginalLen, i]= [_pDom.length, 0]
								while (i < orginalLen)
									_pDomItem = _pDom.pop()
									_pDom.unshift _pDomItem if _pDomItem?
									i += 1

						_pDom = lpp.toArray _pDom if lpp.isNodeList _pDom
					@dom = _pDom
		else if lpp.isArray dom
			@dom = dom
		else
			@dom = [dom]
		
		return if not @dom?

		### Ԫ�ز������� ###
		# ��ȡ��domԪ�صĵ�һ����Ԫ��
		# return ��Ԫ�ص�lppEl����
		@firstChild = (dom) =>
			unless @dom.length == 0
				descendantLppEl = new lppEl(dom, @dom[0])
				unless lpp.isEmpty descendantLppEl.dom
					descendantLppEl = lpp(descendantLppEl.dom[0])
				descendantLppEl.back = @
				descendantLppEl
		@child = (dom) =>
			children = [];
			_doms = []
			for _dom in @dom
				children = Array.prototype.concat.apply children, lpp.toArray(_dom.childNodes)
			for _dom in children
				if dom.indexOf('.') >= 0
					dom = lpp.Str.trim(dom).substring(0,1)
					if _dom.className?.indexOf dom >= 0
						_doms.push _dom
				else if dom.indexOf('#') >= 0
					dom = lpp.Str.trim(dom).substring(0,1)
					if _dom.id?.indexOf dom >= 0
						_doms.push _dom
				else
					if _dom.tagName?.toLocaleLowerCase() is dom.toLocaleLowerCase()
						_doms.push _dom
			lpp(_doms)

		# ��ȡ����domԪ�ص���Ԫ��
		# return ��Ԫ�ص�lppEl����
		@down = (dom) =>
			resultDom = []
			for _dom in @dom
				resultDom = resultDom.concat(new lppEl(dom, _dom, true).dom)
			descendantLppEl = new lpp(resultDom)
			descendantLppEl.back = @ 
			descendantLppEl

		# ��������domԪ�ز�ִ�лص�����
		# callback({lppEl}, {number} index)
		@each = (callback) =>
			return @ unless callback?
			for dom, i in @dom
				callback new lppEl(dom), i
			@

		# ��������ֵ����lppEl����
		@find = (prop, val) =>
			goalLppEls = []
			_prop = {}
			if lpp.isObj prop
				_prop = prop
			else
				_prop[prop] = val
			for dom in @dom
				goalDom = null
				for key, value of _prop
					goalDom = if dom.getAttribute(key)?.match ///#{value}/// then dom else null
				goalLppEls.push new lppEl(goalDom) if goalDom?
			goalLppEls

		# ��������ֵ����lppEl
		@single = (prop, val) =>
			_prop = {}
			if lpp.isObj prop
				_prop = prop
			else
				_prop[prop] = val
			for dom in @dom
				goalDom = null
				for key, value of _prop
					goalDom = if dom.getAttribute(key)?.match ///#{value}/// then dom else null
				if goalDom?
					return new  lppEl(goalDom)
		
		# dynEl������dom��Ϊ@��dom����Ԫ��
		@contains = (dynEl) =>
			return false if lpp.isEmpty @dom || lpp.isEmpty dynEl

			dynEls = dynEl
			unless lpp.isArray dynEls
				(dynEls = []).push dynEl
			isDescendant = false
			for dynEl, i in dynEls
				child = if dynEl.down? then dynEl else new lppEl(dynEl)
				unless child?.dom?
					isDescendant = false
					break
				for dom in child.dom
					break unless isDescendant = @dom[0].contains dom
				break unless isDescendant

			isDescendant

		# ׷����Ԫ�ص���domԪ��
		# dynElΪdom��lppEl����
		# return ��Ԫ�ص�lppEl����
		@append = (dynEl) =>
			dynEls = dynEl
			unless lpp.isArray dynEls
				(dynEls = []).push dynEl
			children = []
			for dynEl, i in dynEls
				child = if dynEl.down? then dynEl else new lppEl(dynEl)
				children.push child
				if  @dom.length != 0 && child?.dom?.length >= 1
					plppEl = new lppEl(@dom[0])
					plppEl.dom[0].appendChild childNode for childNode in child.dom
					child.parent = plppEl
					plppEl.children ?= []
					plppEl.children.push child
			if children.length == 1
				child.back = =>
					@
				child
			else
				children.back = =>
					@
				children

		# ׷������domԪ�ص�ĳlppEl����domԪ����
		# dynElΪdom��lppEl����
		# return ��Ԫ�ص�lppEl����
		@appendAt = (dynEl) =>
			parent = if dynEl.down? then dynEl else new lppEl(dynEl)
			parent.append @ if parent?.dom?.length != 0
			parent
		
		# ������Ԫ�ص���domԪ��
		# dynElΪdom��lppEl����
		# return ��Ԫ�ص�lppEl����
		@insert = (dynEl, index) =>
			child = if dynEl.down? then dynEl else new lppEl(dynEl)
			if @dom.length != 0 && child?.dom?.length >= 1
				plppEl = new lppEl(@dom[0])
				curChildNodes = plppEl.dom[0].childNodes
				refDom = switch index
					when index < 0 then curChildNodes[0]
					when index > curChildNodes.lenght then null
					else curChildNodes[index]
				plppEl.dom[0].insertBefore insertingDom, reDom for insertingDom in child.dom
				child.parent = plppEl
				plppEl.children ?= []
				plppEl.children.push child
			child

		# ��������domԪ�ص�ĳlppEl����domԪ����
		# dynElΪdom��lppEl����
		# return ��Ԫ�ص�lppEl����
		@insertAt = (dynEl, index) =>
			parent = if dynEl.down? then dynEl else new lppEl(dynEl)
			parent.insert @, index
			parent

		# �Ƴ��ڵ�
		# return ���Ƴ���lppEl����
		@remove = (dynEl) =>
			child = if dynEl.down? then dynEl else new lppEl(dynEl)
			return child unless child?.dom.length > 0

			@dom[0].removeChild removingDom for removingDom in child.dom
			child

		# ��������domԪ�ص�style������ֵ
		# ��ȡ��domԪ�ص�style������ֵ
		@css = (prop, val) =>
			return @ unless prop?
			if lpp.isObj prop
				for dom in @dom
					dom.style[key] = value for key, value of prop
				@
			else if val?
				for dom in @dom
					if prop in ['width', 'height', 'left', 'top'] && lpp.isNum val
						dom.style[prop] = "#{val}px"
					else
						dom.style[prop] = val
						dom.style['filter'] = "alpha(opacity=#{val*100})" if prop is 'opacity'
				@
			else if @dom?.length > 0
				@dom[0].style[prop]

		# �����domԪ��className���Ƿ��и�����
		# return {boolean}
		@hasCls = (clsName) =>
			false unless @dom?.length > 0

			clsRegEx = ///(\s|^)#{clsName}(\s|$)///
			clsReEx.match @dom[0].className
		
		# ���className������domԪ��
		@addCls = (clsName) =>
			clsRegEx = ///(\s|^)#{clsName}(\s|$)///
			for curDom in @dom 
				curDom.className = "#{clsName} #{curDom.className}" unless clsRegEx.test curDom.className
			@

		# �������domԪ�ص�ָ��className
		@removeCls = (clsName) =>
			for curDom in @dom 
				curDom.className =  curDom.className.replace ///(\s|^)#{clsName}(\s|$)///, ''
			@

		# �������domԪ�ص�className
		@emptyCls = () =>
			for curDom in @dom 
				curDom.className = ''
			@

		# ���úͻ�ȡ����domԪ�ص�innerHTML
		@html = (val) =>
			if val?
				for dom in @dom
					dom.innerHTML = val
				@
			else
				(dom.innerHTML for dom in @dom).join ''

		# ���úͻ�ȡ����domԪ�ص�value
		@value = (val) =>
			if val?
				for dom in @dom
					switch dom.tagName.toLocaleLowerCase()
						when 'select'
							for optVal, i in dom.options
								if optVal == val
									dom.selectedIndex = i 
									break
						else
							dom.value = val
			else
				value = []
				for dom in @dom
					switch dom.tagName.toLocaleLowerCase()
						when 'select'
							value.push dom.options[dom.selectedIndex]
						else
							value.push dom.value
				value.join ''

		# ��������domԪ�ص�����ֵ
		# ��ȡ��domԪ�ص�����ֵ
		@attr = (prop, val) =>
			return unless prop?
			if lpp.isObj prop
				for dom in @dom
					dom.setAttribute key, value for key, value of prop
				@
			else if val?
				for dom in @dom
					dom.setAttribute prop, val
				@
			else if @dom?.length > 0
				@dom[0].getAttribute prop

		# ��������domԪ�ص�ĳ����ĳЩ�¼�
		@on = (eventName, handler, onePiece = false) =>
			return @ unless eventName?

			_eventSubscriber = {}
			if lpp.isObj(eventName)
				_eventSubscriber = eventName
			else if lpp.isStr(eventName) && handler?
				_eventSubscriber[eventName] = handler
			for _eventName, _handler of _eventSubscriber
				switch _eventName
					when 'leave','mouseleave'
						if onePiece
							_multiHanlders = do (handler = _handler, delay = 150)->
									_timer = null
									_handlers = {}
									_handlers.onmouseout = (e)->
										_handler = -> handler(e)
										_timer = setTimeout _handler, delay
									_handlers.onmousemove = (e)->
										clearTimeout _timer
									_handlers

							@on { mouseout: _multiHanlders.onmouseout, mousemove: _multiHanlders.onmousemove }
						else
							for dom in @dom
								_multiHanlders = do (handler = _handler, delay = 150)->
									_timer = null
									_handlers = {}
									_handlers.onmouseout = (e)->
										_handler = -> handler(e)
										_timer = setTimeout _handler, delay
									_handlers.onmousemove = (e)->
										clearTimeout _timer
									_handlers

								lpp(dom).on({ mouseout: _multiHanlders.onmouseout, mousemove: _multiHanlders.onmousemove })
					else
						for dom in @dom
							# ���������¼�������
							_dealedHandler = do (handler = _handler, dom = dom)->
								innerhandler = -> 
									e = event || arguments[0]
									dealedE = {};
									for propofE, valofE of e
										dealedE[propofE] = valofE

									# ��ǰdom����
									if lpp.isIE()
										dealedE.target = e.srcElement
										dealedE.relatedTarget = if e.srcElement is e.fromElement then e.toElement else e.fromElement
										dealedE.currentTarget = dom
										# ��ֹ�¼�ԭ�в���
										dealedE.preventDefault = ->
											if e.preventDefault? then e.preventDefault() else e.returnValue = false
										# ��ֹ�¼�ð��
										dealedE.stopPropagation = ->
											if e.stopPropagation? then e.stopPropagation() else e.cancelBubble = true
									# ��껬��ֵ��-1Ϊ���£�0Ϊû���˶���1Ϊ����
									dealedE.detail = do ->
										if e.detail then e.detail / 3 else e.wheelDelta / 120 


									handler(dealedE)
							@subscribed ?= {}
							@subscribed[_eventName] ?= []
							addingHandler = {}
							addingHandler[_handler] = _dealedHandler
							@subscribed[_eventName].push addingHandler 
							if dom.attachEvent?
								dom.attachEvent "on#{_eventName}", _dealedHandler
							else 
								dom.addEventListener _eventName, _dealedHandler, false
					
			@
			

		# ȡ����������domԪ�ص�ĳ����ĳЩ�¼�
		@un = (eventName, handler) =>
			return @ unless eventName?

			_eventSubscriber = {}
			if lpp.isObj eventName
				_eventSubscriber = eventName
			else if lpp.isStr(eventName) && handler?
				_eventSubscriber[eventName] = handler
			else if lpp.isStr eventName
				switch eventName
					when 'leave', 'mouseleave'
						_multiHanlders = do (handler = _handler, delay = 150)->
							_timer = null
							_handlers = {}
							_handlers.onmouseout = (e)->
								_timer = setTimeout handler, delay
							_handlers.onmousemove = (e)->
								clearTimeout _timer
							_handlers

						@un 'mouseout'
						@un 'mousemove'
					else
						# ȡ��ĳ�¼������д�����
						for dom in @dom
							_dealedHandler = null
							break unless @subscribed?[eventName]?
							for handlerMap in @subscribed[eventName]
								for _handler, _dealedHandler of handlerMap
									if dom.detachEvent?
										dom.detachEvent "on#{eventName}", _dealedHandler
									else 
										dom.removeEventListener eventName, _dealedHandler, false
			for _eventName, _handler of _eventSubscriber
				switch _eventName
					when 'leave', 'mouseleave'
						_multiHanlders = do (handler = _handler, delay = 150)->
							_timer = null
							_handlers = {}
							_handlers.onmouseout = (e)->
								_timer = setTimeout handler, delay
							_handlers.onmousemove = (e)->
								clearTimeout _timer
							_handlers

						@un { mouseout: _multiHanlders.onmouseout, mousemove: _multiHanlders.onmousemove }
					else
						for dom in @dom
							_dealedHandler = null
							break unless @subscribed?[_eventName]?

							for handlerMap in @subscribed[_eventName]
								if handlerMap?[_handler]?
									_dealedHandler = handlerMap[_handler]
									if dom.detachEvent?
										dom.detachEvent "on#{_eventName}", _dealedHandler
									else 
										dom.removeEventListener _eventName, _dealedHandler, false
					
			@

		# һ���԰�
		@one = (eventName, handler) =>
			_handler = (event)=>
				@un(eventName, _handler)
				handler(event)
			@on(eventName, _handler)

		### fxģ�� ###
		@isHidden = false

		@hide = (config) =>
			if lpp.isObj config
				[during, callback, display] = lpp.getConfigVal config, 
					during: 
						defaultVal: 1
						dataTypes: ['number']
					callback:
						defaultVal: ->
						dataTypes: ['function']
					display:
						defaultVal: 'hidden'
						dataTypes: ['string']
						valRange: ['hidden', 'invisible', 'transparent']
			else if lpp.isNum config
				during = config
				callback = ->
				display = 'hidden'
			else if lpp.isStr config
				if config in ['hidden', 'invisible', 'transparent']
					display = config
				else
					display = 'hidden'
			else if lpp.isFn config
				callback = config
				during = 1
				display = 'hidden'
			@css 'opacity', '1'
			for i in [during..0] by -1
				setTimeout(do (i, during,display, callback)=>
					=>
						@css 'opacity', i/during
						if i == 0
							switch display
								when 'hidden'
									@css 'display', 'none'
								when 'invisible'
									@css 'visiblity', 'hidden'
								when 'transparent'
									@css 'opacity', 0
							callback @
								
				, 1 + during - i)
			@

		@show = (config) =>
			if lpp.isObj config
				[during, callback] = lpp.getConfigVal config, 
					during: 
						defaultVal: 1
						dataTypes: ['number']
					callback:
						defaultVal: ->
						dataTypes: ['function']
			else if lpp.isNum config
				during = config
				callback = ->
			else if lpp.isFn config
				callback = config
				during = 1
			@css 'opacity', '0'
			for i in [0..during]
				setTimeout(do (i, during, callback)=>
					=>
						if i == 0
							@css 'display', 'block'
							@css 'visiblity', 'visible'
						@css 'opacity', i/during
						if i == during
							callback @
				, i)
			@

### lpp���� ###
@lpp = (dom) ->
	new lppEl(dom)


# �������ԭʼ����
_is = (obj, type) ->
	lpp.getType(obj) is type.toLocaleLowerCase()

# ��ȡ�������������
@lpp.getType = (obj)->
	type = typeof obj
	if type is 'object'
		type = if !obj? then 'null' else Object.prototype.toString.call(obj).slice(8, -1)
	type.toLowerCase()
# �������Ƿ�Ϊ�գ�Ϊ���ַ���������Ϊ0�����顢undefined��nullʱ����true�����򷵻�false
@lpp.isEmpty = (obj)->
	return true if !obj?
	switch type = lpp.getType obj
		when 'string'
			lpp.Str.trim(obj) == ''
		when 'array'
			obj.length == 0
		else
			false
	

# ��������װ��Ϊ����
###
@lpp.toArray = (arrayLike)->
	try
		if lpp.isIE() && lpp.isStr arrayLike
			resultArray = []
			for item, i in arrayLike
				resultArray.push arrayLike.substr(i, 1)
			resultArray
		else
			Array.prototype.slice.apply arrayLike
	catch e
		(item for item in arrayLike)


# �и�������
# ���� reduce��ɾ����ֵ���飬items: ɾ���������
@lpp.splice = (obj, startIndex, len, els...) ->
	_obj = if lpp.isArray obj then obj else lpp.toArray obj
	els.unshift len
	els.unshift startIndex
	result = {}
	result.deleted = [].splice.apply _obj,els
	result.items = _obj
	result
###
# �ж϶����Ƿ�Ϊ����
@lpp.isArray = (obj )->
	_is obj, 'array'
@lpp.isObj = (obj) ->
	_is obj, 'object'
# �ж϶����Ƿ�Ϊ�ַ���
@lpp.isStr = (obj) ->
	_is obj, 'string'
# �ж϶����Ƿ�Ϊ����ֵ
@lpp.isBool = (obj)->
	_is obj, 'boolean'
# �ж϶����Ƿ�Ϊ����
@lpp.isDate = (obj) ->
	_is obj, 'date'
# �ж϶����Ƿ�Ϊ����
@lpp.isNum = (obj) ->
	_is obj, 'number'
# �ж϶����Ƿ�Ϊ����
@lpp.isInt = (obj) ->
	lpp.isNum(obj) && (obj + '').indexOf('.') == -1
# �ж϶����Ƿ�Ϊʵ��
@lpp.isDecimal = (obj) ->
	lpp.isNum(obj) && (obj + '').indexOf('.') >= 0
# �ж϶����Ƿ�Ϊ����
@lpp.isFn = (obj) ->
	_is obj, 'function'
# �ж϶����Ƿ�Ϊ�ڵ��б�
@lpp.isNodeList = (obj) ->
	_is obj, 'nodelist'
# ����������ͣ���int��decimal��
@lpp.is = (obj, type) ->
	false unless _is type, 'string'
	lowerCaseType = type.toLocaleLowerCase()
	switch lowerCaseType
		when 'string'
			lpp.isStr obj
		when 'date'
			lpp.isDate obj
		when 'number', 'num'
			lpp.isNum obj
		when 'int', 'integer'
			lpp.isInt obj
		when 'decimal'
			lpp.isDecimal obj
		when 'bool', 'boolean'
			lpp.isBool obj
		when 'obj', 'object'
			lpp.isObj obj
		when 'array', '[]'
			lpp.isArray obj
		when 'nodelist'
			lpp.isNodeList obj
		when 'fn', 'func', 'function'
			lpp.isFn obj

### ������ ###
# ǳ�㸴�ƶ�������
# dest��src��Ϊ���󣬷��򷵻�dest
@lpp.merge = (dest, src) ->
	return dest if not lpp.isObj(dest) && not lpp.isObj(src)
	for p, v of src
		dest[p] = v

# ����Ƿ�Ϊ�������������
@lpp.hasOwnProperty = (obj, propName) ->
	return {}.hasOwnProperty.call(obj, propName) if {}.hasOwnProperty?

	isPrototypeProp = false
	if obj.constructor?
		for p, v of obj.constructor::
			isPrototypeProp = propName == p
			break if isPrototypeProp
	unless isPrototypeProp
		isOwnProp = false
		for p, v of obj
			isOwnProp = propName == p
	isOwnProp

# ��¼��־
@lpp.log = (msg) ->
	console?.log msg if window.location.hash.match /debug/
# ����
@lpp.assert = (condition, msg) ->
	console?.assert condition, msg if window.location.hash.match /debug/
# debugger
@lpp.debugger = () ->
	debugger if window.location.hash.match /debug/


# ������ȡֵ
# config{Object} ������
# propName{String/Object} ��������������,�������������Լ��
# defaultVal{Any} ���Ե�Ĭ��ֵ
# dataTypes{Array} ������������
# valRange{Array} ���Ե�ȡֵ��Χ
# success{Function} ����������ֵ�Ϸ�����ͨ���ú���������мӹ�
# return {Any/Array[Any]}
@lpp.getConfigVal = (config, propName, defaultVal = '', dataTypes, valRange, success = (val)-> val) ->
	if lpp.isStr propName
		return defaultVal unless config?[propName]?

		# ����success��������dataTypes��valRange��λ�õ����
		_success = success
		_success = dataTypes if lpp.isFn dataTypes
		_success = valRange if lpp.isFn valRange

		val = config[propName]
		valid = true
		unless lpp.isEmpty(dataTypes) || lpp.isFn(dataTypes)
			valid = false
			if lpp.isStr dataTypes
				valid = lpp.is val, dataTypes
			else if lpp.isArray dataTypes
				for dataType in dataTypes
					break if valid = lpp.is val, dataType
		unless lpp.isEmpty(valRange) || lpp.isFn(valRange)
			valid = false
			if lpp.isArray valRange
				for rangeItem in valRange
					break if valid = val is rangeItem
			else
				valid = val is valRange
		if valid then _success(val) else defaultVal 
	else if lpp.isObj propName
		val = []
		for _propName, constraints of propName
			unless config?[_propName]?
				val.push if constraints.defaultVal? then constraints.defaultVal else ''
				continue

			# ����success����Ϊ�յ����
			_success = constraints.success ? (val)-> val 

			_val = config[_propName]
			valid = true
			unless lpp.isEmpty constraints.dataTypes
				valid = false
				dataTypes = constraints.dataTypes
				if lpp.isStr dataTypes
					valid = lpp.is _val, dataTypes
				else if lpp.isArray dataTypes
					for dataType in dataTypes
						break if valid = lpp.is _val, dataType
			unless lpp.isEmpty constraints.valRange
				valid = false
				valRange = constraints.valRange
				if lpp.isArray valRange
					for rangeItem in valRange
						break if valid = _val is rangeItem
				else
					valid = _val is valRange
			val.push(if valid then _success(_val) else if constraints.defaultVal? then constraints.defaultVal else '')
		val


### ��ǿnative dom���� ###
#Ϊ��IE��������contains����
if window.find? && not HTMLElement.prototype.contains?
	HTMLElement.prototype.contains = (B) ->
		@compareDocumentPosition(B) - 19 > 0