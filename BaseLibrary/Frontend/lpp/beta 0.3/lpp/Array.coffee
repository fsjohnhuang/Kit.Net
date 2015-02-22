class Array
	# ��������ת��Ϊ����
	toArray: (arrayLike)->
		try
			if lpp.isIE() && lpp.isStr(arrayLike)
				arrayLike.split('')
			else
				# IEִ������Ĵ�������쳣
				Array.prototype.slice.apply(arrayLike)
		catch e
			(item for item in arrayLike)

	# �и�������
	# ���� deleted��ɾ����ֵ���飬items: ɾ���������
	splice: (arrayLike, startIndex, len, els...) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		els.unshift(len)
		els.unshift(startIndex)
		result = {}
		result.deleted = [].splice.apply(_array,els)
		result.items = _array
		result

	# ������������������Ч���ݣ�����������
	# fn(item, i),����true��ֵ������ڷ��ص������У������򲻰���
	# fetchΪfn(item, i)����ֵ����fetchʱ���������Ԫ��������ڷ��ص�������
	grep: (arraryLike, fn, fetch = true) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		if _array.filter? && fetch == true
			_array.filter(fn)
		else
			(item for item, i in _array when fn(item, i) == fetch)

	# ��������������ÿ��Ԫ��ִ�к���
	# fn(item, i), ִ�к���
	each: (arrayLike, fn) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		if _array.forEach?
			_array.forEach(fn)
		else
			fn(item, i) for item, i in _array
		undefined

	# ��������������ÿ��Ԫ��������������������Ϊ���ص��������Ԫ��
	# fn(item, i), ������
	map: (arrayLike, fn) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		if _array.map?
			_array.map(fn)
		else
			(fn(item, i) for item, i in _array)

	# ��������������ÿ��Ԫ�����������д�������Ϊtrueʱ����������true
	# fn(item, i), ������
	every: (arrayLike, fn) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		if _array.every?
			_array.every(fn)
		else
			for item, i in _array
				return false unless fn(item, i)
			true

	# ��������������ÿ��Ԫ�����������д�������Ϊfalseʱ����������false
	# fn(item, i), ������
	some: (arrayLike, fn) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		if _array.some?
			_array.some(fn)
		else
			for item, i in _array
				return true if fn(item, i)
			false

	# �������
	empty: (arrayLike) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		_array.length = 0;
		_array;

	# ��ȡ���ֵ
	max: (arrayLike) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		Math.max.apply(Math, _array)
	# ��ȡ��Сֵ
	min: (arrayLike) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		Math.min.apply(Math, _array)
	# ������������Ԫ��
	random: (arrayLike) ->
		_array = if lpp.isArray arrayLike then arrayLike else @toArray(arrayLike)
		_array[Math.floor(Math.random() * _array.length)]


@lpp ?= {}
lpp.Array = new Array()
lpp.merge?(lpp, lpp.Array)