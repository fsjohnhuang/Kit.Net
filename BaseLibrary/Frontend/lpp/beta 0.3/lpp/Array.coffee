class Array
	# 将类数组转换为数组
	toArray: (arrayLike)->
		try
			if lpp.isIE() && lpp.isStr(arrayLike)
				arrayLike.split('')
			else
				# IE执行下面的代码会抛异常
				Array.prototype.slice.apply(arrayLike)
		catch e
			(item for item in arrayLike)

	# 切割类数组
	# 返回 deleted：删除的值数组，items: 删除后的数组
	splice: (arrayLike, startIndex, len, els...) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		els.unshift(len)
		els.unshift(startIndex)
		result = {}
		result.deleted = [].splice.apply(_array,els)
		result.items = _array
		result

	# 过滤类数组或数组的无效数据，返回新数组
	# fn(item, i),返回true的值会包含在返回的数组中，否则则不包含
	# fetch为fn(item, i)返回值等于fetch时，被处理的元素则包含在返回的数组中
	grep: (arraryLike, fn, fetch = true) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		if _array.filter? && fetch == true
			_array.filter(fn)
		else
			(item for item, i in _array when fn(item, i) == fetch)

	# 对类数组或数组的每个元素执行函数
	# fn(item, i), 执行函数
	each: (arrayLike, fn) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		if _array.forEach?
			_array.forEach(fn)
		else
			fn(item, i) for item, i in _array
		undefined

	# 对类数组或数组的每个元素所处理，并将处理结果作为返回的新数组的元素
	# fn(item, i), 处理函数
	map: (arrayLike, fn) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		if _array.map?
			_array.map(fn)
		else
			(fn(item, i) for item, i in _array)

	# 对类数组或数组的每个元素所处理，所有处理结果均为true时，函数返回true
	# fn(item, i), 处理函数
	every: (arrayLike, fn) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		if _array.every?
			_array.every(fn)
		else
			for item, i in _array
				return false unless fn(item, i)
			true

	# 对类数组或数组的每个元素所处理，所有处理结果均为false时，函数返回false
	# fn(item, i), 处理函数
	some: (arrayLike, fn) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		if _array.some?
			_array.some(fn)
		else
			for item, i in _array
				return true if fn(item, i)
			false

	# 清空数组
	empty: (arrayLike) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		_array.length = 0;
		_array;

	# 获取最大值
	max: (arrayLike) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		Math.max.apply(Math, _array)
	# 获取最小值
	min: (arrayLike) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		Math.min.apply(Math, _array)
	# 随机返回数组的元素
	random: (arrayLike) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		_array[Math.floor(Math.random() * _array.length)]


@lpp ?= {}
lpp.Array = new Array()
lpp.merge?(lpp, lpp.Array)