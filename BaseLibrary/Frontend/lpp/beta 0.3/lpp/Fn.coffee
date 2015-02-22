class Fn
	# 生成拦截器函数
	# origFn 为原函数，可以使函数、函数数组、函数名(逗号隔开多个函数名)或正则表达式
	# newFn 为拦截器函数或函数数组, 这些函数的入参与origFn一致，scope为owner
	# owner 为原函数的拥有者，为null时则返回处理后带拦截器的函数；否则用处理后带拦截器的函数替换调owner中的原函数，且原函数名新增orig属性指向未经处理的原函数
	createInterceptor: (origFn, newFn, owner = null) ->
		# 处理origFn
		_origFns = []
		_owner = if owner? then owner else window
		if origFn.test? && owner?
			_origFns = (prop for prop, fn of owner when lpp.isFn(fn) && origFn.test(prop)) # 保存的是函数名称字符串数组
		else if lpp.isStr(origFn)
			origFns = origFn.split(',')
			_origFns = (fn for fn, i in origFns) # 保存的是函数名称字符串数组
		else if lpp.isFn(origFn)
			for prop, fn of _owner when lpp.isFn(fn)
				if fn.toString() is origFn.toString()
					_origFns.push(prop)			 # 保存的是函数名称字符串数组
					break
		else if lpp.isArray(origFn)
			for _fn, i in origFn				# 保存的是函数名称字符串数组
				if lpp.isFn(_fn)
					for prop, fn of _owner when lpp.isFn(fn)
						if fn.toString() is _fn.toString()
							_origFns.push(prop)			 
							break
				else if lpp.isStr(_fn)
					_origFns.push(_fn)
		else
			return null

		# 处理newFn
		if lpp.isFn(newFn)
			_newFns = [newFn]
		else if lpp.isArray(newFn)
			_newFns = newFn
		else
			return _origFns

		# 生成带拦截器的函数
		if owner?
			_innerOwner = {}
			for ofn, i in _origFns
				_innerOwner[ofn] = owner[ofn]
				owner[ofn] = do (_ofn = ofn) ->
					() ->
						for fn, i in _newFns
							return unless fn.apply(owner, arguments)
						_innerOwner[_ofn].apply(owner, arguments)
				owner[ofn].orig = () ->
					_innerOwner[ofn].apply(owner, arguments)
			undefined
		else
			interceptors = []
			for ofn, i in _origFns
				interceptor = do (_ofn = ofn) ->
					() ->
						for fn, i in _newFns
							return unless fn.apply(window, arguments)
						window[_ofn].apply(window, arguments)
				interceptors.push(interceptor)
			if interceptors.length == 1
				interceptors[0]
			else
				interceptors

	# 生成接收器管道函数
	# origFn 为原函数，可以使函数、函数数组、函数名(逗号隔开多个函数名)或正则表达式
	# newFn 为接收器管道函数或函数数组, 这些函数的入参与origFn一致，scope为owner；origFn返回的结果会经过接收器管道层层处理后返回
	# owner 为原函数的拥有者，为null时则返回处理后带接收器管道的函数；否则用处理后带接收器管道的函数替换调owner中的原函数，且原函数名新增orig属性指向未经处理的原函数
	createReceptor: (origFn, newFn, owner = null) ->
		# 处理origFn
		_origFns = []
		_owner = if owner? then owner else window
		if origFn.test? && owner?
			_origFns = (prop for prop, fn of owner when lpp.isFn(fn) && origFn.test(prop)) # 保存的是函数名称字符串数组
		else if lpp.isStr(origFn)
			origFns = origFn.split(',')
			_origFns = (fn for fn, i in origFns) # 保存的是函数名称字符串数组
		else if lpp.isFn(origFn)
			for prop, fn of _owner when lpp.isFn(fn)
				if fn.toString() is origFn.toString()
					_origFns.push(prop)			 # 保存的是函数名称字符串数组
					break
		else if lpp.isArray(origFn)
			for _fn, i in origFn				# 保存的是函数名称字符串数组
				if lpp.isFn(_fn)
					for prop, fn of _owner when lpp.isFn(fn)
						if fn.toString() is _fn.toString()
							_origFns.push(prop)			 
							break
				else if lpp.isStr(_fn)
					_origFns.push(_fn)
		else
			return null

		# 处理newFn
		if lpp.isFn(newFn)
			_newFns = [newFn]
		else if lpp.isArray(newFn)
			_newFns = newFn
		else
			return _origFns

		# 生成带接收器的函数
		if owner?
			_innerOwner = {}
			for ofn, i in _origFns
				_innerOwner[ofn] = owner[ofn]
				owner[ofn] = do (_ofn = ofn) ->
					() ->
						result = _innerOwner[_ofn].apply(owner, arguments)
						for fn, i in _newFns
							result = fn.call(owner, result)
						result
				owner[ofn].orig = () ->
					_innerOwner[ofn].apply(owner, arguments)
			undefined
		else
			receptors = []
			for ofn, i in _origFns
				receptor = do (_ofn = ofn) ->
					() ->
						result = window[_ofn].apply(window, arguments)
						for fn, i in _newFns
							result = fn.call(window, result)
						result
				receptors.push(receptor)
			if receptors.length == 1
				receptors[0]
			else
				receptors

@lpp ?= {}
lpp.Fn = new Fn()