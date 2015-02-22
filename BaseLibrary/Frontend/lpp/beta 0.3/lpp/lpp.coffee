###
私有类：lppEl
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

								# 过滤null元素
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

		### 元素操作函数 ###
		# 获取首dom元素的第一个子元素
		# return 子元素的lppEl对象
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

		# 获取所有dom元素的子元素
		# return 子元素的lppEl对象
		@down = (dom) =>
			resultDom = []
			for _dom in @dom
				resultDom = resultDom.concat(new lppEl(dom, _dom, true).dom)
			descendantLppEl = new lpp(resultDom)
			descendantLppEl.back = @ 
			descendantLppEl

		# 遍历所有dom元素并执行回调函数
		# callback({lppEl}, {number} index)
		@each = (callback) =>
			return @ unless callback?
			for dom, i in @dom
				callback new lppEl(dom), i
			@

		# 根据属性值返回lppEl数组
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

		# 根据属性值返回lppEl
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
		
		# dynEl的所有dom均为@首dom的子元素
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

		# 追加子元素到首dom元素
		# dynEl为dom或lppEl对象
		# return 子元素的lppEl对象
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

		# 追加所有dom元素到某lppEl的首dom元素中
		# dynEl为dom或lppEl对象
		# return 父元素的lppEl对象
		@appendAt = (dynEl) =>
			parent = if dynEl.down? then dynEl else new lppEl(dynEl)
			parent.append @ if parent?.dom?.length != 0
			parent
		
		# 插入子元素到首dom元素
		# dynEl为dom或lppEl对象
		# return 子元素的lppEl对象
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

		# 插入所有dom元素到某lppEl的首dom元素中
		# dynEl为dom或lppEl对象
		# return 父元素的lppEl对象
		@insertAt = (dynEl, index) =>
			parent = if dynEl.down? then dynEl else new lppEl(dynEl)
			parent.insert @, index
			parent

		# 移除节点
		# return 被移除的lppEl对象
		@remove = (dynEl) =>
			child = if dynEl.down? then dynEl else new lppEl(dynEl)
			return child unless child?.dom.length > 0

			@dom[0].removeChild removingDom for removingDom in child.dom
			child

		# 设置所有dom元素的style的属性值
		# 获取首dom元素的style的属性值
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

		# 检查首dom元素className中是否有该类名
		# return {boolean}
		@hasCls = (clsName) =>
			false unless @dom?.length > 0

			clsRegEx = ///(\s|^)#{clsName}(\s|$)///
			clsReEx.match @dom[0].className
		
		# 添加className到所有dom元素
		@addCls = (clsName) =>
			clsRegEx = ///(\s|^)#{clsName}(\s|$)///
			for curDom in @dom 
				curDom.className = "#{clsName} #{curDom.className}" unless clsRegEx.test curDom.className
			@

		# 清除所有dom元素的指定className
		@removeCls = (clsName) =>
			for curDom in @dom 
				curDom.className =  curDom.className.replace ///(\s|^)#{clsName}(\s|$)///, ''
			@

		# 清除所有dom元素的className
		@emptyCls = () =>
			for curDom in @dom 
				curDom.className = ''
			@

		# 设置和获取所有dom元素的innerHTML
		@html = (val) =>
			if val?
				for dom in @dom
					dom.innerHTML = val
				@
			else
				(dom.innerHTML for dom in @dom).join ''

		# 设置和获取所有dom元素的value
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

		# 设置所有dom元素的属性值
		# 获取首dom元素的属性值
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

		# 订阅所有dom元素的某个或某些事件
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
							# 经处理后的事件处理函数
							_dealedHandler = do (handler = _handler, dom = dom)->
								innerhandler = -> 
									e = event || arguments[0]
									dealedE = {};
									for propofE, valofE of e
										dealedE[propofE] = valofE

									# 当前dom对象
									if lpp.isIE()
										dealedE.target = e.srcElement
										dealedE.relatedTarget = if e.srcElement is e.fromElement then e.toElement else e.fromElement
										dealedE.currentTarget = dom
										# 禁止事件原有操作
										dealedE.preventDefault = ->
											if e.preventDefault? then e.preventDefault() else e.returnValue = false
										# 禁止事件冒泡
										dealedE.stopPropagation = ->
											if e.stopPropagation? then e.stopPropagation() else e.cancelBubble = true
									# 鼠标滑轮值，-1为向下，0为没有运动，1为向上
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
			

		# 取消订阅所有dom元素的某个或某些事件
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
						# 取消某事件的所有处理方法
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

		# 一次性绑定
		@one = (eventName, handler) =>
			_handler = (event)=>
				@un(eventName, _handler)
				handler(event)
			@on(eventName, _handler)

		### fx模块 ###
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

