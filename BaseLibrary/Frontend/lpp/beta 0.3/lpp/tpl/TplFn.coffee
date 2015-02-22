### 内置函数(位于解释器内置作用域) ###
class TplFn
	# 传入bool、number会转换为string并返回字符串长度，传入数组或对象会返回元素个数，传入null会返回0
	len: (obj) ->
		type = lpp.getType(obj)
		switch type
			when 'boolean', 'number'
				obj.toString().length
			when 'array'
				obj.length
			when 'object'
				_len = 0
				for p of obj
					_len += 1
				_len
			else
				0

lpp.define 'lpp.tpl.TplFn', TplFn