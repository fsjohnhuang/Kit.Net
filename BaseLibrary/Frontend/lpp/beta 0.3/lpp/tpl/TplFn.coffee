### ���ú���(λ�ڽ���������������) ###
class TplFn
	# ����bool��number��ת��Ϊstring�������ַ������ȣ�������������᷵��Ԫ�ظ���������null�᷵��0
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