### lpp函数 ###
@lpp = (dom) ->
	new lppEl(dom)


# 检查数据原始类型
_is = (obj, type) ->
	lpp.getType(obj) is type.toLocaleLowerCase()

# 获取对象的数据类型
@lpp.getType = (obj)->
	type = typeof obj
	if type is 'object'
		type = if !obj? then 'null' else Object.prototype.toString.call(obj).slice(8, -1)
	type.toLowerCase()
# 检查对象是否为空，为空字符串、长度为0的数组、undefined或null时返回true，否则返回false
@lpp.isEmpty = (obj)->
	return true if !obj?
	switch type = lpp.getType obj
		when 'string'
			lpp.Str.trim(obj) == ''
		when 'array'
			obj.length == 0
		else
			false
	

# 将类数组装换为数组
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


# 切割类数组
# 返回 reduce：删除的值数组，items: 删除后的数组
@lpp.splice = (obj, startIndex, len, els...) ->
	_obj = if lpp.isArray obj then obj else lpp.toArray obj
	els.unshift len
	els.unshift startIndex
	result = {}
	result.deleted = [].splice.apply _obj,els
	result.items = _obj
	result
###
# 判断对象是否为数组
@lpp.isArray = (obj )->
	_is obj, 'array'
@lpp.isObj = (obj) ->
	_is obj, 'object'
# 判断对象是否为字符串
@lpp.isStr = (obj) ->
	_is obj, 'string'
# 判断对象是否为布尔值
@lpp.isBool = (obj)->
	_is obj, 'boolean'
# 判断对象是否为日期
@lpp.isDate = (obj) ->
	_is obj, 'date'
# 判断对象是否为数字
@lpp.isNum = (obj) ->
	_is obj, 'number'
# 判断对象是否为整形
@lpp.isInt = (obj) ->
	lpp.isNum(obj) && (obj + '').indexOf('.') == -1
# 判断对象是否为实数
@lpp.isDecimal = (obj) ->
	lpp.isNum(obj) && (obj + '').indexOf('.') >= 0
# 判断对象是否为行数
@lpp.isFn = (obj) ->
	_is obj, 'function'
# 判断对象是否为节点列表
@lpp.isNodeList = (obj) ->
	_is obj, 'nodelist'
# 检查数据类型（含int和decimal）
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

### 工具类 ###
# 浅层复制对象属性
# dest和src均为对象，否则返回dest
@lpp.merge = (dest, src) ->
	return dest if not lpp.isObj(dest) && not lpp.isObj(src)
	for p, v of src
		dest[p] = v

# 检查是否为对象的自有属性
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

# 记录日志
@lpp.log = (msg) ->
	console?.log msg if window.location.hash.match /debug/
# 断言
@lpp.assert = (condition, msg) ->
	console?.assert condition, msg if window.location.hash.match /debug/
# debugger
@lpp.debugger = () ->
	debugger if window.location.hash.match /debug/


# 配置项取值
# config{Object} 配置项
# propName{String/Object} 操作的属性名称,多个属性名及其约束
# defaultVal{Any} 属性的默认值
# dataTypes{Array} 属性数据类型
# valRange{Array} 属性的取值范围
# success{Function} 若参数属性值合法，则通过该函数对其进行加工
# return {Any/Array[Any]}
@lpp.getConfigVal = (config, propName, defaultVal = '', dataTypes, valRange, success = (val)-> val) ->
	if lpp.isStr propName
		return defaultVal unless config?[propName]?

		# 处理将success函数移至dataTypes或valRange的位置的情况
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

			# 处理success函数为空的情况
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


### 增强native dom功能 ###
#为非IE浏览器添加contains方法
if window.find? && not HTMLElement.prototype.contains?
	HTMLElement.prototype.contains = (B) ->
		@compareDocumentPosition(B) - 19 > 0