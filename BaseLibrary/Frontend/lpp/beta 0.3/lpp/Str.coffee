###
字符串处理模块
###
@lpp.Str = 
	# 计算字符串宽度
	# @param text{String} 字符串
	# @param lrSpace{Numeric} 左右两边空位宽度
	# @return {Numeric} 长度（单位px）
	calcWidth: (str, lrSpace = 0) ->
		width = 0
		strArray = lpp.toArray str
		for character in strArray
			if /^[\u4e00-\u9fa5]*$/.test character
				width += 14
			else
				width += 8
		width + lrSpace * 2
	# 删除左右两端的空格
	trim: (str)->
		str.replace /(^\s*)|(\s*$)/g, ''
	# 删除左边的空格
	ltrim: (str) ->
		str.replace /(^\s*)/g, ''
	# 删除右边的空格
	rtrim: (str) ->
		str.replace /(\s*$)/g, ''
	# 切割字符串(字符串为类数组，原生没有splice方法)
	splice: (str, startIndex, len, els...) ->
		els.unshift len
		els.unshift startIndex
		els.unshift str
		result = lpp.splice.apply(@, els)
		strResult = {}
		strResult.str = result.items.join ''
		strResult.deleted = result.deleted.join ''
		strResult
	# 格式化字符串{0}{1}来作内容占位符
	format: (str, value...) ->
		for val, i in value
			str = str.replace ///\{#{i}\}///g, val
		str
	# (简单)模板引擎
	tpl: (str, json, prefix = '@') ->
		for prop, val of json
			str = str.replace ///#{prefix}#{prop}///g, val
		str
