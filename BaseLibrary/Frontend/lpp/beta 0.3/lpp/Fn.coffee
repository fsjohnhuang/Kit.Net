class Fn
	# ��������������
	# origFn Ϊԭ����������ʹ�������������顢������(���Ÿ������������)��������ʽ
	# newFn Ϊ������������������, ��Щ�����������origFnһ�£�scopeΪowner
	# owner Ϊԭ������ӵ���ߣ�Ϊnullʱ�򷵻ش������������ĺ����������ô������������ĺ����滻��owner�е�ԭ��������ԭ����������orig����ָ��δ�������ԭ����
	createInterceptor: (origFn, newFn, owner = null) ->
		# ����origFn
		_origFns = []
		_owner = if owner? then owner else window
		if origFn.test? && owner?
			_origFns = (prop for prop, fn of owner when lpp.isFn(fn) && origFn.test(prop)) # ������Ǻ��������ַ�������
		else if lpp.isStr(origFn)
			origFns = origFn.split(',')
			_origFns = (fn for fn, i in origFns) # ������Ǻ��������ַ�������
		else if lpp.isFn(origFn)
			for prop, fn of _owner when lpp.isFn(fn)
				if fn.toString() is origFn.toString()
					_origFns.push(prop)			 # ������Ǻ��������ַ�������
					break
		else if lpp.isArray(origFn)
			for _fn, i in origFn				# ������Ǻ��������ַ�������
				if lpp.isFn(_fn)
					for prop, fn of _owner when lpp.isFn(fn)
						if fn.toString() is _fn.toString()
							_origFns.push(prop)			 
							break
				else if lpp.isStr(_fn)
					_origFns.push(_fn)
		else
			return null

		# ����newFn
		if lpp.isFn(newFn)
			_newFns = [newFn]
		else if lpp.isArray(newFn)
			_newFns = newFn
		else
			return _origFns

		# ���ɴ��������ĺ���
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

	# ���ɽ������ܵ�����
	# origFn Ϊԭ����������ʹ�������������顢������(���Ÿ������������)��������ʽ
	# newFn Ϊ�������ܵ�������������, ��Щ�����������origFnһ�£�scopeΪowner��origFn���صĽ���ᾭ���������ܵ���㴦��󷵻�
	# owner Ϊԭ������ӵ���ߣ�Ϊnullʱ�򷵻ش������������ܵ��ĺ����������ô������������ܵ��ĺ����滻��owner�е�ԭ��������ԭ����������orig����ָ��δ�������ԭ����
	createReceptor: (origFn, newFn, owner = null) ->
		# ����origFn
		_origFns = []
		_owner = if owner? then owner else window
		if origFn.test? && owner?
			_origFns = (prop for prop, fn of owner when lpp.isFn(fn) && origFn.test(prop)) # ������Ǻ��������ַ�������
		else if lpp.isStr(origFn)
			origFns = origFn.split(',')
			_origFns = (fn for fn, i in origFns) # ������Ǻ��������ַ�������
		else if lpp.isFn(origFn)
			for prop, fn of _owner when lpp.isFn(fn)
				if fn.toString() is origFn.toString()
					_origFns.push(prop)			 # ������Ǻ��������ַ�������
					break
		else if lpp.isArray(origFn)
			for _fn, i in origFn				# ������Ǻ��������ַ�������
				if lpp.isFn(_fn)
					for prop, fn of _owner when lpp.isFn(fn)
						if fn.toString() is _fn.toString()
							_origFns.push(prop)			 
							break
				else if lpp.isStr(_fn)
					_origFns.push(_fn)
		else
			return null

		# ����newFn
		if lpp.isFn(newFn)
			_newFns = [newFn]
		else if lpp.isArray(newFn)
			_newFns = newFn
		else
			return _origFns

		# ���ɴ��������ĺ���
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