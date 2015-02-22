class ClsMgr
	# 私有方法：组合
	implementing = (cls, mixins) ->
		for mixin in mixins
			for key, member of mixin
				cls::[key] = member if not lpp.hasOwnProperty(cls::, key)
			for key, member of mixin::
				cls::[key] = member if not lpp.hasOwnProperty(cls::, key)
		cls

	# 定义含命名空间，继承，组合的类
	define: (fullname, cls, extend, mixins...) ->
		target = window
		ns = fullname.split '.'
		for item, i in ns
			target = target[item] or= {} if i < ns.length - 1
			clsName = item if i is ns.length - 1
		if extend?
			lpp.syncRequire extend if lpp.isStr(extend) && lpp.Loader.getAuto()
			eval("var extend = #{extend}") if lpp.isStr(extend)

			for key, value of extend
				cls[key] = value if lpp.hasOwnProperty(extend, key) && not lpp.hasOwnProperty(cls, key)
			_base = ->
				@constructor = cls
				for key, value of cls::
					@[key] = value
				@
			_base:: = extend::
			cls:: = new _base()
			cls.__super__ = extend::
		implementing cls, mixins

		target[clsName] = cls	
		undefined

	# 实例化类
	create: (fullname, args...) ->
		lpp.syncRequire fullname if lpp.Loader.getAuto()
		eval "var targetCls = #{fullname}"
		new targetCls args...


@lpp.class ?= {}
@lpp.class.ClsMgr = new ClsMgr()
@lpp.define = @lpp.class.ClsMgr.define
@lpp.create = @lpp.class.ClsMgr.create