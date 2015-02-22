### �﷨��Ԫ��
type: ����
	0: ��ͨ�ı� StrEl
	1: ���� VarEl
	2: ���� FnEl
	3: ������� IfEl
	4: ѭ����� LoopEl
	5��ָ��, break��continue CmdEl
	6: ���� @{ ...... } BlockEl
	7: ���� @[1..3] �� @[1...3] SectionEl
condition: ������������ѭ�����ʹ�õ�ִ������
scope: ������
###
class SyntaxEl
	constructor: (@content, @type, @condition, @scope, @childEl) ->


class GP
	
	_keyword = ['if', 'elseif', 'elif', 'else', 'unless', 'while', 'for', 'true', 'false', 'null', 'break', 'continue'] # �ؼ���
	_relSymbol = ['&&', '||'] # ��ϵ�����
	_logicSymbol = ['>', '>=', '<', '<=', '==', '!='] # �߼������
	_mathSymbol = ['+', '-', '*', '/', '%', '**'] # ��ѧ�����
	_gpSymbol = ['<-'] # ��ֵ������

	_symbolStart = ///[_a-Z[{]'///
	_symbolEnd = ///\}\]///

	# ��ʶ����������
	_symbolExp = '[_a-Z][_0-9a-Z]'

	_tpl = ''
	_prefix = '@'

	# tpl ģ���ı�
	constructor: (tpl, prefix) ->
		_tpl = tpl
		_prefix = prefix if prefix?


	_root = lpp.create('lpp.structure.TreeNode') # �﷨����ڵ�
	_spider = # �ı�֩��
		i: 0 # ��ǰ����
		g: null # ��ǰ�ַ�
		ng: null # ��һ���ַ�
		el: null # ��ǰ�﷨Ԫ��
		pEl: _root # ��ǰ���﷨Ԫ��
		'{': 0
		'[': 0
		parse: () ->
			

	# ��ģ���ı������﷨�����������﷨��
	_compile = () ->
		grains = lpp.toArray(_tpl)
		grainsLen = grains.length

		for i, grain in grains
			_spider.i = i
			_spider.g = grain
			_spider.ng = if i < grainsLen - 1 then grains[i + 1] else null




lpp.define 'lpp.tpl', GP