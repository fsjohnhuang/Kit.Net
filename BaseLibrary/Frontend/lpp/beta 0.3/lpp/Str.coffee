###
�ַ�������ģ��
###
@lpp.Str = 
	# �����ַ������
	# @param text{String} �ַ���
	# @param lrSpace{Numeric} �������߿�λ���
	# @return {Numeric} ���ȣ���λpx��
	calcWidth: (str, lrSpace = 0) ->
		width = 0
		strArray = lpp.toArray str
		for character in strArray
			if /^[\u4e00-\u9fa5]*$/.test character
				width += 14
			else
				width += 8
		width + lrSpace * 2
	# ɾ���������˵Ŀո�
	trim: (str)->
		str.replace /(^\s*)|(\s*$)/g, ''
	# ɾ����ߵĿո�
	ltrim: (str) ->
		str.replace /(^\s*)/g, ''
	# ɾ���ұߵĿո�
	rtrim: (str) ->
		str.replace /(\s*$)/g, ''
	# �и��ַ���(�ַ���Ϊ�����飬ԭ��û��splice����)
	splice: (str, startIndex, len, els...) ->
		els.unshift len
		els.unshift startIndex
		els.unshift str
		result = lpp.splice.apply(@, els)
		strResult = {}
		strResult.str = result.items.join ''
		strResult.deleted = result.deleted.join ''
		strResult
	# ��ʽ���ַ���{0}{1}��������ռλ��
	format: (str, value...) ->
		for val, i in value
			str = str.replace ///\{#{i}\}///g, val
		str
	# (��)ģ������
	tpl: (str, json, prefix = '@') ->
		for prop, val of json
			str = str.replace ///#{prefix}#{prop}///g, val
		str
